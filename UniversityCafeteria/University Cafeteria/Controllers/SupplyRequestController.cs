using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;

namespace University_Cafeteria.Controllers
{
    public class SupplyRequestController : Controller

    {
        private static List<Dictionary<string, string>> confirmedRequests =
    new List<Dictionary<string, string>>();
        //dispaly supply request form
        [HttpGet]
        public IActionResult Index()
        {
            
            string? data =
                HttpContext.Session.GetString("SupplyProducts");

            List<Dictionary<string, string>> products =
                string.IsNullOrEmpty(data)
                ? new List<Dictionary<string, string>>()
                : JsonSerializer.Deserialize<List<Dictionary<string, string>>>(data)
                  ?? new List<Dictionary<string, string>>();

            ViewBag.Products = products;

            // Read saved priority from Cookie
            string? preferredPriority =
                Request.Cookies["PreferredPriority"];

            if (preferredPriority != "Normal" &&
                preferredPriority != "Urgent" &&
                preferredPriority != "Emergency")
            {
                preferredPriority = "Normal";
            }

            ViewBag.PreferredPriority = preferredPriority;

            return View();

            return View();
        }
        //BindingAddress a product to the request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddItem(
    string? productName,
    string? category,
    int? quantity,
    decimal? unitPrice,
    string? priority)
        {
            //validation 
            if (string.IsNullOrWhiteSpace(productName))
            {
                TempData["Error"] = "Product name is required!";
                return RedirectToAction("Index");
            }

            if (quantity == null || quantity <= 0)
            {
                TempData["Error"] = "Quantity must be greater than zero!";
                return RedirectToAction("Index");
            }
            if (unitPrice == null || unitPrice <= 0)
            {
                TempData["Error"] = "Unit Price must be greater than zero!";
                return RedirectToAction("Index");
            }
            if (string.IsNullOrWhiteSpace(category) ||
       !new[] { "Food", "Drinks", "Packaging" }.Contains(category))
            {
                TempData["Error"] = "Please select a valid category!";
                return RedirectToAction("Index");
            }
            if (!new[] { "Normal", "Urgent", "Emergency" }.Contains(priority))
            {
                TempData["Error"] = "Invalid priority!";
                return RedirectToAction("Index");
            }
            //read product from session
            // Get old products from Session
            string? data = HttpContext.Session.GetString("SupplyProducts");

            List<Dictionary<string, string>> products =
                string.IsNullOrEmpty(data)
                ? new List<Dictionary<string, string>>()
                : JsonSerializer.Deserialize<List<Dictionary<string, string>>>(data)
                  ?? new List<Dictionary<string, string>>();

            // Create new product
            Dictionary<string, string> product =
                new Dictionary<string, string>();

            product["ProductName"] = productName!;
            product["Category"] = category!;
            product["Quantity"] = quantity!.Value.ToString();
            product["UnitPrice"] =
                unitPrice!.Value.ToString(CultureInfo.InvariantCulture);
            product["Priority"] = priority!;

            // Add product
            products.Add(product);

            // Save products
            HttpContext.Session.SetString(
                "SupplyProducts",
                JsonSerializer.Serialize(products)
            );

            TempData["Message"] = "Product added successfully!";

            return RedirectToAction("Index");
        }


