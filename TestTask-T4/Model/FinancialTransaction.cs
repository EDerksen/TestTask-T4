using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestTask_T4.Contracts;
using TestTask_T4.Data.Extensions;

namespace TestTask_T4.Model
{
    public enum FinancialTransactionType
    {
        Debit = 0,
        Credit = 1
    }

    public record FinancialTransaction : ITransaction, IEntityCreatedAt
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }
        public required Client Client { get; set; }

        public Guid BalanceSnapshotId { get; set; }
        public required ClientBalanceSnapshot BalanceSnapshot { get; set; }

        public DateTime DateTime { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public FinancialTransactionType Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
