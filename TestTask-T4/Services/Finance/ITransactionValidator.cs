using TestTask_T4.Contracts;
using TestTask_T4.Model;

namespace TestTask_T4.Services.Finance
{
    public interface ITransactionValidator
    {
        void ValidateClientTransaction(ITransaction transaction, Client client);
        void ValidateDuplicatingTransaction(ITransaction transaction, FinancialTransaction existingTransaction);
    }
}
