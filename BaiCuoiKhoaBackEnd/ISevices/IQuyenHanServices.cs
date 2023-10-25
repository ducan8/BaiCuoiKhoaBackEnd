using BaiCuoiKhoaBackEnd.Constants;
using BaiCuoiKhoaBackEnd.Entities;
using System.Linq;

namespace BaiCuoiKhoaBackEnd.ISevices
{
    public interface IQuyenHanServices
    {
        IQueryable<QuyenHan> HienThiDanhSachQuyenHan();
        ErrorMessage ThemQuyenHan(QuyenHan quyenHan);
        ErrorMessage SuaQuyenHan(QuyenHan quyenHanUpdate);
        ErrorMessage XoaQuyenHan(int id);
        PageResult<QuyenHan> PhanTrangHienThiQuyenHan(Pagination pagination);
    }
}
