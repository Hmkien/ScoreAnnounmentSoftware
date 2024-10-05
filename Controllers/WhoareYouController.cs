using Microsoft.AspNetCore.Mvc;

namespace ScoreAnnouncementSoftware.Controllers
{
    public class WhoareYouController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}