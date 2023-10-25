using System.ComponentModel.DataAnnotations;

namespace BaiCuoiKhoaBackEnd.Entities
{
    public class BaiViet
    {
        public int BaiVietId { get; set; }
        [MaxLength(50)]
        public string TenBaiViet {  get; set; }
        
        public DateTime ThoiGianTao { get; set; }
        [MaxLength(50)]
        public string TenTacGia { get; set; }
        public string NoiDung {  get; set; }
        [MaxLength(1000)]
        public string NoiDungNgan { get; set; }
        public string HinhAnh {  get; set; }
        public int ChuDeId { get; set; }
        public ChuDe? ChuDe { get; set; }
        public int TaiKhoanId { get; set; }
        public TaiKhoan? TaiKhoan { get; set; }
    }
}
