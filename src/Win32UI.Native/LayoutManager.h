#pragma once

#pragma warning(push)
#pragma warning(disable: 4302 4838)
#include <atlbase.h>
#include <atlcore.h>
#include <atlwin.h>
#include <atlapp.h>
#include <atlctrls.h>
#include <atlctrlw.h>
#include <atlctrlx.h>
#include <atldlgs.h>
#include <atlribbon.h>
#include <atltheme.h>
#pragma warning(pop)
#include "dllapi.h"

typedef HANDLE HLAYOUT;

EXTERN_C UINATIVE_API HLAYOUT LayoutCreate();
EXTERN_C UINATIVE_API void LayoutDestroy(HLAYOUT hLayout);
EXTERN_C UINATIVE_API void LayoutInitialize(HLAYOUT hLayout, LPCWSTR lpszCaption, const PRECT rc, DWORD style, DWORD exStyle, LPCWSTR className);
EXTERN_C UINATIVE_API void LayoutInitializeWithFont(HLAYOUT hLayout, LPCWSTR lpszCaption, const PRECT rc, DWORD style, DWORD exStyle, LPCWSTR className, LPCWSTR fontName, WORD fontSize);
EXTERN_C UINATIVE_API void LayoutAddControl(HLAYOUT hLayout, LPCWSTR className, WORD controlId, const PRECT rc, DWORD style, DWORD exStyle, LPCWSTR text);
EXTERN_C UINATIVE_API HWND LayoutCreateDialog(HLAYOUT hLayout, HWND hWndParent, DLGPROC dlgProc, LPARAM createParam);
EXTERN_C UINATIVE_API INT_PTR LayoutRunModally(HLAYOUT hLayout, HWND hWndParent, DLGPROC dlgProc, LPARAM createParam);
EXTERN_C UINATIVE_API PVOID LayoutGetDataPointer(HLAYOUT hLayout);
