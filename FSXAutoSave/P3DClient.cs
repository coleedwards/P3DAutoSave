//
// Prepar3D SimConnect client and autosave functionality
// Author: Jack Harkins
// Forked: Cole Edwards
//

using System;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;
using System.IO;
using LockheedMartin.Prepar3D.SimConnect;

namespace P3DAutoSave
{
    enum EVENTS
    {
        SIM_START,
        SIM_STOP,
        SIM_PAUSE,
        EVENT_MENU,
        EVENT_MENU_ENABLE_DISABLE,
        EVENT_MENU_OPTIONS
    }

    public class P3DClient
    {
        private SimConnect p3d; // The SimConnect client

        private int saveInterval; // seconds
        private int p3dVersion = 5;
        private const string FILENAME_BASE = "P3DAutoSave_";
        private string autoSavedFlightsPath;
        private int maxNumSavesToKeep;

        private bool simRunning = false;
        private bool simPaused = false;

        private bool saveEnabled;
        private bool canSaveWhilePaused;
        private bool autoSaveOnP3DStart;
        private System.Timers.Timer saveTimer;

        private SimConnectDummyWindow dummyWindow;
        private OptionsWindow optionsWindow;

        public P3DClient(SimConnectDummyWindow dummyWindow) {
            this.dummyWindow = dummyWindow;
            setupSimConnect();
            optionsWindow = new P3DAutoSave.OptionsWindow(this);
            optionsWindow.Visible = false;
            saveTimer = new System.Timers.Timer();
            saveTimer.Elapsed += new ElapsedEventHandler(saveGame);

            autoSavedFlightsPath = "C:\\Users\\" + Environment.UserName + "\\Documents\\Prepar3D v " + p3dVersion + " Files\\P3DAutoSave\\";

            loadSettings();

            p3d.MenuAddItem("P3DAutoSave", EVENTS.EVENT_MENU, 0);
            p3d.MenuAddSubItem(EVENTS.EVENT_MENU, "Enable/Disable", EVENTS.EVENT_MENU_ENABLE_DISABLE, 0);
            p3d.MenuAddSubItem(EVENTS.EVENT_MENU, "Options", EVENTS.EVENT_MENU_OPTIONS, 0);
        }

        public void resetSaveTimer()
        {
            saveTimer.Stop();
            saveTimer.Interval = 1000 * 60 * saveInterval;
            //saveTimer.Interval = 1000 * saveInterval;
            saveTimer.Start();
        }

        public void loadSettings()
        {
            saveInterval = Properties.Settings.Default.SaveInterval;
            maxNumSavesToKeep = Properties.Settings.Default.MaxNumSaves;
            canSaveWhilePaused = Properties.Settings.Default.SaveWhilePaused;
            autoSaveOnP3DStart = Properties.Settings.Default.SaveEnabledOnStart;

            saveEnabled = autoSaveOnP3DStart;

            resetSaveTimer();
            optionsWindow.loadSettings();
            Console.WriteLine("Settings loaded.");
            printSettings();
        }

        public void printSettings()
        {
            Console.WriteLine(string.Format("saveInterval: {0}, maxNumSaves: {1}, saveWhilePaused: {2}",
                Properties.Settings.Default.SaveInterval,
                Properties.Settings.Default.MaxNumSaves,
                Properties.Settings.Default.SaveWhilePaused));
        }

        public void saveSettings()
        {
            Properties.Settings.Default.SaveInterval = saveInterval;
            Properties.Settings.Default.MaxNumSaves = maxNumSavesToKeep;
            Properties.Settings.Default.SaveWhilePaused = canSaveWhilePaused;
            Properties.Settings.Default.SaveEnabledOnStart = autoSaveOnP3DStart;

            Properties.Settings.Default.Save();
            Console.WriteLine("Settings saved.");
            printSettings();
        }

        public void setupSimConnect()
        {
            try
            {
                p3d = new SimConnect("P3DAutoSave", dummyWindow.Handle, SimConnectDummyWindow.WM_USER_SIMCONNECT, null, 0);
            }
            catch (COMException e)
            {
                MessageBox.Show("Please start P3D before launching this application.");
                Environment.Exit(1);
            }

            // listen to quit msg
            p3d.OnRecvQuit += new SimConnect.RecvQuitEventHandler(p3d_OnRecvQuit);

            // listen to events
            p3d.OnRecvEvent += new SimConnect.RecvEventEventHandler(p3d_OnRecvEvent);

            // Subscribe to system events
            p3d.SubscribeToSystemEvent(EVENTS.SIM_START, "SimStart");
            p3d.SubscribeToSystemEvent(EVENTS.SIM_STOP, "SimStop");
            p3d.SubscribeToSystemEvent(EVENTS.SIM_PAUSE, "Pause");

            // Turn events on
            p3d.SetSystemEventState(EVENTS.SIM_START, SIMCONNECT_STATE.ON);
            p3d.SetSystemEventState(EVENTS.SIM_STOP, SIMCONNECT_STATE.ON);
            p3d.SetSystemEventState(EVENTS.SIM_PAUSE, SIMCONNECT_STATE.ON);
            Console.WriteLine("SimConnect initialized");
        }

