using Microsoft.AspNetCore.Mvc;

namespace University_Cafeteria.Controllers
{
    public class SupplyController : Controller
    {
        public IActionResult Index()
        {
            string[] categories = {    "Drinks",
        "Food",
        "Snacks",
        "Hot Drinks",
        "Cold Drinks"};

            ViewBag.Categories = categories;
            return View();
        }
        public IActionResult Products()
        {
            ViewData["Page Title"] = "Cafeteria  Products";
            ViewData["Manager Name"] = "Ahmad Ali";
            ViewData["Number of Products"] = 5;

            string[] products = { "Water", "Orange Juice", "Coffee", "Sandwich", "Chocolate" };
            ViewData["Products"] = products;
            return View();
        }
        public IActionResult Suppliers()
        {
            ViewBag.PageTitle = "Our Suppliers";
            ViewBag.Number_of_Suppliers = 3;
            Array suppliers = new string[] { "Fresh Food Company", "Happy Drinks Company", "Daily Supplies Company" };
            ViewBag.Suppliers = suppliers;
            return View();
        }public IActionResult SupplyStatus()
        {
            ViewData["Page Title"] = "Supply Status";
            ViewData["Manager Name"] = "Ahmad Ali";
            ViewData["Number of Products"] = 5;
            string[] products = { "Water", "Orange Juice", "Coffee", "Sandwich", "Chocolate" };
            ViewData["Products"] = products;
            return View();
        }
        [HttpGet]
        public IActionResult CreateRequest()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SubmitRequest()
        {
            string employeeName = Request.Form["EmployeeName"].ToString();
            string productName = Request.Form["ProductName"].ToString();
            string quantity = Request.Form["Quantity"].ToString();
            string requiredDate = Request.Form["RequiredDate"].ToString();

            if (string.IsNullOrWhiteSpace(employeeName))
            {
                ViewBag.Message = "Error: Employee Name is required!";
            }
            TempData["Message"] = $"Success: Request for {quantity} - {productName} " +
        $"submitted by {employeeName} (required by {requiredDate}).";
            return RedirectToAction("About");
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult CheckQuantity(int quantity)
        {
            if (quantity < 20)
            {
                ViewBag.Message = "Small Order";

            }
            else if (quantity <=50)
            {
                ViewBag.Message = "Medium Order";
            }
            else
            {
                ViewBag.Message = "Large Order";
            }
            {
                ViewBag.Message = "Quantity is sufficient.";
            }
            ViewBag.Quantity = quantity;
            return View();

        }
    }
}
