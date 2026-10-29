using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Globalization;

namespace CinemaManagement;

public class RoleDashboardForm : Form
{
    private readonly UserSession _user;
    private readonly string _role;
    private readonly Panel _content = new();
    private readonly Label _pageTitle = new();
    private readonly Label _welcome = new();
    private DataGridView? _activeGrid;
    private string _currentPage = "overview";

    protected RoleDashboardForm(UserSession user, string role)
    {
        _user = user;
        _role = role;
        Text = "CinemaManagement | " + RoleLabel(role);
        WindowState = FormWindowState.Maximized;
        MinimumSize = new Size(1050, 680);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Ui.Background;
        Font = new Font("Segoe UI", 9.5F);
        BuildLayout();
        ShowPage("overview");
    }

    private void BuildLayout()
    {
        Panel sidebar = new() { Dock = DockStyle.Left, Width = 245, BackColor = Ui.Navy, Padding = new Padding(12) };
        Label brand = new() { Text = "CINEMA\nMANAGEMENT", AutoSize = true, ForeColor = Color.White, Font = new Font("Segoe UI", 17, FontStyle.Bold), Location = new Point(20, 20) };
        Label roleLabel = new() { Text = RoleLabel(_role).ToUpperInvariant(), AutoSize = true, ForeColor = Color.FromArgb(174, 182, 208), Font = new Font("Segoe UI Semibold", 8.5F), Location = new Point(23, 79) };
        sidebar.Controls.Add(brand); sidebar.Controls.Add(roleLabel);

        FlowLayoutPanel nav = new() { Location = new Point(10, 125), Width = 220, Height = 640, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, BackColor = Ui.Navy, Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right };
        foreach (var item in MenuItems())
        {
            Button button = new() { Text = "  " + item.Icon + "   " + item.Title, Tag = item.Key, Width = 210, Height = 44,
                TextAlign = ContentAlignment.MiddleLeft, FlatStyle = FlatStyle.Flat, BackColor = Ui.Navy, ForeColor = Color.FromArgb(225, 228, 240),
                Font = new Font("Segoe UI", 9.5F), Cursor = Cursors.Hand, Margin = new Padding(0, 2, 0, 2), Padding = new Padding(8, 0, 0, 0) };
            button.FlatAppearance.BorderSize = 0;
            button.Click += (s, e) => ShowPage((string)((Button)s!).Tag!);
            nav.Controls.Add(button);
        }
        Button logout = Ui.MakeButton("Đăng xuất", Color.FromArgb(51, 57, 82));
        logout.Dock = DockStyle.Bottom; logout.Height = 42; logout.Click += (s, e) => Close();
        sidebar.Controls.Add(nav); sidebar.Controls.Add(logout);

        Panel main = new() { Dock = DockStyle.Fill, BackColor = Ui.Background, Padding = new Padding(24, 20, 24, 22) };
        Panel header = new() { Dock = DockStyle.Top, Height = 72, BackColor = Ui.Background };
        _pageTitle.Text = "Tổng quan"; _pageTitle.Font = new Font("Segoe UI", 22, FontStyle.Bold); _pageTitle.ForeColor = Ui.Text; _pageTitle.AutoSize = true; _pageTitle.Location = new Point(0, 0);
        _welcome.Text = "Xin chào, " + _user.FullName; _welcome.Font = new Font("Segoe UI", 10, FontStyle.Regular); _welcome.ForeColor = Ui.Muted; _welcome.AutoSize = true; _welcome.Anchor = AnchorStyles.Top | AnchorStyles.Right; _welcome.Location = new Point(650, 13);
        header.Controls.Add(_pageTitle); header.Controls.Add(_welcome);
        _content.Dock = DockStyle.Fill; _content.BackColor = Ui.Background;
        main.Controls.Add(_content); main.Controls.Add(header);
        Controls.Add(main); Controls.Add(sidebar);
    }

