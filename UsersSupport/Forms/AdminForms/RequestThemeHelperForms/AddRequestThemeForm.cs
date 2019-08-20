using System;
using System.Windows.Forms;
using UsersSupport.Data;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.AdminForms.RequestThemeHelperForms
{
    public class AddRequestThemeForm : SharedRequestThemeForm
    {
        public AddRequestThemeForm(MainForm mainForm) : base(mainForm)
        {
            RenderForm();
            leftBtn.Click += AddRequestThemeBtn_Click;
        }

        private void RenderForm()
        {
            lbl.Text      = "Введите название новой темы заявки";
            leftBtn.Image = imageList1.Images["addImg"];
        }

        private void AddRequestThemeBtn_Click(object sender, EventArgs e)
        {
            ExceptionOccured = false;
            if (FormHelper.AtLeastOneTxtBoxIsEmpty(txtBox))
            {
                MessageBox.Show("Поле не должно быть пустым", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var db                         = new DataAccess();
            db.SuccessRecordInsert        += SuccessAddedRequestTheme;
            db.RequestThemeExistException += Db_RequestThemeExistException;
            db.SimpleInsert("REQUEST_THEME", txtBox.Text.Trim());

            if (!ExceptionOccured)
                Close();
        }

        private void SuccessAddedRequestTheme(object sender, EventArgs e)
        {
            MainFrm.RenderDataGridViewThemesRequests();
            FormHelper.SuccessRecordInserted();
        }
    }
}
