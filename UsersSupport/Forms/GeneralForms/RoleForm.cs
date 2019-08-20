using System;
using System.Windows.Forms;
using UsersSupport.Data;

namespace UsersSupport.Forms.GeneralForms
{
    public partial class RoleForm : Form
    {
        public delegate void LoginBtnClickedEventHandler(object source, EventArgs e);

        public event LoginBtnClickedEventHandler LoginBtnClicked;

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            if (asAdminRdBtn.Checked)
                MainFrm.User.CurrentRoleId = 1;
            else if (asPerformerRdBtn.Checked)
                MainFrm.User.CurrentRoleId = 2;
            else
                MainFrm.User.CurrentRoleId = 3;
            MainFrm.User.CurrentRoleName = GetCurrentRoleName();
            LoginBtnClicked?.Invoke(this, e);
        }

        private string GetCurrentRoleName()
        {
            var db = new DataAccess();
            return db.GetCurrentRoleName(MainFrm.User.CurrentRoleId);
        }

        private MainForm MainFrm { get; }

        public RoleForm()
        {
            InitializeComponent();
            AcceptButton = loginBtn;
            CancelButton = cancelBtn;
        }

        public RoleForm(MainForm mainFrm) : this()
        {
            MainFrm = mainFrm;
            RenderForm();
        }

        private void RenderForm()
        {
            switch (MainFrm.User.RoleId)
            {
                case 1:
                    // admin role
                    break;
                case 2:
                    // performer role
                    Controls.Remove(asAdminRdBtn);
                    break;
                case 3:
                    // customer role
                    Controls.Remove(asAdminRdBtn);
                    Controls.Remove(asPerformerRdBtn);
                    break;
            }
        }
    }
}
