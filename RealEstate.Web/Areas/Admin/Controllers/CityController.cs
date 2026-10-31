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
                if (city.Id == 0)
                {
                    _unitOfWork.City.Add(city);
                }
                else
                {
                    _unitOfWork.City.Update(city);
                }
                _unitOfWork.Save();
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
                return BadRequest(new { message = "Invalid ID" });
            }
            City? cityFromDb = _unitOfWork.City.Get(u => u.Id == id);
            if (cityFromDb == null)
            {
                return NotFound(new { message = "City not found" });
            }

            _unitOfWork.City.Remove(cityFromDb);
            _unitOfWork.Save();
            return Ok(new { message = "City deleted successfully" });
        }

        #endregion
    }

}
