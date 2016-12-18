#include "Main.h"

HMODULE hModule = NULL;

void __fastcall eject(DWORD exit)
{

	FreeConsole();
	FreeLibraryAndExitThread(hModule, exit);

}

std::vector<char> ReadAllBytes(char const* filename)
{

	std::ifstream ifs(filename, std::ios::binary | std::ios::ate);
	std::ifstream::pos_type pos = ifs.tellg();

	std::vector<char>  result(pos);

	ifs.seekg(0, std::ios::beg);
	ifs.read(&result[0], pos);

	return result;

}

void __fastcall inject(char* path, char* _namespace, char* _class, char* method)
{

	int rootDomain = getRootMonoDomain();

	if (!rootDomain)
	{

		printf("Failed to get mono\n");
		eject(0);

	}

	monoAttachToThread(rootDomain);
	setMonoSecurity(MONO_SECURITY_MODE_NONE);

	int data = openAssembly(getMonoDomain(), path);
	PVOID pMethod = getClassMethodFromName(getClassFromName(getAssemblyImageName(data), _namespace, _class), method, 0);

	if (pMethod == nullptr)
	{

		printf("Failed to load hack\n");
		eject(0);

	}

	printf("Payload: %p\n", pMethod);
	printf("Payload delivered: %d\n", invokeRuntime(pMethod, NULL, NULL, NULL));

	loadBE();

	eject(0);

}

DWORD WINAPI initInject(LPVOID param)
{

	SetThreadPriority(GetCurrentThread(), THREAD_PRIORITY_TIME_CRITICAL);

	AllocConsole();
	freopen("CONOUT$", "w", stdout);

	HMODULE ASM = NULL;

	while (!ASM)
	{

		Sleep(250);
		ASM = GetModuleHandle("CSteamworks.dll");

	}

	HMODULE mono = NULL;

	while (!mono)
	{

		Sleep(250);
		mono = GetModuleHandle("mono.dll");

	}

	MonoInit(mono);

	printf("%i\n", mono);
	printf("%d\n", GetCurrentProcess());

	char tPath[MAX_PATH * 4];
	_getcwd(tPath, sizeof tPath);
	sprintf(tPath, "%s\\MLoader.dll", tPath);

	inject(tPath, "MLoader", "Loading", "executeLoad");

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