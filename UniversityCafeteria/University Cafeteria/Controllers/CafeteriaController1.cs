using Microsoft.AspNetCore.Mvc;

namespace University_Cafeteria.Controllers
{
    public class CafeteriaController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Products()
        {
            return View();
        }
        public IActionResult Suppliers()
        {
            return View();
        }
        public IActionResult SupplyStatus()
        {
            return View();
        }
    }
}
