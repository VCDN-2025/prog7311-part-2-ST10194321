using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AgriCulture.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserType { get; set; } 

        public bool MustChangePassword { get; set; } = true;
    }
} 
//Title: Pro C 7 with.NET and .NET Core 
//Author: Andrew Troelsen; Philip Japikse 
// Date: 2017 
// Code version: Version 1 
//Availability: Textbook / Ebook