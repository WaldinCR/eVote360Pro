using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.App.Controllers.Leader
{
    public class CandidateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
