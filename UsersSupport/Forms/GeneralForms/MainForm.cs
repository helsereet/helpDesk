using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UsersSupport.Data;
using UsersSupport.Forms.AdminForms.DepartmentHelperForms;
using UsersSupport.Forms.AdminForms.EmployeeHelperForms;
using UsersSupport.Forms.AdminForms.PositionHelperForms;
using UsersSupport.Forms.AdminForms.RequestSettingHelperForms;
using UsersSupport.Forms.AdminForms.RequestThemeHelperForms;
using UsersSupport.Forms.AdminForms.RequestTypeHelperForms;
using UsersSupport.Forms.CustomerForms;
using UsersSupport.Forms.PerformerForms;

namespace UsersSupport.Forms.GeneralForms
{
    public partial class MainForm : Form
    {
        enum StatisticsUser
        {
            Admin = 1,
            Performer
        };

        enum StatisticsSelection
        {
            CurrentDay,
            CurrentWeek,
            CurrentMonth
        };

        readonly List<string> employeeInvisibleColumns = new List<string>
        {
            "Id",
            "DepartmentId",
            "PositionId",
            "Password",
            "RoleId"
        };

        readonly List<string> positionInvisibleColumns = new List<string>
        {
            "Id"
        };

        readonly List<string> departmentInvisibleColumns = new List<string>
        {
            "Id"
        };

        readonly List<string> requestThemeInvisibleColumns = new List<string>
        {
            "Id"
        };

        readonly List<string> requestTypeInvisibleColumns = new List<string>
        {
            "Id"
        };

        readonly Dictionary<string, object> commonDataGridViewHeaderFont = new Dictionary<string, object>
        {
            { "Font", "Microsoft YaHei UI" },
            { "FontSize", 12f },
            { "FontStyle", FontStyle.Bold }
        };

        readonly List<string> customerRequestVisibleColumns = new List<string>
        {
            "ShortDescription",
            "Taken",
            "Solved",
            "Closed",
            "CustomerMark",
            "TakenAt"
        };

        readonly List<string> performerTasksVisibleColumns = new List<string>
        {
            "ShortDescription",
            "Taken",
            "Solved",
            "Closed",
            "TakenAt"
        };

        readonly List<string> requestSettingsVisibleColumns = new List<string>
        {
            "TypeName",
            "ThemeName",
            "Description",
            "NormExecutionTimeReadable"
        };

        private Hashtable InfoRequestPanelControls { get; } = new Hashtable();

        private Hashtable InfoTaskPanelControls { get; } = new Hashtable();

        private void InfoRequestPanelControlsAssignment()
        {
            InfoRequestPanelControls["container"] = infoRequestPanel;
            InfoRequestPanelControls["id"] = requestIdLabel;
            InfoRequestPanelControls["typeName"] = requestTypeNameLabel;
            InfoRequestPanelControls["themeName"] = requestThemeNameLabel;
            InfoRequestPanelControls["shortDescription"] = requestShortDescriptionTextBox;
            InfoRequestPanelControls["fullDescription"] = requestFullDescriptionTextBox;
            InfoRequestPanelControls["solution"] = requestSolutionTextBox;
            InfoRequestPanelControls["mark"] = requestCustomerMarkLabel;
        }

        private void InfoTaskPanelControlsAssigment()
        {
            InfoTaskPanelControls["container"] = infoTaskPanel;
            InfoTaskPanelControls["id"] = taskIdLabel;
            InfoTaskPanelControls["typeName"] = taskTypeNameLabel;
            InfoTaskPanelControls["themeName"] = taskThemeNameLabel;
            InfoTaskPanelControls["shortDescription"] = taskShortDescriptionTextBox;
            InfoTaskPanelControls["fullDescription"] = taskFullDescriptionTextBox;
            InfoTaskPanelControls["solution"] = taskSolutionTextBox;
            InfoTaskPanelControls["mark"] = taskPerformerMarkLabel;
        }

        public List<Request> Requests { get; set; }

        public User User { get; set; }

        public MainForm()
        {
            InitializeComponent();
            AddDataGridViewsEventHandlers();

            Visible = false;
            OpenAuthorizationForm();
        }

        private void AddDataGridViewsEventHandlers()
        {
            dataGridPerformerRequests.DataSourceChanged += DataGridPerformerRequests_DataSourceChanged;
            dataGridCustomerRequests.DataSourceChanged  += DataGridCustomerRequests_DataSourceChanged;
        }

        private void DataGridCustomerRequests_DataSourceChanged(object sender, EventArgs e)
        {
            UpdateRequests();
            UpdateCustomerStatistics();

            // Customize DataGridView Representation
            DataHelper.MakeAllColumnsInvisible(dataGridCustomerRequests);
            DataHelper.MakeColumnsVisible(dataGridCustomerRequests, customerRequestVisibleColumns);
            DataHelper.MakeColumnsOrder(dataGridCustomerRequests, customerRequestVisibleColumns);
            DataHelper.UpdateFontOfDataGridView(dataGridCustomerRequests, commonDataGridViewHeaderFont);
        }

