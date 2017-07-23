using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace WinPasswordFilterClient
{
    /// <summary>
    /// Interaction logic for DictionaryWindow.xaml
    /// </summary>
    public partial class DictionaryWindow : Window
    {

        private string dictFile;

        public DictionaryWindow(string title, string filePath)
        {
            InitializeComponent();
            this.Title = title;
            dictFile = filePath;
            if (!File.Exists(dictFile))
            {
                File.Create(dictFile).Dispose();
            }

            pwdValue.Focus();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void openDictionaryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(dictFile);
            }
            catch (Exception)
            {
                File.Create(dictFile).Dispose();
                Process.Start(dictFile);
            }
        }

        private void exportButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = "dict.txt";
            savefile.Filter = "TXT files|*.txt";

            if ((bool)savefile.ShowDialog())
            {
                File.Copy(dictFile, savefile.FileName, true);
            }
        }

        private void importButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open File";
            theDialog.Filter = "TXT files|*.txt";
            theDialog.InitialDirectory = @"C:\";

            if ((bool)theDialog.ShowDialog())
            {
                File.Copy(theDialog.FileName, dictFile, true);
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter w = File.AppendText(dictFile))
            {
                w.WriteLine(pwdValue.Text.ToLowerInvariant());
                pwdValue.Text = "";
                pwdValue.Focus();
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllLines(dictFile,  File.ReadLines(dictFile).Where(l => l != pwdValue.Text.ToLowerInvariant()).ToList());
            pwdValue.Text = "";
            pwdValue.Focus();
        }
    }
}
