using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

namespace WinPasswordFilterClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _settingsFile = @"C:\WinPasswordFilter\settings.wpf";
        private const string _partialDictFile = @"C:\WinPasswordFilter\partialDictionary.txt";
        private const string _exactDictFile = @"C:\WinPasswordFilter\dictionary.txt";

        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            try
            {
                readSettings(_settingsFile);
            }
            catch (Exception)
            {
                writeSettings(_settingsFile);
            }
        }

        private void editPartialDictionaryButton_Click(object sender, RoutedEventArgs e)
        {
            DictionaryWindow pmdw = new DictionaryWindow("Partial match dictionary", _partialDictFile);
            pmdw.Owner = this;
            pmdw.ShowDialog();
        }

        private void editExactDictionaryButton_Click(object sender, RoutedEventArgs e)
        {
            DictionaryWindow emdw = new DictionaryWindow("Exact match dictionary", _exactDictFile);
            emdw.Owner = this;
            emdw.ShowDialog();
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                writeSettings(_settingsFile);
                MessageBox.Show("Successfully applied settings.");
            }
            catch (Exception)
            {
                MessageBox.Show("There was an error while applying settings.");
            }
        }

        private void closeMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void loadMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open File";
            theDialog.Filter = "WPF files|*.wpf";
            theDialog.InitialDirectory = @"C:\";

            if ((bool)theDialog.ShowDialog())
            {
                try
                {
                    readSettings(theDialog.FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show("Save file corrupted");
                }
            }
        }

        private void saveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = "save1.wpf";
            savefile.Filter = "WPF files|*.wpf";

            if ((bool)savefile.ShowDialog())
            {
                writeSettings(savefile.FileName);
            }
        }

        private void partialMatchMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DictionaryWindow pmdw = new DictionaryWindow("Partial match dictionary", _partialDictFile);
            pmdw.Owner = this;
            pmdw.ShowDialog();
        }

        private void exactMatchMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DictionaryWindow emdw = new DictionaryWindow("Exact match dictionary", _exactDictFile);
            emdw.Owner = this;
            emdw.ShowDialog();
        }

        private void aboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // TODO: info about project, author, description
        }

        private void readSettings(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            
            partialMatchDictionaryValue.IsChecked = lines[0].Equals("1");
            exactMatchDictionaryValue.IsChecked = lines[1].Equals("1");
            userDataValue.IsChecked = lines[2].Equals("1");
            minCharactersValue.Value = Int32.Parse(lines[3]);
            maxCharactersValue.Value = Int32.Parse(lines[4]);
        }

        private void writeSettings(string path)
        {
            string lines = "";

            lines += (bool)partialMatchDictionaryValue.IsChecked ? "1\r\n" : "0\r\n";
            lines += (bool)exactMatchDictionaryValue.IsChecked ? "1\r\n" : "0\r\n";
            lines += (bool)userDataValue.IsChecked ? "1\r\n" : "0\r\n";
            lines += minCharactersValue.Value + "\r\n";
            lines += maxCharactersValue.Value + "\r\n";

            System.IO.FileInfo file = new System.IO.FileInfo(path);
            file.Directory.Create();
            System.IO.File.WriteAllText(file.FullName, lines);
            
        }

        //TODO: are you sure you want to leave without saving settings


    }
}
