using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;
using Microsoft.Win32.UserInterface.Interop;
using Vestris.ResourceLib;

namespace Microsoft.Win32.UserInterface
{
    public sealed class DialogTemplate : IDisposable
    {
        private DialogUnitHelper mDLUHelper;
        private Vestris.ResourceLib.DialogTemplate mNativeTemplate = null;

        public DialogTemplate() { }

        public void CreateTemplate(DialogTemplateMode mode, string caption, Rect rc, uint style, uint exStyle, NonOwnedFont font, string className = null, DialogMetric metric = DialogMetric.Pixel)
        {
            if (font == null) throw new ArgumentNullException(nameof(font));

            LOGFONT descriptor = font.GetFontDescriptor();
            int pointSize;
            using (GraphicsContext dc = GraphicsContext.CreateOffscreenContext())
            {
                pointSize = descriptor.GetPointSize(dc);
            }

            CreateTemplate(mode, caption, rc, style, exStyle, descriptor.lfFaceName, pointSize, className, metric);
        }

        public void CreateTemplate(DialogTemplateMode mode, string caption, Rect rc, uint style, uint exStyle, string fontName = "Segoe UI",
            int fontSize = 9, string className = null, DialogMetric metric = DialogMetric.Pixel)
        {
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

            mNativeTemplate = new Vestris.ResourceLib.DialogTemplate();
            mNativeTemplate.x = Convert.ToInt16(adjustedRect.left);
            mNativeTemplate.y = Convert.ToInt16(adjustedRect.top);
            mNativeTemplate.cx = Convert.ToInt16(adjustedRect.Width);
            mNativeTemplate.cy = Convert.ToInt16(adjustedRect.Height);
            mNativeTemplate.Caption = caption;
            mNativeTemplate.Style = realStyle;
            mNativeTemplate.ExtendedStyle = exStyle;
            mNativeTemplate.TypeFace = fontName;
            mNativeTemplate.PointSize = Convert.ToUInt16(fontSize);
        }

        public void AddControl(string className, ushort controlId, Rect rc, string text, int style, int exStyle, DialogMetric metric = DialogMetric.Pixel)
        {
            if (className == null) throw new ArgumentNullException(nameof(className));
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (mNativeTemplate == null) throw new InvalidOperationException($"You must call {nameof(CreateTemplate)}() before you call {nameof(AddControl)}().");

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

            DialogTemplateControl ctrl = new DialogTemplateControl();
            ctrl.x = Convert.ToInt16(adjustedRect.left);
            ctrl.y = Convert.ToInt16(adjustedRect.top);
            ctrl.cx = Convert.ToInt16(adjustedRect.Width);
            ctrl.y = Convert.ToInt16(adjustedRect.Height);
            ctrl.Style = Convert.ToUInt32(realStyle);
            ctrl.ExtendedStyle = Convert.ToUInt32(exStyle);
            ctrl.ControlClassId = new ResourceId(className);
            ctrl.Id = Convert.ToInt16(controlId);
            ctrl.CaptionId = new ResourceId(text);
            mNativeTemplate.Controls.Add(ctrl);
        }

        public HGlobal GetTemplatePointer()
        {
            using (Stream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, true))
                {
                    mNativeTemplate.Write(writer);
                }

                stream.Seek(0, SeekOrigin.Begin);
                byte[] byteArray = new byte[stream.Length];
                stream.Read(byteArray, 0, byteArray.Length);

                HGlobal ptr = new HGlobal(Convert.ToInt32(stream.Length));
                Marshal.Copy(byteArray, 0, ptr.Handle, byteArray.Length);
                return ptr;
            }
        }

        public void Dispose()
        {
            // Currently unneeded.
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
