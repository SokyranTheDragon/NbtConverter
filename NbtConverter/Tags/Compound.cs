using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NbtConverter.Tags
{
    class Compound : Tag
    {
        public List<Tag> Elements { get; } = new List<Tag>();

        public Compound(string? name) : base(name) { }

        public override string ToString() => $"Elements: {Elements.Count}";

        public override void WriteJson(JsonWriter writer, JObject parent, JsonSerializer serializer)
        {
            var self = new JObject();
            ((JArray)parent["Elements"]).Add(self);

            self.Add("SelfType", "Compound");
            if (Name != null) self["Name"] = Name;
            self["Elements"] = new JArray();

            foreach (var element in Elements)
            {
                element.WriteJson(writer, self, serializer);
            }
        }
    }
}
