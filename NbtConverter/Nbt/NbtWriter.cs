using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using NbtConverter.Tags;

namespace NbtConverter.Nbt
{
    static class Writer
    {
        public static void SaveToFile(string file, StoreBase content)
        {
            Debug.Assert(content.BaseCompount != null);

            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                if (content.Compressed)
                {
                    using (var gzipStream = new GZipStream(fileStream, CompressionLevel.Optimal))
                    using (var binaryWriter = new NbtBinaryWriter(gzipStream, Encoding.UTF8))
                    {
                        WriteCompound(binaryWriter, content.BaseCompount);
                    }
                }
                else
                {
                    using (var binaryWriter = new NbtBinaryWriter(fileStream, Encoding.UTF8))
                    {
                        WriteCompound(binaryWriter, content.BaseCompount);
                    }
                }
            }
        }

        private static void WriteTag(NbtBinaryWriter writer, Tag element)
        {
            if (element is TypeValue<sbyte> sbyteValue)
            {
                writer.Write((sbyte)Tag.TagId.Byte);
                if (element.Name != null) writer.Write(element.Name);
                writer.Write(sbyteValue.Value);
            }
            else if (element is TypeValue<short> shortValue)
            {
                writer.Write((sbyte)Tag.TagId.Short);
                if (element.Name != null) writer.Write(element.Name);
                writer.Write(shortValue.Value);
            }
            else if (element is TypeValue<int> intValue)
            {
                writer.Write((sbyte)Tag.TagId.Int);
                if (element.Name != null) writer.Write(element.Name);
                writer.Write(intValue.Value);
            }
            else if (element is TypeValue<long> longValue)
            {
                writer.Write((sbyte)Tag.TagId.Long);
                if (element.Name != null) writer.Write(element.Name);
                writer.Write(longValue.Value);
            }
            else if (element is TypeValue<float> floatValue)
            {
                writer.Write((sbyte)Tag.TagId.Float);
                if (element.Name != null) writer.Write(element.Name);
                writer.Write(floatValue.Value);
            }
            else if (element is TypeValue<double> doubleValue)
            {
                writer.Write((sbyte)Tag.TagId.Double);
                if (element.Name != null) writer.Write(element.Name);
                writer.Write(doubleValue.Value);
            }
            else if (element is TypeValue<string> stringValue)
            {
                writer.Write((sbyte)Tag.TagId.String);
                if (element.Name != null) writer.Write(element.Name);
                writer.Write(stringValue.Value);
            }


            else if (element is ArrayValue<sbyte> sbyteArray)
            {
                writer.Write((sbyte)Tag.TagId.ByteArray);
                if (element.Name != null) writer.Write(element.Name);
                writer.Write(sbyteArray.Elements);
            }
            else if (element is ArrayValue<int> intArray)
            {
                writer.Write((sbyte)Tag.TagId.IntArray);
                if (element.Name != null) writer.Write(element.Name);
                writer.Write(intArray.Elements);
            }
            else if (element is ArrayValue<long> longArray)
            {
                writer.Write((sbyte)Tag.TagId.LongArray);
                if (element.Name != null) writer.Write(element.Name);
                writer.Write(longArray.Elements);
            }


            else if (element is IListValue list)
            {
                writer.Write((sbyte)Tag.TagId.List);
                if (element.Name != null) writer.Write(element.Name);
                WriteList(writer, list);
            }
            else if (element is Compound compound)
            {
                WriteCompound(writer, compound);
            }
            else throw new Exception("Unsupported type.");
        }

        private static void WriteCompound(NbtBinaryWriter writer, Compound nbtRoot, bool writeTag = true)
        {
            if (writeTag) writer.Write((sbyte)Tag.TagId.Compound);

            if (nbtRoot.Name != null) writer.Write(nbtRoot.Name);
            foreach (var element in nbtRoot.Elements) WriteTag(writer, element);
            writer.Write((sbyte)Tag.TagId.End);
        }

        private static void WriteList(NbtBinaryWriter writer, IListValue list)
        {
            if (list is ListValue<TypeValue<object>> emptyList)
            {
                writer.Write((sbyte)Tag.TagId.End);
                writer.Write(emptyList.Elements.Length);
            }


            else if (list is ListValue<TypeValue<sbyte>> sbyteList)
            {
                writer.Write((sbyte)Tag.TagId.Byte);
                writer.Write(sbyteList.Elements.Length);
                foreach (var element in sbyteList.Elements) writer.Write(element.Value);
            }
            else if (list is ListValue<TypeValue<short>> shortList)
            {
                writer.Write((sbyte)Tag.TagId.Short);
                writer.Write(shortList.Elements.Length);
                foreach (var element in shortList.Elements) writer.Write(element.Value);
            }
            else if (list is ListValue<TypeValue<int>> intList)
            {
                writer.Write((sbyte)Tag.TagId.Int);
                writer.Write(intList.Elements.Length);
                foreach (var element in intList.Elements) writer.Write(element.Value);
            }
            else if (list is ListValue<TypeValue<long>> longList)
            {
                writer.Write((sbyte)Tag.TagId.Long);
                writer.Write(longList.Elements.Length);
                foreach (var element in longList.Elements) writer.Write(element.Value);
            }
            else if (list is ListValue<TypeValue<float>> floatList)
            {
                writer.Write((sbyte)Tag.TagId.Float);
                writer.Write(floatList.Elements.Length);
                foreach (var element in floatList.Elements) writer.Write(element.Value);
            }
            else if (list is ListValue<TypeValue<double>> doubleList)
            {
                writer.Write((sbyte)Tag.TagId.Double);
                writer.Write(doubleList.Elements.Length);
                foreach (var element in doubleList.Elements) writer.Write(element.Value);
            }
            else if (list is ListValue<TypeValue<string>> stringList)
            {
                writer.Write((sbyte)Tag.TagId.String);
                writer.Write(stringList.Elements.Length);
                foreach (var element in stringList.Elements) writer.Write(element.Value);
            }


            else if (list is ListValue<ArrayValue<sbyte>> sbyteArrayList)
            {
                writer.Write((sbyte)Tag.TagId.ByteArray);
                writer.Write(sbyteArrayList.Elements.Length);
                foreach (var element in sbyteArrayList.Elements) writer.Write(element.Elements);
            }
            else if (list is ListValue<ArrayValue<int>> intArrayList)
            {
                writer.Write((sbyte)Tag.TagId.IntArray);
                writer.Write(intArrayList.Elements.Length);
                foreach (var element in intArrayList.Elements) writer.Write(element.Elements);
            }
            else if (list is ListValue<ArrayValue<long>> longArrayList)
            {
                writer.Write((sbyte)Tag.TagId.LongArray);
                writer.Write(longArrayList.Elements.Length);
                foreach (var element in longArrayList.Elements) writer.Write(element.Elements);
            }


            else if (list is ListValue<IListValue> listList)
            {
                writer.Write((sbyte)Tag.TagId.List);
                writer.Write(listList.Elements.Length);
                foreach (var element in listList.Elements) WriteList(writer, listList);
            }
            else if (list is ListValue<Compound> compoundList)
            {
                writer.Write((sbyte)Tag.TagId.Compound);
                writer.Write(compoundList.Elements.Length);
                foreach (var element in compoundList.Elements) WriteCompound(writer, element, false);
                //foreach (var subelementelement in element.Elements) 
            }
            else throw new Exception("Unsupported type.");
        }
    }
}
