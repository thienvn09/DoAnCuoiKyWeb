using DoAn.DAL;
using DoAn.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.ViewComponents
{
    public class MenuCategoryViewComponent : ViewComponent
    {
        CategoryDAL categoryDAL = new CategoryDAL();
        public IViewComponentResult Invoke()
        {
            List<CategoryMenu> categoryMenus = new List<CategoryMenu>();
            categoryMenus = categoryDAL.getAllWithCount();
            return View("Default", categoryMenus);
        }
    }
}
