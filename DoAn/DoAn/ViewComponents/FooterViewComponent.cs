using Microsoft.AspNetCore.Mvc;

namespace DoAn.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
       
            public IViewComponentResult Invoke()
            {
                // Trả về view mặc định (chúng ta sẽ sử dụng view "Default.cshtml")
                return View("Default");
            }
        
    }
}
