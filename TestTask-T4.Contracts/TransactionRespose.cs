using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TestTask_T4.Contracts.Converters;

namespace TestTask_T4.Contracts
{
    public record TransactionRespose
    {
        [JsonPropertyName("insertDateTime")]
        [JsonConverter(typeof(DateJsonConverter))]
        public DateTime InsertDateTime { get; init; }

        [JsonPropertyName("clientBalance")]
        public decimal ClientBalance { get; init; }
    }
}
