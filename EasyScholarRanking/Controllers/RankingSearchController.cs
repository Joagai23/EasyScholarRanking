using Microsoft.AspNetCore.Mvc;

namespace EasyScholarRanking.Controllers
{
    public class RankingSearchController : Controller
    {

        // GET: /RankingSearch/
        public IActionResult Index()
        {
            return View();
        }
    }
}
