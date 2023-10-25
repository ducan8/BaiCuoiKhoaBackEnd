using BaiCuoiKhoaBackEnd.Constants;
using BaiCuoiKhoaBackEnd.Entities;

namespace BaiCuoiKhoaBackEnd.ISevices
{
    public interface ILoaiBaiVietServices
    {
        IQueryable<LoaiBaiViet> HienThiDanhSachLoaiBaiViet();
        ErrorMessage ThemLoaiBaiViet(LoaiBaiViet loaiBaiViet);
        ErrorMessage SuaLoaiBaiViet(LoaiBaiViet loaiBaiVietUpdate);
        ErrorMessage XoaLoaiBaiViet(int id);
        PageResult<LoaiBaiViet> PhanTrangDanhSachLoaiBaiViet(Pagination pagination);
    }
}
