using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task7.Data;
using task7.Models;

namespace task7.Controllers
{
    public class CategorynewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategorynewController(ApplicationDbContext context)
        {
            _context = context;
        }

        //  Display all categories
        public IActionResult Index()
        {
            var categories = _context.Categories
                .Include(c => c.Products)
                .ToList();

            return View(categories);
        }

        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //  Save category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category7 category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        //  Display selected category
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // ave changes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category7 category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        //  Display one category
        [HttpGet]
        public IActionResult Details(int id)
        {
            var category = _context.Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //Delete selected category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}