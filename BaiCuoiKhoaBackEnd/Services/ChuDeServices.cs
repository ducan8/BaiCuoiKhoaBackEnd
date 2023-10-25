using BaiCuoiKhoaBackEnd.Constants;
using BaiCuoiKhoaBackEnd.Entities;
using BaiCuoiKhoaBackEnd.ISevices;
using Microsoft.EntityFrameworkCore;

namespace BaiCuoiKhoaBackEnd.Services
{
    public class ChuDeServices : IChuDeServices
    {
        private readonly AppDbContext dbContext;
        public ChuDeServices()
        {
            dbContext = new AppDbContext();
        }
        IQueryable<ChuDe> IChuDeServices.HienThiDanhSachChuDe()
        {
            var listChuDe = dbContext.ChuDe;
            if (listChuDe == null)
            {
                return null;
            }
            return listChuDe;
        }

        PageResult<ChuDe> IChuDeServices.PhanTrangDanhSachChuDe(Pagination pagination)
        {
            var listChuDe = dbContext.ChuDe;
            pagination.TotalCount = listChuDe.Count();
            var res = PageResult<ChuDe>.ToPageResult(pagination, listChuDe);
            return new PageResult<ChuDe>() { Pagination = pagination, Data = res };
        }

        ErrorMessage IChuDeServices.SuaChuDe(ChuDe chudeUpdate)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var chudeCanSua = dbContext.ChuDe.Include(x => x.DanhSachBaiViet).FirstOrDefault(x => x.ChuDeId == chudeUpdate.ChuDeId);
                    var listBaiVietUpdate = chudeUpdate.DanhSachBaiViet;
                    var listBaiVietHienTai = chudeCanSua.DanhSachBaiViet;

                    if (chudeCanSua == null)
                    {
                        return ErrorMessage.ChuaTonTai;
                    }
                    if (!dbContext.LoaiBaiViet.Any(x => x.LoaiBaiVietId == chudeUpdate.LoaiBaiVietId))
                    {
                        return ErrorMessage.ChuaTonTai;
                    }
                    if (listBaiVietUpdate == null || listBaiVietUpdate.Count() == 0)
                    {
                        dbContext.BaiViet.RemoveRange(listBaiVietHienTai);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        if (listBaiVietHienTai != null)
                        {
                            var listBaiVietDelete = new List<BaiViet>();
                            var listTaiKhoan = dbContext.TaiKhoan;

                            foreach (var baiviet in listBaiVietHienTai)
                            {
                                if (!listBaiVietUpdate.Any(x => x.BaiVietId == baiviet.BaiVietId))
                                {
                                    listBaiVietDelete.Add(baiviet);
                                }
                                else
                                {
                                    if (!listTaiKhoan.Any(x => x.TaiKhoanId == baiviet.TaiKhoanId))
                                    {
                                        return ErrorMessage.ChuaTonTai;
                                    }
                                    var baivietMoi = listBaiVietUpdate.FirstOrDefault(x => x.BaiVietId == baiviet.BaiVietId);
                                    baiviet.TenBaiViet = baivietMoi.TenBaiViet;
                                    baiviet.ThoiGianTao = baivietMoi.ThoiGianTao;
                                    baiviet.TenTacGia = baivietMoi.TenTacGia;
                                    baiviet.NoiDung = baivietMoi.NoiDung;
                                    baiviet.NoiDungNgan = baivietMoi.NoiDungNgan;
                                    baiviet.HinhAnh = baivietMoi.HinhAnh;
                                    baiviet.ChuDeId = chudeUpdate.ChuDeId;
                                    baiviet.TaiKhoanId = baiviet.TaiKhoanId;
                                }
                            }
                            dbContext.BaiViet.RemoveRange(listBaiVietDelete);
                            dbContext.SaveChanges();
                            dbContext.BaiViet.UpdateRange(listBaiVietHienTai);
                            dbContext.SaveChanges();
                            foreach (var baiviet in listBaiVietUpdate)
                            {
                                if (!listBaiVietHienTai.Any(x => x.BaiVietId == baiviet.BaiVietId))
                                {
                                    if (!listTaiKhoan.Any(x => x.TaiKhoanId == baiviet.TaiKhoanId))
                                    {
                                        return ErrorMessage.ChuaTonTai;
                                    }
                                    baiviet.ChuDeId = chudeUpdate.ChuDeId;
                                    dbContext.BaiViet.Add(baiviet);
                                    dbContext.SaveChanges();
                                }
                            }
                        }
                    }

                    chudeCanSua.TenChuDe = chudeUpdate.TenChuDe;
                    chudeCanSua.NoiDung = chudeUpdate.NoiDung;
                    dbContext.ChuDe.Update(chudeCanSua);
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

        ErrorMessage IChuDeServices.ThemChuDe(ChuDe chude)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (!dbContext.LoaiBaiViet.Any(x => x.LoaiBaiVietId == chude.LoaiBaiVietId))
                    {
                        return ErrorMessage.ChuaTonTai;
                    }
                    var listBaiViet = chude.DanhSachBaiViet;
                    chude.DanhSachBaiViet = null;
                    dbContext.ChuDe.Add(chude);
                    dbContext.SaveChanges();

                    foreach (var baiviet in listBaiViet)
                    {
                        if (!dbContext.TaiKhoan.Any(x => x.TaiKhoanId == baiviet.TaiKhoanId))
                        {
                            dbContext.ChuDe.Remove(chude);
                            dbContext.SaveChanges();
                            return ErrorMessage.ChuaTonTai;
                        }
                        else
                        {
                            baiviet.ChuDeId = chude.ChuDeId;
                        }
                    }
                    dbContext.BaiViet.AddRange(listBaiViet);
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

        ErrorMessage IChuDeServices.XoaChuDe(int id)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var chuDeCanXoa = dbContext.ChuDe.FirstOrDefault(x => x.ChuDeId == id);
                    if (chuDeCanXoa == null)
                    {
                        return ErrorMessage.ChuaTonTai;
                    }
                    var listBV = dbContext.BaiViet.Where(x => x.ChuDeId == id);
                    dbContext.BaiViet.RemoveRange(listBV);
                    dbContext.SaveChanges();
                    dbContext.ChuDe.Remove(chuDeCanXoa);
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
