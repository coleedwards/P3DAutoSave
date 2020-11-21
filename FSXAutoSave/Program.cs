using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;

namespace P3DAutoSave
{
    public class Program
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            // Setup debug console if in debug mode
            AllocConsole();

            // Setup and start the application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SimConnectDummyWindow form1 = new SimConnectDummyWindow();
            form1.Visible = false;

            P3DClient fsx = new P3DClient(form1);
            form1.registerSimConnect(fsx);
            Application.Run();

            MessageBox.Show("P3DAutoSave Succesfully Started");
        }
        
        public static void showConsole()
        {
            AllocConsole();
        }
    }
}
