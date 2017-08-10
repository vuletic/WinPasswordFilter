using System;
using System.Collections.Generic;
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
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            DescriptionBox.Text = "WinPasswordFilter is a custom password filter dll for Windows that comes with a client application for setting a password policy. The desired password complexity can be configured in the following ways: \n\n";
            DescriptionBox.Text += " - Set minimum number of characters\n - Set maximum number of characters\n - Set minimum number for types of characters\n - Enable/Disable user data in password \n";
            DescriptionBox.Text += " - Exact match dictionary\n - Partial match dictionary\n\nThere are two dictionaries that can be edited from within the application or by manually editing the text files. The dll will check if the given password exactly matches any of the entries in the exact match dictionary, or if it contains any entry from the partial match dictionary. Note that they are two seperate dictionaries. The matching for both dictionaries is case insensetive. \n\nThe dictionaries can be imported and exported. Password policies can also be saved and loaded. Logging of the dll's work can be enabled and disabled.";
        }
    }
}
