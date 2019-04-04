using System;
using System.Collections.Generic;
using System.Text;
using NbtConverter.Tags;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NbtConverter
{
    class StoreBase
    {
        //public bool LittleEndian { get; }
        public bool Compressed { get; }
        public Compound? BaseCompount { get; }

        public StoreBase(/*bool littleEndian, */bool compressed, Compound? baseCompount)
        {
            //LittleEndian = littleEndian;
            Compressed = compressed;
            BaseCompount = baseCompount;
        }

        public static StoreBase ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            JObject self = JObject.Load(reader);

            return new StoreBase(/*(bool)self["LittleEndian"], */(bool)self["Compressed"], Tag.ReadJson(self, serializer));
        }

        public void WriteJson(JsonWriter writer, JsonSerializer serializer)
        {
            if (BaseCompount is null) throw new Exception($"{nameof(BaseCompount)} cannot be null!");

            var self = new JObject
            {
                //["LittleEndian"] = LittleEndian,
                ["Compressed"] = Compressed,
                ["Elements"] = new JArray(),
            };

            BaseCompount.WriteJson(writer, self, serializer);
            
            self.WriteTo(writer);
        }
    }
}
