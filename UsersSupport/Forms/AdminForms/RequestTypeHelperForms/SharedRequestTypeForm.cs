using System;
using System.Windows.Forms;
using UsersSupport.Forms.AdminForms.GeneralForms;
using UsersSupport.Forms.GeneralForms;


namespace UsersSupport.Forms.AdminForms.RequestTypeHelperForms
{
    public class SharedRequestTypeForm : SharedDashboardForm
    {
        public SharedRequestTypeForm(MainForm mainForm, DataGridView dataGridView) : base(mainForm, dataGridView) { }

        public SharedRequestTypeForm(MainForm mainForm) : base(mainForm) { }

        protected internal void Db_RequestTypeExistException(object sender, EventArgs e)
        {
            ExceptionOccured = true;
            MessageBox.Show("Такой тип заявки уже существует!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
