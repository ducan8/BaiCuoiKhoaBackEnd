using BaiCuoiKhoaBackEnd.Constants;
using BaiCuoiKhoaBackEnd.Entities;
using BaiCuoiKhoaBackEnd.ISevices;

namespace BaiCuoiKhoaBackEnd.Services
{
    public class LoaiKhoaHocServices : ILoaiKhoaHocServices
    {
        private readonly AppDbContext dbContext;
        public LoaiKhoaHocServices()
        {
            dbContext = new AppDbContext();
        }

        IQueryable<LoaiKhoaHoc> ILoaiKhoaHocServices.GetDSLoaiKH()
        {
            var listLoaiKH = dbContext.LoaiKhoaHoc.AsQueryable();
            if(listLoaiKH == null)
            {
                return null;
            }
            return listLoaiKH;
        }

        ErrorMessage ILoaiKhoaHocServices.SuaLoaiKhoaHoc(LoaiKhoaHoc loaiKhoaHocUpdate)
        {
            var loaiKH = dbContext.LoaiKhoaHoc.FirstOrDefault(x => x.LoaiKhoaHocId == loaiKhoaHocUpdate.LoaiKhoaHocId);
            if (loaiKH == null)
            {
                return ErrorMessage.ChuaTonTai;
            }

            var listKhoaHocUpdate = loaiKhoaHocUpdate.DanhSachKhoaHoc;
            var listKhoaHocHienTai = dbContext.KhoaHoc.Where(x => x.LoaiKhoaHocId == loaiKhoaHocUpdate.LoaiKhoaHocId);
            if (listKhoaHocUpdate == null)
            {
                dbContext.KhoaHoc.RemoveRange(listKhoaHocHienTai);
                dbContext.SaveChanges();

            }
            else
            {
                if (listKhoaHocHienTai != null)
                {
                    var listKhoaHocDelete = new List<KhoaHoc>();
                    foreach (var khoahoc in listKhoaHocHienTai)
                    {
                        if (!listKhoaHocUpdate.Any(x => x.KhoaHocId == khoahoc.KhoaHocId))
                        {
                            listKhoaHocDelete.Add(khoahoc);
                        }
                        else
                        {
                            var khoahocMoi = listKhoaHocUpdate.FirstOrDefault(x => x.KhoaHocId == khoahoc.KhoaHocId);
                            khoahoc.TenKhoaHoc = khoahocMoi.TenKhoaHoc;
                            khoahoc.ThoiGianHoc = khoahocMoi.ThoiGianHoc;
                            khoahoc.GioiThieu = khoahocMoi.GioiThieu;
                            khoahoc.NoiDung = khoahocMoi.NoiDung;
                            khoahoc.HocPhi = khoahocMoi.HocPhi;
                            khoahoc.SoHocVien = khoahocMoi.SoHocVien;
                            khoahoc.SoLuongMon = khoahocMoi.SoLuongMon;
                            khoahoc.HinhAnh = khoahocMoi.HinhAnh;
                            khoahoc.LoaiKhoaHocId = loaiKhoaHocUpdate.LoaiKhoaHocId;
                        }
                    }
                    dbContext.KhoaHoc.RemoveRange(listKhoaHocDelete);
                    dbContext.SaveChanges();
                    dbContext.KhoaHoc.UpdateRange(listKhoaHocHienTai);
                    dbContext.SaveChanges();

                    foreach (var khoahoc in listKhoaHocUpdate)
                    {
                        if (!listKhoaHocHienTai.Any(x => x.KhoaHocId == khoahoc.KhoaHocId))
                        {
                            khoahoc.LoaiKhoaHocId = loaiKhoaHocUpdate.LoaiKhoaHocId;
                            dbContext.KhoaHoc.Add(khoahoc);
                            dbContext.SaveChanges();
                        }
                    }
                }
                else
                {
                    foreach (var khoahoc in listKhoaHocUpdate)
                    {
                        khoahoc.LoaiKhoaHocId = loaiKhoaHocUpdate.LoaiKhoaHocId;
                        dbContext.KhoaHoc.Add(khoahoc);
                        dbContext.SaveChanges();
                    }
                }
            }

            loaiKH.TenLoai = loaiKhoaHocUpdate.TenLoai;
            dbContext.LoaiKhoaHoc.Update(loaiKH);
            dbContext.SaveChanges();

            return ErrorMessage.ThanhCong;
        }

        ErrorMessage ILoaiKhoaHocServices.ThemLoaiKhoaHoc(LoaiKhoaHoc loaiKhoaHoc)
        {
            dbContext.LoaiKhoaHoc.Add(loaiKhoaHoc);
            dbContext.SaveChanges();
            return ErrorMessage.ThanhCong;
        }

        ErrorMessage ILoaiKhoaHocServices.XoaLoaiKhoaHoc(int id)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var loaiKH = dbContext.LoaiKhoaHoc.FirstOrDefault(x => x.LoaiKhoaHocId == id);
                    if (loaiKH == null)
                    {
                        return ErrorMessage.ChuaTonTai;
                    }
                    var listKH = dbContext.KhoaHoc.Where(x => x.LoaiKhoaHocId == id);
                    dbContext.KhoaHoc.RemoveRange(listKH);
                    dbContext.SaveChanges();
                    dbContext.LoaiKhoaHoc.Remove(loaiKH);
                    dbContext.SaveChanges();
                    trans.Commit();
                    return ErrorMessage.ThanhCong;
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return ErrorMessage.ThatBai;
                }
            }
        }
    }
}
