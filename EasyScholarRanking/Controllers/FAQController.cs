using Microsoft.AspNetCore.Mvc;

namespace EasyScholarRanking.Controllers
{
    public class FAQController : Controller
    {
        // GET: /FAQ/
        public IActionResult Index()
        {
            return View();
        }
    }
}
