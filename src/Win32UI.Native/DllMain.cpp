#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

extern "C" HINSTANCE hDllInstance;
HINSTANCE hDllInstance = nullptr;
INT_PTR WINAPI DllMain(HINSTANCE hInst, DWORD reason, LPVOID reserved) {
    if (reason == DLL_PROCESS_ATTACH) {
        DisableThreadLibraryCalls(hInst);
        hDllInstance = hInst;
    }

    return (INT_PTR)TRUE;
}
