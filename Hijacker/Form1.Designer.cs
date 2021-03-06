﻿namespace Hijacker
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnOptions = new System.Windows.Forms.ToolStripDropDownButton();
            this.itemRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.itemLocations = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblSelected = new System.Windows.Forms.ToolStripLabel();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.panelProgress = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.panelData = new System.Windows.Forms.Panel();
            this.hijackItemsTable1 = new Hijacker.controls.HijackItemsTable();
            this.solutionTree1 = new Hijacker.controls.SolutionTree();
            this.columnName = new Hijacker.controls.Column();
            this.columnPath = new Hijacker.controls.Column();
            this.toolStrip.SuspendLayout();
            this.panelProgress.SuspendLayout();
            this.panelData.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Hijacker";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnIconClick);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOptions,
            this.toolStripSeparator1,
            this.lblSelected});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1060, 25);
            this.toolStrip.TabIndex = 6;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnOptions
            // 
            this.btnOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemRefresh,
            this.itemLocations});
            this.btnOptions.Image = global::Hijacker.Properties.Resources.Options;
            this.btnOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(78, 22);
            this.btnOptions.Text = "Options";
            // 
            // itemRefresh
            // 
            this.itemRefresh.Name = "itemRefresh";
            this.itemRefresh.Size = new System.Drawing.Size(134, 22);
            this.itemRefresh.Text = "Refresh";
            this.itemRefresh.Click += new System.EventHandler(this.OnRefresh);
            // 
            // itemLocations
            // 
            this.itemLocations.Name = "itemLocations";
            this.itemLocations.Size = new System.Drawing.Size(134, 22);
            this.itemLocations.Text = "Locations...";
            this.itemLocations.Click += new System.EventHandler(this.OnChooseLocations);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // lblSelected
            // 
            this.lblSelected.AutoSize = false;
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(125, 22);
            this.lblSelected.Text = "(Empty selection)";
            this.lblSelected.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblSelected.ToolTipText = "Click to reset selected dll";
            this.lblSelected.Click += new System.EventHandler(this.OnResetSelectedDll);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.OnDataLoad);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.OnDataProgress);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.OnDataLoaded);
            // 
            // panelProgress
            // 
            this.panelProgress.Controls.Add(this.progressBar);
            this.panelProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProgress.Location = new System.Drawing.Point(0, 0);
            this.panelProgress.Name = "panelProgress";
            this.panelProgress.Size = new System.Drawing.Size(1060, 493);
            this.panelProgress.TabIndex = 7;
            this.panelProgress.Paint += new System.Windows.Forms.PaintEventHandler(this.OnProgressPaint);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(3, 467);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1054, 23);
            this.progressBar.TabIndex = 0;
            // 
            // panelData
            // 
            this.panelData.Controls.Add(this.hijackItemsTable1);
            this.panelData.Controls.Add(this.solutionTree1);
            this.panelData.Controls.Add(this.toolStrip);
            this.panelData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelData.Location = new System.Drawing.Point(0, 0);
            this.panelData.Name = "panelData";
            this.panelData.Size = new System.Drawing.Size(1060, 493);
            this.panelData.TabIndex = 0;
            // 
            // hijackItemsTable1
            // 
            this.hijackItemsTable1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hijackItemsTable1.Location = new System.Drawing.Point(556, 25);
            this.hijackItemsTable1.Name = "hijackItemsTable1";
            this.hijackItemsTable1.Size = new System.Drawing.Size(492, 46);
            this.hijackItemsTable1.TabIndex = 7;
            // 
            // solutionTree1
            // 
            this.solutionTree1.AutoScroll = true;
            this.solutionTree1.Columns = new Hijacker.controls.Column[] {
        this.columnName,
        this.columnPath};
            this.solutionTree1.Dock = System.Windows.Forms.DockStyle.Left;
            this.solutionTree1.Location = new System.Drawing.Point(0, 25);
            this.solutionTree1.Name = "solutionTree1";
            this.solutionTree1.Size = new System.Drawing.Size(550, 468);
            this.solutionTree1.TabIndex = 0;
            this.solutionTree1.SelectionChanged += new System.EventHandler<Hijacker.controls.ITreeItem>(this.OnTreeSelectionChanged);
            // 
            // columnName
            // 
            this.columnName.Absolute = false;
            this.columnName.Name = "columnName";
            this.columnName.Property = "Name";
            this.columnName.Title = null;
            this.columnName.Width = 200F;
            // 
            // columnPath
            // 
            this.columnPath.Absolute = false;
            this.columnPath.Name = "columnPath";
            this.columnPath.Property = "Path";
            this.columnPath.Title = null;
            this.columnPath.Width = 350F;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1060, 493);
            this.Controls.Add(this.panelData);
            this.Controls.Add(this.panelProgress);
            this.Name = "Form1";
            this.Text = "Hijacker";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.panelProgress.ResumeLayout(false);
            this.panelData.ResumeLayout(false);
            this.panelData.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private controls.SolutionTree solutionTree1;
        private controls.Column columnName;
        private controls.Column columnPath;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Panel panelProgress;
        private System.Windows.Forms.Panel panelData;
        private controls.HijackItemsTable hijackItemsTable1;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel lblSelected;
        private System.Windows.Forms.ToolStripDropDownButton btnOptions;
        private System.Windows.Forms.ToolStripMenuItem itemRefresh;
        private System.Windows.Forms.ToolStripMenuItem itemLocations;
    }
}

