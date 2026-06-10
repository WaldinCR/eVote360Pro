using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.App.Controllers
{
    public class PoliticalLeaderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
