using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestTask_T4.Contracts
{
    public record DebitTransactionRequest
    {
        [Required]
        [JsonPropertyName("id")]
        public Guid Id { get; init; }

        [Required]
        [JsonPropertyName("clientId")]
        public Guid ClientId { get; init; }

        [Required]
        [JsonPropertyName("dateTime")]
        public DateTimeOffset DateTime { get; init; }

        [Required]
        [JsonPropertyName("amount")]
        [Range(0, 9999999999999999.99)]
        public decimal Amount { get; init; }
    }
}
