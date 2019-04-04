using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NbtConverter.Tags
{
    abstract class Tag : ITag
    {
        public string? Name { get; private set; }

        protected Tag() { }

        public Tag(string? name) => Name = name;

        public abstract override string ToString();

        public abstract void WriteJson(JsonWriter writer, JObject parent, JsonSerializer serializer);

        public static Compound ReadJson(JObject parent, JsonSerializer serializer)
        {
            return null;
        }

        private static List<Tag> Read(JObject parent, JsonSerializer serializer)
        {
            return null;
        }

        public enum TagId : sbyte
        {
            // Choosing the value is not needed (as it should start at 0 and incement by 1 with each value),
            // but I decided to do it like this for full clarity.
            End = 0,
            Byte = 1,
            Short = 2,
            Int = 3,
            Long = 4,
            Float = 5,
            Double = 6,
            ByteArray = 7,
            String = 8,
            List = 9,
            Compound = 10,
            IntArray = 11,
            LongArray = 12,
        }
    }

    interface ITag { string? Name { get; } }
}
