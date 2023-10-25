using BaiCuoiKhoaBackEnd.Constants;
using BaiCuoiKhoaBackEnd.Entities;
using BaiCuoiKhoaBackEnd.ISevices;

namespace BaiCuoiKhoaBackEnd.Services
{
    public class TaiKhoanServices : ITaiKhoanServies
    {
        private readonly AppDbContext dbContext;
        public TaiKhoanServices()
        {
            dbContext = new AppDbContext();
        }
        IQueryable<TaiKhoan> ITaiKhoanServies.HienThiDanhSachTaiKhoan()
        {
            var listTK = dbContext.TaiKhoan.ToList();
            if (listTK == null)
            {
                return null;
            }
            return listTK.AsQueryable();
        }

        PageResult<TaiKhoan> ITaiKhoanServies.PhanTrangDanhSachTaiKhoan(Pagination pagination, string? keyword)
        {
            var listTaiKhoan = dbContext.TaiKhoan.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                listTaiKhoan = listTaiKhoan.Where(x => x.TenDangNhap.Contains(keyword));

            }
            pagination.TotalCount = listTaiKhoan.Count();
            var res = PageResult<TaiKhoan>.ToPageResult(pagination, listTaiKhoan);
            return new PageResult<TaiKhoan>() { Pagination = pagination, Data = res };
        }

        ErrorMessage ITaiKhoanServies.SuaTaiKhoan(TaiKhoan taikhoanUpdate)
        {
            var taikhoanCanSua = dbContext.TaiKhoan.FirstOrDefault(x => x.TaiKhoanId == taikhoanUpdate.TaiKhoanId);
            if (taikhoanCanSua == null)
            {
                return ErrorMessage.ChuaTonTai;
            }
            else
            {
                if (!dbContext.QuyenHan.Any(x => x.QuyenHanId == taikhoanUpdate.QuyenHanId))
                {
                    return ErrorMessage.QuyenHanChuaTonTai;
                }
                taikhoanCanSua.TenNguoiDung = taikhoanUpdate.TenNguoiDung;
                if (taikhoanCanSua.TenDangNhap == taikhoanUpdate.TenDangNhap)
                {

                }
                else if (dbContext.TaiKhoan.Any(x => x.TenDangNhap.Equals(taikhoanUpdate.TenDangNhap)))
                {
                    return ErrorMessage.DaTonTai;
                }
                if (!taikhoanUpdate.MatKhau.Any(x => !char.IsLetterOrDigit(x)))
                {
                    return ErrorMessage.MatKhauThieuKyTuDacBiet;
                }
                if (!taikhoanUpdate.MatKhau.Any(x => char.IsDigit(x)))
                {
                    return ErrorMessage.MatKhauThieuChuSo;
                }
                taikhoanCanSua.MatKhau = taikhoanUpdate.MatKhau;
                taikhoanCanSua.QuyenHanId = taikhoanUpdate.QuyenHanId;

                dbContext.TaiKhoan.Update(taikhoanCanSua);
                dbContext.SaveChanges();
                return ErrorMessage.ThanhCong;
            }
        }

        ErrorMessage ITaiKhoanServies.ThemTaiKhoan(TaiKhoan taikhoan)
        {
            if (!dbContext.QuyenHan.Any(x => x.QuyenHanId == taikhoan.QuyenHanId))
            {
                return ErrorMessage.ChuaTonTai;
            }
            var listTK = dbContext.TaiKhoan;
            if (listTK.Any(x => x.TenDangNhap.Equals(taikhoan.TenDangNhap)))
            {
                return ErrorMessage.DaTonTai;
            }
            if (!taikhoan.MatKhau.Any(x => !char.IsLetterOrDigit(x)))
            {
                return ErrorMessage.MatKhauThieuKyTuDacBiet;
            }
            if (!taikhoan.MatKhau.Any(x => char.IsDigit(x)))
            {
                return ErrorMessage.MatKhauThieuChuSo;
            }
            dbContext.TaiKhoan.Add(taikhoan);
            dbContext.SaveChanges();
            return ErrorMessage.ThanhCong;
        }

        IQueryable<TaiKhoan> ITaiKhoanServies.TimKiemTaiKhoan(string keyword)
        {
            var taikhoan = dbContext.TaiKhoan.Where(x => x.TenDangNhap.Contains(keyword));
            if (taikhoan == null)
            {
                return null;
            }
            return taikhoan;
        }

        ErrorMessage ITaiKhoanServies.XoaTaiKhoan(int id)
        {
            var taikhoanCanXoa = dbContext.TaiKhoan.FirstOrDefault(x => x.TaiKhoanId == id);
            if(taikhoanCanXoa == null)
            {
                return ErrorMessage.ChuaTonTai;
            }
            dbContext.TaiKhoan.Remove(taikhoanCanXoa); 
            dbContext.SaveChanges();
            return ErrorMessage.ThanhCong;
        }
    }
}
