#pragma once
#pragma warning(disable:4800)

#include <winsock2.h>
#include <ws2tcpip.h>
#include <Windows.h>
#include <direct.h>
#include <iostream>
#include <TlHelp32.h>
#include <bitset>
#include <AclAPI.h>
#include <strsafe.h>
#include <iomanip>
#include <sstream>
#include <iterator>
#include <vector>
#include <fstream>

struct UNICODE_STRING
{

	WORD Length;
	WORD MaxLength;
	wchar_t * szBuffer;

};

struct LDR_DATA_TABLE_ENTRY
{

	LIST_ENTRY InLoadOrder;
	LIST_ENTRY InMemoryOrder;
	LIST_ENTRY InInitOrder;
	void * DllBase;
	void * EntryPoint;
	ULONG SizeOfImage;
	UNICODE_STRING FullDllName;
	UNICODE_STRING BaseDllName;

};

typedef NTSTATUS(__stdcall * f_LdrLoadDll)(wchar_t * szOptPath, ULONG ulFlags, UNICODE_STRING * pModuleFileName, HANDLE * pOut);
typedef NTSTATUS(__stdcall * f_NtCreateThreadEx)(HANDLE * procH, ACCESS_MASK security, void * pAttr, HANDLE hProc, void * func, void * args, ULONG Flags, SIZE_T zb, SIZE_T sz, SIZE_T mSz, void * out);

struct LDR_LOAD_DLL_DATA
{

	f_LdrLoadDll pLdrLoadDll;
	HANDLE Out;
	UNICODE_STRING pModuleFileName;
	BYTE Data[MAX_PATH * 2];

};

std::string decryptString(std::string str)
{

	std::string originalBack = "";

	for (int i = 0; i < str.size() / 16; i++)
		originalBack += (std::bitset<16>(str.substr(i * 16, 16)).to_ulong() - (str.size() / 16));

	return originalBack;

}

int log(char* format, ...)
{

	va_list args;
	va_start(args, format);
	int i = vprintf((std::string(format) + "\n").c_str(), args);
	va_end(args);

	return i;

}

int log(const char* format, ...)
{

	va_list args;
	va_start(args, format);
	int i = vprintf((std::string(format) + "\n").c_str(), args);
	va_end(args);

	return i;

}

char* curDir()
{

	char tPath[MAX_PATH * 4];
	_getcwd(tPath, sizeof tPath);

	return tPath;

}

char* getFilePath(const char* fn)
{

	char tPath[MAX_PATH * 4];
	sprintf(tPath, "%s\\%s", curDir(), fn);

	return tPath;

}

