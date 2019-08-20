using System;
using System.Windows.Forms;
using UsersSupport.Data;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.AdminForms.PositionHelperForms
{
    public class EditPositionForm : SharedPositionForm
    {
        public EditPositionForm(MainForm mainForm, DataGridView dataGridView) : base(mainForm, dataGridView)
        {
            leftBtn.Click += EditPositionBtn_Click;
            Init();
        }

        private void Init()
        {
            txtBox.Text   = DataGridView[DataGridView.Columns["PositionName"].Index, 
                                         DataGridView.CurrentRow.Index].Value.ToString();
            lbl.Text      = "Измените название должности";
            Name          = "Редактирование должности";
            leftBtn.Image = imageList1.Images["updateImg"];
        }

        private void EditPositionBtn_Click(object sender, EventArgs e)
        {
            ExceptionOccured = false;
            if (FormHelper.AtLeastOneTxtBoxIsEmpty(txtBox))
            {
                MessageBox.Show("Поле не должно быть пустым", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var Id                     = Convert.ToInt32(DataGridView[DataGridView.Columns["Id"].Index, 
                                                         DataGridView.CurrentRow.Index].Value);
            var db                     = new DataAccess();
            db.SuccessRecordUpdate    += SuccessUpdatedPosition;
            db.PositionExistException += Db_PositionExistException;
            db.SimpleUpdate("POSITION", "PositionName", txtBox.Text.Trim(), Id);

            if (!ExceptionOccured)
                Close();
        }

        private void SuccessUpdatedPosition(object sender, EventArgs e)
        {
            MainFrm.RenderDataGridViewPositions();
            FormHelper.SuccessRecordUpdated();
        }
    }
}
