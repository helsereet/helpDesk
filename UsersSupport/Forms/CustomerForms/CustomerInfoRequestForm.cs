using System;
using System.Windows.Forms;
using UsersSupport.Data;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.CustomerForms
{
    public class CustomerInfoRequestForm : RequestForm
    {
        public CustomerInfoRequestForm(MainForm mainForm, int requestId) : base(mainForm, requestId)
        {
            CloseRequestClicked += CloseRequestHandler;

            CustomizeControls();
        }

        private void CloseRequestHandler(object sender, EventArgs e)
        {
            if (customerMarkComboBox.SelectedIndex == -1)
                MessageBox.Show("Пожалуйста, оцените исполнителя", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                CloseRequest();
        }

        private void CloseRequest()
        {
            var itogPerformerPoints  = CalculatePerformerPoints() + Convert.ToInt32(customerMarkComboBox.SelectedItem);
            var db                   = new DataAccess();
            db.RequestAlreadyClosed += Db_RequestAlreadyClosed;
            db.SuccessClosedRequest += Db_SuccessClosedRequest;
            db.CloseRequest(RequestId, Convert.ToInt32(customerMarkComboBox.SelectedItem), (byte)itogPerformerPoints);

            UpdateForm();
        }

        private int CalculatePerformerPoints()
        {
            // 0 - 20% = 1 point, 20 - 40% - 2 points and so on(max points is 20)

            var percent = (Request.NormExecutionTimeInSeconds / Request.SpentOnSolutionInSeconds) * 100;
            var count   = 0;

            if (percent <= 0)
                return -1;
            else if (percent >= 180)
                return 10;
            else
            {
                while (percent >= 0)
                {
                    count++;
                    percent -= 20;
                }
                return count;
            }
        }

        private void UpdateForm()
        {
            RenderForm();
            RenderButtons();
        }

        private void Db_SuccessClosedRequest(object sender, EventArgs e)
        {
            MessageBox.Show("Вы успешно закрыли заявку!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Db_RequestAlreadyClosed(object sender, EventArgs e)
        {
            MessageBox.Show("Заявка уже была закрыта вами. Если это были не вы, обратитесь к администратору", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void CustomizeControls()
        {
            customPanel.Visible = false;
            RenderButtons();
        }

        private void RenderButtons()
        {
            if (Request.Solved && !Request.Closed)
                customerCloseRequestPanel.Visible = true;
            else
                customerCloseRequestPanel.Visible = false;
        }
    }
}
