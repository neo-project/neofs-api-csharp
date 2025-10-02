// Copyright (C) 2015-2025 The Neo Project.
//
// EnumJsonConverter.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

#nullable enable

using Newtonsoft.Json;
using System;
using System.Linq;

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
