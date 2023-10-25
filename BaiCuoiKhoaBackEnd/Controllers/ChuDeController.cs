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
    public class ChuDeController : ControllerBase
    {
        private readonly IChuDeServices chuDeServices;
        public ChuDeController()
        {
            chuDeServices = new ChuDeServices();
        }

        [HttpGet("hienThiDanhSachChuDe")]
        public IActionResult LayDanhSachChuDe()
        {
            var res = chuDeServices.HienThiDanhSachChuDe();
            if(res == null)
            {
                return NotFound("Danh sach chu de trong");
            }
            return Ok(res);
        }

        [HttpGet("phanTrangDuLieuChuDe")]
        public IActionResult PhanTrangDuLieu([FromQuery] Pagination pagination)
        {
            var res = chuDeServices.PhanTrangDanhSachChuDe(pagination);
            return Ok(res);
        }

        [HttpPost("themChuDe")]
        public IActionResult ThemChuDe(ChuDe chude)
        {
            var res = chuDeServices.ThemChuDe(chude);
            if(res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Chua ton tai loai bai viet hoac tai khoan. Them chu de that bai");
            } else if(res == ErrorMessage.ThanhCong)
            {
                return Ok("Them chu de thanh cong");
            } else
            {
                return BadRequest("Them chu de that bai");
            }
        }

        [HttpPut("suaChuDe")]
        public IActionResult SuaChuDe(ChuDe chudeUpdate)
        {
            var res = chuDeServices.SuaChuDe(chudeUpdate);
            if(res == ErrorMessage.ThanhCong)
            {
                return Ok("Sua chu de thanh cong");
            }
             else if(res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Thieu du lieu. Khong the sua chu de");
            } else
            {
                return BadRequest("Sua chu de that bai");
            }
        }

        [HttpDelete("xoaChuDe")]
        public IActionResult XoaChuDe(int id)
        {
            var res = chuDeServices.XoaChuDe(id);
            if(res == ErrorMessage.ThanhCong)
            {
                return Ok("Xoa chu de thanh cong");
            }else if (res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Khong tim thay chu de muon xoa");
            } 
            return BadRequest("Xoa chu de that bai");
        }
    }
}
