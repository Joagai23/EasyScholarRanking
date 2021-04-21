using Microsoft.AspNetCore.Mvc;

namespace EasyScholarRanking.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
