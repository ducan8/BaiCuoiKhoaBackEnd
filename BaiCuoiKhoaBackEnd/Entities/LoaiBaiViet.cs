using System.ComponentModel.DataAnnotations;

namespace BaiCuoiKhoaBackEnd.Entities
{
    public class LoaiBaiViet
    {
        public int LoaiBaiVietId { get; set; }
        [MaxLength(50)]
        public string TenLoai {  get; set; }
        public IEnumerable<ChuDe>? DanhSachChuDe { get; set; }
    }
}
