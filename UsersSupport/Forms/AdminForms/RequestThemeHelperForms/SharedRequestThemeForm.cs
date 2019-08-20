using System;
using System.Windows.Forms;
using UsersSupport.Forms.AdminForms.GeneralForms;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.AdminForms.RequestThemeHelperForms
{
    public class SharedRequestThemeForm : SharedDashboardForm
    {
        public SharedRequestThemeForm(MainForm mainForm, DataGridView dataGridView) : base(mainForm, dataGridView) { }

        public SharedRequestThemeForm(MainForm mainForm) : base(mainForm) { }

        protected internal void Db_RequestThemeExistException(object sender, EventArgs e)
        {
            ExceptionOccured = true;
            MessageBox.Show("Такая тема заявки уже существует!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
