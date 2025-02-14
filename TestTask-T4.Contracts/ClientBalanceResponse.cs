using System.Text.Json.Serialization;
using TestTask_T4.Contracts.Converters;

namespace TestTask_T4.Contracts
{
    public record ClientBalanceResponse
    {
        [JsonPropertyName("balanceDateTime")]
        [JsonConverter(typeof(DateJsonConverter))]
        public DateTime BalanceDateTime { get; init; }

        [JsonPropertyName("clientBalance")]
        public decimal Balance { get; init; }
    }
}
