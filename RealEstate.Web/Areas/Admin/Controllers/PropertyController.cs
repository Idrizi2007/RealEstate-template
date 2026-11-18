using Microsoft.AspNetCore.Mvc;
using RealEstate.DataAccess.Repository.IRepository;
using RealEstate.Models;
using RealEstate.Web.ViewModels;
using RealEstate.Web.Extensions;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class PropertyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PropertyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            PropertyVM PropertyFromVMDb = new PropertyVM()
            {
                Property = new Property(),
                PropertyTypes = (await _unitOfWork.PropertyTypes.GetAllAsync()).ToSelectList(),
                Cities = (await _unitOfWork.City.GetAllAsync()).ToSelectList(),
                Neighbourhoods = (await _unitOfWork.Neighbourhood.GetAllAsync()).ToSelectList()
            };
            if (id == null || id == 0)
            {
                return View(PropertyFromVMDb);
            }
             PropertyFromVMDb.Property = await _unitOfWork.Properties.GetAsync(u => u.Id == id, includeProperties: "PropertyType,Neighbourhood,City");
            if (PropertyFromVMDb.Property == null)
            {
                return NotFound();
            }
            return View(PropertyFromVMDb);
        }
        [HttpPost]

        public async Task<IActionResult> Upsert(PropertyVM propertyvm)
        {
            var existingProperty = await _unitOfWork.Properties.GetAsync(u => u.ReferenceCode == propertyvm.Property.ReferenceCode && u.Id != propertyvm.Property.Id);
            if(existingProperty != null)
            {
                ModelState.AddModelError("Property.ReferenceCode", "Kodi i referencës duhet të jetë unik.");
            }
            if (ModelState.IsValid)
            {
                bool isNew = propertyvm.Property.Id == 0;
                if (isNew)
                {
                    propertyvm.Property.CreatedAt = DateTime.UtcNow;
                    _unitOfWork.Properties.Add(propertyvm.Property);

                }
                else
                {
                    _unitOfWork.Properties.Update(propertyvm.Property);
                }
                await _unitOfWork.SaveAsync();
                TempData["success"] = isNew ? "Prona u krijua me sukses" : "Prona u përditësua me sukses";
                return RedirectToAction(nameof(Index));
            }
            propertyvm.PropertyTypes = (await _unitOfWork.PropertyTypes.GetAllAsync()).ToSelectList();
            propertyvm.Cities = (await _unitOfWork.City.GetAllAsync()).ToSelectList();
            propertyvm.Neighbourhoods = (await _unitOfWork.Neighbourhood.GetAllAsync()).ToSelectList();
            return View(propertyvm);
        }
        #region Api Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var propertyList = await _unitOfWork.Properties.GetAllAsync(includeProperties: "PropertyType,Neighbourhood,City");
            return Json(new { data = propertyList });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest(new { message = "Id i pavlefshëm" });
            }
            var property = await _unitOfWork.Properties.GetAsync(u => u.Id == id);
            if (property == null)
            {
                return NotFound(new { message = "Prona nuk u gjet" });
            }
            _unitOfWork.Properties.Remove(property);
            await _unitOfWork.SaveAsync();
            return Ok(new { message = "Prona u fshi me sukses" });
        }
        #endregion
    }

}
