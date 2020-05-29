using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CyberTigerLibrary.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveLastCharacter(this String instr)
        {
            return instr.Substring(0, instr.Length - 1);
        }
        public static string RemoveLast(this String instr, int number)
        {
            return instr.Substring(0, instr.Length - number);
        }
        public static string RemoveFirstCharacter(this String instr)
        {
            return instr.Substring(1);
        }
        public static string RemoveFirst(this String instr, int number)
        {
            return instr.Substring(number);
        }
        public static string Remove(this string input, string strToRemove)
        {
            if (input.IsNullOrEmpty())
            {
                return null;
            }

            return input.Replace(strToRemove, "");
        }

        public static string ToCamelCasing(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return value.Substring(0, 1).ToUpper() + value.Substring(1, value.Length - 1);
            }

            return value;
        }

        public static string FormatFirstLetterUpperCase(this string value, string culture = "en-US")
        {
            return CultureInfo.GetCultureInfo(culture).TextInfo.ToTitleCase(value);
        }

        public static Stream ToStream(this string str)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            //byte[] byteArray = Encoding.ASCII.GetBytes(str);
            return new MemoryStream(byteArray);
        }
        public static string ToString(this Stream stream)
        {
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
        public static void CopyTo(this Stream fromStream, Stream toStream)
        {
            if (fromStream == null)
                throw new ArgumentNullException("fromStream");
            if (toStream == null)
                throw new ArgumentNullException("toStream");
            var bytes = new byte[8092];
            int dataRead;
            while ((dataRead = fromStream.Read(bytes, 0, bytes.Length)) > 0)
                toStream.Write(bytes, 0, dataRead);
        }

        public static int Occurrence(this String instr, string search)
        {
            return Regex.Matches(instr, search).Count;
        }

        public static string OnlyDigits(this string value)
        {
            return new string(value?.Where(c => char.IsDigit(c)).ToArray());
        }

        public static string RandomString(this string chars, int length = 8)
        {
            var randomString = new StringBuilder();
            var random = new Random();

            for (int i = 0; i < length; i++)
                randomString.Append(chars[random.Next(chars.Length)]);

            return randomString.ToString();
        }

        public static string Reverse(this string input)
        {
            char[] chars = input.ToCharArray();
            Array.Reverse(chars);
            return new String(chars);
        }
    }
}
