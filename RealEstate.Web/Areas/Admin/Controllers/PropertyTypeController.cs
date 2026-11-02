using Microsoft.AspNetCore.Mvc;
using RealEstate.DataAccess.Repository.IRepository;
using RealEstate.Models;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PropertyTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PropertyTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new PropertyType());
            }

            PropertyType? propertyTypeFromDb = _unitOfWork.PropertyTypes.Get(u => u.Id == id);
            if (propertyTypeFromDb == null)
            {
                return NotFound();
            }
            return View(propertyTypeFromDb);
        }
        [HttpPost]
        public IActionResult Upsert(PropertyType propertyType)
        {
            if (ModelState.IsValid)
            {
                bool isNew = propertyType.Id == 0;
                if (isNew)
                {
                    _unitOfWork.PropertyTypes.Add(propertyType);
                }
                else
                {
                    _unitOfWork.PropertyTypes.Update(propertyType);
                }
                _unitOfWork.Save();
                TempData["success"] = isNew ? "Lloji i pronës u shtua me sukses" : "Lloji i pronës u përditësua me sukses";
                return RedirectToAction(nameof(Index));
            }
            return View(propertyType);
        }

        #region Api Calls
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var propertyTypeFromDb = _unitOfWork.PropertyTypes.GetAll().OrderBy(o => o.Name);
            return Json(new { data = propertyTypeFromDb });
        }
        
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return BadRequest(new { message = "Invalid ID" });
            }
            PropertyType? propertyTypeFromDb = _unitOfWork.PropertyTypes.Get(u => u.Id == id);
            if (propertyTypeFromDb == null)
            {
                return NotFound(new { message = "Property type not found" });
            }
            _unitOfWork.PropertyTypes.Remove(propertyTypeFromDb);
            _unitOfWork.Save();
            return Json(new { message = "Property type deleted successfully" });
        }
        #endregion
    }
}
