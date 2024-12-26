using DoAn.Areas.Admin.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class HomeController : Controller
    {
        private readonly HomeAdminDAL _homeAdminDAL;

        public HomeController()
        {
            _homeAdminDAL = new HomeAdminDAL();
        }

        public IActionResult Index()
        {
            int year = DateTime.Now.Year;

            // Lấy tổng doanh thu cho tháng hiện tại
            decimal monthlyRevenue = _homeAdminDAL.GetMonthlyRevenue(year, DateTime.Now.Month);
            ViewBag.MonthlyRevenue = monthlyRevenue;

            // Lấy tổng số thanh toán
            int totalPayments = _homeAdminDAL.GetTotalPayments();
            ViewBag.TotalPayments = totalPayments;

            // Lấy tổng số sản phẩm
            int totalProducts = _homeAdminDAL.GetTotalProducts();
            ViewBag.TotalProducts = totalProducts;

            // Lấy tổng số tài khoản
            int totalCustomers = _homeAdminDAL.GetTotalCustomers();
            ViewBag.TotalCustomers = totalCustomers;

            // Lấy doanh thu theo tháng cho biểu đồ
            ViewBag.RevenueMonth1 = _homeAdminDAL.GetMonthlyRevenue(year, 1);
            ViewBag.RevenueMonth2 = _homeAdminDAL.GetMonthlyRevenue(year, 2);
            ViewBag.RevenueMonth3 = _homeAdminDAL.GetMonthlyRevenue(year, 3);
            ViewBag.RevenueMonth4 = _homeAdminDAL.GetMonthlyRevenue(year, 4);
            ViewBag.RevenueMonth5 = _homeAdminDAL.GetMonthlyRevenue(year, 5);
            ViewBag.RevenueMonth6 = _homeAdminDAL.GetMonthlyRevenue(year, 6);
            ViewBag.RevenueMonth7 = _homeAdminDAL.GetMonthlyRevenue(year, 7);
            ViewBag.RevenueMonth8 = _homeAdminDAL.GetMonthlyRevenue(year, 8);
            ViewBag.RevenueMonth9 = _homeAdminDAL.GetMonthlyRevenue(year, 9);
            ViewBag.RevenueMonth10 = _homeAdminDAL.GetMonthlyRevenue(year, 10);
            ViewBag.RevenueMonth11 = _homeAdminDAL.GetMonthlyRevenue(year, 11);
            ViewBag.RevenueMonth12 = _homeAdminDAL.GetMonthlyRevenue(year, 12);

            return View();
        
        
        }
    }
}
