using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BitDNS
{
    public partial class frmAddressBook : Form
    {
        public frmAddressBook()
        {
            InitializeComponent();
            NI.ShowBalloonTip(10000, "BitDNS launched", "BitDNS has been started and will close itself with bitmessage when done", ToolTipIcon.Info);
            BMA[] AA = AddressBook.Addresses;
            foreach (BMA A in AA)
            {
                lvEntries.Items.Add(A.Label).SubItems.Add(A.Address);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void NI_DoubleClick(object sender, EventArgs e)
        {
            Show();
        }

        private void frmAddressBook_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
            else
            {
                NI.Dispose();
            }
        }

        private void frmAddressBook_ResizeEnd(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.Hide();
            }
        }

        private void showAddressBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void frmAddressBook_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Insert)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                addEntry();
            }
        }

        private void addEntry()
        {
            frmEntry E = new frmEntry();
            if (E.ShowDialog(this) == DialogResult.OK)
            {
                lvEntries.Items.Add(E.bmLabel).SubItems.Add(E.bmAddress);
                AddressBook.Add(new BMA(E.bmLabel, E.bmAddress));
            }
            E.Dispose();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            addEntry();
        }

        private void lvEntries_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && lvEntries.SelectedItems.Count > 0)
            {
                if (MessageBox.Show(string.Format("Delete {0} entr{1}?", lvEntries.SelectedItems.Count, lvEntries.SelectedItems.Count != 1 ? "ies" : "y"), "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    lvEntries.SuspendLayout();
                    while (lvEntries.SelectedItems.Count > 0)
                    {
                        AddressBook.RemoveAt(lvEntries.SelectedItems[0].Index);
                        lvEntries.SelectedItems[0].Remove();
                    }
                    lvEntries.ResumeLayout();
                }
            }
        }

        private void lvEntries_DoubleClick(object sender, EventArgs e)
        {
            if (lvEntries.SelectedItems.Count > 0)
            {
                int i = lvEntries.SelectedItems[0].Index;
                frmEntry E = new frmEntry(AddressBook.Addresses[i].Label, AddressBook.Addresses[i].Address);
                if (E.ShowDialog(this) == DialogResult.OK)
                {
                    lvEntries.Items[i].Text = E.bmLabel;
                    lvEntries.Items[i].SubItems[lvEntries.Items[i].SubItems.Count - 1].Text = E.bmAddress;
                    BMA[] Temp = AddressBook.Addresses;
                    Temp[i].Address = E.bmAddress;
                    Temp[i].Label = E.bmLabel;
                    AddressBook.Addresses = Temp;
                    Temp = null;
                }
                E.Dispose();
            }
        }
    }
}
