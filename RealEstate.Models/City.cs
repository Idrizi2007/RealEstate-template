using System.ComponentModel.DataAnnotations;

namespace RealEstate.Models
{
    public class City : ILookup
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Emri i qytetit është i detyrueshëm")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; } = true;

    }
}
