// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using AgriCulture.Models;

namespace AgriCulture.Areas.Identity.Pages.Account
{
    // Handles user logout functionality
    // Manages the sign-out process and redirects users appropriately
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        // Handles POST requests for logout
        // Signs out the user and redirects to the specified return URL or home page
        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            // Sign out the current user
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
// Title: Role-based authorization in ASP.NET Core
// Author: Microsoft Docs Team
// Date: 2024
// Code version: ASP.NET Core 8.0
// Availability: Online at https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-8.0