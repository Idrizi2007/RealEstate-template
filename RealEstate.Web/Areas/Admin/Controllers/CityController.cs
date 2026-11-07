using Microsoft.AspNetCore.Mvc;
using RealEstate.DataAccess.Repository.IRepository;
using RealEstate.Models;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CityController(IUnitOfWork unitOfWork)
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
                return View(new City());
            }

            City? cityFromDb = _unitOfWork.City.Get(u => u.Id == id);
            if (cityFromDb == null)
            {
                return NotFound();
            }
            return View(cityFromDb);

        }
        [HttpPost]
        public IActionResult Upsert(City city)
        {
            if (ModelState.IsValid)
            {
                bool isNew = city.Id == 0;
                if (isNew)
                {
                    _unitOfWork.City.Add(city);
                }
                else
                {
                    _unitOfWork.City.Update(city);
                }
                _unitOfWork.Save();
                TempData["success"] = isNew ? "Qyteti u shtua me sukses" : "Qyteti u perditesua me sukses";
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }
        #region Api Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var cityList = _unitOfWork.City.GetAll();
            return Json(new { data = cityList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return BadRequest(new { message = "Id i pavlefshëm" });
            }
            City? cityFromDb = _unitOfWork.City.Get(u => u.Id == id);
            if (cityFromDb == null)
            {
                return NotFound(new { message = "Qyteti nuk u gjet" });
            }

            _unitOfWork.City.Remove(cityFromDb);
            _unitOfWork.Save();
            return Ok(new { message = "Qyteti u fshi me sukses" });
        }

        #endregion
    }

}
