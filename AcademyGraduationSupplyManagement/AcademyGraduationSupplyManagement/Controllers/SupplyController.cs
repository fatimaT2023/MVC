
using AcademyGraduationSupplyManagement.Data;
using AcademyGraduationSupplyManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AcademyGraduationSupplyManagement.Controllers
{
    public class SupplyController : Controller
    {
        
        // PHASE 3 - Dependency Injection
   

        private readonly AppDbContext _context;

        public SupplyController(AppDbContext context)
        {
            _context = context;
        }

       
        // INDEX - Display All Offers
        

        [HttpGet]
        public IActionResult Index()
        {
            List<Offer> offers = GetOffers();

            return View(offers);
        }

        /*
       
        PHASE 1 & 2 - OLD GETOFFERS
      
      

        private List<Offer> GetOffers()
        {
            return new List<Offer>
            {
                new Offer
                {
                    Id = 1,
                    Category = "Graduation Certificates",
                    SupplierName = "Supplier A",
                    OfferName = "Premium Certificate Package",
                    Quantity = 35,
                    UnitPrice = 2m
                },
                new Offer
                {
                    Id = 2,
                    Category = "Graduation Certificates",
                    SupplierName = "Supplier B",
                    OfferName = "Standard Certificate Package",
                    Quantity = 35,
                    UnitPrice = 1.5m
                },
                new Offer
                {
                    Id = 3,
                    Category = "Graduation Certificates",
                    SupplierName = "Supplier C",
                    OfferName = "Luxury  Certificate Package",
                    Quantity = 35,
                    UnitPrice = 2.5m
                },
                new Offer
                {
                    Id = 4,
                    Category = "Hospitality",
                    SupplierName = "Supplier D",
                    OfferName = "Coffee & Drinks Package",
                    Quantity = 100,
                    UnitPrice = 2m
                },
                new Offer
                {
                    Id = 5,
                    Category = "Hospitality",
                    SupplierName = "Supplier E",
                    OfferName = "Sandwiches & Snacks Packag",
                    Quantity = 100,
                    UnitPrice = 3m
                },
                new Offer
                {
                    Id = 6,
                    Category = "Hospitality",
                    SupplierName = "Supplier F",
                    OfferName = "Full Meal Package",
                    Quantity = 100,
                    UnitPrice = 4m
                },
                new Offer
                {
                    Id = 7,
                    Category = "Graduation Gifts",
                    SupplierName = "Supplier G",
                    OfferName = "Customized Mugs Package",
                    Quantity = 1,
                    UnitPrice = 100m
                },
                new Offer
                {
                    Id = 8,
                    Category = "Graduation Gifts",
                    SupplierName = "Supplier H",
                    OfferName = "Graduation Bags Package",
                    Quantity = 35,
                    UnitPrice = 3m
                },
                new Offer
                {
                    Id = 9,
                    Category = "Graduation Gifts",
                    SupplierName = "Supplier I",
                    OfferName = "Premium Gift Boxes Package",
                    Quantity = 35,
                    UnitPrice = 4m
                }
            };
        }

     
        */

      
        // PHASE 3 - Get Offers from Database
     

        private List<Offer> GetOffers()
        {
            return _context.Offers
                .Include(o => o.Category)
                .Include(o => o.Supplier)
                .AsNoTracking()
                .ToList();
        }

        // SELECT OFFERS - POST
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectOffers(
            Dictionary<string, int> selectedOffers)
        {
            // Validate all three categories
            if (selectedOffers.Count != 3 ||
                !selectedOffers.ContainsKey("Graduation Certificates") ||
                !selectedOffers.ContainsKey("Hospitality") ||
                !selectedOffers.ContainsKey("Graduation Gifts"))
            {
                return RedirectToAction("Index");
            }

            /*
        
            PHASE 1 & 2 - OLD ID VALIDATION
          

            int certificateId =
                selectedOffers["Graduation Certificates"];

            int hospitalityId =
                selectedOffers["Hospitality"];

            int giftId =
                selectedOffers["Graduation Gifts"];

            if (certificateId < 1 || certificateId > 3 ||
                hospitalityId < 4 || hospitalityId > 6 ||
                giftId < 7 || giftId > 9)
            {
                return RedirectToAction("Index");
            }

            */

            // PHASE 3 - Validate IDs using Database
            // Do not assume fixed ID ranges

            List<int> chosenOfferIds =
                selectedOffers.Values.ToList();

            List<Offer> chosenOffers = _context.Offers
                .Where(o => chosenOfferIds.Contains(o.Id))
                .Include(o => o.Category)
                .AsNoTracking()
                .ToList();

            if (chosenOffers.Count != 3)
            {
                return RedirectToAction("Index");
            }

            // Check that each selected offer belongs
            // to the category submitted by the form

            foreach (var selected in selectedOffers)
            {
                bool valid = chosenOffers.Any(o =>
                    o.Id == selected.Value &&
                    o.Category.Name == selected.Key);

                if (!valid)
                {
                    return RedirectToAction("Index");
                }
            }

            /*
          
            PHASE 1 - OLD SESSION CODE
            Save each ID separately

            HttpContext.Session.SetInt32(
                "SelectedCertificate",
                selectedOffers["Graduation Certificates"]);

            HttpContext.Session.SetInt32(
                "SelectedHospitality",
                selectedOffers["Hospitality"]);

            HttpContext.Session.SetInt32(
                "SelectedGift",
                selectedOffers["Graduation Gifts"]);

            
            */

            /*
           
            PHASE 2 - OLD SESSION CODE
            Save List<Offer> as JSON
            
            List<Offer> offers = GetOffers();

            List<Offer> chosenOffers = offers
                .Where(o => selectedOffers.ContainsValue(o.Id))
                .ToList();

            string json =
                JsonSerializer.Serialize(chosenOffers);

            HttpContext.Session.SetString(
                "SelectedOffers", json);

      
            */

           
            // PHASE 3 - Save List<int> in Session

            string json =
                JsonSerializer.Serialize(chosenOfferIds);

            HttpContext.Session.SetString(
                "SelectedOffers", json);

            return Content(
                "Selected offers saved successfully!");
        }

     
      

        /*
       
        PHASE 1 - OLD CHECKSESSION
        [HttpGet]
        public IActionResult CheckSession()
        {
            int? certificateId =
                HttpContext.Session.GetInt32(
                    "SelectedCertificate");

            int? hospitalityId =
                HttpContext.Session.GetInt32(
                    "SelectedHospitality");

            int? giftId =
                HttpContext.Session.GetInt32(
                    "SelectedGift");

            if (certificateId == null ||
                hospitalityId == null ||
                giftId == null)
            {
                return Content("Some offers are missing!");
            }

            return Content(
                $"Certificate ID: {certificateId}, " +
                $"Hospitality ID: {hospitalityId}, " +
                $"Gift ID: {giftId}");
        }

        */

        // PHASE 3 - Check List<int> in Session

        [HttpGet]
        public IActionResult CheckSession()
        {
            string? json =
                HttpContext.Session.GetString(
                    "SelectedOffers");

            if (string.IsNullOrEmpty(json))
            {
                return Content("Some offers are missing!");
            }

            List<int>? ids;

            try
            {
                ids = JsonSerializer.Deserialize<List<int>>(json);
            }
            catch (JsonException)
            {
                return Content("Invalid Session data!");
            }

            if (ids == null || ids.Count != 3)
            {
                return Content("Some offers are missing!");
            }

            return Content(
                $"Selected Offer IDs: {string.Join(", ", ids)}");
        }

       

        [HttpGet]
        public IActionResult Review()
        {
            // Read selected IDs from Session
            string? json =
                HttpContext.Session.GetString(
                    "SelectedOffers");

            if (string.IsNullOrEmpty(json))
            {
                return RedirectToAction("Index");
            }

            /*
           
            List<Offer>? selectedOffers =
                JsonSerializer.Deserialize<List<Offer>>(json);

           
            */

            // PHASE 3 - Deserialize List<int>
            List<int>? selectedIds;

            try
            {
                selectedIds =
                    JsonSerializer.Deserialize<List<int>>(json);
            }
            catch (JsonException)
            {
                return RedirectToAction("Index");
            }

            if (selectedIds == null ||
                selectedIds.Count != 3 ||
                selectedIds.Distinct().Count() != 3)
            {
                return RedirectToAction("Index");
            }

            // Get selected offers from Database
            List<Offer> selectedOffers = _context.Offers
                .Where(o => selectedIds.Contains(o.Id))
                .Include(o => o.Category)
                .Include(o => o.Supplier)
                .AsNoTracking()
                .ToList();

            // Validate selected offers
            if (selectedOffers.Count != 3 ||
                selectedOffers
                    .Select(o => o.CategoryId)
                    .Distinct()
                    .Count() != 3)
            {
                return RedirectToAction("Index");
            }

            

            Dictionary<int, decimal> offerTotals =
                new Dictionary<int, decimal>();

            foreach (var offer in selectedOffers)
            {
                decimal total =
                    offer.Quantity * offer.UnitPrice;

                offerTotals.Add(offer.Id, total);
            }

            ViewBag.OfferTotals = offerTotals;

            decimal totalPrice =
                offerTotals.Values.Sum();

            decimal discountPercentage = 0;

            if (totalPrice > 500)
            {
                discountPercentage = 0.15m;
            }
            else if (totalPrice > 300)
            {
                discountPercentage = 0.10m;
            }

            decimal discountAmount =
                totalPrice * discountPercentage;

            decimal finalTotal =
                totalPrice - discountAmount;

            ViewBag.TotalPrice = totalPrice;

            ViewBag.DiscountPercentage =
                discountPercentage * 100;

            ViewBag.DiscountAmount = discountAmount;

            ViewBag.FinalTotal = finalTotal;

            return View(selectedOffers);
        }
    }
}
