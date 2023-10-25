using BaiCuoiKhoaBackEnd.Constants;
using BaiCuoiKhoaBackEnd.Entities;

namespace BaiCuoiKhoaBackEnd.ISevices
{
    public interface ITinhTrangHocServies
    {
        IQueryable<TinhTrangHoc> HienThiDanhSachTinhTrangHoc();
        ErrorMessage ThemTinhTrangHoc(TinhTrangHoc tinhTrangHoc);
        ErrorMessage SuaTinhTrangHoc(TinhTrangHoc tinhTrangHocUpdate);
        ErrorMessage XoaTinhTrangHoc(int id);
    }
}
