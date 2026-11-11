
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task8.Data;
using task8.Models;

namespace task8.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // 1. Display all users
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var model = new List<UserRoleDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                model.Add(new UserRoleDTO
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email ?? "",
                    Role = roles.FirstOrDefault() ?? "User"
                });
            }

            return View(model);
        }

        // 2. Edit GET
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var model = new UserRoleDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? "",
                Role = roles.FirstOrDefault() ?? "User"
            };

            return View(model);
        }

        // 3. Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserRoleDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string[] allowedRoles = { "User", "Admin", "SuperAdmin" };

            if (!allowedRoles.Contains(model.Role))
                return BadRequest("Invalid role.");

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                return NotFound();

            // Prevent changing your own role
            if (_userManager.GetUserId(User) == user.Id)
                return BadRequest("You cannot change your own role.");

            var oldRoles = await _userManager.GetRolesAsync(user);

            if (oldRoles.Count == 1 && oldRoles[0] == model.Role)
                return RedirectToAction("Index");

            var removeResult = await _userManager.RemoveFromRolesAsync(
                user, oldRoles);

            if (!removeResult.Succeeded)
            {
                foreach (var error in removeResult.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            var addResult = await _userManager.AddToRoleAsync(
                user, model.Role);

            if (!addResult.Succeeded)
            {
                // Restore old roles if adding the new role fails
                await _userManager.AddToRolesAsync(user, oldRoles);

                foreach (var error in addResult.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            return RedirectToAction("Index");
        }

        // 4. Delete GET
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            if (_userManager.GetUserId(User) == user.Id)
                return BadRequest("You cannot delete your own account.");

            return View(user);
        }

        // 5. Delete POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            // Prevent deleting your own account
            if (_userManager.GetUserId(User) == user.Id)
                return BadRequest("You cannot delete your own account.");

            // Prevent deleting users with supply requests
            bool hasRequests = await _context.SupplyRequests
                .AnyAsync(r => r.UserId == user.Id);

            if (hasRequests)
            {
                ModelState.AddModelError(
                    "",
                    "Cannot delete a user who has supply requests."
                );

                return View("Delete", user);
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View("Delete", user);
            }

            return RedirectToAction("Index");
        }
    }
}
