#nullable enable
using System;
using System.Drawing;
using System.Windows.Forms;

namespace cinema_project.UI.Forms.frmAdmin
{
    partial class frmAdmin
    {
        private static readonly Color MauNen = Color.FromArgb(245, 246, 250);
        private static readonly Color MauChu = Color.FromArgb(35, 35, 42);
        private static readonly Color MauPhu = Color.FromArgb(105, 105, 116);
        private static readonly Color MauVien = Color.FromArgb(226, 227, 233);
        private static readonly Color MauDo = Color.FromArgb(231, 26, 15);
        private static readonly Color MauSidebar = Color.FromArgb(28, 28, 35);

        private Label _lblEmailAdmin = null!;
        private Label _lblTieuDeTrang = null!;
        private Button _btnLichChieu = null!;
        private Button _btnVe = null!;
        private Button _btnBaoCao = null!;
        private Button _btnSidebarLichChieu = null!;
        private Button _btnSidebarVe = null!;
        private Button _btnSidebarBaoCao = null!;

        private void InitializeComponent()
        {
            SuspendLayout();

            AutoScaleMode = AutoScaleMode.Font;
            BackColor = MauNen;
            ClientSize = new Size(1180, 760);
            MinimumSize = new Size(980, 650);
            Font = new Font("Segoe UI", 9.5F);
            FormBorderStyle = FormBorderStyle.Sizable;
            StartPosition = FormStartPosition.CenterScreen;
            Name = "frmAdmin";
            Text = "CGV Cinemas - Quản trị";

            var sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 238,
                BackColor = MauSidebar,
                Padding = new Padding(16, 20, 16, 16)
            };
            var logo = new Label
            {
                AutoSize = true,
                Text = "CGV",
                Font = new Font("Segoe UI", 23F, FontStyle.Bold),
                ForeColor = MauDo,
                Location = new Point(20, 20)
            };
            sidebar.Controls.Add(logo);

            var brandCaption = new Label
            {
                AutoSize = true,
                Text = "ADMIN CONSOLE",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(190, 190, 202),
                Location = new Point(23, 58)
            };
            sidebar.Controls.Add(brandCaption);

