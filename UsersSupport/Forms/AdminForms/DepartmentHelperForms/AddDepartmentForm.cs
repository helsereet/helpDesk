using System;
using System.Windows.Forms;
using UsersSupport.Data;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.AdminForms.DepartmentHelperForms
{
    public class AddDepartmentForm : SharedDepartmentForm
    {
        public AddDepartmentForm(MainForm mainForm) : base(mainForm)
        {
            RenderForm();
            leftBtn.Click += AddDepartmentBtn_Click;
        }

        private void RenderForm()
        {
            lbl.Text      = "Введите название нового отдела";
            leftBtn.Image = imageList1.Images["addImg"];
        }

        private void AddDepartmentBtn_Click(object sender, EventArgs e)
        {
            ExceptionOccured = false;
            if (FormHelper.AtLeastOneTxtBoxIsEmpty(txtBox))
            {
                MessageBox.Show("Поле не должно быть пустым", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var db = new DataAccess();
            db.SuccessRecordInsert      += SuccessAddedDepartment;
            db.DepartmentExistException += Db_DepartmentExistException;
            db.SimpleInsert("DEPARTMENT", txtBox.Text.Trim());

            if (!ExceptionOccured)
                Close();
        }

        private void SuccessAddedDepartment(object sender, EventArgs e)
        {
            MainFrm.RenderDataGridViewDepartments();
            FormHelper.SuccessRecordDeleted();
        }
    }
}