void writeFile(char* fn, LPCVOID data, unsigned int size, bool create) {

	HANDLE hFile = CreateFile(fn, GENERIC_WRITE | GENERIC_READ, 0, NULL, create ? CREATE_ALWAYS : OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
	ULONG bytesWritten = 0;
	WriteFile(hFile, data, size, &bytesWritten, NULL);
	CloseHandle(hFile);
	hFile = NULL;

}

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

bool checkFile(const char* fn)
{

	if (!fn)
	{

		log(decryptString("000000000101011100000000010111100000000001010101000000000101010100000000001010010000000001110111000000000110101000000000011101100000000001101110").c_str());
		return false;

	}

	return true;

}

bool setDebugPriv(bool flag)
{

	log(decryptString("000000000101011000000000010001110000000001010011").c_str());

	HANDLE hTCur;

	if (OpenProcessToken(GetCurrentProcess(), TOKEN_QUERY | TOKEN_ADJUST_PRIVILEGES, &hTCur))
	{

		TOKEN_PRIVILEGES tPriv = { 0 };
		tPriv.PrivilegeCount = 1;
		tPriv.Privileges[0].Attributes = flag ? 0x2L : 0;

		if (LookupPrivilegeValueA(nullptr, decryptString("0000000001100011000000000111010100000000010101000000000001110101000000000111001000000000100001010000000001110111000000000110000000000000100000100000000001111001000000001000011000000000011110010000000001111100000000000111010100000000011101110000000001110101").c_str(), &tPriv.Privileges[0].Luid)) {

			if (AdjustTokenPrivileges(hTCur, 0, &tPriv, sizeof(TOKEN_PRIVILEGES), nullptr, nullptr))
			{

				log(decryptString("000000000110101100000000011111010000000010001100000000000011100000000000011010110000000001111101000000000101110000000000011111010000000001111010000000001000110100000000011111110000000001101000000000001000101000000000100000010000000010001110000000001000000100000000100001000000000001111101000000000111111100000000011111010000000001010010000000000011100000000000001111010000000010010000").c_str(), flag);
				CloseHandle(hTCur);
				return true;

			}

			CloseHandle(hTCur);
			log(decryptString("000000000110010000000000011111110000000010000111000000001000101000000000100000110000000010000010000000000011111000000000100100100000000010001101000000000011111000000000100100010000000010000011000000001001001000000000001111100000000001110001000000001000001100000000011000100000000010000011000000001000000000000000100100110000000010000101000000000110111000000000100100000000000010000111000000001001010000000000100001110000000010001010000000001000001100000000100001010000000010000011").c_str());
			return false;

		}

		CloseHandle(hTCur);
		log(decryptString("000000000110010000000000011111110000000010000111000000001000101000000000100000110000000010000010000000000011111000000000100100100000000010001101000000000011111000000000100100010000000010000011000000001001001000000000001111100000000001110001000000001000001100000000011000100000000010000011000000001000000000000000100100110000000010000101000000000110111000000000100100000000000010000111000000001001010000000000100001110000000010001010000000001000001100000000100001010000000010000011").c_str());
		return false;

	}

	CloseHandle(hTCur);
	log(decryptString("000000000110000100000000011111000000000010000100000000001000011100000000100000000000000001111111000000000011101100000000100011110000000010001010000000000011101100000000100000100000000010000000000000001000111100000000001110110000000010001011000000001000110100000000100010100000000001111110000000001000000000000000100011100000000010001110000000000011101100000000100011110000000010001010000000001000011000000000100000000000000010001001").c_str());
	return false;

}

HANDLE getProc(const char* name, DWORD security)
{

	HANDLE h32 = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
	PROCESSENTRY32 pe32 = { 0 };
	pe32.dwSize = sizeof PROCESSENTRY32;

	if (!checkFile(name))
		return nullptr;

	if (!h32)
	{

		log(decryptString("00000000010110010000000001111010000000000010101100000000010100110000000000111110000000000011110100000000010111100000000001011110000000000010101100000000010001010000000001000111").c_str());
		return nullptr;

	}

	bool flag = Process32First(h32, &pe32);

	while (flag)
	{

		if (!strcmp(name, pe32.szExeFile))
		{

			CloseHandle(h32);
			HANDLE out = OpenProcess(security, 0, pe32.th32ProcessID);

			if (out)
				return out;

			/*
			log(decryptString("0000000001100010000000000111110100000000100001010000000010001000000000001000000100000000100000000000000000111100000000001001000000000000100010110000000000111100000000001000101100000000100011000000000010000001000000001000101000000000001111000000000010001100000000001000111000000000100010110000000001111111000000001000000100000000100011110000000010001111000000000101011000000000001111000000000001001100000000001001010000000000010000010000000001110100").c_str(), GetLastError());
			std::cin.ignore();
			exit(0);*/

		}

		flag = Process32Next(h32, &pe32);

	}

	log(decryptString("000000000110010100000000100001110000000010000100000000000111100000000000011110100000000010001000000000001000100000000000001101010000000010000011000000001000010000000000100010010000000000110101000000000111101100000000100001000000000010001010000000001000001100000000011110010000000001001111000000000011010100000000001110100000000010001000").c_str(), name);
	CloseHandle(h32);
	return nullptr;

}

HANDLE shellExecRThread(HANDLE hProc, void* func, void* args, bool hijack, bool fastcall)
{

	log(decryptString("0000000001010111000000000100100100000000010101100000000001011000").c_str());

	if (!hProc)
	{

		log(decryptString("00000000010111000000000001111101000000000010111000000000011111100000000010000000000000000111110100000000011100010000000000101110000000000111011000000000011011110000000001111100000000000111001000000000011110100000000001110011").c_str());
		return nullptr;

	}

	if (!func)
	{

		log(decryptString("000000000101101000000000011110110000000000101100000000000111000100000000100001000000000001110001000000000110111100000000001011000000000001110010000000001000000100000000011110100000000001101111").c_str());
		return nullptr;

	}

	log(decryptString("0000000001010101000000000111011000000000011101110000000001101110000000000111000000000000011110000000000001110110000000000111101100000000011101000000000001000111000000000010110100000000001100100000000001100101").c_str(), hijack);

	if (hijack)
	{

		DWORD pid = GetProcessId(hProc);

		if (!pid)
		{

			log(decryptString("00000000010101010000000001110011000000001000001000000000001011100000000001011110000000000101011100000000010100100000000000101110000000000111010000000000011011110000000001110111000000000111101000000000011100110000000001110010").c_str());
			return nullptr;

		}

		HANDLE h32 = CreateToolhelp32Snapshot(TH32CS_SNAPTHREAD, 0);

		if (!h32)
		{

			log(decryptString("00000000010110010000000001111010000000000010101100000000010100110000000000111110000000000011110100000000010111100000000001011110000000000010101100000000010001010000000001000111").c_str());
			return nullptr;

		}

		THREADENTRY32 TE32 = { 0 };
		TE32.dwSize = sizeof(TE32);

		bool tRet = Thread32First(h32, &TE32);

		while (tRet)
		{

			if (TE32.th32OwnerProcessID == pid && TE32.th32ThreadID != GetCurrentThreadId())
				break;

			tRet = Thread32Next(h32, &TE32);

		}

		CloseHandle(h32);

		if (!tRet)
		{

			log(decryptString("000000000101100000000000011100110000000001111011000000000111111000000000011101110000000001110110000000000011001000000000100001100000000010000001000000000011001000000000011110010000000001110111000000001000011000000000001100100000000001100110000000000101011100000000010001010000000001000100").c_str());
			return nullptr;

		}

		HANDLE oThread = OpenThread(THREAD_ALL_ACCESS, 0, TE32.th32ThreadID);

		if (!oThread)
		{

			log(decryptString("00000000011000100000000001010011000000000100000100000000010000000000000000101110000000000101110100000000011000100000000000101110000000000111010000000000011011110000000001110111000000000111101000000000011100110000000001110010").c_str());
			return nullptr;

		}

		if (!SuspendThread(oThread))
		{

			log(decryptString("000000000101111100000000011000000000000001101011000000000111101100000000011000000000000000101100000000000111001000000000011011010000000001110101000000000111100000000000011100010000000001110000").c_str());
			CloseHandle(oThread);
			return nullptr;

		}

		CONTEXT backupCTX;
		backupCTX.ContextFlags = CONTEXT_CONTROL;

		if (!GetThreadContext(oThread, &backupCTX))
		{

			log(decryptString("00000000010101110000000001110010000000000111101000000000011111010000000001110110000000000111010100000000001100010000000010000101000000001000000000000000001100010000000001111000000000000111011000000000100001010000000000110001000000000101010000000000011001010000000001101001").c_str());
			ResumeThread(oThread);
			CloseHandle(oThread);
			return nullptr;

		}

		void* VAEx = VirtualAllocEx(hProc, nullptr, 0x100, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);

		if (!VAEx)
		{

			log(decryptString("00000000011000010000000001001100000000000101000000000000100000110000000000101011000000000111000100000000011011000000000001110100000000000111011100000000011100000000000001101111").c_str());
			ResumeThread(oThread);
			CloseHandle(oThread);
			return nullptr;

		}

#ifdef _WIN64

		fastcall = true;

		BYTE shell[] =
		{

			0x48, 0x83, 0xEC, 0x08, //sub rsp,08
			0xC7, 0x04, 0x24, 0xEF, 0xBE, 0xAD, 0xDE, //mov [rsp],lRIP
			0xC7, 0x44, 0x24, 0x04, 0xEF, 0xBE, 0xAD, 0xDE, //mov [rsp+04],hRIP
			0x50, 0x51, 0x52, 0x53, 0x41, 0x50, 0x41, 0x51, 0x41, 0x52, 0x41, 0x53,	//push r(acdb)x r(8-11)
			0x48, 0xBB, 0xEF, 0xBE, 0xAD, 0xDE, 0xEF, 0xBE, 0xAD, 0xDE, //mov rbx,func
			0x48, 0xB9, 0xEF, 0xBE, 0xAD, 0xDE, 0xEF, 0xBE, 0xAD, 0xDE, //mov rcx,args
			0x48, 0x83, 0xEC, 0x20, //sub rsp,32
			0xFF, 0xD3, //call rbx
			0x48, 0x83, 0xC4, 0x20, //add rsp,32
			0x41, 0x5B, 0x41, 0x5A, 0x41, 0x59, 0x41, 0x58, 0x5B, 0x5A, 0x59, 0x58, //pop r(11-8) r(bdca)x
			0xC6, 0x05, 0xB0, 0xFF, 0xFF, 0xFF, 0x00, //mov byte ptr[$-0x49],0
			0xC3 //ret

		}; // 0x51

		DWORD lRIP = (DWORD)(backupCTX.Rip & 0xFFFFFFFF);
		DWORD hRIP = (DWORD)((backupCTX.Rip >> 32) & 0xFFFFFFFF);

		*reinterpret_cast<DWORD*>(shell + 0x07) = lRIP;
		*reinterpret_cast<DWORD*>(shell + 0x0F) = hRIP;
		*reinterpret_cast<void**>(shell + 0x21) = func;
		*reinterpret_cast<void**>(shell + 0x2B) = args;

		backupCTX.Rip = reinterpret_cast<DWORD64>(VAEx);

#else

		BYTE shell[] =
		{

			0x60, //pushad
			0xE8, 0x00, 0x00, 0x00, 0x00, //call pCodecave+6
			0x58, //pop eax
			0xB9, 0xEF, 0xBE, 0xAD, 0xDE, //mov ecx,args
			0xB8, 0xEF, 0xBE, 0xAD, 0xDE, //mov eax,func
			0x90, //__fastcall(default): nop //__stdcall(assumed):  push ecx
			0xFF, 0xD0, //call eax
			0x61, //popad
			0x68, 0xEF, 0xBE, 0xAD, 0xDE, //push Eip
			0xC6, 0x05, 0xEF, 0xBE, 0xAD, 0xDE, 0x00,	//mov byte ptr[pCodecave],0
			0xC3, //ret

		}; // 0x22

		if (!fastcall)
			shell[17] = 0x51;

		*reinterpret_cast<void**>(shell + 0x08) = args;
		*reinterpret_cast<void**>(shell + 0x0D) = func;
		*reinterpret_cast<DWORD*>(shell + 0x16) = backupCTX.Eip;
		*reinterpret_cast<void**>(shell + 0x1C) = VAEx;

		backupCTX.Eip = reinterpret_cast<DWORD>(VAEx);

#endif

		if (!WriteProcessMemory(hProc, VAEx, shell, sizeof(shell), 0))
		{

			log(decryptString("000000000110100100000000011000100000000001011111000000000011001000000000001110100000000001100101000000000111101000000000011101110000000001111110000000000111111000000000001110110000000000110010000000000111100000000000011100110000000001111011000000000111111000000000011101110000000001110110").c_str());
			VirtualFreeEx(hProc, VAEx, 0, MEM_RELEASE);
			ResumeThread(oThread);
			CloseHandle(oThread);
			return nullptr;

		}

		if (!SetThreadContext(oThread, &backupCTX))
		{

			log(decryptString("0000000001011101000000000101111000000000010011010000000000101010000000000111000000000000011010110000000001110011000000000111011000000000011011110000000001101110").c_str());
			VirtualFreeEx(hProc, VAEx, 0, MEM_RELEASE);
			ResumeThread(oThread);
			CloseHandle(oThread);
			return nullptr;

		}

		if (!ResumeThread(oThread))
		{

			VirtualFreeEx(hProc, VAEx, 0, MEM_RELEASE);
			CloseHandle(oThread);
			return nullptr;

		}

		BYTE cB = 1;

		while (cB)
			ReadProcessMemory(hProc, VAEx, &cB, 1, 0);

		CloseHandle(oThread);
		VirtualFreeEx(hProc, VAEx, 0, MEM_RELEASE);

		return (HANDLE)1;

	}

	HINSTANCE NT = GetModuleHandleA(decryptString("000000000111011100000000011111010000000001101101000000000111010100000000011101010000000000110111000000000110110100000000011101010000000001110101").c_str());

	if (!NT)
	{

		log(decryptString("0000000001010110000000000111000100000000011110010000000001111100000000000111010100000000011101000000000000110000000000001000010000000000011111110000000000110000000000000111011100000000011101010000000010000100000000000011000000000000010111100000000001100100").c_str());
		return nullptr;

	}

	f_NtCreateThreadEx NTCTEx = reinterpret_cast<f_NtCreateThreadEx>(GetProcAddress(NT, decryptString("0000000001011110000000001000010000000000010100110000000010000010000000000111010100000000011100010000000010000100000000000111010100000000011001000000000001111000000000001000001000000000011101010000000001110001000000000111010000000000010101010000000010001000").c_str()));

	if (!NTCTEx)
	{

		log(decryptString("00000000010110100000000001110101000000000111110100000000100000000000000001111001000000000111100000000000001101000000000010001000000000001000001100000000001101000000000001111011000000000111100100000000100010000000000000110100000000000110001000000000011010000000000001010111000000000110100000000000010110010000000010001100").c_str());
		return nullptr;

	}

	HANDLE ret = nullptr;
	NTCTEx(&ret, THREAD_ALL_ACCESS, nullptr, hProc, func, args, 0, 0, 0, 0, nullptr);

	return ret;

}

bool injLLA(HANDLE hProc, char* dllPath)
{

	log(decryptString("000000000100111100000000010011110000000001000100").c_str());

	if (!hProc)
	{

		log(decryptString("00000000010111000000000001111101000000000010111000000000011111100000000010000000000000000111110100000000011100010000000000101110000000000111011000000000011011110000000001111100000000000111001000000000011110100000000001110011").c_str());
		return false;

	}

	if (!checkFile(dllPath))
		return false;

	HMODULE k32 = GetModuleHandleA(decryptString("000000000111011100000000011100010000000001111110000000000111101000000000011100010000000001111000000000000011111100000000001111100000000000111010000000000111000000000000011110000000000001111000").c_str());

	if (!k32)
	{

		log(decryptString("0000000001011100000000000111011100000000011111110000000010000010000000000111101100000000011110100000000000110110000000001000101000000000100001010000000000110110000000000111110100000000011110110000000010001010000000000011011000000000100000010000000001111011000000001000100000000000100001000000000001111011000000001000001000000000010010010000000001001000").c_str());
		return false;

	}

	LPVOID LLA = GetProcAddress(k32, decryptString("000000000101100000000000011110110000000001101101000000000111000000000000010110000000000001110101000000000110111000000000011111100000000001101101000000000111111000000000100001010000000001001101").c_str());

	if (!LLA)
	{

		log(decryptString("00000000010101110000000001110010000000000111101000000000011111010000000001110110000000000111010100000000001100010000000010000101000000001000000000000000001100010000000001111000000000000111011000000000100001010000000000110001000000000101110100000000010111010000000001010010").c_str());
		return false;

	}

	LPVOID VAEx = VirtualAllocEx(hProc, NULL, strlen(dllPath), MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);

	if (!VAEx)
	{

		log(decryptString("00000000011000010000000001001100000000000101000000000000100000110000000000101011000000000111000100000000011011000000000001110100000000000111011100000000011100000000000001101111").c_str());
		return false;

	}

	if (!WriteProcessMemory(hProc, VAEx, dllPath, strlen(dllPath), 0))
	{

		log(decryptString("0000000001100001000000000101101000000000010101110000000000101010000000000111000000000000011010110000000001110011000000000111011000000000011011110000000001101110").c_str());
		VirtualFreeEx(hProc, VAEx, strlen(dllPath), MEM_RELEASE);
		return false;

	}

	HANDLE rThread = CreateRemoteThread(hProc, NULL, NULL, (LPTHREAD_START_ROUTINE)LLA, VAEx, 0, NULL);

	if (!rThread)
	{

		log(decryptString("0000000001001101000000000101110000000000010111100000000000101010000000000111000000000000011010110000000001110011000000000111011000000000011011110000000001101110").c_str());
		VirtualFreeEx(hProc, VAEx, strlen(dllPath), MEM_RELEASE);
		return false;

	}

	WaitForSingleObject(rThread, INFINITE);
	VirtualFreeEx(hProc, VAEx, strlen(dllPath), MEM_RELEASE);
	CloseHandle(rThread);

	return true;

}

void __stdcall LdrLoadDllShell(LDR_LOAD_DLL_DATA * pData)
{

	if (!pData)
		return;

	pData->pModuleFileName.szBuffer = reinterpret_cast<wchar_t*>(pData->Data);
	pData->pLdrLoadDll(nullptr, 0, &pData->pModuleFileName, &pData->Out);

}

bool injLDR(HANDLE hProc, char* dllPath, bool hijack)
{

	log(decryptString("000000000100111100000000010001110000000001010101").c_str());

	if (!checkFile(dllPath))
		return false;

	LDR_LOAD_DLL_DATA ldr = { 0 };
	ldr.pModuleFileName.szBuffer = reinterpret_cast<wchar_t*>(ldr.Data);
	ldr.pModuleFileName.MaxLength = MAX_PATH * 2;

	size_t dllLen = strlen(dllPath);
	mbstowcs_s(&dllLen, ldr.pModuleFileName.szBuffer, dllLen + 1, dllPath, dllLen);
	ldr.pModuleFileName.Length = (WORD)(dllLen * 2) - 2;

	HINSTANCE NT = GetModuleHandleA(decryptString("000000000111011100000000011111010000000001101101000000000111010100000000011101010000000000110111000000000110110100000000011101010000000001110101").c_str());

	if (!NT)
	{

		log(decryptString("0000000001010110000000000111000100000000011110010000000001111100000000000111010100000000011101000000000000110000000000001000010000000000011111110000000000110000000000000111011100000000011101010000000010000100000000000011000000000000010111100000000001100100").c_str());
		return false;

	}

	FARPROC ldrLoadDll = GetProcAddress(NT, decryptString("0000000001010110000000000110111000000000011111000000000001010110000000000111100100000000011010110000000001101110000000000100111000000000011101100000000001110110").c_str());

	if (!ldrLoadDll)
	{

		log(decryptString("00000000010101110000000001110010000000000111101000000000011111010000000001110110000000000111010100000000001100010000000010000101000000001000000000000000001100010000000001111000000000000111011000000000100001010000000000110001000000000101110100000000010101010000000001100011").c_str());
		return false;

	}

	ldr.pLdrLoadDll = reinterpret_cast<f_LdrLoadDll>(ldrLoadDll);

	BYTE* VAEx = reinterpret_cast<BYTE*>(VirtualAllocEx(hProc, nullptr, sizeof(LDR_LOAD_DLL_DATA) + 0x200, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE));

	if (!VAEx)
	{

		log(decryptString("00000000011000010000000001001100000000000101000000000000100000110000000000101011000000000111000100000000011011000000000001110100000000000111011100000000011100000000000001101111").c_str());
		return false;

	}

	if (!WriteProcessMemory(hProc, VAEx, &ldr, sizeof(LDR_LOAD_DLL_DATA), 0))
	{

		log(decryptString("00000000011010000000000001100001000000000101111000000000001100010000000000111001000000000101110100000000010101010000000001100011000000000101010100000000001110100000000000110001000000000111011100000000011100100000000001111010000000000111110100000000011101100000000001110101").c_str());
		VirtualFreeEx(hProc, VAEx, 0, MEM_RELEASE);
		return false;

	}

	if (!WriteProcessMemory(hProc, VAEx + sizeof(LDR_LOAD_DLL_DATA), LdrLoadDllShell, 0x100, 0))
	{

		log(decryptString("000000000110011000000000010111110000000001011100000000000010111100000000001101110000000001100010000000000101001000000000001110000000000000101111000000000111010100000000011100000000000001111000000000000111101100000000011101000000000001110011").c_str());
		VirtualFreeEx(hProc, VAEx, 0, MEM_RELEASE);
		return false;

	}

	HANDLE rThread = shellExecRThread(hProc, VAEx + sizeof(LDR_LOAD_DLL_DATA), VAEx, hijack, false);

	if (!rThread)
	{

		log(decryptString("00000000010111100000000001010000000000000101110100000000010111110000000000101011000000000111000100000000011011000000000001110100000000000111011100000000011100000000000001101111").c_str());
		VirtualFreeEx(hProc, VAEx, 0, MEM_RELEASE);
		return false;

	}

	if (!hijack)
	{

		WaitForSingleObject(rThread, INFINITE);
		CloseHandle(rThread);

	}

	VirtualFreeEx(hProc, VAEx, 0, MEM_RELEASE);

	return true;

}

DWORD selfProtect()
{

	HANDLE hProc = NULL;
	PVOID pToken = NULL;

	PSID idSystem = NULL;
	PSID idAdmin = NULL;
	PSID idUser = NULL;

	PACL acl = NULL;
	PSECURITY_DESCRIPTOR pDesc = NULL;

	DWORD dwRes = -1;

	__try
	{

		DWORD dwSize = 0;
		bool flag = 0;

		if (!OpenProcessToken(GetCurrentProcess(), TOKEN_READ, &hProc)) {

			dwRes = GetLastError();
			__leave;

		}

		flag = GetTokenInformation(hProc, TokenUser, NULL, 0, &dwSize);
		dwRes = GetLastError();

		if (flag && dwRes != ERROR_INSUFFICIENT_BUFFER)
			__leave;

		if (dwSize)
		{

			pToken = HeapAlloc(GetProcessHeap(), 0, dwSize);
			dwRes = GetLastError();

			if (!pToken)
				__leave;

		}

		flag = GetTokenInformation(hProc, TokenUser, pToken, dwSize, &dwSize);
		dwRes = GetLastError();

		if (!flag && !pToken)
			__leave;

		PSID curUser = ((TOKEN_USER*)pToken)->User.Sid;

		SID_IDENTIFIER_AUTHORITY sidSystem = SECURITY_NT_AUTHORITY;

		flag = AllocateAndInitializeSid(&sidSystem, 1, SECURITY_LOCAL_SYSTEM_RID, 0, 0, 0, 0, 0, 0, 0, &idSystem);
		dwRes = GetLastError();

		if (!flag && !idSystem)
			__leave;

		SID_IDENTIFIER_AUTHORITY sidAdmin = SECURITY_NT_AUTHORITY;

		flag = AllocateAndInitializeSid(&sidAdmin, 2, SECURITY_BUILTIN_DOMAIN_RID, DOMAIN_ALIAS_RID_ADMINS, 0, 0, 0, 0, 0, 0, &idAdmin);
		dwRes = GetLastError();

		if (!flag && !idAdmin)
			__leave;

		SID_IDENTIFIER_AUTHORITY sidUser = SECURITY_WORLD_SID_AUTHORITY;

		flag = AllocateAndInitializeSid(&sidUser, 1, SECURITY_WORLD_RID, 0, 0, 0, 0, 0, 0, 0, &idUser);
		dwRes = GetLastError();

		if (!flag && !idUser)
			__leave;

		const PSID psidArray[] = { idUser, curUser, idSystem, idAdmin };

		dwSize = sizeof(ACL);
		dwSize += GetLengthSid(psidArray[0]);
		dwSize += sizeof(ACCESS_DENIED_ACE) - sizeof(DWORD);

		for (UINT i = 1; i < _countof(psidArray); i++)
		{

			dwSize += GetLengthSid(psidArray[i]);
			dwSize += sizeof(ACCESS_DENIED_ACE) - sizeof(DWORD);

		}

		acl = (PACL)HeapAlloc(GetProcessHeap(), 0, dwSize);
		dwRes = GetLastError();

		if (!acl)
			__leave;

		flag = InitializeAcl(acl, dwSize, ACL_REVISION);
		dwRes = GetLastError();

		if (!flag)
			__leave;

		static const DWORD ppMimic = WRITE_DAC | WRITE_OWNER | PROCESS_CREATE_PROCESS | PROCESS_CREATE_THREAD | PROCESS_DUP_HANDLE | PROCESS_QUERY_INFORMATION | PROCESS_SET_QUOTA | PROCESS_SET_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE | PROCESS_SUSPEND_RESUME | PROCESS_TERMINATE;
		flag = AddAccessDeniedAce(acl, ACL_REVISION, ppMimic, psidArray[0]);

		dwRes = GetLastError();

		if (!flag)
			__leave;

		static const DWORD dwRights = ~ppMimic & 0x1FFF;
		flag = AddAccessDeniedAce(acl, ACL_REVISION, ppMimic, psidArray[1]);
		dwRes = GetLastError();

		if (!flag)
			__leave;

		flag = AddAccessDeniedAce(acl, ACL_REVISION, ppMimic, psidArray[2]);
		dwRes = GetLastError();

		if (!flag)
			__leave;

		flag = AddAccessDeniedAce(acl, ACL_REVISION, ppMimic, psidArray[3]);
		dwRes = GetLastError();

		if (!flag)
			__leave;

		pDesc = (PSECURITY_DESCRIPTOR)HeapAlloc(GetProcessHeap(), 0, SECURITY_DESCRIPTOR_MIN_LENGTH);
		dwRes = GetLastError();

		if (!pDesc)
			__leave;

		flag = InitializeSecurityDescriptor(pDesc, SECURITY_DESCRIPTOR_REVISION);
		dwRes = GetLastError();

		if (!flag)
			__leave;

		dwRes = SetSecurityInfo(GetCurrentProcess(), SE_KERNEL_OBJECT, OWNER_SECURITY_INFORMATION | DACL_SECURITY_INFORMATION, curUser, NULL, acl, NULL);
		dwRes = GetLastError();

		if (dwRes != ERROR_SUCCESS)
			__leave;

		dwRes = ERROR_SUCCESS;

	}
	__finally
	{

		if (pDesc)
		{

			HeapFree(GetProcessHeap(), 0, pDesc);
			pDesc = NULL;

		}

		if (acl)
		{

			HeapFree(GetProcessHeap(), 0, acl);
			acl = NULL;

		}

		if (idSystem)
		{

			FreeSid(idSystem);
			idSystem = NULL;

		}

		if (idAdmin)
		{

			FreeSid(idAdmin);
			idAdmin = NULL;

		}

		if (idUser)
		{

			FreeSid(idUser);
			idUser = NULL;

		}

		if (pToken)
		{

			HeapFree(GetProcessHeap(), 0, pToken);
			pToken = NULL;

		}

		if (hProc)
		{

			CloseHandle(hProc);
			hProc = NULL;

		}

	}

	return dwRes;

}

std::string getPSN()
{

	int cpuinfo[4] = { -1 };
	__cpuid(cpuinfo, 0);

	std::string str = "";

	for (int i = 0; i < 3; i++)
		str += std::to_string(cpuinfo[i]);

	return str;

}

std::string getHDDSN()
{

	DWORD sn;
	GetVolumeInformationA("C:\\", NULL, NULL, &sn, NULL, NULL, NULL, NULL);

	return std::to_string(sn);

}

std::string getOSID()
{

	char name[MAX_COMPUTERNAME_LENGTH + 1] = {};
	DWORD len = sizeof name;

	GetComputerNameA(name, &len);

	std::stringstream ss;
	for (int i = 0; i< strlen(name); ++i)
		ss << std::hex << (int)name[i];

	return ss.str();

}

std::string getID()
{

	std::string str = "";

	str += getPSN().c_str();
	str += "-";
	str += getHDDSN().c_str();
	str += "-";
	str += getOSID().c_str();

	return str;

}

std::string getIP(const char* host)
{

	hostent* he = gethostbyname(host);
	BYTE* ip = reinterpret_cast<BYTE*>(he->h_addr_list[0]);

	std::ostringstream stream;
	std::copy(ip, ip + 4, std::ostream_iterator<unsigned int>(stream, "."));

	return stream.str().substr(0, stream.str().length() - 1);

}

std::string getIP(char* host)
{

	hostent* he = gethostbyname(host);
	BYTE* ip = reinterpret_cast<BYTE*>(he->h_addr_list[0]);

	std::ostringstream stream;
	std::copy(ip, ip + 4, std::ostream_iterator<unsigned int>(stream, "."));

	return stream.str().substr(0, stream.str().length() - 1);

}

void removeString(std::string& parent, const std::string& r)
{

	size_t n = r.length();

	for (size_t i = parent.find(r); i != std::string::npos; i = parent.find(r))
		parent.erase(i, n);

}

SOCKET getSocket(const char* ip, unsigned short port)
{

	SOCKET sockDesc = socket(AF_INET, SOCK_STREAM, 0);

	if (sockDesc < 0)
	{
		log("Failed to open socket: %d", sockDesc);
		std::cin.ignore();
		return 0;
	}

	struct sockaddr_in serverAddress;
	serverAddress.sin_family = AF_INET;
	serverAddress.sin_port = htons(port);
	serverAddress.sin_addr.s_addr = inet_addr(ip);

	if (connect(sockDesc, (struct sockaddr*)&serverAddress, sizeof(serverAddress)) < 0)
	{

		log("Failed to connect");
		std::cin.ignore();
		return 0;

	}

	log("Connected to: %s:%d", ip, port);

	return sockDesc;

}

std::string readSocketLine(int socket, bool includeEndl = false)
{

	int i;
	std::string out = "";
	char c = '\0';

	while ((i = recv(socket, &c, 1, 0)) > 0)
	{

		if (c == '\r')
		{

			if (includeEndl)
				out += c;

			i = recv(socket, &c, 1, MSG_PEEK);

			if (i > 0 && c == '\n')
			{

				i = recv(socket, &c, 1, 0);

				if (includeEndl)
					out += c;

				break;

			}

		}

		out += c;

	}

	return out;

}

/* Begin stolen shit functions */
static const std::string base64_chars =
"ABCDEFGHIJKLMNOPQRSTUVWXYZ"
"abcdefghijklmnopqrstuvwxyz"
"0123456789+/";
static inline bool is_base64(BYTE c) {
	return (isalnum(c) || (c == '+') || (c == '/'));
}
std::string base64_encode(unsigned char const* bytes_to_encode, unsigned int in_len) {

	std::string ret;
	int i = 0;
	int j = 0;
	unsigned char char_array_3[3];
	unsigned char char_array_4[4];

	while (in_len--) {
		char_array_3[i++] = *(bytes_to_encode++);
		if (i == 3) {
			char_array_4[0] = (char_array_3[0] & 0xfc) >> 2;
			char_array_4[1] = ((char_array_3[0] & 0x03) << 4) + ((char_array_3[1] & 0xf0) >> 4);
			char_array_4[2] = ((char_array_3[1] & 0x0f) << 2) + ((char_array_3[2] & 0xc0) >> 6);
			char_array_4[3] = char_array_3[2] & 0x3f;

			for (i = 0; (i <4); i++)
				ret += base64_chars[char_array_4[i]];
			i = 0;
		}
	}

	if (i)
	{
		for (j = i; j < 3; j++)
			char_array_3[j] = '\0';

		char_array_4[0] = (char_array_3[0] & 0xfc) >> 2;
		char_array_4[1] = ((char_array_3[0] & 0x03) << 4) + ((char_array_3[1] & 0xf0) >> 4);
		char_array_4[2] = ((char_array_3[1] & 0x0f) << 2) + ((char_array_3[2] & 0xc0) >> 6);
		char_array_4[3] = char_array_3[2] & 0x3f;

		for (j = 0; (j < i + 1); j++)
			ret += base64_chars[char_array_4[j]];

		while ((i++ < 3))
			ret += '=';

	}

	return ret;

}
std::vector<BYTE> base64_decode(std::string const& encoded_string) {

	int in_len = encoded_string.size();
	int i = 0;
	int j = 0;
	int in_ = 0;
	BYTE char_array_4[4], char_array_3[3];
	std::vector<BYTE> ret;

	while (in_len-- && (encoded_string[in_] != '=') && is_base64(encoded_string[in_])) {
		char_array_4[i++] = encoded_string[in_]; in_++;
		if (i == 4) {
			for (i = 0; i <4; i++)
				char_array_4[i] = base64_chars.find(char_array_4[i]);

			char_array_3[0] = (char_array_4[0] << 2) + ((char_array_4[1] & 0x30) >> 4);
			char_array_3[1] = ((char_array_4[1] & 0xf) << 4) + ((char_array_4[2] & 0x3c) >> 2);
			char_array_3[2] = ((char_array_4[2] & 0x3) << 6) + char_array_4[3];

			for (i = 0; (i < 3); i++)
				ret.push_back(char_array_3[i]);
			i = 0;
		}
	}

	if (i) {
		for (j = i; j <4; j++)
			char_array_4[j] = 0;

		for (j = 0; j <4; j++)
			char_array_4[j] = base64_chars.find(char_array_4[j]);

		char_array_3[0] = (char_array_4[0] << 2) + ((char_array_4[1] & 0x30) >> 4);
		char_array_3[1] = ((char_array_4[1] & 0xf) << 4) + ((char_array_4[2] & 0x3c) >> 2);
		char_array_3[2] = ((char_array_4[2] & 0x3) << 6) + char_array_4[3];

		for (j = 0; (j < i - 1); j++) ret.push_back(char_array_3[j]);
	}

	return ret;
}