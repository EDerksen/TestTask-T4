using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestTask_T4.Contracts;

namespace TestTask_T4.Model
{
    public enum FinanceTransactionType
    {
        Debit = 0,
        Credit = 1
    }

    public record FinanceTransaction : ITransaction
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }
        public required Client Client { get; set; }

        public DateTime DateTime { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public FinanceTransactionType Type { get; set; }
    }
}
