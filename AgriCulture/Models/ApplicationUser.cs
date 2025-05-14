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