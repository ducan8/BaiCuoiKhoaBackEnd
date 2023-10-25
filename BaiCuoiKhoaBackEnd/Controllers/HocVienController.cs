using BaiCuoiKhoaBackEnd.Services;
using BaiCuoiKhoaBackEnd.ISevices;
using BaiCuoiKhoaBackEnd.Entities;
using BaiCuoiKhoaBackEnd.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaiCuoiKhoaBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HocVienController : ControllerBase
    {
        private readonly IHocVienServices hocvienService;
        public HocVienController()
        {
            hocvienService = new HocVienServices();
        }


        [HttpGet("hienThiDanhSachHocVien")]
        public IActionResult HienThiDanhSachHocVien()
        {
            var res = hocvienService.HienThiDanhSachHocVien();
            if(res == null)
            {
                return NotFound("Danh sach hoc vien trong");
            } return Ok(res);
        }

        [HttpGet("timKiemHocVienTheoEmailVaSDT")]
        public IActionResult TimKiemTheoEmailVaSDT(string keyword)
        {
            var res = hocvienService.TimKiemTheoTenVaEmail(keyword);
            if(res == null)
            {
                return NotFound("Khong tim thay hoc vien");
            } return Ok(res);
        }

        [HttpGet("phanTrangDuLieu")]
        public IActionResult PhanTrangDuLieu([FromQuery] Pagination pagination, string? keyword) 
        {
            var res = hocvienService.PhanTrangDuLieu(pagination, keyword);
            return Ok(res);
        }
        
        [HttpPost("themHocVien")]
        public IActionResult ThemHocVien(HocVien hocVien)
        {
            var res = hocvienService.ThemHocVien(hocVien);
            if(res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Thieu du lieu. Khong the them hoc vien");
            } else if(res == ErrorMessage.ThanhCong)
            {
                return Ok("Them hoc vien thanh cong");
            } else if(res == ErrorMessage.DaTonTai)
            {
                return BadRequest("Thong tin da ton tai. Khong the them hoc vien moi");
            }
            else
            {
                return BadRequest("Them that bai");
            }
        }

        [HttpPut("suaHocVien")]
        public IActionResult SuaHocVien(HocVien hocvienUpdate)
        {
            var res = hocvienService.SuaHocVien(hocvienUpdate);
            if(res == ErrorMessage.ThanhCong)
            {
                return Ok("Sua hoc vien thanh cong");
            } else if(res == ErrorMessage.ChuaTonTai)
            {
                return BadRequest("Hoc vien khong ton tai. Khong the sua thong tin hoc vien");
            } else if(res == ErrorMessage.DaTonTai)
            {
                return BadRequest("Thong tin bi trung lap. Khong the sua thong tin hoc vien");
            } else
            {
                return BadRequest("Sua thong tin that bai");
            }
        }

        [HttpDelete("xoaHocVien")]
        public IActionResult XoaHocVien(int id)
        {
            var res = hocvienService.XoaHocVien(id);
            if(res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Chua ton tai hoc vien muon xoa");
            } else if(res == ErrorMessage.ThanhCong)
            {
                return Ok("Xoa thanh cong");
            }
            else
            {
                return BadRequest("Xoa that bai");
            }
        }
    }
}
