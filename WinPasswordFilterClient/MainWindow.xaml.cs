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
        private const string _settingsFile = @"C:\WinPasswordFilter\settings.txt";
        
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

        }

        private void editExactDictionaryButton_Click(object sender, RoutedEventArgs e)
        {

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
            //TODO: open file dialog and then call readsettings
        }

        private void saveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //TODO: save file dialog and then call writesettings
        }

        private void partialMatchMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void exactMatchMenuItem_Click(object sender, RoutedEventArgs e)
        {

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


    }
}
