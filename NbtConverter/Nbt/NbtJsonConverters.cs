using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NbtConverter.Tags;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NbtConverter.Nbt
{
    class NbtJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(StoreBase);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return StoreBase.ReadJson(reader, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ((StoreBase)value).WriteJson(writer, serializer);
        }
    }
}
