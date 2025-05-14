using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriCulture.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Description{get; set;}

        [Display(Name = "Product Image")]
        public string? ImageUrl { get; set; }

        [Required]
        public double Price{get; set;}

        [Required]
        public DateTime ProductionDate { get; set; }

        public int FarmerId { get; set; }
        [ForeignKey("FarmerId")]
        public Farmer? Farmer { get; set; }
    }
} 