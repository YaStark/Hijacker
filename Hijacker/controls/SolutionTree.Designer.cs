using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Hijacker.controls
{
    partial class SolutionTree
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.canvas = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.folderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.canvas.Location = new System.Drawing.Point(0, 0);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(203, 365);
            this.canvas.TabIndex = 0;
            this.canvas.TabStop = false;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.CanvasOnPaint);
            this.canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CanvasOnMouseDown);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openSolutionToolStripMenuItem,
            this.folderToolStripMenuItem,
            this.toolStripSeparator1,
            this.hideToolStripMenuItem,
            this.showAllToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(153, 120);
            // 
            // folderToolStripMenuItem
            // 
            this.folderToolStripMenuItem.Name = "folderToolStripMenuItem";
            this.folderToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.folderToolStripMenuItem.Text = "Open folder";
            this.folderToolStripMenuItem.Click += new System.EventHandler(this.OnLocation);
            // 
            // openSolutionToolStripMenuItem
            // 
            this.openSolutionToolStripMenuItem.Name = "openSolutionToolStripMenuItem";
            this.openSolutionToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.openSolutionToolStripMenuItem.Text = "Open solution";
            this.openSolutionToolStripMenuItem.Click += new System.EventHandler(this.OnOpenSln);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(146, 6);
            // 
            // hideToolStripMenuItem
            // 
            this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
            this.hideToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.hideToolStripMenuItem.Text = "Hide";
            this.hideToolStripMenuItem.Click += new System.EventHandler(this.OnHide);
            // 
            // showAllToolStripMenuItem
            // 
            this.showAllToolStripMenuItem.CheckOnClick = true;
            this.showAllToolStripMenuItem.Name = "showAllToolStripMenuItem";
            this.showAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.showAllToolStripMenuItem.Text = "Show all";
            this.showAllToolStripMenuItem.Click += new System.EventHandler(this.OnShowAll);
            // 
            // SolutionTree
            // 
            this.AutoScroll = true;
            this.Controls.Add(this.canvas);
            this.Name = "SolutionTree";
            this.Size = new System.Drawing.Size(206, 365);
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox canvas;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem folderToolStripMenuItem;
        private ToolStripMenuItem openSolutionToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem hideToolStripMenuItem;
        private ToolStripMenuItem showAllToolStripMenuItem;
    }
}
