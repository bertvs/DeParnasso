using System;

namespace DeParnasso.Core.Extensions
{
    public static class CharExtensions
    {
        public static int ToInt(this char source)
        {
            return source - '0';
        }
    }
}
