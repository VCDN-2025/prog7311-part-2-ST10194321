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
    // Controller responsible for managing user accounts, specifically focused on farmer account creation
    // and management. Only accessible by employees.
    [Authorize(Roles = "Employee")]
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

        // Displays the form to create a new farmer account
        // Optionally displays a generated password if one was created
        public IActionResult CreateFarmer(string password = null)
        {
            ViewBag.GeneratedPassword = password;
            return View();
        }

        // Handles the creation of a new farmer account
        // Creates both ApplicationUser and Farmer records, assigns appropriate role
        // Generates a secure random password for the new account
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
        // Title: Role-Based Access Control (RBAC) in C# and ASP.NET Core
// Author: Nwonah R. (Medium)
// Date: 2023
// Code version: ASP.NET Core
// Availability: Online at https://medium.com/@nwonahr/role-based-access-control-rbac-in-c-and-asp-net-core-the-security-backbone-of-modern-apps-dea1204a0870

        // Generates a secure random password that meets common security requirements:
        // - At least one uppercase letter
        // - At least one lowercase letter
        // - At least one digit
        // - At least one special character
        // - Minimum length of 8 characters
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
// Title: Generating random passwords
// Author: Stack Overflow Community
// Date: 2008
// Availability: Online at https://stackoverflow.com/questions/54991/generating-random-passwords


        // Displays a list of all registered farmers in the system
        public IActionResult ListFarmers()
        {
            var farmers = _context.Set<Farmer>().ToList();
            return View(farmers);
        }
    }
} 

//Title: Pro C 7 with.NET and .NET Core 
//Author: Andrew Troelsen; Philip Japikse 
// Date: 2017 
// Code version: Version 1 
//Availability: Textbook / Ebook