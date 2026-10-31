#nullable enable
using System;
using System.Drawing;
using System.Windows.Forms;

namespace cinema_project.UI.Forms.frmTaiKhoan
{
    partial class frmTaiKhoan
    {
        private static readonly Color MauNen = Color.FromArgb(245, 246, 250);
        private static readonly Color MauChu = Color.FromArgb(35, 35, 42);
        private static readonly Color MauPhu = Color.FromArgb(105, 105, 116);
        private static readonly Color MauVien = Color.FromArgb(226, 227, 233);
        private static readonly Color MauDo = Color.FromArgb(231, 26, 15);

        private readonly Panel _noiDung = new();
        private readonly Button[] _nutDieuHuong = new Button[3];
        private readonly Panel[] _cacTrang = new Panel[3];
        private Button _btnMoDanhSachVe = null!;
        private Button _btnDangXuat = null!;
        private Label _lblHoTenTaiKhoan = null!;
        private Label _lblEmailTaiKhoan = null!;
        private Control _txtEmail = null!;
        private TextBox _txtMatKhauHienTai = null!;
        private TextBox _txtMatKhauMoi = null!;
        private TextBox _txtXacNhanMatKhauMoi = null!;
        private Button _btnDoiMatKhau = null!;

        private void InitializeComponent()
        {
            Text = "CGV Cinemas - Tài khoản của tôi";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1160, 760);
            MinimumSize = new Size(960, 640);
            BackColor = MauNen;
            Font = new Font("Segoe UI", 9.5F);
            AutoScaleMode = AutoScaleMode.Font;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                BackColor = MauNen,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 82));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var header = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(26, 0, 24, 0),
                Margin = Padding.Empty
            };
            header.Paint += (_, e) =>
            {
                using var pen = new Pen(MauVien);
                e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
            };

            var logo = new Label
            {
                AutoSize = true,
                Text = "CGV",
                Font = new Font("Segoe UI", 21F, FontStyle.Bold),
                ForeColor = MauDo,
                Location = new Point(26, 20)
            };
            header.Controls.Add(logo);

            var title = new Label
            {
                AutoSize = true,
                Text = "Tài khoản của tôi",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = MauChu,
                Location = new Point(120, 17)
            };
            header.Controls.Add(title);

            var subtitle = new Label
            {
                AutoSize = true,
                Text = "Hồ sơ cá nhân · Vé đã đặt · Bảo mật",
                Font = new Font("Segoe UI", 9F),
                ForeColor = MauPhu,
                Location = new Point(122, 44)
            };
            header.Controls.Add(subtitle);

            _lblEmailTaiKhoan = new Label
            {
                AutoEllipsis = true,
                Text = "minhanh@example.com",
                Font = new Font("Segoe UI", 9F),
                ForeColor = MauChu,
                TextAlign = ContentAlignment.MiddleRight,
                Size = new Size(230, 28),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(header.ClientSize.Width - 254, 26)
            };
            header.Controls.Add(_lblEmailTaiKhoan);
            header.Resize += (_, _) =>
                _lblEmailTaiKhoan.Location = new Point(header.ClientSize.Width - 254, 26);

            var navigation = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(18, 22, 18, 18),
                Margin = Padding.Empty
            };
            navigation.Paint += (_, e) =>
            {
                using var pen = new Pen(MauVien);
                e.Graphics.DrawLine(pen, navigation.Width - 1, 0, navigation.Width - 1, navigation.Height);
            };

            var avatar = new Panel
            {
                Location = new Point(20, 22),
                Size = new Size(46, 46),
                BackColor = Color.FromArgb(240, 249, 255)
            };
            avatar.Paint += (_, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var brush = new SolidBrush(Color.FromArgb(240, 249, 255));
                e.Graphics.FillEllipse(brush, 1, 1, avatar.Width - 3, avatar.Height - 3);
                using var pen = new Pen(Color.FromArgb(184, 230, 254));
                e.Graphics.DrawEllipse(pen, 1, 1, avatar.Width - 3, avatar.Height - 3);
                using var font = new Font("Segoe UI", 14F, FontStyle.Bold);
                TextRenderer.DrawText(e.Graphics, "N", font,
                    avatar.ClientRectangle, Color.FromArgb(0, 105, 168),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            navigation.Controls.Add(avatar);

            _lblHoTenTaiKhoan = new Label
            {
                AutoEllipsis = true,
                Text = "Nguyễn Minh Anh",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = MauChu,
                Location = new Point(78, 24),
                Size = new Size(146, 22),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            navigation.Controls.Add(_lblHoTenTaiKhoan);
            var accountCaption = new Label
            {
                AutoSize = true,
                Text = "Thành viên CGV",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MauPhu,
                Location = new Point(79, 48)
            };
            navigation.Controls.Add(accountCaption);

            var separator = new Panel
            {
                BackColor = MauVien,
                Location = new Point(20, 88),
                Size = new Size(200, 1),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            navigation.Controls.Add(separator);

            string[] tenMuc = { "Hồ sơ cá nhân", "Vé của tôi", "Cài đặt tài khoản" };
            for (int i = 0; i < tenMuc.Length; i++)
            {
                int trang = i;
                var button = new Button
                {
                    Text = tenMuc[i],
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(12, 0, 6, 0),
                    Size = new Size(214, 44),
                    Location = new Point(8, 108 + (i * 50)),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Tag = trang
                };
                button.FlatAppearance.BorderSize = 0;
                button.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 242, 242);
                button.Click += (_, _) => ChuyenTrang(trang);
                _nutDieuHuong[i] = button;
                navigation.Controls.Add(button);
            }

            _btnDangXuat = new Button
            {
                Text = "Đăng xuất",
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 6, 0),
                Size = new Size(214, 42),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(190, 30, 30),
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            _btnDangXuat.FlatAppearance.BorderSize = 0;
            _btnDangXuat.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 242, 242);
            navigation.Controls.Add(_btnDangXuat);
            navigation.Resize += (_, _) =>
                _btnDangXuat.Location = new Point(8, navigation.ClientSize.Height - _btnDangXuat.Height - 18);

            _noiDung.Dock = DockStyle.Fill;
            _noiDung.BackColor = MauNen;
            _noiDung.Margin = Padding.Empty;
            layout.Controls.Add(header, 0, 0);
            layout.SetColumnSpan(header, 2);
            layout.Controls.Add(navigation, 0, 1);
            layout.Controls.Add(_noiDung, 1, 1);
            Controls.Add(layout);

            _cacTrang[0] = TaoTrangHoSo();
            _cacTrang[1] = TaoTrangVe();
            _cacTrang[2] = TaoTrangCaiDat();

            foreach (Panel page in _cacTrang)
            {
                page.Dock = DockStyle.Fill;
                page.Visible = false;
                _noiDung.Controls.Add(page);
            }
        }

        private Panel TaoTrangHoSo()
        {
            Panel page = TaoKhungTrang(
                "Hồ sơ cá nhân",
                "Thông tin dùng cho tài khoản và các lần đặt vé.");
            FlowLayoutPanel sections = TaoDanhSachKhoi(page);

            Panel card = TaoThe(274);
            card.Controls.Add(TaoTieuDeThe("Thông tin cá nhân", "Cập nhật thông tin liên hệ của bạn."));

            var fields = new TableLayoutPanel
            {
                Location = new Point(22, 72),
                Size = new Size(760, 116),
                ColumnCount = 2,
                RowCount = 2,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.White,
                Margin = Padding.Empty
            };
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            fields.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            fields.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            fields.Controls.Add(TaoTruong("Họ và tên", "Nguyễn Minh Anh"), 0, 0);
            fields.Controls.Add(TaoTruong("Số điện thoại", "090 123 4567"), 1, 0);
            _txtEmail = TaoTruong("Email", "minhanh@example.com", readOnly: true);
            fields.Controls.Add(_txtEmail, 0, 1);
            fields.Controls.Add(TaoTruongNgaySinh(), 1, 1);
            card.Resize += (_, _) => fields.Width = Math.Max(0, card.ClientSize.Width - 44);
            card.Controls.Add(fields);

            var save = TaoNut("Lưu thay đổi", primary: true);
            save.Location = new Point(card.ClientSize.Width - save.Width - 22, 208);
            save.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            card.Resize += (_, _) => save.Location = new Point(card.ClientSize.Width - save.Width - 22, 208);
            card.Controls.Add(save);

            sections.Controls.Add(card);
            return page;
        }

        private Panel TaoTrangVe()
        {
            Panel page = TaoKhungTrang(
                "Vé của tôi",
                "Xem lại lịch chiếu và thông tin các ghế đã đặt.");
            FlowLayoutPanel sections = TaoDanhSachKhoi(page);

            Panel ticketsCard = TaoThe(190);
            ticketsCard.Controls.Add(TaoTieuDeThe(
                "Vé đã đặt",
                "Danh sách vé và ghế của tài khoản đang đăng nhập."));
            ticketsCard.Controls.Add(new Label
            {
                AutoSize = false,
                Text = "Mở danh sách vé để xem suất chiếu, ghế, trạng thái và ngày đặt.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = MauPhu,
                Location = new Point(22, 79),
                Size = new Size(640, 24),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            });

            _btnMoDanhSachVe = TaoNut("Mở danh sách vé");
            _btnMoDanhSachVe.Location = new Point(22, 119);
            ticketsCard.Controls.Add(_btnMoDanhSachVe);

            sections.Controls.Add(ticketsCard);

            return page;
        }

        private Panel TaoTrangCaiDat()
        {
            Panel page = TaoKhungTrang(
                "Cài đặt tài khoản",
                "Quản lý mật khẩu và tuỳ chọn nhận thông tin.");
            FlowLayoutPanel sections = TaoDanhSachKhoi(page);

            Panel security = TaoThe(310);
            security.Controls.Add(TaoTieuDeThe("Đổi mật khẩu", "Tạo mật khẩu mới để bảo vệ tài khoản."));

            var passwordFields = new TableLayoutPanel
            {
                Location = new Point(22, 72),
                Size = new Size(760, 150),
                ColumnCount = 1,
                RowCount = 3,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.White,
                Margin = Padding.Empty
            };
            passwordFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < 3; i++)
                passwordFields.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            passwordFields.Controls.Add(TaoTruongMatKhau("Mật khẩu hiện tại", out _txtMatKhauHienTai), 0, 0);
            passwordFields.Controls.Add(TaoTruongMatKhau("Mật khẩu mới", out _txtMatKhauMoi), 0, 1);
            passwordFields.Controls.Add(TaoTruongMatKhau("Nhập lại mật khẩu mới", out _txtXacNhanMatKhauMoi), 0, 2);
            security.Resize += (_, _) => passwordFields.Width = Math.Max(0, security.ClientSize.Width - 44);
            security.Controls.Add(passwordFields);

            _btnDoiMatKhau = TaoNut("Cập nhật mật khẩu", primary: true);
            _btnDoiMatKhau.Location = new Point(22, 246);
            _btnDoiMatKhau.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            security.Controls.Add(_btnDoiMatKhau);
            sections.Controls.Add(security);

            Panel preferences = TaoThe(116);
            preferences.Controls.Add(TaoTieuDeThe("Thông báo", "Tuỳ chọn nhận thông tin liên quan đến tài khoản."));
            var emailUpdates = new CheckBox
            {
                AutoSize = true,
                Text = "Nhận thông báo về vé và tài khoản qua email",
                Checked = true,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = MauChu,
                Location = new Point(22, 67)
            };
            preferences.Controls.Add(emailUpdates);
            sections.Controls.Add(preferences);

            return page;
        }

        private Panel TaoKhungTrang(string title, string description)
        {
            var page = new Panel { BackColor = MauNen, Padding = new Padding(24, 20, 24, 20) };
            var heading = new Panel
            {
                Dock = DockStyle.Top,
                Height = 68,
                BackColor = MauNen
            };
            heading.Controls.Add(new Label
            {
                AutoSize = true,
                Text = title,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = MauChu,
                Location = new Point(0, 0)
            });
            heading.Controls.Add(new Label
            {
                AutoSize = true,
                Text = description,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = MauPhu,
                Location = new Point(2, 36)
            });

            var body = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = MauNen,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(2)
            };
            body.Name = "pageBody";
            page.Controls.Add(body);
            page.Controls.Add(heading);
            return page;
        }

        private static FlowLayoutPanel TaoDanhSachKhoi(Panel page)
        {
            var sections = page.Controls["pageBody"] as FlowLayoutPanel
                ?? throw new InvalidOperationException("Không tìm thấy vùng nội dung trang tài khoản.");
            sections.Resize += (_, _) =>
            {
                int width = Math.Max(0, sections.ClientSize.Width - sections.Padding.Horizontal -
                    SystemInformation.VerticalScrollBarWidth - 4);
                foreach (Control control in sections.Controls)
                    control.Width = width;
            };
            return sections;
        }

        private static Panel TaoThe(int height) => new TheTaiKhoan
        {
            Height = height,
            Width = 900,
            Margin = new Padding(0, 0, 0, 16),
            Padding = new Padding(20)
        };

        private static Panel TaoTieuDeThe(string title, string subtitle)
        {
            var panel = new Panel
            {
                Location = new Point(22, 16),
                Size = new Size(760, 48),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.White
            };
            panel.Controls.Add(new Label
            {
                AutoSize = true,
                Text = title,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = MauChu,
                Location = Point.Empty
            });
            panel.Controls.Add(new Label
            {
                AutoSize = true,
                Text = subtitle,
                Font = new Font("Segoe UI", 9F),
                ForeColor = MauPhu,
                Location = new Point(0, 25)
            });
            panel.ParentChanged += (_, _) =>
            {
                if (panel.Parent is Panel parent)
                    parent.Resize += (_, _) => panel.Width = Math.Max(0, parent.ClientSize.Width - 44);
            };
            return panel;
        }

        private static Control TaoTruong(string label, string value, bool readOnly = false)
        {
            var field = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(0, 0, 12, 4) };
            field.Controls.Add(new Label
            {
                AutoSize = true,
                Text = label,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = MauChu,
                Location = new Point(0, 0)
            });
            field.Controls.Add(new TextBox
            {
                Text = value,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = readOnly ? MauPhu : MauChu,
                BackColor = readOnly ? Color.FromArgb(245, 246, 248) : Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = readOnly,
                Location = new Point(0, 23),
                Height = 30,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Width = Math.Max(80, field.ClientSize.Width - 12)
            });
            field.Resize += (_, _) =>
            {
                if (field.Controls.Count > 1)
                    field.Controls[1].Width = Math.Max(80, field.ClientSize.Width - 12);
            };
            return field;
        }

        private static Control TaoTruongNgaySinh()
        {
            var field = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(0, 0, 12, 4) };
            field.Controls.Add(new Label
            {
                AutoSize = true,
                Text = "Ngày sinh",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = MauChu,
                Location = new Point(0, 0)
            });
            field.Controls.Add(new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                Font = new Font("Segoe UI", 9.5F),
                Value = new DateTime(1998, 4, 13),
                Location = new Point(0, 23),
                Height = 30,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Width = Math.Max(80, field.ClientSize.Width - 12)
            });
            field.Resize += (_, _) =>
            {
                if (field.Controls.Count > 1)
                    field.Controls[1].Width = Math.Max(80, field.ClientSize.Width - 12);
            };
            return field;
        }

        private static Control TaoTruongMatKhau(string label, out TextBox textBox)
        {
            var field = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(0, 0, 12, 4) };
            field.Controls.Add(new Label
            {
                AutoSize = true,
                Text = label,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = MauChu,
                Location = new Point(0, 0)
            });
            textBox = new TextBox
            {
                UseSystemPasswordChar = true,
                Font = new Font("Segoe UI", 9.5F),
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(0, 22),
                Height = 28,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Width = Math.Max(80, field.ClientSize.Width - 12)
            };
            field.Controls.Add(textBox);
            field.Resize += (_, _) =>
            {
                if (field.Controls.Count > 1)
                    field.Controls[1].Width = Math.Max(80, field.ClientSize.Width - 12);
            };
            return field;
        }

        private static Button TaoNut(string text, bool primary = false, bool selected = false)
        {
            var button = new Button
            {
                Text = text,
                Size = new Size(150, 38),
                Margin = new Padding(0, 0, 10, 0),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = primary || selected ? Color.White : MauChu,
                BackColor = primary || selected ? MauDo : Color.White,
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = primary || selected ? 0 : 1;
            button.FlatAppearance.BorderColor = MauVien;
            return button;
        }

        private sealed class TheTaiKhoan : Panel
        {
            public TheTaiKhoan()
            {
                DoubleBuffered = true;
                BackColor = Color.White;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                if (ClientSize.Width < 2 || ClientSize.Height < 2)
                    return;

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Rectangle bounds = new(0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
                using var path = TaoDuongVienBoGoc(bounds, 12);
                using var pen = new Pen(MauVien);
                e.Graphics.DrawPath(pen, path);
            }

            protected override void OnSizeChanged(EventArgs e)
            {
                base.OnSizeChanged(e);
                if (ClientSize.Width < 2 || ClientSize.Height < 2)
                    return;

                Rectangle bounds = new(0, 0, ClientSize.Width, ClientSize.Height);
                using var path = TaoDuongVienBoGoc(bounds, 12);
                Region?.Dispose();
                Region = new Region(path);
            }

            private static System.Drawing.Drawing2D.GraphicsPath TaoDuongVienBoGoc(Rectangle bounds, int radius)
            {
                int diameter = radius * 2;
                var path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
                path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
                path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
                path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
                path.CloseFigure();
                return path;
            }
        }

    }
}