    private List<(string Key, string Title, string Icon)> MenuItems()
    {
        if (_role == "Admin") return new()
        {
            ("overview", "Tổng quan", "◈"), ("movies", "Quản lý phim", "▣"), ("rooms", "Phòng chiếu", "▤"),
            ("showtimes", "Lịch chiếu", "◷"), ("users", "Tài khoản", "♙"), ("bookings", "Đặt vé & soát vé", "▣"), ("revenue", "Doanh thu", "↗")
        };
        if (_role == "Manager") return new()
        {
            ("overview", "Tổng quan", "◈"), ("moviesView", "Danh sách phim", "▣"), ("showtimes", "Lịch chiếu", "◷"),
            ("bookings", "Đặt vé & soát vé", "▣"), ("revenue", "Doanh thu", "↗")
        };
        return new()
        {
            ("overview", "Trang chủ", "◈"), ("shows", "Phim & suất chiếu", "▣"), ("mybookings", "Vé của tôi", "▤"), ("profile", "Tài khoản", "♙")
        };
    }

    private static string RoleLabel(string role) => role switch { "Admin" => "Quản trị viên", "Manager" => "Quản lý rạp", _ => "Khách hàng" };

    private void ShowPage(string key)
    {
        _currentPage = key;
        _activeGrid = null;
        _content.Controls.Clear();
        _pageTitle.Text = key switch
        {
            "overview" => "Tổng quan", "movies" => "Quản lý phim", "moviesView" => "Danh sách phim",
            "rooms" => "Quản lý phòng chiếu", "showtimes" => "Quản lý lịch chiếu", "users" => "Quản lý tài khoản",
            "bookings" => "Đặt vé & soát vé", "revenue" => "Báo cáo doanh thu", "shows" => "Phim & suất chiếu",
            "mybookings" => "Vé của tôi", "profile" => "Thông tin tài khoản", _ => "Tổng quan"
        };
        try
        {
            switch (key)
            {
                case "overview": ShowOverview(); break;
                case "movies": ShowEntityPage(GetMovieDefinition(), true); break;
                case "moviesView": ShowReadOnlyGrid("SELECT MovieID, Title AS [Tên phim], Genre AS [Thể loại], DurationMinutes AS [Thời lượng (phút)], AgeRating AS [Phân loại], ReleaseDate AS [Ngày khởi chiếu] FROM dbo.Movies WHERE IsActive=1 ORDER BY Title", "Tìm phim..."); break;
                case "rooms": ShowEntityPage(GetRoomDefinition(), true); break;
                case "showtimes": ShowEntityPage(GetShowtimeDefinition(), true); break;
                case "users": ShowEntityPage(GetUserDefinition(), true); break;
                case "bookings": ShowBookings(); break;
                case "revenue": ShowRevenue(); break;
                case "shows": ShowCustomerShows(); break;
                case "mybookings": ShowMyBookings(); break;
                case "profile": ShowProfile(); break;
                default: ShowOverview(); break;
            }
        }
        catch (Exception ex)
        {
            ShowError("Không thể mở chức năng này", ex);
        }
    }

