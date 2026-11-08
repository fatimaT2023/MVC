using Microsoft.AspNetCore.Mvc;
using task6.Data;
using task6.Models;

namespace task6.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupplierController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display all suppliers
        public IActionResult Index()
        {
            var suppliers = _context.Suppliers.ToList();

            return View(suppliers);
        }

        // Display Create form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Save new supplier
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Suppliers.Add(supplier);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(supplier);
        }
    }
}