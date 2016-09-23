#pragma once

#ifndef UINATIVE_API
#ifdef  WIN32UINATIVE_EXPORTS
#define UINATIVE_API __declspec(dllexport)
#else
#define UINATIVE_API __declspec(dllimport)
#endif
#endif