        public void closeConnection()
        {
            if (p3d != null)
            {
                p3d.Dispose();
                p3d = null;
            }
        }

        // Simulator event handler
        public void p3d_OnRecvEvent(SimConnect sender, SIMCONNECT_RECV_EVENT recEvent)
        {
            switch (recEvent.uEventID)
            {
                case (uint)EVENTS.SIM_START:
                    Console.WriteLine("Sim started");
                    simRunning = true;
                    break;
                case (uint)EVENTS.SIM_STOP:
                    Console.WriteLine("Sim stopped");
                    simRunning = false;
                    break;
                case (uint)EVENTS.SIM_PAUSE:
                    if (recEvent.dwData == 0) // unpause
                    {
                        Console.WriteLine("Sim unpaused");
                        simPaused = false;
                    }
                    else if (recEvent.dwData == 1) // pause
                    {
                        Console.WriteLine("Sim paused");
                        simPaused = true;
                    }
                    break;
                case (uint)EVENTS.EVENT_MENU_ENABLE_DISABLE:
                    if (saveEnabled)
                    {
                        saveEnabled = false;
                        Console.WriteLine("Autosave disabled.");
                    }
                    else
                    {
                        saveEnabled = true;
                        Console.WriteLine("Autosave enabled.");
                    }
                    break;
                case (uint)EVENTS.EVENT_MENU_OPTIONS:
                    Console.WriteLine("Options menu pressed");
                    showOptionsWindow();
                    break;
            }
        }

        // Close P3D
        public void p3d_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            closeConnection();
            Application.Exit();
        }

        public void saveGame(object sender, EventArgs e)
        {
            if (saveEnabled && simRunning)
            {
                if (canSaveWhilePaused || (!canSaveWhilePaused && !simPaused))
                {
                    string currentTime = DateTime.Now.ToString();
                    // Filter slashes, colons, and spaces
                    currentTime = currentTime.Replace('/', '_').Replace(':', '_').Replace(' ', '_');

                    // Get files in directory and delete least recent one if number of files exceeds capacity
                    var fileArr = Directory.GetFiles(autoSavedFlightsPath);
                    Array.Sort(fileArr);

                    #if !DEBUG
                        string fullPath = "P3DAutoSave\\" + FILENAME_BASE + currentTime;
                        Console.WriteLine("Full path: " + fullPath);
                        fsx.FlightSave(fullPath, null, "P3DAutoSave autosaved flight", 0);
                    #endif
                    Console.WriteLine("Game saved: " + currentTime);
                }
            }
        }


        // events raised from options window or 'dummy' P3D event handler window
        public void receiveMessage()
        {
            if (p3d != null)
            {
                p3d.ReceiveMessage();
            }
        }

        public void showOptionsWindow()
        {
            optionsWindow.Visible = true;
        }

        public void hideOptionsWindow()
        {
            optionsWindow.Visible = false;
        }

        public void enableSaveWhilePaused()
        {
            canSaveWhilePaused = true;
            Console.WriteLine("Saving while paused ENABLED.");
        }

        public void disableSaveWhilePaused()
        {
            canSaveWhilePaused = false;
            Console.WriteLine("Saving while paused DISABLED.");
        }

        public void setSaveInterval(int minutes)
        {
            saveInterval = minutes; // seconds for now...
            resetSaveTimer();
            Console.WriteLine("Save interval changed to " + minutes + " seconds.");
        }

        public void setMaxNumSavesToKeep(int numSavesToKeep)
        {
            maxNumSavesToKeep = numSavesToKeep;
            Console.WriteLine("Max. number of saves changed to " + maxNumSavesToKeep);
        }

        public void enableAutoSaveOnP3DStart()
        {
            autoSaveOnP3DStart = true;
        }

        public void disableAutoSaveOnP3DStart()
        {
            autoSaveOnP3DStart = false;
        }

        public void setP3DVersion(int version)
        {
            this.p3dVersion = version;
            Console.WriteLine("P3D version changed to " + version);
        }
    }
}
