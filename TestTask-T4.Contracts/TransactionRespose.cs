using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestTask_T4.Contracts
{
    public record TransactionRespose
    {
        [JsonPropertyName("insertDateTime")]
        public DateTimeOffset InsertDateTime { get; init; }

        [JsonPropertyName("clientBalance")]
        public decimal ClientBalance { get; init; }
    }
}
