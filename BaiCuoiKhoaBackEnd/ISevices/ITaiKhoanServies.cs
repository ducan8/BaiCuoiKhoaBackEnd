using BaiCuoiKhoaBackEnd.Constants;
using BaiCuoiKhoaBackEnd.Entities;

namespace BaiCuoiKhoaBackEnd.ISevices
{
    public interface ITaiKhoanServies
    {
        IQueryable<TaiKhoan> HienThiDanhSachTaiKhoan();
        ErrorMessage ThemTaiKhoan(TaiKhoan taikhoan);
        ErrorMessage SuaTaiKhoan(TaiKhoan taikhoanUpdate);
        ErrorMessage XoaTaiKhoan(int id);
        IQueryable<TaiKhoan> TimKiemTaiKhoan(string keyword);
        PageResult<TaiKhoan> PhanTrangDanhSachTaiKhoan(Pagination pagination, string? keyword);

    }
}
