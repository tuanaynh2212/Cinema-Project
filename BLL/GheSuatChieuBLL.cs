using cinema_project.DAL;
using cinema_project.DAL.DTO;

namespace cinema_project.BLL
{
    public sealed class GheSuatChieuBLL
    {
        private readonly GheSuatChieuDAL _dal;
        private readonly VeDaDatDAL _veDaDatDAL;

        public GheSuatChieuBLL(GheSuatChieuDAL dal, VeDaDatDAL veDaDatDAL)
        {
            _dal = dal;
            _veDaDatDAL = veDaDatDAL;
        }

        public async Task<List<string>> LayGheDaDatAsync(string maSuatChieu)
        {
            if (!int.TryParse(maSuatChieu, out int maSuatChieuInt))
            {
                throw new ArgumentException("Mã suất chiếu không hợp lệ.", nameof(maSuatChieu));
            }

            return await _dal.LayGheDaDatAsync(maSuatChieuInt);
        }
        public async Task DatGheAsync(List<cinema_project.DAL.GheSuatChieuDat> ghedat)
        {
            if (ghedat == null || ghedat.Count == 0)
            {
                throw new ArgumentException("Danh sách ghế đặt không hợp lệ.", nameof(ghedat));
            }
             await _dal.DatGheAsync(ghedat);
        }
        public async Task<List<VeDaDatItem>> TatCaGheDaDat()
        {
            return await _veDaDatDAL.LayTatCaGheDaDatAsync();
        }
    }
}