using System.ComponentModel.DataAnnotations;

namespace RealEstate.Models
{
    public class Amenity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(100)]
        public string? IconClass { get; set; }
    }
}
