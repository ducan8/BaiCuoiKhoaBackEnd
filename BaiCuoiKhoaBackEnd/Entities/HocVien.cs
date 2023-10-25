using System.ComponentModel.DataAnnotations;

namespace BaiCuoiKhoaBackEnd.Entities
{
    public class HocVien
    {
        public int HocVienId { get; set; }
        public string HinhAnh { get; set; }
        [MaxLength(50)]
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        [MaxLength(11)]
        public string SoDienThoai { get; set; }
        [MaxLength(40)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string TinhThanh { get; set; }
        [MaxLength(50)]
        public string QuanHuyen { get; set; }
        [MaxLength(50)]
        public string PhuongXa { get;set; }
        [MaxLength(50)]
        public string SoNha {  get; set; }
        public IEnumerable<DangKyHoc>? DanhSachDangKyHoc { get; set; }
    }
}
