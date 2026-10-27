namespace cinema_project.UI.Forms
{
    partial class Dangky
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
            lblUsername = new Label();
            lblPassword = new Label();
            lblConfirm = new Label();
            lblTitle = new Label();
            btnRegister = new Button();
            btnCancel = new Button();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            SuspendLayout();
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(80, 126);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(110, 20);
            lblUsername.TabIndex = 1;
            lblUsername.Text = "Tên đăng nhập:";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(80, 207);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(73, 20);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "Mật khẩu:";
            // 
            // lblConfirm
            // 
            lblConfirm.AutoSize = true;
            lblConfirm.Location = new Point(80, 278);
            lblConfirm.Name = "lblConfirm";
            lblConfirm.Size = new Size(98, 20);
            lblConfirm.TabIndex = 3;
            lblConfirm.Text = "Xác nhận MK:";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(366, 31);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(151, 20);
            lblTitle.TabIndex = 4;
            lblTitle.Text = "TẠO TÀI KHOẢN MỚI";
            // 
            // btnRegister
            // 
            btnRegister.Location = new Point(319, 361);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(94, 29);
            btnRegister.TabIndex = 5;
            btnRegister.Text = "Đăng ký";
            btnRegister.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(519, 361);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(286, 126);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(327, 27);
            textBox2.TabIndex = 8;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(286, 207);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(327, 27);
            textBox3.TabIndex = 9;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(286, 278);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(327, 27);
            textBox4.TabIndex = 10;
            // 
            // Dangky
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(btnCancel);
            Controls.Add(btnRegister);
            Controls.Add(lblTitle);
            Controls.Add(lblConfirm);
            Controls.Add(lblPassword);
            Controls.Add(lblUsername);
            Name = "Dangky";
            Text = "Dangky";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblUsername;
        private Label lblPassword;
        private Label lblConfirm;
        private Label lblTitle;
        private Button btnRegister;
        private Button btnCancel;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
    }
}