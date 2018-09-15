using System.Collections.Generic;

namespace Sunburst.Win32UI.Layout
{
    public abstract class LayoutEngine
    {
        public virtual void Initialize(Control parent) { }
        public abstract void DoLayout(Control parent, IEnumerable<Control> children);
    }
}
