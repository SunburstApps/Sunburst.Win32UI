using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface
{
    public sealed class DialogTemplate : IDisposable
    {
        private DialogUnitHelper mDLUHelper;
        private IntPtr mLayoutHandle = IntPtr.Zero;

        public DialogTemplate() { }

        public void CreateTemplate(DialogTemplateMode mode, string caption, Rect rc, uint style, uint exStyle, string fontName = "Segoe UI",
            int fontSize = 9, string className = null, DialogMetric metric = DialogMetric.Pixel)
        {
            if (mLayoutHandle != IntPtr.Zero) NativeMethods.LayoutDestroy(mLayoutHandle);
            mLayoutHandle = NativeMethods.LayoutCreate();

            uint realStyle = style;

            // Since I am setting the font here, the DS_SETFONT style must be provided.
            realStyle |= DialogStyles.DS_SETFONT;
            // If the caller specified an embedded dialog, the WS_CHILD and DS_CONTROL styles must be provided.
            if (mode == DialogTemplateMode.ChildControl)
            {
                realStyle |= DialogStyles.DS_CONTROL;
                realStyle |= WindowStyles.WS_CHILD;
            }

            mDLUHelper = DialogUnitHelper.GetForFont(fontName, fontSize);

            Rect adjustedRect;
            switch (metric)
            {
                case DialogMetric.Pixel:
                    adjustedRect = mDLUHelper.ConvertToDialogUnits(rc);
                    break;
                case DialogMetric.DialogUnit:
                    adjustedRect = rc;
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(DialogMetric)} value", nameof(metric));
            }

            NativeMethods.LayoutInitializeWithFont(mLayoutHandle, caption, ref adjustedRect, realStyle, exStyle, className, fontName, fontSize);
        }

        public void AddControl(string className, ushort controlId, Rect rc, string text, int style, int exStyle, DialogMetric metric = DialogMetric.Pixel)
        {
            if (className == null) throw new ArgumentNullException(nameof(className));
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (mLayoutHandle == IntPtr.Zero) throw new InvalidOperationException($"You must call {nameof(CreateTemplate)}() before you call {nameof(AddControl)}().");

            int realStyle = style;
            // All dialog controls have WS_CHILD and WS_VISIBLE by default.
            realStyle |= WindowStyles.WS_CHILD;
            realStyle |= WindowStyles.WS_VISIBLE;

            Rect adjustedRect;
            switch (metric)
            {
                case DialogMetric.Pixel:
                    adjustedRect = mDLUHelper.ConvertToDialogUnits(rc);
                    break;
                case DialogMetric.DialogUnit:
                    adjustedRect = rc;
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(DialogMetric)} value", nameof(metric));
            }

            NativeMethods.LayoutAddControl(mLayoutHandle, className, controlId, ref adjustedRect, realStyle, exStyle, text);
        }

        public IntPtr GetTemplatePointer()
        {
            return NativeMethods.LayoutGetDataPointer(mLayoutHandle);
        }

        public void Dispose()
        {
            if (mLayoutHandle != IntPtr.Zero) NativeMethods.LayoutDestroy(mLayoutHandle);
        }
    }

    public enum DialogMetric
    {
        Pixel = 0,
        DialogUnit
    }

    public enum DialogTemplateMode
    {
        TopLevelWindow = 0,
        ChildControl
    }
}
