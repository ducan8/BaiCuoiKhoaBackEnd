using System.ComponentModel.DataAnnotations;

namespace BaiCuoiKhoaBackEnd.Entities
{
    public class LoaiKhoaHoc
    {
        public int LoaiKhoaHocId { get; set; }
        [MaxLength(30)]
        public string TenLoai {  get; set; }
        public IEnumerable<KhoaHoc>? DanhSachKhoaHoc { get; set; }
    }
}
