using System.Drawing;

namespace CinemaManagement;

public sealed class SeatPickerForm : Form
{
    private readonly int _showtimeId;
    private readonly HashSet<string> _bookedSeats;
    private readonly decimal _ticketPrice;
    private readonly HashSet<string> _selected = new(StringComparer.OrdinalIgnoreCase);
    private readonly Label _summary = new();
    private readonly FlowLayoutPanel _seatPanel = new();
    public IReadOnlyList<string> SelectedSeats => _selected.OrderBy(s => s).ToList();

    public SeatPickerForm(int showtimeId)
    {
        _showtimeId = showtimeId;
        _bookedSeats = DatabaseHelper.GetBookedSeats(showtimeId);
        _ticketPrice = DatabaseHelper.GetTicketPrice(showtimeId);
        Text = "Chọn ghế ngồi";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(790, 610);
        MinimumSize = new Size(760, 590);
        BackColor = Ui.Background;
        BuildLayout();
    }

    private void BuildLayout()
    {
        Panel top = new() { Dock = DockStyle.Top, Height = 110, BackColor = Color.White, Padding = new Padding(20) };
        Label title = Ui.MakeLabel("Chọn ghế của bạn", 20, true); title.Location = new Point(20, 12);
        Label instruction = Ui.MakeLabel("Nhấp vào ghế trống để chọn. Ghế màu xám đã được đặt.", 9.5F); instruction.ForeColor = Ui.Muted; instruction.Location = new Point(22, 48);
        Panel screen = new() { Height = 28, Dock = DockStyle.Bottom, BackColor = Ui.Navy };
        Label screenLabel = new() { Text = "MÀN HÌNH CHIẾU", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 8.5F) };
        screen.Controls.Add(screenLabel); top.Controls.Add(title); top.Controls.Add(instruction); top.Controls.Add(screen);

        Panel center = new() { Dock = DockStyle.Fill, BackColor = Ui.Background, Padding = new Padding(26, 15, 26, 12) };
        _seatPanel.Dock = DockStyle.Fill; _seatPanel.FlowDirection = FlowDirection.TopDown; _seatPanel.WrapContents = false; _seatPanel.AutoScroll = true; _seatPanel.Padding = new Padding(8); _seatPanel.BackColor = Color.White;
        for (char row = 'A'; row <= 'F'; row++)
        {
            FlowLayoutPanel rowPanel = new() { Width = 660, Height = 48, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, Margin = new Padding(0, 2, 0, 2), BackColor = Color.White };
            Label rowLabel = new() { Text = row.ToString(), Width = 24, Height = 36, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Ui.Muted, Margin = new Padding(0, 1, 4, 1) };
            rowPanel.Controls.Add(rowLabel);
            for (int i = 1; i <= 10; i++)
            {
                string code = row.ToString() + i;
                bool booked = _bookedSeats.Contains(code);
                Button seat = new() { Text = code, Width = 48, Height = 36, Margin = new Padding(3), Font = new Font("Segoe UI Semibold", 8.5F), FlatStyle = FlatStyle.Flat, Cursor = booked ? Cursors.No : Cursors.Hand, Tag = code, UseVisualStyleBackColor = false };
                seat.FlatAppearance.BorderSize = 0;
                if (booked) { seat.BackColor = Color.FromArgb(220, 224, 233); seat.ForeColor = Color.FromArgb(140, 146, 160); seat.Enabled = false; }
                else { seat.BackColor = Color.FromArgb(223, 245, 237); seat.ForeColor = Color.FromArgb(23, 112, 84); }
                seat.Click += (s, e) => ToggleSeat((Button)s!);
                rowPanel.Controls.Add(seat);
            }
            _seatPanel.Controls.Add(rowPanel);
        }
        center.Controls.Add(_seatPanel);

        Panel footer = new() { Dock = DockStyle.Bottom, Height = 78, BackColor = Color.White, Padding = new Padding(20) };
        _summary.Text = "Chưa chọn ghế"; _summary.AutoSize = true; _summary.Font = new Font("Segoe UI Semibold", 10); _summary.ForeColor = Ui.Text; _summary.Location = new Point(20, 18);
        Button cancel = Ui.MakeButton("Quay lại", Color.FromArgb(226, 229, 239), Ui.Text); cancel.Size = new Size(105, 40); cancel.Anchor = AnchorStyles.Right | AnchorStyles.Top; cancel.Location = new Point(500, 15); cancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        Button confirm = Ui.MakeButton("XÁC NHẬN ĐẶT VÉ", Ui.Accent); confirm.Size = new Size(170, 40); confirm.Anchor = AnchorStyles.Right | AnchorStyles.Top; confirm.Location = new Point(610, 15); confirm.Click += (s, e) =>
        {
            if (_selected.Count == 0) { MessageBox.Show("Vui lòng chọn ít nhất một ghế."); return; }
            DialogResult = DialogResult.OK; Close();
        };
        footer.Controls.AddRange(new Control[] { _summary, cancel, confirm });
        Controls.Add(center); Controls.Add(footer); Controls.Add(top);
    }

    private void ToggleSeat(Button button)
    {
        string code = Convert.ToString(button.Tag) ?? "";
        if (_selected.Remove(code))
        {
            button.BackColor = Color.FromArgb(223, 245, 237); button.ForeColor = Color.FromArgb(23, 112, 84);
        }
        else
        {
            _selected.Add(code); button.BackColor = Ui.Accent; button.ForeColor = Color.White;
        }
        _summary.Text = _selected.Count == 0 ? "Chưa chọn ghế" : $"Ghế: {string.Join(", ", _selected.OrderBy(s => s))}  |  Tổng: {Ui.FormatMoney(_ticketPrice * _selected.Count)}";
    }
}