            var separator = new Panel
            {
                BackColor = Color.FromArgb(58, 58, 68),
                Location = new Point(20, 92),
                Size = new Size(194, 1),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            sidebar.Controls.Add(separator);

            _btnSidebarLichChieu = TaoNutSidebar("Lịch chiếu");
            _btnSidebarLichChieu.Location = new Point(12, 112);
            sidebar.Controls.Add(_btnSidebarLichChieu);

            _btnSidebarVe = TaoNutSidebar("Vé");
            _btnSidebarVe.Location = new Point(12, 158);
            sidebar.Controls.Add(_btnSidebarVe);

            _btnSidebarBaoCao = TaoNutSidebar("Thống kê & báo cáo", selected: true);
            _btnSidebarBaoCao.Location = new Point(12, 204);
            sidebar.Controls.Add(_btnSidebarBaoCao);

            var adminLabel = new Label
            {
                AutoSize = true,
                Text = "ĐANG ĐĂNG NHẬP",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(155, 155, 168),
                Location = new Point(22, 654),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            sidebar.Controls.Add(adminLabel);

            _lblEmailAdmin = new Label
            {
                AutoEllipsis = true,
                Text = "admin@example.com",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.White,
                Location = new Point(22, 676),
                Size = new Size(188, 22),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            sidebar.Controls.Add(_lblEmailAdmin);

            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 76,
                BackColor = Color.White,
                Padding = new Padding(28, 0, 28, 0)
            };
            header.Paint += (_, e) =>
            {
                using var pen = new Pen(MauVien);
                e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
            };

            _lblTieuDeTrang = new Label
            {
                AutoSize = true,
                Text = "Thống kê & báo cáo",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = MauChu,
                Location = new Point(28, 22)
            };
            header.Controls.Add(_lblTieuDeTrang);

            var dateLabel = new Label
            {
                AutoSize = true,
                Text = DateTime.Today.ToString("dddd, dd/MM/yyyy"),
                Font = new Font("Segoe UI", 9F),
                ForeColor = MauPhu,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            header.Controls.Add(dateLabel);
            header.Resize += (_, _) =>
                dateLabel.Location = new Point(header.ClientSize.Width - dateLabel.Width - 28, 29);

            var content = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = MauNen,
                AutoScroll = true,
                Padding = new Padding(28, 24, 28, 24)
            };

            var greeting = new Label
            {
                AutoSize = true,
                Text = "Quản lý rạp phim",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = MauChu,
                Location = new Point(0, 0)
            };
            content.Controls.Add(greeting);

            var description = new Label
            {
                AutoSize = true,
                Text = "Quản lý lịch chiếu, theo dõi vé và thống kê doanh thu.",
                Font = new Font("Segoe UI", 10F),
                ForeColor = MauPhu,
                Location = new Point(2, 40)
            };
            content.Controls.Add(description);

            Panel scheduleCard = TaoTheModule(
                "Lịch chiếu",
                "Xem, thêm, cập nhật và xóa suất chiếu; lọc theo ngày, phim và phòng chiếu.",
                "Mở quản lý lịch chiếu");
            scheduleCard.Location = new Point(0, 94);
            scheduleCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _btnLichChieu = (Button)scheduleCard.Controls["moduleAction"]!;
            content.Controls.Add(scheduleCard);

            Panel ticketCard = TaoTheModule(
                "Vé",
                "Xem danh sách ghế đã đặt gắn với tài khoản đang đăng nhập.",
                "Mở danh sách vé");
            ticketCard.Location = new Point(0, 264);
            ticketCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _btnVe = (Button)ticketCard.Controls["moduleAction"]!;
            content.Controls.Add(ticketCard);

            Panel reportCard = TaoTheModule(
                "Thống kê & báo cáo",
                "Theo dõi tổng doanh thu, số vé đã thanh toán và chi tiết giao dịch theo khoảng ngày.",
                "Mở báo cáo doanh thu");
            reportCard.Location = new Point(0, 434);
            reportCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _btnBaoCao = (Button)reportCard.Controls["moduleAction"]!;
            content.Controls.Add(reportCard);

            var note = new Label
            {
                AutoSize = false,
                Text = "Báo cáo doanh thu chỉ dành cho tài khoản quản trị viên. Danh sách vé tuân theo user_id của phiên đăng nhập.",
                Font = new Font("Segoe UI", 9F),
                ForeColor = MauPhu,
                Location = new Point(2, 604),
                Size = new Size(760, 38),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            content.Controls.Add(note);

            content.Resize += (_, _) =>
            {
                int width = Math.Max(0, content.ClientSize.Width - content.Padding.Horizontal -
                    SystemInformation.VerticalScrollBarWidth - 8);
                scheduleCard.Width = width;
                ticketCard.Width = width;
                reportCard.Width = width;
                note.Width = width;
            };

            Controls.Add(content);
            Controls.Add(header);
            Controls.Add(sidebar);

            ResumeLayout(false);
        }

        private static Button TaoNutSidebar(string text, bool selected = false)
        {
            var button = new Button
            {
                Text = text,
                Size = new Size(198, 40),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0),
                ForeColor = selected ? Color.White : Color.FromArgb(190, 190, 202),
                BackColor = selected ? Color.FromArgb(49, 49, 59) : MauSidebar,
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 49, 59);
            return button;
        }

        private static Panel TaoTheModule(string title, string description, string actionText)
        {
            var card = new Panel
            {
                Name = "moduleCard",
                Size = new Size(860, 150),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(20)
            };
            card.Resize += (_, _) =>
            {
                if (card.Controls["moduleAction"] is Button action)
                    action.Location = new Point(card.ClientSize.Width - action.Width - 20, card.ClientSize.Height - action.Height - 18);
                if (card.Controls["moduleDescription"] is Label text)
                    text.Width = Math.Max(0, card.ClientSize.Width - 44);
            };
            card.Controls.Add(new Label
            {
                AutoSize = true,
                Text = title,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = MauChu,
                Location = new Point(20, 19)
            });
            card.Controls.Add(new Label
            {
                Name = "moduleDescription",
                AutoSize = false,
                Text = description,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = MauPhu,
                Location = new Point(22, 52),
                Size = new Size(810, 28),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            });
            Button actionButton = TaoNutModule(actionText);
            actionButton.Name = "moduleAction";
            actionButton.Location = new Point(20, 94);
            actionButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            card.Controls.Add(actionButton);
            return card;
        }

        private static Button TaoNutModule(string text)
        {
            var button = new Button
            {
                Text = text,
                Size = new Size(196, 38),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = MauChu,
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderColor = MauVien;
            button.FlatAppearance.BorderSize = 1;
            return button;
        }

    }
}
