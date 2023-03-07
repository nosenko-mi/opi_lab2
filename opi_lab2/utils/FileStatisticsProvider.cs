using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace opi_lab2.utils
{

    public class FileStatisticsProvider
    {
        public string Text { get; set; }

        public FileStatisticsProvider(string text)
        {
            Text = text;
        }

        public void FullStatistics()
        {

        }

        private void FullStatisticsOptimized()
        {

        }

        public float SizeKBytes()
        {
            return Text.Length * sizeof(char) / 1024;
        }

        public int Characters()
        {
            return Text.Length;
        }

        public int Paragraphs()
        {
            // https://stackoverflow.com/questions/23987032/how-to-count-the-number-of-paragraphs-in-a-string-in-c-sharp
            string pattern = @"[^\r\n]+((\r|\n|\r\n)[^\r\n]+)*";
            return Regex.Matches(Text, pattern).Count;
        }

        public int EmptyLines()
        {
            return Text.Split('\n').Count(line => string.IsNullOrWhiteSpace(line));
        }

        public int AuthorPages()
        {
            return Text.Length / 1800;
        }

        public int Vowels()
        {
            int total = 0;
            //var vowels = new HashSet<char> { 'a', 'e', 'i', 'o', 'u', 'у', 'е', 'а', 'о', 'я', 'и', 'ю', 'і', 'ы'};
            var vowels = "aeiouyеаояиюіыAEIOUYЕАОЯИЮІ";
            for (int i = 0; i < Text.Length; i++)
            {
                //if (vowels.Contains(Text[i]))

                if (vowels.IndexOf(Text[i]) >= 0)
                {
                    total++;
                }
            }

            return total;
        }
        public int Consonants()
        {
            int total = 0;
            var consonants = new HashSet<char>() {
                'б', 'в', 'г', 'ґ', 'д', 'ж', 'з', 'й', 'к', 'л', 'м', 'н', 'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'Ґ', 'Б', 'В', 'Г', 'Д', 'Ж', 'З', 'Й', 'К', 'Л', 'М', 'Н', 'П', 'Р', 'С', 'Т', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ',
                'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'y', 'z', 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Y', 'Z' };

            for (int i = 0; i < Text.Length; i++)
            {
                // invert if 
                if (consonants.Contains(Text[i]))
                {
                    total++;
                }
            }

            return total;
        }
        public int Digits()
        {
            return Text.Count(char.IsDigit);
        }
        public int SpecialDigits()
        {
            return Text.Count(char.IsSymbol);
        }

        public int Puncuations()
        {
            return Text.Count(char.IsPunctuation);
        }

        public int Latin()
        {
            string pattern = @"[a-zA-Z]";
            return Regex.Matches(Text, pattern).Count;
        }
        public int Cyrillic()
        {
            string pattern = @"\p{IsCyrillic}";
            return Regex.Matches(Text, pattern).Count;
        }

    }
}

//переглядати його статистику
//(розмір у кілобайтах, +
//кількість символів, +
//абзаців, +
//порожніх рядків,
//авторських сторінок (1 авторська сторінка = 1800 символів), +
//кількість голосних та приголосних для текстів кирилицею або латиницею , +
//кількість цифр, +
//спеціальних символів, +
//знаків пунктуації, +
//кириличних та латинських літер), +
//а також видаляти всі незначні прогалини (зайві прогалини, табуляції та порожні рядки).
//Результат видалення повинен відображатися поруч із вихідним текстом (двома колонками),
//користувач повинен сам вирішувати чи зберігати результат обробки тексту.
