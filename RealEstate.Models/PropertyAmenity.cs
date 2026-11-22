using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Models
{
    public class PropertyAmenity
    {
        public int PropertyId { get; set; }
        [ForeignKey("PropertyId")]
        public Property? Property { get; set; }

        public int AmenityId { get; set; }
        [ForeignKey("AmenityId")]
        public Amenity? Amenity { get; set; }
    }
}
