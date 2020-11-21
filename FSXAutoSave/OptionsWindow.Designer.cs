namespace P3DAutoSave
{
    partial class OptionsWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelMaxNumSavesToKeep = new System.Windows.Forms.Label();
            this.selectorMaxNumSavesToKeep = new System.Windows.Forms.NumericUpDown();
            this.labelSaveInterval = new System.Windows.Forms.Label();
            this.selectorSaveInterval = new System.Windows.Forms.NumericUpDown();
            this.checkBoxSaveWhilePaused = new System.Windows.Forms.CheckBox();
            this.buttonSaveSettings = new System.Windows.Forms.Button();
            this.checkBoxAutosaveEnabledWhenFSXStarts = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.p3dVersionSelector = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.selectorMaxNumSavesToKeep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectorSaveInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p3dVersionSelector)).BeginInit();
            this.SuspendLayout();
            // 
            // labelMaxNumSavesToKeep
            // 
            this.labelMaxNumSavesToKeep.AutoSize = true;
            this.labelMaxNumSavesToKeep.Location = new System.Drawing.Point(12, 9);
            this.labelMaxNumSavesToKeep.Name = "labelMaxNumSavesToKeep";
            this.labelMaxNumSavesToKeep.Size = new System.Drawing.Size(174, 13);
            this.labelMaxNumSavesToKeep.TabIndex = 0;
            this.labelMaxNumSavesToKeep.Text = "Maximum number of saves to keep:";
            // 
            // selectorMaxNumSavesToKeep
            // 
            this.selectorMaxNumSavesToKeep.Location = new System.Drawing.Point(192, 7);
            this.selectorMaxNumSavesToKeep.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.selectorMaxNumSavesToKeep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.selectorMaxNumSavesToKeep.Name = "selectorMaxNumSavesToKeep";
            this.selectorMaxNumSavesToKeep.Size = new System.Drawing.Size(50, 20);
            this.selectorMaxNumSavesToKeep.TabIndex = 1;
            this.selectorMaxNumSavesToKeep.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.selectorMaxNumSavesToKeep.ValueChanged += new System.EventHandler(this.selectorMaxNumSavesToKeep_ValueChanged);
            // 
            // labelSaveInterval
            // 
            this.labelSaveInterval.AutoSize = true;
            this.labelSaveInterval.Location = new System.Drawing.Point(12, 35);
            this.labelSaveInterval.Name = "labelSaveInterval";
            this.labelSaveInterval.Size = new System.Drawing.Size(117, 13);
            this.labelSaveInterval.TabIndex = 2;
            this.labelSaveInterval.Text = "Save interval (minutes):";
            // 
            // selectorSaveInterval
            // 
            this.selectorSaveInterval.Location = new System.Drawing.Point(192, 33);
            this.selectorSaveInterval.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.selectorSaveInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.selectorSaveInterval.Name = "selectorSaveInterval";
            this.selectorSaveInterval.Size = new System.Drawing.Size(50, 20);
            this.selectorSaveInterval.TabIndex = 3;
            this.selectorSaveInterval.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.selectorSaveInterval.ValueChanged += new System.EventHandler(this.selectorSaveInterval_ValueChanged);
            // 
            // checkBoxSaveWhilePaused
            // 
            this.checkBoxSaveWhilePaused.AutoSize = true;
            this.checkBoxSaveWhilePaused.Location = new System.Drawing.Point(12, 90);
            this.checkBoxSaveWhilePaused.Name = "checkBoxSaveWhilePaused";
            this.checkBoxSaveWhilePaused.Size = new System.Drawing.Size(116, 17);
            this.checkBoxSaveWhilePaused.TabIndex = 4;
            this.checkBoxSaveWhilePaused.Text = "Save while paused";
            this.checkBoxSaveWhilePaused.UseVisualStyleBackColor = true;
            this.checkBoxSaveWhilePaused.CheckedChanged += new System.EventHandler(this.checkBoxSaveWhilePaused_CheckedChanged);
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Location = new System.Drawing.Point(76, 136);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(100, 23);
            this.buttonSaveSettings.TabIndex = 5;
            this.buttonSaveSettings.Text = "Save Settings";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            this.buttonSaveSettings.Click += new System.EventHandler(this.buttonSaveSettings_Click);
            // 
            // checkBoxAutosaveEnabledWhenFSXStarts
            // 
            this.checkBoxAutosaveEnabledWhenFSXStarts.AutoSize = true;
            this.checkBoxAutosaveEnabledWhenFSXStarts.Location = new System.Drawing.Point(12, 113);
            this.checkBoxAutosaveEnabledWhenFSXStarts.Name = "checkBoxAutosaveEnabledWhenFSXStarts";
            this.checkBoxAutosaveEnabledWhenFSXStarts.Size = new System.Drawing.Size(216, 17);
            this.checkBoxAutosaveEnabledWhenFSXStarts.TabIndex = 6;
            this.checkBoxAutosaveEnabledWhenFSXStarts.Text = "Autosave auto-enabled when FSX starts";
            this.checkBoxAutosaveEnabledWhenFSXStarts.UseVisualStyleBackColor = true;
            this.checkBoxAutosaveEnabledWhenFSXStarts.CheckedChanged += new System.EventHandler(this.checkBoxAutosaveEnabledWhenFSXStarts_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Prepar3D version:";
            // 
            // p3dVersionSelector
            // 
            this.p3dVersionSelector.Location = new System.Drawing.Point(192, 61);
            this.p3dVersionSelector.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.p3dVersionSelector.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.p3dVersionSelector.Name = "p3dVersionSelector";
            this.p3dVersionSelector.Size = new System.Drawing.Size(50, 20);
            this.p3dVersionSelector.TabIndex = 8;
            this.p3dVersionSelector.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.p3dVersionSelector.ValueChanged += new System.EventHandler(this.p3dVersionSelector_ValueChanged);
            // 
            // OptionsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 171);
            this.Controls.Add(this.p3dVersionSelector);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxAutosaveEnabledWhenFSXStarts);
            this.Controls.Add(this.buttonSaveSettings);
            this.Controls.Add(this.checkBoxSaveWhilePaused);
            this.Controls.Add(this.selectorSaveInterval);
            this.Controls.Add(this.labelSaveInterval);
            this.Controls.Add(this.selectorMaxNumSavesToKeep);
            this.Controls.Add(this.labelMaxNumSavesToKeep);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsWindow";
            this.Text = "P3DAutoSave Options";
            this.Load += new System.EventHandler(this.OptionsWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.selectorMaxNumSavesToKeep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectorSaveInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p3dVersionSelector)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelMaxNumSavesToKeep;
        private System.Windows.Forms.NumericUpDown selectorMaxNumSavesToKeep;
        private System.Windows.Forms.Label labelSaveInterval;
        private System.Windows.Forms.NumericUpDown selectorSaveInterval;
        private System.Windows.Forms.CheckBox checkBoxSaveWhilePaused;
        private System.Windows.Forms.Button buttonSaveSettings;
        private System.Windows.Forms.CheckBox checkBoxAutosaveEnabledWhenFSXStarts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown p3dVersionSelector;
    }
}