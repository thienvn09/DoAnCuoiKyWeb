using DoAn.DAL;
using DoAn.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers
{
    public class ProductController : Controller
    {
        ProductDAL productDAL = new ProductDAL();
        public IActionResult Index(int? idCategory, int page = 1, string sortOrder = "")
        {
            // Khai báo số lượng Row trong 1 trang
            int pageSize = 5;

            // Lưu Id Category vào ViewData
            ViewData["IdCategory"] = idCategory;
            // Lưu Cột được sắp xếp vào ViewData
            ViewData["SortColumn"] = sortOrder;

            // Lấy danh sách sản phẩm sau khi phân trang
            List<Product> products = new List<Product>();
            products = productDAL.GetProducts_Pagination(idCategory, page, pageSize, sortOrder);

            // Lấy tổng số lượng sản phẩm cho phân trang
            int rowCount = productDAL.GetListProduct(idCategory).Count();

            // Tính toán số lượng trang
            double pageCount = (double)rowCount / pageSize;
            int maxPage = (int)Math.Ceiling(pageCount);

            // Tạo đối tượng ProductPagination để trả về view
            ProductPagination model = new ProductPagination();
            model.Products = products;
            model.CurrentPageIndex = page;
            model.PageCount = maxPage;

            return View(model);
        }


        //View Detail
        public IActionResult Detail(int id)
        {
            Product product = new Product();
            product = productDAL.GetProductById(id);
            return View(product);
        }
    }
}
