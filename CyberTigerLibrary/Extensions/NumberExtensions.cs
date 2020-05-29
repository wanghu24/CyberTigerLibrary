using System;
using System.Collections.Generic;
using System.Text;

namespace CyberTigerLibrary.Extensions
{
    public static class NumberExtensions
    {
        public static string OrdinalSuffix(this int num)
        {
            string number = num.ToString();
            if (number.EndsWith("11")) return number + "th";
            if (number.EndsWith("12")) return number + "th";
            if (number.EndsWith("13")) return number + "th";
            if (number.EndsWith("1")) return number + "st";
            if (number.EndsWith("2")) return number + "nd";
            if (number.EndsWith("3")) return number + "rd";
            return number + "th";
        }

        public static bool IsInRange(this int target, int start, int end)
        {

            return target >= start && target <= end;

        }

        public static bool IsEven(this int value)
        {
            return value % 2 == 0;
        }

        public static bool IsOdd(this int value)
        {
            return value % 2 != 0;
        }
    }
}
