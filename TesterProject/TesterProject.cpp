// TesterProject.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <stdio.h>
#include <string>
#include <atlbase.h>
#include <fstream>
#include <iostream>
#include <Windows.h>
#include <time.h>

using namespace std;

const wstring Root = L"C:\\WinPasswordFilter\\";
const wstring LogFile = Root + L"FilterLog.txt";
const wstring DictionaryFile = Root + L"Dictionary.txt";
const wstring PartialDictionaryFile = Root + L"PartialDictionary.txt";


// This is a helper function to write log lines to file
void WriteToLog(string str){

	if (str.empty())
	{
		return;
	}

	// If directory exists or is successfully created
	if (CreateDirectory(Root.c_str(), NULL) || ERROR_ALREADY_EXISTS == GetLastError())
	{
		ofstream log(LogFile, ios::app);

		if (log.is_open())
		{
			log << str + "\r\n";
			log.close();
		}
	}

	return;
}

// Checks if the password exists in the dictionary
// TODO: maybe invert true/false?
// TODO: optimize file search
// TODO: clear password from memory!!! securezeromemory
BOOLEAN DictionaryCheck(wstring pass) {

	wstring line;
	wifstream dict(DictionaryFile);

	if (dict.is_open())
	{
		while (getline(dict, line))
		{
			if (line == pass) {
				dict.close();
				return FALSE;
			}
		}
		dict.close();
	}
	else { // RETURNS TRUE IF FILE CANNOT BE OPENED (does not exist)
		return TRUE;
	}

	return TRUE;

}

// check if password contains substring from dictionary
// TODO: make case insensetive?
BOOLEAN PartialCheck(wstring pass) {

	wstring line;
	wifstream dict(PartialDictionaryFile);

	if (dict.is_open())
	{
		while (getline(dict, line))
		{
			if (pass.find(line) != string::npos) {
				dict.close();
				return FALSE;
			}
		}
		dict.close();
	}
	else { // RETURNS TRUE IF FILE CANNOT BE OPENED (does not exist)
		return TRUE;
	}

	return TRUE;

}

// TODO: case insensetive
// TODO: watch for unicode
BOOLEAN UserDataCheck(wstring accName, wstring fullName, wstring pwd) {

	if (pwd.find(accName) != string::npos) {
		return FALSE;
	}

	wstring delimiter = L" ";

	size_t pos = 0;
	wstring token;
	
	// tokenize full name and check if it is contained in password
	while ((pos = fullName.find(delimiter)) != std::string::npos) {
		token = fullName.substr(0, pos);
		fullName.erase(0, pos + delimiter.length());
		if (pwd.find(token) != string::npos) {
			return FALSE;
		}
	}

	if (pwd.find(fullName) != string::npos) {
		return FALSE;
	}
	

	return TRUE;
}

// TODO: call checks based on settings 
BOOLEAN __stdcall PasswordFilter(
	PUNICODE_STRING AccountName,
	PUNICODE_STRING FullName,
	PUNICODE_STRING Password,
	BOOLEAN SetOperation
)
{

	WriteToLog("Entering PasswordFilter()");

	BOOLEAN retVal = true;

	wchar_t* wszPassword = NULL;
	wstring wstrPassword;

	wchar_t* wszAccountName = NULL;
	wstring wstrAccountName;

	wchar_t* wszFullName = NULL;
	wstring wstrFullName;

	try
	{
		// Convert Unicode Strings to wstring
		wszPassword = new wchar_t[Password->Length + 1];
		if (NULL == wszPassword)
		{
			throw E_OUTOFMEMORY;
		}
		wcsncpy(wszPassword, Password->Buffer, Password->Length);
		wszPassword[Password->Length] = 0;
		wstrPassword = wszPassword;


		wszAccountName = new wchar_t[AccountName->Length + 1];
		if (NULL == wszAccountName)
		{
			throw E_OUTOFMEMORY;
		}
		wcsncpy(wszAccountName, AccountName->Buffer, AccountName->Length);
		wszAccountName[AccountName->Length] = 0;
		wstrAccountName = wszAccountName;


		wszFullName = new wchar_t[FullName->Length + 1];
		if (NULL == wszFullName)
		{
			throw E_OUTOFMEMORY;
		}
		wcsncpy(wszFullName, FullName->Buffer, FullName->Length);
		wszFullName[FullName->Length] = 0;
		wstrFullName = wszFullName;



		// Perform checks
		WriteToLog("User data check");
		retVal = UserDataCheck(wstrAccountName, wstrFullName, wstrPassword);

		if (retVal) {
			WriteToLog("Partial check");
			retVal = PartialCheck(wstrPassword);
		}

		if (retVal) {
			WriteToLog("Full check");
			retVal = DictionaryCheck(wstrPassword);
		}

	}
	catch (HRESULT)
	{
	}
	catch (...)
	{
		WriteToLog("catch(...)");
	}



	// Erase all temporary password data
	// for security reasons
	wstrPassword.replace(0, wstrPassword.length(), wstrPassword.length(), (wchar_t)'?');  // TODO: same thing on copies in functions?
	wstrPassword.erase();
	if (NULL != wszPassword)
	{
		SecureZeroMemory(wszPassword, Password->Length);

												   // Assure that there is no compiler optimizations and read random byte
												   // from cleaned password string
		srand(time(NULL));
		wchar_t wch = wszPassword[rand() % Password->Length];
		delete[] wszPassword;
		wszPassword = NULL;
	}

	return retVal;

}


int main()
{

	WCHAR stringBuffer[30] = L"koliko123";

	WCHAR fnBuffer[40] = L"David Vuletic";
	WCHAR anBuffer[40] = L"bleja93";
	UNICODE_STRING  fn;
	UNICODE_STRING  an;

	fn.Buffer = fnBuffer;
	fn.Length = 40;
	fn.MaximumLength = sizeof(fnBuffer);
	an.Buffer = anBuffer;
	an.Length = 40;
	an.MaximumLength = sizeof(stringBuffer);

	UNICODE_STRING  string;
	wstring input;

	while(true){
		cout << "Password: ";
		wcin >> input;

		wcscpy(stringBuffer, input.c_str());

		string.Buffer = stringBuffer;
		string.Length = 40;
		string.MaximumLength = sizeof(stringBuffer);

		if (PasswordFilter(&an, &fn, &string, TRUE)) {
			cout << "ACCEPTED\n";
		}
		else {
			cout << "INVALID\n";
		}
		
	}

    return 0;
}
