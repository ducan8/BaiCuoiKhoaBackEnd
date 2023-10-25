using System.ComponentModel.DataAnnotations;

namespace BaiCuoiKhoaBackEnd.Entities
{
    public class QuyenHan
    {
        public int QuyenHanId { get; set; }
        [MaxLength(50)]
        public string TenQuyenHan {  get; set; }
        public IEnumerable<TaiKhoan>? DanhSachTaiKhoan { get; set; }
    }
}
