using System;
using System.Windows.Forms;
using UsersSupport.Data;

namespace UsersSupport.Forms.GeneralForms
{
    public partial class RequestForm : Form
    {
        protected internal event EventHandler TakeRequestClicked;

        protected internal event EventHandler ReturnRequestClicked;

        protected internal event EventHandler SolveRequestClicked;

        protected internal event EventHandler CloseRequestClicked;

        protected internal MainForm MainForm { get; }

        protected internal int RequestId { get; set; }

        protected internal Request Request { get; set; }

        public RequestForm()
        {
            InitializeComponent();
            CancelButton = closeButton;
        }

        public RequestForm(MainForm mainForm, int requestId) : this()
        {
            MainForm  = mainForm;
            RequestId = requestId;

            RenderForm();
        }

        protected internal void RenderForm()
        {
            FormHelper.HideHeaderTabPage(requestTabControl);
            GetRequestInfo();
            SetTitleLabel();
            FillLabelsFromRequest();
        }

        private void SetTitleLabel()
        {
            requestTitleLabel.Text = $"Заявка {RequestId.ToString()}";

            if (Request.Closed)
                requestTitleLabel.Text += " (Закрыта)";
            else if (!Request.Closed && Request.Solved)
                requestTitleLabel.Text += " (Решена, не закрыта)";
            else if (!Request.Closed && !Request.Solved && Request.Taken)
                requestTitleLabel.Text += " (Взята, не решена)";
            else requestTitleLabel.Text += " (Открыта, не взята)";
        }

        private void GetRequestInfo()
        {
            var db = new DataAccess();
            Request = db.GetRequest(RequestId);
        }

        private void FillLabelsFromRequest()
        {
            typeRequestLabel.Text        = Request?.TypeName;
            themeRequestLabel.Text       = Request?.ThemeName;
            requestDescriptionLabel.Text = Request?.Description;
            shortDescriptionTextBox.Text = Request?.ShortDescription;
            fullDescriptionTextBox.Text  = Request?.FullDescription;
            solutionTextBox.Text         = Request?.Solution;
            customerUserNameLabel.Text   = Request?.CustomerFullName;
            performerUserNameLabel.Text  = Request?.PerformerFullName;
            customerMarkLabel.Text       = Request?.CustomerMark.ToString();
            performerMarkLabel.Text      = Request?.PerformerMark.ToString();
            createdAtLabel.Text          = Request?.CreatedAt.ToString();
            solvedAtLabel.Text           = Request?.SolvedAt.ToString();
            closedAtLabel.Text           = Request?.ClosedAt.ToString();
            spentOnSolutionTime.Text     = Request?.SpentOnSolutionInSeconds.ToString();
        }

        private void TakeRequestButton_Click(object sender, EventArgs e)
        { this?.TakeRequestClicked.Invoke(this, null); }

        private void ReturnRequestButton_Click(object sender, EventArgs e)
        { ReturnRequestClicked?.Invoke(this, null); }

        private void SolveRequestButton_Click(object sender, EventArgs e)
        { SolveRequestClicked?.Invoke(this, null); }

        private void CloseRequestButton_Click(object sender, EventArgs e)
        { CloseRequestClicked?.Invoke(this, null); }
    }
}
