using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestTask_T4.Contracts
{
    public record DebitTransactionRequest
    {
        [JsonPropertyName("id")]
        public Guid Id { get; init; }

        [JsonPropertyName("clientId")]
        public Guid ClientId { get; init; }

        [JsonPropertyName("dateTime")]
        public DateTimeOffset DateTime { get; init; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; init; }
    }
}
