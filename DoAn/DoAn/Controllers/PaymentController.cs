using DoAn.DAL;
using DoAn.Models;
using DoAn.Models.Momo;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaymentDAL _paymentDAL = new PaymentDAL();

        // Xử lý callback từ Momo
        [HttpPost("callback")]
        public IActionResult MomoCallback([FromBody] PaymentInfo paymentInfo)
        {
            try
            {
                if (paymentInfo.ResultCode == 0) // Thanh toán thành công
                {
                    _paymentDAL.SavePayment(paymentInfo);
                    return Ok(new { message = "Lưu thông tin thanh toán thành công!" });
                }
                else
                {
                    return BadRequest(new { message = "Thanh toán thất bại!" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi xử lý callback", error = ex.Message });
            }
        }

        // API lấy danh sách thanh toán
        [HttpGet("list")]
        public IActionResult PaymentList()
        {
            try
            {
                var payments = _paymentDAL.GetAllPayments();
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách thanh toán", error = ex.Message });
            }
        }

        // Render View thanh toán
        [HttpGet("Payment/PaymentView")]
        public IActionResult PaymentView()
        {
            try
            {
                var payments = _paymentDAL.GetAllPayments();
                return View("PaymentView", payments);  // Render PaymentView.cshtml
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Lỗi khi lấy danh sách thanh toán: " + ex.Message;
                return View("Error");
            }
        }
    }
}