        //Views_SupplyRequest_Review request before confermation
        // Review request and calculate total cost
        [HttpGet]
        public IActionResult Review()
        {
            // 1. Read products from Session
            string? data = HttpContext.Session.GetString("SupplyProducts");

            List<Dictionary<string, string>> products =
                string.IsNullOrEmpty(data)
                ? new List<Dictionary<string, string>>()
                : JsonSerializer.Deserialize<List<Dictionary<string, string>>>(data)
                  ?? new List<Dictionary<string, string>>();

            // 2. Prevent empty request
            if (products.Count == 0)
            {
                TempData["Error"] = "Cannot review an empty request!";
                return RedirectToAction("Index");
            }

            // 3. Variables for calculations
            decimal subtotal = 0;
            decimal preparationFee = 0;
            decimal discount = 0;

            string selectedPriority = "Normal";

            // 4. Calculate each product total
            foreach (var product in products)
            {
                int quantity = int.Parse(product["Quantity"]);

                decimal unitPrice = decimal.Parse(
                    product["UnitPrice"],
                    CultureInfo.InvariantCulture
                );

                decimal productTotal = quantity * unitPrice;

                product["ProductTotal"] =
                    productTotal.ToString(CultureInfo.InvariantCulture);

                subtotal += productTotal;

                // Determine the highest priority
                if (product["Priority"] == "Emergency")
                {
                    selectedPriority = "Emergency";
                }
                else if (product["Priority"] == "Urgent" &&
                         selectedPriority != "Emergency")
                {
                    selectedPriority = "Urgent";
                }
            }

            // 5. Preparation Fee using switch
            switch (selectedPriority)
            {
                case "Normal":
                    preparationFee = 2;
                    break;

                case "Urgent":
                    preparationFee = 5;
                    break;

                case "Emergency":
                    preparationFee = 10;
                    break;
            }

            // 6. Discount using if / else if
            if (subtotal > 200)
            {
                discount = subtotal * 0.15m;
            }
            else if (subtotal > 100)
            {
                discount = subtotal * 0.10m;
            }

            // 7. Final Total
            decimal finalTotal = subtotal + preparationFee - discount;

            // 8. Send calculation results to View
            ViewBag.Products = products;
            ViewBag.Subtotal = subtotal;
            ViewBag.PreparationFee = preparationFee;
            ViewBag.Discount = discount;
            ViewBag.FinalTotal = finalTotal;
            ViewBag.Priority = selectedPriority;

            return View();
        }
        //DefaultUserConfirmation the supply request

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmRequest()
        {
            // 1. Get draft products from Session
            string? data = HttpContext.Session.GetString("SupplyProducts");

            List<Dictionary<string, string>> products =
                string.IsNullOrEmpty(data)
                ? new List<Dictionary<string, string>>()
                : JsonSerializer.Deserialize<List<Dictionary<string, string>>>(data)
                  ?? new List<Dictionary<string, string>>();

            // 2. Prevent empty requests
            if (products.Count == 0)
            {
                TempData["Error"] = "Cannot confirm an empty request!";
                return RedirectToAction("Index");
            }

            // 3. Calculate Subtotal
            decimal subtotal = 0;
            string selectedPriority = "Normal";

            foreach (var product in products)
            {
                int quantity = int.Parse(product["Quantity"]);

                decimal unitPrice = decimal.Parse(
                    product["UnitPrice"],
                    CultureInfo.InvariantCulture
                );

                decimal productTotal = quantity * unitPrice;

                subtotal += productTotal;

                // Determine highest priority
                if (product["Priority"] == "Emergency")
                {
                    selectedPriority = "Emergency";
                }
                else if (product["Priority"] == "Urgent" &&
                         selectedPriority != "Emergency")
                {
                    selectedPriority = "Urgent";
                }
            }

            // 4. Calculate Preparation Fee
            decimal preparationFee = 0;

            switch (selectedPriority)
            {
                case "Normal":
                    preparationFee = 2;
                    break;

                case "Urgent":
                    preparationFee = 5;
                    break;

                case "Emergency":
                    preparationFee = 10;
                    break;
            }

            // 5. Calculate Discount
            decimal discount = 0;

            if (subtotal > 200)
            {
                discount = subtotal * 0.15m;
            }
            else if (subtotal > 100)
            {
                discount = subtotal * 0.10m;
            }

            // 6. Calculate Final Total
            decimal finalTotal =
                subtotal + preparationFee - discount;

            // 7. Create confirmed request
            Dictionary<string, string> request =
                new Dictionary<string, string>();

            lock (confirmedRequests)
            {
                request["RequestID"] =
                    (confirmedRequests.Count + 1).ToString();

                request["NumberOfProducts"] = products.Count.ToString();

                request["Subtotal"] =
                    subtotal.ToString(CultureInfo.InvariantCulture);

                request["PreparationFee"] =
                    preparationFee.ToString(CultureInfo.InvariantCulture);

                request["Discount"] =
                    discount.ToString(CultureInfo.InvariantCulture);

                request["FinalTotal"] =
                    finalTotal.ToString(CultureInfo.InvariantCulture);

                request["Status"] = "Confirmed";

                // 8. Save request in List
                confirmedRequests.Add(request);
            }

            // 9. Clear draft after confirmation
            HttpContext.Session.Remove("SupplyProducts");

            TempData["Message"] = "Request confirmed successfully!";

            return RedirectToAction("MyRequests");
        }
        //clear current draft
        // Clear current draft from Session
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClearDraft()
        {
            HttpContext.Session.Remove("SupplyProducts");

            TempData["Message"] = "Draft cleared successfully!";

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SavePreference(string? priority)
        {
            if (priority != "Normal" &&
                priority != "Urgent" &&
                priority != "Emergency")
            {
                TempData["Error"] = "Invalid priority!";
                return RedirectToAction("Index");
            }

            CookieOptions options = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                IsEssential = true,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            };

            Response.Cookies.Append(
                "PreferredPriority",
                priority,
                options
            );

            TempData["Message"] = "Preference saved successfully!";

            return RedirectToAction("Index");
        }
        //dispaly previous supply requests
        [HttpGet]
        public IActionResult MyRequests()
        {
            List<Dictionary<string, string>> requests;

            lock (confirmedRequests)
            {
                requests = confirmedRequests
                    .Select(request =>
                        new Dictionary<string, string>(request))
                    .ToList();
            }

            ViewBag.Requests = requests;

            return View();
        }
        //dispaly infomation about the sys
        [HttpGet]
        public IActionResult About()
        {
            return View();
        }
    }
}
