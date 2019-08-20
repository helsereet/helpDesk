using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UsersSupport.Data;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.AdminForms.EmployeeHelperForms
{
    public partial class SharedEmployeeForm : Form
    {
        protected internal DataGridView DataGrid { get; set; }

        protected internal MainForm MainFrm { get; set; }

        protected internal bool ExceptionOccured { get; set; } = false;

        protected internal Dictionary<string, string> TextBoxes { get; set; }

        protected internal Dictionary<string, int> ComboBoxes { get; set; }

        protected internal Dictionary<string, bool> CheckBoxes { get; set; }

        public SharedEmployeeForm()
        {
            InitializeComponent();
            CancelButton = cancelBtn;
            AcceptButton = leftBtn;
        }

        public SharedEmployeeForm(MainForm mainForm, DataGridView dataGridView) : this(mainForm)
        {
            DataGrid = dataGridView;
        }

        public SharedEmployeeForm(MainForm mainForm) : this()
        {
            MainFrm = mainForm;
        }

        protected internal void BindCmbBoxes()
        {
            BindDepartmentsCmbBox();
            BindPositionsCmbBox();
            BindRolesCmbBox();
        }

        private void BindRolesCmbBox()
        {
            var db                    = new DataAccess();
            var roles                 = db.GetRoles();
            rolesCmbBox.DataSource    = roles;
            rolesCmbBox.DisplayMember = "RoleName";
            rolesCmbBox.ValueMember   = "Id";
        }

        private void BindPositionsCmbBox()
        {
            var db                        = new DataAccess();
            var positions                 = db.GetPositions();
            positionsCmbBox.DataSource    = positions;
            positionsCmbBox.DisplayMember = "PositionName";
            positionsCmbBox.ValueMember   = "Id";
        }

        private void BindDepartmentsCmbBox()
        {
            var db                          = new DataAccess();
            var departments                 = db.GetDepartments();
            departmentsCmbBox.DataSource    = departments;
            departmentsCmbBox.DisplayMember = "DepartmentName";
            departmentsCmbBox.ValueMember   = "Id";
        }

        protected internal void Db_EmailExistException(object sender, EventArgs e)
        {
            ExceptionOccured = true;
            MessageBox.Show("Данная почта УЖЕ занята", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected internal void Db_UserNameExistException(object sender, EventArgs e)
        {
            ExceptionOccured = true;
            MessageBox.Show("Данный логин УЖЕ занят", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected internal void MessageFieldsMustBeFilled()
        {
            MessageBox.Show("ФИО, логин, пароль и почта должны быть заполнены", "Заполните необходимые поля",
                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        protected internal void GrabFields()
        {
            FormHelper.TrimAllTxtBoxes(Controls);

            TextBoxes  = FormHelper.GetTxtBoxesValues(Controls);
            ComboBoxes = FormHelper.GetCmbBoxesValues(Controls);
            CheckBoxes = FormHelper.GetChkBoxesValues(Controls);
        }

        protected internal bool AtLeastOneNeededFieldIsEmpty()
        {
            return FormHelper.AtLeastOneTxtBoxIsEmpty(firstNameTxtBox, lastNameTxtBox, surNameTxtBox,
                                                      userNameTxtBox, passwordTxtBox, emailTxtBox);
        }
    }
}
