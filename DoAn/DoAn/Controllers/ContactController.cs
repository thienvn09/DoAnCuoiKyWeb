using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
