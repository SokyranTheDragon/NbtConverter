using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NbtConverter.Tags
{
    class ArrayValue<T> : Tag
    {
        public T[] Elements { get; }

        public ArrayValue(string? name, int count) : base(name)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            Elements = new T[count];
        }

        public ArrayValue(string? name, T[] elements) : base(name) => Elements = elements;

        public override string ToString() => $"Type: {typeof(T)}, Count: {Elements.Length}";

        public override void WriteJson(JsonWriter writer, JObject parent, JsonSerializer serializer)
        {
            var self = new JObject();
            ((JArray)parent["Elements"]).Add(self);

            self.Add("SelfType", "ArrayValue");
            if (Name != null) self["Name"] = Name;
            self["Type"] = typeof(T).ToString();
            self["Elements"] = new JArray(Elements.Select(x => x.ToString()));
        }
    }
}
