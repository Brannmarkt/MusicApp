using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            List<Album> albumList = _unitOfWork.Album.GetAll(includeProperties: "Songs").ToList();

            return View(albumList);
        }

        public IActionResult Create(Guid id)
        {
            AlbumViewModel albumViewModel = new()
            {
                SongList = _unitOfWork.Song.GetAll().Select(u => new SelectListItem
                {
                    Text = u.SongTitle,
                    Value = u.Id.ToString()
                }),

                Album = new Album()
                {
                    ArtistId = id,
                }
            };

            return View(albumViewModel);
        }
        [HttpPost]
        public IActionResult Create(AlbumViewModel albumViewModel, IFormFile? file, IFormFile? archive)
        {
            if (ModelState.IsValid)
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

                if(archive != null)
                {
                    byte[] fileData;
                    using (var binaryReader = new BinaryReader(archive.OpenReadStream()))
                    {
                        fileData = binaryReader.ReadBytes((int)archive.Length);
                    }

                    albumViewModel.Album.Archive = fileData;
                }

                _unitOfWork.Album.Add(albumViewModel.Album);
                _unitOfWork.Save();
                TempData["success"] = "Category was created successfully";

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            AlbumViewModel albumFromDb = new()
            {
                SongList = _unitOfWork.Song.GetAll().Select(u => new SelectListItem
                {
                    Text = u.SongTitle,
                    Value = u.Id.ToString()
                }),

                Album = new Album()
                {

                }
            };

            albumFromDb.Album = _unitOfWork.Album.Get(u => u.Id == id);

            if (albumFromDb == null)
            {
                return NotFound();
            }

            return View(albumFromDb);
        }
        [HttpPost]
        public IActionResult Edit(AlbumViewModel albumViewModel, IFormFile? file, IFormFile? archive)
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

                if (archive != null)
                {
                    byte[] fileData;
                    using (var binaryReader = new BinaryReader(archive.OpenReadStream()))
                    {
                        fileData = binaryReader.ReadBytes((int)archive.Length);
                    }

                    albumViewModel.Album.Archive = fileData;
                }

                _unitOfWork.Album.Update(albumViewModel.Album);
                _unitOfWork.Save();
                TempData["success"] = "Album updated successfully";
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }
            Album? albumFromDb = _unitOfWork.Album.Get(u => u.Id == id);

            if (albumFromDb == null)
            {
                return NotFound();
            }
            return View(albumFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(Guid? id)
        {
            Album? obj = _unitOfWork.Album.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                               obj.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Album.Delete(obj);
            _unitOfWork.Save();
            TempData["success"] = "Album was deleted successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Details(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            AlbumViewModel albumFromDb = new()
            {
                SongList = _unitOfWork.Song.GetAll().Select(u => new SelectListItem
                {
                    Text = u.SongTitle,
                    Value = u.Id.ToString()
                }),

                Album = new Album()
                {

                }
            };

            albumFromDb.Album = _unitOfWork.Album.Get(u => u.Id == id);

            if (albumFromDb == null)
            {
                return NotFound();
            }

            return View(albumFromDb);
        }

        public IActionResult DownloadArchive(Guid? id)
        {
            var file = _unitOfWork.Album.Get(u => u.Id == id);
            if (file != null && file.Archive != null)
            {
                byte[] fileData = file.Archive;
                string fileName = file.AlbumTitle + ".zip";

                return File(fileData, "application/zip", fileName);
            }
            else
            {
                return NotFound(); 
            }
        }

        //public IActionResult Upsert(Guid? id)
        //{
        //    AlbumViewModel albumViewModel = new()
        //    {
        //        Album = new Album()
        //        {
        //            ArtistId = id ?? throw new ArgumentNullException(nameof(id)),
        //        }
        //    };

        //    if (_unitOfWork.Album.Get(u => u.ArtistId == id) == null)
        //    {
        //        return View(albumViewModel); // create new album
        //    }
        //    else
        //    {
        //        albumViewModel.Album = _unitOfWork.Album.Get(u => u.ArtistId == id);
        //        return View(albumViewModel);
        //    }
        //}
        //[HttpPost]
        //public IActionResult Upsert(AlbumViewModel albumViewModel, IFormFile? file)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        string wwwRootPath = _webHostEnvironment.WebRootPath;

        //        if (file != null)
        //        {
        //            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //            string productPath = Path.Combine(wwwRootPath, @"images\albums");

        //            if (!string.IsNullOrEmpty(albumViewModel.Album.ImageUrl))
        //            {
        //                //delete the old image
        //                var oldImagePath =
        //                    Path.Combine(wwwRootPath, albumViewModel.Album.ImageUrl.TrimStart('\\'));

        //                if (System.IO.File.Exists(oldImagePath))
        //                {
        //                    System.IO.File.Delete(oldImagePath);
        //                }
        //            }

        //            using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
        //            {
        //                file.CopyTo(fileStream);
        //            }

        //            albumViewModel.Album.ImageUrl = @"\images\albums\" + fileName;
        //        }

        //        if (albumViewModel.Album.Id == Guid.Empty)
        //        {

        //            _unitOfWork.Album.Add(albumViewModel.Album);
        //        }
        //        else
        //        {
        //            _unitOfWork.Album.Update(albumViewModel.Album);
        //        }

        //        _unitOfWork.Save();
        //        TempData["success"] = "Album created successfully";
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        return View(albumViewModel);
        //    }
        //}
    }
}
