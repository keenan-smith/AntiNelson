#include "stdafx.h"
#include "MonoLoader.h"
#include <Psapi.h>
#include <tchar.h>
#include <iostream>

#define INJECT_DLL "EAC1.dll"
#define TARGET_PROCESS L"Unturned.exe"
#define USE_THREADING
#define DEBUG

using namespace std;

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
		case DLL_PROCESS_ATTACH:
			TCHAR currentProcessName[MAX_PATH];

			GetModuleBaseName(GetCurrentProcess(), GetModuleHandle(NULL), currentProcessName, MAX_PATH);

			if (_tcscmp(currentProcessName, TARGET_PROCESS) != 0){
				FreeLibraryAndExitThread(hModule, 0);
				return FALSE;
			}

			AllocConsole();
			cout << "Loaded ManPAD Console!";
			cin.get();

			break;
		case DLL_THREAD_ATTACH:
			break;
		case DLL_THREAD_DETACH:
			break;
		case DLL_PROCESS_DETACH:
			break;
	}
	return TRUE;
}

