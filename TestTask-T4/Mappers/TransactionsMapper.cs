using Riok.Mapperly.Abstractions;
using TestTask_T4.Contracts;
using TestTask_T4.Model;

namespace TestTask_T4.Mappers
{
    [Mapper]
    public static partial class TransactionsMapper
    {
        public static partial CreditTransaction ToTransaction(this CreditTransactionRequest creditTransactionRequest);
        public static partial DebitTransaction ToTransaction(this DebitTransactionRequest debitTransactionRequest);
        public static partial TransactionRespose ToResponse(this TransactionResult transactionResult);

        public static DateTime ToDateTime(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.UtcDateTime;
        }
    }
}
