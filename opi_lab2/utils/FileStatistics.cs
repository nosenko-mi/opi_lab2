using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opi_lab2.utils
{
    public struct FileStatistics
    {
        int sizeKb; int characters; int paragraphs; int emptyLines; int authorPages; int vowels; int consonants; int digits; int specialDigits; int punctuations; int latin; int cyrillic;

        public FileStatistics(int sizeKb, int characters, int paragraphs, int emptyLines, int authorPages, int vowels, int consonants, int digits, int specialDigits, int punctuations, int latin, int cyrillic)
        {
            this.sizeKb = sizeKb;
            this.characters = characters;
            this.paragraphs = paragraphs;
            this.emptyLines = emptyLines;
            this.authorPages = authorPages;
            this.vowels = vowels;
            this.consonants = consonants;
            this.digits = digits;
            this.specialDigits = specialDigits;
            this.punctuations = punctuations;
            this.latin = latin;
            this.cyrillic = cyrillic;
        }

        public override string ToString()
        {
            return $"Size(Kb) = {sizeKb}; Characters = {characters}; Paragraphs = {paragraphs}; Empty lines = {emptyLines}; Autor pages = {authorPages}; Vowels = {vowels}; Consonants = {consonants}; " +
                $"Digits = {digits}; Special digits = {specialDigits}; Punctuations = {punctuations}; Latin = {latin}; Cyrillic = {cyrillic}";
        }
    }
}
