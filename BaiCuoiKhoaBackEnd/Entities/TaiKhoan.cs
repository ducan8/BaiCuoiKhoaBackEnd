using System.ComponentModel.DataAnnotations;

namespace BaiCuoiKhoaBackEnd.Entities
{
    public class TaiKhoan
    {
        public int TaiKhoanId { get; set;}
        [MaxLength(50)]
        public string TenNguoiDung { get; set;}
        [MaxLength(50)]
        public string TenDangNhap {  get; set;}
        [MaxLength(50)]
        public string MatKhau { get; set;}
        public int QuyenHanId { get; set;}
        public QuyenHan? QuyenHan { get; set;}

    }
}
