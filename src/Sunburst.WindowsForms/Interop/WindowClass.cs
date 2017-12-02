using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace Sunburst.WindowsForms.Interop
{
    internal sealed class WindowClass
    {
        private static readonly Dictionary<string, WindowClass> Cache = new Dictionary<string, WindowClass>();

        private static string GetFullClassName(string className, int classStyle)
        {
            string realClassName = className ?? "<Window>";
            return $"Sunburst.WindowsForms:{realClassName}:{classStyle}";
        }

        public static WindowClass GetWindowClass(string className, int classStyle)
        {
            string fullClassName = GetFullClassName(className, classStyle);

            bool alreadyExists = Cache.TryGetValue(fullClassName, out var value);
            if (alreadyExists) return value;

            value = new WindowClass(className, classStyle);
            Cache[fullClassName] = value;

            return value;
        }

        private WindowClass(string className, int classStyle)
        {
            ClassName = className;
            ClassStyle = classStyle;
        }

        public string ClassName { get; }
        public int ClassStyle { get; }

        private string registeredClassName = null;

        public string Register(IntPtr wndProc, out IntPtr superclassWndProc)
        {
            bool ok = NativeMethods.GetClassInfo(IntPtr.Zero, ClassName, out var classInfo);
            if (!ok) throw new Win32Exception("Invalid window class name '" + ClassName + "'");
            superclassWndProc = classInfo.lpfnWndProc;

            if (registeredClassName != null)
            {
                // We've already registered ourselves, no need to do so again.
                return registeredClassName;
            }

            classInfo.lpszClassName = GetFullClassName(ClassName, ClassStyle);
            classInfo.lpfnWndProc = wndProc;
            classInfo.hInstance = NativeMethods.GetModuleHandle(null);

            NativeMethods.RegisterClassExW(ref classInfo);
            registeredClassName = classInfo.lpszClassName;
            return registeredClassName;
        }
    }
}
