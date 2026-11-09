using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using task6.Data;
using task6.Models;

namespace task6.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display all products
        public IActionResult Index()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .ToList();

            return View(products);
        }

        // Display Create form
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(
                _context.Categories.ToList(), "Id", "Name");

            ViewBag.Suppliers = new SelectList(
                _context.Suppliers.ToList(), "Id", "Name");

            return View();
        }

        // Save new product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            ModelState.Remove("Category");
            ModelState.Remove("Supplier");

            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(
                _context.Categories.ToList(), "Id", "Name");

            ViewBag.Suppliers = new SelectList(
                _context.Suppliers.ToList(), "Id", "Name");

            return View(product);
        }
    }
}