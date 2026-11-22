using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using RealEstate.Models;

namespace RealEstate.Web.ViewModels
{
    public class PropertyVM
    {
        public Property Property { get; set; } = new();

        [ValidateNever]
        public IEnumerable<SelectListItem>? Cities { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? Neighbourhoods { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? PropertyTypes { get; set; }

        [ValidateNever]
        public IEnumerable<Amenity>? Amenities { get; set; }

        public List<int> SelectedAmenityIds { get; set; } = new();

        [ValidateNever]
        public IEnumerable<PropertyImage>? Images { get; set; }


        [ValidateNever]
        public List<IFormFile>? NewImages { get; set; }
    }
}
