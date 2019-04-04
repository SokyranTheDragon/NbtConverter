using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NbtConverter.Nbt
{
    class NbtBinaryWriter : BinaryWriter
    {
        protected NbtBinaryWriter() { }

        public NbtBinaryWriter(Stream output) : base(output) { }

        public NbtBinaryWriter(Stream output, Encoding encoding) : base(output, encoding) { }

        public NbtBinaryWriter(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen) { }

        public override void Write(string value)
        {
            Write((short)value.Length);
            base.Write(value.ToCharArray());
        }
        
        public void Write(sbyte[] values)
        {
            Write(values.Length);
            foreach (var value in values) Write(value);
        }

        public override void Write(short value)
        {
            if (!BitConverter.IsLittleEndian) base.Write(value);
            else
            {
                var data = BitConverter.GetBytes(value);
                Array.Reverse(data);
                Write(data);
            }
        }

        public void Write(short[] values)
        {
            Write(values.Length);
            foreach (var value in values) Write(value);
        }

        public override void Write(int value)
        {
            if (!BitConverter.IsLittleEndian) base.Write(value);
            else
            {
                var data = BitConverter.GetBytes(value);
                Array.Reverse(data);
                Write(data);
            }
        }

        public void Write(int[] values)
        {
            Write(values.Length);
            foreach (var value in values) Write(value);
        }

        public override void Write(long value)
        {
            if(BitConverter.IsLittleEndian) base.Write(value);
            else
            {
                var data = BitConverter.GetBytes(value);
                Array.Reverse(data);
                Write(data);
            }
        }

        public void Write(long[] values)
        {
            Write(values.Length);
            foreach (var value in values) Write(value);
        }

        public void Write(float[] values)
        {
            Write(values.Length);
            foreach (var value in values) Write(value);
        }

        public void Write(double[] values)
        {
            Write(values.Length);
            foreach (var value in values) Write(value);
        }
    }
}
