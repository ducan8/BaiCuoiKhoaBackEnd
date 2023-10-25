using System.ComponentModel.DataAnnotations;

namespace BaiCuoiKhoaBackEnd.Entities
{
    public class KhoaHoc
    {
        public int KhoaHocId { get; set; }
        [MaxLength(50)]
        public string TenKhoaHoc {  get; set; }
        public int ThoiGianHoc {  get; set; }
        public string GioiThieu { get; set; }
        public string NoiDung {  get; set; }
        public double HocPhi { get; set; }
        public int SoHocVien { get; set; }
        public int SoLuongMon { get; set; }
        public string HinhAnh {  get; set; }
        public int LoaiKhoaHocId { get; set; }
        public LoaiKhoaHoc? LoaiKhoaHoc { get; set; }
        public IEnumerable<DangKyHoc>? DanhSachDangKyHoc {  get; set; } 
    }
}
