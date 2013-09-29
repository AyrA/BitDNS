using BitDNS.Properties;
namespace BitDNS
{
    partial class frmAddressBook
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddressBook));
            this.label1 = new System.Windows.Forms.Label();
            this.lvEntries = new System.Windows.Forms.ListView();
            this.chLookup = new System.Windows.Forms.ColumnHeader();
            this.chAddress = new System.Windows.Forms.ColumnHeader();
            this.NI = new System.Windows.Forms.NotifyIcon(this.components);
            this.CMS = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showAddressBookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAdd = new System.Windows.Forms.Button();
            this.CMS.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(429, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // lvEntries
            // 
            this.lvEntries.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvEntries.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chLookup,
            this.chAddress});
            this.lvEntries.FullRowSelect = true;
            this.lvEntries.Location = new System.Drawing.Point(12, 66);
            this.lvEntries.Name = "lvEntries";
            this.lvEntries.Size = new System.Drawing.Size(460, 184);
            this.lvEntries.TabIndex = 2;
            this.lvEntries.UseCompatibleStateImageBehavior = false;
            this.lvEntries.View = System.Windows.Forms.View.Details;
            this.lvEntries.DoubleClick += new System.EventHandler(this.lvEntries_DoubleClick);
            this.lvEntries.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvEntries_KeyDown);
            // 
            // chLookup
            // 
            this.chLookup.Text = "Name";
            this.chLookup.Width = 150;
            // 
            // chAddress
            // 
            this.chAddress.Text = "Address";
            this.chAddress.Width = 280;
            // 
            // NI
            // 
            this.NI.ContextMenuStrip = this.CMS;
            this.NI.Icon = global::BitDNS.Properties.Resources.image_icon;
            this.NI.Text = "BitDNS";
            this.NI.Visible = true;
            this.NI.DoubleClick += new System.EventHandler(this.NI_DoubleClick);
            // 
            // CMS
            // 
            this.CMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showAddressBookToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.CMS.Name = "CMS";
            this.CMS.Size = new System.Drawing.Size(179, 48);
            // 
            // showAddressBookToolStripMenuItem
            // 
            this.showAddressBookToolStripMenuItem.Name = "showAddressBookToolStripMenuItem";
            this.showAddressBookToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.showAddressBookToolStripMenuItem.Text = "&Show Address book";
            this.showAddressBookToolStripMenuItem.Click += new System.EventHandler(this.showAddressBookToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(448, 37);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(24, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // frmAddressBook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 262);
            this.Controls.Add(this.lvEntries);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "frmAddressBook";
            this.Text = "Address book Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAddressBook_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAddressBook_KeyDown);
            this.ResizeEnd += new System.EventHandler(this.frmAddressBook_ResizeEnd);
            this.CMS.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvEntries;
        private System.Windows.Forms.ColumnHeader chLookup;
        private System.Windows.Forms.ColumnHeader chAddress;
        private System.Windows.Forms.NotifyIcon NI;
        private System.Windows.Forms.ContextMenuStrip CMS;
        private System.Windows.Forms.ToolStripMenuItem showAddressBookToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button btnAdd;
    }
}