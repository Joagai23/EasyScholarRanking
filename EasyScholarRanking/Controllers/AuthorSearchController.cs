using Microsoft.AspNetCore.Mvc;

namespace EasyScholarRanking.Controllers
{
    public class AuthorSearchController : Controller
    {

        // GET: /AuthorSearch/
        public IActionResult Index()
        {
            return View();
        }
    }
}
