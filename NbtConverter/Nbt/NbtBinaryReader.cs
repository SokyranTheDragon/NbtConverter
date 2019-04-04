using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NbtConverter.Nbt
{
    class NbtBinaryReader : BinaryReader
    {
        public NbtBinaryReader(Stream input) : base(input) { }

        public NbtBinaryReader(Stream input, Encoding encoding) : base(input, encoding) { }

        public NbtBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen) { }

        public string ReadString(int length) => new string(ReadChars(length));
        
        public string[] ReadStrings(int count)
        {
            var strings = new string[count];

            for (int i = 0; i < count; i++) strings[i] = ReadString(ReadInt16());

            return strings;
        }

        public sbyte[] ReadSBytes(int count)
        {
            var bytes = new sbyte[count];

            for (int i = 0; i < count; i++) bytes[i] = ReadSByte();

            return bytes;
        }

        public override short ReadInt16()
        {
            if (!BitConverter.IsLittleEndian) return base.ReadInt16();

            var data = base.ReadBytes(2);
            Array.Reverse(data);
            return BitConverter.ToInt16(data);
        }

        public short[] ReadInt16(int count)
        {
            var shorts = new short[count];

            for (int i = 0; i < count; i++) shorts[i] = ReadInt16();

            return shorts;
        }

        public override int ReadInt32()
        {
            if (!BitConverter.IsLittleEndian) return base.ReadInt32();

            var data = base.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToInt32(data);
        }

        public int[] ReadInt32(int count)
        {
            var ints = new int[count];

            for (int i = 0; i < count; i++) ints[i] = ReadInt32();

            return ints;
        }

        //public override long ReadInt64()
        //{
        //    if (!BitConverter.IsLittleEndian) return base.ReadInt64();

        //    var data = base.ReadBytes(8);
        //    Array.Reverse(data);
        //    return BitConverter.ToInt64(data);
        //}

        public long[] ReadInt64(int count)
        {
            var longs = new long[count];

            for (int i = 0; i < count; i++) longs[i] = ReadInt64();

            return longs;
        }

        public float[] ReadFloat(int count)
        {
            var floats = new float[count];

            for (int i = 0; i < count; i++) floats[i] = ReadSingle();

            return floats;
        }

        public double[] ReadDouble(int count)
        {
            var doubles = new double[count];

            for (int i = 0; i < count; i++) doubles[i] = ReadDouble();

            return doubles;
        }
    }
}
