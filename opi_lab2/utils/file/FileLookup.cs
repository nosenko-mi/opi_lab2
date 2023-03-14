using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace opi_lab2.utils.file
{
    public static class FileLookup
    {
        private static readonly string COMPLEX_NUMBER_PATTERN = @"['""“‘’”][+-]?\d?[i]['""“‘’”]|['""“‘’”][+-]?\d+\s?[+-]\s?[i]['""“‘’”]|['""“‘’”][+-]?\d+\s?[+-]\s?\d+[i]['""“‘’”]|['""“‘’”][+-]?\d+\s?[+-]\s?[i]\d+['""“‘’”]|['""“‘’”][+-]?\d+\s?[+-]\s?[i]\*\d+['""“‘’”]|['""“‘’”][+-]?\d+\s?[+-]\s?\d+\*[i]['""“‘’”]";

        public static List<TextRange> FindComplexNumbers(ref RichTextBox textBox)
        {
            string text = StringFromRichTextBox(textBox);

            Regex regex = new Regex(COMPLEX_NUMBER_PATTERN, RegexOptions.Compiled);

            MatchCollection matchs = regex.Matches(text);

            List<TextRange> ranges = new List<TextRange>();
            TextPointer position = textBox.Document.ContentStart;
            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    foreach (Match match in matchs)
                    {
                        int index = match.Index;
                        int length = match.Length;
                        string value = match.Value;
                        Console.WriteLine($"Highlight: {value}: [{index}]-[{index + length}]");
                        TextPointer start = position.GetPositionAtOffset(index);
                        TextPointer end = start.GetPositionAtOffset(length);

                        TextRange matchRange = new TextRange(start, end);
                        ranges.Add(matchRange);
                    }
                }
                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }
            return ranges;
        }

        public static List<TextRange> FindTextStrict(ref RichTextBox contentTextBox, ref RichTextBox searchTextBox)
        {
            string contentText = StringFromRichTextBox(contentTextBox);
            string searchText = StringFromRichTextBox(searchTextBox);
            if (searchText.Contains("\r\n"))
                searchText = searchText.Replace("\r\n", "");

            List<int> lines = new List<int>();
            int line = 0;
            List<TextRange> ranges = new List<TextRange>();

            string[] l = LinesFromRichTextBox(contentTextBox);
            for (int i = 0; i < l.Length; i++)
            {
                List<int> indexes = FindAllOccurrences(l[i], searchText);

            }

            for (TextPointer startPointer = contentTextBox.Document.ContentStart;
                startPointer.CompareTo(contentTextBox.Document.ContentEnd) <= 0;
                startPointer = startPointer.GetNextContextPosition(LogicalDirection.Forward))
            {
                //check if end of text
                if (startPointer.CompareTo(contentTextBox.Document.ContentEnd) == 0)
                    break;

                //get the adjacent string
                string parsedString = startPointer.GetTextInRun(LogicalDirection.Forward);

                line++;
                //Console.WriteLine($"line: {line}; string: {parsedString}");
                List<int> indexes = FindAllOccurrences(parsedString, searchText);
                foreach (int i in indexes)
                {
                    if (i < 0)
                        break;

                    Console.WriteLine($"start position: {i}");
                    //setting up the pointer here at this matched index
                    startPointer = startPointer.GetPositionAtOffset(i);
                    if (startPointer == null)
                        break;

                    //next pointer will be the length of the search string
                    TextPointer nextPointer = startPointer.GetPositionAtOffset(searchText.Length);
                    TextRange searchedTextRange = new TextRange(startPointer, nextPointer);
                    ranges.Add(searchedTextRange);
                    lines.Add(line);
                }
            }
            lines.ForEach(e => Console.WriteLine(e));
            return ranges;
        }

        public static Dictionary<int, string> FindTextStrictLines(ref RichTextBox contentTextBox, ref RichTextBox searchTextBox)
        {
            string searchText = StringFromRichTextBox(searchTextBox);
            if (searchText.Contains("\r\n"))
                searchText = searchText.Replace("\r\n", "");

            string[] lines = LinesFromRichTextBox(contentTextBox);
            Dictionary<int, string> result = new Dictionary<int, string>();

            for (int i = 0; i < lines.Length; i++)
            {
                List<int> indexes = FindAllOccurrences(lines[i], searchText);
                if (indexes.Count > 0)
                {
                    result.Add(i, lines[i]);
                }
            }

            return result;
        }

        private static string[] LinesFromRichTextBox(RichTextBox textBox)
        {
            TextRange textRange = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd);
            return textRange.Text.Split('\n');
        }

        private static string StringFromRichTextBox(RichTextBox textBox)
        {
            TextRange textRange = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd);
            textRange.ClearAllProperties();
            return textRange.Text;
        }

        private static List<int> FindAllOccurrences(string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");

            List<int> indexes = new List<int>();

            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);

                if (index == -1)
                    return indexes;

                indexes.Add(index);
            }
        }

        private static List<string> FindText(string text, IEnumerable<string> words, int threshold)
        {
            List<string> matchs = new List<string>();
            foreach (string word in words)
            {
                int distance = ComputeDistance(word, text);
                Console.WriteLine($"[ {word} : {text} ] distanse = {distance}");

                if (distance <= threshold)
                {
                    matchs.Add(word);
                }
            }
            return matchs;
        }

        private static int ComputeDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];
            // Verify arguments.
            if (n == 0)
            {
                return m;
            }
            if (m == 0)
            {
                return n;
            }
            // Initialize arrays.
            for (int i = 0; i <= n; d[i, 0] = i++) { }
            for (int j = 0; j <= m; d[0, j] = j++) { }
            // Begin looping.
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    // Compute cost.
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
                }
            }
            // Return cost.
            return d[n, m];
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            var sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '\'' || c == ' ')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
