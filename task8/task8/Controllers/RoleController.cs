using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace task8.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();// بجيب قائمه الادوار من الداتا بيز

            return View(roles);
        }
        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                ViewBag.Error = "Role name is required";
                return View();
            }

            if (await _roleManager.RoleExistsAsync(roleName))
            {
                ViewBag.Error = "Role already exists";
                return View();
            }

            var result = await _roleManager.CreateAsync(
                new IdentityRole(roleName)
            );

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Failed to create role";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDefaultRoles()
        {
            string[] roles = { "User", "Admin", "SuperAdmin" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(
                        new IdentityRole(role)
                    );
                }
            }

            return RedirectToAction("Index");
        }
    }
}