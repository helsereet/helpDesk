namespace UsersSupport.Forms.GeneralForms
{
    partial class RoleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoleForm));
            this.asCustomerRdBtn = new System.Windows.Forms.RadioButton();
            this.asPerformerRdBtn = new System.Windows.Forms.RadioButton();
            this.asAdminRdBtn = new System.Windows.Forms.RadioButton();
            this.loginBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // asCustomerRdBtn
            // 
            this.asCustomerRdBtn.AutoSize = true;
            this.asCustomerRdBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.asCustomerRdBtn.Location = new System.Drawing.Point(32, 35);
            this.asCustomerRdBtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.asCustomerRdBtn.Name = "asCustomerRdBtn";
            this.asCustomerRdBtn.Size = new System.Drawing.Size(434, 24);
            this.asCustomerRdBtn.TabIndex = 0;
            this.asCustomerRdBtn.TabStop = true;
            this.asCustomerRdBtn.Text = "Войти пользователем (создание заявок на техподдержку)";
            this.asCustomerRdBtn.UseVisualStyleBackColor = true;
            // 
            // asPerformerRdBtn
            // 
            this.asPerformerRdBtn.AutoSize = true;
            this.asPerformerRdBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.asPerformerRdBtn.Location = new System.Drawing.Point(32, 74);
            this.asPerformerRdBtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.asPerformerRdBtn.Name = "asPerformerRdBtn";
            this.asPerformerRdBtn.Size = new System.Drawing.Size(428, 24);
            this.asPerformerRdBtn.TabIndex = 1;
            this.asPerformerRdBtn.TabStop = true;
            this.asPerformerRdBtn.Text = "Войти исполнителем (работа с заявками пользователей)";
            this.asPerformerRdBtn.UseVisualStyleBackColor = true;
            // 
            // asAdminRdBtn
            // 
            this.asAdminRdBtn.AutoSize = true;
            this.asAdminRdBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.asAdminRdBtn.Location = new System.Drawing.Point(32, 113);
            this.asAdminRdBtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.asAdminRdBtn.Name = "asAdminRdBtn";
            this.asAdminRdBtn.Size = new System.Drawing.Size(423, 24);
            this.asAdminRdBtn.TabIndex = 2;
            this.asAdminRdBtn.TabStop = true;
            this.asAdminRdBtn.Text = "Войти администратором (адимнистрирование системы)";
            this.asAdminRdBtn.UseVisualStyleBackColor = true;
            // 
            // loginBtn
            // 
            this.loginBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginBtn.Location = new System.Drawing.Point(115, 148);
            this.loginBtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.loginBtn.Name = "loginBtn";
            this.loginBtn.Size = new System.Drawing.Size(94, 30);
            this.loginBtn.TabIndex = 3;
            this.loginBtn.Text = "Вход";
            this.loginBtn.UseVisualStyleBackColor = true;
            this.loginBtn.Click += new System.EventHandler(this.LoginBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelBtn.Location = new System.Drawing.Point(265, 148);
            this.cancelBtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(96, 30);
            this.cancelBtn.TabIndex = 4;
            this.cancelBtn.Text = "Отмена";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // RoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 185);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.loginBtn);
            this.Controls.Add(this.asAdminRdBtn);
            this.Controls.Add(this.asPerformerRdBtn);
            this.Controls.Add(this.asCustomerRdBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MinimizeBox = false;
            this.Name = "RoleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Форма выбора роли пользователя в текущем сеансе";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton asCustomerRdBtn;
        private System.Windows.Forms.RadioButton asPerformerRdBtn;
        private System.Windows.Forms.RadioButton asAdminRdBtn;
        private System.Windows.Forms.Button loginBtn;
        private System.Windows.Forms.Button cancelBtn;
    }
}