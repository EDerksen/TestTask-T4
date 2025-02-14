using TestTask_T4.Contracts;
using TestTask_T4.Model;

namespace TestTask_T4.Services.Finance
{
    public class TransactionValidator : ITransactionValidator
    {
        public void ValidateClientTransaction(ITransaction transaction, Client client)
        {
            switch (transaction)
            {
                case DebitTransaction debitTransaction:
                    ValidateDebitTransaction(debitTransaction, client);
                    break;
                case CreditTransaction creditTransaction:
                    ValidateCreditTransaction(creditTransaction);
                    break;
                default:
                    throw new InvalidOperationException("Invalid transaction type");
            }
        }

        private void ValidateCreditTransaction(CreditTransaction creditTransaction)
        {
            if (creditTransaction.Amount <= 0)
            {
                throw new InvalidOperationException("Invalid credit amount");
            }
        }

        private void ValidateDebitTransaction(DebitTransaction debitTransaction, Client client)
        {
            if (debitTransaction.Amount > client.Balance)
            {
                throw new InvalidOperationException("Insufficient funds");
            }
        }
    }
}
