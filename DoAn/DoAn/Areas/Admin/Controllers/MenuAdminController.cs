using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DoAn.Areas.Admin.DAL;
using DoAn.Models;
using DoAn.Areas.Admin.Models;
using DoAn.DAL;

namespace DoAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class MenuAdminController : Controller
    {
        private readonly MenuAdminDAL _menuAdminDAL = new MenuAdminDAL();
       

        // GET: MenuAdminController
        public ActionResult Index()
        {
         List<MenuAdmin> menus = _menuAdminDAL.GetAllMenus();
            return View(menus);
        }


        // GET: MenuAdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MenuAdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MenuAdminController/Create
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

        // GET: MenuAdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MenuAdminController/Edit/5
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

        // GET: MenuAdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MenuAdminController/Delete/5
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
