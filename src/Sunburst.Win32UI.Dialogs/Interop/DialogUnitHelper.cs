using System;
using Sunburst.Win32UI.Graphics;

namespace Sunburst.Win32UI.Interop
{
    public sealed class DialogUnitHelper
    {
        public static DialogUnitHelper GetForFont(string fontName, int fontSize)
        {
            Rect windowRect = new Rect() { left = 0, top = 0, right = 100, bottom = 100 };

            DialogTemplate template = new DialogTemplate();
            template.CreateTemplate(DialogTemplateMode.TopLevelWindow, "Dialog Unit Helper", windowRect, 0, 0, metric: DialogMetric.DialogUnit);
            Dialog dialog = new Dialog();
            dialog.Create(template);

            DialogUnitHelper unitHelper = new DialogUnitHelper(dialog);
            dialog.DestroyWindow();
            return unitHelper;
        }

        public DialogUnitHelper(IWin32Window ownerWindow)
        {
            Rect rc = new Rect
            {
                top = 0,
                left = 0,
                right = 4,
                bottom = 8
            };

            if (!NativeMethods.MapDialogRect(ownerWindow.Handle, ref rc))
                throw new System.ComponentModel.Win32Exception("MapDialogRect() failed", new System.ComponentModel.Win32Exception());

            SizeUnits = new Size(rc.right, rc.bottom);
        }

        public Size SizeUnits { get; private set; }

        public int ConvertToDialogUnitsX(int pixels)
        {
            return NativeMethods.MulDiv(pixels, 4, SizeUnits.width);
        }

        public int ConvertToDialogUnitsY(int pixels)
        {
            return NativeMethods.MulDiv(pixels, 8, SizeUnits.height);
        }

        public Point ConvertToDialogUnits(Point point)
        {
            return new Point(ConvertToDialogUnitsX(point.x), ConvertToDialogUnitsY(point.y));
        }

        public Size ConvertToDialogUnits(Size size)
        {
            return new Size(ConvertToDialogUnitsX(size.width), ConvertToDialogUnitsY(size.height));
        }

        public Rect ConvertToDialogUnits(Rect rect)
        {
            return new Rect
            {
                top = ConvertToDialogUnitsY(rect.top),
                left = ConvertToDialogUnitsX(rect.left),
                right = ConvertToDialogUnitsX(rect.right),
                bottom = ConvertToDialogUnitsY(rect.bottom)
            };
        }

        public int ConvertToPixelsX(int dialogUnits)
        {
            return NativeMethods.MulDiv(dialogUnits, SizeUnits.width, 4);
        }

        public int ConvertToPixelsY(int dialogUnits)
        {
            return NativeMethods.MulDiv(dialogUnits, SizeUnits.height, 8);
        }

        public Point ConvertToPixels(Point point)
        {
            return new Point(ConvertToPixelsX(point.x), ConvertToPixelsY(point.y));
        }

        public Size ConvertToPixels(Size size)
        {
            return new Size(ConvertToPixelsX(size.width), ConvertToPixelsY(size.height));
        }

        public Rect ConvertToPixels(Rect rect)
        {
            return new Rect
            {
                top = ConvertToPixelsY(rect.top),
                left = ConvertToPixelsX(rect.left),
                right = ConvertToPixelsX(rect.right),
                bottom = ConvertToPixelsY(rect.bottom)
            };
        }
    }
}
