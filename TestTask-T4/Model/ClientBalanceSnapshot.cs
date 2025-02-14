using System.ComponentModel.DataAnnotations.Schema;

namespace TestTask_T4.Model
{
    public record ClientBalanceSnapshot
    {
        public Guid Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
    }
}
