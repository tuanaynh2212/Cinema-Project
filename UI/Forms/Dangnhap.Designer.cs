namespace cinema_project.UI.Forms
{
    partial class Dangnhap
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
            btndangnhap = new Button();
            btnthoat = new Button();
            lbldangnhap = new Label();
            lblUsername = new Label();
            lblPassword = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            SuspendLayout();
            // 
            // btndangnhap
            // 
            btndangnhap.Location = new Point(322, 332);
            btndangnhap.Name = "btndangnhap";
            btndangnhap.Size = new Size(94, 29);
            btndangnhap.TabIndex = 0;
            btndangnhap.Text = "Đăng Nhập";
            btndangnhap.UseVisualStyleBackColor = true;
            btndangnhap.Click += btndangnhap_Click;
            // 
            // btnthoat
            // 
            btnthoat.Location = new Point(554, 332);
            btnthoat.Name = "btnthoat";
            btnthoat.Size = new Size(94, 29);
            btnthoat.TabIndex = 1;
            btnthoat.Text = "Thoát";
            btnthoat.UseVisualStyleBackColor = true;
            btnthoat.Click += button2_Click;
            // 
            // lbldangnhap
            // 
            lbldangnhap.AutoSize = true;
            lbldangnhap.Location = new Point(365, 43);
            lbldangnhap.Name = "lbldangnhap";
            lbldangnhap.Size = new Size(118, 20);
            lbldangnhap.TabIndex = 2;
            lbldangnhap.Text = "Cinema Logging";
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(125, 129);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(76, 20);
            lblUsername.TabIndex = 3;
            lblUsername.Text = "Tài Khoản:";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(125, 231);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(75, 20);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Mật Khẩu:";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(241, 129);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(407, 27);
            txtUsername.TabIndex = 5;
            txtUsername.TextChanged += textBox1_TextChanged;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(241, 231);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(407, 27);
            txtPassword.TabIndex = 6;
            txtPassword.TextChanged += txtPassword_TextChanged;
            // 
            // Dangnhap
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(lblPassword);
            Controls.Add(lblUsername);
            Controls.Add(lbldangnhap);
            Controls.Add(btnthoat);
            Controls.Add(btndangnhap);
            Name = "Dangnhap";
            Text = "Dangnhap";
            Load += Dangnhap_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btndangnhap;
        private Button btnthoat;
        private Label lbldangnhap;
        private Label lblUsername;
        private Label lblPassword;
        private TextBox txtUsername;
        private TextBox txtPassword;
    }
}