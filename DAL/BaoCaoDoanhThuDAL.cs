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

        // Chi lay giao dich da thanh toan thanh cong.
        public async Task<List<BaoCaoDoanhThu>> LayDoanhThuDaThanhToanAsync()
        {
            string path =
                "ticket_sales?select=id,movie_id,movie_title,quantity,unit_price,total_amount,payment_status,purchased_at" +
                "&payment_status=eq.paid&order=purchased_at.desc";

            using (HttpRequestMessage request =
                   _supabase.CreateRequest(HttpMethod.Get, path))
            using (HttpResponseMessage response =
                   await new HttpClient { Timeout = TimeSpan.FromSeconds(20) }.SendAsync(request))
            {
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
}
