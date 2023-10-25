using BaiCuoiKhoaBackEnd.Constants;
using BaiCuoiKhoaBackEnd.Entities;
using BaiCuoiKhoaBackEnd.ISevices;
using Microsoft.EntityFrameworkCore;

namespace BaiCuoiKhoaBackEnd.Services
{
    public class TinhTrangHocServices : ITinhTrangHocServies
    {
        private readonly AppDbContext dbContext;
        public TinhTrangHocServices()
        {
            dbContext = new AppDbContext();
        }
        IQueryable<TinhTrangHoc> ITinhTrangHocServies.HienThiDanhSachTinhTrangHoc()
        {
            var listTTH = dbContext.TinhTrangHoc.AsQueryable();
            if (listTTH == null)
            {
                return null;
            }
            return listTTH;
        }

        ErrorMessage ITinhTrangHocServies.SuaTinhTrangHoc(TinhTrangHoc tinhTrangHocUpdate)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var tinhtranghocCanSua = dbContext.TinhTrangHoc.Include(x => x.DanhSachDangKyHoc).FirstOrDefault(x => x.TinhTrangHocId == tinhTrangHocUpdate.TinhTrangHocId);
                    if (tinhtranghocCanSua == null)
                    {
                        return ErrorMessage.ChuaTonTai;
                    }
                    else
                    {
                        var listDangKyHocUpdate = tinhTrangHocUpdate.DanhSachDangKyHoc;
                        var listDangKyHocHienTai = tinhtranghocCanSua.DanhSachDangKyHoc;
                        if (tinhTrangHocUpdate.DanhSachDangKyHoc == null)
                        {
                            dbContext.DangKyHoc.RemoveRange(listDangKyHocHienTai);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            if (listDangKyHocHienTai != null || listDangKyHocHienTai.Count() == 0)
                            {
                                var listDangKyHocDelete = new List<DangKyHoc>();
                                foreach (var dkhoc in listDangKyHocHienTai)
                                {
                                    if (!listDangKyHocUpdate.Any(x => x.DangKyHocId == dkhoc.DangKyHocId))
                                    {
                                        listDangKyHocDelete.Add(dkhoc);
                                    }
                                    else
                                    {
                                        dkhoc.TinhTrangHocId = tinhTrangHocUpdate.TinhTrangHocId;
                                    }
                                }
                                dbContext.RemoveRange(listDangKyHocDelete);
                                dbContext.SaveChanges();
                                dbContext.DangKyHoc.UpdateRange(listDangKyHocHienTai);
                                dbContext.SaveChanges();
                                foreach (var item in listDangKyHocUpdate)
                                {
                                    if (!listDangKyHocHienTai.Any(x => x.DangKyHocId == item.DangKyHocId)){

                                        item.TinhTrangHocId = tinhTrangHocUpdate.TinhTrangHocId;
                                        dbContext.DangKyHoc.Add(item);
                                        dbContext.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                foreach (var item in listDangKyHocUpdate)
                                {
                                    item.TinhTrangHocId = tinhTrangHocUpdate.TinhTrangHocId;
                                    dbContext.DangKyHoc.Add(item);
                                    dbContext.SaveChanges();
                                }
                            }
                        }
                        tinhtranghocCanSua.TenTinhTrang = tinhTrangHocUpdate.TenTinhTrang;
                        dbContext.TinhTrangHoc.Update(tinhtranghocCanSua);
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

        ErrorMessage ITinhTrangHocServies.ThemTinhTrangHoc(TinhTrangHoc tinhTrangHoc)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var danhSachDKhoc = tinhTrangHoc.DanhSachDangKyHoc;
                    tinhTrangHoc.DanhSachDangKyHoc = null;

                    dbContext.TinhTrangHoc.Add(tinhTrangHoc);
                    dbContext.SaveChanges();

                    if (danhSachDKhoc != null)
                    {
                        foreach (var dangKyHoc in danhSachDKhoc)
                        {
                            if (!dbContext.HocVien.Any(x => x.HocVienId == dangKyHoc.HocVienId) ||
                                !dbContext.TaiKhoan.Any(x => x.TaiKhoanId == dangKyHoc.TaiKhoanId) ||
                                !dbContext.KhoaHoc.Any(x => x.KhoaHocId == dangKyHoc.KhoaHocId))
                            {
                                dbContext.TinhTrangHoc.Remove(tinhTrangHoc);
                                dbContext.SaveChanges();
                                return ErrorMessage.ChuaTonTai;
                            }
                            dangKyHoc.TinhTrangHocId = tinhTrangHoc.TinhTrangHocId;
                        }
                        dbContext.DangKyHoc.AddRange(danhSachDKhoc);
                        dbContext.SaveChanges();
                    }
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

        ErrorMessage ITinhTrangHocServies.XoaTinhTrangHoc(int id)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var tinhTrangHoc = dbContext.TinhTrangHoc.FirstOrDefault(x => x.TinhTrangHocId == id);
                    if (tinhTrangHoc == null)
                    {
                        return ErrorMessage.ChuaTonTai;
                    }
                    var listDKHOC = dbContext.DangKyHoc.Where(x => x.TinhTrangHocId == id);
                    dbContext.DangKyHoc.RemoveRange(listDKHOC);
                    dbContext.SaveChanges();
                    dbContext.TinhTrangHoc.Remove(tinhTrangHoc);
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
