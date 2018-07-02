using System;
using Sunburst.Win32UI;

namespace Win32UI.Samples.HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageBox.Show("Hello, World!", "Win32UI Hello World", MessageBoxFlags.ButtonOK | MessageBoxFlags.IconInformation);
        }
    }
}
