using System;
using System.Windows.Forms;
using UsersSupport.Data;

namespace UsersSupport.Forms.GeneralForms
{
    public partial class ChangePassForm : Form
    {
        public MainForm MainForm { get; set; }

        public ChangePassForm(MainForm mainForm)
        {
            InitializeComponent();

            MainForm     = mainForm;
            AcceptButton = changePasswordButton;
            CancelButton = cancelButton;

            RenderForm();
        }

        private void RenderForm()
        {
            userNameLabel.Text = MainForm.User.UserName;
        }

        private void ChangePasswordButton_Click(object sender, EventArgs e)
        {
            if (FormHelper.AtLeastOneTxtBoxIsEmpty(currentPasswordTextBox, newPasswordTextBox, reEnterNewPasswordTextBox))
                MessageBox.Show("Заполните все поля", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (newPasswordTextBox.Text != reEnterNewPasswordTextBox.Text)
                MessageBox.Show("Введённые новые пароли должны совпадать!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                ChangePassword();
        }

        private void ChangePassword()
        {
            var db                     = new DataAccess();
            db.SuccessUpdatedPassword += Db_SuccessUpdatedPassword;
            db.PasswordDoesnotMatch   += Db_PasswordDoesnotMatch;
            db.UpdatePassword(MainForm.User.UserName, currentPasswordTextBox.Text, newPasswordTextBox.Text);
        }

        private void Db_PasswordDoesnotMatch(object sender, EventArgs e)
        {
            MessageBox.Show("Введённый текущий пароль неверен!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            currentPasswordTextBox.Text = "";
        }

        private void Db_SuccessUpdatedPassword(object sender, EventArgs e)
        {
            MessageBox.Show("Пароль был успешно обновлён", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}

