using System;
using System.Windows.Forms;
using UsersSupport.Data;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.AdminForms.DepartmentHelperForms
{
    public class EditDepartmentForm : SharedDepartmentForm
    {
        public EditDepartmentForm(MainForm mainForm, DataGridView dataGridView) : base(mainForm, dataGridView)
        {
            RenderForm();
            leftBtn.Click += EditDepartmentBtn_Click;
        }

        private void RenderForm()
        {
            txtBox.Text   = DataGridView[DataGridView.Columns["DepartmentName"].Index, DataGridView.CurrentRow.Index].Value.ToString();
            lbl.Text      = "Измените название отдела";
            leftBtn.Image = imageList1.Images["updateImg"];
        }

        private void EditDepartmentBtn_Click(object sender, EventArgs e)
        {
            ExceptionOccured = false;
            if (FormHelper.AtLeastOneTxtBoxIsEmpty(txtBox))
            {
                MessageBox.Show("Поле не должно быть пустым", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var Id = Convert.ToInt32(DataGridView[DataGridView.Columns["Id"].Index,
                                     DataGridView.CurrentRow.Index].Value);
            var db = new DataAccess();

            db.SuccessRecordUpdate      += SuccessUpdatedDepartment;
            db.DepartmentExistException += Db_DepartmentExistException;
            db.SimpleUpdate("DEPARTMENT", "DepartmentName", txtBox.Text.Trim(), Id);

            if (!ExceptionOccured)
                Close();
        }

        private void SuccessUpdatedDepartment(object sender, EventArgs e)
        {
            MainFrm.RenderDataGridViewDepartments();
            FormHelper.SuccessRecordUpdated();
        }
    }
}
