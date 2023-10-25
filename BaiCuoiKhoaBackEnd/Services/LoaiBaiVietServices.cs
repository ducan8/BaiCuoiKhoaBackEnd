using BaiCuoiKhoaBackEnd.Constants;
using BaiCuoiKhoaBackEnd.Entities;
using BaiCuoiKhoaBackEnd.ISevices;

namespace BaiCuoiKhoaBackEnd.Services
{
    public class LoaiBaiVietServices : ILoaiBaiVietServices
    {
        private readonly AppDbContext dbContext;
        public LoaiBaiVietServices()
        {
            dbContext = new AppDbContext();
        }
        IQueryable<LoaiBaiViet> ILoaiBaiVietServices.HienThiDanhSachLoaiBaiViet()
        {
            var listLoaiBaiViet = dbContext.LoaiBaiViet.AsQueryable();
            if (listLoaiBaiViet == null)
            {
                return null;
            }
            return listLoaiBaiViet;
        }

        PageResult<LoaiBaiViet> ILoaiBaiVietServices.PhanTrangDanhSachLoaiBaiViet(Pagination pagination)
        {
            var listLoaiBaiViet = dbContext.LoaiBaiViet;
            pagination.TotalCount = listLoaiBaiViet.Count();
            var res = PageResult<LoaiBaiViet>.ToPageResult(pagination, listLoaiBaiViet);
            return new PageResult<LoaiBaiViet>() { Data = res, Pagination = pagination };
        }

        ErrorMessage ILoaiBaiVietServices.SuaLoaiBaiViet(LoaiBaiViet loaiBaiVietUpdate)
        {
            var loaiBV = dbContext.LoaiBaiViet.FirstOrDefault(x => x.LoaiBaiVietId == loaiBaiVietUpdate.LoaiBaiVietId);
            if (loaiBV == null)
            {
                return ErrorMessage.ChuaTonTai;
            }

            var listChuDeUpdate = loaiBaiVietUpdate.DanhSachChuDe;
            var listChuDeHienTai = dbContext.ChuDe.Where(x => x.LoaiBaiVietId == loaiBaiVietUpdate.LoaiBaiVietId);
            if (listChuDeUpdate == null)
            {
                dbContext.ChuDe.RemoveRange(listChuDeHienTai);
                dbContext.SaveChanges();

            }
            else
            {
                if (listChuDeHienTai != null)
                {

                    var listChuDeDelete = new List<ChuDe>();
                    foreach (var chude in listChuDeHienTai)
                    {
                        if (!listChuDeUpdate.Any(x => x.ChuDeId == chude.ChuDeId))
                        {
                            listChuDeDelete.Add(chude);
                        }
                        else
                        {
                            var chudeMoi = listChuDeUpdate.FirstOrDefault(x => x.ChuDeId == chude.ChuDeId);
                            chude.TenChuDe = chudeMoi.TenChuDe;
                            chude.NoiDung = chudeMoi.NoiDung;
                            chude.LoaiBaiVietId = loaiBaiVietUpdate.LoaiBaiVietId;
                        }
                    }
                    dbContext.ChuDe.RemoveRange(listChuDeDelete);
                    dbContext.SaveChanges();
                    dbContext.ChuDe.UpdateRange(listChuDeHienTai);
                    dbContext.SaveChanges();

                    foreach (var chude in listChuDeUpdate)
                    {
                        if (!listChuDeHienTai.Any(x => x.ChuDeId == chude.ChuDeId))
                        {
                            chude.LoaiBaiVietId = loaiBaiVietUpdate.LoaiBaiVietId;
                            dbContext.ChuDe.Add(chude);
                            dbContext.SaveChanges();
                        }
                    }
                }
                else
                {
                    foreach (var chude in listChuDeUpdate)
                    {
                        chude.LoaiBaiVietId = loaiBaiVietUpdate.LoaiBaiVietId;
                        dbContext.ChuDe.Add(chude);
                        dbContext.SaveChanges();
                    }
                }
            }

            loaiBV.TenLoai = loaiBaiVietUpdate.TenLoai;
            dbContext.LoaiBaiViet.Update(loaiBV);
            dbContext.SaveChanges();

            return ErrorMessage.ThanhCong;
        }

        ErrorMessage ILoaiBaiVietServices.ThemLoaiBaiViet(LoaiBaiViet loaiBaiViet)
        {
            dbContext.LoaiBaiViet.Add(loaiBaiViet);
            dbContext.SaveChanges();
            return ErrorMessage.ThanhCong;
        }

        ErrorMessage ILoaiBaiVietServices.XoaLoaiBaiViet(int id)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var loaiBV = dbContext.LoaiBaiViet.FirstOrDefault(x => x.LoaiBaiVietId == id);
                    if (loaiBV == null)
                    {
                        return ErrorMessage.ChuaTonTai;
                    }
                    var listCD = dbContext.ChuDe.Where(x => x.LoaiBaiVietId == id);
                    dbContext.ChuDe.RemoveRange(listCD);
                    dbContext.SaveChanges();
                    dbContext.LoaiBaiViet.Remove(loaiBV);
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
