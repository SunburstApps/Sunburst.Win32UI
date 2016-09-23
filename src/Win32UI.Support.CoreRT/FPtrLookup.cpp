#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <cassert>
#include <cstring>

_EXTERN_C

extern void Win32UI_DialogProc();
extern void Win32UI_SubclassWndProc();
extern void Win32UI_CustomWndProc();
extern void Win32UI_TaskDialogCallback();

#define MATCH(symbol) if (strcmp(name, #symbol) == 0) return symbol

PVOID Win32UI_FPtrLookup(const char *name) {
	assert(name != nullptr);

	MATCH(Win32UI_DialogProc);
	MATCH(Win32UI_SubclassWndProc);
	MATCH(Win32UI_CustomWndProc);
	MATCH(Win32UI_TaskDialogCallback);

	assert(!"Unrecognized FPtrLookup symbol name");
	return nullptr;
}

_END_EXTERN_C
