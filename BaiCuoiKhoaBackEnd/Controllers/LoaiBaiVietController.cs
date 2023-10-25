using BaiCuoiKhoaBackEnd.Constants;
using BaiCuoiKhoaBackEnd.Entities;
using BaiCuoiKhoaBackEnd.ISevices;
using BaiCuoiKhoaBackEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaiCuoiKhoaBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiBaiVietController : ControllerBase
    {
        private readonly ILoaiBaiVietServices loaiBaiVietService;
        public LoaiBaiVietController()
        {
            loaiBaiVietService = new LoaiBaiVietServices();
        }

        [HttpGet("getDSLoaiBaiViet")]
        public ActionResult<IQueryable<LoaiBaiViet>> GetDSLoaiBaiViet()
        {
            var result = loaiBaiVietService.HienThiDanhSachLoaiBaiViet();
            if (result == null)
            {
                return NotFound("Khong co loai bai viet nao ton tai");
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("phanTrangDuLieuLoaiBaiViet")]
        public IActionResult PhanTrangDuLieu([FromQuery] Pagination pagination)
        {
            var res = loaiBaiVietService.PhanTrangDanhSachLoaiBaiViet(pagination);
            return Ok(res);
        }

        [HttpPost("themLoaiBaiViet")]
        public IActionResult ThemLoaiBaiViet(LoaiBaiViet loaiBaiViet)
        {
            var result = loaiBaiVietService.ThemLoaiBaiViet(loaiBaiViet);
            if (result == ErrorMessage.ThanhCong)
            {
                return Ok("Them loai bai viet thanh cong");
            }
            else
            {
                return BadRequest("Them loai bai viet that bai");
            }
        }

        [HttpPut("suaLoaiBaiViet")]
        public IActionResult SuaLoaiBaiViet(LoaiBaiViet loaiBaiVietUpdate)
        {
            var result = loaiBaiVietService.SuaLoaiBaiViet(loaiBaiVietUpdate);
            if (result == ErrorMessage.ThanhCong)
            {
                return Ok("Sua loai bai viet thanh cong");
            }
            else if (result == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Loai bai viet chua ton tai");
            }
            else
            {
                return BadRequest("Them loai bai viet that bai");
            }
        }

        [HttpDelete("xoaLoaiBaiViet")]
        public IActionResult XoaLoaiBaiViet(int id)
        {
            var result = loaiBaiVietService.XoaLoaiBaiViet(id);
            if (result == ErrorMessage.ThanhCong)
            {
                return Ok("Xoa loai bai viet thanh cong");
            }
            else if (result == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Loai bai viet chua ton tai");
            }
            else
            {
                return BadRequest("Xoa loai bai viet that bai");
            }
        }
    }
}
