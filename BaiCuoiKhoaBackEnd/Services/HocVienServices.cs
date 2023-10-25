using BaiCuoiKhoaBackEnd.Constants;
using BaiCuoiKhoaBackEnd.Entities;
using BaiCuoiKhoaBackEnd.ISevices;

namespace BaiCuoiKhoaBackEnd.Services
{
    public class HocVienServices : IHocVienServices
    {
        private readonly AppDbContext dbContext;
        public HocVienServices()
        {
            dbContext = new AppDbContext();
        }

        public string DinhDangHoTen(string hoTen)
        {
            string[] hoten = hoTen.Trim().Split(" ");
            string result = "";
            for (int i = 0; i < hoten.Length; i++)
            {
                hoten[i] = hoten[i].Substring(0, 1).ToUpper() + hoten[i].Substring(1).ToLower();
            }
            result = string.Join(" ", hoten);
            return result;
        }

        IQueryable<HocVien> IHocVienServices.HienThiDanhSachHocVien()
        {
            var listHocVien = dbContext.HocVien;
            if (listHocVien == null) return null;
            return listHocVien;
        }

        PageResult<HocVien> IHocVienServices.PhanTrangDuLieu(Pagination pagination, string? keyword)
        {
            var listSearch = dbContext.HocVien.ToList();
            pagination.TotalCount = listSearch.Count();
            if (!string.IsNullOrEmpty(keyword))
            {
                listSearch = listSearch.Where(x => x.SoDienThoai.Contains(keyword) ||
                                                        x.Email.ToLower().Contains(keyword.ToLower()) ||
                                                        x.HoTen.ToLower().Contains(keyword.ToLower())).ToList();
            }
            var res = PageResult<HocVien>.ToPageResult(pagination, listSearch);
            return new PageResult<HocVien>() { Pagination = pagination, Data = res };
        }

        ErrorMessage IHocVienServices.SuaHocVien(HocVien hocvienUpdate)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var hocvienCanSua = dbContext.HocVien.FirstOrDefault(x => x.HocVienId == hocvienUpdate.HocVienId);
                    if (hocvienCanSua == null)
                    {
                        return ErrorMessage.ChuaTonTai;
                    }

