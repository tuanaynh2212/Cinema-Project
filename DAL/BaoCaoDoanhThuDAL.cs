using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace cinema_project.DAL
{
    public class BaoCaoDoanhThuDAL
    {
        private readonly SupabaseService _supabase = new SupabaseService();

        private static readonly JsonSerializerOptions _json =
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        // Chỉ lấy giao dịch đã thanh toán. Admin xem toàn bộ; người dùng thường chỉ xem giao dịch của mình.
        // Quyền truy cập cuối cùng vẫn phải được bảo vệ bằng RLS ở Supabase.
        public async Task<List<BaoCaoDoanhThu>> LayDoanhThuDaThanhToanAsync()
        {
            AuthLoginResult? phien = PhienDangNhap.NguoiDung;
            if (phien == null || string.IsNullOrWhiteSpace(phien.UserId) ||
                string.IsNullOrWhiteSpace(phien.AccessToken))
            {
                throw new UnauthorizedAccessException(
                    "Vui lòng đăng nhập trước khi xem báo cáo doanh thu.");
            }

            string locNguoiDung = phien.LaAdmin
                ? string.Empty
                : "&user_id=eq." + Uri.EscapeDataString(phien.UserId);

            string path =
                "ticket_sales?select=id,user_id,movie_id,movie_title,quantity,unit_price,total_amount,payment_status,purchased_at" +
                "&payment_status=eq.paid" + locNguoiDung +
                "&order=purchased_at.desc";

            using HttpRequestMessage request =
                _supabase.CreateRequest(HttpMethod.Get, path, phien.AccessToken);
            using HttpResponseMessage response =
                await new HttpClient { Timeout = TimeSpan.FromSeconds(20) }.SendAsync(request);

            string noiDung = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    "Không lấy được dữ liệu doanh thu từ Supabase. HTTP " +
                    (int)response.StatusCode + ": " + noiDung);
            }

            return JsonSerializer.Deserialize<List<BaoCaoDoanhThu>>(noiDung, _json)
                   ?? new List<BaoCaoDoanhThu>();
        }

    }
}
