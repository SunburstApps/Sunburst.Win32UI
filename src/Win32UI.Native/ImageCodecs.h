#pragma once

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include "dllapi.h"

EXTERN_C UINATIVE_API HRESULT ImageCodecCreateBitmapFromFile(LPCWSTR filePath, HBITMAP *hBitmapPtr);
EXTERN_C UINATIVE_API HRESULT ImageCodecCreateBitmapFromMemory(LPCVOID memory, ULONG64 size, HBITMAP *hBitmapPtr);
