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
        public async Task<IActionResult> Upsert(int? id)
        {
            var neighberhoodFromDb = new NeighbourhoodVM()
            {
                Neighbourhood = new Neighbourhood(),
                Cities = (await _unitOfWork.City.GetAllAsync(u => u.IsActive == true)).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })

            };
            
            if (id == null || id == 0)
            {
                return View(neighberhoodFromDb);
            }
            neighberhoodFromDb.Neighbourhood = await _unitOfWork.Neighbourhood.GetAsync(u => u.Id == id);
            if(neighberhoodFromDb.Neighbourhood == null)
            {
                return NotFound();
            }
            return View(neighberhoodFromDb);
        }
        [HttpPost]
        public async Task<IActionResult> Upsert(NeighbourhoodVM neighbourhoodVM)
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
                await _unitOfWork.SaveAsync();
                TempData["success"] = isNew ? "Lagjja u shtua me sukses" : "Lagjja u përditësua me sukses";
                return RedirectToAction(nameof(Index));
            }
            neighbourhoodVM.Cities = (await _unitOfWork.City.GetAllAsync(u => u.IsActive)).Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            return View(neighbourhoodVM);
        }
        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var neighbourhoodList = await _unitOfWork.Neighbourhood.GetAllAsync(includeProperties: "City");
            return Json(new { data = neighbourhoodList });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest(new { message = "Id i pavlefshëm" });
            }
            var neighbourhood = await _unitOfWork.Neighbourhood.GetAsync(u => u.Id == id);
            if (neighbourhood == null)
            {
                return NotFound(new { message = "Lagjja nuk u gjet" });
            }
            _unitOfWork.Neighbourhood.Remove(neighbourhood);
            await _unitOfWork.SaveAsync();
            return Ok(new { message = "Lagjja u fshi me sukses" });
        }
        #endregion
    }
}
    