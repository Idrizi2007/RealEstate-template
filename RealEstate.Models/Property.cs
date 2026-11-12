using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [DisplayName("Kodi i referencës")]
        public string? ReferenceCode { get; set; }

        [Required]
        [StringLength(150)]
        [DisplayName("Titulli")]
        public string? Title { get; set; }

        [Required]
        [DisplayName("Lloji i pronës")]
        public int? PropertyTypeId { get; set; }
        [ForeignKey("PropertyTypeId")]
        public PropertyType? PropertyType { get; set; }

        [Required]
        [DisplayName("Qyteti")]
        public int? CityId { get; set; }
        [ForeignKey("CityId")]
        public City? City { get; set; }

        [DisplayName("Lagjja")]
        public int? NeighbourhoodId { get; set; }
        [ForeignKey("NeighbourhoodId")]
        public Neighbourhood? Neighbourhood { get; set; }

        [StringLength(250)]
        [DisplayName("Adresa")]
        public string? Address { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Çmimi i shitjes")]
        public decimal? SalePrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Çmimi i qirasë (në muaj)")]
        public decimal? RentPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Depozita")]
        public decimal? Deposit { get; set; }

        [DisplayName("Kohëzgjatja minimale (muaj)")]
        public int? MinimumTermMonths { get; set; }

        [Required]
        [DisplayName("Sipërfaqja (m²)")]
        public int? AreaM2 { get; set; }

        [DisplayName("Dhoma gjumi")]
        public int? Bedrooms { get; set; }

        [DisplayName("Banjo")]
        public int? Bathrooms { get; set; }

        [DisplayName("Kati")]
        public int? Floor { get; set; }

        [DisplayName("Katet gjithsej")]
        public int? TotalFloors { get; set; }

        [DisplayName("Viti i ndërtimit")]
        public int? YearBuilt { get; set; }

        [StringLength(4000)]
        [DisplayName("Përshkrimi")]
        public string? Description { get; set; }

        [DisplayName("I veçuar")]
        public bool IsFeatured { get; set; }

        [DisplayName("Aktiv")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
