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

#define LOGFILE "C:\\WinPasswordFilter\\FilterLog.txt"
#define DICTIONARYFILE "C:\\WinPasswordFilter\\Dictionary.txt"

using namespace std;

// This is a helper function to write log lines to file
void WriteToLog(const char* str)
{
	if (NULL == str)
	{
		return;
	}
	FILE* log;
	log = fopen(LOGFILE, "a+");
	if (NULL == log)
	{
		return;
	}
	fprintf(log, "%s\r\n", str);
	fclose(log);

	return;
}

// Checks if the password exists in the dictionary
// TODO: maybe invert true/false?
// TODO: optimize file search
BOOLEAN DictionaryCheck(wstring pass) {

	wstring line;
	wifstream dict(DICTIONARYFILE);

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

	try
	{
		wszPassword = new wchar_t[Password->Length + 1];
		if (NULL == wszPassword)
		{
			throw E_OUTOFMEMORY;
		}
		wcsncpy(wszPassword, Password->Buffer, Password->Length);
		wszPassword[Password->Length] = 0;

		WriteToLog("Going to check password");
		wstrPassword = wszPassword;

		retVal = DictionaryCheck(wstrPassword);


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
	wstrPassword.replace(0, wstrPassword.length(), wstrPassword.length(), (wchar_t)'?');
	wstrPassword.erase();
	if (NULL != wszPassword)
	{
		ZeroMemory(wszPassword, Password->Length); // TODO: securezeromemory?

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

	WCHAR stringBuffer[20] = L"koliko123";
	UNICODE_STRING  string;


	string.Buffer = stringBuffer;
	string.Length = 20;
	string.MaximumLength = sizeof(stringBuffer);

	PasswordFilter(&string, &string, &string, TRUE);

	while(true){

	}

    return 0;
}

