using System;
using System.Collections.Generic;
using System.Text;

namespace DeParnasso.Core.Extensions
{
    public static class StringExtensions
    {
        public static int? TryGetInt(this string input)
        {
            int i;
            bool success = int.TryParse(input, out i);
            return success ? i : default(int?);
        }
    }
}
