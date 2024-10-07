using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ClientBOT
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            TimeBeginPeriod(1);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod", CharSet = CharSet.Auto)]
        private static extern int TimeBeginPeriod(int msec);
    }
}
