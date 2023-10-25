using BaiCuoiKhoaBackEnd.Constants;
using BaiCuoiKhoaBackEnd.Entities;

namespace BaiCuoiKhoaBackEnd.ISevices
{
    public interface IHocVienServices
    {
        IQueryable<HocVien> HienThiDanhSachHocVien();
        ErrorMessage ThemHocVien(HocVien hocVien);
        ErrorMessage SuaHocVien(HocVien hocvienUpdate);
        ErrorMessage XoaHocVien(int id);
        IQueryable<HocVien> TimKiemTheoTenVaEmail(string keyword);
        PageResult<HocVien> PhanTrangDuLieu(Pagination pagination, string? keyword);
    }
}
