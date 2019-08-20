using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using UsersSupport.Data;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.CustomerForms
{
    public partial class CreateRequestForm : Form
    {
        public List<RequestTypeRequestTheme> RequestTypeRequestThemes { get; set; } = new List<RequestTypeRequestTheme>();

        public int RequestTypeId { get; set; }

        public int RequestThemeId { get; set; }

        public int RequestTypeRequestThemeId { get; set; }

        public List<RequestType> RequestTypes { get; set; } = new List<RequestType>();

        public List<RequestTheme> RequestThemes { get; set; } = new List<RequestTheme>();

        public MainForm MainForm { get; }

        public CreateRequestForm()
        {
            InitializeComponent();

            BindCloseButtons();
            RenderForm();
        }

        public CreateRequestForm(MainForm mainForm) : this()
        {
            MainForm = mainForm;
        }

        private void BindCloseButtons()
        {
            CancelButton = closeBtn1;
            CancelButton = closeBtn2;
            CancelButton = closeBtn3;
        }

        private void RenderForm()
        {
            FormHelper.HideHeaderTabPage(createRequestTabControl);
            RenderRequestTypesTabPage();
        }

        private void RenderRequestTypesTabPage()
        {
            FormHelper.RenderTabPage(createRequestTabControl, requestTypesTabPage);
            BindRequestTypesListBox();
        }

        private void RenderRequestThemesTabPage()
        {
            RequestTypeId = Convert.ToInt32(requestTypesListBox?.SelectedValue);
            FormHelper.RenderTabPage(createRequestTabControl, requestThemesTabPage);

            BindRequestThemesListBox();
            GetRequestThemesDescription();
            ChangeThemeDescription();
        }

        private void GetRequestThemesDescription()
        {
            var db                   = new DataAccess();
            RequestTypeRequestThemes = db.GetRequestTypeRequestThemes(RequestTypeId);
        }

        private void RenderSummaryTabPage()
        {
            FormHelper.RenderTabPage(createRequestTabControl, summaryTabPage);
        }

        private void BindRequestThemesListBox()
        {
            var db        = new DataAccess();
            RequestThemes = db.GetRequestThemes(RequestTypeId);

            DataHelper.BindListBox(RequestThemes, requestThemesListBox, new Dictionary<string, string>
            {
                { "DisplayMember", "ThemeName" },
                { "ValueMember", "Id"},
            });
        }

        private void BindRequestTypesListBox()
        {
            var db       = new DataAccess();
            RequestTypes = db.GetRequestTypes();

            DataHelper.BindListBox(RequestTypes, requestTypesListBox, new Dictionary<string, string>
            {
                { "DisplayMember", "TypeName" },
                { "ValueMember", "Id" }
            });
        }

        private void GoToThemeBtn1_Click(object sender, EventArgs e)
        {
            RenderRequestThemesTabPage();
        }

        private void GoToTypeBtn_Click(object sender, EventArgs e)
        {
            RenderRequestTypesTabPage();
        }

        private void ChangeLabelsName(ListBox listBox, params Label[] labels)
        {
            foreach (var label in labels)
                label.Text = listBox.GetItemText(listBox.SelectedItem);
        }

        private void ChangeLabelsName(string name, params Label[] labels)
        {
            foreach (var label in labels)
                label.Text = name ?? "";
        }

        private void GoToDetailsBtn_Click(object sender, EventArgs e)
        {
            RequestThemeId            = Convert.ToInt32(requestThemesListBox?.SelectedValue);
            RequestTypeRequestThemeId = RequestTypeRequestThemes.Where(item => item.RequestTypeId == RequestTypeId && item.RequestThemeId == RequestThemeId)
                                                                .FirstOrDefault().Id;
            RenderSummaryTabPage();
        }

        private void RequestTypesListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            ChangeLabelsName(requestTypesListBox, choosenRequestTypeLbl1,
                             choosenRequestTypeLbl2, choosenRequestTypeLbl3);
        }

        private void RequestThemesListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            ChangeLabelsName(requestThemesListBox, choosenRequestThemeLbl1, choosenRequestThemeLbl2);

            // checking, because that method called always listbox properties changed (in BindListBox method)
            if (requestThemesListBox.ValueMember != string.Empty)
                ChangeThemeDescription();
        }

        private void ChangeThemeDescription()
        {
            RequestThemeId = Convert.ToInt32(requestThemesListBox?.SelectedValue);
            foreach (var item in RequestTypeRequestThemes)
            {
                if (item.RequestThemeId == RequestThemeId)
                {
                    ChangeLabelsName(item?.Description, requestTypeThemeDescriptionLbl1, requestTypeThemeDescriptionLbl2);
                    break;
                }
            }

        }

        private void GoToThemeBtn2_Click(object sender, EventArgs e)
        {
            RenderRequestThemesTabPage();
        }

        private void AddRequestBtn_Click(object sender, EventArgs e)
        {
            if (FormHelper.AtLeastOneTxtBoxIsEmpty(shortRequestDescriptionTextBox, fullRequestDescriptionTextBox))
                MessageFieldsMustBeFilled();
            else AddRequest();
        }

        private void AddRequest()
        {
            var db                  = new DataAccess();
            db.UndefinedException  += ProblemOccuredAddRequest;
            db.SuccessRecordInsert += SuccessAddedRequest;

            db.AddRequest(new Hashtable
            {
                { "requestTypeRequestThemeId", RequestTypeRequestThemeId },
                { "shortDescription", shortRequestDescriptionTextBox.Text },
                { "fullDescription", fullRequestDescriptionTextBox.Text },
                { "createdByEmployeeId", MainForm.User.Id }
            });
        }

        private void SuccessAddedRequest(object sender, EventArgs e)
        {
            MessageBox.Show("Заявка была успешно добавлена", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MainForm.RenderOpenedRequests();
            Close();
        }

        private void ProblemOccuredAddRequest(object sender, EventArgs e)
        {
            MessageBox.Show("Произошла неизвестная ошибка, попробуйте позже!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void MessageFieldsMustBeFilled()
        {
            MessageBox.Show("Все поля должны быть заполнены!", "Заполните необходимые поля",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
