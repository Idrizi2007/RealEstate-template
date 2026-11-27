using Microsoft.AspNetCore.Mvc;
using RealEstate.DataAccess.Repository.IRepository;
namespace RealEstate.Web.Areas.Customer
{
    [Area("Customer")]
    public class PropertyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PropertyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var propetiesFromDb = (await _unitOfWork.Properties.GetAllAsync(u => u.IsActive && u.City.IsActive, includeProperties: "PropertyType,Neighbourhood,City,Images")).OrderByDescending(u => u.CreatedAt);
            return View(propetiesFromDb);
        }
    }
}
