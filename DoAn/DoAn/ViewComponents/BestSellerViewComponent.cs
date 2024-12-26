using DoAn.DAL;
using DoAn.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.ViewComponents
{
    public class BestSellerViewComponent : ViewComponent
    {
        ProductDAL dal = new ProductDAL();
        public IViewComponentResult Invoke(int? limit)
        {
            int limitProduct = limit ?? 6; // Số lượng mặc định nếu không truyền tham số
            ProductDAL dal = new ProductDAL();
            List<Product> bestSellerProducts = dal.GetFeaturedProducts(limitProduct);

            return View("BestSeller", bestSellerProducts);
        }

    }
}
