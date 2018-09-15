using System;
using System.Collections.Generic;
using System.Linq;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Layout
{
    internal class DefaultLayoutEngine : LayoutEngine
    {
        private Rect mLastRect;

        public override void Initialize(Control parent)
        {
            base.Initialize(parent);
            mLastRect = parent.NativeWindow.ClientRect;
        }

        public override void DoLayout(Control parent, IEnumerable<Control> children)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));
            if (children == null) throw new ArgumentNullException(nameof(children));

            using (DeferWindowPos dwp = new DeferWindowPos(children.Count()))
            {
                Rect currentRect = parent.NativeWindow.ClientRect;

                foreach (Control child in children)
                {
                    Rect itemRect = child.WindowRect;

                    if (child.Anchor.HasFlag(AnchorStyle.Left) && child.Anchor.HasFlag(AnchorStyle.Right))
                    {
                        itemRect.right += currentRect.right - mLastRect.right;
                    }
                    else if (child.Anchor.HasFlag(AnchorStyle.Left))
                    {
                        // This is the default.
                    }
                    else if (child.Anchor.HasFlag(AnchorStyle.Right))
                    {
                        itemRect.right += currentRect.right - mLastRect.right;
                        itemRect.left += currentRect.right - mLastRect.right;
                    }
                    else
                    {
                        int diff = ((currentRect.right - currentRect.left) / 2) - ((mLastRect.right - mLastRect.left) / 2);
                        itemRect.right += diff;
                        itemRect.left += diff;
                    }

                    if (child.Anchor.HasFlag(AnchorStyle.Top) && child.Anchor.HasFlag(AnchorStyle.Bottom))
                    {
                        itemRect.bottom += currentRect.bottom - mLastRect.bottom;
                    }
                    else if (child.Anchor.HasFlag(AnchorStyle.Top))
                    {
                        // This is the default.
                    }
                    else if (child.Anchor.HasFlag(AnchorStyle.Bottom))
                    {
                        itemRect.bottom += currentRect.bottom - mLastRect.bottom;
                        itemRect.top += currentRect.bottom - mLastRect.bottom;
                    }
                    else
                    {
                        int diff = ((currentRect.bottom - currentRect.top) / 2) - ((mLastRect.bottom - mLastRect.top) / 2);
                        itemRect.bottom += diff;
                        itemRect.top += diff;
                    }

                    if (child.Dock.HasFlag(AnchorStyle.Top)) itemRect.top = currentRect.top;
                    if (child.Dock.HasFlag(AnchorStyle.Left)) itemRect.left = currentRect.left;
                    if (child.Dock.HasFlag(AnchorStyle.Right)) itemRect.right = currentRect.right;
                    if (child.Dock.HasFlag(AnchorStyle.Bottom)) itemRect.bottom = currentRect.bottom;

                    dwp.AddControl(child, itemRect, MoveWindowFlags.DoNotActivate);
                }

                mLastRect = currentRect;
            }
        }
    }
}
