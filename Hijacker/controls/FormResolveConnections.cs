using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Hijacker.controls
{
    public class FormResolveConnections : Form
    {
        private readonly List<Control> m_left = new List<Control>();
        private readonly List<Control> m_right = new List<Control>();

        public FormResolveConnections(Control[] left, Control[] right)
        {
            InitializeComponent();
            buttonLeft.Click += ButtonLeftOnClick;
            buttonRight.Click += ButtonRightOnClick;
            listBoxLeft.SelectedValueChanged += OnListSelectionChanged;
            listBoxRight.SelectedValueChanged += OnListSelectionChanged;
            m_left.AddRange(left);
            m_right.AddRange(right);
            UpdateView();
        }

        private void ButtonRightOnClick(object sender, EventArgs eventArgs)
        {
            var toRight = listBoxLeft.SelectedItems.OfType<Control>();
            listBoxLeft.ClearSelected();
            m_left.RemoveAll(toRight.Contains);
            m_right.AddRange(toRight);
            UpdateView();
        }

        private void ButtonLeftOnClick(object sender, EventArgs eventArgs)
        {
            var toLeft = listBoxRight.SelectedItems.OfType<Control>();
            listBoxRight.ClearSelected();
            m_right.RemoveAll(toLeft.Contains);
            m_left.AddRange(toLeft);
            UpdateView();
        }

        private void OnListSelectionChanged(object sender, EventArgs e)
        {
            UpdateView();
        }

        public Control[] GetLeft()
        {
            return m_left.ToArray();
        }

        public Control[] GetRight()
        {
            return m_right.ToArray();
        }

        private void UpdateView()
        {
            listBoxLeft.Items.Clear();
            listBoxLeft.Items.AddRange(GetLeft());
            listBoxRight.Items.Clear();
            listBoxRight.Items.AddRange(GetRight());
            buttonRight.Enabled = m_left.Count > 0 && listBoxLeft.SelectedItems.Count > 0;
            buttonLeft.Enabled = m_right.Count > 0 && listBoxRight.SelectedItems.Count > 0;
        }

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelRight = new System.Windows.Forms.GroupBox();
            this.listBoxRight = new System.Windows.Forms.ListBox();
            this.panelLeft = new System.Windows.Forms.GroupBox();
            this.listBoxLeft = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonLeft = new System.Windows.Forms.Button();
            this.buttonRight = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panelRight, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelLeft, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(411, 197);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.listBoxRight);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(224, 3);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(184, 159);
            this.panelRight.TabIndex = 5;
            this.panelRight.TabStop = false;
            this.panelRight.Text = "Right";
            // 
            // listBoxRight
            // 
            this.listBoxRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxRight.FormattingEnabled = true;
            this.listBoxRight.Location = new System.Drawing.Point(3, 16);
            this.listBoxRight.Name = "listBoxRight";
            this.listBoxRight.Size = new System.Drawing.Size(178, 140);
            this.listBoxRight.TabIndex = 1;
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.listBoxLeft);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeft.Location = new System.Drawing.Point(3, 3);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(183, 159);
            this.panelLeft.TabIndex = 4;
            this.panelLeft.TabStop = false;
            this.panelLeft.Text = "Left";
            // 
            // listBoxLeft
            // 
            this.listBoxLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxLeft.FormattingEnabled = true;
            this.listBoxLeft.Location = new System.Drawing.Point(3, 16);
            this.listBoxLeft.Name = "listBoxLeft";
            this.listBoxLeft.Size = new System.Drawing.Size(177, 140);
            this.listBoxLeft.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonLeft);
            this.panel1.Controls.Add(this.buttonRight);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(192, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(26, 159);
            this.panel1.TabIndex = 2;
            // 
            // buttonLeft
            // 
            this.buttonLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLeft.Location = new System.Drawing.Point(0, 87);
            this.buttonLeft.Name = "buttonLeft";
            this.buttonLeft.Size = new System.Drawing.Size(26, 26);
            this.buttonLeft.TabIndex = 1;
            this.buttonLeft.Text = "<";
            this.buttonLeft.UseVisualStyleBackColor = false;
            // 
            // buttonRight
            // 
            this.buttonRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRight.Location = new System.Drawing.Point(0, 55);
            this.buttonRight.Name = "buttonRight";
            this.buttonRight.Size = new System.Drawing.Size(26, 26);
            this.buttonRight.TabIndex = 0;
            this.buttonRight.Text = ">";
            this.buttonRight.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel2, 3);
            this.panel2.Controls.Add(this.buttonOk);
            this.panel2.Controls.Add(this.buttonCancel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 168);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(405, 26);
            this.panel2.TabIndex = 3;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOk.Location = new System.Drawing.Point(200, 0);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(99, 26);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = false;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Location = new System.Drawing.Point(306, 0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(99, 26);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            // 
            // FormResolveConnections
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 197);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormResolveConnections";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FormResolveConnections";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private Button buttonRight;
        private Button buttonLeft;
        private Panel panel2;
        private Button buttonCancel;
        private Button buttonOk;
        private GroupBox panelLeft;
        private GroupBox panelRight;
        private ListBox listBoxRight;
        private ListBox listBoxLeft;
    }
}
