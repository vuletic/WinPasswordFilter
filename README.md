# WinPasswordFilter
WinPasswordFilter is a custom password filter dll for Windows that comes with a client application for setting a password policy. The desired password complexity can be configured in the following ways:
+ Set minimum number of characters
+ Set maximum number of characters
+ Set minimum number of uppercase/lowercase/digit/special characters
+ Enable/Disable user data in password (Windows account name and full name)
+ Exact match dictionary
+ Partial match dictionary

There are two dictionaries that can be edited from within the application or by manually editing the text files. The dll will check if the given password **exactly matches** any of the entries in the exact match dictionary, or if it **contains** any entry from the partial match dictionary. Note that they are two seperate dictionaries. The matching for both dictionaries is case insensetive.

The dictionaries can be imported and exported. Password policies can also be saved and loaded. Logging of the dll's work can be enabled and disabled.

## Installation
### Prerequisites
+ [.NET Framework 4.5.2](https://www.microsoft.com/en-us/download/details.aspx?id=42642)
+ [Visual C++ 2015 Redistributable](https://www.microsoft.com/en-us/download/details.aspx?id=48145)
### Procedure
1. Run installer ([x86](https://ufile.io/brdii) for 32-bit Windows, and [x64](https://ufile.io/paeeg) for 64-bit)
2. Run regfilter.bat (from the installation folder) **as administrator**
3. Restart your computer  

## Author
David Vuletić - [vuletic](https://github.com/vuletic) <br/>
vuletic93@gmail.com

## License
This project is licensed under the terms of the MIT License.
