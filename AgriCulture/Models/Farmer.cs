using System.ComponentModel.DataAnnotations;

namespace AgriCulture.Models
{
    // Represents a farmer in the system
    // Contains personal information and farm details
    public class Farmer
    {
        [Key]
        public int Id { get; set; }

        // Farmer's first name
        [Required]
        public string FirstName { get; set; }

        // Farmer's last name
        [Required]
        public string LastName { get; set; }

        // Farmer's email address (used for authentication)
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // Name of the farmer's farm
        public string FarmName { get; set; }

        // Physical location of the farm
        public string FarmLocation { get; set; }

        // Size of the farm (e.g., "10 acres", "5 hectares")
        public string FarmSize { get; set; }

        // Reference to the associated ApplicationUser account
        public string? UserId { get; set; }
    }
} 
//Title: Pro C 7 with.NET and .NET Core 
//Author: Andrew Troelsen; Philip Japikse 
// Date: 2017 
// Code version: Version 1 
//Availability: Textbook / Ebook