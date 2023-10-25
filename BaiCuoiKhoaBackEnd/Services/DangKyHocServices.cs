using BaiCuoiKhoaBackEnd.Constants;
using BaiCuoiKhoaBackEnd.Entities;
using BaiCuoiKhoaBackEnd.ISevices;

namespace BaiCuoiKhoaBackEnd.Services
{
    public class DangKyHocServices : IDangKyHocServices
    {
        private readonly AppDbContext dbContext;
        public DangKyHocServices()
        {
            dbContext = new AppDbContext();
        }
        IQueryable<DangKyHoc> IDangKyHocServices.HienThiDanhSachDangKyHoc()
        {
            var listDKHoc = dbContext.DangKyHoc;
            if (listDKHoc == null)
            {
                return null;
            }
            return listDKHoc;
        }

        PageResult<DangKyHoc> IDangKyHocServices.PhanTrangDuLieu(Pagination pagination)
        {
            var listDKhoc = dbContext.DangKyHoc;
            pagination.TotalCount = listDKhoc.Count();
            var res = PageResult<DangKyHoc>.ToPageResult(pagination, listDKhoc);
            return new PageResult<DangKyHoc>() { Data = res, Pagination = pagination };
        }

        ErrorMessage IDangKyHocServices.SuaDangKyHoc(DangKyHoc dangKyHocUpdate)
        {
            var dangKyHocCanSua = dbContext.DangKyHoc.FirstOrDefault(x => x.DangKyHocId == dangKyHocUpdate.DangKyHocId);
            if (dangKyHocCanSua == null)
            {
                return ErrorMessage.ChuaTonTai;
            }
            else
            {
                if (!dbContext.KhoaHoc.Any(x => x.KhoaHocId == dangKyHocUpdate.KhoaHocId) ||
               !dbContext.HocVien.Any(x => x.HocVienId == dangKyHocUpdate.HocVienId) ||
               !dbContext.TinhTrangHoc.Any(x => x.TinhTrangHocId == dangKyHocUpdate.TinhTrangHocId) ||
               !dbContext.TaiKhoan.Any(x => x.TaiKhoanId == dangKyHocUpdate.TaiKhoanId))
                {
                    return ErrorMessage.ChuaTonTai;
                }
                
                var tinhTrangHoc = dbContext.TinhTrangHoc.FirstOrDefault(x => x.TinhTrangHocId == dangKyHocUpdate.TinhTrangHocId);
                if (tinhTrangHoc.TenTinhTrang.Contains("Cho duyet"))
                {
                    dangKyHocCanSua.NgayBatDau = new DateTime();
                    dangKyHocCanSua.NgayKetThuc = new DateTime();
                    dangKyHocCanSua.NgayDangKy = new DateTime();
                } else if(tinhTrangHoc.TenTinhTrang.Contains("Dang hoc") && dangKyHocUpdate.TinhTrangHocId != dangKyHocCanSua.TinhTrangHocId)
                {
                    dangKyHocCanSua.NgayDangKy = DateTime.Now;
                    dangKyHocCanSua.NgayBatDau = DateTime.Now;
                    dangKyHocCanSua.NgayKetThuc = dangKyHocCanSua.NgayBatDau.AddMonths(dbContext.KhoaHoc.FirstOrDefault(x => x.KhoaHocId == dangKyHocUpdate.KhoaHocId).ThoiGianHoc);
                }
                else if(tinhTrangHoc.TenTinhTrang.Contains("Hoc xong") && dangKyHocUpdate.TinhTrangHocId != dangKyHocCanSua.TinhTrangHocId)
                {
                    dangKyHocCanSua.NgayBatDau = DateTime.Now;
                    dangKyHocCanSua.NgayKetThuc = DateTime.Now;
                    dangKyHocCanSua.NgayDangKy = DateTime.Now;
                } else if(tinhTrangHoc.TenTinhTrang.Contains("Chua hoan thanh") && dangKyHocUpdate.TinhTrangHocId != dangKyHocCanSua.TinhTrangHocId)
                {
                    dangKyHocCanSua.NgayBatDau = new DateTime();
                    dangKyHocCanSua.NgayKetThuc = new DateTime();
                    dangKyHocCanSua.NgayDangKy = new DateTime();
                }
                dangKyHocCanSua.KhoaHocId = dangKyHocUpdate.KhoaHocId;
                dangKyHocCanSua.HocVienId = dangKyHocUpdate.HocVienId;
                dangKyHocCanSua.TinhTrangHocId = dangKyHocUpdate.TinhTrangHocId;
                dangKyHocCanSua.TaiKhoanId = dangKyHocUpdate.TaiKhoanId;

                dbContext.DangKyHoc.Update(dangKyHocCanSua);
                dbContext.SaveChanges();
                return ErrorMessage.ThanhCong;
            }
        }

        ErrorMessage IDangKyHocServices.ThemDangKyHoc(DangKyHoc dangKyHoc)
        {
            if (!dbContext.KhoaHoc.Any(x => x.KhoaHocId == dangKyHoc.KhoaHocId) ||
               !dbContext.HocVien.Any(x => x.HocVienId == dangKyHoc.HocVienId) ||
               !dbContext.TinhTrangHoc.Any(x => x.TinhTrangHocId == dangKyHoc.TinhTrangHocId) ||
               !dbContext.TaiKhoan.Any(x => x.TaiKhoanId == dangKyHoc.TaiKhoanId))
            {
                return ErrorMessage.ChuaTonTai;
            }
            dangKyHoc.NgayDangKy = DateTime.Now;
            var tinhTrangDangHoc = dbContext.TinhTrangHoc.FirstOrDefault(x => x.TenTinhTrang.Contains("Dang hoc"));
            if (tinhTrangDangHoc.TinhTrangHocId == dangKyHoc.TinhTrangHocId)
            {
                dangKyHoc.NgayBatDau = DateTime.Now;
                dangKyHoc.NgayKetThuc = dangKyHoc.NgayBatDau.AddMonths(dbContext.KhoaHoc.FirstOrDefault(x => x.KhoaHocId == dangKyHoc.KhoaHocId).ThoiGianHoc);
            }

            dbContext.DangKyHoc.Add(dangKyHoc);
            dbContext.SaveChanges();
            return ErrorMessage.ThanhCong;
        }

        ErrorMessage IDangKyHocServices.XoaDangKyHoc(int id)
        {
            var dangKyHoc = dbContext.DangKyHoc.FirstOrDefault(x => x.DangKyHocId == id);
            if (dangKyHoc == null)
            {
                return ErrorMessage.ChuaTonTai;
            }
            dbContext.Remove(dangKyHoc);
            dbContext.SaveChanges();
            return ErrorMessage.ThanhCong;
        }
    }
}
