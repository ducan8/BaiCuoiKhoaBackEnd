using BaiCuoiKhoaBackEnd.Constants;
using BaiCuoiKhoaBackEnd.Entities;

namespace BaiCuoiKhoaBackEnd.ISevices
{
    public interface IDangKyHocServices
    {
        IQueryable<DangKyHoc> HienThiDanhSachDangKyHoc();
        ErrorMessage ThemDangKyHoc(DangKyHoc dangKyHoc);
        ErrorMessage SuaDangKyHoc(DangKyHoc dangKyHocUpdate);
        ErrorMessage XoaDangKyHoc(int id);
        PageResult<DangKyHoc> PhanTrangDuLieu(Pagination pagination);
    }
}
