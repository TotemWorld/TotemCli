using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotemCli.Utils
{
    public static class StringParsingUtils
    {
        public static string[] SpitByComma(this string rawString)
        {
            var separations = rawString.Split(',');
            var splitted = new string[separations.Length];
            for(var i = 0; i < separations.Length; i++)
            {
                splitted[i] = separations[i].Trim();
            }
            return splitted;
        }
    }
}
