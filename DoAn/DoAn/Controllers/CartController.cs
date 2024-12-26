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
        private readonly ProductDAL _productDAL;
        private readonly CustomerDAL _customerDAL;
        private readonly CartDAL _cartDAL;
        private readonly IMomoService _momoService;
        private readonly IVnPayService _vnPayService;

        public CartController(IMomoService momoService, IVnPayService vnPayService)
        {
            _productDAL = new ProductDAL();
            _customerDAL = new CustomerDAL();
            _cartDAL = new CartDAL();
            _momoService = momoService;
            _vnPayService = vnPayService;
        }

        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MyConst.CART_KEY) ?? new List<CartItem>();

        public IActionResult Index()
        {
            var payment = new PaymentInformationModel
            {
                Name = "Guest",
                Amount = Cart.Sum(p => p.Total) * 100,
                OrderType = "Giỏ hàng",
                OrderDescription = "Thanh toán qua giỏ hàng"
            };
            ViewData["PaymentResponseModel"] = payment;
            return View(Cart);
        }

        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var cart = Cart;
            var item = cart.SingleOrDefault(p => p.IdProduct == id);
            if (item == null)
            {
                var product = _productDAL.GetProductById(id);
                if (product == null)
                {
                    TempData["Message"] = "Không tìm thấy sản phẩm";
                    return RedirectToAction("Index");
                }

                item = new CartItem
                {
                    IdProduct = product.Id,
                    Img = product.Img,
                    Name = product.Title,
                    Price = product.Price,
                    Rate = product.Rate,
                    Quantity = quantity
                };
                cart.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }

            HttpContext.Session.Set(MyConst.CART_KEY, cart);
            return RedirectToAction("Index");
        }

        public IActionResult ChangeQuantityCart(int id, bool isIncrement = true, int quantity = 1)
        {
            var cart = Cart;
            var item = cart.SingleOrDefault(p => p.IdProduct == id);

            if (item == null)
            {
                return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
            }

            if (isIncrement)
            {
                item.Quantity += quantity;
            }
            else
            {
                item.Quantity -= quantity;
                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
            }

            HttpContext.Session.Set(MyConst.CART_KEY, cart);

            return Json(new
            {
                success = true,
                newQuantity = item.Quantity,
                newTotal = item.Total.ToString("#,##0 VND"),
                cartTotal = cart.Sum(p => p.Total).ToString("#,##0 VND")
            });
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
        public async Task<IActionResult> CheckOut(string paymentMethod, string orderName, string orderDescription, string amount)
        {
            try
            {
                if (Cart.Count == 0)
                {
                    TempData["CheckOutErrorMessage"] = "Không có sản phẩm trong giỏ hàng.";
                    return RedirectToAction("Index");
                }

                string? customerIdStr = HttpContext.User.FindFirstValue("CustomerId");
                if (string.IsNullOrEmpty(customerIdStr))
                {
                    TempData["CheckOutErrorMessage"] = "Vui lòng đăng nhập để thanh toán.";
                    return Redirect("/Customer/SignIn");
                }
                int customerId = Convert.ToInt32(customerIdStr);

                Customer? customer = _customerDAL.GetCustomerById(customerId);
                if (customer == null)
                {
                    TempData["CheckOutErrorMessage"] = "Không tìm thấy thông tin khách hàng.";
                    return Redirect("/404");
                }

                var orderInfo = new OrderInfoModel
                {
                    FullName = orderName,
                    Amount = Cart.Sum(p => p.Total).ToString(),
                    OrderInfo = orderDescription
                };

                if (paymentMethod == "momo")
                {
                    var response = await _momoService.CreatePaymentAsync(orderInfo);
                    if (!string.IsNullOrEmpty(response.PayUrl))
                    {
                        return Redirect(response.PayUrl);
                    }
                    TempData["CheckOutErrorMessage"] = "Không thể tạo liên kết thanh toán qua MoMo.";
                }
                else if (paymentMethod == "vnpay")
                {
                    var paymentUrl = _vnPayService.CreatePaymentUrl(new PaymentInformationModel
                    {
                        Name = customer.FirstName,
                        Amount = double.Parse(orderInfo.Amount),
                        OrderDescription = orderDescription,
                        OrderType = "Giỏ hàng"
                    }, HttpContext);

                    return Redirect(paymentUrl);
                }
                else if (paymentMethod == "cod")
                {
                    bool isSuccess = _cartDAL.CheckOut(customer, Cart);
                    if (isSuccess)
                    {
                        HttpContext.Session.Remove(MyConst.CART_KEY);
                        TempData["CheckOutSuccessMessage"] = "Đặt hàng thành công.";
                        return RedirectToAction("Index");
                    }
                    TempData["CheckOutErrorMessage"] = "Đặt hàng thất bại.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["CheckOutErrorMessage"] = $"Lỗi hệ thống: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public IActionResult PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            return Json(response);
        }

        private string HmacSha512(string key, string inputData)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            return string.Concat(hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData)).Select(b => b.ToString("x2")));
        }

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            var queryData = Request.Query.Keys
                .Where(k => k != "vnp_SecureHash" && k != "vnp_SecureHashType")
                .OrderBy(k => k)
                .Select(k => WebUtility.UrlEncode(k) + "=" + WebUtility.UrlEncode(Request.Query[k]));

            var signData = string.Join("&", queryData);
            return string.Equals(inputHash, HmacSha512(secretKey, signData), StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
