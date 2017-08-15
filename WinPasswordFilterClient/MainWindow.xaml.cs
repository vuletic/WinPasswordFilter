using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
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
        private const string _directory = @"C:\WinPasswordFilter";

        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.KeyDown += HandleKeyPress;
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
            this.Close();
        }

        private void loadMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void saveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Save();
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
            AboutWindow about = new AboutWindow();
            about.Owner = this;
            about.ShowDialog();
        }

        private void readSettings(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            
            enableFilterValue.IsChecked = lines[0].Equals("1");
            enableLoggingValue.IsChecked = lines[1].Equals("1");
            partialMatchDictionaryValue.IsChecked = lines[2].Equals("1");
            exactMatchDictionaryValue.IsChecked = lines[3].Equals("1");
            userDataValue.IsChecked = lines[4].Equals("1");
            minCharactersValue.Value = Int32.Parse(lines[5]);
            maxCharactersValue.Value = Int32.Parse(lines[6]);
            lowercasesValue.Value = Int32.Parse(lines[7]);
            uppercasesValue.Value = Int32.Parse(lines[8]);
            digitsValue.Value = Int32.Parse(lines[9]);
            specialsValue.Value = Int32.Parse(lines[10]);
        }

        private void writeSettings(string path)
        {
            string lines = "";

            lines += (bool)enableFilterValue.IsChecked ? "1\r\n" : "0\r\n";
            lines += (bool)enableLoggingValue.IsChecked ? "1\r\n" : "0\r\n";
            lines += (bool)partialMatchDictionaryValue.IsChecked ? "1\r\n" : "0\r\n";
            lines += (bool)exactMatchDictionaryValue.IsChecked ? "1\r\n" : "0\r\n";
            lines += (bool)userDataValue.IsChecked ? "1\r\n" : "0\r\n";
            lines += minCharactersValue.Value + "\r\n";
            lines += maxCharactersValue.Value + "\r\n";
            lines += lowercasesValue.Value + "\r\n";
            lines += uppercasesValue.Value + "\r\n";
            lines += digitsValue.Value + "\r\n";
            lines += specialsValue.Value + "\r\n";

            FileSystemAccessRule administratorRule = new FileSystemAccessRule("Administrators", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);

            FileSystemAccessRule guestRule = new FileSystemAccessRule("Guest", FileSystemRights.CreateDirectories | FileSystemRights.CreateFiles, AccessControlType.Allow);

            DirectorySecurity dirSec = new DirectorySecurity();
            dirSec.AddAccessRule(administratorRule);
            dirSec.AddAccessRule(guestRule);

            Directory.CreateDirectory(_directory, dirSec);

            System.IO.FileInfo file = new System.IO.FileInfo(path);
            file.Directory.Create();
            System.IO.File.WriteAllText(file.FullName, lines);
            
        }
        //TODO: are you sure you want to leave without saving settings
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want to exit the application?", "Exit Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.No)
            {
                e.Cancel = true;
            }

        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if(e.Key == Key.S)
                {
                    Save();
                }
                if(e.Key == Key.O)
                {
                    Load();
                }
               
            }
        }

        private void Save()
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = "save1.wpf";
            savefile.Filter = "WPF files|*.wpf";

            if ((bool)savefile.ShowDialog())
            {
                writeSettings(savefile.FileName);
            }
        }

        private void Load()
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


    }
}
