using DoAn.DAL;
using DoAn.Helper;
using DoAn.Models;
using DoAn.Models.Vnpay;
using DoAn.Service;
using DoAn.Service.Vnpay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;
using System.Net;
using System.Text;
using System.Security.Cryptography;

namespace DoAn.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductDAL _productDAL = new ProductDAL();
        private readonly CustomerDAL _customerDAL = new CustomerDAL();
        private readonly CartDAL _cartDAL = new CartDAL();
        private readonly IMomoService _momoService;
        private readonly MomoPaymentDAL _momoPaymentDAL;
        public CartController(IMomoService momoService)
        {
            _momoService = momoService;
        }

        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MyConst.CART_KEY) ?? new List<CartItem>();

        public IActionResult Index()
        {
            return View(Cart);
        }

        // Lấy tổng tiền thanh toán
        public IActionResult GetTotalAmount()
        {
            int totalAmount = Cart.Sum(item => item.Total);
            return Json(totalAmount);
        }

        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.IdProduct == id);
            if (item == null)
            {
                Product productById = _productDAL.GetProductById(id);
                if (productById == null)
                {
                    TempData["Message"] = "Không tìm thấy sản phẩm";
                    return Redirect("/404");
                }
                item = new CartItem
                {
                    IdProduct = productById.Id,
                    Img = productById.Img,
                    Name = productById.Title,
                    Price = productById.Price,
                    Rate = productById.Rate,
                    Quantity = quantity
                };
                gioHang.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }
            HttpContext.Session.Set(MyConst.CART_KEY, gioHang);
            return RedirectToAction("Index");
        }

        public IActionResult ChangeQuantityCart(int id, bool isIncrement = true, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.IdProduct == id);
            if (item == null)
            {
                TempData["Message"] = "Không tìm thấy sản phẩm";
                return Redirect("/404");
            }
            else
            {
                if (isIncrement)
                {
                    item.Quantity += quantity;
                }
                else
                {
                    item.Quantity -= quantity;
                    if (item.Quantity <= 0)
                    {
                        gioHang.Remove(item);
                    }
                }
            }
            HttpContext.Session.Set(MyConst.CART_KEY, gioHang);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveCart(int id)
        {
            var cart = Cart;
            var item = cart.SingleOrDefault(p => p.IdProduct == id);

            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.Set(MyConst.CART_KEY, cart);
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> CheckOut(string paymentMethod, string orderName, string orderDescription)
        {
            try
            {
                // Kiểm tra giỏ hàng có sản phẩm hay không
                if (!Cart.Any())
                {
                    TempData["CheckOutErrorMessage"] = "Giỏ hàng trống.";
                    return RedirectToAction("Index");
                }

                // Lấy thông tin người dùng từ session
                string customerIdStr = HttpContext.User.FindFirstValue("CustomerId");
                if (string.IsNullOrEmpty(customerIdStr))
                {
                    TempData["CheckOutErrorMessage"] = "Bạn cần đăng nhập để thanh toán.";
                    return Redirect("/Customer/SignIn");
                }

                int customerId = int.Parse(customerIdStr);
                var customer = _customerDAL.GetCustomerById(customerId);

                if (customer == null)
                {
                    TempData["CheckOutErrorMessage"] = "Không tìm thấy thông tin khách hàng.";
                    return RedirectToAction("Index");
                }

                // Tạo thông tin đơn hàng
                var orderInfo = new OrderInfoModel
                {
                    FullName = orderName ?? $"{customer.FirstName} {customer.LastName}", // Nếu orderName null thì lấy tên khách hàng
                    Amount = Cart.Sum(p => p.Total).ToString("#,##0"), // Tính tổng tiền trong giỏ hàng
                    OrderInfo = orderDescription
                };

                // Xử lý theo phương thức thanh toán
                switch (paymentMethod.ToLower())
                {
                    case "momo":
                        var momoResponse = await _momoService.CreatePaymentAsync(orderInfo);
                        if (!string.IsNullOrEmpty(momoResponse.PayUrl))
                        {
                            // Lưu thông tin thanh toán MoMo vào cơ sở dữ liệu
                            var momoPayment = new MomoPayment
                            {
                                CustomerId = customer.Id,
                                FirstName = customer.FirstName,
                                LastName = customer.LastName,
                                Phone = customer.Phone,
                                Email = customer.Email,
                                CreateAt = DateTime.Now,
                                Total = (float?).orderInfo.Amount, // Chuyển thành decimal
                                MomoTransactionId = momoResponse.TransactionId,
                                PayUrl = momoResponse.PayUrl,
                                PaymentStatus = "Pending",
                                PaymentDate = DateTime.Now,
                                OrderInfo = orderDescription
                            };

                            var success = await _momoPaymentDAL.AddMomoPaymentAsync(momoPayment);

                            // Nếu tạo link thanh toán thành công, chuyển hướng người dùng
                            if (success)
                            {
                                HttpContext.Session.Remove(MyConst.CART_KEY); // Xóa giỏ hàng sau khi thanh toán
                                return Redirect(momoResponse.PayUrl); // Chuyển hướng tới URL thanh toán MoMo
                            }
                        }

                        TempData["CheckOutErrorMessage"] = "Không thể tạo liên kết thanh toán qua MoMo.";
                        break;

                    case "cod":
                        // Thanh toán khi nhận hàng (COD)
                        var isOrderSuccessful = _cartDAL.CheckOut(customer, Cart);
                        if (isOrderSuccessful)
                        {
                            HttpContext.Session.Remove(MyConst.CART_KEY); // Xóa giỏ hàng
                            TempData["CheckOutSuccessMessage"] = "Đặt hàng thành công.";
                            return RedirectToAction("Index");
                        }

                        TempData["CheckOutErrorMessage"] = "Đặt hàng thất bại.";
                        break;

                    default:
                        TempData["CheckOutErrorMessage"] = "Phương thức thanh toán không hợp lệ.";
                        break;
                }

                // Nếu có lỗi, quay lại trang giỏ hàng
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["CheckOutErrorMessage"] = $"Lỗi hệ thống: {ex.Message}";
                return RedirectToAction("Index");
            }
        }



        // Refresh Cart View Component
        public IActionResult RefreshCartViewComponent()
        {
            return ViewComponent("Cart");
        }

        // Lấy tổng tiền theo từng Product
        public IActionResult GetTotalProduct(int idProduct)
        {
            var productFind = Cart.Find(item => item.IdProduct == idProduct);

            int totalAmount = 0;
            if (productFind != null)
            {
                totalAmount = productFind.Total;
            }
            return Json(totalAmount);
        }

        // Phương thức callback từ MoMo để xác nhận thanh toán
        [HttpPost]
        public IActionResult MoMoCallback(string transactionId, string status)
        {
            // Xử lý callback từ MoMo, cập nhật trạng thái thanh toán
            if (status == "success")
            {
                var success = _cartDAL.UpdatePaymentStatus(transactionId, "Completed");
                if (success)
                {
                    TempData["CheckOutSuccessMessage"] = "Thanh toán thành công!";
                }
            }
            else
            {
                _cartDAL.UpdatePaymentStatus(transactionId, "Failed");
                TempData["CheckOutErrorMessage"] = "Thanh toán thất bại!";
            }

            return RedirectToAction("Index");
        }
    }
}
