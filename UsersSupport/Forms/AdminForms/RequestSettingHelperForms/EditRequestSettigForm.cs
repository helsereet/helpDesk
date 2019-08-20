using System;
using System.Collections;
using System.Windows.Forms;
using UsersSupport.Data;
using UsersSupport.Forms.GeneralForms;


namespace UsersSupport.Forms.AdminForms.RequestSettingHelperForms
{
    public class EditRequestSettigForm : SharedRequestSettingForm
    {
        public EditRequestSettigForm(MainForm mainForm, DataGridView dataGridView) : base(mainForm, dataGridView)
        {
            RenderForm();
            leftBtn.Click += EditRequestSettingBtn_Click;
        }

        private void RenderForm()
        {
            CustomizeControls();

            BindCmbBoxes();
            SetComboxBoxesStartValue();
            SetTextBoxesStartValue();
            SetNumericsUpDownStartValue();
        }

        private void SetTextBoxesStartValue()
        {
            descriptionTxtBox.Text = DataGrid[DataGrid.Columns["Description"].Index, DataGrid.CurrentRow.Index].Value as string;
        }

        private void SetNumericsUpDownStartValue()
        {
            var pairs                  = ConvertSecondsToDaysHoursMinutes(Convert.ToDecimal(DataGrid[DataGrid.Columns["NormExecutionTimeInSeconds"].Index, 
                                                                                            DataGrid.CurrentRow.Index].Value));
            daysNumericUpDownn.Value   = pairs["days"];
            hoursNumericUpDown.Value   = pairs["hours"];
            minutesNumericUpDown.Value = pairs["minutes"];
        }

        private void SetComboxBoxesStartValue()
        {
            requestTypesCmbBox.SelectedIndex  = requestTypesCmbBox.FindStringExact(DataGrid[DataGrid.Columns["TypeName"].Index, 
                                                                                   DataGrid.CurrentRow.Index].Value as string);
            requestThemesCmbBox.SelectedIndex = requestThemesCmbBox.FindStringExact(DataGrid[DataGrid.Columns["ThemeName"].Index, 
                                                                                    DataGrid.CurrentRow.Index].Value as string);
        }

        private void CustomizeControls()
        {
            Text          = "Настройка заявки";
            lbl.Text      = "Добавьте настройку для выбраного типа и темы заявки";
            leftBtn.Image = imageList1.Images["addImg"];
        }

        private void EditRequestSettingBtn_Click(object sender, EventArgs e)
        {
            ExceptionOccured = false;

            if (AllInputsValid())
                EditRequestSetting();
        }

        private void EditRequestSetting()
        {
            var cmbBoxes = FormHelper.GetCmbBoxesValues(Controls);
            var txtBoxes = FormHelper.GetTxtBoxesValues(Controls);
            var id       = Convert.ToInt32(DataGrid[DataGrid.Columns["Id"].Index, DataGrid.CurrentRow.Index].Value);
            var normExecutionTimeInSeconds = (daysNumericUpDownn.Value * OneDayInSeconds) +
                                             (hoursNumericUpDown.Value * OneHourInSeconds) +
                                             (minutesNumericUpDown.Value * OneMinuteInSeconds);

            var db = new DataAccess();
            db.SuccessRecordUpdate          += SuccessUpdatedRequestSetting;
            db.RequestSettingExistException += Db_RequestSettingExistException;

            db.UpdateRequestSetting(new Hashtable
            {
                { "requestType", cmbBoxes["requestTypesCmbBox"] },
                { "requestTheme", cmbBoxes["requestThemesCmbBox"] },
                { "description", txtBoxes["descriptionTxtBox"] },
                { "normExecutionTimeInSeconds", normExecutionTimeInSeconds },
                { "normExecutionTimeReadable", ConvertSecondsToReadableTime(normExecutionTimeInSeconds) },
                { "id", id }
            });

            if (!ExceptionOccured)
                Close();
        }

        private void SuccessUpdatedRequestSetting(object sender, EventArgs e)
        {
            MainFrm.RenderDataGridViewRequestsSettings();
            FormHelper.SuccessRecordUpdated();
        }
    }
}
