using BaiCuoiKhoaBackEnd.Constants;
using BaiCuoiKhoaBackEnd.Entities;
using BaiCuoiKhoaBackEnd.ISevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace BaiCuoiKhoaBackEnd.Services
{
    public class KhoaHocServices : IKhoaHocServices
    {
        private readonly AppDbContext dbContext;
        public KhoaHocServices()
        {
            dbContext = new AppDbContext();
        }
        IQueryable<KhoaHoc> IKhoaHocServices.HienThiDanhSachKhoaHoc()
        {
            var listKhoaHoc = dbContext.KhoaHoc.Include(x => x.LoaiKhoaHoc).Include(x => x.DanhSachDangKyHoc).AsQueryable();
            if (listKhoaHoc == null)
            {
                return null;
            }
            return listKhoaHoc;
        }

        PageResult<KhoaHoc> IKhoaHocServices.PhanTrangDanhSachKhoaHoc(Pagination pagination)
        {
            var listKhoaHoc = dbContext.KhoaHoc.AsQueryable();
            var result = PageResult<KhoaHoc>.ToPageResult(pagination, listKhoaHoc);
            pagination.TotalCount = listKhoaHoc.Count();
            return new PageResult<KhoaHoc>() { Pagination = pagination, Data = result };
        }

        ErrorMessage IKhoaHocServices.SuaKhoaHoc(KhoaHoc khoahocUpdate)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var khoahocCanSua = dbContext.KhoaHoc.FirstOrDefault(x => x.KhoaHocId == khoahocUpdate.KhoaHocId);
                    if (khoahocCanSua == null)
                    {
                        return ErrorMessage.KhoaHocChuaTonTai;
                    }
                    else
                    {
                        var listDangKyHocUpdate = khoahocUpdate.DanhSachDangKyHoc;
                        var listDangKyHocHienTai = dbContext.DangKyHoc.Where(x => x.KhoaHocId == khoahocUpdate.KhoaHocId).AsQueryable();
                        if (listDangKyHocUpdate == null)
                        {
                            dbContext.DangKyHoc.RemoveRange(listDangKyHocHienTai);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            List<DangKyHoc> listDangKyHocDelete = new List<DangKyHoc>();
                            var listKH = dbContext.KhoaHoc.ToList();
                            var listHV = dbContext.HocVien.ToList();
                            var listTK = dbContext.TaiKhoan.ToList();
                            var listTTH = dbContext.TinhTrangHoc.ToList();

                            foreach (var dkhoc in listDangKyHocHienTai)
                            {
                                if (!listDangKyHocUpdate.Any(x => x.DangKyHocId == dkhoc.DangKyHocId))
                                {
                                    listDangKyHocDelete.Add(dkhoc);
                                }
                                else
                                {
                                    var dkhocMoi = listDangKyHocUpdate.FirstOrDefault(x => x.DangKyHocId == dkhoc.DangKyHocId);

                                    if (!listKH.Any(x => x.KhoaHocId == dkhocMoi.KhoaHocId))
                                    {
                                        return ErrorMessage.KhoaHocChuaTonTai;
                                    }
                                    else
                                    {
                                        dkhoc.KhoaHocId = dkhocMoi.KhoaHocId;
                                    }
                                    if (!listHV.Any(x => x.HocVienId == dkhocMoi.HocVienId))
                                    {
                                        return ErrorMessage.HocVienChuaTonTai;
                                    }
                                    else
                                    {
                                        dkhoc.HocVienId = dkhocMoi.HocVienId;
                                    }
                                    if (!listTK.Any(x => x.TaiKhoanId == dkhocMoi.TaiKhoanId))
                                    {
                                        return ErrorMessage.TaiKhoanChuaTonTai;
                                    }
                                    else
                                    {
                                        dkhoc.TaiKhoanId = dkhocMoi.TaiKhoanId;
                                    }
                                    if (!listTTH.Any(x => x.TinhTrangHocId == dkhocMoi.TinhTrangHocId))
                                    {
                                        return ErrorMessage.TaiKhoanChuaTonTai;
                                    }
                                    else
                                    {
                                        dkhoc.TinhTrangHocId = dkhocMoi.TinhTrangHocId;
                                    }
                                    dkhoc.NgayDangKy = dkhocMoi.NgayDangKy;
                                    dkhoc.NgayKetThuc = dkhocMoi.NgayKetThuc;
                                    dkhoc.NgayBatDau = dkhocMoi.NgayBatDau;
                                }
                            }
                            dbContext.DangKyHoc.RemoveRange(listDangKyHocDelete);
                            dbContext.SaveChanges();
                            dbContext.DangKyHoc.UpdateRange(listDangKyHocHienTai);
                            dbContext.SaveChanges();

                            foreach (var dkhoc in listDangKyHocUpdate)
                            {
                                if (!listDangKyHocHienTai.Any(x => x.DangKyHocId == dkhoc.DangKyHocId))
                                {
                                    if (!listHV.Any(x => x.HocVienId == dkhoc.HocVienId) || !listTK.Any(x => x.TaiKhoanId == dkhoc.TaiKhoanId) || !listKH.Any(x => x.KhoaHocId == dkhoc.KhoaHocId) || !listTTH.Any(x => x.TinhTrangHocId == dkhoc.TinhTrangHocId))
                                    {
                                        return ErrorMessage.ChuaTonTai;
                                    }
                                    dkhoc.KhoaHocId = khoahocUpdate.KhoaHocId;
                                    dbContext.DangKyHoc.Add(dkhoc);
                                    dbContext.SaveChanges();
                                }

                            }
                        }
                    }
                    khoahocCanSua.TenKhoaHoc = khoahocUpdate.TenKhoaHoc;
                    khoahocCanSua.ThoiGianHoc = khoahocUpdate.ThoiGianHoc;
                    khoahocCanSua.GioiThieu = khoahocUpdate.GioiThieu;
                    khoahocCanSua.NoiDung = khoahocUpdate.NoiDung;
                    khoahocCanSua.HocPhi = khoahocUpdate.HocPhi;
                    khoahocCanSua.SoHocVien = dbContext.DangKyHoc.Where(x => x.KhoaHocId == khoahocUpdate.KhoaHocId).Count(x => x.TinhTrangHocId == 2 || x.TinhTrangHocId == 3 || x.TinhTrangHocId == 4);
                    khoahocCanSua.SoLuongMon = khoahocUpdate.SoLuongMon;
                    khoahocCanSua.HinhAnh = khoahocUpdate.HinhAnh;
                    if (!dbContext.LoaiKhoaHoc.Any(x => x.LoaiKhoaHocId == khoahocUpdate.LoaiKhoaHocId))
                    {
                        return ErrorMessage.LoaiKhoaHocChuaTonTai;
                    }
                    khoahocCanSua.LoaiKhoaHocId = khoahocUpdate.LoaiKhoaHocId;

                    dbContext.KhoaHoc.Update(khoahocCanSua);
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

        ErrorMessage IKhoaHocServices.ThemKhoaHoc(KhoaHoc khoahoc)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var loaiKhoaHoc = dbContext.LoaiKhoaHoc.FirstOrDefault(x => x.LoaiKhoaHocId == khoahoc.LoaiKhoaHocId);
                    if (loaiKhoaHoc == null)
                    {
                        return ErrorMessage.ChuaTonTai;
                    }
                    dbContext.KhoaHoc.Add(khoahoc);
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

        IQueryable<KhoaHoc> IKhoaHocServices.TimKhoaHocTheoTen(string keyword)
        {
            var listKhoahocCanTim = dbContext.KhoaHoc.Where(x => x.TenKhoaHoc.ToLower().Contains(keyword.ToLower())).AsQueryable();
            if(listKhoahocCanTim == null)
            {
                return null;
            } return listKhoahocCanTim;
        }

        ErrorMessage IKhoaHocServices.XoaKhoaHoc(int id)
        {
            var khoahocCanXoa = dbContext.KhoaHoc.FirstOrDefault(x => x.KhoaHocId == id);
            if(khoahocCanXoa == null)
            {
                return ErrorMessage.KhoaHocChuaTonTai;
            }
            dbContext.DangKyHoc.RemoveRange(dbContext.DangKyHoc.Where(x => x.KhoaHocId == id));
            dbContext.SaveChanges() ;
            dbContext.KhoaHoc.Remove(khoahocCanXoa);
            dbContext.SaveChanges();
            return ErrorMessage.ThanhCong;
        }
    }
}
