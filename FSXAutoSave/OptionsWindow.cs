﻿//
// P3DAutoSave options dialog
// Author: Jack Harkins
//

using System;
using System.Windows.Forms;

namespace P3DAutoSave
{
    public partial class OptionsWindow : Form
    {

        private P3DClient fsx;

        public OptionsWindow(P3DClient fsx)
        {
            InitializeComponent();
            this.fsx = fsx;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Console.WriteLine("Options window closed");
            this.Hide();
            e.Cancel = true;
        }

        private void checkBoxSaveWhilePaused_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSaveWhilePaused.Checked)
            {
                fsx.enableSaveWhilePaused();
            }
            else
            {
                fsx.disableSaveWhilePaused();
            }
        }

        private void selectorMaxNumSavesToKeep_ValueChanged(object sender, EventArgs e)
        {
            fsx.setMaxNumSavesToKeep((int)selectorMaxNumSavesToKeep.Value);
        }

        private void selectorSaveInterval_ValueChanged(object sender, EventArgs e)
        {
            fsx.setSaveInterval((int)selectorSaveInterval.Value);
        }

        public void loadSettings()
        {
            selectorSaveInterval.Value = Properties.Settings.Default.SaveInterval;
            selectorMaxNumSavesToKeep.Value = Properties.Settings.Default.MaxNumSaves;
            checkBoxSaveWhilePaused.Checked = Properties.Settings.Default.SaveWhilePaused;
            checkBoxAutosaveEnabledWhenFSXStarts.Checked = Properties.Settings.Default.SaveEnabledOnStart;
            checkBoxSaveWhileOnGround.Checked = Properties.Settings.Default.SaveWhileGround;
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            fsx.saveSettings();
        }

        private void checkBoxAutosaveEnabledWhenFSXStarts_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSaveWhilePaused.Checked)
            {
                fsx.enableAutoSaveOnP3DStart();
            }
            else
            {
                fsx.disableAutoSaveOnP3DStart();
            }
        }

        private void OptionsWindow_Load(object sender, EventArgs e)
        {

        }

        private void checkBoxSaveWhileOnGround_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
