using System.ComponentModel.DataAnnotations;

namespace BaiCuoiKhoaBackEnd.Entities
{
    public class ChuDe
    {
        public int ChuDeId { get; set; }
        [MaxLength(50)]
        public string TenChuDe {  get; set; }
        public string NoiDung {  get; set; }
        public int LoaiBaiVietId { get; set; }
        public LoaiBaiViet? LoaiBaiViet { get; set; }
        public IEnumerable<BaiViet>? DanhSachBaiViet { get; set; }

    }
}
