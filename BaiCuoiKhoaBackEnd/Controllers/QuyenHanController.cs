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
    public class QuyenHanController : ControllerBase
    {
        private readonly IQuyenHanServices quyenHanServices;
        public QuyenHanController()
        {
            quyenHanServices = new QuyenHanServices();
        }

        [HttpGet("hienThiDanhSachQuyenHan")]
        public IActionResult HienThiDanhSachQuyenHan() {
            var res = quyenHanServices.HienThiDanhSachQuyenHan();
            if(res == null)
            {
                return NotFound("Danh sach quyen han trong");
            } return Ok(res);
        }

        [HttpGet("phanTrangHienThiDanhSachQuyenHan")]
        public IActionResult PhanTrangDuLieu([FromQuery] Pagination pagination)
        {
            var res = quyenHanServices.PhanTrangHienThiQuyenHan(pagination);
            return Ok(res);
        }

        [HttpPost("themQuyenHan")]
        public IActionResult ThemQuyenHan(QuyenHan quyenHan)
        {
            var res = quyenHanServices.ThemQuyenHan(quyenHan);
            if(res == ErrorMessage.ThanhCong)
            {
                return Ok("Them quyen han thanh cong");
            } else
            {
                return BadRequest("Them quyen han that bai");
            }
        }

        [HttpPut("suaQuyenHan")]
        public IActionResult SuaQuyenHan(QuyenHan quyenHanUpdate)
        {
            var res = quyenHanServices.SuaQuyenHan(quyenHanUpdate);
            if(res == ErrorMessage.ThanhCong)
            {
                return Ok("Sua quyen han thanh cong");
            } else if (res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Khong tim thay quyen han muon sua");
            } else
            {
                return BadRequest("Sua quyen han that bai");
            }
        }

        [HttpDelete("xoaQuyenHan")]
        public IActionResult XoaQuyenHan(int id)
        {
            var res = quyenHanServices.XoaQuyenHan(id);
            if(res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Khong tim thay quyen han can xoa");
            } else if(res == ErrorMessage.ThanhCong)
            {
                return Ok("Xoa quyen han thanh cong");
            } else
            {
                return BadRequest("Xoa quyen han that bai");
            }
        }
    }
}
