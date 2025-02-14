using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TestTask_T4.Contracts.Converters;

namespace TestTask_T4.Contracts
{
    public record RevertTransactionResponse
    {
        [JsonPropertyName("revertDateTime")]
        [JsonConverter(typeof(DateJsonConverter))]
        public DateTime RevertDateTime { get; init; }

        [JsonPropertyName("clientBalance")]
        public decimal ClientBalance { get; init; }
    }
}
