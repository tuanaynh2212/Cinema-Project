using System.Drawing;

namespace CinemaManagement;

public static class Ui
{
    public static readonly Color Navy = Color.FromArgb(22, 27, 48);
    public static readonly Color Accent = Color.FromArgb(105, 76, 235);
    public static readonly Color Background = Color.FromArgb(245, 247, 252);
    public static readonly Color Text = Color.FromArgb(38, 44, 62);
    public static readonly Color Muted = Color.FromArgb(120, 128, 148);
    public static readonly Color Border = Color.FromArgb(224, 228, 238);

    public static Label MakeLabel(string text, float size = 10, bool bold = false)
        => new() { Text = text, AutoSize = true, ForeColor = Text, Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular) };

    public static Button MakeButton(string text, Color backColor, Color? foreColor = null)
    {
        Button button = new()
        {
            Text = text, BackColor = backColor, ForeColor = foreColor ?? Color.White,
            FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI Semibold", 9.5F),
            Cursor = Cursors.Hand, Height = 38, Padding = new Padding(10, 0, 10, 0),
            Margin = new Padding(4), UseVisualStyleBackColor = false
        };
        button.FlatAppearance.BorderSize = 0;
        return button;
    }

    public static void StyleTextBox(TextBox textBox)
    {
        textBox.BorderStyle = BorderStyle.FixedSingle;
        textBox.BackColor = Color.White;
        textBox.ForeColor = Text;
    }

    public static DataGridView MakeGrid()
    {
        DataGridView grid = new()
        {
            Dock = DockStyle.Fill, BackgroundColor = Color.White, BorderStyle = BorderStyle.None,
            AllowUserToAddRows = false, AllowUserToDeleteRows = false, ReadOnly = true,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
            RowHeadersVisible = false, EnableHeadersVisualStyles = false,
            GridColor = Border, Font = new Font("Segoe UI", 9.5F),
            ColumnHeadersHeight = 42, RowTemplate = { Height = 36 }
        };
        grid.ColumnHeadersDefaultCellStyle.BackColor = Navy;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5F);
        grid.DefaultCellStyle.BackColor = Color.White;
        grid.DefaultCellStyle.ForeColor = Text;
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(229, 224, 255);
        grid.DefaultCellStyle.SelectionForeColor = Text;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 253);
        return grid;
    }

    public static Panel StatCard(string title, string value, string foot, Color accent)
    {
        Panel card = new() { BackColor = Color.White, Margin = new Padding(6), Padding = new Padding(16), Height = 128, Width = 220 };
        Panel strip = new() { BackColor = accent, Width = 5, Dock = DockStyle.Left };
        Label label = MakeLabel(title.ToUpperInvariant(), 8.5F, true); label.ForeColor = Muted; label.Location = new Point(20, 15);
        Label number = MakeLabel(value, 23, true); number.Location = new Point(20, 42);
        Label description = MakeLabel(foot, 8.5F); description.ForeColor = Muted; description.Location = new Point(20, 83);
        card.Controls.AddRange(new Control[] { label, number, description, strip });
        return card;
    }

    public static Panel WhitePanel()
        => new() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(14), Margin = new Padding(0) };

    public static string FormatMoney(object? value)
    {
        if (value == null || value == DBNull.Value) return "0 ₫";
        return string.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:N0} ₫", Convert.ToDecimal(value));
    }
}
