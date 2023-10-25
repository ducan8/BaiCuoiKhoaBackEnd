using BaiCuoiKhoaBackEnd.Constants;
using BaiCuoiKhoaBackEnd.Entities;

namespace BaiCuoiKhoaBackEnd.ISevices
{
    public interface IBaiVietServices
    {
        IQueryable<BaiViet> HienThiDanhSachBaiViet();
        ErrorMessage ThemBaiViet(BaiViet baiviet);
        ErrorMessage SuaBaiViet(BaiViet baiVietUpdate);
        ErrorMessage XoaBaiViet(int id);
        IQueryable<BaiViet> TimBaiVietTheoTen(string keyword);
        PageResult<BaiViet> PhanTrangDanhSachBaiViet(Pagination pagination, string? keyword);
    }
}
