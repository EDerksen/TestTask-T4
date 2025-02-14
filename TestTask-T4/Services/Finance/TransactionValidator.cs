using TestTask_T4.Contracts;
using TestTask_T4.Exceptions;
using TestTask_T4.Model;

namespace TestTask_T4.Services.Finance
{
    public class TransactionValidator : ITransactionValidator
    {
        private readonly TimeProvider _timeProvider;

        public TransactionValidator(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public void ValidateClientTransaction(ITransaction transaction, Client client)
        {
            if (transaction.DateTime > _timeProvider.GetUtcNow())
            {
                throw new FinancialException("Invalid transaction date", $"Transaction date ({transaction.DateTime}) can not be in the future");
            }

            switch (transaction)
            {
                case DebitTransaction debitTransaction:
                    ValidateDebitTransaction(debitTransaction, client);
                    break;
                case CreditTransaction creditTransaction:
                    ValidateCreditTransaction(creditTransaction);
                    break;
                default:
                    throw new FinancialException("Unknown transaction type", $"Unknown transaction type:{transaction.GetType().Name}");
            }
        }

        private void ValidateCreditTransaction(CreditTransaction creditTransaction)
        {
            if (creditTransaction.Amount <= 0)
            {
                throw new FinancialException("Invalid transaction amount", $"Credit transaction amount ({creditTransaction.Amount}) can not be less or equal to zero");
            }
        }

        private void ValidateDebitTransaction(DebitTransaction debitTransaction, Client client)
        {
            if (debitTransaction.Amount > client.Balance)
            {
                throw new FinancialException("Insufficient funds", $"Debit transaction amount ({debitTransaction.Amount}) is higher than the client balance ({client.Balance})");
            }
        }
    }
}
