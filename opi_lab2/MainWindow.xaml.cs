using Microsoft.Win32;
using opi_lab2.utils;
using opi_lab2.utils.file;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Linq;

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

        public FileStatisticsProvider FileStatisticsProvider
        {
            get => default;
            set
            {
            }
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

            File.WriteAllText(path, text, _currentEncoding);

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
            if (_isFileChanged)
            {
                if (MessageBox.Show("Save changes?", "Text is changed", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    SaveFile();
                }
            }
            Close();
        }

        private void GetStatisticsClick(object sender, RoutedEventArgs e)
        {
            FileStatisticsProvider statistics = new FileStatisticsProvider(StringFromRichTextBox(contentTextBox));
            MessageBox.Show(statistics.FullStatistics().ToString(), "Statistics") ;
        }
        
        private void FormatTextClick(object sender, RoutedEventArgs e)
        {
            RenderNewContentTextBox();
            // format text
            string formattedText = FileFormatter.FormatToPlainText(StringFromRichTextBox(contentTextBox));

            StringToRichTextBox(formattedText, ref newContentTextBox);
        }

        private void AcceptFormatChangesClick(object sender, RoutedEventArgs e)
        {
            HideNewContentTextBox();
            StringToRichTextBox(StringFromRichTextBox(newContentTextBox), ref contentTextBox);
            SaveFileAs();
        }

        private void DeclineFormatChangesClick(object sender, RoutedEventArgs e)
        {
            HideNewContentTextBox();
        }

        private void FindClick(object sender, RoutedEventArgs e)
        {
            // render ui
            RenderFindContentTextBox();
        }

        private void FindButtonClick(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(
                            contentTextBox.Document.ContentStart,
                            contentTextBox.Document.ContentEnd);

            textRange.ClearAllProperties();
            string textBoxText = textRange.Text;
            string searchText = StringFromRichTextBox(newContentTextBox);
            if (searchText.Contains("\r\n"))
                searchText = searchText.Replace("\r\n", "");

            if (string.IsNullOrWhiteSpace(textBoxText) || string.IsNullOrWhiteSpace(searchText))
            {
                MessageBox.Show("Please provide search text or source text to search from");
                return;
            }
            Dictionary<int, string> lines = FileLookup.FindTextStrictLines(ref contentTextBox, ref newContentTextBox);
            string asString = "";
            foreach (KeyValuePair<int, string> kvp in lines)
            {
                asString += String.Format("[{0}]: {1}", kvp.Key, kvp.Value);
            }
            if (lines.Count > 0)
            {
                MessageBox.Show(asString);
            }
            else
            {
                MessageBox.Show("No Match found");
            }
        }

        private void FindComplexNumbersClick(object sender, RoutedEventArgs e)
        {
            List<TextRange> ranges = FileLookup.FindComplexNumbers(ref contentTextBox);
            foreach (TextRange range in ranges)
            {
                range.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.Coral));
            }
            MessageBox.Show("Total Match Found : " + ranges.Count);
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

        private void StringToRichTextBox(string text, ref RichTextBox richTextBox)
        {
            richTextBox.Document.Blocks.Clear();
            richTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));
        }

        private void RenderNewContentTextBox()
        {
            contentTextBox.SetValue(Grid.ColumnSpanProperty, 1);
            newContentGrid.Visibility = Visibility.Visible;
            findButton.Visibility = Visibility.Collapsed;
            acceptButton.Visibility = Visibility.Visible;
        }

        private void RenderFindContentTextBox()
        {
            contentTextBox.SetValue(Grid.ColumnSpanProperty, 1);
            newContentGrid.Visibility = Visibility.Visible;
            findButton.Visibility = Visibility.Visible;
            acceptButton.Visibility = Visibility.Collapsed;
        }

        private void HideNewContentTextBox()
        {
            contentTextBox.SetValue(Grid.ColumnSpanProperty, 2);
            newContentGrid.Visibility = Visibility.Hidden;
        }

    }
}
