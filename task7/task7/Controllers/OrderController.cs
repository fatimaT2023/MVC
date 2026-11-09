using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using task7.Data;
using task7.Models;

namespace task7.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Index
        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.Employee)
                .ToList();

            return View(orders);
        }

        // 2. Create GET
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Employees = new SelectList(
                _context.Employees, "Id", "FullName");

            return View();
        }

        // 3. Create POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // 4. Edit GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
                return NotFound();

            ViewBag.Employees = new SelectList(
                _context.Employees,
                "Id", "FullName", order.EmployeeId);

            return View(order);
        }

        // 5. Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // 6. Details
        [HttpGet]
        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .Include(o => o.Employee)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // 7. Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}