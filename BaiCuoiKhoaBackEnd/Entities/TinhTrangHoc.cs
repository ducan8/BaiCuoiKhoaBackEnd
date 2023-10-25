namespace BaiCuoiKhoaBackEnd.Entities
{
    public class TinhTrangHoc
    {
        public int TinhTrangHocId { get; set; }
        public string TenTinhTrang {  get; set; }
        public IEnumerable<DangKyHoc>? DanhSachDangKyHoc { get; set; }
    }
}
