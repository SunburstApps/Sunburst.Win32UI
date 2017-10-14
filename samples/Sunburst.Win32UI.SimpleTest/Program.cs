using Sunburst.Win32UI;

namespace Sunburst.Win32UI.SimpleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageBox.Show("Inside Program.Main(), please attach debugger now.", "Win32UI Test", MessageBoxFlags.ButtonOK | MessageBoxFlags.IconInformation);
        }
    }
}
