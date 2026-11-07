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

        public async Task<IActionResult> Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new City());
            }

            City? cityFromDb = await _unitOfWork.City.GetAsync(u => u.Id == id);
            if (cityFromDb == null)
            {
                return NotFound();
            }
            return View(cityFromDb);

        }
        [HttpPost]
        public async Task<IActionResult> Upsert(City city)
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
                await _unitOfWork.SaveAsync();
                TempData["success"] = isNew ? "Qyteti u shtua me sukses" : "Qyteti u perditesua me sukses";
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }
        #region Api Calls

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cityList = await _unitOfWork.City.GetAllAsync();
            return Json(new { data = cityList });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return BadRequest(new { message = "Id i pavlefshëm" });
            }
            City? cityFromDb = await _unitOfWork.City.GetAsync(u => u.Id == id);
            if (cityFromDb == null)
            {
                return NotFound(new { message = "Qyteti nuk u gjet" });
            }

            _unitOfWork.City.Remove(cityFromDb);
            await _unitOfWork.SaveAsync();
            return Ok(new { message = "Qyteti u fshi me sukses" });
        }

        #endregion
    }

}
