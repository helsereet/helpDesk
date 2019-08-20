using System;
using System.Windows.Forms;
using UsersSupport.Data;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.AdminForms.PositionHelperForms
{
    public partial class AddPositionForm : SharedPositionForm
    {
        public AddPositionForm(MainForm mainForm) : base(mainForm)
        {
            Init();
            leftBtn.Click += AddPositionBtn_Click;
        }

        private void Init()
        {
            lbl.Text      = "Добавьте новую должность";
            Name          = "Добавление должности";
            leftBtn.Image = imageList1.Images["addImg"];
        }

        private void AddPositionBtn_Click(object sender, EventArgs e)
        {
            ExceptionOccured = false;
            if (FormHelper.AtLeastOneTxtBoxIsEmpty(txtBox))
            {
                MessageBox.Show("Поле не должно быть пустым", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var db                     = new DataAccess();
            db.PositionExistException += Db_PositionExistException;
            db.SuccessRecordInsert    += SuccessAddedPosition;
            db.SimpleInsert("POSITION", txtBox.Text.Trim());
            if (!ExceptionOccured)
                Close();
        }

        private void SuccessAddedPosition(object sender, EventArgs e)
        {
            MainFrm.RenderDataGridViewPositions();
            FormHelper.SuccessRecordInserted();
        }
    }
}
