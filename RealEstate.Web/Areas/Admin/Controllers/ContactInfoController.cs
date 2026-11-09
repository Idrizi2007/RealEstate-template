using Microsoft.AspNetCore.Mvc;
using RealEstate.DataAccess.Repository.IRepository;
using RealEstate.Models;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ContactInfoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ContactInfoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var contactInfoFromDb = (await _unitOfWork.ContactInfo.GetAsync());
            return View(contactInfoFromDb ?? new ContactInfo());
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactInfo contactInfo)
        {
            if (ModelState.IsValid)
            {
                var contactInfoFromDb = (await _unitOfWork.ContactInfo.GetAsync()) ?? new ContactInfo();
                contactInfo.Id = contactInfoFromDb.Id;
                _unitOfWork.ContactInfo.Update(contactInfo);
                TempData["success"] = "Kontaktet u ruajtën me sukses";
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactInfo);

        }
    }
}