        private void UpdateCustomerStatistics()
        {
            var db                           = new DataAccess();
            var openedRequestsCount          = db.GetOpenedRequestsCount(User.Id);
            var solvedNotClosedRequestsCount = db.GetSolvedNotClosedRequestsCount(User.Id);
            var closedRequestsCount          = db.GetClosedRequestsCount(User.Id);

            openedRequestsLabel.Text          = openedRequestsCount.ToString();
            solvedNotClosedRequestsLabel.Text = solvedNotClosedRequestsCount.ToString();
            closedRequestsLabel.Text          = closedRequestsCount.ToString();
        }

        private void DataGridPerformerRequests_DataSourceChanged(object sender, EventArgs e)
        {
            UpdateRequests();
            UpdatePerformersStatisticsFields();

            // Customize DataGridView Representation
            // TODO: make list of visible columns for performer -> edit all tasks requests for that
            DataHelper.MakeAllColumnsInvisible(dataGridPerformerRequests);
            DataHelper.MakeColumnsVisible(dataGridPerformerRequests, performerTasksVisibleColumns);
            DataHelper.MakeColumnsOrder(dataGridPerformerRequests, performerTasksVisibleColumns);
            DataHelper.UpdateFontOfDataGridView(dataGridPerformerRequests, commonDataGridViewHeaderFont);
        }

        private void UpdatePerformersStatisticsFields()
        {
            var db                        = new DataAccess();
            var openedTasksCount          = db.GetOpenedTasksCount(User.Id);
            var solvedNotClosedTasksCount = db.GetSolvedNotClosedTasksCount(User.Id);
            var closedTasksCount          = db.GetClosedTasksCount(User.Id);

            takedTasksLabel.Text  = openedTasksCount.ToString();
            solvedTasksLabel.Text = solvedNotClosedTasksCount.ToString();
            closedTasksLabel.Text = closedTasksCount.ToString();
        }

        private void UpdateRequests()
        {
            var db   = new DataAccess();
            Requests = db.GetRequests();
        }

        private void OpenAuthorizationForm()
        {
            var authorizationFrm               = new AuthorizationForm(this);
            authorizationFrm.FormClosed       += AuthorizationFrm_FormClosed;
            authorizationFrm.AuthorizeSuccess += AuthorizationFrm_AuthorizeSuccess;
            authorizationFrm.ShowDialog();
        }

        private void AuthorizationFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }

        private void AuthorizationFrm_AuthorizeSuccess(object source, EventArgs e)
        {
            ((AuthorizationForm)source).Visible = false;
            RenderForm();
            Visible = true;
        }

        private void RenderForm()
        {
            RenderMenuStrip();
            FormHelper.HideHeaderTabPage(mainFormTabControl);
            FormHelper.HideHeaderTabPage(statisticsTabControl);
            RenderTabPage();
            RenderToolStrip();
        }

        private void RenderToolStrip()
        {
            userNameLbl.Text        = User.UserName;
            fullNameLbl.Text        = User.GetFullName();
            positionNameLbl.Text    = User.PositionName;
            roleNameLbl.Text        = User.RoleName;
            currentRoleNameLbl.Text = User.CurrentRoleName;
            dateLbl.Text            = DateTime.Now.ToLongTimeString();
        }

        private void RenderTabPage()
        {
            switch (User.CurrentRoleId)
            {
                case 1:
                    DisplayAdminStartTabPage();
                    break;
                case 2:
                    DisplayPerformerTabPage();
                    break;
                case 3:
                    DisplayCustomerTabPage();
                    break;
            }
        }

        private void RenderMenuStrip()
        {
            switch (User.RoleId)
            {
                case 1:
                    DisplayAdminMenuStrip();
                    break;
                case 2:
                    DisplayPerformerMenuStrip();
                    break;
                case 3:
                    DisplayCustomerMenuStrip();
                    break;
            }
        }

        private void DisplayCustomerMenuStrip()
        {
            menuStrip.Items.Clear();
            menuStrip.Items.Add(fileToolStripMenuItem);
            menuStrip.Items.Add(helpToolStripMenuItem);
        }

        private void DisplayPerformerMenuStrip()
        {
            menuStrip.Items.Clear();
            menuStrip.Items.Add(fileToolStripMenuItem);
            menuStrip.Items.Add(tasksToolStripMenuItem);
            menuStrip.Items.Add(statisticsToolStripMenuItem);
            menuStrip.Items.Add(changeRoleToolStripMenuItem);
            menuStrip.Items.Add(helpToolStripMenuItem);
        }

