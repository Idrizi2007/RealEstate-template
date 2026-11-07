using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RealEstate.DataAccess.Repository.IRepository;
using RealEstate.Models;
using RealEstate.Web.ViewModels;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NeighbourhoodController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public NeighbourhoodController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            var neighberhoodFromDb = new NeighbourhoodVM()
            {
                Neighbourhood = new Neighbourhood(),
                Cities = _unitOfWork.City.GetAll(u => u.IsActive == true).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })

            };
            
            if (id == null || id == 0)
            {
                return View(neighberhoodFromDb);
            }
            neighberhoodFromDb.Neighbourhood = _unitOfWork.Neighbourhood.Get(u => u.Id == id);
            if(neighberhoodFromDb.Neighbourhood == null)
            {
                return NotFound();
            }
            return View(neighberhoodFromDb);
        }
        [HttpPost]
        public IActionResult Upsert(NeighbourhoodVM neighbourhoodVM)
        {
            if (ModelState.IsValid)
            {
                bool isNew = neighbourhoodVM.Neighbourhood.Id == 0;
                if (isNew)
                {
                    _unitOfWork.Neighbourhood.Add(neighbourhoodVM.Neighbourhood);
                }
                else
                {
                    _unitOfWork.Neighbourhood.Update(neighbourhoodVM.Neighbourhood);
                }
                _unitOfWork.Save();
                TempData["success"] = isNew ? "Lagjja u shtua me sukses" : "Lagjja u përditësua me sukses";
                return RedirectToAction(nameof(Index));
            }
            neighbourhoodVM.Cities = _unitOfWork.City.GetAll(u => u.IsActive).Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            return View(neighbourhoodVM);
        }
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var neighbourhoodList = _unitOfWork.Neighbourhood.GetAll(includeProperties: "City");
            return Json(new { data = neighbourhoodList });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest(new { message = "Id i pavlefshëm" });
            }
            var neighbourhood = _unitOfWork.Neighbourhood.Get(u => u.Id == id);
            if (neighbourhood == null)
            {
                return NotFound(new { message = "Lagjja nuk u gjet" });
            }
            _unitOfWork.Neighbourhood.Remove(neighbourhood);
            _unitOfWork.Save();
            return Ok(new { message = "Lagjja u fshi me sukses" });
        }
        #endregion
    }
}
    