using Microsoft.AspNetCore.Mvc;
using task6.Data;
using task6.Models;

namespace task6.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context; //database

        public CategoryController(ApplicationDbContext context) //constructor(depandancy injection)
        {
            _context = context;  //DI
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList(); //to show all categories from database  select * from catirgiories

            return View(categories);  //return to view with categories
        }

        [HttpGet]
        public IActionResult Create()   //to show create form  بحط بس فورم داخله 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category) // رح تستقبل بيانات من الكايقوري من الاوبجكت الي اسمه كاتيقوري
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges(); //save to database

                return RedirectToAction("Index");
            }

            return View(category);
        }
    }
}