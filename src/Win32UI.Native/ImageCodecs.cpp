#include "ImageCodecs.h"
#include <wincodec.h>
#include <atlbase.h>
#include <atlcom.h>

static HRESULT ImageCodecCreateBitmapFromFrame(CComPtr<IWICBitmapSource>& frameSource, HBITMAP *hBitmapPtr)
{
	INT width = 0, height = 0;
	HRESULT hr = frameSource->GetSize((UINT *)&width, (UINT *)&height);
	if (FAILED(hr) || width == 0 || height == 0) return hr;

	*hBitmapPtr = nullptr;

	BITMAPINFO bminfo;
	ZeroMemory(&bminfo, sizeof(bminfo));
	bminfo.bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
	bminfo.bmiHeader.biWidth = width;
	bminfo.bmiHeader.biHeight = -height; // the negation is required here
	bminfo.bmiHeader.biPlanes = 1;
	bminfo.bmiHeader.biBitCount = 32;
	bminfo.bmiHeader.biCompression = BI_RGB;

	PVOID pvImageBits = nullptr;
	HDC hdcScreen = GetDC(nullptr);
	*hBitmapPtr = CreateDIBSection(hdcScreen, &bminfo, DIB_RGB_COLORS, &pvImageBits, nullptr, 0);
	ReleaseDC(nullptr, hdcScreen);
	if (*hBitmapPtr == nullptr) return HRESULT_FROM_WIN32(GetLastError());

	const UINT stride = width * 4;
	const UINT cbImage = stride * height;
	hr = frameSource->CopyPixels(nullptr, stride, cbImage, (BYTE *)pvImageBits);
	if (FAILED(hr)) {
		DeleteObject(*hBitmapPtr);
		return hr;
	}

	return S_OK;
}

HRESULT ImageCodecCreateBitmapFromFile(LPCWSTR filePath, HBITMAP *hBitmapPtr)
{
	if (filePath == nullptr || hBitmapPtr == nullptr) return E_INVALIDARG;

	CComPtr<IWICImagingFactory2> factory;
	HRESULT hr = factory.CoCreateInstance(CLSID_WICImagingFactory, nullptr, CLSCTX_INPROC_SERVER);
	if (FAILED(hr)) return hr;

	CComPtr<IWICBitmapDecoder> decoder;
	hr = factory->CreateDecoderFromFilename(filePath, nullptr, GENERIC_READ, WICDecodeMetadataCacheOnDemand, &decoder);
	if (FAILED(hr)) return hr;

	CComPtr<IWICBitmapFrameDecode> frame;
	hr = decoder->GetFrame(0, &frame);
	if (FAILED(hr)) return hr;

	CComPtr<IWICBitmapSource> frameSource;
	hr = frame.QueryInterface(&frameSource);
	if (FAILED(hr)) return hr;

	return ImageCodecCreateBitmapFromFrame(frameSource, hBitmapPtr);
}


HRESULT ImageCodecCreateBitmapFromMemory(LPCVOID memory, ULONG64 size, HBITMAP *hBitmapPtr) {
	if (memory == nullptr || size == 0 || hBitmapPtr == nullptr) return E_INVALIDARG;

	HGLOBAL hMem = ::GlobalAlloc(0, size);
	::memcpy(hMem, memory, size);
	CComPtr<IStream> stream;
	HRESULT hr = CreateStreamOnHGlobal(hMem, true, &stream);
	if (FAILED(hr)) return hr;

	CComPtr<IWICImagingFactory2> factory;
	hr = factory.CoCreateInstance(CLSID_WICImagingFactory, nullptr, CLSCTX_INPROC_SERVER);
	if (FAILED(hr)) return hr;

	CComPtr<IWICBitmapDecoder> decoder;
	hr = factory->CreateDecoderFromStream(stream, nullptr, WICDecodeMetadataCacheOnDemand, &decoder);
	if (FAILED(hr)) return hr;

	CComPtr<IWICBitmapFrameDecode> frame;
	hr = decoder->GetFrame(0, &frame);
	if (FAILED(hr)) return hr;

	CComPtr<IWICBitmapSource> frameSource;
	hr = frame.QueryInterface(&frameSource);
	if (FAILED(hr)) return hr;

	return ImageCodecCreateBitmapFromFrame(frameSource, hBitmapPtr);
}
