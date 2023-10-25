using BaiCuoiKhoaBackEnd.Entities;
using BaiCuoiKhoaBackEnd.ISevices;
using BaiCuoiKhoaBackEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaiCuoiKhoaBackEnd.Constants;

namespace BaiCuoiKhoaBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiKhoaHocController : ControllerBase
    {
        private readonly ILoaiKhoaHocServices loaiKhoaHocService;
        public LoaiKhoaHocController()
        {
            loaiKhoaHocService = new LoaiKhoaHocServices();
        }

        [HttpGet("getDSLoaiKhoaHoc")]
        public ActionResult<IQueryable<LoaiKhoaHoc>> GetLoaiKhoaHoc() {
            var result = loaiKhoaHocService.GetDSLoaiKH();
            if(result == null)
            {
                return NotFound("Loai khoa hoc chua ton tai");
            } else
            {
                return Ok(result);
            }
        }

        [HttpPost("themLoaiKhoaHoc")]
        public IActionResult ThemLoaiKhoaHoc(LoaiKhoaHoc loaiKH)
        {
            var result = loaiKhoaHocService.ThemLoaiKhoaHoc(loaiKH);
            if (result == ErrorMessage.ThanhCong)
            {
                return Ok("Them loai khoa hoc thanh cong");
            }
            else
            {
                return BadRequest("Them loai khoa hoc that bai");
            }
        }

        [HttpPut("suaLoaiKhoaHoc")]
        public IActionResult SuaLoaiKhoaHoc(LoaiKhoaHoc loaiKHUpdate)
        {
            var result = loaiKhoaHocService.SuaLoaiKhoaHoc(loaiKHUpdate);
            if (result == ErrorMessage.ThanhCong)
            {
                return Ok("Sua loai khoa hoc thanh cong");
            }
            else if (result == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Loai khoa hoc chua ton tai");
            }
            else
            {
                return BadRequest("Them loai khoa hoc that bai");
            }
        }

        [HttpDelete("xoaLoaiKhoaHoc")]
        public IActionResult XoaLoaiKhoaHoc(int id)
        {
            var result = loaiKhoaHocService.XoaLoaiKhoaHoc(id);
            if (result == ErrorMessage.ThanhCong)
            {
                return Ok("Xoa loai khoa hoc thanh cong");
            }
            else if (result == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Loai khoa hoc chua ton tai");
            }
            else
            {
                return BadRequest("Xoa loai khoa hoc that bai");
            }
        }
    }
}
