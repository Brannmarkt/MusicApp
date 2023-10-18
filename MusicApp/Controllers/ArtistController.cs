using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicApp.Models;
using MusicApp.Models.ViewModels;
using MusicApp.Repositories.Interfaces;

namespace MusicApp.Controllers
{
    public class ArtistController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArtistController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Artist> artistsList = _unitOfWork.Artist.GetAll(includeProperties: "Albums").ToList();

            return View(artistsList);
        }

        public IActionResult Upsert(Guid? id)
        {
            ArtistViewModel artistViewModel = new()
            {
                AlbumList = _unitOfWork.Album.GetAll().Select(u => new SelectListItem
                {
                    Text = u.AlbumTitle,
                    Value = u.Id.ToString()
                }),

                Artist = new Artist()
            };

            if(id == null)
            {
                return View(artistViewModel); // create new artist
            }
            else
            {
                artistViewModel.Artist = _unitOfWork.Artist.Get(u => u.Id == id);
                return View(artistViewModel);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ArtistViewModel artistViewModel, IFormFile? file)
        {
            if(ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\artists");

                    if (!string.IsNullOrEmpty(artistViewModel.Artist.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath =
                            Path.Combine(wwwRootPath, artistViewModel.Artist.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    artistViewModel.Artist.ImageUrl = @"\images\artists\" + fileName;
                }

                if (artistViewModel.Artist.Id == Guid.Empty)
                {
                    _unitOfWork.Artist.Add(artistViewModel.Artist);
                }
                else
                {
                    _unitOfWork.Artist.Update(artistViewModel.Artist);
                }

                _unitOfWork.Save();
                TempData["success"] = "Artist created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                artistViewModel.AlbumList = _unitOfWork.Album.GetAll().Select(u => new SelectListItem
                {
                    Text = u.AlbumTitle,
                    Value = u.Id.ToString()
                });
                return View(artistViewModel);
            }
        }

        public IActionResult Delete(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }
            Artist? artistFromDb = _unitOfWork.Artist.Get(u => u.Id == id);

            if (artistFromDb == null)
            {
                return NotFound();
            }
            return View(artistFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(Guid? id)
        {
            Artist? obj = _unitOfWork.Artist.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            if(obj.ImageUrl != null)
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                               obj.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _unitOfWork.Artist.Delete(obj);
            _unitOfWork.Save();
            TempData["success"] = "Artist was deleted successfully";
            return RedirectToAction("Index");
        }

        //[HttpDelete]
        //public IActionResult Delete(Guid id)
        //{
        //    var artistToBeDeleted = _unitOfWork.Artist.GetById(u => u.Id == id);
        //    if (artistToBeDeleted == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting" });
        //    }

        //    var oldImagePath =
        //                   Path.Combine(_webHostEnvironment.WebRootPath,
        //                   artistToBeDeleted.ImageUrl.TrimStart('\\'));

        //    if (System.IO.File.Exists(oldImagePath))
        //    {
        //        System.IO.File.Delete(oldImagePath);
        //    }

        //    _unitOfWork.Artist.Delete(artistToBeDeleted);
        //    _unitOfWork.Save();

        //    return Json(new { success = true, message = "Delete Successful" });
        //}
    }
}
