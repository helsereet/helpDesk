using System;
using System.Windows.Forms;
using UsersSupport.Forms.AdminForms.GeneralForms;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.AdminForms.DepartmentHelperForms
{
    public class SharedDepartmentForm : SharedDashboardForm
    {
        public SharedDepartmentForm(MainForm mainForm, DataGridView dataGridView) : base(mainForm, dataGridView) { }

        public SharedDepartmentForm(MainForm mainForm) : base(mainForm) { }

        protected internal void Db_DepartmentExistException(object sender, EventArgs e)
        {
            ExceptionOccured = true;
            MessageBox.Show("Такая должность уже существует!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
