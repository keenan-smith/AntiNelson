#include <string>
#include <iostream>
#include <Windows.h>
#include <fstream>
#include "resource.h"

ULONG protect(ULONG u)
{

	ULONG mapping[] = { PAGE_NOACCESS, PAGE_EXECUTE, PAGE_READONLY, PAGE_EXECUTE_READ, PAGE_READWRITE, PAGE_EXECUTE_READWRITE, PAGE_READWRITE, PAGE_EXECUTE_READWRITE };

	return mapping[u >> 29];

}

