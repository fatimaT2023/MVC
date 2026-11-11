
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task8.Data;
using task8.Models;

namespace task8.Controllers
{
    [Authorize]
    public class SupplyRequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SupplyRequestController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private bool IsAdmin()
        {
            return User.IsInRole("Admin") ||
                   User.IsInRole("SuperAdmin");
        }

        // Show requests
        public async Task<IActionResult> Index()
        {
            var requests = _context.SupplyRequests
                .Include(r => r.User)
                .AsQueryable();

            if (!IsAdmin())
            {
                var userId = _userManager.GetUserId(User);
                requests = requests.Where(r => r.UserId == userId);
            }

            return View(await requests.ToListAsync());
        }

        // Create GET
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Products = await _context.Products.ToListAsync();
            return View();
        }

        // Create POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplyRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = await _context.Products.ToListAsync();
                return View(dto);
            }

            var selected = dto.Items
                .Where(i => i.Quantity > 0)
                .ToList();
            if (selected.Select(i => i.ProductId).Distinct().Count()
    != selected.Count)
            {
                ModelState.AddModelError("", "Duplicate products are not allowed.");
                ViewBag.Products = await _context.Products.ToListAsync();
                return View(dto);
            }
            if (!selected.Any())
            {
                ModelState.AddModelError("", "Select at least one product.");
                ViewBag.Products = await _context.Products.ToListAsync();
                return View(dto);
            }

            var request = new SupplyRequest
            {
                RequestDate = DateTime.Now,
                Status = "Pending",
                UserId = _userManager.GetUserId(User)!
            };

            foreach (var item in selected)
            {
                var product = await _context.Products
                    .FindAsync(item.ProductId);

                if (product == null)
                {
                    ModelState.AddModelError("", "Product not found.");
                    ViewBag.Products = await _context.Products.ToListAsync();
                    return View(dto);
                }

                if (item.Quantity > product.Quantity)
                {
                    ModelState.AddModelError(
                        "", $"Not enough quantity for {product.Name}");

                    ViewBag.Products = await _context.Products.ToListAsync();
                    return View(dto);
                }

                request.RequestItems.Add(new RequestItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    Price = product.Price
                });

                request.TotalPrice += product.Price * item.Quantity;
            }

            _context.SupplyRequests.Add(request);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Details
        public async Task<IActionResult> Details(int id)
        {
            var request = await _context.SupplyRequests
                .Include(r => r.RequestItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);

            if (!IsAdmin() && request.UserId != userId)
                return Forbid();

            return View(request);
        }

        // Edit GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var request = await _context.SupplyRequests
                .Include(r => r.RequestItems)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
                return NotFound();

            if (!IsAdmin() &&
                request.UserId != _userManager.GetUserId(User))
                return Forbid();

            if (request.Status != "Pending")
                return BadRequest("Only pending requests can be edited.");

            ViewBag.Products = await _context.Products.ToListAsync();

            return View(new SupplyRequestDTO
            {
                Items = request.RequestItems.Select(i => new RequestItemDTO
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            });
        }

        // Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SupplyRequestDTO dto)
        {
            var request = await _context.SupplyRequests
                .Include(r => r.RequestItems)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
                return NotFound();

            if (!IsAdmin() &&
                request.UserId != _userManager.GetUserId(User))
                return Forbid();

            if (request.Status != "Pending")
                return BadRequest("Only pending requests can be edited.");

            if (!ModelState.IsValid)
            {
                ViewBag.Products = await _context.Products.ToListAsync();
                return View(dto);
            }

            var selected = dto.Items.Where(i => i.Quantity > 0).ToList();

            if (!selected.Any() ||
                selected.Select(i => i.ProductId).Distinct().Count() != selected.Count)
            {
                ModelState.AddModelError("", "Select valid products without duplicates.");
                ViewBag.Products = await _context.Products.ToListAsync();
                return View(dto);
            }

            var newItems = new List<RequestItem>();
            decimal total = 0;

            foreach (var item in selected)
            {
                var product = await _context.Products.FindAsync(item.ProductId);

                if (product == null || item.Quantity > product.Quantity)
                {
                    ModelState.AddModelError("", "Invalid product or quantity.");
                    ViewBag.Products = await _context.Products.ToListAsync();
                    return View(dto);
                }

                newItems.Add(new RequestItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    Price = product.Price
                });

                total += product.Price * item.Quantity;
            }

            _context.RequestItems.RemoveRange(request.RequestItems);
            request.RequestItems = newItems;
            request.TotalPrice = total;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Delete GET
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _context.SupplyRequests.FindAsync(id);

            if (request == null)
                return NotFound();

            if (!IsAdmin() &&
                request.UserId != _userManager.GetUserId(User))
                return Forbid();

            if (request.Status != "Pending")
                return BadRequest("Only pending requests can be deleted.");

            return View(request);
        }

        // Delete POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.SupplyRequests
                .Include(r => r.RequestItems)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
                return NotFound();

            if (!IsAdmin() &&
                request.UserId != _userManager.GetUserId(User))
                return Forbid();

            if (request.Status != "Pending")
                return BadRequest("Only pending requests can be deleted.");

            _context.RequestItems.RemoveRange(request.RequestItems);
            _context.SupplyRequests.Remove(request);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Admin approves or rejects
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, string status)
        {
            if (status != "Approved" && status != "Rejected")
                return BadRequest();

            var request = await _context.SupplyRequests.FindAsync(id);

            if (request == null)
                return NotFound();

            if (request.Status != "Pending")
                return BadRequest("Request already processed.");

            request.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
