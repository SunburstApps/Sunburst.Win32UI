# .NET Core Win32 User Interface Library

This repository contains a built-from-scratch Win32 GUI library
targeting .NET Core. It works much like Windows Forms, but with
some important differences (namely, WNDPROC is not abstracted
away as well, and must be overridden in many cases).

Note that you will need to run the `external/update.py` Python script before this project can be
restored or built. Python 2.7 is required.
