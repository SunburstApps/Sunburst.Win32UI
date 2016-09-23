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
        private BinaryWriter mLayoutWriter;

        public DialogTemplate() { }

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

            mLayoutWriter = new BinaryWriter(new MemoryStream());

            // First, write the fields in the DIALOGTEMPLATE structure.
            mLayoutWriter.Write(style);
            mLayoutWriter.Write(exStyle);
            mLayoutWriter.Write((ushort)0); // This is the control count, and it will be adjusted later.
            mLayoutWriter.Write(adjustedRect.left); // origin.x, in DLUs
            mLayoutWriter.Write(adjustedRect.top); // origin.y, in DLUs
            mLayoutWriter.Write(adjustedRect.Width); // size.Width, in DLUs
            mLayoutWriter.Write(adjustedRect.Height); // size.Height, in DLUs

            // Now I need to specify the menu ID and the window-class ID. Since the menu ID is supported in this implementation, just pass null.
            DialogUtils.WriteResourceId(mLayoutWriter, null);

            if (className != null)
            {
                DialogUtils.WriteResourceId(mLayoutWriter, new ResourceId(className));
            }
            else
            {
                DialogUtils.WriteResourceId(mLayoutWriter, null);
            }

            // Now I write the window caption.
            DialogUtils.WriteResourceId(mLayoutWriter, new ResourceId(caption));

            // Now I write the font point size and the face name.
            mLayoutWriter.Write((ushort)fontSize);
            DialogUtils.WriteResourceId(mLayoutWriter, new ResourceId(fontName));
        }

        public void AddControl(string className, ushort controlId, Rect rc, string text, int style, int exStyle, DialogMetric metric = DialogMetric.Pixel)
        {
            if (className == null) throw new ArgumentNullException(nameof(className));
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (mLayoutWriter == null) throw new InvalidOperationException($"You must call {nameof(CreateTemplate)}() before you call {nameof(AddControl)}().");

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

            // First, update the cdit field in the DIALOGTEMPLATE structure.
            mLayoutWriter.Seek(2 * 2, SeekOrigin.Begin);
            byte[] buffer = new byte[2];
            mLayoutWriter.BaseStream.Read(buffer, 0, 2);
            ushort controlCount = (ushort)BitConverter.ToInt16(buffer, 0);
            controlCount += 1;
            mLayoutWriter.Seek(2 * 2, SeekOrigin.Begin);
            mLayoutWriter.Write(controlCount);
            mLayoutWriter.Seek(0, SeekOrigin.End);

            // Next, write the fields in the DIALOGITEMTEMPLATE structure.
            mLayoutWriter.Write(realStyle);
            mLayoutWriter.Write(exStyle);
            mLayoutWriter.Write((short)adjustedRect.left);
            mLayoutWriter.Write((short)adjustedRect.top);
            mLayoutWriter.Write((short)adjustedRect.Width);
            mLayoutWriter.Write((short)adjustedRect.Height);
            mLayoutWriter.Write((short)controlId);

            // Then, write the control class and the control's caption.
            DialogUtils.WriteResourceId(mLayoutWriter, new ResourceId(className));
            DialogUtils.WriteResourceId(mLayoutWriter, new ResourceId(text));

            // Then, write the creation data. Since we don't support this, just write zero.
            mLayoutWriter.Write((short)0);
        }

        public HGlobal CreateTemplatePointer()
        {
            int count = Convert.ToInt32(mLayoutWriter.BaseStream.Length);
            byte[] buffer = new byte[count];
            mLayoutWriter.BaseStream.Seek(0, SeekOrigin.Begin);
            mLayoutWriter.BaseStream.Read(buffer, 0, count);

            HGlobal ptr = new HGlobal(count);
            Marshal.Copy(buffer, 0, ptr.Handle, count);
            return ptr;
        }

        public void Dispose()
        {
            mLayoutWriter.Dispose();
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
