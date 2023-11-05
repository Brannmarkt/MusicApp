using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicApp.Models;
using MusicApp.Models.ViewModels;
using MusicApp.Repositories;
using MusicApp.Repositories.Interfaces;
using MusicApp.Services;
using MusicApp.Services.Interfaces;

namespace MusicApp.Controllers
{
    public class AlbumController : Controller
    {
        private readonly IAlbumService _albumService;
        private readonly IUnitOfWork _unitOfWork;

        public AlbumController(IAlbumService albumService, IUnitOfWork unitOfWork)
        {
            _albumService = albumService;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Album> albumList = _albumService.GetAllAlbums();

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
        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,
        ValueLengthLimit = int.MaxValue)]
        public IActionResult Create(AlbumViewModel albumViewModel, IFormFile? file, IFormFile? archive)
        {
            if (ModelState.IsValid)
            {
                _albumService.CreateAlbum(albumViewModel, file, archive);

                TempData["success"] = "Category was created successfully";

                return RedirectToAction("Index");
            }

            //return View();
            return RedirectToAction("Index", "Artist");
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
        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,
        ValueLengthLimit = int.MaxValue)]
        public IActionResult Edit(AlbumViewModel albumViewModel, IFormFile? file, IFormFile? archive)
        {
            if(ModelState.IsValid)
            {
                _albumService.EditAlbum(albumViewModel, file, archive);

                TempData["success"] = "Album updated successfully";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }
            Album? albumFromDb = _albumService.GetAlbum(id);

            if (albumFromDb == null)
            {
                return NotFound();
            }
            return View(albumFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(Guid? id)
        {
            _albumService.DeleteAlbum(id);

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

            albumFromDb.Album = _albumService.GetAlbum(id);

            if (albumFromDb == null)
            {
                return NotFound();
            }

            return View(albumFromDb);
        }

        public IActionResult DownloadArchive(Guid? id)
        {
            var file = _albumService.GetAlbum(id);
            if (file != null && file.Archive != null)
            {
                byte[] fileData = _albumService.DownloadArchive(file);
                string fileName = file.AlbumTitle + ".zip";

                return File(fileData, "application/zip", fileName);
            }
            else
            {
                return NotFound(); 
            }
        }

    }
}
