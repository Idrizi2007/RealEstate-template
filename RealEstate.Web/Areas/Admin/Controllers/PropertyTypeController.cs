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

        public async Task<IActionResult> Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new PropertyType());
            }

            PropertyType? propertyTypeFromDb = await _unitOfWork.PropertyTypes.GetAsync(u => u.Id == id);
            if (propertyTypeFromDb == null)
            {
                return NotFound();
            }
            return View(propertyTypeFromDb);
        }
        [HttpPost]
        public async Task<IActionResult> Upsert(PropertyType propertyType)
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
                await _unitOfWork.SaveAsync();
                TempData["success"] = isNew ? "Lloji i pronës u shtua me sukses" : "Lloji i pronës u përditësua me sukses";
                return RedirectToAction(nameof(Index));
            }
            return View(propertyType);
        }

        #region Api Calls
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var propertyTypeFromDb = (await _unitOfWork.PropertyTypes.GetAllAsync()).OrderBy(o => o.Name);
            return Json(new { data = propertyTypeFromDb });
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return BadRequest(new { message = "Id i pavlefshëm" });
            }
            PropertyType? propertyTypeFromDb = await _unitOfWork.PropertyTypes.GetAsync(u => u.Id == id);
            if (propertyTypeFromDb == null)
            {
                return NotFound(new { message = "Lloji i pronës nuk u gjet" });
            }
            _unitOfWork.PropertyTypes.Remove(propertyTypeFromDb);
            await _unitOfWork.SaveAsync();
            return Json(new { message = "Lloji i pronës u fshi me sukses" });
        }
        #endregion
    }
}
