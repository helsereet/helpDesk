using System;
using UsersSupport.Data;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.AdminForms.EmployeeHelperForms
{
    public class AddEmployeeForm : SharedEmployeeForm
    {
        public AddEmployeeForm(MainForm mainForm) : base(mainForm)
        {
            RenderForm();
            leftBtn.Click += AddEmployeeBtn_Click;
        }

        private void RenderForm()
        {
            CustomizeControls();
            BindCmbBoxes();
        }

        private void CustomizeControls()
        {
            Text = "Добавление нового сотрудника";
            prefixEmployeeIdLbl.Visible = false;
            employeeIdLbl.Visible = false;
            leftBtn.Image = imageList.Images["addImg"];
        }

        private void AddEmployeeBtn_Click(object sender, EventArgs e)
        {
            ExceptionOccured = false;
            if (AtLeastOneNeededFieldIsEmpty())
                MessageFieldsMustBeFilled();
            else AddEmployee();
        }

        private void AddEmployee()
        {
            GrabFields();

            var db = new DataAccess();
            db.UserNameExistException += Db_UserNameExistException;
            db.EmailExistException    += Db_EmailExistException;
            db.SuccessRecordInsert    += SuccessAddedEmployee;
            db.AddEmployee(TextBoxes, ComboBoxes, CheckBoxes);

            if (!ExceptionOccured) Close();
        }

        private void SuccessAddedEmployee(object sender, EventArgs e)
        {
            MainFrm.RenderDataGridViewEmployees();
            FormHelper.SuccessRecordInserted();
        }
    }
}
