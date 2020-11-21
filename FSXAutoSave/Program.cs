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
            #if !DEBUG
                // Check if program was started by user or FSX
                if (args.Length != 1 || args[0] != "-f")
                {
                    MessageBox.Show("You have started P3DAutoSave outside of FSX (i.e. by double-clicking the "
                        + "executable file). Please be aware that running multiple instances of P3DAutoSave may "
                        + "result in strange behavior. The application will now close, but you may override this "
                        + "functionality by running P3DAutoSave with the -f command-line flag.");
                    Environment.Exit(1);
                }
            #endif

            // Setup debug console if in debug mode
            #if DEBUG
                AllocConsole();
            #endif

            // Setup and start the application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SimConnectDummyWindow form1 = new SimConnectDummyWindow();
            form1.Visible = false;


            P3DClient fsx = new P3DClient(form1);
            form1.registerSimConnect(fsx);
            Application.Run();
        }
    }
}
