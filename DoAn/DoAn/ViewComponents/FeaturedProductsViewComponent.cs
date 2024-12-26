using DoAn.DAL;
using DoAn.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.ViewComponents
{
    public class FeaturedProductsViewComponent:ViewComponent
    {
        ProductDAL productDAL = new ProductDAL();
        public IViewComponentResult Invoke(int? limit)
        {
            int limitProduct = limit ?? 4;
            List<Product> featuredProducts = new List<Product>();
            featuredProducts = productDAL.GetFeaturedProducts(limitProduct);
            return View("FeatureProduct", featuredProducts);
        }

    }
}
