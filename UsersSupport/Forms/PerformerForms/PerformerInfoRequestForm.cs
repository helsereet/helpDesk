using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UsersSupport.Data;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.PerformerForms
{
    public class PerformerInfoRequestForm : RequestForm
    {
        public PerformerInfoRequestForm(MainForm mainForm, int requestId) : base(mainForm, requestId)
        {
            TakeRequestClicked   += TakeRequestHandler;
            ReturnRequestClicked += ReturnRequestHandler;
            SolveRequestClicked  += SolveRequestHandler;

            CustomizeControls();
        }

        private void SolveRequestHandler(object sender, EventArgs e)
        {
            if (FormHelper.AtLeastOneTxtBoxIsEmpty(solutionTextBox))
                MessageBox.Show("Решение не может быть пустым!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                SolveRequest();
        }

        private void SolveRequest()
        {
            var db = new DataAccess();
            db.SolveRequest(new Hashtable
            {
                { "requestId", RequestId },
                { "solvedByEmployeeId", MainForm.User.Id },
                { "solution",  solutionTextBox.Text }
            });

            UpdateForm();
        }

        private void UpdateForm()
        {
            RenderForm();
            RenderButtons();
        }

        private void ReturnRequestHandler(object sender, EventArgs e)
        {
            if (FormHelper.ConfirmReturnRequest())
                ReturnRequest();
        }

        private void ReturnRequest()
        {
            var db = new DataAccess();
            db.ReturnRequest(RequestId);

            UpdateForm();
        }

        private void TakeRequestHandler(object sender, EventArgs e)
        {
            if (Request.LastTakenByEmployeeId == MainForm.User.Id)
                MessageBox.Show("Вы не можете взять заявку, так как вы ее уже отклонили", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (FormHelper.ConfirmTakeRequest())
                TakeRequest();
        }

        private void TakeRequest()
        {
            var db                  = new DataAccess();
            db.RequestAlreadyTaken += Db_RequestAlreadyTaken;
            db.SuccessTakenRequest += Db_SuccessTakenRequest;
            db.TakeRequest(MainForm.User.Id, RequestId);

            UpdateForm();
        }

        private void Db_SuccessTakenRequest(object sender, EventArgs e)
        {
            MessageBox.Show("Вы первый забрали заявку. Форма будет перерисована", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Db_RequestAlreadyTaken(object sender, EventArgs e)
        {
            MessageBox.Show("Данная заявка была УЖЕ принята другим исполнителем. Форма будет перерисована", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void CustomizeControls()
        {
            HideOtherRolesPanel();
            RenderButtons();
        }

        private void HideOtherRolesPanel()
        {
            adminPanel.Visible                 = false;
            customerCloseRequestPanel.Visible  = false;
            performerTakeRequestPanel.Visible  = false;
            performerSolveRequestPanel.Visible = false;
        }

        private void RenderButtons()
        {
            PrepareControlsBeforeChoosing();

            if (Request.Closed || Request.Solved ||
               (Request.Solved && !Request.Closed))
            { PrepareControlsForJustLookingInfo(); }
            else if (!Request.Closed && !Request.Solved && Request.Taken)
            {
                if (Request.LastTakenByEmployeeId == MainForm.User.Id)
                    PrepareControlsForSolve();
                else customPanel.Visible = false;
            }
            else if (!Request.Closed && !Request.Taken)
            {
                performerTakeRequestPanel.Visible = true;
                returnRequestButton.Enabled = false;
            }
            else { ProgramLogicIsBroken(); }
        }

        private void ProgramLogicIsBroken()
        {
            MessageBox.Show("Логика программы сломана, обратитесь к администратору", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
        }

        private void PrepareControlsForJustLookingInfo()
        {
            customPanel.Visible      = false;
            solutionTextBox.ReadOnly = true;
        }

        private void PrepareControlsForSolve()
        {
            performerTakeRequestPanel.Visible  = true;
            performerSolveRequestPanel.Visible = true;
            takeRequestButton.Enabled          = false;
            solutionTextBox.ReadOnly           = false;
        }

        private void PrepareControlsBeforeChoosing()
        {
            performerTakeRequestPanel.Visible  = false;
            performerSolveRequestPanel.Visible = false;

            takeRequestButton.Enabled          = true;
            returnRequestButton.Enabled        = true;
        }
    }
}
