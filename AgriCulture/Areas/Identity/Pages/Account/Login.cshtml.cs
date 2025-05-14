// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using AgriCulture.Models;

namespace AgriCulture.Areas.Identity.Pages.Account
{
    // Handles user authentication and login functionality
    // Supports both regular and external login methods
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        // Model for login form input validation
        public class InputModel
        {
            // User's email address (used as username)
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            // User's password
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            // Whether to remember the user's login session
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            // User's role (Farmer or Employee)
            [Required]
            public string Role { get; set; }
        }

        // Title: Role-Based Access Control (RBAC) in C# and ASP.NET Core
// Author: Nwonah R. (Medium)
// Date: 2023
// Code version: ASP.NET Core
// Availability: Online at https://medium.com/@nwonahr/role-based-access-control-rbac-in-c-and-asp-net-core-the-security-backbone-of-modern-apps-dea1204a0870

        // Handles GET requests to the login page
        // Clears any existing external authentication cookies and prepares the login form
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        // Handles POST requests for login attempts
        // Validates credentials, checks user role, and manages authentication state
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // Attempt to sign in the user with provided credentials
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // Verify user exists and has the correct role
                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    if (user != null && await _userManager.IsInRoleAsync(user, Input.Role))
                    {
                        _logger.LogInformation("User logged in.");
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        // Sign out if role verification fails
                        await _signInManager.SignOutAsync();
                        ModelState.AddModelError(string.Empty, "Invalid role for this user.");
                        return Page();
                    }
                }
                // Handle various authentication scenarios
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }
// Title: Role-based authorization in ASP.NET Core
// Author: Microsoft Docs Team
// Date: 2024
// Code version: ASP.NET Core 8.0
// Availability: Online at https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-8.0


            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
//Title: Pro C 7 with.NET and .NET Core 
//Author: Andrew Troelsen; Philip Japikse 
// Date: 2017 
// Code version: Version 1 
//Availability: Textbook / Ebook