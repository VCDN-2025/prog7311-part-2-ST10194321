using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriCulture.Models
{
    // Represents an agricultural product in the system
    // Contains information about the product and its relationship with the farmer
    public class Product
    {
        [Key]
        public int Id { get; set; }

        // The name of the agricultural product
        [Required]
        public string Name { get; set; }

        // The category of the product (e.g., Vegetables, Fruits, Dairy)
        [Required]
        public string Category { get; set; }

        // Detailed description of the product
        [Required]
        public string Description{get; set;}

        // Optional URL to the product's image
        [Display(Name = "Product Image")]
        public string? ImageUrl { get; set; }

        // The price of the product
        [Required]
        public double Price{get; set;}

        // The date when the product was produced
        [Required]
        public DateTime ProductionDate { get; set; }

        // Foreign key to the Farmer who produced this product
        public int FarmerId { get; set; }
        [ForeignKey("FarmerId")]
        public Farmer? Farmer { get; set; }
    }
} 
//Title: Pro C 7 with.NET and .NET Core 
//Author: Andrew Troelsen; Philip Japikse 
// Date: 2017 
// Code version: Version 1 
//Availability: Textbook / Ebook