        private void DisplayAdminMenuStrip()
        {
            menuStrip.Items.Clear();
            menuStrip.Items.Add(fileToolStripMenuItem);
            menuStrip.Items.Add(adminToolStripMenuItem);
            menuStrip.Items.Add(changeRoleToolStripMenuItem);
            menuStrip.Items.Add(helpToolStripMenuItem);
        }

        private void DisplayCustomerTabPage()
        {
            mainFormTabControl.TabPages.Clear();
            mainFormTabControl.TabPages.Add(customerTabPage);
            openedRequestsRadioButton.Checked = true;

            InfoRequestPanelControlsAssignment();
        }

        private void DisplayPerformerTabPage()
        {
            mainFormTabControl.TabPages.Clear();
            mainFormTabControl.TabPages.Add(performerTabPage);
            takenTasksRadioButton.Checked = true;

            InfoTaskPanelControlsAssigment();
        }

        private void DisplayAdminStartTabPage()
        {
            mainFormTabControl.TabPages.Clear();
            mainFormTabControl.TabPages.Add(adminDashboardEmployeesTabPage);
            RenderDataGridViewEmployees();
        }

        protected internal void RenderDataGridViewEmployees()
        {
            var db = new DataAccess();
            DataHelper.BindDataGridView(db.GetEmployees(), dataGridEmployees);
            DataHelper.MakeColumnsInvisible(dataGridEmployees, employeeInvisibleColumns);
            DataHelper.UpdateFontOfDataGridView(dataGridEmployees, commonDataGridViewHeaderFont);
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ChangeRoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var roleFrm              = new RoleForm(this);
            roleFrm.LoginBtnClicked += RoleFrm_LoginBtnClicked;
            roleFrm.ShowDialog();
        }

        private void RoleFrm_LoginBtnClicked(object source, EventArgs e)
        {
            ((RoleForm)source).Close();
            RenderForm();
        }

        private void EditEmployeeBtn_Click(object sender, EventArgs e)
        {
            if (dataGridEmployees.CurrentCell != null)
            {
                var editEmployeeFrm = new EditEmployeeForm(this, dataGridEmployees);
                editEmployeeFrm.ShowDialog();
            }
        }

