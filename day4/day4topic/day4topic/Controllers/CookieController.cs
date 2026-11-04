using Microsoft.AspNetCore.Mvc;

namespace day4topic.Controllers
{
    public class CookieController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Username = Request.Cookies["Username"];

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(string? username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                TempData["Error"] = "Please enter your username";
                return RedirectToAction("Index");
            }

            CookieOptions options = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            };

            Response.Cookies.Append(
                "Username",
                username,
                options
            );

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete()
        {
            Response.Cookies.Delete("Username");
            return RedirectToAction("Index");
        }
    }
}
        
        
     
            