                    var listDangKyHocUpdate = hocvienUpdate.DanhSachDangKyHoc;
                    var listDangKyHocHienTai = dbContext.DangKyHoc;
                    if (listDangKyHocUpdate == null)
                    {
                        dbContext.DangKyHoc.RemoveRange(listDangKyHocHienTai);
                        dbContext.SaveChanges();
                    }
                    else
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
                                var listHV = dbContext.HocVien;
                                var listKH = dbContext.KhoaHoc;
                                var listTTH = dbContext.TinhTrangHoc;
                                var listTK = dbContext.TaiKhoan;
                                var dkhocMoi = listDangKyHocUpdate.FirstOrDefault(x => x.DangKyHocId == dkhoc.DangKyHocId);
                                if (!listHV.Any(x => x.HocVienId == dkhocMoi.HocVienId) ||
                                   !listKH.Any(x => x.KhoaHocId == dkhocMoi.KhoaHocId) ||
                                   !listTTH.Any(x => x.TinhTrangHocId == dkhocMoi.TinhTrangHocId) ||
                                   !listTK.Any(x => x.TaiKhoanId == dkhocMoi.TaiKhoanId))
                                {
                                    return ErrorMessage.ChuaTonTai;
                                }
                                dkhoc.KhoaHocId = dkhocMoi.KhoaHocId;
                                dkhoc.HocVienId = dkhocMoi.HocVienId;
                                dkhoc.NgayBatDau = dkhocMoi.NgayBatDau;
                                dkhoc.NgayDangKy = dkhocMoi.NgayDangKy;
                                dkhoc.NgayKetThuc = dkhocMoi.NgayKetThuc;
                                dkhoc.TinhTrangHocId = dkhocMoi.TinhTrangHocId;
                                dkhoc.TaiKhoanId = dkhocMoi.TaiKhoanId;
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
                                dkhoc.HocVienId = hocvienUpdate.HocVienId;
                                dbContext.DangKyHoc.Add(dkhoc);
                                dbContext.SaveChanges();
                            }
                        }
                    }

                    var listHocVien = dbContext.HocVien;
                    if (hocvienCanSua.Email.ToLower().Equals(hocvienUpdate.Email.ToLower()))
                    {
                    }
                    else if (listHocVien.Any(x => x.Email.ToLower().Equals(hocvienUpdate.Email.ToLower())))
                    {
                        return ErrorMessage.DaTonTai;
                    }
                    if (hocvienCanSua.SoDienThoai.Equals(hocvienUpdate.SoDienThoai))
                    {
                    }
                    else if (listHocVien.Any(x => x.SoDienThoai.Equals(hocvienUpdate.SoDienThoai)))
                    {
                        return ErrorMessage.DaTonTai;
                    }
                    hocvienCanSua.Email = hocvienUpdate.Email;
                    hocvienCanSua.SoDienThoai = hocvienUpdate.SoDienThoai;
                    hocvienCanSua.HoTen = DinhDangHoTen(hocvienUpdate.HoTen);
                    hocvienCanSua.HinhAnh = hocvienUpdate.HinhAnh;
                    hocvienCanSua.NgaySinh = hocvienUpdate.NgaySinh;
                    hocvienCanSua.TinhThanh = hocvienUpdate.TinhThanh;
                    hocvienCanSua.QuanHuyen = hocvienUpdate.QuanHuyen;
                    hocvienCanSua.PhuongXa = hocvienUpdate.PhuongXa;
                    hocvienCanSua.SoNha = hocvienUpdate.SoNha;
                    dbContext.HocVien.Update(hocvienCanSua);
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

        ErrorMessage IHocVienServices.ThemHocVien(HocVien hocVien)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var listHocVien = dbContext.HocVien;
                    if (listHocVien.Any(x => x.SoDienThoai.Contains(hocVien.SoDienThoai) || x.Email.ToLower().Contains(hocVien.Email.ToLower())))
                    {
                        return ErrorMessage.DaTonTai;
                    }
                    hocVien.HoTen = DinhDangHoTen(hocVien.HoTen);
                    var listDangKyHoc = hocVien.DanhSachDangKyHoc;
                    hocVien.DanhSachDangKyHoc = null;
                    dbContext.HocVien.Add(hocVien);
                    dbContext.SaveChanges();
                    if (listDangKyHoc != null)
                    {
                        var listHV = dbContext.HocVien;
                        var listKH = dbContext.KhoaHoc;
                        var listTK = dbContext.TaiKhoan;
                        foreach (var dkhoc in listDangKyHoc)
                        {
                            if (!listHV.Any(x => x.HocVienId == dkhoc.HocVienId) ||
                                !listKH.Any(x => x.KhoaHocId == dkhoc.KhoaHocId) ||
                                !listTK.Any(x => x.TaiKhoanId == dkhoc.TaiKhoanId))
                            {
                                dbContext.HocVien.Remove(hocVien);
                                dbContext.SaveChanges();
                                return ErrorMessage.ChuaTonTai;
                            }
                            dkhoc.HocVienId = hocVien.HocVienId;
                        }
                        dbContext.DangKyHoc.AddRange(listDangKyHoc);
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

        IQueryable<HocVien> IHocVienServices.TimKiemTheoTenVaEmail(string keyword)
        {
            var listhocvien = dbContext.HocVien.Where(x => x.Email.ToLower().Contains(keyword.ToLower()) || x.SoDienThoai.Contains(keyword));
            if (listhocvien == null)
            {
                return null;
            }
            else { return listhocvien; }
        }

        ErrorMessage IHocVienServices.XoaHocVien(int id)
        {
            var hocvienCanXoa = dbContext.HocVien.FirstOrDefault(x => x.HocVienId == id);
            if (hocvienCanXoa == null)
            {
                return ErrorMessage.ChuaTonTai;
            }
            else
            {
                dbContext.DangKyHoc.RemoveRange(dbContext.DangKyHoc.Where(x => x.HocVienId == id));
                dbContext.SaveChanges();
                dbContext.HocVien.Remove(hocvienCanXoa);
                dbContext.SaveChanges();
                return ErrorMessage.ThanhCong;
            }
        }
    }
}
