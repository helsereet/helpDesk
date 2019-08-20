using System;
using System.Windows.Forms;
using UsersSupport.Data;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.AdminForms.RequestThemeHelperForms
{
    public class EditRequestThemeForm : SharedRequestThemeForm
    {
        public EditRequestThemeForm(MainForm mainForm, DataGridView dataGridView) : base(mainForm, dataGridView)
        {
            RenderForm();
            leftBtn.Click += EditRequestThemeBtn_Click;
        }

        private void RenderForm()
        {
            txtBox.Text   = DataGridView[DataGridView.Columns["ThemeName"].Index, DataGridView.CurrentRow.Index].Value.ToString();
            lbl.Text      = "Измените название темы заявки";
            leftBtn.Image = imageList1.Images["updateImg"];
        }

        private void EditRequestThemeBtn_Click(object sender, EventArgs e)
        {
            ExceptionOccured = false;
            if (FormHelper.AtLeastOneTxtBoxIsEmpty(txtBox))
            {
                MessageBox.Show("Поле не должно быть пустым", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var id                         = Convert.ToInt32(DataGridView[DataGridView.Columns["Id"].Index, 
                                                             DataGridView.CurrentRow.Index].Value);
            var db                         = new DataAccess();
            db.SuccessRecordUpdate        += SuccessUpdatedRequestTheme;
            db.RequestThemeExistException += Db_RequestThemeExistException;
            db.SimpleUpdate("REQUEST_THEME", "ThemeName", txtBox.Text.Trim(), id);

            if (!ExceptionOccured)
                Close();
        }

        private void SuccessUpdatedRequestTheme(object sender, EventArgs e)
        {
            MainFrm.RenderDataGridViewThemesRequests();
            FormHelper.SuccessRecordUpdated();
        }
    }
}
