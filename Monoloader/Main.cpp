#include "Main.h"

HMODULE hModule = NULL;

void __fastcall eject(DWORD exit)
{

	FreeConsole();
	FreeLibraryAndExitThread(hModule, exit);

}

std::vector<char> ReadAllBytes(const char* filename)
{

	std::ifstream ifs(filename, std::ios::binary | std::ios::ate);
	std::ifstream::pos_type pos = ifs.tellg();

	std::vector<char>  result(pos);

	ifs.seekg(0, std::ios::beg);
	ifs.read(&result[0], pos);

	return result;

}

void __fastcall inject(const char* path, const char* _namespace, const char* _class, const char* method)
{

	int rootDomain = getRootMonoDomain();

	if (!rootDomain)
	{

		printf(decryptString("0000000001011001000000000111010000000000011111000000000001111111000000000111100000000000011101110000000000110011000000001000011100000000100000100000000000110011000000000111101000000000011110000000000010000111000000000011001100000000100000000000000010000010000000001000000100000000100000100000000000011101").c_str());
		eject(0);

	}

	monoAttachToThread(rootDomain);
	setMonoSecurity(MONO_SECURITY_MODE_NONE);

	int data = openAssembly(getMonoDomain(), path);
	PVOID pMethod = getClassMethodFromName(getClassFromName(getAssemblyImageName(data), _namespace, _class), method, 0);

	if (pMethod == nullptr)
	{

		printf(decryptString("00000000010110100000000001110101000000000111110100000000100000000000000001111001000000000111100000000000001101000000000010001000000000001000001100000000001101000000000010000000000000001000001100000000011101010000000001111000000000000011010000000000011111000000000001110101000000000111011100000000011111110000000000011110").c_str());
		eject(0);

	}

	//printf(decryptString("000000000101110000000000011011010000000010000101000000000111100000000000011110110000000001101101000000000111000000000000010001100000000000101100000000000011000100000000011111000000000000010110").c_str(), pMethod);
	//printf(decryptString("0000000001100110000000000111011100000000100011110000000010000010000000001000010100000000011101110000000001111010000000000011011000000000011110100000000001111011000000001000001000000000011111110000000010001100000000000111101100000000100010000000000001111011000000000111101000000000010100000000000000110110000000000011101100000000011110100000000000100000").c_str(), invokeRuntime(pMethod, NULL, NULL, NULL));
	
	invokeRuntime(pMethod, NULL, NULL, NULL);
	loadBE();

	eject(0);

}

DWORD WINAPI initInject(LPVOID param)
{
	
	HMODULE ASM = NULL;

	while (!ASM)
	{

		Sleep(250);
		ASM = GetModuleHandle(decryptString("000000000101001000000000011000100000000010000011000000000111010000000000011100000000000001111100000000001000011000000000011111100000000010000001000000000111101000000000100000100000000000111101000000000111001100000000011110110000000001111011").c_str());

	}

	Sleep(1000);

	SetThreadPriority(GetCurrentThread(), THREAD_PRIORITY_TIME_CRITICAL);

	AllocConsole();
	freopen(decryptString("0000000001001010000000000101011000000000010101010000000001010110000000000101110000000000010110110000000000101011").c_str(), "w", stdout);

	

	HMODULE mono = NULL;

	while (!mono)
	{

		Sleep(250);
		mono = GetModuleHandle(decryptString("00000000011101010000000001110111000000000111011000000000011101110000000000110110000000000110110000000000011101000000000001110100").c_str());

	}

	MonoInit(mono);

	//printf("%i\n", mono);
	//printf("%d\n", GetCurrentProcess());

	char tPath[MAX_PATH * 4];
	_getcwd(tPath, sizeof tPath);
	sprintf(tPath, decryptString("00000000001100110000000010000001000000000110101000000000010110110000000001011010000000000111110100000000011011110000000001110010000000000111001100000000100000000000000000111100000000000111001000000000011110100000000001111010").c_str(), tPath);

	inject(tPath, decryptString("0000000001010100000000000101001100000000011101100000000001101000000000000110101100000000011011000000000001111001").c_str(), decryptString("0000000001010011000000000111011000000000011010000000000001101011000000000111000000000000011101010000000001101110").c_str(), decryptString("00000000011100000000000010000011000000000111000000000000011011100000000010000000000000000111111100000000011100000000000001010111000000000111101000000000011011000000000001101111").c_str());

	return 1;

}

BOOL WINAPI DllMain(HMODULE hInst, DWORD dwR, LPVOID lpR)
{

	switch (dwR)
	{

	case DLL_PROCESS_ATTACH:

		CreateThread(NULL, 0, initInject, NULL, 0, NULL);
		hModule = hInst;

		break;

	}

	return 1;

}