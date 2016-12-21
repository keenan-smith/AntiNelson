#include <Windows.h>
#include <string>
#include <bitset>
#include <iostream>


std::string decryptString(std::string str)
{

	std::string originalBack = "";
	for (int i = 0; i < str.size() / 16; i++)
		originalBack += (std::bitset<16>(str.substr(i * 16, 16)).to_ulong() - (str.size() / 16));

	return originalBack;

}
