using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NbtConverter.Tags
{
    class ListValue<T> : ArrayValue<T>, IListValue where T : ITag
    {
        public int ElementsCount => Elements.Length;

        public ListValue(string? name, int count) : base(name, count) { }

        public ListValue(string? name, T[] elements) : base(name, elements) { }

        public override void WriteJson(JsonWriter writer, JObject parent, JsonSerializer serializer)
        {
            var self = new JObject();
            ((JArray)parent["Elements"]).Add(self);

            self.Add("SelfType", "ListValue");
            if (Name != null) self["Name"] = Name;

            if (Elements.Length == 0)
            {
                self["Type"] = null;
            }
            else
            {
                self["Type"] = typeof(T).ToString();
                self["Elements"] = new JArray();
                foreach (var element in Elements) (element as Tag).WriteJson(writer, self, serializer);
            }
        }
    }

    interface IListValue : ITag { }
}
