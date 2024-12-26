using DoAn.Models;
using DoAn.Models.Vnpay;
using DoAn.Service;
using DoAn.Service.Vnpay;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IVnPayService _vnPayService;

        public PaymentController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        public IActionResult CreatePaymentUrl(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }

        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            return Json(response);
        }
    }

}
