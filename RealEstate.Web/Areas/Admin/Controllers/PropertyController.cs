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
        private readonly IWebHostEnvironment _hostEnvironment;
        public PropertyController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            PropertyVM propertyVM = new PropertyVM()        
            {
                Property = new Property(),
                PropertyTypes = (await _unitOfWork.PropertyTypes.GetAllAsync()).ToSelectList(),
                Cities = (await _unitOfWork.City.GetAllAsync()).ToSelectList(),
                Neighbourhoods = (await _unitOfWork.Neighbourhood.GetAllAsync()).ToSelectList(),
                Amenities = (await _unitOfWork.Amenities.GetAllAsync())
            };
            if (id == null || id == 0)
            {
                return View(propertyVM);
            }
            propertyVM.Property = await _unitOfWork.Properties.GetAsync(u => u.Id == id, includeProperties: "PropertyType,Neighbourhood,City,Images");
            if (propertyVM.Property == null)
            {
                return NotFound();
            }
            propertyVM.Images = propertyVM.Property.Images?.OrderBy(i => i.SortOrder);
            propertyVM.SelectedAmenityIds = (await _unitOfWork.PropertyAmenities.GetAllAsync(u => u.PropertyId == id)).Select(u => u.AmenityId).ToList();
            return View(propertyVM);
        }
        [HttpPost]

        public async Task<IActionResult> Upsert(PropertyVM propertyvm)
        {
            var existingProperty = await _unitOfWork.Properties.GetAsync(u => u.ReferenceCode == propertyvm.Property.ReferenceCode && u.Id != propertyvm.Property.Id);
            if (existingProperty != null)
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
                
                var existingAmenities =  await _unitOfWork.PropertyAmenities.GetAllAsync(u => u.PropertyId == propertyvm.Property.Id);
                _unitOfWork.PropertyAmenities.RemoveRange(existingAmenities);
                await _unitOfWork.SaveAsync();
                foreach(var amenityId in propertyvm.SelectedAmenityIds)
                {
                    var propertyAmenity = new PropertyAmenity
                    {
                        PropertyId = propertyvm.Property.Id,
                        AmenityId = amenityId
                    };
                    _unitOfWork.PropertyAmenities.Add(propertyAmenity);
                }
                await _unitOfWork.SaveAsync();

                if (propertyvm.NewImages != null && propertyvm.NewImages.Count > 0)
                {
                    string rootPath = _hostEnvironment.WebRootPath;
                    string defaultImagePath = Path.Combine(rootPath, "images", "properties", propertyvm.Property.Id.ToString());
                    Directory.CreateDirectory(defaultImagePath);
                    

                    int SortOrder = (await _unitOfWork.PropertyImages.GetAllAsync(u => u.PropertyId == propertyvm.Property.Id)).Count();
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".avif" };
                    
                    foreach (var image in propertyvm.NewImages)
                    {
                        var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

                        if (!allowedExtensions.Contains(extension))
                        {
                            TempData["error"] = "Formatet e lejuara të imazheve janë: .jpg, .jpeg, .png, .webp, .avif";
                            continue;
                        }

                        if (image.Length > 10 * 1024 * 1024 || image.Length == 0)
                        {
                            TempData["error"] = "Madhësia e imazhit duhet të jetë më e vogël se 10MB dhe më e madhe se 0KB.";
                            continue;
                        }


                        var fileName = Guid.NewGuid().ToString() + extension;
                        var filePath = Path.Combine(defaultImagePath, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }

                        string imageUrl = $"/images/properties/{propertyvm.Property.Id}/{fileName}";
                        var propertyImage = new PropertyImage
                        {
                            PropertyId = propertyvm.Property.Id,
                            ImageUrl = imageUrl,
                            IsPrimary = SortOrder == 0,
                            SortOrder = SortOrder++
                        };
                        _unitOfWork.PropertyImages.Add(propertyImage);

                    }
                    await _unitOfWork.SaveAsync();
                }

                TempData["success"] = isNew ? "Prona u krijua me sukses" : "Prona u përditësua me sukses";
                return RedirectToAction(nameof(Index));
            }

            propertyvm.PropertyTypes = (await _unitOfWork.PropertyTypes.GetAllAsync()).ToSelectList();
            propertyvm.Cities = (await _unitOfWork.City.GetAllAsync()).ToSelectList();
            propertyvm.Neighbourhoods = (await _unitOfWork.Neighbourhood.GetAllAsync()).ToSelectList();
            propertyvm.Amenities = (await _unitOfWork.Amenities.GetAllAsync());
            propertyvm.Images = (await _unitOfWork.PropertyImages.GetAllAsync(u => u.PropertyId == propertyvm.Property.Id)).OrderBy(i => i.SortOrder);


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
            var propertyImagespath = Path.Combine(_hostEnvironment.WebRootPath, "images", "properties", property.Id.ToString());
            
            _unitOfWork.Properties.Remove(property);
           
            await _unitOfWork.SaveAsync();
            if (Directory.Exists(propertyImagespath))
            {
                Directory.Delete(propertyImagespath, true);
            }
           
            return Ok(new { message = "Prona u fshi me sukses" });
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null || id == 0)
            {
                return BadRequest(new { message = "Id i pavlefshëm" });
            }
            var ImageFromDb = await _unitOfWork.PropertyImages.GetAsync(u => u.Id == id);
            if (ImageFromDb == null)
            {
                return NotFound(new { message = "Imazhi nuk u gjet" });
            }
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, ImageFromDb.ImageUrl.TrimStart('/'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _unitOfWork.PropertyImages.Remove(ImageFromDb);
            await _unitOfWork.SaveAsync();
            return Ok(new { message = "Imazhi u fshi me sukses" });
        }
        [HttpPost]
        public async Task<IActionResult> SetPrimaryImage(int? id)
        {
            if (id == null || id == 0)
            {
                return BadRequest(new { message = "Id i pavlefshëm" });
            }
            var imageFromDb = await _unitOfWork.PropertyImages.GetAsync(u => u.Id == id);
            if (imageFromDb == null)
            {
                return NotFound(new { message = "Imazhi nuk u gjet" });
            }
            var imagesFromDb = await _unitOfWork.PropertyImages.GetAllAsync(u => u.PropertyId == imageFromDb.PropertyId);
            foreach (var img in imagesFromDb)
            {
                img.IsPrimary = img.Id == imageFromDb.Id;
                _unitOfWork.PropertyImages.Update(img);
            }
           
            await _unitOfWork.SaveAsync();
            return Ok(new { message = "Imazhi kryesor u ndryshua me sukses" });
        }

        #endregion
    }

}
