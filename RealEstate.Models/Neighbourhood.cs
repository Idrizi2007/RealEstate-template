using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RealEstate.Models
{
    public class Neighbourhood : ILookup
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        [Required]
        [DisplayName("Qyteti")]
        public int? CityId { get; set; }
        [ForeignKey("CityId")]
        public City? City { get; set; }

        [DisplayName("Aktiv")]
        public bool IsActive { get; set; } = true;
    }
}
