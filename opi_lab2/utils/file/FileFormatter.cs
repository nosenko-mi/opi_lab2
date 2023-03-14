using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace opi_lab2.utils
{
    public static class FileFormatter
    {
        //видаляти всі незначні прогалини(зайві прогалини, табуляції та порожні рядки).
        public static string FormatToPlainText(string input)
        {
            // @"\s{2,}?" - all whitespaces
            //  @"^\s+$[\r\n]*" - only empty lines
            // @" {2,}|\t" - 2+ whitespaces or tab
            string resultString = Regex.Replace(input, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
            string output = Regex.Replace(resultString, @" {2,}|\t", " ");
            return output;
        }
    }
}
