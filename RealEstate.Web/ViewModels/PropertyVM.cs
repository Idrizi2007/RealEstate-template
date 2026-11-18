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
    }
}
