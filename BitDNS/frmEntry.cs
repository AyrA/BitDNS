using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BitDNS
{
    public partial class frmEntry : Form
    {
        public frmEntry()
        {
            InitializeComponent();
        }

        public frmEntry(string l,string a)
        {
            InitializeComponent();
            tbLabel.Text = l;
            tbAddress.Text = a;
        }

        public string bmLabel
        {
            get
            {
                return tbLabel.Text;
            }
            set
            {
                tbLabel.Text = value;
            }
        }
        public string bmAddress
        {
            get
            {
                return tbAddress.Text;
            }
            set
            {
                tbAddress.Text = value;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult=DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
