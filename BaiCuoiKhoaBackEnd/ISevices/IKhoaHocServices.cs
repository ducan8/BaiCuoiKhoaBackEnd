using BaiCuoiKhoaBackEnd.Entities;
using BaiCuoiKhoaBackEnd.Constants;

namespace BaiCuoiKhoaBackEnd.ISevices
{
    public interface IKhoaHocServices
    {
        IQueryable<KhoaHoc> HienThiDanhSachKhoaHoc();
        ErrorMessage ThemKhoaHoc(KhoaHoc khoahoc);
        ErrorMessage SuaKhoaHoc (KhoaHoc khoahocUpdate);
        ErrorMessage XoaKhoaHoc(int id);
        IQueryable<KhoaHoc> TimKhoaHocTheoTen(string keyword);
        PageResult<KhoaHoc> PhanTrangDanhSachKhoaHoc(Pagination pagination);
    }
}
