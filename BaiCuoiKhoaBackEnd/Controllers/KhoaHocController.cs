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
    public class KhoaHocController : ControllerBase
    {
        private readonly IKhoaHocServices khoaHocServices;
        public KhoaHocController()
        {
            khoaHocServices = new KhoaHocServices();
        }

        [HttpGet("hienThiDanhSachKhoaHoc")]
        public IActionResult HienThiDanhSachKhoaHoc()
        {
            var res = khoaHocServices.HienThiDanhSachKhoaHoc();
            if (res == null)
            {
                return NotFound("Khoa hoc chua ton tai");
            }
            else
            {
                return Ok(res);
            }
        }

        [HttpGet("timKiemKhoaHocTheoTen")]
        public IActionResult TimKiemTheoTen(string keyword)
        {
            var res = khoaHocServices.TimKhoaHocTheoTen(keyword);
            if(res == null)
            {
                return NotFound("Khong tim thay khoa hoc");
            } else
            {
                return Ok(res);
            }
        }

        [HttpGet("phanTrangDuLieu")]
        public IActionResult PhanTrangDuLieu([FromQuery] Pagination pagination)
        {
            var res = khoaHocServices.PhanTrangDanhSachKhoaHoc(pagination);
            return Ok(res);
        }

        [HttpPost("themKhoaHoc")]
        public IActionResult ThemKhoaHoc(KhoaHoc khoahoc)
        {
            var res = khoaHocServices.ThemKhoaHoc(khoahoc);
            if (res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Loai khoa hoc chua ton tai. Khong the them khoa hoc");
            }
            else if (res == ErrorMessage.ThanhCong)
            {
                return Ok("Them khoa hoc thanh cong");
            }
            else
            {
                return BadRequest("Them khoa hoc that bai");
            }
        }

        [HttpPut("suaKhoaHoc")]
        public IActionResult SuaKhoaHoc(KhoaHoc khoaHocUpdate)
        {
            var res = khoaHocServices.SuaKhoaHoc(khoaHocUpdate);
            if(res == ErrorMessage.ThanhCong)
            {
                return Ok("Sua khoa hoc thanh cong");
            } else if(res == ErrorMessage.KhoaHocChuaTonTai)
            {
                return NotFound("Khoa hoc chua ton tai");
            } else if (res == ErrorMessage.LoaiKhoaHocChuaTonTai)
            {
                return NotFound("Loai khoa hoc chua ton tai");
            } else if(res == ErrorMessage.TaiKhoanChuaTonTai)
            {
                return NotFound("Tai khoan chua ton tai");
            } else if(res == ErrorMessage.HocVienChuaTonTai)
            {
                return NotFound("Hoc vien chua ton tai");
            } else if(res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Thieu du lieu. Khong the them khoa hoc");
            } else
            {
                return BadRequest("Them khoa hoc that bai");
            }
        }

        [HttpDelete("xoaKhoaHoc")]
        public IActionResult XoaKhoaHoc([FromQuery] int id)
        {
            var res = khoaHocServices.XoaKhoaHoc(id);
            if(res == ErrorMessage.KhoaHocChuaTonTai)
            {
                return NotFound("Khoa hoc khong ton tai. Xoa that bai");
            } else if(res == ErrorMessage.ThanhCong)
            {
                return Ok("Xoa khoa hoc thanh cong");
            } else
            {
                return BadRequest("Xoa khoa hoc that bai");
            }
        }
    }
}
