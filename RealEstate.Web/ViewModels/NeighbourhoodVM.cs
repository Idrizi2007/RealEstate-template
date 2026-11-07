using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using RealEstate.Models;

namespace RealEstate.Web.ViewModels
{
    public class NeighbourhoodVM
    {
        public Neighbourhood Neighbourhood { get; set; } = new();
        
        [ValidateNever]
        public IEnumerable<SelectListItem>? Cities { get; set; }
    }
}
