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
    public class DangKyHocController : ControllerBase
    {
        private readonly IDangKyHocServices dangKyHocServices;
        public DangKyHocController()
        {
            dangKyHocServices = new DangKyHocServices();
        }

        [HttpGet("layDanhSachDangKyHoc")]
        public IActionResult LayDanhSachDangKyHoc()
        {
            var res = dangKyHocServices.HienThiDanhSachDangKyHoc();
            if(res == null)
            {
                return NotFound("Danh sach dang ky hoc trong");
            } 
            return Ok(res);
        }

        [HttpGet("phanTrangDuLieu")]
        public IActionResult PhanTrangDuLieu([FromQuery] Pagination pagination)
        {
            var res = dangKyHocServices.PhanTrangDuLieu(pagination);
            return Ok(res);
        }

        [HttpPost("themDangKyHoc")]
        public IActionResult ThemDangKyHoc(DangKyHoc dangKyHoc)
        {
            var res = dangKyHocServices.ThemDangKyHoc(dangKyHoc);
            if(res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Thieu du lieu. Khong the them dang ky hoc");
            }  else if (res == ErrorMessage.ThanhCong)
            {
                return Ok("Them dang ky hoc thanh cong");
            } else
            {
                return BadRequest("Them dang ky hoc that bai");
            }
        }

        [HttpPut("suaDangKyHoc")]
        public IActionResult SuaDangKyHoc(DangKyHoc dangKyHocUpdate)
        {
            var res = dangKyHocServices.SuaDangKyHoc(dangKyHocUpdate);
            if(res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Thieu du lieu. Khong the sua dang ky hoc");
            } else if (res == ErrorMessage.ThanhCong)
            {
                return Ok("Sua dang ky hoc thanh cong");
            } else
            {
                return BadRequest("Sua dang ky hoc that bai");
            }
        }

        [HttpDelete("xoaDangKyHoc")]
        public IActionResult XoaDangKyHoc(int id)
        {
            var res = dangKyHocServices.XoaDangKyHoc(id);
            if(res == ErrorMessage.ThanhCong)
            {
                return Ok("Xoa thanh cong");
            } else if(res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Khong tim thay dang ky hoc can xoa");
            } else
            {
                return BadRequest("Xoa that bai");      
            }
        }
    }
}
