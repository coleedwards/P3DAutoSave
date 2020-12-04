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
using System.Diagnostics;

namespace P3DAutoSave
{
    enum EVENTS
    {
        SIM_START,
        SIM_STOP,
        SIM_PAUSE,
        RECEIVE_DATA,
        EVENT_MENU,
        EVENT_MENU_ENABLE_DISABLE,
        EVENT_MENU_OPTIONS,
        EVENT_MENU_SAVE,
        EVENT_MENU_CONSOLE
    }

    enum VARIABLES
    {
        ON_GROUND
    }

    public class P3DClient
    { 

        private SimConnect p3d; // The SimConnect client

        private int saveInterval; // seconds
        private int maxNumSavesToKeep;

        private bool simRunning = true;
        private bool simPaused = false;

        private bool saveEnabled = true;
        private bool canSaveWhilePaused;
        private bool autoSaveOnP3DStart;
        private bool canSaveWhileGround;
        private System.Timers.Timer saveTimer;

        private SimConnectDummyWindow dummyWindow;
        private OptionsWindow optionsWindow;

        private bool onGround = true;

        public P3DClient(SimConnectDummyWindow dummyWindow) {
            this.dummyWindow = dummyWindow;
            setupSimConnect();
            optionsWindow = new P3DAutoSave.OptionsWindow(this);
            optionsWindow.Visible = false;
            saveTimer = new System.Timers.Timer();
            saveTimer.Elapsed += new ElapsedEventHandler(saveGame);

            loadSettings();

            p3d.MenuAddItem("P3DAutoSave", EVENTS.EVENT_MENU, 0);
            p3d.MenuAddSubItem(EVENTS.EVENT_MENU, "Enable/Disable", EVENTS.EVENT_MENU_ENABLE_DISABLE, 0);
            p3d.MenuAddSubItem(EVENTS.EVENT_MENU, "Options", EVENTS.EVENT_MENU_OPTIONS, 0);
            p3d.MenuAddSubItem(EVENTS.EVENT_MENU, "Save", EVENTS.EVENT_MENU_SAVE, 0);
            p3d.MenuAddSubItem(EVENTS.EVENT_MENU, "Open Console", EVENTS.EVENT_MENU_CONSOLE, 0);
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
            canSaveWhileGround = Properties.Settings.Default.SaveWhileGround;

            saveEnabled = autoSaveOnP3DStart;

            resetSaveTimer();
            optionsWindow.loadSettings();
            Console.WriteLine("Settings loaded.");
            printSettings();
        }

        public void printSettings()
        {
            Console.WriteLine(string.Format("saveInterval: {0}, maxNumSaves: {1}, saveWhilePaused: {2}, saveWhileGround: {3}",
                Properties.Settings.Default.SaveInterval,
                Properties.Settings.Default.MaxNumSaves,
                Properties.Settings.Default.SaveWhilePaused,
                Properties.Settings.Default.SaveWhileGround));
        }

        public void saveSettings()
        {
            Properties.Settings.Default.SaveInterval = saveInterval;
            Properties.Settings.Default.MaxNumSaves = maxNumSavesToKeep;
            Properties.Settings.Default.SaveWhilePaused = canSaveWhilePaused;
            Properties.Settings.Default.SaveEnabledOnStart = autoSaveOnP3DStart;
            Properties.Settings.Default.SaveWhileGround = canSaveWhileGround;

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
                MessageBox.Show(e.Message);
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

            // SimConnect variables
            p3d.AddToDataDefinition(VARIABLES.ON_GROUND, "SIM ON GROUND", "Bool", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
            p3d.RequestDataOnSimObject(EVENTS.RECEIVE_DATA, VARIABLES.ON_GROUND, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_PERIOD.SECOND, 0, 0, 0, 0);

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
                case (uint)EVENTS.EVENT_MENU_SAVE:
                    Console.WriteLine("Save menu pressed");
                    saveGame(null, null);
                    break;
                case (uint)EVENTS.EVENT_MENU_CONSOLE:
                    Program.showConsole();
                    break;
                case (uint)EVENTS.RECEIVE_DATA:
                    // TODO: Recode this section as I might need to grab more data at a later time
                    onGround = recEvent.dwData==1;
                    Console.WriteLine("ground status changed");
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
                    if (!canSaveWhileGround && onGround)
                    {
                        return;
                    }

                    string time = DateTime.Now.ToString();
                    try
                    {
                        p3d.FlightSave("save-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second, "AutoSave " + DateTime.Now.DayOfWeek.ToString() + " " + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) + " (saved by P3DAutosave)", "P3DAutoSave autosaved flight", 0);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("AutoSave failed. See console for more information.");
                        Console.WriteLine(exception.StackTrace);
                    }
                    Console.WriteLine("Saved at " + time);
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
            optionsWindow.Show();
        }

        public void hideOptionsWindow()
        {
            optionsWindow.Visible = false;
            optionsWindow.Hide();
        }

        public void enableSaveWhilePaused()
        {
            canSaveWhilePaused = true;
            Console.WriteLine("Saving while paused ENABLED");
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

        public void toggleSaveOnGround()
        {
            canSaveWhileGround = !canSaveWhileGround;
        }
        
    }
}
