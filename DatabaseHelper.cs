using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CinemaManagement;

public sealed class UserSession
{
    public int UserId { get; init; }
    public string Username { get; init; } = "";
    public string FullName { get; init; } = "";
    public string Role { get; init; } = "Customer";
}

public static class DatabaseHelper
{
    private static string? _connectionString;

    public static string ConnectionString
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(_connectionString)) return _connectionString;
            string? fromEnvironment = Environment.GetEnvironmentVariable("CINEMA_CONNECTION_STRING");
            if (!string.IsNullOrWhiteSpace(fromEnvironment)) return _connectionString = fromEnvironment;

            string configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (File.Exists(configPath))
            {
                using JsonDocument doc = JsonDocument.Parse(File.ReadAllText(configPath));
                if (doc.RootElement.TryGetProperty("ConnectionStrings", out JsonElement section) &&
                    section.TryGetProperty("CinemaDb", out JsonElement value) &&
                    !string.IsNullOrWhiteSpace(value.GetString()))
                    return _connectionString = value.GetString()!;
            }
            return _connectionString = @"Server=LAPTOP-A5O29CMV\LOCALDB#3CFFF476\MSSQLLocalDB;Database=CinemaDB;Trusted_Connection=True;TrustServerCertificate=True;";
        }
    }

    public static SqlConnection GetConnection() => new(ConnectionString);

    public static DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
    {
        using SqlConnection conn = GetConnection();
        using SqlCommand cmd = new(query, conn);
        if (parameters.Length > 0) cmd.Parameters.AddRange(parameters);
        using SqlDataAdapter adapter = new(cmd);
        DataTable table = new();
        adapter.Fill(table);
        return table;
    }

    public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
    {
        using SqlConnection conn = GetConnection();
        using SqlCommand cmd = new(query, conn);
        if (parameters.Length > 0) cmd.Parameters.AddRange(parameters);
        conn.Open();
        return cmd.ExecuteNonQuery();
    }

    public static object? ExecuteScalar(string query, params SqlParameter[] parameters)
    {
        using SqlConnection conn = GetConnection();
        using SqlCommand cmd = new(query, conn);
        if (parameters.Length > 0) cmd.Parameters.AddRange(parameters);
        conn.Open();
        return cmd.ExecuteScalar();
    }

    public static string HashPassword(string password)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    public static UserSession? Authenticate(string username, string password)
    {
        const string sql = @"SELECT UserID, Username, FullName, Role
                             FROM dbo.Users
                             WHERE Username = @Username AND PasswordHash = @PasswordHash AND IsActive = 1;";
        DataTable table = ExecuteQuery(sql,
            new SqlParameter("@Username", SqlDbType.NVarChar, 80) { Value = username.Trim() },
            new SqlParameter("@PasswordHash", SqlDbType.Char, 64) { Value = HashPassword(password) });
        if (table.Rows.Count == 0) return null;
        DataRow row = table.Rows[0];
        return new UserSession
        {
            UserId = Convert.ToInt32(row["UserID"]),
            Username = Convert.ToString(row["Username"]) ?? "",
            FullName = Convert.ToString(row["FullName"]) ?? "",
            Role = Convert.ToString(row["Role"]) ?? "Customer"
        };
    }

    public static void Register(string username, string fullName, string password)
    {
        const string sql = @"INSERT INTO dbo.Users(Username, FullName, PasswordHash, Role, IsActive)
                             VALUES(@Username, @FullName, @PasswordHash, 'Customer', 1);";
        ExecuteNonQuery(sql,
            new SqlParameter("@Username", SqlDbType.NVarChar, 80) { Value = username.Trim() },
            new SqlParameter("@FullName", SqlDbType.NVarChar, 120) { Value = fullName.Trim() },
            new SqlParameter("@PasswordHash", SqlDbType.Char, 64) { Value = HashPassword(password) });
    }

    public static string CreateBooking(int userId, int showtimeId, IReadOnlyList<string> seats)
    {
        if (seats.Count == 0) throw new InvalidOperationException("Vui lòng chọn ít nhất một ghế.");
        using SqlConnection conn = GetConnection();
        conn.Open();
        using SqlTransaction tx = conn.BeginTransaction(IsolationLevel.Serializable);
        try
        {
            decimal price;
            using (SqlCommand cmd = new("SELECT TicketPrice FROM dbo.Showtimes WHERE ShowtimeID=@ShowtimeID AND StartTime > SYSDATETIME()", conn, tx))
            {
                cmd.Parameters.Add("@ShowtimeID", SqlDbType.Int).Value = showtimeId;
                object? result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value) throw new InvalidOperationException("Suất chiếu không tồn tại hoặc đã bắt đầu.");
                price = Convert.ToDecimal(result);
            }

            foreach (string seat in seats)
            {
                using SqlCommand check = new(@"SELECT COUNT(*) FROM dbo.BookingSeats bs
                    INNER JOIN dbo.Bookings b ON b.BookingID=bs.BookingID
                    WHERE bs.ShowtimeID=@ShowtimeID AND bs.SeatCode=@SeatCode AND b.Status IN ('Pending','Paid')", conn, tx);
                check.Parameters.Add("@ShowtimeID", SqlDbType.Int).Value = showtimeId;
                check.Parameters.Add("@SeatCode", SqlDbType.NVarChar, 10).Value = seat;
                if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                    throw new InvalidOperationException($"Ghế {seat} vừa được người khác đặt. Vui lòng chọn ghế khác.");
            }

            string ticketCode = "CIN" + DateTime.Now.ToString("yyMMdd") + Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
            int bookingId;
            using (SqlCommand insert = new(@"INSERT INTO dbo.Bookings(UserID, ShowtimeID, TicketCode, TotalAmount, Status)
                OUTPUT INSERTED.BookingID VALUES(@UserID, @ShowtimeID, @TicketCode, @TotalAmount, 'Paid')", conn, tx))
            {
                insert.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                insert.Parameters.Add("@ShowtimeID", SqlDbType.Int).Value = showtimeId;
                insert.Parameters.Add("@TicketCode", SqlDbType.VarChar, 30).Value = ticketCode;
                SqlParameter total = insert.Parameters.Add("@TotalAmount", SqlDbType.Decimal);
                total.Precision = 12; total.Scale = 2; total.Value = price * seats.Count;
                bookingId = Convert.ToInt32(insert.ExecuteScalar());
            }
            foreach (string seat in seats)
            {
                using SqlCommand insertSeat = new(@"INSERT INTO dbo.BookingSeats(BookingID, ShowtimeID, SeatCode, SeatPrice)
                    VALUES(@BookingID, @ShowtimeID, @SeatCode, @SeatPrice)", conn, tx);
                insertSeat.Parameters.Add("@BookingID", SqlDbType.Int).Value = bookingId;
                insertSeat.Parameters.Add("@ShowtimeID", SqlDbType.Int).Value = showtimeId;
                insertSeat.Parameters.Add("@SeatCode", SqlDbType.NVarChar, 10).Value = seat;
                SqlParameter seatPrice = insertSeat.Parameters.Add("@SeatPrice", SqlDbType.Decimal);
                seatPrice.Precision = 12; seatPrice.Scale = 2; seatPrice.Value = price;
                insertSeat.ExecuteNonQuery();
            }
            tx.Commit();
            return ticketCode;
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    public static bool ChangePassword(int userId, string currentPassword, string newPassword)
    {
        int affected = ExecuteNonQuery(@"UPDATE dbo.Users SET PasswordHash=@NewHash
            WHERE UserID=@UserID AND PasswordHash=@CurrentHash AND IsActive=1",
            new SqlParameter("@NewHash", SqlDbType.Char, 64) { Value = HashPassword(newPassword) },
            new SqlParameter("@UserID", SqlDbType.Int) { Value = userId },
            new SqlParameter("@CurrentHash", SqlDbType.Char, 64) { Value = HashPassword(currentPassword) });
        return affected == 1;
    }

    public static HashSet<string> GetBookedSeats(int showtimeId)
    {
        DataTable table = ExecuteQuery(@"SELECT bs.SeatCode FROM dbo.BookingSeats bs
            INNER JOIN dbo.Bookings b ON b.BookingID=bs.BookingID
            WHERE bs.ShowtimeID=@ShowtimeID AND b.Status IN ('Pending','Paid')",
            new SqlParameter("@ShowtimeID", SqlDbType.Int) { Value = showtimeId });
        return table.Rows.Cast<DataRow>().Select(r => Convert.ToString(r["SeatCode"]) ?? "").ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    public static decimal GetTicketPrice(int showtimeId)
    {
        object? value = ExecuteScalar("SELECT TicketPrice FROM dbo.Showtimes WHERE ShowtimeID=@ID AND StartTime > SYSDATETIME()",
            new SqlParameter("@ID", SqlDbType.Int) { Value = showtimeId });
        if (value == null || value == DBNull.Value) throw new InvalidOperationException("Suất chiếu không tồn tại hoặc đã bắt đầu.");
        return Convert.ToDecimal(value);
    }

    public static bool CanConnect(out string message)
    {
        try
        {
            using SqlConnection conn = GetConnection();
            conn.Open();
            message = "Kết nối SQL Server thành công.";
            return true;
        }
        catch (Exception ex)
        {
            message = "Không kết nối được SQL Server. Hãy kiểm tra appsettings.json và trạng thái SQL Server.\n\nChi tiết: " + ex.Message;
            return false;
        }
    }
}
