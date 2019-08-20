using System;
using System.Windows.Forms;
using UsersSupport.Forms.AdminForms.GeneralForms;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.AdminForms.PositionHelperForms
{
    public class SharedPositionForm : SharedDashboardForm
    {
        public SharedPositionForm(MainForm mainForm, DataGridView dataGridView) : base(mainForm, dataGridView) { }

        public SharedPositionForm(MainForm mainForm) : base(mainForm) { }

        protected internal void Db_PositionExistException(object sender, EventArgs e)
        {
            ExceptionOccured = true;
            MessageBox.Show("Такая должность уже существует", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
