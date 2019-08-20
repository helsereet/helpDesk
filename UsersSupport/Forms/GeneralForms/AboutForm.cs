using System;
using System.Windows.Forms;

namespace UsersSupport.Forms.GeneralForms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
