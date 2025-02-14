using TestTask_T4.Contracts;

namespace TestTask_T4.Model
{
    public record CreditTransaction : ITransaction
    {
        public Guid Id { get; init; }

        public Guid ClientId { get; init; }

        public DateTime DateTime { get; init; }

        public decimal Amount { get; init; }
    }
}
