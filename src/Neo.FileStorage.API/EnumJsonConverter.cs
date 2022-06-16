#nullable enable

using System;
using System.Linq;
using Newtonsoft.Json;

namespace Neo.FileStorage.API
{
    public class EnumJsonConverter : JsonConverter
    {
        public override bool CanRead { get; } = true;
        public override bool CanWrite { get; } = true;

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.ValueType != typeof(string)) return existingValue;
            var s = (string?)reader.Value;
            if (s?.Contains('_') == true)
            {
                s = s.Split('_').Last();
            }
            return s is not null ? Enum.Parse(objectType, s, true) : existingValue;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is null)
            {
                writer.WriteValue(value);
                return;
            }
            string s = "";
            if ((int)value == 0)
            {
                s = value.GetType().Name.ToUpper() + "_" + value?.ToString()?.ToUpper();
            }
            writer.WriteValue(s);
        }
    }
}
