#include "Main.h"

HMODULE hModule = NULL;

void __fastcall eject(DWORD exit)
{

	FreeConsole();
	FreeLibraryAndExitThread(hModule, exit);

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
	printf("Attached to mono main thread\n");

	setMonoSecurity(MONO_SECURITY_MODE_NONE);
	printf("Set mono security to MONO_SECURITY_MODE_NONE\n");

	int data = openAssembly(getMonoDomain(), path);

	PVOID pMethod = getClassMethodFromName(getClassFromName(getAssemblyImageName(data), _namespace, _class), method, 0);

	if (pMethod == nullptr)
	{

		printf("Failed to load hack\n");
		eject(0);

	}

	printf("Payload: %p\n", pMethod);
	printf("Payload delivered: %d", invokeRuntime(pMethod, NULL, NULL, NULL));

	eject(0);

}

DWORD WINAPI initInject(LPVOID param)
{

	SetThreadPriority(GetCurrentThread(), THREAD_PRIORITY_TIME_CRITICAL);

	HMODULE mono = NULL;

	while (!mono)
	{

		mono = GetModuleHandle("mono.dll");
		Sleep(250);

	}

	char tempPath[MAX_PATH * 4];
	GetTempPathA(MAX_PATH * 4, tempPath);
	sprintf_s(tempPath, "%s-", tempPath);
	printf("%s\n", tempPath);

	inject(tempPath, "ManPAD.ManPAD_Loading", "Hook", "callMeToHook");

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