using System;
using System.Windows.Forms;
using UsersSupport.Data;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.AdminForms.RequestTypeHelperForms
{
    public class EditRequestTypeForm : SharedRequestTypeForm
    {
        public EditRequestTypeForm(MainForm mainForm, DataGridView dataGridView) : base(mainForm, dataGridView)
        {
            RenderForm();
            leftBtn.Click += EditRequestTypeBtn_Click;
        }

        private void RenderForm()
        {
            txtBox.Text   = DataGridView[DataGridView.Columns["TypeName"].Index, DataGridView.CurrentRow.Index].Value.ToString();
            lbl.Text      = "Измените название типа заявки";
            leftBtn.Image = imageList1.Images["updateImg"];
        }

        private void EditRequestTypeBtn_Click(object sender, EventArgs e)
        {
            ExceptionOccured = false;
            if (FormHelper.AtLeastOneTxtBoxIsEmpty(txtBox))
            {
                MessageBox.Show("Поле не должно быть пустым", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var id                        = Convert.ToInt32(DataGridView[DataGridView.Columns["Id"].Index, 
                                                            DataGridView.CurrentRow.Index].Value);
            var db                        = new DataAccess();
            db.SuccessRecordUpdate       += SuccessUpdatedRequestType;
            db.RequestTypeExistException += Db_RequestTypeExistException;
            db.SimpleUpdate("REQUEST_TYPE", "TypeName", txtBox.Text.Trim(), id);

            if (!ExceptionOccured)
                Close();
        }

        private void SuccessUpdatedRequestType(object sender, EventArgs e)
        {
            MainFrm.RenderDataGridViewTypesRequests();
            FormHelper.SuccessRecordUpdated();
        }
    }
}
