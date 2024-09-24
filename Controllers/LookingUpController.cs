using Microsoft.AspNetCore.Mvc;

namespace ScoreAnnouncementSoftware.Controllers
{
    public class LookingUpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult OptionsExam(string examType)
        {
            if (string.IsNullOrEmpty(examType))
            {
                return RedirectToAction("Index");
            }
            else if (examType == "1")
            {
                return RedirectToAction("EnglishIn");
            }
            else if (examType == "2")
            {
                return RedirectToAction("EnglishOut");
            }
            else
            {
                return RedirectToAction("InfomaticOut");
            }
        }
        public ActionResult EnglishIn()
        {
            return View();
        }
        public ActionResult EnglishOut()
        {
            return View();
        }
        public ActionResult InfomaticOut()
        {
            return View();
        }


    }
}