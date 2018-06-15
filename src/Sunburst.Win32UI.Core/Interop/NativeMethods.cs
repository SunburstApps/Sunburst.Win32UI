using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.VisualStyles;

namespace Sunburst.Win32UI.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate IntPtr WNDPROC(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct WNDCLASSEXW
    {
        public uint cbSize;
        public uint style;
        public IntPtr lpfnWndProc;
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public IntPtr hCursor;
        public IntPtr hbrBackground;
        public string lpszMenuName;
        public string lpszClassName;
        public IntPtr hIconSm;
    }

    internal static class NativeMethods
    {
        public const int LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
            LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,
            LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 0x00000100,
            LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 0x00000200,
            LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800;

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandleW(string moduleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibraryEx(string fileName, IntPtr hFile, int dwFlags);

        [DllImport("kernel32.dll")]
        public static extern void FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll")]
        public static extern IntPtr FindResourceW(IntPtr hModule, string lpType, IntPtr lpName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LockResource(IntPtr hResourceData);

        [DllImport("kernel32.dll")]
        public static extern int SizeofResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("user32.dll")]
        public static extern int GetMessageW(out MSG msg, IntPtr hWnd, int a, int b);

        [DllImport("user32.dll")]
        public static extern void TranslateMessage(ref MSG msg);

        [DllImport("user32.dll")]
        public static extern void DispatchMessageW(ref MSG msg);

        [DllImport("user32.dll")]
        public static extern void PostQuitMessage(int exitCode);

        [DllImport("user32.dll")]
        public static extern int TranslateAcceleratorW(IntPtr hWnd, IntPtr hAccel, ref MSG msg);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool ShutdownBlockReasonCreate(IntPtr hWnd, string reason);

        [DllImport("user32.dll")]
        public static extern bool ShutdownBlockReasonDestroy(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr CreateAcceleratorTableW([In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] AcceleratorTableEntry[] entries, int count);

        [DllImport("user32.dll")]
        public static extern IntPtr CreateWindowEx(int extendedStyle, string windowClass, string windowName,
            int style, int left, int top, int width, int height, IntPtr hWndParent, IntPtr hMenu,
            IntPtr hInstance, IntPtr createParam);

        [DllImport("user32.dll")]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void GetWindowText(IntPtr hWnd, IntPtr buffer, int bufferLength);

        [DllImport("user32.dll")]
        public static extern void SetWindowText(IntPtr hWnd, string text);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint messageId, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint messageId, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SendNotifyMessage(IntPtr hWnd, uint messageId, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool IsWindowEnabled(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void EnableWindow(IntPtr hWnd, bool enable);

        [DllImport("user32.dll")]
        public static extern void SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsWindowIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsWindowZoomed(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void MoveWindow(IntPtr hWnd, int left, int top, int width, int height, bool redraw);

        [DllImport("user32.dll")]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void GetWindowRect(IntPtr hWnd, ref Rect rect);

        [DllImport("user32.dll")]
        public static extern void GetClientRect(IntPtr hWnd, ref Rect rect);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtrW")]
        public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int index);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW")]
        public static extern void SetWindowLongPtr(IntPtr hWnd, int index, IntPtr value);

        [DllImport("user32.dll")]
        public static extern void InvalidateRect(IntPtr hWnd, IntPtr rectPtr, bool redrawBackground);

        [DllImport("user32.dll")]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr CallWindowProc(WNDPROC wndProc, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr CallWindowProc(IntPtr wndProcRaw, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        public static extern IntPtr SetWindowProc(IntPtr hWnd, int index, WNDPROC wndProc);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        public static extern IntPtr SetWindowProcRaw(IntPtr hWnd, int index, IntPtr wndProc);

        [DllImport("user32.dll", EntryPoint = "DefWindowProcW")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, int relationship);
        public const int GW_CHILD = 5, GW_HWNDNEXT = 2;

        [DllImport("user32.dll")]
        public static extern IntPtr GetProp(IntPtr hWnd, string name);

        [DllImport("user32.dll")]
        public static extern void SetProp(IntPtr hWnd, string name, IntPtr value);

        [DllImport("user32.dll")]
        public static extern void RemoveProp(IntPtr hWnd, string name);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetClassInfo(IntPtr hInstance, string className, out WNDCLASSEXW classInfo);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string fileName);

        [DllImport("user32.dll")]
        public static extern IntPtr RegisterClassExW(ref WNDCLASSEXW windowClass);

        [DllImport("user32.dll")]
        public static extern uint SetTimer(IntPtr hWnd, uint nIDEvent, uint uElapse, IntPtr timerProc);

        [DllImport("user32.dll")]
        public static extern bool KillTimer(IntPtr hWnd, uint nIDEvent);

        [DllImport("user32.dll")]
        public static extern IntPtr CreateMenu();

        [DllImport("user32.dll")]
        public static extern IntPtr CreatePopupMenu();

        [DllImport("user32.dll")]
        public static extern bool DestroyMenu(IntPtr hMenu);

        [DllImport("user32.dll")]
        public static extern bool DeleteMenu(IntPtr hMenu, int position, int flags);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point pt);

        [DllImport("user32.dll")]
        public static extern bool TrackPopupMenuEx(IntPtr hMenu, uint flags, int x, int y, IntPtr hWnd, IntPtr parameterPtr);

        [DllImport("user32.dll")]
        public static extern bool InsertMenuItemW(IntPtr hMenu, uint item, bool byPosition, ref MENUITEMINFO info);

        [DllImport("user32.dll")]
        public static extern bool GetMenuItemInfoW(IntPtr hMenu, uint item, bool byPosition, ref MENUITEMINFO info);

        [DllImport("user32.dll")]
        public static extern bool SetMenuItemInfoW(IntPtr hMenu, uint item, bool byPosition, ref MENUITEMINFO info);

        [DllImport("user32.dll")]
        public static extern IntPtr GetMenu(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr SetMenu(IntPtr hWnd, IntPtr hMenu);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool revert);

        [DllImport("comctl32.dll")]
        public static extern bool InitCommonControlsEx(ref INITCOMMONCONTROLSEX init_struct);

        public struct INITCOMMONCONTROLSEX
        {
            public int dwSize, dwICC;
        }

        [DllImport("ole32.dll", PreserveSig = true)]
        public static extern int CreateStreamOnHGlobal(IntPtr hGlobal, bool deleteOnFree, [MarshalAs(UnmanagedType.Interface)] out IStream stream);

        [DllImport("user32.dll")]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr iconName);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadBitmap(IntPtr hInstance, IntPtr bitmapId);

        [DllImport("user32.dll", EntryPoint = "LoadCursorW")]
        public static extern IntPtr LoadCursor(IntPtr hInstance, IntPtr cursorId);

        [DllImport("gdi32.dll")]
        public static extern void DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateFontIndirect(ref LOGFONT font_struct);

        [DllImport("gdi32.dll")]
        public static extern int GetObject(IntPtr hObject, int structSize, IntPtr structPtr);

        [DllImport("kernel32.dll")]
        public static extern int MulDiv(int number, int numer, int denom);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePen(int penStyle, int width, int color_ref);

        [DllImport("gdi32.dll")]
        public static extern IntPtr ExtCreatePen(int penStyle, int width, ref LOGBRUSH logBrush, int styleCount, [In, MarshalAs(UnmanagedType.LPArray)] int[] style);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateSolidBrush(int color_ref);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateHatchBrush(int index, int color_ref);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePatternBrush(IntPtr hBitmap);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSysColorBrush(int colorId);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hDC, int index);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int width, int height);

        [DllImport("gdi32.dll")]
        public static extern int GetDIBits(IntPtr hDC, IntPtr hBitmap, uint startScanline, uint scanlineCount, [Out] IntPtr bits, ref BITMAPINFO info, uint usage);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRectRgnIndirect(ref Rect frame);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateEllipticRgnIndirect(ref Rect frame);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePolygonRgn([MarshalAs(UnmanagedType.LPArray)] Point[] points, int pointCount, int fillMode);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePolyPolygonRgn([MarshalAs(UnmanagedType.LPArray)] Point[] points, [MarshalAs(UnmanagedType.LPArray)] int[] polygonCounts, int pointCount, int fillMode);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom, int widthRadius, int heightRadius);

        [DllImport("gdi32.dll")]
        public static extern IntPtr PathToRegion(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern int CombineRgn(IntPtr destination, IntPtr source, IntPtr otherSource, int combineMode);

        [DllImport("gdi32.dll")]
        public static extern bool EqualRgn(IntPtr leftRegion, IntPtr rightRegion);

        [DllImport("gdi32.dll")]
        public static extern int OffsetRgn(IntPtr hRegion, int x, int y);

        [DllImport("gdi32.dll")]
        public static extern int GetRgnBox(IntPtr hRegion, ref Rect box);

        [DllImport("gdi32.dll")]
        public static extern bool PtInRegion(IntPtr hRegion, int x, int y);

        [DllImport("gdi32.dll")]
        public static extern bool RectInRegion(IntPtr hRegion, ref Rect frame);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern IntPtr GetCurrentObject(IntPtr hDC, int typeId);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern int SaveDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern bool RestoreDC(IntPtr hDC, int saveId);

        [DllImport("gdi32.dll")]
        public static extern uint GetBoundsRect(IntPtr hDC, ref Rect rect, uint flags);

        [DllImport("gdi32.dll")]
        public static extern uint SetBoundsRect(IntPtr hDC, ref Rect rect, uint flags);

        [DllImport("gdi32.dll")]
        public static extern int GetBkColor(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern int SetBkColor(IntPtr hDC, int color_ref);

        [DllImport("gdi32.dll")]
        public static extern int GetTextColor(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern int SetTextColor(IntPtr hDC, int color_ref);

        [DllImport("gdi32.dll")]
        public static extern bool FillRgn(IntPtr hDC, IntPtr hRegion, IntPtr hBrush);

        [DllImport("gdi32.dll")]
        public static extern bool FrameRgn(IntPtr hDC, IntPtr hRegion, IntPtr hBrush, int width, int height);

        [DllImport("gdi32.dll")]
        public static extern bool InvertRgn(IntPtr hDC, IntPtr hRegion);

        [DllImport("gdi32.dll")]
        public static extern int GetClipRgn(IntPtr hDC, ref IntPtr hRegion);

        [DllImport("gdi32.dll")]
        public static extern int ExtSelectClipRgn(IntPtr hDC, IntPtr hRegion, int mode);

        [DllImport("user32.dll")]
        public static extern bool FillRect(IntPtr hDC, ref Rect rect, IntPtr hBrush);

        [DllImport("user32.dll")]
        public static extern bool FrameRect(IntPtr hDC, ref Rect rect, IntPtr hBrush);

        [DllImport("gdi32.dll")]
        public static extern bool DrawIconEx(IntPtr hDC, int x, int y, IntPtr hIcon, int width, int height, uint stepForAnimatedCursor, IntPtr hBrushForNonFlicker, uint flags = 3);

        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hDC, int x, int y, int width, int height, IntPtr hSourceDC, int sourceX, int sourceY, int rop);

        [DllImport("gdi32.dll")]
        public static extern bool StretchBlt(IntPtr hDC, int x, int y, int width, int height, IntPtr hSourceDC, int sourceX, int sourceY, int sourceWidth, int sourceHeight, int rop);

        [DllImport("gdi32.dll")]
        public static extern bool GradientFill(IntPtr hDC, [MarshalAs(UnmanagedType.LPArray)] TRIVERTEX[] points, int pointCount, ref GRADIENT_RECT rect, int gradientRectCount, int orientation);

        [DllImport("gdi32.dll")]
        public static extern bool AlphaBlend(IntPtr hDC, int x, int y, int width, int height, IntPtr hSourceDC, int sourceX, int sourceY, int sourceWidth, int sourceHeight, BLENDFUNCTION bf);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        public static extern bool ExtTextOut(IntPtr hDC, int x, int y, uint options, ref Rect rect, string text, int textLength, [MarshalAs(UnmanagedType.LPArray)] int[] lpDx);

        [DllImport("user32.dll")]
        public static extern int DrawText(IntPtr hDC, string text, int charCount, ref Rect frame, uint formatFlags);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        public static extern bool GetTextExtentPoint32(IntPtr hDC, string text, int textLength, ref Size size);

        [DllImport("user32.dll")]
        public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDIBSection(IntPtr hDC, ref BITMAPINFO bitmapInfo, int colors, out IntPtr imageBits, IntPtr hSection, int offset);

        [DllImport("gdi32.dll")]
        public static extern bool SetViewportOrgEx(IntPtr hDC, int x, int y, out Point oldOrg);

        [DllImport("gdi32.dll")]
        public static extern void GetTextMetrics(IntPtr hDC, out TEXTMETRIC metric);

        [DllImport("gdi32.dll")]
        public static extern void MapWindowPoints(IntPtr hWndParent, IntPtr hWnd, ref Rect rect, int pointCount = 2);

        [DllImport("gdi32.dll")]
        public static extern void MapWindowPoints(IntPtr hWndParent, IntPtr hWnd, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] Point[] points, int pointCount);

        [DllImport("user32.dll")]
        public static extern IntPtr GetCursor();

        [DllImport("user32.dll")]
        public static extern IntPtr SetCursor(IntPtr hCursor);

        [DllImport("user32.dll")]
        public static extern void SetWindowPos(IntPtr hWnd, IntPtr hWndNewParent, int left, int top, int width, int height, MoveWindowFlags flags);

        [DllImport("user32.dll")]
        public static extern IntPtr BeginDeferWindowPos(int controlCount);

        [DllImport("user32.dll")]
        public static extern IntPtr DeferWindowPos(IntPtr hDefer, IntPtr hWnd, IntPtr hWndInsertAfter,
            int left, int top, int width, int height, MoveWindowFlags flags);

        [DllImport("user32.dll")]
        public static extern void EndDeferWindowPos(IntPtr hDefer);

        [DllImport("comctl32.dll")]
        public static extern int ImageList_GetImageCount(IntPtr hImageList);

        [DllImport("comctl32.dll")]
        public static extern int ImageList_GetBkColor(IntPtr hImageList);

        [DllImport("comctl32.dll")]
        public static extern int ImageList_SetBkColor(IntPtr hImageList, int cr);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_GetImageInfo(IntPtr hImageList, int i, ref IMAGEINFO info);

        [DllImport("comctl32.dll")]
        public static extern IntPtr ImageList_GetIcon(IntPtr hImageList, int i, uint flags);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_GetIconSize(IntPtr hImageList, out int cx, out int cy);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_SetIconSize(IntPtr hImageList, int cx, int cy);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_SetImageCount(IntPtr hImageList, int count);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_SetOverlayImage(IntPtr hImageList, int nImage, int nOverlay);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_Destroy(IntPtr hImageList);

        [DllImport("comctl32.dll")]
        public static extern IntPtr ImageList_Create(int cx, int cy, uint flags, int cInitial, int cGrow);

        [DllImport("comctl32.dll")]
        public static extern int ImageList_Add(IntPtr hImageList, IntPtr hBitmap, IntPtr hBitmapMask);

        [DllImport("comctl32.dll")]
        public static extern int ImageList_AddMasked(IntPtr hImageList, IntPtr hBitmap, int crMask);

        [DllImport("comctl32.dll")]
        public static extern int ImageList_ReplaceIcon(IntPtr hImageList, int nImage, IntPtr hIcon);

        [DllImport("comctl32.dll")]
        public static extern IntPtr ImageList_GetIcon(IntPtr hImageList, int nImage, int flags);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_Draw(IntPtr hImageList, int image, IntPtr hDC, int x, int y, uint style);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_DrawEx(IntPtr hImageList, int index, IntPtr hDC, int x, int y, int width, int height, int backColor, int foreColor, uint style);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_BeginDrag(IntPtr hImageList, int index, int hotspotX, int hotspotY);

        [DllImport("comctl32.dll")]
        public static extern void ImageList_EndDrag();

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_DragMove(int x, int y);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_DragEnter(IntPtr hWnd, int x, int y);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_DragLeave(IntPtr hWnd);

        [DllImport("comctl32.dll")]
        public static extern IntPtr ImageList_Duplicate(IntPtr hImageList);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr OpenThemeData(IntPtr hWnd, string themeClassList);

        [DllImport("uxtheme.dll")]
        public static extern int CloseThemeData(IntPtr hTheme);

        [DllImport("uxtheme.dll")]
        public static extern int DrawThemeBackground(IntPtr hTheme, IntPtr hDC, int partId, int startId, [In] ref Rect rect, IntPtr clipRectPtr);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public static extern int DrawThemeText(IntPtr hTheme, IntPtr hDC, int partId, int stateId, string pchText, int textLength, int flags, int reservedFlags, ref Rect rect);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemeBackgroundContentRect(IntPtr hTheme, IntPtr hDC, int partId, int stateId, [In] ref Rect boundingRect, out Rect contentRect);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemeBackgroundExtent(IntPtr hTheme, IntPtr hDC, int partId, int stateId, [In] ref Rect contentRect, out Rect boundingRect);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemePartSize(IntPtr hTheme, IntPtr hDC, int partId, int stateId, [In] ref Rect rect, PartSizingMode sizeMode, out Size size);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public static extern int GetThemeTextExtent(IntPtr hTheme, IntPtr hDC, int partId, int stateId, string text, int textLength, int flags, [In] ref Rect boundingRect, out Rect extentRect);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemeBackgroundRegion(IntPtr hTheme, IntPtr hDC, int partId, int stateId, [In] ref Rect rect, out IntPtr hRegion);

        [DllImport("uxtheme.dll")]
        public static extern int DrawThemeIcon(IntPtr hTheme, IntPtr hDC, int partId, int stateId, [In] ref Rect rect, IntPtr hImageList, int imageIndex);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemeColor(IntPtr hTheme, int partId, int stateId, int propId, out int color_ref);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemeMetric(IntPtr hTheme, IntPtr hDC, int partId, int stateId, int propId, out int value);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemePosition(IntPtr hTheme, int partId, int stateId, int propId, out Point value);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemeFont(IntPtr hTheme, IntPtr hDC, int partId, int stateId, int propId, out LOGFONT value);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemeRect(IntPtr hTheme, IntPtr hDC, int partId, int stateId, int propId, out Rect value);

        [DllImport("uxtheme.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsThemePartDefined(IntPtr hTheme, int partId, int stateId);

        [DllImport("user32.dll", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfoW(uint id, int byteSize, IntPtr data, int flags);

        #region Windows Imaging Component

        public enum WICDecodeOptions
        {
            WICDecodeMetadataCacheOnDemand = 0,
            WICDecodeMetadataCacheOnLoad = 0x1,
            WICMETADATACACHEOPTION_FORCE_DWORD = 0x7fffffff
        }

        [ComImport, Guid("0000000c-0000-0000-C000-000000000046")]
        public interface IStream { }

        [ComImport, Guid("ec5ec8a9-c395-4314-9c77-54d7a935ff70")]
        public interface IWICImagingFactory
        {
            IWICBitmapDecoder CreateBitmapFromFilename(
                [MarshalAs(UnmanagedType.LPWStr)] string filename,
                [In, Optional] ref Guid pguidVendor,
                int desiredAccess,
                WICDecodeOptions options);

            IWICBitmapDecoder CreateDecoderFromStream(
                [In, MarshalAs(UnmanagedType.Interface)] IStream stream,
                [In, Optional] ref Guid pguidVendor,
                WICDecodeOptions options);
        }

        [ComImport, Guid("9EDDE9E7-8DEE-47ea-99DF-E6FAF2ED44BF")]
        public interface IWICBitmapDecoder
        {
            void QueryCapability(IStream stream, out int capability);
            void Initialize(IStream stream, WICDecodeOptions options);
            void GetContainerFormat(out Guid formatGuid);
            void GetDecoderInfo([MarshalAs(UnmanagedType.Interface)] out object decoderInfo);
            void CopyPalette([MarshalAs(UnmanagedType.Interface)] object palette);
            void GetMetadataQueryReader([MarshalAs(UnmanagedType.Interface)] out object queryReader);
            void GetPreview(out IWICBitmapSource source);
            void GetColorContexts(uint count, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Interface, SizeParamIndex = 0)] object[] colorContexts, out uint actualCount);
            void GetThumbnail(out IWICBitmapSource thumbnail);
            void GetFrameCount(out uint count);
            void GetFrame(uint index, out IWICBitmapFrameDecode frame);
        }

        [ComImport, Guid("00000120-a8f2-4877-ba0a-fd2b6645fb94")]
        public interface IWICBitmapSource
        {
            void GetSize(out int width, out int height);
            void GetPixelFormat(IntPtr pPixelFormat);
            void GetResolution(out double dpX, out double dpiY);
            void CopyPalette([MarshalAs(UnmanagedType.Interface)] object palette);
            void CopyPixels(IntPtr pWicRect, int stride, int bufferSize, IntPtr pbBuffer);
        }

        [ComImport, Guid("3B16811B-6A43-4ec9-A813-3D930C13B940")]
        public interface IWICBitmapFrameDecode { }

        [ComImport, Guid("317d06e8-5f24-433d-bdf7-79ce68d8abc2")]
        [ClassInterface(ClassInterfaceType.None)]
        public class WICImagingFactory { }

        #endregion
    }
}
