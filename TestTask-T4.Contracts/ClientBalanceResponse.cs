using System.Text.Json.Serialization;

namespace TestTask_T4.Contracts
{
    public record ClientBalanceResponse
    {
        [JsonPropertyName("balanceDateTime")]
        public DateTime BalanceDateTime { get; init; }

        [JsonPropertyName("clientBalance")]
        public decimal Balance { get; init; }
    }
}
