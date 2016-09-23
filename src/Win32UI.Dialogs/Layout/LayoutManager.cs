using System;
using System.Collections.Generic;
using Microsoft.Win32.UserInterface.Graphics;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Layout
{
    internal sealed class LayoutManagerItem
    {
        public Window Window { get; set; }
        public Rect Rect { get; set; }
        public LayoutPosition Position { get; set; }
    }

    public sealed class LayoutManager
    {
        private List<LayoutManagerItem> mControls = new List<LayoutManagerItem>();
        private Window mOwner = null;
        private Rect mLastRect = Rect.Zero;

        public LayoutManager(Window owner)
        {
            mOwner = owner;
            mLastRect = owner.ClientRect;
        }

        public void Add(Window window, LayoutPosition position)
        {
            Rect rect = window.WindowRect;
            Point pt = new Point(rect.left, rect.top);
            NativeMethods.ScreenToClient(mOwner.Handle, ref pt);
            rect.left = Convert.ToInt32(pt.x); rect.top = Convert.ToInt32(pt.y);
            pt = new Point(rect.right, rect.bottom);
            NativeMethods.ScreenToClient(mOwner.Handle, ref pt);
            rect.right = Convert.ToInt32(pt.x); rect.bottom = Convert.ToInt32(pt.y);

            mControls.Add(new LayoutManagerItem
            {
                Window = window,
                Rect = rect,
                Position = position
            });
        }

        public void Remove(Window window)
        {
            List<LayoutManagerItem> toRemove = new List<LayoutManagerItem>();
            foreach (var item in mControls)
            {
                if (item.Window.Handle.Equals(window.Handle))
                {
                    toRemove.Add(item);
                }
            }

            foreach (var item in toRemove) mControls.Remove(item);
        }

        public void SetPosition(Window window, LayoutPosition position)
        {
            foreach (var item in mControls)
            {
                if (item.Window.Handle.Equals(window.Handle)) item.Position = position;
            }
        }

        public void UpdateLocation(Window window)
        {
            Rect rect = window.WindowRect;
            Point pt = new Point(rect.left, rect.top);
            NativeMethods.ScreenToClient(mOwner.Handle, ref pt);
            rect.left = Convert.ToInt32(pt.x); rect.top = Convert.ToInt32(pt.y);
            pt = new Point(rect.right, rect.bottom);
            NativeMethods.ScreenToClient(mOwner.Handle, ref pt);
            rect.right = Convert.ToInt32(pt.x); rect.bottom = Convert.ToInt32(pt.y);

            foreach (var item in mControls)
            {
                if (item.Window.Handle.Equals(window.Handle)) item.Rect = rect;
            }
        }

        public void UpdateAllLocations()
        {
            foreach (LayoutManagerItem item in mControls) UpdateLocation(item.Window);
        }

        public void OnResize(bool repaint = true)
        {
            Rect currentRect = mOwner.ClientRect;
            IntPtr hDWP = NativeMethods.BeginDeferWindowPos(mControls.Count);
            foreach (var item in mControls)
            {
                Rect itemRect = item.Rect;

                if (item.Position.HasFlag(LayoutPosition.AnchorLeft) && item.Position.HasFlag(LayoutPosition.AnchorRight))
                {
                    itemRect.right += currentRect.right - mLastRect.right;
                }
                else if (item.Position.HasFlag(LayoutPosition.AnchorLeft))
                {
                    // This is the default.
                }
                else if (item.Position.HasFlag(LayoutPosition.AnchorRight))
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

                if (item.Position.HasFlag(LayoutPosition.AnchorTop) && item.Position.HasFlag(LayoutPosition.AnchorBottom))
                {
                    itemRect.bottom += currentRect.bottom - mLastRect.bottom;
                }
                else if (item.Position.HasFlag(LayoutPosition.AnchorTop))
                {
                    // This is the default.
                }
                else if (item.Position.HasFlag(LayoutPosition.AnchorBottom))
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

                if (item.Position.HasFlag(LayoutPosition.DockTop)) itemRect.top = currentRect.top;
                if (item.Position.HasFlag(LayoutPosition.DockLeft)) itemRect.left = currentRect.left;
                if (item.Position.HasFlag(LayoutPosition.DockRight)) itemRect.right = currentRect.right;
                if (item.Position.HasFlag(LayoutPosition.DockBottom)) itemRect.bottom = currentRect.bottom;

                item.Rect = itemRect;
                hDWP = NativeMethods.DeferWindowPos(hDWP, item.Window.Handle, IntPtr.Zero,
                    itemRect.left, itemRect.top, itemRect.Width, itemRect.Height, 8); // 8 == SWP_NOZORDER
            }

            NativeMethods.EndDeferWindowPos(hDWP);
            mLastRect = currentRect;
        }
    }
}
