using System;
using System.Collections.Generic;
using System.Text;

namespace NbtConverter.Nbt
{
    static class JsonReaderStringExtension
    {
        public static string GetElementType(this string element)
        {
            int index = element.LastIndexOf(' ');

            if (index == -1) return element;
            else return element.Substring(0, index);
        }
    }
}
