using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using NbtConverter.Tags;

namespace NbtConverter.Nbt
{
    static class Reader
    {
        public static StoreBase ReadFromFile(string file)
        {
            var compressed = false;
            
            using (var fileStream = new FileStream(file, FileMode.Open))
            using (var reader = new BinaryReader(fileStream))
            {
                if (reader.ReadByte() == 0x1f && reader.ReadByte() == 0x8b)
                    compressed = true;
            }

            if (compressed)
            {
                using (var fileStream = new FileStream(file, FileMode.Open))
                using (var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
                using (var reader = new NbtBinaryReader(gzipStream))
                {
                    return new StoreBase(/*BitConverter.IsLittleEndian, */true, ReadCompound(reader));
                }
            }
            else
            {
                using (var fileStream = new FileStream(file, FileMode.Open))
                using (var reader = new NbtBinaryReader(fileStream))
                {
                    return new StoreBase(/*BitConverter.IsLittleEndian, */false, ReadCompound(reader));
                }
            }
        }

        private static Tag ReadTag(Tag.TagId id, string? name, NbtBinaryReader reader)
        {
            switch (id)
            {
                case Tag.TagId.Byte:
                    return new TypeValue<sbyte>(name, reader.ReadSByte());
                case Tag.TagId.Short:
                    return new TypeValue<short>(name, reader.ReadInt16());
                case Tag.TagId.Int:
                    return new TypeValue<int>(name, reader.ReadInt32());
                case Tag.TagId.Long:
                    return new TypeValue<long>(name, reader.ReadInt64());
                case Tag.TagId.Float:
                    return new TypeValue<float>(name, reader.ReadSingle());
                case Tag.TagId.Double:
                    return new TypeValue<double>(name, reader.ReadDouble());
                case Tag.TagId.String:
                    return new TypeValue<string>(name, reader.ReadString(reader.ReadInt16()));

                case Tag.TagId.ByteArray:
                    return new ArrayValue<sbyte>(name, reader.ReadSBytes(reader.ReadInt32()));
                case Tag.TagId.IntArray:
                    return new ArrayValue<int>(name, reader.ReadInt32(reader.ReadInt32()));
                case Tag.TagId.LongArray:
                    return new ArrayValue<long>(name, reader.ReadInt64(reader.ReadInt32()));

                case Tag.TagId.List:
                    return ReadList(reader, name);
                case Tag.TagId.Compound:
                    return ReadCompound(reader, new Compound(name)) ?? throw new Exception("Failed reading compound.");
                default:
                    throw new ArgumentException($"ID value not supported: {id}", nameof(id));
            }
        }

        private static Compound? ReadCompound(NbtBinaryReader reader, Compound? nbtRoot = null, bool namelessRoot = false)
        {
            while (true)
            {
                var id = (Tag.TagId)reader.ReadSByte();

                if (!Enum.IsDefined(typeof(Tag.TagId), id))
                {
                    Console.WriteLine($"Unknown tag encountered with id {(int)id}, aborting operation.");
                    throw new NotSupportedException();
                }

                if (id == Tag.TagId.End)
                {
                    break;
                }

                if (nbtRoot == null)
                {
                    if (id == Tag.TagId.Compound)
                    {
                        if (namelessRoot)
                        {
                            nbtRoot = new Compound(null);
                        }
                        else
                        {
                            var rootLength = reader.ReadInt16();
                            var rootName = new string(reader.ReadChars(rootLength));
                            nbtRoot = new Compound(rootName);
                        }

                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Root element isn't a compound, aborting operation.");
                        throw new Exception();
                    }
                }

                var length = reader.ReadInt16();

                var name = new string(reader.ReadChars(length));

                nbtRoot.Elements.Add(ReadTag(id, name, reader));
            }

            return nbtRoot;
        }

        private static Tag ReadList(NbtBinaryReader reader, string? name)
        {
            var id = (Tag.TagId)reader.ReadSByte();

            if (!Enum.IsDefined(typeof(Tag.TagId), id) && id == Tag.TagId.End)
            {
                Console.WriteLine($"Unknown tag encountered with id {(int)id}, aborting operation.");
                throw new NotSupportedException();
            }

            var length = reader.ReadInt32();

            switch (id)
            {
                case Tag.TagId.End:
                    return new ListValue<TypeValue<object>>(name, length);

                case Tag.TagId.Byte:
                    var sbyteValues = new ListValue<TypeValue<sbyte>>(name, length);
                    for (int i = 0; i < length; i++) sbyteValues.Elements[i] = (TypeValue<sbyte>)ReadTag(Tag.TagId.Byte, null, reader);
                    return sbyteValues;

                case Tag.TagId.Short:
                    var shortValues = new ListValue<TypeValue<short>>(name, length);
                    for (int i = 0; i < length; i++) shortValues.Elements[i] = (TypeValue<short>)ReadTag(Tag.TagId.Short, null, reader);
                    return shortValues;

                case Tag.TagId.Int:
                    var intValues = new ListValue<TypeValue<int>>(name, length);
                    for (int i = 0; i < length; i++) intValues.Elements[i] = (TypeValue<int>)ReadTag(Tag.TagId.Int, null, reader);
                    return intValues;

                case Tag.TagId.Long:
                    var longValues = new ListValue<TypeValue<long>>(name, length);
                    for (int i = 0; i < length; i++) longValues.Elements[i] = (TypeValue<long>)ReadTag(Tag.TagId.Long, null, reader);
                    return longValues;

                case Tag.TagId.Float:
                    var floatValues = new ListValue<TypeValue<float>>(name, length);
                    for (int i = 0; i < length; i++) floatValues.Elements[i] = (TypeValue<float>)ReadTag(Tag.TagId.Float, null, reader);
                    return floatValues;

                case Tag.TagId.Double:
                    var doubleValues = new ListValue<TypeValue<double>>(name, length);
                    for (int i = 0; i < length; i++) doubleValues.Elements[i] = (TypeValue<double>)ReadTag(Tag.TagId.Double, null, reader);
                    return doubleValues;

                case Tag.TagId.String:
                    var stringValues = new ListValue<TypeValue<string>>(name, length);
                    for (int i = 0; i < length; i++) stringValues.Elements[i] = (TypeValue<string>)ReadTag(Tag.TagId.String, null, reader);
                    return stringValues;


                case Tag.TagId.ByteArray:
                    var byteArrays = new ListValue<ArrayValue<sbyte>>(name, length);
                    for (int i = 0; i < length; i++) byteArrays.Elements[i] = (ArrayValue<sbyte>)ReadTag(Tag.TagId.ByteArray, null, reader);
                    return byteArrays;

                case Tag.TagId.IntArray:
                    var intArrays = new ListValue<ArrayValue<int>>(name, length);
                    for (int i = 0; i < length; i++) intArrays.Elements[i] = (ArrayValue<int>)ReadTag(Tag.TagId.IntArray, null, reader);
                    return intArrays;

                case Tag.TagId.LongArray:
                    var longArrays = new ListValue<ArrayValue<long>>(name, length);
                    for (int i = 0; i < length; i++) longArrays.Elements[i] = (ArrayValue<long>)ReadTag(Tag.TagId.LongArray, null, reader);
                    return longArrays;


                case Tag.TagId.List:
                    var lists = new ListValue<Tag>(name, length);
                    for (int i = 0; i < length; i++) lists.Elements[i] = ReadList(reader, null);
                    return lists;

                case Tag.TagId.Compound:
                    var compounds = new ListValue<Compound>(name, length);
                    for (int i = 0; i < length; i++) compounds.Elements[i] = ReadCompound(reader, new Compound(null));
                    return compounds;

                default:
                    throw new ArgumentOutOfRangeException(nameof(id));
            }
        }
    }
}
