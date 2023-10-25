using BaiCuoiKhoaBackEnd.Entities;
using BaiCuoiKhoaBackEnd.Constants;

namespace BaiCuoiKhoaBackEnd.ISevices
{
    public interface ILoaiKhoaHocServices
    {
        IQueryable<LoaiKhoaHoc> GetDSLoaiKH();
        ErrorMessage ThemLoaiKhoaHoc(LoaiKhoaHoc loaiKhoaHoc);
        ErrorMessage SuaLoaiKhoaHoc(LoaiKhoaHoc loaiKhoaHocUpdate);
        ErrorMessage XoaLoaiKhoaHoc(int id);
    }
}
