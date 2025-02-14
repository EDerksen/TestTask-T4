using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestTask_T4.Contracts.Converters
{
    public class FinancePrecisionConverter : JsonConverter<decimal>
    {
        private const int Precision=18;
        private const int Scale=2;


        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            decimal value = reader.GetDecimal();
            decimal roundedValue = Math.Round(value, Scale, MidpointRounding.AwayFromZero);

            if(roundedValue != value)
            {
                throw new JsonException($"Value exceeds max scale of {Scale} digits.");
            }

            if (roundedValue.ToString().Replace(".", "").Length > Precision)
            {
                throw new JsonException($"Value exceeds max precision of {Precision} digits.");
            }

            return roundedValue;
        }

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(Math.Round(value, Scale));
        }
    }
}
