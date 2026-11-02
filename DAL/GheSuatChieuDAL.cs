#nullable enable
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using cinema_project.DAL.DTO;
using cinema_project.Logging;
using Microsoft.Extensions.Logging;

namespace cinema_project.DAL
{
    public sealed class GheSuatChieuDAL
    {
        private readonly SupabaseService _supabase = new SupabaseService();
        private readonly ILogger<GheSuatChieuDAL> _logger;
        private static readonly HttpClient _http = new HttpClient { Timeout = TimeSpan.FromSeconds(20) };
        private static readonly JsonSerializerOptions _json = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public GheSuatChieuDAL(ILogger<GheSuatChieuDAL>? logger = null)
        {
            _logger = logger ?? AppLogging.CreateLogger<GheSuatChieuDAL>();
        }

        private AuthLoginResult LayPhienDangNhap()
        {
            AuthLoginResult? phien = PhienDangNhap.NguoiDung;
            if (phien == null || string.IsNullOrWhiteSpace(phien.UserId) ||
                string.IsNullOrWhiteSpace(phien.AccessToken))
            {
                _logger.LogWarning("Chưa đăng nhập hoặc phiên không hợp lệ");
                throw new UnauthorizedAccessException("Vui lòng đăng nhập trước khi đặt vé.");
            }

            return phien;
        }

        public async Task<List<string>> LayGheDaDatAsync(int maSuatChieu)
        {
            if (maSuatChieu <= 0)
            {
                _logger.LogWarning("Mã suất chiếu không hợp lệ: {MaSuatChieu}", maSuatChieu);
                throw new ArgumentOutOfRangeException(nameof(maSuatChieu), "Mã suất chiếu không hợp lệ.");
            }

            AuthLoginResult phien = LayPhienDangNhap();
            _logger.LogDebug("Tải ghế đã đặt của suất chiếu {MaSuatChieu}", maSuatChieu);

            try
            {
                using HttpRequestMessage request = _supabase.CreateRequest(
                    HttpMethod.Get,
                    $"ghe_suat_chieu?select=ma_ghe&ma_suat_chieu=eq.{maSuatChieu}&trang_thai=eq.da_dat",
                    phien.AccessToken);

                using HttpResponseMessage response = await _http.SendAsync(request);
                string noiDung = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Tải ghế đã đặt thất bại: suất {MaSuatChieu}, HTTP {Status}, body={Body}",
                        maSuatChieu, (int)response.StatusCode, noiDung);
                    throw new InvalidOperationException(
                        $"Không tải được ghế đã đặt (HTTP {(int)response.StatusCode}). {noiDung}");
                }

                List<GheSuatChieuDat>? danhSach = JsonSerializer.Deserialize<List<GheSuatChieuDat>>(noiDung, _json);
                var daThem = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var ketQua = new List<string>();

                if (danhSach != null)
                {
                    foreach (GheSuatChieuDat ghe in danhSach)
                    {
                        if (string.IsNullOrWhiteSpace(ghe.MaGhe)) continue;
                        string ma = ghe.MaGhe.Trim().ToUpperInvariant();
                        if (daThem.Add(ma)) ketQua.Add(ma);
                    }
                }

                _logger.LogInformation("Suất chiếu {MaSuatChieu} có {Count} ghế đã đặt", maSuatChieu, ketQua.Count);
                return ketQua;
            }
            catch (Exception ex) when (ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Lỗi khi tải ghế đã đặt của suất chiếu {MaSuatChieu}", maSuatChieu);
                throw;
            }
        }

        public async Task DatGheAsync(List<GheSuatChieuDat> danhSachGhe)
        {
            if (danhSachGhe == null)
                throw new ArgumentNullException(nameof(danhSachGhe));

            AuthLoginResult phien = LayPhienDangNhap();

            var ds = new List<GheSuatChieuDat>();
            var dsMaGhe = new List<string>();
            bool hopLe = true;

            foreach (GheSuatChieuDat x in danhSachGhe)
            {
                if (x == null) continue;

                string maGhe = (x.MaGhe ?? "").Trim().ToUpperInvariant();
                if (x.MaSuatChieu <= 0 || string.IsNullOrWhiteSpace(maGhe) || x.GiaGhe < 0)
                    hopLe = false;

                ds.Add(new GheSuatChieuDat
                {
                    MaSuatChieu = x.MaSuatChieu,
                    UserId = phien.UserId,
                    MaGhe = maGhe,
                    GiaGhe = x.GiaGhe,
                    TrangThai = "da_dat"
                });
                dsMaGhe.Add(maGhe);
            }

            if (ds.Count == 0)
            {
                _logger.LogWarning("Đặt ghế: danh sách ghế rỗng");
                throw new InvalidOperationException("Không có ghế nào được chọn.");
            }
            if (!hopLe)
            {
                _logger.LogWarning("Đặt ghế: thông tin ghế không hợp lệ");
                throw new InvalidOperationException("Thông tin ghế đặt không hợp lệ.");
            }

            int maSuatChieu = ds[0].MaSuatChieu;
            string chuoiGhe = string.Join(",", dsMaGhe);

            _logger.LogInformation("Đặt {Count} ghế: suất {MaSuatChieu}, ghế {DanhSachGhe}, user {UserId}",
                ds.Count, maSuatChieu, chuoiGhe, phien.UserId);

            try
            {
                using HttpRequestMessage request = _supabase.CreateRequest(
                    HttpMethod.Post, "ghe_suat_chieu", phien.AccessToken);
                request.Headers.TryAddWithoutValidation("Prefer", "return=minimal");
                request.Content = new StringContent(JsonSerializer.Serialize(ds), Encoding.UTF8, "application/json");

                using HttpResponseMessage response = await _http.SendAsync(request);
                string noiDung = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    int status = (int)response.StatusCode;
                    if (status == 409)
                        _logger.LogWarning("Đặt ghế xung đột (409): suất {MaSuatChieu}, ghế {DanhSachGhe}",
                            maSuatChieu, chuoiGhe);
                    else
                        _logger.LogError("Đặt ghế thất bại: HTTP {Status}, body={Body}", status, noiDung);

                    string giaiThich = status == 409
                        ? "Một hoặc nhiều ghế vừa được người khác đặt. Hãy tải lại sơ đồ ghế và chọn lại."
                        : $"Không thể lưu ghế (HTTP {status}).";
                    throw new InvalidOperationException($"{giaiThich}\n{noiDung}");
                }

                _logger.LogInformation("Đặt ghế thành công: suất {MaSuatChieu}, {Count} ghế", maSuatChieu, ds.Count);
            }
            catch (Exception ex) when (ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Lỗi khi đặt ghế suất {MaSuatChieu}", maSuatChieu);
                throw;
            }
        }
    }
}