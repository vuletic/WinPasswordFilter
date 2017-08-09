# WinPasswordFilter
WinPasswordFilter is a custom password filter dll for Windows that comes with a client application for setting a password policy. The desired password complexity can be configured in many ways:
+ Set minimum number of characters
+ Set maximum number of characters
+ Set minimum number of uppercase/lowercase/digit/special characters
+ Enable/Disable user data in password (Windows account name and full name)
+ Exact match dictionary
+ Partial match dictionary

There are two dictionaries that can be edited from within the application or by manually editing the text files. The dll will check if the given password **exactly matches** any of the entries in the exact match dictionary, or if it **contains** any entry from the partial match dictionary. Note that they are two seperate dictionaries. The matching for both dictionaries is case insensetive.

The dictionaries can be imported and exported. Password policies can also be saved and loaded. Logging of the dll's work can be enabled and disabled.

## Installation
x64 and x86

## Author
David Vuletić - [vuletic](https://github.com/vuletic) <br/>
vuletic93@gmail.com

## License
This project is licensed under the terms of the MIT License.
