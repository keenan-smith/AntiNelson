#include <fstream>
#include <vector>
#include <wchar.h>
#include <Windows.h>
#include <stdint.h>
#include <stdio.h>

#ifndef _MONO_FUNCTIONS_
#define _MONO_FUNCTIONS_

typedef enum {
	MONO_SECURITY_MODE_NONE,
	MONO_SECURITY_MODE_CORE_CLR,
	MONO_SECURITY_MODE_CAS,
	MONO_SECURITY_MODE_SMCS_HACK
} MonoSecurityMode;

typedef void(*mono_security_set_t) (MonoSecurityMode a_security);
typedef PVOID(*mono_domain_get_t) (void);
typedef int(*mono_get_root_domain_t) (void);
typedef int(*mono_assembly_load_from_full_t) (int a_image, int *a_fname, int *a_status, bool a_refonly);
typedef int(*mono_domain_assembly_open_t)(PVOID a_domain, PCHAR a_file);
typedef int(*mono_assembly_get_image_t) (int a_assembly);
typedef PVOID(*mono_class_from_name_t) (int a_image, const char* a_name_space, const char *a_name);
typedef PVOID(*mono_class_get_method_from_name_t) (PVOID a_klass, const char *a_name, int a_param_count);
typedef int(*mono_runtime_invoke_t) (PVOID a_method, void *a_obj, void **a_params, int **a_exc);
typedef PVOID(*mono_thread_attach_t) (int a_domain);

mono_security_set_t setMonoSecurity;
mono_domain_get_t getMonoDomain;
mono_get_root_domain_t getRootMonoDomain;
mono_domain_assembly_open_t openAssembly;
mono_assembly_get_image_t getAssemblyImageName;
mono_class_from_name_t getClassFromName;
mono_class_get_method_from_name_t getClassMethodFromName;
mono_runtime_invoke_t invokeRuntime;;
mono_thread_attach_t monoAttachToThread;

void __fastcall MonoInit(HMODULE hMono)
{

	setMonoSecurity = (mono_security_set_t)GetProcAddress(hMono, "mono_security_set_mode");
	getMonoDomain = (mono_domain_get_t)GetProcAddress(hMono, "mono_domain_get");
	getRootMonoDomain = (mono_get_root_domain_t)GetProcAddress(hMono, "mono_get_root_domain");
	openAssembly = (mono_domain_assembly_open_t)GetProcAddress(hMono, "mono_domain_assembly_open");
	getAssemblyImageName = (mono_assembly_get_image_t)GetProcAddress(hMono, "mono_assembly_get_image");
	getClassFromName = (mono_class_from_name_t)GetProcAddress(hMono, "mono_class_from_name");
	getClassMethodFromName = (mono_class_get_method_from_name_t)GetProcAddress(hMono, "mono_class_get_method_from_name");
	invokeRuntime = (mono_runtime_invoke_t)GetProcAddress(hMono, "mono_runtime_invoke");
	monoAttachToThread = (mono_thread_attach_t)GetProcAddress(hMono, "mono_thread_attach");

}
#endif _MONO_FUNCTIONS_