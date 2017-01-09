#pragma once
#include <winsock2.h>
#include <ws2tcpip.h>
#include <Windows.h>
#include <direct.h>
#include <string>
#include <bitset>
#include <iostream>
#include <iterator>
#include <sstream>
#include <vector>

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

std::string encryptString(char* s)
{

	std::string output = "";
	for (std::size_t i = 0; i < strlen(s); i++)
		output += std::bitset<16>(s[i] + strlen(s)).to_string();

	return output;

}

std::string decryptString(std::string str)
{

	std::string originalBack = "";
	for (int i = 0; i < str.size() / 16; i++)
		originalBack += (std::bitset<16>(str.substr(i * 16, 16)).to_ulong() - (str.size() / 16));

	return originalBack;

}

char* curDir()
{

	char tPath[MAX_PATH * 4];
	_getcwd(tPath, sizeof tPath);

	return tPath;

}

const char* getFilePath(const char* fn)
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

void removeString(std::string& parent, const std::string& r)
{

	size_t n = r.length();

	for (size_t i = parent.find(r); i != std::string::npos; i = parent.find(r))
		parent.erase(i, n);

}

std::string readSocketLine(int socket, bool includeEndl = false)
{

	int i;
	std::string out = "";
	char c = '\0';

	while((i = recv(socket, &c, 1, 0)) > 0)
	{

		if (c == '\r')
		{

			if (includeEndl)
				out += c;

			i = recv(socket, &c, 1, MSG_PEEK);

			if(i > 0 && c == '\n')
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