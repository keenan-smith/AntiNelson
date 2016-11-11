#include "Injector.h"

void removeFile()
{

	char tempPath[MAX_PATH * 4];

	GetTempPathA(MAX_PATH * 4, tempPath);
	sprintf_s(tempPath, "%s-", tempPath);
	std::remove(tempPath);

	GetTempPathA(MAX_PATH * 4, tempPath);
	sprintf_s(tempPath, "%s.-", tempPath);
	std::remove(tempPath);

	GetTempPathA(MAX_PATH * 4, tempPath);
	sprintf_s(tempPath, "%sEAC1.tmp", tempPath);
	std::remove(tempPath);

	GetTempPathA(MAX_PATH * 4, tempPath);
	sprintf_s(tempPath, "%sEAC2.tmp", tempPath);
	std::remove(tempPath);

}

BOOL IsElevated() {

	BOOL fRet = FALSE;
	HANDLE hToken = NULL;

	if (OpenProcessToken(GetCurrentProcess(), TOKEN_QUERY, &hToken)) {

		TOKEN_ELEVATION Elevation;
		DWORD cbSize = sizeof(TOKEN_ELEVATION);

		if (GetTokenInformation(hToken, TokenElevation, &Elevation, sizeof(Elevation), &cbSize))
			fRet = Elevation.TokenIsElevated;

	}

	if (hToken)
		CloseHandle(hToken);

	return fRet;

}

int main(int argc, char * args[])
{

	std::atexit(removeFile);

	if (!IsElevated())
	{

		printf("You need to run this program as admin!");
		std::cin.ignore();

		return 0;

	}

	HRSRC rSrc = FindResource(NULL, MAKEINTRESOURCE(IDR_RCDATA1), RT_RCDATA);
	unsigned int rSize = SizeofResource(NULL, rSrc);
	HGLOBAL rGlobal = LoadResource(NULL, rSrc);
	byte* rData = (byte*)LockResource(rGlobal);

	char tempPath[MAX_PATH * 4];
	GetTempPathA(MAX_PATH * 4, tempPath);
	sprintf_s(tempPath, "%s-", tempPath);

	HANDLE hFile = CreateFile(tempPath, GENERIC_WRITE | GENERIC_READ, 0, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
	ULONG bytesWritten = 0;
	WriteFile(hFile, rData, rSize, &bytesWritten, NULL);
	CloseHandle(hFile);

	PROCESS_INFORMATION pi;
	STARTUPINFO si = { sizeof si };
	printf_s("Started: %d\n", CreateProcess(tempPath, NULL, NULL, NULL, FALSE, 0, NULL, NULL, &si, &pi));

	DWORD exitCode;
	while (GetExitCodeProcess(pi.hProcess, &exitCode)) {
		
		Sleep(100);

		if (exitCode != STILL_ACTIVE)
			break;

	}

}

