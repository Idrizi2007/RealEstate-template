using Microsoft.AspNetCore.Mvc;
using RealEstate.DataAccess.Repository.IRepository;
using RealEstate.Models;
using RealEstate.Utility;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AmenityController(IUnitOfWork unitOfWork)
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
                return View(new Amenity());
            }

            Amenity? amenityFromDb = _unitOfWork.Amenities.Get(u => u.Id == id);
            if (amenityFromDb == null)
            {
                return NotFound();
            }
            return View(amenityFromDb);
        }
        [HttpPost]
        public IActionResult Upsert(Amenity amenity)
        {
            if (!string.IsNullOrEmpty(amenity.IconClass) && !AmenityIcons.All.Contains(amenity.IconClass))
            {
                ModelState.AddModelError(nameof(amenity.IconClass), "Ikona nuk është e vlefshme");
            }

            if (ModelState.IsValid)
            {
                bool isNew = amenity.Id == 0;
                if (isNew)
                {
                    _unitOfWork.Amenities.Add(amenity);
                }
                else
                {
                    _unitOfWork.Amenities.Update(amenity);
                }
                _unitOfWork.Save();
                TempData["success"] = isNew ? "Pajisja u shtua me sukses" : "Pajisja u përditësua me sukses";
                return RedirectToAction(nameof(Index));
            }
            return View(amenity);
        }

        #region Api Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var amenitiesFromDb = _unitOfWork.Amenities.GetAll().OrderBy(o => o.Name);
            return Json(new { data = amenitiesFromDb });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return BadRequest(new { message = "Id i pavlefshëm" });
            }
            Amenity? amenityFromDb = _unitOfWork.Amenities.Get(u => u.Id == id);
            if (amenityFromDb == null)
            {
                return NotFound(new { message = "Pajisja nuk u gjet" });
            }
            _unitOfWork.Amenities.Remove(amenityFromDb);
            _unitOfWork.Save();
            return Json(new { message = "Pajisja u fshi me sukses" });
        }
        #endregion
    }
}