    private void ShowOverview()
    {
        FlowLayoutPanel cards = new() { Dock = DockStyle.Top, Height = 150, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, AutoScroll = true, BackColor = Ui.Background, Padding = new Padding(0, 2, 0, 6) };
        if (_role == "Customer")
        {
            cards.Controls.Add(Ui.StatCard("Phim đang chiếu", SafeCount("SELECT COUNT(*) FROM dbo.Movies WHERE IsActive=1"), "Khám phá phim", Ui.Accent));
            cards.Controls.Add(Ui.StatCard("Suất sắp tới", SafeCount("SELECT COUNT(*) FROM dbo.Showtimes WHERE StartTime>SYSDATETIME()"), "Chọn suất phù hợp", Color.FromArgb(28, 166, 133)));
            cards.Controls.Add(Ui.StatCard("Vé đã đặt", SafeCount("SELECT COUNT(*) FROM dbo.Bookings WHERE UserID=" + _user.UserId), "Tất cả đơn vé", Color.FromArgb(230, 145, 40)));
        }
        else
        {
            cards.Controls.Add(Ui.StatCard("Phim hoạt động", SafeCount("SELECT COUNT(*) FROM dbo.Movies WHERE IsActive=1"), "Danh mục phim", Ui.Accent));
            cards.Controls.Add(Ui.StatCard("Suất chiếu sắp tới", SafeCount("SELECT COUNT(*) FROM dbo.Showtimes WHERE StartTime>SYSDATETIME()"), "Lịch chiếu tương lai", Color.FromArgb(28, 166, 133)));
            cards.Controls.Add(Ui.StatCard("Vé đã thanh toán", SafeCount("SELECT COUNT(*) FROM dbo.Bookings WHERE Status='Paid'"), "Đơn vé hợp lệ", Color.FromArgb(230, 145, 40)));
            if (_role == "Admin" || _role == "Manager")
                cards.Controls.Add(Ui.StatCard("Doanh thu", Ui.FormatMoney(SafeScalar("SELECT ISNULL(SUM(TotalAmount),0) FROM dbo.Bookings WHERE Status='Paid'")), "Tổng đơn đã thanh toán", Color.FromArgb(41, 123, 220)));
        }
        Label section = Ui.MakeLabel(_role == "Customer" ? "SUẤT CHIẾU SẮP TỚI" : "LỊCH CHIẾU SẮP TỚI", 11, true);
        section.Location = new Point(2, 166);
        DataGridView grid = Ui.MakeGrid(); grid.Dock = DockStyle.Fill;
        Panel tablePanel = new() { Dock = DockStyle.Fill, Padding = new Padding(0, 8, 0, 0), BackColor = Ui.Background };
        tablePanel.Controls.Add(grid);
        _content.Controls.Add(tablePanel); _content.Controls.Add(section); _content.Controls.Add(cards);
        grid.DataSource = DatabaseHelper.ExecuteQuery(@"SELECT TOP 12 s.ShowtimeID, m.Title AS [Phim], r.RoomName AS [Phòng],
            s.StartTime AS [Bắt đầu], s.EndTime AS [Kết thúc], s.TicketPrice AS [Giá vé]
            FROM dbo.Showtimes s INNER JOIN dbo.Movies m ON m.MovieID=s.MovieID INNER JOIN dbo.Rooms r ON r.RoomID=s.RoomID
            WHERE s.StartTime>SYSDATETIME() AND m.IsActive=1 AND r.IsActive=1 ORDER BY s.StartTime");
        HideColumns(grid, "ShowtimeID");
        FormatMoneyColumn(grid, "Giá vé");
    }

    private string SafeCount(string sql) => Convert.ToString(SafeScalar(sql), CultureInfo.InvariantCulture) ?? "0";
    private object SafeScalar(string sql)
    {
        try { return DatabaseHelper.ExecuteScalar(sql) ?? 0; }
        catch { return 0; }
    }

    private void ShowEntityPage(EntityDefinition definition, bool editable)
    {
        Panel toolbar = new() { Dock = DockStyle.Top, Height = 58, BackColor = Ui.Background };
        TextBox search = new() { PlaceholderText = "Tìm kiếm dữ liệu...", Width = 280, Height = 34, Location = new Point(0, 4), Font = new Font("Segoe UI", 10) };
        Ui.StyleTextBox(search);
        Button refresh = Ui.MakeButton("Làm mới", Color.FromArgb(224, 228, 239), Ui.Text); refresh.Location = new Point(290, 0); refresh.Width = 105;
        Button add = Ui.MakeButton("+ Thêm mới", Ui.Accent); add.Location = new Point(410, 0); add.Width = 120;
        Button edit = Ui.MakeButton("Sửa", Color.FromArgb(231, 155, 40)); edit.Location = new Point(535, 0); edit.Width = 90;
        Button delete = Ui.MakeButton("Xóa / khóa", Color.FromArgb(215, 70, 82)); delete.Location = new Point(630, 0); delete.Width = 110;
        DataGridView grid = Ui.MakeGrid(); _activeGrid = grid;
        _content.Controls.Add(grid); _content.Controls.Add(toolbar);
        toolbar.Controls.Add(search); toolbar.Controls.Add(refresh);
        if (editable) toolbar.Controls.AddRange(new Control[] { add, edit, delete });

        void LoadRows()
        {
            try
            {
                DataTable table = DatabaseHelper.ExecuteQuery(definition.SelectSql);
                if (!string.IsNullOrWhiteSpace(search.Text))
                {
                    string? stringColumn = table.Columns.Cast<DataColumn>().FirstOrDefault(c => c.DataType == typeof(string))?.ColumnName;
                    if (stringColumn != null)
                    {
                        string safeColumn = stringColumn.Replace("]", "]]" );
                        string safeText = search.Text.Replace("'", "''");
                        table.DefaultView.RowFilter = $"[{safeColumn}] LIKE '%{safeText}%'";
                        grid.DataSource = table.DefaultView;
                    }
                    else grid.DataSource = table;
                }
                else grid.DataSource = table;
                HideColumns(grid, definition.IdColumn);
                FormatMoneyColumn(grid, "Giá vé");
            }
            catch (Exception ex) { ShowError("Không tải được dữ liệu", ex); }
        }
        search.TextChanged += (s, e) => LoadRows();
        refresh.Click += (s, e) => LoadRows();
        if (editable)
        {
            add.Click += (s, e) => { using EntityEditorForm form = new(definition); if (form.ShowDialog(this) == DialogResult.OK) LoadRows(); };
            edit.Click += (s, e) =>
            {
                int? id = SelectedId(grid, definition.IdColumn);
                if (id == null) { MessageBox.Show("Vui lòng chọn một dòng trước."); return; }
                using EntityEditorForm form = new(definition, id.Value);
                if (form.ShowDialog(this) == DialogResult.OK) LoadRows();
            };
            delete.Click += (s, e) =>
            {
                int? id = SelectedId(grid, definition.IdColumn);
                if (id == null) { MessageBox.Show("Vui lòng chọn một dòng trước."); return; }
                if (MessageBox.Show("Bạn chắc chắn muốn thực hiện thao tác này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                try
                {
                    int affected = definition.Table == "Users"
                        ? DatabaseHelper.ExecuteNonQuery("UPDATE dbo.Users SET IsActive=0 WHERE UserID=@ID", new SqlParameter("@ID", SqlDbType.Int) { Value = id.Value })
                        : DatabaseHelper.ExecuteNonQuery($"DELETE FROM dbo.[{definition.Table}] WHERE [{definition.IdColumn}]=@ID", new SqlParameter("@ID", SqlDbType.Int) { Value = id.Value });
                    if (affected == 0) MessageBox.Show("Không có dữ liệu nào thay đổi.");
                    LoadRows();
                }
                catch (Exception ex) { ShowError("Không thể xóa. Dữ liệu có thể đang được lịch chiếu hoặc đơn vé sử dụng.", ex); }
            };
        }
        LoadRows();
    }

    private void ShowReadOnlyGrid(string query, string placeholder)
    {
        Panel toolbar = new() { Dock = DockStyle.Top, Height = 52 };
        TextBox search = new() { PlaceholderText = placeholder, Width = 300, Height = 34, Location = new Point(0, 4), Font = new Font("Segoe UI", 10) };
        Ui.StyleTextBox(search);
        Button refresh = Ui.MakeButton("Làm mới", Color.FromArgb(224, 228, 239), Ui.Text); refresh.Location = new Point(310, 0);
        DataGridView grid = Ui.MakeGrid(); _activeGrid = grid;
        _content.Controls.Add(grid); _content.Controls.Add(toolbar); toolbar.Controls.Add(search); toolbar.Controls.Add(refresh);
        void LoadRows()
        {
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            if (!string.IsNullOrWhiteSpace(search.Text))
            {
                string? col = dt.Columns.Cast<DataColumn>().FirstOrDefault(c => c.DataType == typeof(string))?.ColumnName;
                if (col != null) { string safeColumn = col.Replace("]", "]]" ); string safeText = search.Text.Replace("'", "''"); dt.DefaultView.RowFilter = $"[{safeColumn}] LIKE '%{safeText}%'"; grid.DataSource = dt.DefaultView; }
                else grid.DataSource = dt;
            }
            else grid.DataSource = dt;
            FormatMoneyColumn(grid, "Giá vé");
        }
        search.TextChanged += (s, e) => { try { LoadRows(); } catch { } };
        refresh.Click += (s, e) => LoadRows();
        LoadRows();
    }

    private void ShowBookings()
    {
        Panel toolbar = new() { Dock = DockStyle.Top, Height = 58, BackColor = Ui.Background };
        TextBox search = new() { PlaceholderText = "Tìm mã vé hoặc khách hàng...", Width = 260, Height = 34, Location = new Point(0, 4), Font = new Font("Segoe UI", 10) }; Ui.StyleTextBox(search);
        TextBox ticketCode = new() { PlaceholderText = "Nhập mã vé để soát", Width = 210, Height = 34, Location = new Point(270, 4), Font = new Font("Segoe UI", 10) }; Ui.StyleTextBox(ticketCode);
        Button checkIn = Ui.MakeButton("Xác nhận soát vé", Ui.Accent); checkIn.Location = new Point(490, 0); checkIn.Width = 150;
        Button cancel = Ui.MakeButton("Hủy vé", Color.FromArgb(215, 70, 82)); cancel.Location = new Point(650, 0); cancel.Width = 90;
        Button refresh = Ui.MakeButton("Làm mới", Color.FromArgb(224, 228, 239), Ui.Text); refresh.Location = new Point(750, 0); refresh.Width = 100;
        DataGridView grid = Ui.MakeGrid(); _activeGrid = grid;
        _content.Controls.Add(grid); _content.Controls.Add(toolbar); toolbar.Controls.AddRange(new Control[] { search, ticketCode, checkIn, cancel, refresh });
        string query = @"SELECT b.BookingID, b.TicketCode AS [Mã vé], u.Username AS [Tài khoản], u.FullName AS [Khách hàng],
            m.Title AS [Phim], r.RoomName AS [Phòng], s.StartTime AS [Giờ chiếu], b.TotalAmount AS [Tổng tiền],
            b.Status AS [Trạng thái], b.IsCheckedIn AS [Đã soát vé]
            FROM dbo.Bookings b INNER JOIN dbo.Users u ON u.UserID=b.UserID
            INNER JOIN dbo.Showtimes s ON s.ShowtimeID=b.ShowtimeID
            INNER JOIN dbo.Movies m ON m.MovieID=s.MovieID INNER JOIN dbo.Rooms r ON r.RoomID=s.RoomID
            ORDER BY b.BookingTime DESC";
        void LoadRows()
        {
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            if (!string.IsNullOrWhiteSpace(search.Text))
            {
                string q = search.Text.Replace("'", "''");
                dt.DefaultView.RowFilter = $"[Mã vé] LIKE '%{q}%' OR [Tài khoản] LIKE '%{q}%' OR [Khách hàng] LIKE '%{q}%' OR [Phim] LIKE '%{q}%'";
                grid.DataSource = dt.DefaultView;
            }
            else grid.DataSource = dt;
            HideColumns(grid, "BookingID"); FormatMoneyColumn(grid, "Tổng tiền");
        }
        void DoCheckIn(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) { MessageBox.Show("Hãy nhập mã vé hoặc chọn một đơn trong bảng."); return; }
            int changed = DatabaseHelper.ExecuteNonQuery(@"UPDATE dbo.Bookings SET IsCheckedIn=1, CheckInTime=SYSDATETIME()
                WHERE TicketCode=@Code AND Status='Paid' AND IsCheckedIn=0 AND EXISTS
                (SELECT 1 FROM dbo.Showtimes s WHERE s.ShowtimeID=Bookings.ShowtimeID)",
                new SqlParameter("@Code", SqlDbType.VarChar, 30) { Value = code.Trim() });
            if (changed == 1) MessageBox.Show("Soát vé thành công. Khách có thể vào phòng chiếu.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else MessageBox.Show("Không tìm thấy vé hợp lệ, vé chưa thanh toán hoặc vé đã được soát.", "Không thể soát vé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            ticketCode.Clear(); LoadRows();
        }
        checkIn.Click += (s, e) =>
        {
            string code = ticketCode.Text.Trim();
            if (code.Length == 0 && grid.SelectedRows.Count > 0) code = Convert.ToString(grid.SelectedRows[0].Cells["Mã vé"].Value) ?? "";
            try { DoCheckIn(code); } catch (Exception ex) { ShowError("Soát vé thất bại", ex); }
        };
        ticketCode.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { try { DoCheckIn(ticketCode.Text); } catch (Exception ex) { ShowError("Soát vé thất bại", ex); } } };
        cancel.Click += (s, e) =>
        {
            int? id = SelectedId(grid, "BookingID");
            if (id == null) { MessageBox.Show("Chọn một đơn đặt vé trước."); return; }
            if (MessageBox.Show("Xác nhận hủy đơn đặt vé này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                int changed = DatabaseHelper.ExecuteNonQuery("UPDATE dbo.Bookings SET Status='Cancelled' WHERE BookingID=@ID AND IsCheckedIn=0 AND Status<>'Cancelled'",
                    new SqlParameter("@ID", SqlDbType.Int) { Value = id.Value });
                MessageBox.Show(changed > 0 ? "Đã hủy đơn vé." : "Không thể hủy vé đã soát hoặc đã hủy."); LoadRows();
            }
            catch (Exception ex) { ShowError("Không thể hủy vé", ex); }
        };
        refresh.Click += (s, e) => LoadRows(); search.TextChanged += (s, e) => { try { LoadRows(); } catch { } };
        LoadRows();
    }

    private void ShowCustomerShows()
    {
        Panel toolbar = new() { Dock = DockStyle.Top, Height = 58, BackColor = Ui.Background };
        TextBox search = new() { PlaceholderText = "Tìm tên phim...", Width = 290, Height = 34, Location = new Point(0, 4), Font = new Font("Segoe UI", 10) }; Ui.StyleTextBox(search);
        Button refresh = Ui.MakeButton("Làm mới", Color.FromArgb(224, 228, 239), Ui.Text); refresh.Location = new Point(300, 0);
        Button book = Ui.MakeButton("Chọn ghế & đặt vé", Ui.Accent); book.Location = new Point(410, 0); book.Width = 160;
        DataGridView grid = Ui.MakeGrid(); _activeGrid = grid;
        _content.Controls.Add(grid); _content.Controls.Add(toolbar); toolbar.Controls.AddRange(new Control[] { search, refresh, book });
        const string query = @"SELECT s.ShowtimeID, m.Title AS [Phim], m.Genre AS [Thể loại], m.DurationMinutes AS [Phút],
            r.RoomName AS [Phòng], s.StartTime AS [Giờ chiếu], s.TicketPrice AS [Giá vé]
            FROM dbo.Showtimes s INNER JOIN dbo.Movies m ON m.MovieID=s.MovieID INNER JOIN dbo.Rooms r ON r.RoomID=s.RoomID
            WHERE s.StartTime>SYSDATETIME() AND m.IsActive=1 AND r.IsActive=1 ORDER BY s.StartTime";
        void LoadRows()
        {
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            if (!string.IsNullOrWhiteSpace(search.Text)) { string q = search.Text.Replace("'", "''"); dt.DefaultView.RowFilter = $"[Phim] LIKE '%{q}%' OR [Thể loại] LIKE '%{q}%'"; grid.DataSource = dt.DefaultView; }
            else grid.DataSource = dt;
            HideColumns(grid, "ShowtimeID"); FormatMoneyColumn(grid, "Giá vé");
        }
        book.Click += (s, e) =>
        {
            int? showtimeId = SelectedId(grid, "ShowtimeID");
            if (showtimeId == null) { MessageBox.Show("Vui lòng chọn một suất chiếu trước."); return; }
            try
            {
                using SeatPickerForm seatPicker = new(showtimeId.Value);
                if (seatPicker.ShowDialog(this) != DialogResult.OK) return;
                string ticketCode = DatabaseHelper.CreateBooking(_user.UserId, showtimeId.Value, seatPicker.SelectedSeats);
                MessageBox.Show($"Đặt vé thành công!\nMã vé: {ticketCode}\nGhế: {string.Join(", ", seatPicker.SelectedSeats)}\n\nLưu ý: đây là thanh toán mô phỏng trong đồ án, chưa kết nối cổng thanh toán thật.", "Đặt vé thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRows();
            }
            catch (Exception ex) { ShowError("Không thể đặt vé", ex); }
        };
        search.TextChanged += (s, e) => { try { LoadRows(); } catch { } }; refresh.Click += (s, e) => LoadRows();
        LoadRows();
    }

    private void ShowMyBookings()
    {
        const string query = @"SELECT b.BookingID, b.TicketCode AS [Mã vé], m.Title AS [Phim], r.RoomName AS [Phòng],
            s.StartTime AS [Giờ chiếu], STUFF((SELECT ', ' + bs.SeatCode FROM dbo.BookingSeats bs WHERE bs.BookingID=b.BookingID FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'),1,2,'') AS [Ghế],
            b.TotalAmount AS [Tổng tiền], b.Status AS [Trạng thái], b.IsCheckedIn AS [Đã soát]
            FROM dbo.Bookings b INNER JOIN dbo.Showtimes s ON s.ShowtimeID=b.ShowtimeID
            INNER JOIN dbo.Movies m ON m.MovieID=s.MovieID INNER JOIN dbo.Rooms r ON r.RoomID=s.RoomID
            WHERE b.UserID=@UserID ORDER BY b.BookingTime DESC";
        Panel toolbar = new() { Dock = DockStyle.Top, Height = 50 };
        Button refresh = Ui.MakeButton("Làm mới", Color.FromArgb(224, 228, 239), Ui.Text); refresh.Location = new Point(0, 0);
        DataGridView grid = Ui.MakeGrid(); _activeGrid = grid;
        _content.Controls.Add(grid); _content.Controls.Add(toolbar); toolbar.Controls.Add(refresh);
        void LoadRows()
        {
            grid.DataSource = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@UserID", SqlDbType.Int) { Value = _user.UserId });
            HideColumns(grid, "BookingID"); FormatMoneyColumn(grid, "Tổng tiền");
        }
        refresh.Click += (s, e) => LoadRows(); LoadRows();
    }

    private void ShowRevenue()
    {
        TableLayoutPanel layout = new() { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, BackColor = Ui.Background };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 112)); layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        Panel summary = new() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(18) };
        Label totalLabel = Ui.MakeLabel("TỔNG DOANH THU ĐÃ THANH TOÁN", 9, true); totalLabel.ForeColor = Ui.Muted; totalLabel.Location = new Point(18, 15);
        Label total = Ui.MakeLabel(Ui.FormatMoney(SafeScalar("SELECT ISNULL(SUM(TotalAmount),0) FROM dbo.Bookings WHERE Status='Paid'")), 25, true); total.Location = new Point(18, 40);
        summary.Controls.Add(totalLabel); summary.Controls.Add(total);
        DataGridView grid = Ui.MakeGrid();
        grid.DataSource = DatabaseHelper.ExecuteQuery(@"SELECT CAST(BookingTime AS date) AS [Ngày], COUNT(*) AS [Số đơn], SUM(TotalAmount) AS [Doanh thu]
            FROM dbo.Bookings WHERE Status='Paid' GROUP BY CAST(BookingTime AS date) ORDER BY [Ngày] DESC");
        FormatMoneyColumn(grid, "Doanh thu");
        layout.Controls.Add(grid, 0, 1); layout.Controls.Add(summary, 0, 0); _content.Controls.Add(layout);
    }

    private void ShowProfile()
    {
        Panel card = new() { BackColor = Color.White, Dock = DockStyle.Top, Height = 260, Padding = new Padding(26) };
        Label title = Ui.MakeLabel("Thông tin đăng nhập", 17, true); title.Location = new Point(25, 20);
        Label nameLabel = Ui.MakeLabel("Họ và tên", 9, true); nameLabel.ForeColor = Ui.Muted; nameLabel.Location = new Point(28, 72);
        Label name = Ui.MakeLabel(_user.FullName, 12, true); name.Location = new Point(28, 94);
        Label accountLabel = Ui.MakeLabel("Tên đăng nhập", 9, true); accountLabel.ForeColor = Ui.Muted; accountLabel.Location = new Point(28, 140);
        Label account = Ui.MakeLabel(_user.Username, 12, true); account.Location = new Point(28, 162);
        Label roleLabel = Ui.MakeLabel("Vai trò", 9, true); roleLabel.ForeColor = Ui.Muted; roleLabel.Location = new Point(330, 72);
        Label role = Ui.MakeLabel(RoleLabel(_user.Role), 12, true); role.Location = new Point(330, 94);
        Button changePassword = Ui.MakeButton("ĐỔI MẬT KHẨU", Ui.Accent); changePassword.Location = new Point(330, 190); changePassword.Size = new Size(155, 38);
        changePassword.Click += (s, e) => { using ChangePasswordForm form = new(_user); form.ShowDialog(this); };
        Label info = Ui.MakeLabel("Bảo vệ tài khoản bằng mật khẩu riêng và không chia sẻ mật khẩu.", 9, false); info.ForeColor = Ui.Muted; info.Location = new Point(28, 215);
        card.Controls.AddRange(new Control[] { title, nameLabel, name, accountLabel, account, roleLabel, role, info, changePassword });
        _content.Controls.Add(card);
    }

    private static EntityDefinition GetMovieDefinition() => new()
    {
        Title = "phim", Table = "Movies", IdColumn = "MovieID",
        SelectSql = "SELECT MovieID, Title AS [Tên phim], Genre AS [Thể loại], DurationMinutes AS [Thời lượng (phút)], AgeRating AS [Phân loại], ReleaseDate AS [Ngày khởi chiếu], IsActive AS [Hoạt động] FROM dbo.Movies ORDER BY MovieID DESC",
        Fields = new()
        {
            new() { Column="Title", Label="Tên phim", Kind="text", Required=true },
            new() { Column="Genre", Label="Thể loại", Kind="text" },
            new() { Column="DurationMinutes", Label="Thời lượng (phút)", Kind="int", Required=true },
            new() { Column="Description", Label="Mô tả phim", Kind="multiline" },
            new() { Column="AgeRating", Label="Phân loại độ tuổi (ví dụ: T13)", Kind="text" },
            new() { Column="ReleaseDate", Label="Ngày khởi chiếu", Kind="date" },
            new() { Column="IsActive", Label="Trạng thái", Kind="bool" }
        }
    };

    private static EntityDefinition GetRoomDefinition() => new()
    {
        Title = "phòng chiếu", Table = "Rooms", IdColumn = "RoomID",
        SelectSql = "SELECT RoomID, RoomName AS [Tên phòng], Capacity AS [Số ghế], IsActive AS [Hoạt động] FROM dbo.Rooms ORDER BY RoomID DESC",
        Fields = new()
        {
            new() { Column="RoomName", Label="Tên phòng", Kind="text", Required=true },
            new() { Column="Capacity", Label="Số ghế", Kind="int", Required=true },
            new() { Column="IsActive", Label="Trạng thái", Kind="bool" }
        }
    };

    private static EntityDefinition GetShowtimeDefinition() => new()
    {
        Title = "lịch chiếu", Table = "Showtimes", IdColumn = "ShowtimeID",
        SelectSql = @"SELECT s.ShowtimeID, m.Title AS [Tên phim], r.RoomName AS [Phòng], s.StartTime AS [Bắt đầu], s.EndTime AS [Kết thúc], s.TicketPrice AS [Giá vé]
            FROM dbo.Showtimes s INNER JOIN dbo.Movies m ON m.MovieID=s.MovieID INNER JOIN dbo.Rooms r ON r.RoomID=s.RoomID ORDER BY s.StartTime DESC",
        Fields = new()
        {
            new() { Column="MovieID", Label="Phim", Kind="comboMovie", Required=true },
            new() { Column="RoomID", Label="Phòng chiếu", Kind="comboRoom", Required=true },
            new() { Column="StartTime", Label="Ngày giờ bắt đầu", Kind="datetime", Required=true },
            new() { Column="EndTime", Label="Ngày giờ kết thúc", Kind="datetime", Required=true },
            new() { Column="TicketPrice", Label="Giá vé (VNĐ)", Kind="decimal", Required=true }
        }
    };

    private static EntityDefinition GetUserDefinition() => new()
    {
        Title = "tài khoản", Table = "Users", IdColumn = "UserID",
        SelectSql = "SELECT UserID, Username AS [Tên đăng nhập], FullName AS [Họ tên], Role AS [Vai trò], IsActive AS [Hoạt động], CreatedAt AS [Ngày tạo] FROM dbo.Users ORDER BY UserID DESC",
        Fields = new()
        {
            new() { Column="Username", Label="Tên đăng nhập (tối thiểu 3 ký tự)", Kind="text", Required=true },
            new() { Column="FullName", Label="Họ và tên", Kind="text", Required=true },
            new() { Column="PasswordHash", Label="Mật khẩu", Kind="password", Required=true },
            new() { Column="Role", Label="Vai trò", Kind="comboRole", Required=true },
            new() { Column="IsActive", Label="Trạng thái tài khoản", Kind="bool" }
        }
    };

    private static int? SelectedId(DataGridView grid, string idColumn)
    {
        if (grid.SelectedRows.Count == 0 || !grid.Columns.Contains(idColumn)) return null;
        object? value = grid.SelectedRows[0].Cells[idColumn].Value;
        if (value == null || value == DBNull.Value) return null;
        return int.TryParse(Convert.ToString(value), out int id) ? id : null;
    }

    private static void HideColumns(DataGridView grid, params string[] names)
    {
        foreach (string name in names)
        {
            DataGridViewColumn ? column = grid.Columns[name];

            if (column != null)
            {
                column.Visible = false;
            }
        }
    }
    private static void FormatMoneyColumn(DataGridView grid, string column)
    {
        DataGridViewColumn ? moneyColumn = grid.Columns[column];

        if (moneyColumn != null)
        {
            moneyColumn.DefaultCellStyle.Format = "N0";
        }
    }

    private void ShowError(string title, Exception ex)
        => MessageBox.Show(title + ".\n\n" + ex.Message, "CinemaManagement", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
