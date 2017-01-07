#include "kCrypt.h"
#include "LoadDLL.h"

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
}

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

bool SetupCurrentDir()
{
	wchar_t buf[MAX_PATH];
	DWORD len = GetModuleFileNameW(NULL, buf, MAX_PATH);

	if (!len || len >= MAX_PATH)
		return false;

	wchar_t* p = buf + len;

	while (p>buf && p[-1] != L'\\' && p[-1] != '/')
		--p;

	while (p>buf && (p[-1] == L'\\' || p[-1] == '/'))
		--p;

	*p = 0;

	if (!SetCurrentDirectoryW(buf))
		return false;

	return true;
}

int main(int argc, char** args)
{
	/*
	size_t fLen;
	BYTE* patch = readFileBytes(getFilePath("Unturned_Data\\Managed\\UnityEngine.UI.TestDLL"), &fLen);
	std::string encoded = base64_encode(patch, fLen);
	setClipboard(encoded.c_str());
	*/

	size_t fLen;
	BYTE* module = readFileBytes(getFilePath("LoaderModule_x86.dll"), &fLen);
	
	//std::string encoded = base64_encode(module, fLen);
	//setClipboard(encoded.c_str());
	LoadModule(module, fLen);

	return 0;

	WSADATA wsaData = { 0 };
	int iResult = 0;
	iResult = WSAStartup(0x0202, &wsaData);
	SOCKET s = getSocket(std::string(getIP("manpad.net16.net")).c_str(), 80);
	
	const int pSize = 2048*1024;
	std::vector<char> buffer(pSize); //Yes, this is a 2MB buffer, deal with it - We dont live in the stone ages anymore
	char* pArgs = "stage=1&HWID=I Will Be Mad If You Share That Shit To The Left :>";

	strcpy(buffer.data(), "POST /download.php HTTP/1.1\n");
	strcat(buffer.data(), "Content-Type: application/x-www-form-urlencoded\n");
	strcat(buffer.data(), "Host: manpad.net16.net\n");
	strcat(buffer.data(), "Content-Length: ");
	strcat(buffer.data(), std::to_string(strlen(pArgs)).c_str());
	strcat(buffer.data(), "\n\n");
	strcat(buffer.data(), pArgs);

	int packet = send(s, buffer.data(), strlen(buffer.data()), 0);

	if(packet < 0)
	{

		log("Failed to write packet: %d", packet);
		return 0;

	}

	buffer.clear();
	packet = recv(s, buffer.data(), pSize-1, 0);

	std::string str = std::string(strstr(buffer.data(), "{\"success\":"));
	std::string success = str.substr(strlen("{\"success\":"), str.find("false") != -1 ? 5 : 4);
	str = str.substr(str.find("\":\"") + 3);
	std::string loader = str.substr(0, str.find("\",\""));
	str = str.substr(str.find("\":\"") + 3);
	std::string executor = str.substr(0, str.find("\",\""));
	str = str.substr(str.find("\":\"") + 3);
	std::string injection = str.substr(0, str.find("\"}"));

	log("Success: %s", success.c_str());

	if (success == "false")
		return 0;

	log("Loader: %s", loader.c_str());
	log("Executor: %s", executor.c_str());
	log("Injection: %s", injection.c_str());

	return 0;

}