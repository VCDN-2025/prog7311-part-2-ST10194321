using System.ComponentModel.DataAnnotations;

namespace AgriCulture.Models
{
    public class Farmer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string FarmName { get; set; }
        public string FarmLocation { get; set; }
        public string FarmSize { get; set; }

      
        public string? UserId { get; set; }
    }
} 