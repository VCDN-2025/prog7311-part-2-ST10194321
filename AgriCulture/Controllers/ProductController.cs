using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AgriCulture.Models;
using AgriCulture.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AgriCulture.Controllers
{
    // Controller responsible for managing agricultural products, including CRUD operations
    // and product listing with filtering capabilities
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;  

        public ProductController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,  IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;  
        }

        // Displays a list of products for the currently logged-in farmer
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var farmer = _context.Farmers.FirstOrDefault(f => f.Email == user.Email);
            if (farmer == null)
                return Unauthorized();
            var products = _context.Products.Where(p => p.FarmerId == farmer.Id).ToList();
            return View(products);
        }
        //Title: Pro C 7 with.NET and .NET Core 
//Author: Andrew Troelsen; Philip Japikse 
// Date: 2017 
// Code version: Version 1 
//Availability: Textbook / Ebook

        // Displays the form to create a new product
        public IActionResult Create()
        {
            return View();
        }

        // Handles the creation of a new product, including image upload and validation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model, IFormFile ImageUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            var farmer = _context.Farmers.FirstOrDefault(f => f.Email == user.Email);
            if (farmer == null)
            {
                ModelState.AddModelError("", $"Farmer profile not found for user email: {user.Email}");
                return View(model);
            }

            // Allow image to be optional
            if (ImageUrl == null || ImageUrl.Length == 0)
            {
                ModelState.Remove("ImageUrl");
            }

            if (ImageUrl != null && ImageUrl.Length > 0)
            {
                // ensure /uploads folder exists under wwwroot
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // generate a unique filename
                var uniqueFileName = Guid.NewGuid().ToString() 
                                     + Path.GetExtension(ImageUrl.FileName);

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageUrl.CopyToAsync(fileStream);
                }

                // save the relative URL to the model
                model.ImageUrl = "/uploads/" + uniqueFileName;
            }

            if (ModelState.IsValid)
            {
                model.FarmerId = farmer.Id;
                _context.Products.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Product added successfully!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "ModelState is not valid. Please check all fields.");
            return View(model);
        }

        // Title: Upload Files in ASP.NET MVC 5
// Author: Suraj Kumar (C# Corner)
// Date: 2019
// Code version: ASP.NET MVC 5
// Availability: Online at https://www.c-sharpcorner.com/article/upload-files-in-asp-net-mvc-5/


        // Displays all products with filtering options (accessible only to employees)
        // Supports filtering by farmer, category, and date range
        [Authorize(Roles = "Employee")]
        public IActionResult All(int? farmerId, string category, DateTime? startDate, DateTime? endDate)
        {
            var farmers = _context.Farmers.ToList();
            ViewBag.Farmers = farmers;

            // Get distinct categories for the dropdown
            var categories = _context.Products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
            ViewBag.Categories = categories;

            var products = _context.Products.Include(p => p.Farmer).AsQueryable();
            if (farmerId.HasValue)
                products = products.Where(p => p.FarmerId == farmerId);
            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.Category == category);
            if (startDate.HasValue)
                products = products.Where(p => p.ProductionDate >= startDate);
            if (endDate.HasValue)
                products = products.Where(p => p.ProductionDate <= endDate);
            products = products.OrderByDescending(p => p.ProductionDate);
            return View(products.ToList());
        }

        // Title: Filtering Data - LINQ (C#)
// Author: Microsoft Docs Team
// Date: 2024
// Code version: .NET
// Availability: Online at https://learn.microsoft.com/en-us/dotnet/csharp/linq/standard-query-operators/filtering-data

        // Displays the edit form for a specific product
        // Ensures only the product owner can edit their products
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var farmer = _context.Farmers.FirstOrDefault(f => f.Email == user.Email);
            if (farmer == null)
                return Unauthorized();

            var product = await _context.Products.FindAsync(id);
            if (product == null || product.FarmerId != farmer.Id)
                return NotFound();

            return View(product);
        }

        // Handles the update of an existing product, including image replacement
        // Includes validation and security checks to ensure only the owner can modify
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product model, IFormFile ImageUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            var farmer = _context.Farmers.FirstOrDefault(f => f.Email == user.Email);
            if (farmer == null)
                return Unauthorized();

            var product = await _context.Products.FindAsync(id);
            if (product == null || product.FarmerId != farmer.Id)
                return NotFound();

            // Allow image to be optional
            if (ImageUrl == null || ImageUrl.Length == 0)
            {
                ModelState.Remove("ImageUrl");
            }

            if (ModelState.IsValid)
            {
                product.Name = model.Name;
                product.Category = model.Category;
                product.Description = model.Description;
                product.Price = model.Price;
                product.ProductionDate = model.ProductionDate;

                if (ImageUrl != null && ImageUrl.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageUrl.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(fileStream);
                    }
                    product.ImageUrl = "/uploads/" + uniqueFileName;
                }

                _context.Update(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Product updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        // Title: Implement CRUD operations in ASP.NET Core MVC
// Author: Microsoft Docs Team
// Date: 2024
// Code version: ASP.NET Core 9.0
// Availability: Online at https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/crud?view=aspnetcore-9.0

        // Handles the deletion of a product
        // Includes security checks to ensure only the owner can delete their products
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var farmer = _context.Farmers.FirstOrDefault(f => f.Email == user.Email);
            if (farmer == null)
                return Unauthorized();

            var product = await _context.Products.FindAsync(id);
            if (product == null || product.FarmerId != farmer.Id)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Product deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
} 

//Title: Pro C 7 with.NET and .NET Core
//Author: Andrew Troelsen; Philip Japikse
// Date: 2017
// Code version: Version 1
//Availability: Textbook / Ebook