using Microsoft.EntityFrameworkCore;
using System.Threading;
using TestTask_T4.Contracts;
using TestTask_T4.Data;
using TestTask_T4.Exceptions;
using TestTask_T4.Model;
using TestTask_T4.Services.Clients;

namespace TestTask_T4.Services.Finance
{
    public class FinanceService : IFinanceService
    {
        private readonly ILogger<FinanceService> _logger;
        private readonly FinanceDbContext _dbContext;
        private readonly IClientsService _clientsService;
        private readonly ITransactionValidator _transactionValidator;
        private readonly TimeProvider _timeProvider;

        public FinanceService(ILogger<FinanceService> logger,
            FinanceDbContext dbContext,
            IClientsService clientsService,
            ITransactionValidator transactionValidator,
            TimeProvider timeProvider)
        {
            _logger = logger;
            _dbContext = dbContext;
            _clientsService = clientsService;
            _transactionValidator = transactionValidator;
            _timeProvider = timeProvider;
        }

        public async Task<TransactionResult> ProcessTransaction(ITransaction transaction, CancellationToken cancellationToken = default)
        {
            var existingTransaction = await FindTransaction(transaction.Id, cancellationToken);
            if (existingTransaction != null)
            {
                _transactionValidator.ValidateDuplicatingTransaction(transaction, existingTransaction);
                return new TransactionResult()
                {
                    ClientBalance = existingTransaction.BalanceSnapshot.Balance,
                    InsertDateTime = existingTransaction.CreatedAt
                };
            }

            await using var dbTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            var result = await ProcessClientTransaction(transaction, cancellationToken);

            await dbTransaction.CommitAsync();

            return result;
        }

        public async Task<RevertTransactionResult> RevertTransaction(Guid transactionId, CancellationToken cancellationToken = default)
        {
            await using var dbTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            var transaction = await SelectTransactionForUpdate(transactionId, cancellationToken);
            if (transaction == null)
            {
                throw new NotFoundException("Transaction not found", $"Can't found Transaction with id={transactionId}");
            }
            else if (transaction.RevertTransactionId.HasValue)
            {
                var revertTransaction = await FindTransaction(transaction.RevertTransactionId.Value, cancellationToken);
                return new RevertTransactionResult()
                {
                    ClientBalance = revertTransaction!.BalanceSnapshot.Balance,
                    RevertDateTime = revertTransaction.CreatedAt
                };
            }

            var compensatory = GetCompensatoryTransaction(transaction);

            var result = await ProcessClientTransaction(compensatory, cancellationToken);
            transaction.RevertTransactionId = compensatory.Id;

            await _dbContext.SaveChangesAsync(cancellationToken);
            await dbTransaction.CommitAsync();

            return new RevertTransactionResult()
            {
                ClientBalance = result.ClientBalance,
                RevertDateTime = result.InsertDateTime
            };
        }

        private async Task<TransactionResult> ProcessClientTransaction(ITransaction transaction, CancellationToken cancellationToken = default)
        {
            var client = await _clientsService.GetClientForUpdate(transaction.ClientId);

            _transactionValidator.ValidateClientTransaction(transaction, client);
            return await ApplyTransaction(transaction, client, cancellationToken);
        }

        private ITransaction GetCompensatoryTransaction(FinancialTransaction financialTransaction)
        {
            return financialTransaction.Type switch
            {
                FinancialTransactionType.Debit => GetCreditTransaction(financialTransaction),
                FinancialTransactionType.Credit => GetDebitTransaction(financialTransaction),
                _ => throw new FinancialException("Unknown transaction type", $"Unknown transaction type:{financialTransaction.Type}")
            };
        }

        private ITransaction GetCreditTransaction(FinancialTransaction financialTransaction, CancellationToken cancellationToken = default)
        {
            return new CreditTransaction
            {
                Id = Guid.NewGuid(),
                ClientId = financialTransaction.ClientId,
                DateTime = _timeProvider.GetUtcNow().UtcDateTime,
                Amount = financialTransaction.Amount
            };
        }

        private ITransaction GetDebitTransaction(FinancialTransaction financialTransaction, CancellationToken cancellationToken = default)
        {
            return new DebitTransaction
            {
                Id = Guid.NewGuid(),
                ClientId = financialTransaction.ClientId,
                DateTime = _timeProvider.GetUtcNow().UtcDateTime,
                Amount = financialTransaction.Amount
            };
        }

        private Task<FinancialTransaction?> FindTransaction(Guid transactionId, CancellationToken cancellationToken = default)
        {
            return _dbContext.Transactions
                .Include(t => t.BalanceSnapshot)
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.Id == transactionId, cancellationToken);
        }

        private Task<FinancialTransaction?> SelectTransactionForUpdate(Guid transactionId, CancellationToken cancellationToken = default)
        {
            return _dbContext.Transactions
                .FromSqlRaw($"SELECT * FROM \"{nameof(FinanceDbContext.Transactions)}\" WHERE \"{nameof(FinancialTransaction.Id)}\" = @p0 FOR UPDATE", transactionId)
                .SingleOrDefaultAsync(cancellationToken);
        }

        private async Task<TransactionResult> ApplyTransaction(ITransaction transaction, Client client, CancellationToken cancellationToken = default)
        {
            var financialTransaction = transaction switch
            {
                DebitTransaction debitTransaction => ApplyDebitTransaction(debitTransaction, client),
                CreditTransaction creditTransaction => ApplyCreditTransaction(creditTransaction, client),
                _ => throw new FinancialException("Unknown transaction type", $"Unknown transaction type:{transaction.GetType().Name}")
            };

            await _dbContext.SaveChangesAsync(cancellationToken);
            return new TransactionResult()
            {
                ClientBalance = client.Balance,
                InsertDateTime = financialTransaction.CreatedAt
            };
        }

        private FinancialTransaction ApplyDebitTransaction(DebitTransaction debitTransaction, Client client)
        {
            var newBalance = client.Balance - debitTransaction.Amount;
            var financialTransaction = new FinancialTransaction
            {
                Id = debitTransaction.Id,
                Client = client,
                DateTime = debitTransaction.DateTime,
                Amount = debitTransaction.Amount,
                Type = FinancialTransactionType.Debit,
                BalanceSnapshot = new ClientBalanceSnapshot
                {
                    Balance = newBalance
                }
            };

            client.Balance = newBalance;
            _dbContext.Add(financialTransaction);

            return financialTransaction;
        }

        private FinancialTransaction ApplyCreditTransaction(CreditTransaction creditTransaction, Client client)
        {
            var newBalance = client.Balance + creditTransaction.Amount;
            var financialTransaction = new FinancialTransaction
            {
                Id = creditTransaction.Id,
                Client = client,
                DateTime = creditTransaction.DateTime,
                Amount = creditTransaction.Amount,
                Type = FinancialTransactionType.Credit,
                BalanceSnapshot = new ClientBalanceSnapshot
                {
                    Balance = newBalance
                }
            };

            client.Balance = newBalance;
            _dbContext.Add(financialTransaction);

            return financialTransaction;
        }
    }
}