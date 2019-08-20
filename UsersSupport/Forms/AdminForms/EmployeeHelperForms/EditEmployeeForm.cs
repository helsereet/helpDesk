using System;
using System.Windows.Forms;
using UsersSupport.Data;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.AdminForms.EmployeeHelperForms
{
    public class EditEmployeeForm : SharedEmployeeForm
    {
        public EditEmployeeForm(MainForm mainForm, DataGridView dataGridView) : base(mainForm, dataGridView)
        {
            RenderForm();
            leftBtn.Click += EditEmployeeBtn_Click;
        }

        private void RenderForm()
        {
            CustomizeControls();

            BindCmbBoxes();
            FillFields();
        }

        private void CustomizeControls()
        {
            Text               = "Редактирование сотрудника";
            employeeIdLbl.Text = Convert.ToString(DataGrid[DataGrid.Columns["Id"].Index,
                                                  DataGrid.CurrentRow.Index].Value);
            leftBtn.Image      = imageList.Images["updateImg"];
        }

        private void FillFields()
        {
            SetCmbBoxesStartValue();
            SetTxtBoxesStartValue();
            SetChkBoxesStartValue();
        }

        private void SetChkBoxesStartValue()
        {
            workingStateChkBox.Checked = Convert.ToBoolean(DataGrid[DataGrid.Columns["WorkingState"].Index,
                                                           DataGrid.CurrentRow.Index].Value);
        }

        private void SetTxtBoxesStartValue()
        {
            firstNameTxtBox.Text   = Convert.ToString(DataGrid[DataGrid.Columns["FirstName"].Index, DataGrid.CurrentRow.Index].Value) ?? "";
            lastNameTxtBox.Text    = Convert.ToString(DataGrid[DataGrid.Columns["LastName"].Index, DataGrid.CurrentRow.Index].Value) ?? "";
            surNameTxtBox.Text     = Convert.ToString(DataGrid[DataGrid.Columns["SurName"].Index, DataGrid.CurrentRow.Index].Value) ?? "";
            phoneNumberTxtBox.Text = Convert.ToString(DataGrid[DataGrid.Columns["PhoneNumber"].Index, DataGrid.CurrentRow.Index].Value) ?? "";
            roomNumberTxtBox.Text  = Convert.ToString(DataGrid[DataGrid.Columns["RoomNumber"].Index, DataGrid.CurrentRow.Index].Value) ?? "";
            addressTxtBox.Text     = Convert.ToString(DataGrid[DataGrid.Columns["Address"].Index, DataGrid.CurrentRow.Index].Value) ?? "";
            userNameTxtBox.Text    = Convert.ToString(DataGrid[DataGrid.Columns["UserName"].Index, DataGrid.CurrentRow.Index].Value) ?? "";
            passwordTxtBox.Text    = Convert.ToString(DataGrid[DataGrid.Columns["Password"].Index, DataGrid.CurrentRow.Index].Value) ?? "";
            emailTxtBox.Text       = Convert.ToString(DataGrid[DataGrid.Columns["Email"].Index, DataGrid.CurrentRow.Index].Value) ?? "";
        }

        private void SetCmbBoxesStartValue()
        {
            departmentsCmbBox.SelectedIndex = departmentsCmbBox.FindStringExact(DataGrid[DataGrid.Columns["DepartmentName"].Index, 
                                                                                DataGrid.CurrentRow.Index].Value as string);
            positionsCmbBox.SelectedIndex   = positionsCmbBox.FindStringExact(DataGrid[DataGrid.Columns["PositionName"].Index, 
                                                                              DataGrid.CurrentRow.Index].Value as string);
            rolesCmbBox.SelectedIndex       = rolesCmbBox.FindStringExact(DataGrid[DataGrid.Columns["RoleName"].Index, 
                                                                          DataGrid.CurrentRow.Index].Value as string);
        }

        private void EditEmployeeBtn_Click(object sender, EventArgs e)
        {
            ExceptionOccured = false;
            if (AtLeastOneNeededFieldIsEmpty())
                MessageFieldsMustBeFilled();
            else UpdateEmployee();
        }

        private void UpdateEmployee()
        {
            GrabFields();
            var employeeId = Convert.ToInt32(DataGrid[DataGrid.Columns["Id"].Index,
                                             DataGrid.CurrentRow.Index].Value);

            var db = new DataAccess();
            db.UserNameExistException += Db_UserNameExistException;
            db.EmailExistException    += Db_EmailExistException;
            db.SuccessRecordUpdate    += SuccessUpdatedEmployee;
            db.UpdateEmployee(TextBoxes, ComboBoxes, CheckBoxes, employeeId);

            if (!ExceptionOccured) Close();
        }

        private void SuccessUpdatedEmployee(object sender, EventArgs e)
        {
            MainFrm.RenderDataGridViewEmployees();
            FormHelper.SuccessRecordUpdated();
        }
    }
}
