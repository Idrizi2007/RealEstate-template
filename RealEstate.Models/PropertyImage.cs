using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Models
{
    public class PropertyImage
    {
        [Key]
        public int Id { get; set; }

        public int PropertyId { get; set; }
        [ForeignKey("PropertyId")]
        public Property? Property { get; set; }
        [Required]
        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [StringLength(150)]
        [DisplayName("Përshkrimi i fotos")]
        public string? AltText { get; set; }

        [DisplayName("Kryesore")]
        public bool IsPrimary { get; set; }

        public int SortOrder { get; set; }
    }
}
