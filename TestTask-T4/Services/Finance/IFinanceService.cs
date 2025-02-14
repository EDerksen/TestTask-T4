using TestTask_T4.Contracts;
using TestTask_T4.Model;

namespace TestTask_T4.Services.Finance
{
    public interface IFinanceService
    {
        Task<TransactionResult> ProcessTransaction(ITransaction transaction, CancellationToken cancellationToken = default);
        Task<RevertTransactionResult> RevertTransaction(Guid transactionId, CancellationToken cancellationToken = default);
    }
}
