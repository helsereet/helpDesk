using System;
using System.Windows.Forms;
using UsersSupport.Data;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.AdminForms.RequestTypeHelperForms
{
    public class AddRequestTypeForm : SharedRequestTypeForm
    {
        public AddRequestTypeForm(MainForm mainForm) : base(mainForm)
        {
            RenderForm();
            leftBtn.Click += AddRequestTypeBtn_Click;
        }

        private void RenderForm()
        {
            lbl.Text      = "Введите название нового типа заявки";
            leftBtn.Image = imageList1.Images["addImg"];
        }

        private void AddRequestTypeBtn_Click(object sender, EventArgs e)
        {
            ExceptionOccured = false;
            if (FormHelper.AtLeastOneTxtBoxIsEmpty(txtBox))
            {
                MessageBox.Show("Поле не должно быть пустым", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var db                        = new DataAccess();
            db.SuccessRecordInsert       += SuccessAddedRequestType;
            db.RequestTypeExistException += Db_RequestTypeExistException;
            db.SimpleInsert("REQUEST_TYPE", txtBox.Text.Trim());

            if (!ExceptionOccured)
                Close();
        }

        private void SuccessAddedRequestType(object sender, EventArgs e)
        {
            MainFrm.RenderDataGridViewTypesRequests();
            FormHelper.SuccessRecordInserted();
        }
    }
}
