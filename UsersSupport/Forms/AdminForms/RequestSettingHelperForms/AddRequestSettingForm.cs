using System;
using System.Collections;
using UsersSupport.Data;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.AdminForms.RequestSettingHelperForms
{
    public class AddRequestSettingForm : SharedRequestSettingForm
    {
        public AddRequestSettingForm(MainForm mainForm) : base(mainForm)
        {
            RenderForm();
            leftBtn.Click += AddRequestSettingBtn_Click;
        }

        private void AddRequestSettingBtn_Click(object sender, EventArgs e)
        {
            ExceptionOccured = false;

            if (AllInputsValid())
                AddRequestSetting();
        }

        private void AddRequestSetting()
        {
            var normExecutionTimeInSeconds = (daysNumericUpDownn.Value * OneDayInSeconds) +
                                             (hoursNumericUpDown.Value * OneHourInSeconds) +
                                             (minutesNumericUpDown.Value * OneMinuteInSeconds);

            var normExecutionTimeReadable  = ConvertSecondsToReadableTime(normExecutionTimeInSeconds);

            var cmbBoxes = FormHelper.GetCmbBoxesValues(Controls);
            var txtBoxes = FormHelper.GetTxtBoxesValues(Controls);

            var db = new DataAccess();
            db.SuccessRecordInsert          += SuccessAddedRequestSetting;
            db.RequestSettingExistException += Db_RequestSettingExistException;
            db.AddRequestSetting(new Hashtable
            {
                { "requestType", cmbBoxes["requestTypesCmbBox"] },
                { "requestTheme", cmbBoxes["requestThemesCmbBox"] },
                { "description", txtBoxes["descriptionTxtBox"] },
                { "normExecutionTimeInSeconds", normExecutionTimeInSeconds },
                { "normExecutionTimeReadable", normExecutionTimeReadable }
            });

            if (!ExceptionOccured)
                Close();
        }

        private void SuccessAddedRequestSetting(object sender, EventArgs e)
        {
            MainFrm.RenderDataGridViewRequestsSettings();
            FormHelper.SuccessRecordInserted();
        }

        private void RenderForm()
        {
            CustomizeControls();
            BindCmbBoxes();
        }

        private void CustomizeControls()
        {
            Text          = "Настройка заявки";
            lbl.Text      = "Добавьте настройку для выбраного типа и темы заявки";
            leftBtn.Image = imageList1.Images["addImg"];
        }
    }
}