        private void DataGridEmployees_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridEmployees.CurrentCell != null)
            {
                var editEmployeeFrm = new EditEmployeeForm(this, dataGridEmployees);
                editEmployeeFrm.ShowDialog();
            }
        }

        private void AddEmployeeBtn_Click(object sender, EventArgs e)
        {
            var addEmployeeFrm = new AddEmployeeForm(this);
            addEmployeeFrm.ShowDialog();
        }

        private void DeleteEmployeeBtn_Click(object sender, EventArgs e)
        {
            if (dataGridEmployees.CurrentCell != null)
                if (FormHelper.ConfirmDeletionRecord())
                    DeleteEmployee();
        }

        private void DeleteEmployee()
        {
            if (dataGridEmployees.CurrentCell != null)
            {
                var db                  = new DataAccess();
                db.SuccessRecordDelete += Db_SuccessRecordDelete;
                DataHelper.DeleteSelectedRecord(dataGridEmployees, db.SimpleDelete, "EMPLOYEE");
                RenderDataGridViewEmployees();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            RenderDataGridViewEmployees();
        }

        private void EmployeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayAdminStartTabPage();
        }

        private void OtherDashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainFormTabControl.TabPages.Clear();
            mainFormTabControl.TabPages.Add(adminDashboardOtherTabPage);
            RenderPositionsDepartmentsTabPage();
        }

        private void OtherDashboardTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (otherDashboardTabControl.SelectedTab == otherDashboardTabControl.TabPages["positionsDepartmentsTabPage"])
            {
                RenderPositionsDepartmentsTabPage();
            }
            else if (otherDashboardTabControl.SelectedTab == otherDashboardTabControl.TabPages["themesTypesRequestsTabPage"])
            {
                RenderThemesTypesRequestsTabPage();
            }
            else if (otherDashboardTabControl.SelectedTab == otherDashboardTabControl.TabPages["requestsSettingsTabPage"])
            {
                RenderRequestSettingsTabPage();
            }
        }

        private void RenderRequestSettingsTabPage()
        {
            RenderDataGridViewRequestsSettings();
        }

        protected internal void RenderDataGridViewRequestsSettings()
        {
            var db = new DataAccess();
            DataHelper.BindDataGridView(db.GetRequestSettings(), dataGridRequestsSettings);

            DataHelper.MakeAllColumnsInvisible(dataGridRequestsSettings);
            DataHelper.MakeColumnsVisible(dataGridRequestsSettings, requestSettingsVisibleColumns);
            DataHelper.MakeColumnsOrder(dataGridRequestsSettings, requestSettingsVisibleColumns);
            DataHelper.UpdateFontOfDataGridView(dataGridRequestsSettings, commonDataGridViewHeaderFont);
        }

        private void RenderThemesTypesRequestsTabPage()
        {
            RenderDataGridViewThemesRequests();
            RenderDataGridViewTypesRequests();
        }

        protected internal void RenderDataGridViewTypesRequests()
        {
            var db = new DataAccess();
            DataHelper.BindDataGridView(db.GetRequestTypes(), dataGridTypesRequests);
            DataHelper.MakeColumnsInvisible(dataGridTypesRequests, requestTypeInvisibleColumns);
            DataHelper.UpdateFontOfDataGridView(dataGridTypesRequests, commonDataGridViewHeaderFont);
        }

        protected internal void RenderDataGridViewThemesRequests()
        {
            var db = new DataAccess();
            DataHelper.BindDataGridView(db.GetRequestThemes(), dataGridThemesRequests);
            DataHelper.MakeColumnsInvisible(dataGridThemesRequests, requestThemeInvisibleColumns);
            DataHelper.UpdateFontOfDataGridView(dataGridThemesRequests, commonDataGridViewHeaderFont);
        }

        private void RenderPositionsDepartmentsTabPage()
        {
            RenderDataGridViewPositions();
            RenderDataGridViewDepartments();
        }

        protected internal void RenderDataGridViewDepartments()
        {
            var db = new DataAccess();
            DataHelper.BindDataGridView(db.GetDepartments(), dataGridDepartments);
            DataHelper.MakeColumnsInvisible(dataGridDepartments, departmentInvisibleColumns);
            DataHelper.UpdateFontOfDataGridView(dataGridDepartments, commonDataGridViewHeaderFont);
        }

        protected internal void RenderDataGridViewPositions()
        {
            var db = new DataAccess();
            DataHelper.BindDataGridView(db.GetPositions(), dataGridPositions);
            DataHelper.MakeColumnsInvisible(dataGridPositions, positionInvisibleColumns);
            DataHelper.UpdateFontOfDataGridView(dataGridPositions, commonDataGridViewHeaderFont);
        }

        private void EditPositionBtn_Click(object sender, EventArgs e)
        {
            if (dataGridPositions.CurrentCell != null)
            {
                var editPositionFrm = new EditPositionForm(this, dataGridPositions);
                editPositionFrm.ShowDialog();
            }
        }

        private void AddPositionBtn_Click(object sender, EventArgs e)
        {
            var addPositionFrm = new AddPositionForm(this);
            addPositionFrm.ShowDialog();
        }

        private void AddPositionFrm_SuccessfullyPositionAdded(object sender, EventArgs e)
        {
            RenderDataGridViewPositions();
            MessageBox.Show("Должность была успешно добавлена!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeletePositionBtn_Click(object sender, EventArgs e)
        {
            if (dataGridPositions.CurrentCell != null)
                if (FormHelper.ConfirmDeletionRecord())
                    DeletePosition();
        }

        private void DeletePosition()
        {
            var db                  = new DataAccess();
            db.SuccessRecordDelete += Db_SuccessRecordDelete;
            DataHelper.DeleteSelectedRecord(dataGridPositions, db.SimpleDelete, "POSITION");
            RenderDataGridViewPositions();
        }

        private void Db_SuccessRecordDelete(object sender, EventArgs e)
        {
            FormHelper.SuccessRecordDeleted();
        }

        private void DeleteDepartmentBtn_Click(object sender, EventArgs e)
        {
            if (dataGridDepartments.CurrentCell != null)
                if (FormHelper.ConfirmDeletionRecord())
                    DeleteDepartment();
        }

        private void DeleteDepartment()
        {
            var db                  = new DataAccess();
            db.SuccessRecordDelete += Db_SuccessRecordDelete;
            DataHelper.DeleteSelectedRecord(dataGridDepartments, db.SimpleDelete, "DEPARTMENT");
            RenderDataGridViewDepartments();
        }

        private void AddDepartmentBtn_Click(object sender, EventArgs e)
        {
            var addDepartmentFrm = new AddDepartmentForm(this);
            addDepartmentFrm.ShowDialog();
        }

        private void EditDepartmentBtn_Click(object sender, EventArgs e)
        {
            if (dataGridDepartments.CurrentCell != null)
            {
                var editDepartmentFrm = new EditDepartmentForm(this, dataGridDepartments);
                editDepartmentFrm.ShowDialog();
            }
        }

        private void DeleteRequestSettingBtn_Click(object sender, EventArgs e)
        {
            if (dataGridRequestsSettings.CurrentCell != null)
                if (FormHelper.ConfirmDeletionRecord())
                    DeleteRequestSetting();
        }

        private void DeleteRequestSetting()
        {
            var db                  = new DataAccess();
            db.SuccessRecordDelete += Db_SuccessRecordDelete;
            DataHelper.DeleteSelectedRecord(dataGridRequestsSettings, db.SimpleDelete,
                "REQUEST_TYPE_REQUEST_THEME");
            RenderDataGridViewRequestsSettings();
        }

        private void AddRequestSettingBtn_Click(object sender, EventArgs e)
        {
            var addRequestSettingForm = new AddRequestSettingForm(this);
            addRequestSettingForm.ShowDialog();
        }

        private void EditRequestSettingBtn_Click(object sender, EventArgs e)
        {
            if (dataGridRequestsSettings.CurrentCell != null)
            {
                var editRequestSettingFrm = new EditRequestSettigForm(this, dataGridRequestsSettings);
                editRequestSettingFrm.ShowDialog();
            }
        }

        private void EditTypeRequestBtn_Click(object sender, EventArgs e)
        {
            if (dataGridTypesRequests.CurrentCell != null)
            {
                var editTypeRequestFrm = new EditRequestTypeForm(this, dataGridTypesRequests);
                editTypeRequestFrm.ShowDialog();
            }
        }

        private void AddThemeRequestBtn_Click(object sender, EventArgs e)
        {
            var addThemeRequestFrm = new AddRequestThemeForm(this);
            addThemeRequestFrm.ShowDialog();
        }

        private void AddTypeRequestBtn_Click(object sender, EventArgs e)
        {
            if (dataGridTypesRequests.CurrentCell != null)
            {
                var addRequestTypeFrm = new AddRequestTypeForm(this);
                addRequestTypeFrm.ShowDialog();
            }
        }

        private void DeleteTypeRequestBtn_Click(object sender, EventArgs e)
        {
            if (dataGridTypesRequests.CurrentCell != null)
                if (FormHelper.ConfirmDeletionRecord())
                    DeleteTypeRequest();
        }

        private void DeleteTypeRequest()
        {
            var db                  = new DataAccess();
            db.SuccessRecordDelete += Db_SuccessRecordDelete;
            DataHelper.DeleteSelectedRecord(dataGridTypesRequests, db.SimpleDelete, "REQUEST_TYPE");
            RenderDataGridViewTypesRequests();
        }

        private void DeleteThemeRequestBtn_Click(object sender, EventArgs e)
        {
            if (dataGridThemesRequests.CurrentCell != null)
                if (FormHelper.ConfirmDeletionRecord())
                    DeleteThemeRequest();
        }

        private void DeleteThemeRequest()
        {
            var db                  = new DataAccess();
            db.SuccessRecordDelete += Db_SuccessRecordDelete;
            DataHelper.DeleteSelectedRecord(dataGridThemesRequests, db.SimpleDelete, "REQUEST_THEME");
            RenderDataGridViewThemesRequests();
        }

        private void EditThemeRequestBtn_Click(object sender, EventArgs e)
        {
            if (dataGridThemesRequests.CurrentCell != null)
            {
                var editRequestThemeFrm = new EditRequestThemeForm(this, dataGridThemesRequests);
                editRequestThemeFrm.ShowDialog();
            }
        }

        private void CreateRequestBtn_Click(object sender, EventArgs e)
        {
            var createRequestFrm = new CreateRequestForm(this);
            createRequestFrm.ShowDialog();
        }

        private void RequestInfoBtn_Click(object sender, EventArgs e)
        {
            if (dataGridCustomerRequests.CurrentCell != null)
            {
                var requestId      = Convert.ToInt32(dataGridCustomerRequests[dataGridCustomerRequests.Columns["Id"].Index, 
                                                     dataGridCustomerRequests.CurrentRow.Index].Value);
                var infoRequestFrm = new CustomerInfoRequestForm(this, requestId);
                infoRequestFrm.ShowDialog();
            }
        }

        private void ToggleInfoRequestPanel_Click(object sender, EventArgs e)
        { TogglePanel(infoRequestPanel, toggleInfoRequestButton); }

        private void TogglePanel(Panel panel, Button toggleButton)
        {
            if (panel.Visible)
            {
                panel.Visible      = false;
                toggleButton.Image = toggleButtonsImageList.Images["showPanel"];
            }
            else
            {
                panel.Visible      = true;
                toggleButton.Image = toggleButtonsImageList.Images["hidePanel"];
            }
        }

        private void OpenedRequestsRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RenderOpenedRequests();
        }

        private void OpenedNotTakenRequestsRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RenderOpenedNotTakenRequests();
        }

        private void AllOpenedTasksRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RenderAllOpenedTasks();
        }

        private void ToggleInfoTaskButton_Click(object sender, EventArgs e)
        { TogglePanel(infoTaskPanel, toggleInfoTaskButton); }

        private void TaskInfoButton_Click(object sender, EventArgs e)
        {
            if (dataGridPerformerRequests.CurrentCell != null)
            {
                var requestId               = Convert.ToInt32(dataGridPerformerRequests[dataGridPerformerRequests.Columns["Id"].Index, 
                                                              dataGridPerformerRequests.CurrentRow.Index].Value);
                var performerInfoRequestFrm = new PerformerInfoRequestForm(this, requestId);
                performerInfoRequestFrm.ShowDialog();
            }
        }

        private void RefreshTasksButton_Click(object sender, EventArgs e)
        {
            if (takenTasksRadioButton.Checked)
                RenderTakenRequests();
            else if (solvedNotClosedTasksRadioButtonn.Checked)
                RenderSolvedNotClosedTasks();
            else if (closedTasksRadioButton.Checked)
                RenderClosedTasks();
            else if (allOpenedTasksRadioButton.Checked)
                RenderAllOpenedTasks();
            else if (allOpenedNotTakenTasksRadioButton.Checked)
                RenderAllOpenedNotTakenTasks();
            else if (allOpenedAtWorkTasksRadioButton.Checked)
                RenderAllOpenedAtWorkTasks();
            else if (allClosedTasksRadioButton.Checked)
                RennderAllClosedRequests();
        }

        private void RennderAllClosedRequests()
        {
            throw new NotImplementedException();
        }

        private void RenderAllOpenedAtWorkTasks()
        {
            var db = new DataAccess();
            DataHelper.BindDataGridView(db.GetAllOpenedAtWorkTasks(), dataGridPerformerRequests);
        }

        private void RenderAllOpenedNotTakenTasks()
        {
            var db = new DataAccess();
            DataHelper.BindDataGridView(db.GetAllOpenedNotTakenTasks(), dataGridPerformerRequests);
        }

        private void RenderAllOpenedTasks()
        {
            var db = new DataAccess();
            DataHelper.BindDataGridView(db.GetAllOpenedTasks(), dataGridPerformerRequests);
        }

        private void RenderTakenRequests()
        {
            var db = new DataAccess();
            DataHelper.BindDataGridView(db.GetTakenTasks(User.Id), dataGridPerformerRequests);
        }

        private void RefreshRequestsButton_Click(object sender, EventArgs e)
        {
            if (openedRequestsRadioButton.Checked)
                RenderOpenedRequests();
            else if (openedNotTakenRequestsRadioButton.Checked)
                RenderOpenedNotTakenRequests();
            else if (solvedNotClosedRequestsRadioButton.Checked)
                RenderSolvedNotClosedRequests();
            else if (closedRequestsRadioButton.Checked)
                RenderClosedRequests();
        }

        private void RenderClosedRequests()
        {
            var db = new DataAccess();
            DataHelper.BindDataGridView(db.GetClosedRequests(User.Id), dataGridCustomerRequests);
        }

        private void RenderClosedTasks()
        {
            var db = new DataAccess();
            DataHelper.BindDataGridView(db.GetClosedTasks(User.Id), dataGridPerformerRequests);
        }

        private void RenderSolvedNotClosedRequests()
        {
            var db = new DataAccess();
            DataHelper.BindDataGridView(db.GetSolvedNotClosedRequests(User.Id), dataGridCustomerRequests);
        }

        private void RenderSolvedNotClosedTasks()
        {
            var db = new DataAccess();
            DataHelper.BindDataGridView(db.GetSolvedNotClosedTasks(User.Id), dataGridPerformerRequests);
        }

        private void RenderOpenedNotTakenRequests()
        {
            var db = new DataAccess();
            DataHelper.BindDataGridView(db.GetOpenedNotTakenRequests(User.Id), dataGridCustomerRequests);
        }

        protected internal void RenderOpenedRequests()
        {
            var db = new DataAccess();
            DataHelper.BindDataGridView(db.GetOpenedRequests(User.Id), dataGridCustomerRequests);
        }

        private void SolvedNotClosedRequestsRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RenderSolvedNotClosedRequests();
        }

        private void ClosedRequestsRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RenderClosedRequests();
        }

        private void TakenTasksRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RenderTakenRequests();
        }

        private void SolvedNotClosedTasksRadioButtonn_CheckedChanged(object sender, EventArgs e)
        {
            RenderSolvedNotClosedTasks();
        }

        private void AllClosedTasksRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RenderAllClosedTasks();
        }

        private void RenderAllClosedTasks()
        {
            var db = new DataAccess();
            DataHelper.BindDataGridView(db.GetAllClosedTasks(), dataGridPerformerRequests);
        }

        private void ClosedTasksRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RenderClosedTasks();
        }

        private void AllOpenedNotTakenTasksRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RenderAllOpenedNotTakenTasks();
        }

        private void AllOpenedAtWorkTasksRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RenderAllOpenedAtWorkTasks();
        }

        private void DataGridCustomerRequests_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridCustomerRequests.Rows.Count < 1)
                return;
            FillInfoPanel(InfoRequestPanelControls, dataGridCustomerRequests);
        }

        private void DataGridPerformerRequests_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridPerformerRequests.Rows.Count < 1)
                return;
            FillInfoPanel(InfoTaskPanelControls, dataGridPerformerRequests);
        }

        private void FillInfoPanel(Hashtable panelControls, DataGridView dataGridView)
        {
            var requestId       = Convert.ToInt32(dataGridView[dataGridView.Columns["Id"].Index, dataGridView.CurrentRow.Index].Value);
            var selectedRequest = Requests.Single(request => request.Id == requestId);

            (panelControls["id"] as Label).Text                 = selectedRequest.Id.ToString();
            (panelControls["typeName"] as Label).Text           = selectedRequest.TypeName;
            (panelControls["themeName"] as Label).Text          = selectedRequest.ThemeName;
            (panelControls["shortDescription"] as TextBox).Text = selectedRequest.ShortDescription;
            (panelControls["fullDescription"] as TextBox).Text  = selectedRequest.FullDescription;
            (panelControls["solution"] as TextBox).Text         = selectedRequest.Solution;
            (panelControls["mark"] as Label).Text               = selectedRequest.CustomerMark.ToString();
        }

        private void ChangePassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var changePasswordFrm = new ChangePassForm(this);
            changePasswordFrm.ShowDialog();
        }

        private void CurrentDayStatisticsButton_Click(object sender, EventArgs e)
        {
            if (User.RoleId == 2)
                RenderPerformersStatisticsPageForCurrentDayForCurrentPerformer();
        }

        private void RenderPerformersStatisticsPageForCurrentDayForCurrentPerformer()
        {
            var db                         = new DataAccess();
            var allPerformers              = db.GetAllPerformersStatisticsForCurrentDay();
            var resultPerformersStatistics = GetResultPerformers(db.GetAllPerformersStatisticsForCurrentDay, db.GetPerformerStatisticsForCurrentDay);

            DataHelper.BindChartStatistics(chart, resultPerformersStatistics);
            UpdateStatisticsFields(allPerformers);
        }

        private void CustomizeComboBox(StatisticsUser statisticsUser)
        {
            switch (statisticsUser)
            {
                case StatisticsUser.Admin:
                    performersStatisticsComboBox.Enabled = true;
                    break;
                case StatisticsUser.Performer:
                    performersStatisticsComboBox.Text = User.GetFullName();
                    performersStatisticsComboBox.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        private void StatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainFormTabControl.TabPages.Clear();
            mainFormTabControl.TabPages.Add(performerTabPage);
            takenTasksRadioButton.Checked = true;

            InfoTaskPanelControlsAssigment();
        }

        private void CurrentWeekStatisticsButton_Click(object sender, EventArgs e)
        {
            if (User.RoleId == 2)
                RenderPerformersStatisticsPageForCurrentWeekForCurrentPerformer();
        }

        private void RenderPerformersStatisticsPageForCurrentWeekForCurrentPerformer()
        {
            var db                         = new DataAccess();
            var allPerformers              = db.GetAllPerformersStatisticsForCurrentWeek();
            var resultPerformersStatistics = GetResultPerformers(db.GetAllPerformersStatisticsForCurrentWeek, db.GetPerformerStatisticsForCurrentWeek);

            DataHelper.BindChartStatistics(chart, resultPerformersStatistics);
            UpdateStatisticsFields(allPerformers);
        }

        private void CurrentMonthStatisticsButton_Click(object sender, EventArgs e)
        {
            if (User.RoleId == 2)
                RenderPerformersStatisticsPageForCurrentMonthForCurrentPerformer();
        }

        private void RenderPerformersStatisticsPageForCurrentMonthForCurrentPerformer()
        {
            var db                         = new DataAccess();
            var allPerformers              = db.GetAllPerformersStatisticsForCurrentMonth();
            var resultPerformersStatistics = GetResultPerformers(db.GetAllPerformersStatisticsForCurrentMonth, db.GetPerformerStatisticsForCurrentMonth);

            DataHelper.BindChartStatistics(chart, resultPerformersStatistics);
            UpdateStatisticsFields(allPerformers);
        }

        /// <summary>
        /// if current user in top 3: return top 3 performersStatistics
        /// else: return top 3 + current performerStatistic
        /// </summary>
        /// <param name="funcAllPerformersStatistics"></param>
        /// <param name="funcPerformerStatistic"></param>
        /// <returns></returns>
        private List<PerformerStatistics> GetResultPerformers(Func<List<PerformerStatistics>> funcAllPerformersStatistics,
                                                              Func<int, PerformerStatistics> funcPerformerStatistic)
        {
            var allPerformers    = funcAllPerformersStatistics();
            var top3Performers   = allPerformers.Take(3).ToList();
            var resultPerformers = top3Performers;

            if (top3Performers.SingleOrDefault(performer => performer.Id == User.Id) == null)
            {
                // current user not in top 3
                var currentPerformer = funcPerformerStatistic(User.Id);
                if (currentPerformer != null)
                    resultPerformers.Add(currentPerformer);
            }
            return resultPerformers;
        }

        private void UpdateStatisticsFields(List<PerformerStatistics> allPerformersStatistics)
        {
            ResetStatisticsLabels();

            var top3Performers = allPerformersStatistics.Take(3).ToList();

            SetTop3PerformersLabels(top3Performers);
            if (top3Performers.SingleOrDefault(performer => performer.Id == User.Id) == null)
                SetCurrentPerformerLabel(allPerformersStatistics);
            HightLightCurrentPerformerLabel(allPerformersStatistics);
        }

        private void ResetStatisticsLabels()
        {
            FormHelper.ClearLabelsByTagName(RatingGroupBox.Controls, "needToUpdateLabel");
            FormHelper.MakeLabelsRegularByTagName(RatingGroupBox.Controls, "needToUpdateLabel");
        }

        private void HightLightCurrentPerformerLabel(List<PerformerStatistics> allPerformersStatistics)
        {
            var position = allPerformersStatistics.FindIndex(performer => performer?.Id == User.Id) + 1;

            if (position == 0)
                return;
            else
            {
                switch (position)
                {
                    case 1:
                        FormHelper.MakelabelsBold(firstPerformerPositionLabel, firstPerformerNameRatingLabel, firstPerformerPointsLabel);
                        break;
                    case 2:
                        FormHelper.MakelabelsBold(secondPerformerPositionLabel, secondPerformerNameRatingLabel, secondPerformerPointsLabel);
                        break;
                    case 3:
                        FormHelper.MakelabelsBold(thirdPerformerPositionLabel, thirdPerformerNameRatingLabel, thirdPerformerPointsLabel);
                        break;
                    default:
                        FormHelper.MakelabelsBold(currentPerformerPositionLabel, currentPerformerNameRatingLabel, currentPerformerPointsLabel);
                        break;
                }
            }
        }

        private void SetCurrentPerformerLabel(List<PerformerStatistics> allPerformersStatistics)
        {
            var position = allPerformersStatistics.FindIndex(performer => performer?.Id == User.Id) + 1;

            if (position == 0)
                return;
            else
            {
                currentPerformerPositionLabel.Text   = position.ToString();
                currentPerformerNameRatingLabel.Text = allPerformersStatistics[position - 1].FIO;
                currentPerformerPointsLabel.Text     = allPerformersStatistics[position - 1].TotalPoints;
                FormHelper.MakelabelsBold(currentPerformerPositionLabel, currentPerformerNameRatingLabel, currentPerformerPointsLabel);
            }
        }

        private void SetTop3PerformersLabels(List<PerformerStatistics> top3Performers)
        {
            if (top3Performers.Count < 1)
                return;
            else if (top3Performers.Count == 1)
                SetFirstPerformerLabels(top3Performers);
            else if (top3Performers.Count == 2)
            {
                SetFirstPerformerLabels(top3Performers);
                SetSecondPerformerLabels(top3Performers);
            }
            else
            {
                SetFirstPerformerLabels(top3Performers);
                SetSecondPerformerLabels(top3Performers);
                SetThirdPerformerLabels(top3Performers);
            }
        }

        private void SetFirstPerformerLabels(List<PerformerStatistics> top3Performers)
        {
            firstPerformerPositionLabel.Text   = "1";
            firstPerformerNameRatingLabel.Text = top3Performers.FirstOrDefault()?.FIO;
            firstPerformerPointsLabel.Text     = top3Performers.FirstOrDefault()?.TotalPoints;
        }

        private void SetSecondPerformerLabels(List<PerformerStatistics> top3Performers)
        {
            secondPerformerPositionLabel.Text   = "2";
            secondPerformerNameRatingLabel.Text = top3Performers.ElementAtOrDefault(1)?.FIO;
            secondPerformerPointsLabel.Text     = top3Performers.ElementAtOrDefault(1)?.TotalPoints;
        }

        private void SetThirdPerformerLabels(List<PerformerStatistics> top3Performers)
        {
            thirdPerformerPositionLabel.Text   = "3";
            thirdPerformerNameRatingLabel.Text = top3Performers.LastOrDefault()?.FIO;
            thirdPerformerPointsLabel.Text     = top3Performers.LastOrDefault()?.TotalPoints;
        }

        private void StatisticsToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            mainFormTabControl.TabPages.Clear();
            mainFormTabControl.TabPages.Add(statisticsTabPage);

            CustomizeComboBox(StatisticsUser.Performer);
            FormHelper.ClearLabelsByTagName(RatingGroupBox.Controls, "needToUpdateLabel");
        }

        private void AboutAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void DocumentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var helpFile = Path.Combine(Directory.GetCurrentDirectory() + "\\Help.docx");
            Process.Start(helpFile);
        }
    }
}