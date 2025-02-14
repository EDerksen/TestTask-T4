using Microsoft.EntityFrameworkCore;
using TestTask_T4.Contracts;
using TestTask_T4.Data;
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

        public FinanceService(ILogger<FinanceService> logger,
            FinanceDbContext dbContext,
            IClientsService clientsService,
            ITransactionValidator transactionValidator)
        {
            _logger = logger;
            _dbContext = dbContext;
            _clientsService = clientsService;
            _transactionValidator = transactionValidator;
        }

        public async Task<TransactionResult> ProcessTransaction(ITransaction transaction, CancellationToken cancellationToken = default)
        {
            var client = await _clientsService.GetClientForUpdate(transaction.ClientId);

            _transactionValidator.ValidateClientTransaction(transaction, client);

            return await ApplyTransaction(transaction, client, cancellationToken);
        }

        private async Task<TransactionResult> ApplyTransaction(ITransaction transaction, Client client, CancellationToken cancellationToken = default)
        {
            var financialTransaction = transaction switch
            {
                DebitTransaction debitTransaction => ApplyDebitTransaction(debitTransaction, client),
                CreditTransaction creditTransaction => ApplyCreditTransaction(creditTransaction, client),
                _ => throw new InvalidOperationException("Unknown transaction type")
            };

            _dbContext.Add(financialTransaction);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new TransactionResult()
            {
                ClientBalance = client.Balance,
                DateTime = financialTransaction.CreatedAt
            };
        }

        private FinancialTransaction ApplyDebitTransaction(DebitTransaction debitTransaction, Client client)
        {
            var newBalance = client.Balance - debitTransaction.Amount;
            var financialTransaction = new FinancialTransaction
            {
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
            return financialTransaction;
        }

        private FinancialTransaction ApplyCreditTransaction(CreditTransaction creditTransaction, Client client)
        {
            var newBalance = client.Balance + creditTransaction.Amount;
            var financialTransaction = new FinancialTransaction
            {
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
            return financialTransaction;
        }

        private async Task<decimal> GetClientBalance(Guid clientId, CancellationToken cancellationToken = default)
        {
            var lastTransaction = await _dbContext.Transactions
                .Where(t => t.ClientId == clientId)
                .OrderByDescending(t => t.DateTime)
                .FirstOrDefaultAsync(cancellationToken);

            return lastTransaction?.BalanceSnapshot?.Balance ?? 0;
        }
    }
}