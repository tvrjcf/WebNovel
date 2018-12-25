using MiniBlinkPinvoke;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WebBookReader
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            BlinkBrowserPInvoke.ResourceAssemblys.Add("WebBookReader", System.Reflection.Assembly.GetExecutingAssembly());
            Application.Run(new frmMain());
        }
    }
}
