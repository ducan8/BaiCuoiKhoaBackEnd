using BaiCuoiKhoaBackEnd.Entities;
using BaiCuoiKhoaBackEnd.ISevices;
using BaiCuoiKhoaBackEnd.Services;
using BaiCuoiKhoaBackEnd.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaiCuoiKhoaBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaiKhoanController : ControllerBase
    {
        private readonly ITaiKhoanServies taiKhoanServies;
        public TaiKhoanController()
        {
            taiKhoanServies = new TaiKhoanServices();
        }
        [HttpGet("hienThiDanhSachTaiKhoan")]
        public IActionResult HienThiDanhSachTaiKhoan()
        {
            var res = taiKhoanServies.HienThiDanhSachTaiKhoan();
            if(res == null)
            {
                return NotFound("Danh sach tai khoan trong");
            } else
            {
                return Ok(res);
            }
        }

        [HttpGet("timKiemTaiKhoan")]
        public IActionResult TimKiemTaiKhoan(string keyword)
        {
            var res = taiKhoanServies.TimKiemTaiKhoan(keyword);
            if(res == null)
            {
                return NotFound("Khong ton tai tai khoan can tim");
            } else
            {
                return Ok(res);
            }
        }

        [HttpGet("phanTrangTimKiemTaiKhoan")]
        public IActionResult PhanTrangTimKiemTaiKhoan([FromQuery] Pagination pagination, string? keyword)
        {
            return Ok(taiKhoanServies.PhanTrangDanhSachTaiKhoan(pagination, keyword));
        }

        [HttpPost("themTaiKhoan")]
        public IActionResult ThemTaiKhoan(TaiKhoan taiKhoan)
        {
            var res = taiKhoanServies.ThemTaiKhoan(taiKhoan);
            if(res == ErrorMessage.ThanhCong)
            {
                return Ok("Them tai khoan thanh cong");
            } else if(res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Khong ton tai quyen han. Them tai khoan that bai");
            } else if (res == ErrorMessage.MatKhauThieuKyTuDacBiet)
            {
                return BadRequest("Mat khau phai chua ky tu dac biet");
            } else if(res == ErrorMessage.MatKhauThieuChuSo)
            {
                return BadRequest("Mat khau phai chua so");
            } else if(res == ErrorMessage.DaTonTai)
            {
                return BadRequest("Ten dang nhap da ton tai");
            }
            
            else
            {
                return BadRequest("Them tai khoan that bai");
            }
        }

        [HttpPut("suaTaiKhoan")]
        public IActionResult SuaTaiKhoan(TaiKhoan taikhoanUpdate)
        {
            var res = taiKhoanServies.SuaTaiKhoan(taikhoanUpdate);
            if( res == ErrorMessage.ThanhCong)
            {
                return Ok("Sua tai khoan thanh cong");
            } else if(res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Khong ton tai tai khoan ban muon sua");
            } else if(res == ErrorMessage.QuyenHanChuaTonTai)
            {
                return NotFound("Khong ton tai quyen han ban muon sua");
            } else if(res == ErrorMessage.DaTonTai)
            {
                return BadRequest("Da ton tai ten dang nhap");
            } else if(res == ErrorMessage.MatKhauThieuKyTuDacBiet || res == ErrorMessage.MatKhauThieuChuSo)
            {
                return BadRequest("Mat khau thieu ky tu hoac so");
            } else
            {
                return BadRequest("Sua tai khoan that bai");
            }
        }

        [HttpDelete("xoaTaiKhoan")]
        public IActionResult XoaTaiKhoan(int id)
        {
            var res = taiKhoanServies.XoaTaiKhoan(id);
            if(res == ErrorMessage.ThanhCong)
            {
                return Ok("Xoa tai khoan thanh cong");
            } else if (res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Khong ton tai tai khoan muon xoa");
            } else { return BadRequest("Xoa tai khoan that bai"); }
        }


    }
}
