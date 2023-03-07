using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using opi_lab2.utils;

namespace opi_lab2_test
{
    [TestClass]
    public class FileStatisticsTest
    {
        private static string str = "\t\t1234567890\n\nlatin\n\n\n\n\tКирилиця\n!,;";
        private FileStatisticsProvider statistics = new FileStatisticsProvider(str);

        [TestMethod]
        public void TestLatin()
        {
            // Arrange

            // Act
            int actual = statistics.Latin();

            // Assert
            int expected = 5;
            Assert.AreEqual(expected, actual, $"Latin amount is not correct: expected {expected} actual{actual}");
        }

        [TestMethod]
        public void TestCyrillic()
        {
            // Arrange

            // Act
            int actual = statistics.Cyrillic();

            // Assert
            int expected = 8;
            Assert.AreEqual(expected, actual, $"Cyrillic amount is not correct: expected {expected} actual{actual}");
        }

        [TestMethod]
        public void TestEmptyLines()
        {
            int actual = statistics.EmptyLines();
            int expected = 4;
            Assert.AreEqual(expected, actual, $"Empty lines amount is not correct: expected {expected} actual {actual}");

        }

        [TestMethod]
        public void TestVowels()
        {
            int actual = statistics.Vowels();
            int expected = 6;
            Assert.AreEqual(expected, actual, $"Vowels amount is not correct: expected {expected} actual {actual}");
        }

        [TestMethod]
        public void TestConsonants()
        {
            int actual = statistics.Consonants();
            int expected = 7;
            Assert.AreEqual(expected, actual, $"Consonants amount is not correct: expected {expected} actual {actual}");
        }

        [TestMethod]
        public void TestParagraphs()
        {
            // Arrange

            // Act
            int actual = statistics.Paragraphs();

            // Assert
            int expected = 3;
            Assert.AreEqual(expected, actual, $"Paragraphs amount is not correct: expected {expected} actual {actual}");
        }
    }
}
