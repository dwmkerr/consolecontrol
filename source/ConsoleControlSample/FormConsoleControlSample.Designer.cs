namespace ConsoleControlSample
{
  partial class FormConsoleControlSample
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConsoleControlSample));
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelConsoleState = new System.Windows.Forms.ToolStripStatusLabel();
            this.consoleControl = new ConsoleControl.ConsoleControl();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonRunCMD = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonNewProcess = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStopProcess = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonShowDiagnostics = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonInputEnabled = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSendKeyboardCommandsToProcess = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonClearOutput = new System.Windows.Forms.ToolStripButton();
            this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
            this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.BottomToolStripPanel
            // 
            this.toolStripContainer.BottomToolStripPanel.Controls.Add(this.statusStrip);
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.consoleControl);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(784, 514);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(784, 561);
            this.toolStripContainer.TabIndex = 1;
            this.toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip);
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabelConsoleState});
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(784, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(82, 17);
            this.toolStripStatusLabel1.Text = "Console State:";
            // 
            // toolStripStatusLabelConsoleState
            // 
            this.toolStripStatusLabelConsoleState.Name = "toolStripStatusLabelConsoleState";
            this.toolStripStatusLabelConsoleState.Size = new System.Drawing.Size(75, 17);
            this.toolStripStatusLabelConsoleState.Text = "Not Running";
            // 
            // consoleControl
            // 
            this.consoleControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consoleControl.IsInputEnabled = true;
            this.consoleControl.Location = new System.Drawing.Point(0, 0);
            this.consoleControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.consoleControl.Name = "consoleControl";
            this.consoleControl.SendKeyboardCommandsToProcess = false;
            this.consoleControl.ShowDiagnostics = false;
            this.consoleControl.Size = new System.Drawing.Size(784, 514);
            this.consoleControl.TabIndex = 0;
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonRunCMD,
            this.toolStripSeparator2,
            this.toolStripButtonNewProcess,
            this.toolStripButtonStopProcess,
            this.toolStripSeparator1,
            this.toolStripButtonShowDiagnostics,
            this.toolStripButtonInputEnabled,
            this.toolStripButtonSendKeyboardCommandsToProcess,
            this.toolStripSeparator3,
            this.toolStripButtonClearOutput});
            this.toolStrip.Location = new System.Drawing.Point(3, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(191, 25);
            this.toolStrip.TabIndex = 0;
            // 
            // toolStripButtonRunCMD
            // 
            this.toolStripButtonRunCMD.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRunCMD.Image = global::ConsoleControlSample.Properties.Resources.ConsoleControl;
            this.toolStripButtonRunCMD.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRunCMD.Name = "toolStripButtonRunCMD";
            this.toolStripButtonRunCMD.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRunCMD.Text = "Run Command Prompt";
            this.toolStripButtonRunCMD.Click += new System.EventHandler(this.toolStripButtonRunCMD_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonNewProcess
            // 
            this.toolStripButtonNewProcess.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNewProcess.Image = global::ConsoleControlSample.Properties.Resources.Play;
            this.toolStripButtonNewProcess.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNewProcess.Name = "toolStripButtonNewProcess";
            this.toolStripButtonNewProcess.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNewProcess.Text = "Start a New Process";
            this.toolStripButtonNewProcess.Click += new System.EventHandler(this.toolStripButtonNewProcess_Click);
            // 
            // toolStripButtonStopProcess
            // 
            this.toolStripButtonStopProcess.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonStopProcess.Image = global::ConsoleControlSample.Properties.Resources.Stop;
            this.toolStripButtonStopProcess.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStopProcess.Name = "toolStripButtonStopProcess";
            this.toolStripButtonStopProcess.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonStopProcess.Text = "Stop Process";
            this.toolStripButtonStopProcess.Click += new System.EventHandler(this.toolStripButtonStopProcess_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonShowDiagnostics
            // 
            this.toolStripButtonShowDiagnostics.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonShowDiagnostics.Image = global::ConsoleControlSample.Properties.Resources.Information;
            this.toolStripButtonShowDiagnostics.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowDiagnostics.Name = "toolStripButtonShowDiagnostics";
            this.toolStripButtonShowDiagnostics.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonShowDiagnostics.Text = "Show Diagnostics";
            this.toolStripButtonShowDiagnostics.Click += new System.EventHandler(this.toolStripButtonShowDiagnostics_Click);
            // 
            // toolStripButtonInputEnabled
            // 
            this.toolStripButtonInputEnabled.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonInputEnabled.Image = global::ConsoleControlSample.Properties.Resources.Control_TextBox;
            this.toolStripButtonInputEnabled.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonInputEnabled.Name = "toolStripButtonInputEnabled";
            this.toolStripButtonInputEnabled.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonInputEnabled.Text = "Input Enabled";
            this.toolStripButtonInputEnabled.Click += new System.EventHandler(this.toolStripButtonInputEnabled_Click);
            // 
            // toolStripButtonSendKeyboardCommandsToProcess
            // 
            this.toolStripButtonSendKeyboardCommandsToProcess.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSendKeyboardCommandsToProcess.Image = global::ConsoleControlSample.Properties.Resources.GotoShortcuts;
            this.toolStripButtonSendKeyboardCommandsToProcess.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSendKeyboardCommandsToProcess.Name = "toolStripButtonSendKeyboardCommandsToProcess";
            this.toolStripButtonSendKeyboardCommandsToProcess.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSendKeyboardCommandsToProcess.Text = "Send Keyboard Commands to Process";
            this.toolStripButtonSendKeyboardCommandsToProcess.Click += new System.EventHandler(this.toolStripButtonSendKeyboardCommandsToProcess_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonClearOutput
            // 
            this.toolStripButtonClearOutput.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonClearOutput.Image = global::ConsoleControlSample.Properties.Resources.Delete;
            this.toolStripButtonClearOutput.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClearOutput.Name = "toolStripButtonClearOutput";
            this.toolStripButtonClearOutput.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonClearOutput.Text = "Clear Output";
            this.toolStripButtonClearOutput.Click += new System.EventHandler(this.toolStripButtonClearOutput_Click);
            // 
            // timerUpdateUI
            // 
            this.timerUpdateUI.Enabled = true;
            this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
            // 
            // FormConsoleControlSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.toolStripContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormConsoleControlSample";
            this.Text = "Console";
            this.Load += new System.EventHandler(this.FormConsoleControlSample_Load);
            this.toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);

    }

    #endregion

    private ConsoleControl.ConsoleControl consoleControl;
    private System.Windows.Forms.ToolStripContainer toolStripContainer;
    private System.Windows.Forms.StatusStrip statusStrip;
    private System.Windows.Forms.ToolStrip toolStrip;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    private System.Windows.Forms.ToolStripButton toolStripButtonNewProcess;
    private System.Windows.Forms.ToolStripButton toolStripButtonStopProcess;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelConsoleState;
    private System.Windows.Forms.ToolStripButton toolStripButtonShowDiagnostics;
    private System.Windows.Forms.Timer timerUpdateUI;
    private System.Windows.Forms.ToolStripButton toolStripButtonInputEnabled;
    private System.Windows.Forms.ToolStripButton toolStripButtonRunCMD;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripButton toolStripButtonClearOutput;
    private System.Windows.Forms.ToolStripButton toolStripButtonSendKeyboardCommandsToProcess;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
  }
}

