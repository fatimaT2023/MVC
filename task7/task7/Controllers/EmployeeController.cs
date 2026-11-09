using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task7.Data;
using task7.Models;

namespace task7.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Index
        public IActionResult Index()
        {
            var employees = _context.Employees
                .Include(e => e.Orders)
                .ToList();

            return View(employees);
        }

        // 2. Create GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 3. Create POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee7 employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // 4. Edit GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _context.Employees.Find(id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // 5. Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee7 employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // 6. Details
        [HttpGet]
        public IActionResult Details(int id)
        {
            var employee = _context.Employees
                .Include(e => e.Orders)
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // 7. Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var employee = _context.Employees.Find(id);

            if (employee == null)
                return NotFound();

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}