using BaiCuoiKhoaBackEnd.Entities;
using BaiCuoiKhoaBackEnd.ISevices;
using BaiCuoiKhoaBackEnd.Services;
using BaiCuoiKhoaBackEnd.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace BaiCuoiKhoaBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaiVietController : ControllerBase
    {
        private readonly IBaiVietServices baivietServices;
        public BaiVietController()
        {
            baivietServices = new BaiVietServices();
        }

        [HttpGet("hienThiDanhSachBaiViet")]
        public IActionResult LayDanhSachBaiViet()
        {
            var res = baivietServices.HienThiDanhSachBaiViet();
            if (res == null)
            {
                return NotFound("Danh sach bai viet trong");
            }
            return Ok(res);
        }

        [HttpGet("phanTrangDanhSachBaiViet")]
        public IActionResult PhanTrangDuLieu([FromQuery] Pagination pagination, string? keyword)
        {
            var res = baivietServices.PhanTrangDanhSachBaiViet(pagination, keyword);
            return Ok(res);
        }

        [HttpGet("timKiemBaiVietTheoTen")]
        public IActionResult TimKiemBaiVietTheoTen(string keyword)
        {
            var res = baivietServices.TimBaiVietTheoTen(keyword);
            if(res == null)
            {
                return NotFound("Khong tim thay bai viet");
            } return Ok(res);

        }

        [HttpPost("themBaiViet")]
        public IActionResult ThemBaiViet(BaiViet baiviet)
        {
            var res = baivietServices.ThemBaiViet(baiviet);
            if(res == ErrorMessage.ThanhCong)
            {
                return Ok("Them bai viet thanh cong");
            }
            else
            {
                return BadRequest("Them bai viet that bai");
            }
        }

        [HttpPut("suaBaiViet")]
        public IActionResult SuaBaiViet(BaiViet baiViet)
        {
            var res = baivietServices.SuaBaiViet(baiViet);
            if (res == ErrorMessage.ThanhCong)
            {
                return Ok("Sua bai viet thanh cong");
            }
            else if (res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Thieu du lieu. Khong the sua bai viet");
            }
            else
            {
                return BadRequest("Sua bai viet that bai");
            }
        }

        [HttpDelete("xoaBaiViet")]
        public IActionResult XoaBaiViet(int id)
        {
            var res = baivietServices.XoaBaiViet(id);
            if(res == ErrorMessage.ThanhCong)
            {
                return Ok("Xoa thanh cong");
            } else if(res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Khong tim thay bai viet can xoa");
            } else
            {
                return BadRequest("Xoa bai viet that bai");
            }
        }
    }
}
