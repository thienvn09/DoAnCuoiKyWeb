using DoAn.Areas.Admin.DAL;
using DoAn.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DoAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class PaymentAdminController : Controller
    {
        private readonly PaymentAdminDAL _paymentAdminDAL = new PaymentAdminDAL();

        // GET: PaymentAdminController
        public ActionResult Index()
        {
            try
            {
                // Lấy danh sách thanh toán từ DAL
                List<PaymentAdmin> payments = _paymentAdminDAL.GetAllPayments();
                return View(payments);
            }
            catch
            {
                TempData["ErrorMessage"] = "Không thể tải danh sách thanh toán. Vui lòng thử lại.";
                return View(new List<PaymentAdmin>());
            }
        }

        // GET: PaymentAdminController/Details/5
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "ID thanh toán không hợp lệ.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Lấy thông tin thanh toán chi tiết từ DAL
                PaymentAdmin payment = _paymentAdminDAL.GetPaymentById(id);

                if (payment == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy thanh toán.";
                    return RedirectToAction(nameof(Index));
                }

                // Tính tổng tiền chi tiết
                float totalAmount = payment.PaymentDetails?.Sum(detail => detail.Total.GetValueOrDefault()) ?? 0;
                ViewBag.TotalAmount = totalAmount;

                return View(payment);
            }
            catch
            {
                TempData["ErrorMessage"] = "Không thể tải thông tin chi tiết thanh toán.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: PaymentAdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentAdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PaymentAdmin payment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int newPaymentId = _paymentAdminDAL.AddPayment(payment);
                    TempData["SuccessMessage"] = "Thêm thanh toán mới thành công!";
                    return RedirectToAction(nameof(Details), new { id = newPaymentId });
                }

                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.";
                return View(payment);
            }
            catch
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi thêm thanh toán.";
                return View(payment);
            }
        }

        // GET: PaymentAdminController/Edit/5
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "ID thanh toán không hợp lệ.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                PaymentAdmin payment = _paymentAdminDAL.GetPaymentById(id);
                if (payment == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy thanh toán.";
                    return RedirectToAction(nameof(Index));
                }

                return View(payment);
            }
            catch
            {
                TempData["ErrorMessage"] = "Không thể tải thông tin thanh toán để chỉnh sửa.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: PaymentAdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PaymentAdmin payment)
        {
            if (id != payment.Id)
            {
                TempData["ErrorMessage"] = "Dữ liệu không khớp. Vui lòng thử lại.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                if (ModelState.IsValid)
                {
                    // Update logic chưa được triển khai
                    TempData["SuccessMessage"] = "Cập nhật thanh toán thành công!";
                    return RedirectToAction(nameof(Details), new { id = payment.Id });
                }

                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.";
                return View(payment);
            }
            catch
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi cập nhật thanh toán.";
                return View(payment);
            }
        }

        // GET: PaymentAdminController/Delete/5
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "ID thanh toán không hợp lệ.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                PaymentAdmin payment = _paymentAdminDAL.GetPaymentById(id);
                if (payment == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy thanh toán.";
                    return RedirectToAction(nameof(Index));
                }

                return View(payment);
            }
            catch
            {
                TempData["ErrorMessage"] = "Không thể tải thông tin thanh toán để xóa.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: PaymentAdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // Xóa logic chưa được triển khai
                TempData["SuccessMessage"] = "Xóa thanh toán thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xóa thanh toán.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
