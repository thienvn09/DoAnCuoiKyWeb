using DoAn.Helper;
using DoAn.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Lấy danh sách giỏ hàng từ session
            var cart = HttpContext.Session.Get<List<CartItem>>(MyConst.CART_KEY) ?? new List<CartItem>();
            
            // Trả về View với CartModel chứa tổng số lượng và tổng giá trị của giỏ hàng
            return View(new CartModel()
            {
                Quantity = cart.Sum(p => p.Quantity),  // Tổng số lượng sản phẩm trong giỏ
                Total = cart.Sum(p => p.Total)         // Tổng tiền của giỏ hàng
            });
        }
    }
}
