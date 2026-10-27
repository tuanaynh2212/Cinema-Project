namespace cinema_project.UI.Forms
{
    partial class QuenMK
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
            lblTitle = new Label();
            lblEmailOrUser = new Label();
            lblOTP = new Label();
            lblNewPass = new Label();
            lblConfirmPass = new Label();
            btnResetPass = new Button();
            btnCancel = new Button();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(364, 25);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(128, 20);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "QUÊN MẬT KHẨU";
            // 
            // lblEmailOrUser
            // 
            lblEmailOrUser.AutoSize = true;
            lblEmailOrUser.Location = new Point(47, 81);
            lblEmailOrUser.Name = "lblEmailOrUser";
            lblEmailOrUser.Size = new Size(161, 20);
            lblEmailOrUser.TabIndex = 1;
            lblEmailOrUser.Text = "Tên đăng nhập / Email:";
            // 
            // lblOTP
            // 
            lblOTP.AutoSize = true;
            lblOTP.Location = new Point(47, 143);
            lblOTP.Name = "lblOTP";
            lblOTP.Size = new Size(135, 20);
            lblOTP.TabIndex = 2;
            lblOTP.Text = "Mã xác nhận (OTP):";
            // 
            // lblNewPass
            // 
            lblNewPass.AutoSize = true;
            lblNewPass.Location = new Point(47, 212);
            lblNewPass.Name = "lblNewPass";
            lblNewPass.Size = new Size(111, 20);
            lblNewPass.TabIndex = 3;
            lblNewPass.Text = "lblEmailOrUser:";
            // 
            // lblConfirmPass
            // 
            lblConfirmPass.AutoSize = true;
            lblConfirmPass.Location = new Point(47, 274);
            lblConfirmPass.Name = "lblConfirmPass";
            lblConfirmPass.Size = new Size(128, 20);
            lblConfirmPass.TabIndex = 4;
            lblConfirmPass.Text = "Xác nhận MK mới:";
            // 
            // btnResetPass
            // 
            btnResetPass.Location = new Point(300, 349);
            btnResetPass.Name = "btnResetPass";
            btnResetPass.Size = new Size(133, 29);
            btnResetPass.TabIndex = 5;
            btnResetPass.Text = "Đổi mật khẩu";
            btnResetPass.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(537, 349);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(281, 81);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(350, 27);
            textBox1.TabIndex = 7;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(281, 143);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(350, 27);
            textBox2.TabIndex = 8;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(281, 212);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(350, 27);
            textBox3.TabIndex = 9;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(281, 274);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(350, 27);
            textBox4.TabIndex = 10;
            // 
            // QuenMK
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(btnCancel);
            Controls.Add(btnResetPass);
            Controls.Add(lblConfirmPass);
            Controls.Add(lblNewPass);
            Controls.Add(lblOTP);
            Controls.Add(lblEmailOrUser);
            Controls.Add(lblTitle);
            Name = "QuenMK";
            Text = "QuenMK";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblEmailOrUser;
        private Label lblOTP;
        private Label lblNewPass;
        private Label lblConfirmPass;
        private Button btnResetPass;
        private Button btnCancel;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
    }
}