#ifndef UNICODE
#define UNICODE
#endif
#include "LayoutManager.h"

typedef WTL::CMemDlgTemplateT<CWinTraits<WS_CLIPCHILDREN | WS_CLIPSIBLINGS, 0>> LAYOUT;
typedef LAYOUT * PLAYOUT;

HLAYOUT LayoutCreate()
{
    PLAYOUT layout = new LAYOUT;
    return (HLAYOUT)layout;
}

void LayoutDestroy(HLAYOUT hLayout) {
    PLAYOUT layout = (PLAYOUT)hLayout;
    delete layout;
}

void LayoutInitialize(HLAYOUT hLayout, LPCWSTR lpszCaption, const PRECT rc, DWORD style, DWORD exStyle, LPCWSTR className)
{
    LayoutInitializeWithFont(hLayout, lpszCaption, rc, style | DS_SETFONT, exStyle, className, L"Segoe UI", 9);
}

void LayoutInitializeWithFont(HLAYOUT hLayout, LPCWSTR lpszCaption, const PRECT rc, DWORD style, DWORD exStyle, LPCWSTR className, LPCWSTR fontName, WORD fontSize) {
    PLAYOUT layout = (PLAYOUT)hLayout;
    layout->Create(true, lpszCaption, *rc, style, exStyle, fontName, fontSize, 0, 0, 0, 0, ATL::_U_STRINGorID(className), ATL::_U_STRINGorID(nullptr));
}

void LayoutAddControl(HLAYOUT hLayout, LPCWSTR className, WORD controlId, const PRECT rc, DWORD style, DWORD exStyle, LPCWSTR text) {
    PLAYOUT layout = (PLAYOUT)hLayout;
    layout->AddControl(ATL::_U_STRINGorID(className), controlId, *rc, style, exStyle, ATL::_U_STRINGorID(text));
}

HWND LayoutCreateDialog(HLAYOUT hLayout, HWND hWndParent, DLGPROC dlgProc, LPARAM createParam) {
    PLAYOUT layout = (PLAYOUT)hLayout;
    return ::CreateDialogIndirectParam(GetModuleHandle(nullptr), layout->GetTemplatePtr(), hWndParent, dlgProc, createParam);
}

INT_PTR LayoutRunModally(HLAYOUT hLayout, HWND hWndParent, DLGPROC dlgProc, LPARAM createParam) {
    PLAYOUT layout = (PLAYOUT)hLayout;
    return ::DialogBoxIndirectParam(GetModuleHandle(nullptr), layout->GetTemplatePtr(), hWndParent, dlgProc, createParam);
}

PVOID LayoutGetDataPointer(HLAYOUT hLayout) {
	PLAYOUT layout = (PLAYOUT)hLayout;
	return (PVOID)layout->GetTemplatePtr();
}
