using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UsersSupport.Data;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.AdminForms.RequestSettingHelperForms
{
    public partial class SharedRequestSettingForm : Form
    {
        protected internal DataGridView DataGrid { get; set; }

        protected internal MainForm MainFrm { get; set; }

        protected internal bool ExceptionOccured { get; set; } = false;

        public SharedRequestSettingForm()
        {
            InitializeComponent();
            CancelButton = cancelBtn;
            AcceptButton = leftBtn;
        }

        public SharedRequestSettingForm(MainForm mainForm, DataGridView dataGridView) : this(mainForm)
        {
            DataGrid = dataGridView;
        }

        public SharedRequestSettingForm(MainForm mainForm) : this()
        {
            MainFrm = mainForm;
        }

        protected internal const int OneDayInSeconds = 86400;

        protected internal const int OneHourInSeconds = 3600;

        protected internal const int OneMinuteInSeconds = 60;

        protected internal void BindCmbBoxes()
        {
            BindRequestThemesCmbBox();
            BindRequestTypesCmbBox();
        }

        private void BindRequestThemesCmbBox()
        {
            var db            = new DataAccess();
            var requestThemes = db.GetRequestThemes();

            requestThemesCmbBox.DataSource    = requestThemes;
            requestThemesCmbBox.DisplayMember = "ThemeName";
            requestThemesCmbBox.ValueMember   = "Id";
        }

        private void BindRequestTypesCmbBox()
        {
            var db           = new DataAccess();
            var requestTypes = db.GetRequestTypes();

            requestTypesCmbBox.DataSource    = requestTypes;
            requestTypesCmbBox.DisplayMember = "TypeName";
            requestTypesCmbBox.ValueMember   = "Id";
        }

        protected internal void Db_RequestSettingExistException(object sender, EventArgs e)
        {
            ExceptionOccured = true;
            MessageBox.Show("Настройка к данному типу и теме заявки уже существует!", "",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected internal Dictionary<string, decimal> ConvertSecondsToDaysHoursMinutes(decimal timeInSeconds)
        {
            decimal days    = 0;
            decimal hours   = 0;
            decimal minutes = 0;

            while (timeInSeconds / OneDayInSeconds >= 1)
            {
                timeInSeconds -= OneDayInSeconds;
                days++;
            }

            while (timeInSeconds / OneHourInSeconds >= 1)
            {
                timeInSeconds -= OneHourInSeconds;
                hours++;
            }

            while (timeInSeconds / OneMinuteInSeconds >= 1)
            {
                timeInSeconds -= OneMinuteInSeconds;
                minutes++;
            }

            return new Dictionary<string, decimal>
            {
                { "days", days },
                { "hours", hours },
                { "minutes", minutes }
            };
        }

        protected internal bool AllInputsValid()
        {
            if (FormHelper.AtLeastOneTxtBoxIsEmpty(descriptionTxtBox))
            {
                MessageBox.Show("Описание не должно быть пустым!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (daysNumericUpDownn.Value == 0 &&
                     hoursNumericUpDown.Value == 0 &&
                     minutesNumericUpDown.Value == 0)
            {
                MessageBox.Show("Норма выполнения не может быть пустой!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        protected internal string ConvertSecondsToReadableTime(decimal timeInSeconds)
        {
            var pairs = ConvertSecondsToDaysHoursMinutes(timeInSeconds);

            return $"{ pairs["days"] } дн., { pairs["hours"] } час., { pairs["minutes"] } мин.";
        }
    }
}
