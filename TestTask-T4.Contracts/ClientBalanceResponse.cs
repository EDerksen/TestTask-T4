using System.Text.Json.Serialization;

namespace TestTask_T4.Contracts
{
    public record CLientBalanceResponse
    {
        [JsonPropertyName("balanceDateTime")]
        public DateTimeOffset BalanceDateTime { get; init; }

        [JsonPropertyName("clientBalance")]
        public decimal Balance { get; init; }
    }
}
