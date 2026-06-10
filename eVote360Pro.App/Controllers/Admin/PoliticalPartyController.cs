using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.App.Controllers
{
    public class PoliticalPartyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
