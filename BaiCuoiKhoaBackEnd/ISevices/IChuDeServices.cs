using BaiCuoiKhoaBackEnd.Constants;
using BaiCuoiKhoaBackEnd.Entities;

namespace BaiCuoiKhoaBackEnd.ISevices
{
    public interface IChuDeServices
    {
        IQueryable<ChuDe> HienThiDanhSachChuDe();
        ErrorMessage ThemChuDe(ChuDe chude);
        ErrorMessage SuaChuDe(ChuDe chudeUpdate);
        ErrorMessage XoaChuDe(int id);
        PageResult<ChuDe> PhanTrangDanhSachChuDe(Pagination pagination);
    }
}
