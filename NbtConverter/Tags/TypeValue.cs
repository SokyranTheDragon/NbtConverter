using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NbtConverter.Tags
{
    class TypeValue<T> : Tag
    {
        public T Value { get; }

        public TypeValue(string? name, T value) : base(name)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            Value = value;
        }

        public override string ToString() => $"Value: {Value}";

        public override void WriteJson(JsonWriter writer, JObject parent, JsonSerializer serializer)
        {
            var self = new JObject();
            ((JArray)parent["Elements"]).Add(self);

            self.Add("SelfType", "TypeValue");
            if (Name != null) self["Name"] = Name;
            self["Type"] = typeof(T).ToString();
            self["Value"] = Value?.ToString();
        }
    }
}
