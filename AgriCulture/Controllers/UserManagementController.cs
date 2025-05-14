using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AgriCulture.Models;
using AgriCulture.Data;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace AgriCulture.Controllers
{
    [Authorize(Roles = "Employee")] // Only employees can access this controller
    public class UserManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagementController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Create Farmer
        public IActionResult CreateFarmer(string password = null)
        {
            ViewBag.GeneratedPassword = password;
            return View();
        }

        // POST: Create Farmer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFarmer(Farmer model)
        {
            if (ModelState.IsValid)
            {
                // 1. Create ApplicationUser
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserType = "Farmer"
                };
                // 2. Generate a random password
                var password = GeneratePassword();
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    // 3. Assign Farmer role
                    if (!await _roleManager.RoleExistsAsync("Farmer"))
                        await _roleManager.CreateAsync(new IdentityRole("Farmer"));
                    await _userManager.AddToRoleAsync(user, "Farmer");
                    // 4. Store user Id in Farmer
                    model.UserId = user.Id;
                    _context.Add(model);
                    await _context.SaveChangesAsync();
                    // 5. Show password to employee
                    return RedirectToAction("CreateFarmer", new { password });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        private string GeneratePassword()
        {
            var rand = new Random();
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "!@#$%^&*";
            const string all = upper + lower + digits + special;
            return new string(new[] {
                upper[rand.Next(upper.Length)],
                lower[rand.Next(lower.Length)],
                digits[rand.Next(digits.Length)],
                special[rand.Next(special.Length)],
                all[rand.Next(all.Length)],
                all[rand.Next(all.Length)],
                all[rand.Next(all.Length)],
                all[rand.Next(all.Length)]
            });
        }

        // GET: List all Farmers
        public IActionResult ListFarmers()
        {
            var farmers = _context.Set<Farmer>().ToList();
            return View(farmers);
        }
    }
} 