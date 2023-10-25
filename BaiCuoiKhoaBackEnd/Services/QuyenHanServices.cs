using BaiCuoiKhoaBackEnd.Constants;
using BaiCuoiKhoaBackEnd.Entities;
using BaiCuoiKhoaBackEnd.ISevices;
using Microsoft.EntityFrameworkCore;

namespace BaiCuoiKhoaBackEnd.Services
{
    public class QuyenHanServices : IQuyenHanServices
    {
        private readonly AppDbContext dbContext;
        public QuyenHanServices()
        {
            dbContext = new AppDbContext();
        }
        IQueryable<QuyenHan> IQuyenHanServices.HienThiDanhSachQuyenHan()
        {
            var listQH = dbContext.QuyenHan.AsQueryable();
            if (listQH == null)
            {
                return null;
            }
            else { return listQH; }
        }

        PageResult<QuyenHan> IQuyenHanServices.PhanTrangHienThiQuyenHan(Pagination pagination)
        {
            var listQuyenHan = dbContext.QuyenHan.ToList();
            pagination.TotalCount = listQuyenHan.Count();
            var res = PageResult<QuyenHan>.ToPageResult(pagination,listQuyenHan);
            return new PageResult<QuyenHan>() { Pagination = pagination, Data = res };
        }

        ErrorMessage IQuyenHanServices.SuaQuyenHan(QuyenHan quyenHanUpdate)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var quyenHanCanSua = dbContext.QuyenHan.Include(x => x.DanhSachTaiKhoan).FirstOrDefault(x => x.QuyenHanId == quyenHanUpdate.QuyenHanId);
                    if (quyenHanCanSua == null)
                    {
                        return ErrorMessage.ChuaTonTai;
                    }
                    else
                    {
                        if (quyenHanUpdate.DanhSachTaiKhoan == null)
                        {
                            dbContext.RemoveRange(quyenHanCanSua.DanhSachTaiKhoan);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            var listTaiKhoanUpdate = quyenHanUpdate.DanhSachTaiKhoan;
                            var listTaiKhoanHienTai = quyenHanCanSua.DanhSachTaiKhoan;
                            var listTaiKhoanDelete = new List<TaiKhoan>();

                            if (listTaiKhoanHienTai != null)
                            {
                                foreach (var taikhoan in listTaiKhoanHienTai)
                                {
                                    if (!listTaiKhoanUpdate.Any(x => x.TaiKhoanId == taikhoan.TaiKhoanId))
                                    {
                                        listTaiKhoanDelete.Add(taikhoan);
                                    }
                                    else
                                    {
                                        var taikhoanMoi = listTaiKhoanUpdate.FirstOrDefault(x => x.TaiKhoanId == taikhoan.TaiKhoanId);
                                        taikhoan.QuyenHanId = quyenHanUpdate.QuyenHanId;
                                        taikhoan.TenNguoiDung = taikhoanMoi.TenNguoiDung;
                                        taikhoan.TenDangNhap = taikhoanMoi.TenDangNhap;
                                        taikhoan.MatKhau = taikhoanMoi.MatKhau;
                                    }
                                }
                                dbContext.TaiKhoan.RemoveRange(listTaiKhoanDelete);
                                dbContext.SaveChanges();
                                dbContext.TaiKhoan.UpdateRange(listTaiKhoanHienTai);
                                dbContext.SaveChanges();
                                foreach (var taikhoan in listTaiKhoanUpdate)
                                {
                                    if (!listTaiKhoanHienTai.Any(x => x.TaiKhoanId == taikhoan.TaiKhoanId))
                                    {
                                        taikhoan.QuyenHanId = quyenHanUpdate.QuyenHanId;
                                        dbContext.TaiKhoan.Add(taikhoan);
                                        dbContext.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                foreach (var taikhoan in listTaiKhoanUpdate)
                                {
                                    taikhoan.QuyenHanId = quyenHanUpdate.QuyenHanId;
                                    dbContext.TaiKhoan.Add(taikhoan);
                                    dbContext.SaveChanges();
                                }
                            }
                        }
                        quyenHanCanSua.TenQuyenHan = quyenHanUpdate.TenQuyenHan;
                        dbContext.QuyenHan.Update(quyenHanCanSua);
                        dbContext.SaveChanges();
                        trans.Commit();
                        return ErrorMessage.ThanhCong;
                    }
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return ErrorMessage.ThatBai;
                }
            }
        }

        ErrorMessage IQuyenHanServices.ThemQuyenHan(QuyenHan quyenHan)
        {
            dbContext.QuyenHan.Add(quyenHan);
            dbContext.SaveChanges();
            return ErrorMessage.ThanhCong;
        }

        ErrorMessage IQuyenHanServices.XoaQuyenHan(int id)
        {
            var quyenHanCanXoa = dbContext.QuyenHan.Include(x => x.DanhSachTaiKhoan).FirstOrDefault(x => x.QuyenHanId == id);
            if (quyenHanCanXoa == null)
            {
                return ErrorMessage.ChuaTonTai;
            } else
            {
                dbContext.RemoveRange(quyenHanCanXoa.DanhSachTaiKhoan);
                dbContext.SaveChanges();
                dbContext.Remove(quyenHanCanXoa);
                dbContext.SaveChanges();
                return ErrorMessage.ThanhCong;
            }
        }
    }
}
