using BaiCuoiKhoaBackEnd.Constants;
using BaiCuoiKhoaBackEnd.Entities;
using BaiCuoiKhoaBackEnd.ISevices;
using Microsoft.EntityFrameworkCore;

namespace BaiCuoiKhoaBackEnd.Services
{
    public class BaiVietServices : IBaiVietServices
    {
        private readonly AppDbContext dbContext;
        public BaiVietServices()
        {
            dbContext = new AppDbContext();
        }
        IQueryable<BaiViet> IBaiVietServices.HienThiDanhSachBaiViet()
        {
            var listBaiViet = dbContext.BaiViet.AsQueryable();
            if (listBaiViet == null)
            {
                return null;
            }
            return listBaiViet;
        }

        PageResult<BaiViet> IBaiVietServices.PhanTrangDanhSachBaiViet(Pagination pagination, string? keyword)
        {
            var listBaiViet = dbContext.BaiViet.AsQueryable();
            pagination.TotalCount = listBaiViet.Count();
            if (!string.IsNullOrEmpty(keyword))
            {
                listBaiViet = listBaiViet.Where(x => x.TenBaiViet.Contains(keyword));
            }
            var res = PageResult<BaiViet>.ToPageResult(pagination, listBaiViet);
            return new PageResult<BaiViet>() { Data = res, Pagination = pagination };

        }

        ErrorMessage IBaiVietServices.SuaBaiViet(BaiViet baiVietUpdate)
        {
            var baivietCanSua = dbContext.BaiViet.FirstOrDefault(x => x.BaiVietId == baiVietUpdate.BaiVietId);
            if(baivietCanSua == null)
            {
                return ErrorMessage.ChuaTonTai;
            }

            if(!dbContext.ChuDe.Any(x => x.ChuDeId == baiVietUpdate.ChuDeId) || !dbContext.TaiKhoan.Any(x => x.TaiKhoanId == baiVietUpdate.TaiKhoanId))
            {
                return ErrorMessage.ChuaTonTai;
            }
            
            baivietCanSua.TenBaiViet = baiVietUpdate.TenBaiViet;
            baivietCanSua.ThoiGianTao = DateTime.Now;
            baivietCanSua.TenTacGia = baiVietUpdate.TenTacGia;
            baivietCanSua.NoiDung = baiVietUpdate.NoiDung;
            baivietCanSua.NoiDungNgan = baiVietUpdate.NoiDungNgan;
            baivietCanSua.HinhAnh = baiVietUpdate.HinhAnh;
            baivietCanSua.ChuDeId = baiVietUpdate.ChuDeId;
            baivietCanSua.TaiKhoanId = baiVietUpdate.TaiKhoanId;

            dbContext.BaiViet.Update(baivietCanSua);
            dbContext.SaveChanges();
            return ErrorMessage.ThanhCong;
        }

        ErrorMessage IBaiVietServices.ThemBaiViet(BaiViet baiviet)
        {
            if (!dbContext.ChuDe.Any(x => x.ChuDeId == baiviet.ChuDeId) || !dbContext.TaiKhoan.Any(x => x.TaiKhoanId == baiviet.TaiKhoanId))
            {
                return ErrorMessage.ChuaTonTai;
            }
            baiviet.ThoiGianTao = DateTime.Now;
            dbContext.BaiViet.Add(baiviet);
            dbContext.SaveChanges();
            return ErrorMessage.ThanhCong;
        }

        IQueryable<BaiViet> IBaiVietServices.TimBaiVietTheoTen(string keyword)
        {
            var listBV = dbContext.BaiViet.Where(x => x.TenBaiViet.Contains(keyword)).AsQueryable();
            if(listBV == null)
            {
                return null;
            } return listBV;
        }

        ErrorMessage IBaiVietServices.XoaBaiViet(int id)
        {
            var baivietCanXoa = dbContext.BaiViet.FirstOrDefault(x => x.BaiVietId == id);
            if (baivietCanXoa == null)
            {
                return ErrorMessage.ChuaTonTai;
            }
            else
            {
                dbContext.BaiViet.Remove(baivietCanXoa);
                dbContext.SaveChanges();
                return ErrorMessage.ThanhCong;
            }
        }
    }
}
