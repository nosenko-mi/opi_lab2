using Microsoft.Win32;
using opi_lab2.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace opi_lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string NEW_FILE_DEFAULT_NAME = "New File";
        private static readonly Encoding ENCODING_UTF8 = Encoding.UTF8;
        private static readonly Encoding ENCODING_DEFAULT = Encoding.Default;

        private Encoding _currentEncoding = ENCODING_DEFAULT;
        private string _currentFile = NEW_FILE_DEFAULT_NAME;
        private bool _isFileChanged = false;
 
        public MainWindow()
        {
            InitializeComponent();
            RenderStatusBar();
        }

        private void RenderStatusBar()
        {
            textEncodingLabel.Text = _currentEncoding.EncodingName.ToString();
        }

        private void NewFile()
        {
            contentTextBox.Document.Blocks.Clear();
            _currentFile = NEW_FILE_DEFAULT_NAME;
            _isFileChanged = false;
            Title = _currentFile;
        }

        private void SaveFileAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                WriteFile(saveFileDialog.FileName);
            }
        }

        private void SaveFile()
        {
            if (_currentFile.Equals(NEW_FILE_DEFAULT_NAME) || _currentFile == null)
            {
                SaveFileAs();
            }
            else
            {
                WriteFile(_currentFile);
            }
        }

        private void WriteFile(string path)
        {
            string text = new TextRange(contentTextBox.Document.ContentStart, contentTextBox.Document.ContentEnd).Text;

            File.WriteAllText(path, text);

            _currentFile = path;
            Title = _currentFile;
            _isFileChanged = false;
        }

        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //string text = File.ReadAllText(openFileDialog.FileName);

                string text = "";
                using (StreamReader reader = new StreamReader(openFileDialog.FileName, _currentEncoding))
                {
                    //if (reader.Peek() >= 0) // you need this!
                    //    reader.Read();
                    //Encoding currentEncoding = reader.CurrentEncoding;

                    text = reader.ReadToEnd();
                }

                contentTextBox.Document.Blocks.Clear();
                contentTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));

                _currentFile = openFileDialog.FileName;
                _isFileChanged = false;
                Title = _currentFile;
            }
        }

        // Menu item listeners.

        private void OpenFileClick(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void CreateFileClick(object sender, RoutedEventArgs e)
        {
            NewFile();
        }

        private void SaveFileClick(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private void SaveFileAsClick(object sender, RoutedEventArgs e)
        {
            SaveFileAs();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            // save file
            if (_isFileChanged)
            {
                if (MessageBox.Show("Save changes?", "Text is changed", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    SaveFile();
                }
            }
            // exit
            Close();
        }

        private void GetStatisticsClick(object sender, RoutedEventArgs e)
        {
            FileStatisticsProvider statistics = new FileStatisticsProvider(StringFromRichTextBox(contentTextBox));

            int vowels = statistics.Vowels();
            int consonants = statistics.Consonants();
            int digits = statistics.Digits();
            int special = statistics.SpecialDigits();
            int punctuation = statistics.Puncuations();
            int author = statistics.AuthorPages();
            int latin = statistics.Latin();
            int cyrillic = statistics.Cyrillic();

            MessageBox.Show($"vowels: {vowels}; consonants: {consonants};\ndigits: {digits}; special: {special}; punctuation: {punctuation}; author: {author};\nLatin: {latin}; Cyrillyc: {cyrillic}");
        }

        private void txtEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
        }

        private void ChangeEncodingToUtf8Click(object sender, RoutedEventArgs e)
        {
            _currentEncoding = ENCODING_UTF8;
            RenderStatusBar();
        }

        private void ChangeEncodingToCyrrilicClick(object sender, RoutedEventArgs e)
        {
            _currentEncoding = ENCODING_DEFAULT;
            RenderStatusBar();
        }

        private void contentTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isFileChanged)
            {
                _isFileChanged = true;
                Title += "*";
            }
        }

        // Utility.

        private string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                // TextPointer to the start of content in the RichTextBox.
                rtb.Document.ContentStart,
                // TextPointer to the end of content in the RichTextBox.
                rtb.Document.ContentEnd
            );

            // The Text property on a TextRange object returns a string
            // representing the plain text content of the TextRange.
            return textRange.Text;
        }
    }
}
