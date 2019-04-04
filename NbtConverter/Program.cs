using System;
using System.IO;
using NbtConverter.Nbt;
using NbtConverter.Tags;
using Newtonsoft.Json;

namespace NbtConverter
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            if (args.Length == 0)
            {
                args = new [] { /*"level.dat",*/ "level.dat.json" };
            }
#endif

            var converter = new NbtJsonConverter();

            foreach (var arg in args)
            {
                var file = new FileInfo(arg);

                if (file.Exists)
                {
                    if (file.Extension == ".json")
                    {
                        string input = "";

                        try
                        {
                            input = File.ReadAllText(file.FullName);
                        }
                        catch (Exception) { }
                        
                        if (!string.IsNullOrWhiteSpace(input))
                        {
                            var data = JsonConvert.DeserializeObject<StoreBase>(input, converter);
                        }
                    }
                    else
                    {
                        var data = Reader.ReadFromFile(file.FullName);

                        Writer.SaveToFile($"{file.FullName}.copy.nbt", data);

                        var output = JsonConvert.SerializeObject(data, Formatting.Indented, converter);

                        try
                        {
                            File.WriteAllText($"{file.FullName}.json", output);
                        }
                        catch (Exception) { }
                    }
                }
            }
        }
    }
}
