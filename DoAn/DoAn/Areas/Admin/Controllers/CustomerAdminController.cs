using DoAn.Areas.Admin.DAL;
using DoAn.Areas.Admin.Models;
using DoAn.Areas.DAL;
using DoAn.DAL;
using DoAn.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class CustomerAdminController : Controller
    {
        CustomerAdminDAL CustomerAdminDAL = new CustomerAdminDAL();
        // GET: CustomerAdminController
        public ActionResult Index()
        {
            List<Customer> customers = new List<Customer>();
            customers = CustomerAdminDAL.getAll();
            return View(customers);
        }

        // GET: CustomerAdminController/Details/5
        public ActionResult Details(int id)
        {
            // Lấy Thông tin Product từ Id
            Customer customer = new Customer();
            customer = CustomerAdminDAL.GetCustomerById(id);

            return View(customer);
        }

        // GET: CustomerAdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerAdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerAdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CustomerAdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerAdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CustomerAdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
