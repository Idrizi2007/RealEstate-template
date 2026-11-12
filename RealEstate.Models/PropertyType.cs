using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RealEstate.Models
{
    public class PropertyType : ILookup
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string? Name { get; set; } = string.Empty;
    }
}
