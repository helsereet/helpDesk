using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UsersSupport
{
    public static class FormHelper
    {
        public static void TrimAllTxtBoxes(Control.ControlCollection Controls)
        {
            void func(Control.ControlCollection controls)
            {
                foreach (Control control in controls)
                    if (control is TextBox)
                        (control as TextBox).Text = (control as TextBox).Text.Trim();
                    else
                        func(control.Controls);
            }
            func(Controls);
        }

        public static void ClearLabelsByTagName(Control.ControlCollection Controls, string tagName)
        {
            void func(Control.ControlCollection controls)
            {
                foreach (Control control in controls)
                {
                    if (control is Label && (string)control.Tag == tagName)
                        (control as Label).Text = string.Empty;
                    else
                        func(control.Controls);
                }
            }
            func(Controls);
        }

        public static void MakeLabelsRegularByTagName(Control.ControlCollection Controls, string tagName)
        {
            void func(Control.ControlCollection controls)
            {
                foreach (Control control in controls)
                {
                    if (control is Label && (string)control.Tag == tagName)
                        (control as Label).Font = new Font((control as Label).Font.FontFamily, 10, FontStyle.Regular);
                    else
                        func(control.Controls);
                }
            }
            func(Controls);
        }

        public static void RenderTabPage(TabControl tabControl, TabPage tabPage)
        {
            tabControl.TabPages.Clear();
            tabControl.TabPages.Add(tabPage);
        }

        public static void HideHeaderTabPage(TabControl tabControl)
        {
            tabControl.Appearance = TabAppearance.FlatButtons;
            tabControl.ItemSize = new Size(0, 1);
            tabControl.SizeMode = TabSizeMode.Fixed;
        }

        public static bool AtLeastOneTxtBoxIsEmpty(params TextBox[] txtBoxes)
        {
            foreach (var txtBox in txtBoxes)
                if (string.IsNullOrWhiteSpace(txtBox.Text)) return true;
            return false;
        }

        public static bool ConfirmDeletionRecord()
        {
            return MessageBox.Show("Вы действительно хотите удалить данную запись?",
                                   "Предупреждение", MessageBoxButtons.YesNo,
                                   MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        public static bool ConfirmTakeRequest()
        {
            return MessageBox.Show("Вы действительно хотите взять данную заявку?",
                                   "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        public static void SuccessRecordInserted()
        {
            MessageBox.Show("Запись была успешно добавлена!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void SuccessRecordDeleted()
        {
            MessageBox.Show("Данная запись была успешно удалена!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void SuccessRecordUpdated()
        {
            MessageBox.Show("Запись была успешно изменена!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static Dictionary<string, string> GetTxtBoxesValues(Control.ControlCollection Controls)
        {
            var textBoxes = new Dictionary<string, string>();
            void func(Control.ControlCollection controls)
            {
                foreach (Control control in controls)
                {
                    if (control is TextBox)
                        textBoxes[control.Name] = (control as TextBox).Text;
                    else
                        func(control.Controls);
                }
            }
            func(Controls);
            return textBoxes;
        }

        public static bool ConfirmReturnRequest()
        {
            return MessageBox.Show("Вы действительно хотите отказаться от выполнения данного задания? В данном случае вы больше не сможете его взять",
                       "Предупреждение", MessageBoxButtons.YesNo,
                       MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        public static Dictionary<string, int> GetCmbBoxesValues(Control.ControlCollection Controls)
        {
            var cmbBoxes = new Dictionary<string, int>();
            void func(Control.ControlCollection controls)
            {
                foreach (Control control in controls)
                {
                    if (control is ComboBox)
                        cmbBoxes[control.Name] = Convert.ToInt32((control as ComboBox).SelectedValue);
                    else
                        func(control.Controls);
                }
            }
            func(Controls);
            return cmbBoxes;
        }

        public static Dictionary<string, bool> GetChkBoxesValues(Control.ControlCollection Controls)
        {
            var chkBoxes = new Dictionary<string, bool>();
            void func(Control.ControlCollection controls)
            {
                foreach (Control control in controls)
                {
                    if (control is CheckBox)
                        chkBoxes[control.Name] = (control as CheckBox).Checked;
                    else
                        func(control.Controls);
                }
            }
            func(Controls);
            return chkBoxes;
        }

        public static void MakelabelsBold(params Label[] labels)
        {
            foreach (var label in labels)
                label.Font = new Font(label.Font.FontFamily, 14, FontStyle.Bold);
        }
    }
}