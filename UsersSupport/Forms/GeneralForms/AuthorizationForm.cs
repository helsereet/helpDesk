using System;
using System.Windows.Forms;
using UsersSupport.Data;

namespace UsersSupport.Forms.GeneralForms
{
    public partial class AuthorizationForm : Form
    {
        private MainForm MainFrm { get; }

        public AuthorizationForm()
        {
            InitializeComponent();
            AcceptButton = AuthorizeBtn;
        }

        public AuthorizationForm(MainForm mainFrm) : this()
        {
            MainFrm = mainFrm;
        }

        private void AuthorizeBtn_Click(object sender, EventArgs e)
        {
            // 1. All fields not empty
            // 2. Request to db (select userName, roleName, where userName = userNameTxtBox.Text AND Password = passTxtBox.Text)
            // 3. If not row returned then alert "That user not exist". Return to step 1
            // 4. Else: If roleName = "Customer" -> goTo MainForm
            // 5.       else if roleName = "Performer" -> show 2 rdBtn
            //          show 3 rdBtn(roleName = "Admin")
            // 6. Set role (Prop), and seanseRole (Prop) in MainForm
            if (AtLeastOneFieldIsEmpty())
            {
                MessageBox.Show("Необходимо заполнить все поля", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var db   = new DataAccess();
            var user = db.GetUser(userNameTxtBox.Text, passwordTxtBox.Text);
            if (user is null)
            {
                MessageBox.Show("Пользователь/пароль не подходят!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MainFrm.User = user;
            OpenRoleForm();
        }

        private void OpenRoleForm()
        {
            var roleFrm              = new RoleForm(MainFrm);
            roleFrm.LoginBtnClicked += RoleFrm_LoginBtnClicked;
            roleFrm.ShowDialog();
        }

        public delegate void AuthorizeSuccessEventHandler(object source, EventArgs e);

        public event AuthorizeSuccessEventHandler AuthorizeSuccess;

        private void RoleFrm_LoginBtnClicked(object source, EventArgs e)
        {
            ((RoleForm)source).Close();
            AuthorizeSuccess?.Invoke(this, e);
        }

        private bool AtLeastOneFieldIsEmpty()
        {
            return string.IsNullOrWhiteSpace(userNameTxtBox.Text) || string.IsNullOrWhiteSpace(passwordTxtBox.Text);
        }
    }
}

