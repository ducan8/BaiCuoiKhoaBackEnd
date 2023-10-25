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
    public class TinhTrangHocController : ControllerBase
    {
        private readonly ITinhTrangHocServies tinhTrangHocServies;
        public TinhTrangHocController()
        {
            tinhTrangHocServies = new TinhTrangHocServices();
        }

        [HttpGet("hienThiTinhTrangHoc")]
        public IActionResult HienThiTinhTrangHoc()
        {
            var res = tinhTrangHocServies.HienThiDanhSachTinhTrangHoc();
            if(res  == null)
            {
                return NotFound("Danh sach tinh trang hoc trong");
            } return Ok(res);
        }

        [HttpPost("themTinhTrangHoc")]
        public IActionResult ThemTinhTrangHoc(TinhTrangHoc tinhTrangHoc)
        {
            var res = tinhTrangHocServies.ThemTinhTrangHoc(tinhTrangHoc);
            if(res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Thieu du lieu. Khong the them tinh trang hoc");
            } else if(res == ErrorMessage.ThanhCong)
            {
                return Ok("Them tinh trang hoc thanh cong");
            } else
            {
                return BadRequest("Them tinh trang hoc that bai");
            }
        }

        [HttpPut("suaTinhTrangHoc")]
        public IActionResult SuaTinhTrangHoc(TinhTrangHoc tinhTrangHoc)
        {
            var res = tinhTrangHocServies.SuaTinhTrangHoc(tinhTrangHoc);
            if(res == ErrorMessage.ThanhCong)
            {
                return Ok("Sua thanh cong");
            } else if (res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Thieu du lieu. Khong the sua tinh trang hoc");
            } else
            {
                return BadRequest("Sua that bai");
            }
        }

        [HttpDelete("xoaTinhTrangHoc")]
        public IActionResult XoaTinhTrangHoc(int id)
        {
            var res = tinhTrangHocServies.XoaTinhTrangHoc(id);
            if(res == ErrorMessage.ChuaTonTai)
            {
                return NotFound("Khong ton tai tinh trang hoc can xoa");
            } else if(res == ErrorMessage.ThanhCong)
            {
                return Ok("Xoa tinh trang hoc thanh cong");
            } else
            {
                return BadRequest("Xoa tinh trang hoc that bai");
            }
        }
    }
}
