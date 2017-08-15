// TesterProject.cpp : Defines the entry point for the console application.
#include "stdafx.h"
#include <stdio.h>
#include <string>
#include <atlbase.h>
#include <fstream>
#include <iostream>
#include <Windows.h>
#include <time.h>
#include <cwctype>
#include <algorithm>

using namespace std;

const wstring Root = L"C:\\WinPasswordFilter\\";
const wstring LogFile = Root + L"FilterLog.txt";
const wstring DictionaryFile = Root + L"Dictionary.txt";
const wstring PartialDictionaryFile = Root + L"PartialDictionary.txt";
const wstring SettingsFile = Root + L"settings.wpf";

bool filterEnabled = true;
bool logEnabled = true;
bool partialEnabled = false;
bool exactEnabled = false;
bool userDataEnabled = false;

int minChars = 0;
int maxChars = 30000;
int lowercases = 0;
int uppercases = 0;
int numbers = 0;
int specials = 0;

// This is a helper function to write log lines to file
void WriteToLog(string str){

	if (str.empty() || !logEnabled)
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

void ReadSettings() {
	// TODO: try, if there is no file all can pass
	ifstream settings(SettingsFile);
	string line;
	try
	{
		if (settings.is_open()) {
			getline(settings, line);
			filterEnabled = line == "1";
			getline(settings, line);
			logEnabled = line == "1";
			getline(settings, line);
			partialEnabled = line == "1";
			getline(settings, line);
			exactEnabled = line == "1";
			getline(settings, line);
			userDataEnabled = line == "1";
			getline(settings, line);
			minChars = stoi(line);
			getline(settings, line);
			maxChars = stoi(line);
			maxChars = maxChars == 0 ? 30000 : maxChars;
			getline(settings, line);
			lowercases = stoi(line);
			getline(settings, line);
			uppercases = stoi(line);
			getline(settings, line);
			numbers = stoi(line);
			getline(settings, line);
			specials = stoi(line);

			settings.close();
		}
	}
	catch (const std::exception&)
	{
		return;
	}

}

// Checks if the password exists in the dictionary
// TODO: maybe invert true/false?
// TODO: optimize file search
// TODO: clear password from memory!!! securezeromemory
BOOLEAN DictionaryCheck(wstring pass) {

	wstring line;
	wifstream dict(DictionaryFile);

	//transform to lowercase for case insensitivity
	transform(pass.begin(), pass.end(), pass.begin(), towlower);

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
BOOLEAN PartialCheck(wstring pass) {

	wstring line;
	wifstream dict(PartialDictionaryFile);

	//transform to lowercase for case insensitivity
	transform(pass.begin(), pass.end(), pass.begin(), towlower);

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

// TODO: watch for unicode?
BOOLEAN UserDataCheck(wstring accName, wstring fullName, wstring pwd) {

	// transform to lowercase for case insensitivity
	transform(accName.begin(), accName.end(), accName.begin(), towlower);
	transform(fullName.begin(), fullName.end(), fullName.begin(), towlower);
	transform(pwd.begin(), pwd.end(), pwd.begin(), towlower);

	if (accName != L"" && pwd.find(accName) != string::npos) {
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

	if (fullName != L"" && pwd.find(fullName) != string::npos) {
		return FALSE;
	}
	

	return TRUE;
}

BOOLEAN __stdcall PasswordFilter( 
	PUNICODE_STRING AccountName,
	PUNICODE_STRING FullName,
	PUNICODE_STRING Password,
	BOOLEAN SetOperation
)
{

	WriteToLog("Entering PasswordFilter()");

	ReadSettings();

	if (!filterEnabled) {
		return true;
	}

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
		wszPassword[Password->Length / sizeof(wchar_t)] = 0;
		wstrPassword = wszPassword;


		wszAccountName = new wchar_t[AccountName->Length + 1];
		if (NULL == wszAccountName)
		{
			throw E_OUTOFMEMORY;
		}
		wcsncpy(wszAccountName, AccountName->Buffer, AccountName->Length);
		wszAccountName[AccountName->Length / sizeof(wchar_t)] = 0;
		wstrAccountName = wszAccountName;


		wszFullName = new wchar_t[FullName->Length + 1];
		if (NULL == wszFullName)
		{
			throw E_OUTOFMEMORY;
		}
		wcsncpy(wszFullName, FullName->Buffer, FullName->Length);
		wszFullName[FullName->Length / sizeof(wchar_t)] = 0;
		wstrFullName = wszFullName;

		// Count characters
		int pLowercases = 0;
		int pUppercases = 0;
		int pNumbers = 0;
		int pSpecials = 0;
		int pTotal = 0;
		int i = 0;

		while (wstrPassword[i] != L'\0') {
			if (isupper(wstrPassword[i]))
				pUppercases++;
			else if (islower(wstrPassword[i]))
				pLowercases++;
			else if (isdigit(wstrPassword[i]))
				pNumbers++;
			else
				pSpecials++;

			pTotal++;
			i++;
		}

		// Perform checks
		WriteToLog("Number of characters check: " + to_string(wstrPassword.length()) + " " + to_string(pTotal));
		if (pTotal < minChars || pTotal > maxChars || pNumbers < numbers || pUppercases < uppercases || 
			pLowercases < lowercases || pSpecials < specials) {
			
			retVal = FALSE;
		}

		if (retVal && userDataEnabled){
			WriteToLog("User data check");
			retVal = UserDataCheck(wstrAccountName, wstrFullName, wstrPassword);
		}

		if (retVal && partialEnabled) {
			WriteToLog("Partial check");
			retVal = PartialCheck(wstrPassword);
		}

		if (retVal && exactEnabled) {
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
	WCHAR anBuffer[40] = L"Vuletic";
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

