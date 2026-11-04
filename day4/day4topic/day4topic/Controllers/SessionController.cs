using Microsoft.AspNetCore.Mvc;

namespace day4topic.Controllers
{
    public class SessionController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Name=HttpContext.Session.GetString("Username");

            ViewBag.Age = HttpContext.Session.GetInt32("Age");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(string? name, int age)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["Error"] = "Please enter your name";
                return RedirectToAction("Index");
            }

            HttpContext.Session.SetString("Username", name);
            HttpContext.Session.SetInt32("Age", age);

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("Age");
            return RedirectToAction("Index");
        }
    }
}
