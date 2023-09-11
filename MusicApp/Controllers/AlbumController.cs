using Microsoft.AspNetCore.Mvc;
using MusicApp.Models;
using MusicApp.Models.ViewModels;
using MusicApp.Repositories.Interfaces;

namespace MusicApp.Controllers
{
    public class AlbumController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AlbumController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            

            return View();
        }

        public IActionResult Upsert(Guid? id)
        {
            AlbumViewModel albumViewModel = new()
            {
                Album = new Album()
                {
                    ArtistId = id ?? throw new ArgumentNullException(nameof(id)),
                }
            };

            if ((_unitOfWork.Album.Get(u => u.ArtistId == id)).Id == Guid.Empty)
            {
                return View(albumViewModel); // create new album
            }
            else
            {
                albumViewModel.Album = _unitOfWork.Album.Get(u => u.ArtistId == id);
                return View(albumViewModel);
            }
        }
        [HttpPost]
        public IActionResult Upsert(AlbumViewModel albumViewModel, IFormFile file)
        {
            if(ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\albums");

                    if (!string.IsNullOrEmpty(albumViewModel.Album.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath =
                            Path.Combine(wwwRootPath, albumViewModel.Album.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    albumViewModel.Album.ImageUrl = @"\images\albums\" + fileName;
                }

                if (albumViewModel.Album.Id == Guid.Empty)
                {
                    
                    _unitOfWork.Album.Add(albumViewModel.Album);
                }
                else
                {
                    _unitOfWork.Album.Update(albumViewModel.Album);
                }

                _unitOfWork.Save();
                TempData["success"] = "Album created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(albumViewModel);
            }
        }
    }
}
