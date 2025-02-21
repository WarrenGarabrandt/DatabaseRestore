using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseRestoreGUI
{
    public partial class frmPassword : Form
    {
        public frmPassword()
        {
            InitializeComponent();
        }

        public bool CreateNewPassword = true;

        private void frmPassword_Shown(object sender, EventArgs e)
        {
            if (!CreateNewPassword)
            {
                lblConfirm.Visible = false;
                txtPass2.Visible = false;
                this.Height -= lblConfirm.Height + txtPass2.Height;
            }
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            if (CreateNewPassword)
            {
                if (txtPass1.Text != txtPass2.Text)
                {
                    lblError.Visible = true;
                }
                else
                {
                    lblError.Visible = false;
                }
            }
            if (lblError.Visible)
            {
                this.DialogResult = DialogResult.None;
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void txtPass2_TextChanged(object sender, EventArgs e)
        {
            if (txtPass1.Text != txtPass2.Text)
            {
                lblError.Visible = true;
            }
            else
            {
                lblError.Visible = false;
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult= DialogResult.Cancel;
            this.Close();
        }
    }
}
