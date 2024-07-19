using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicApp.Data;
using MusicApp.Models;
using MusicApp.Models.ViewModels;
using MusicApp.Repositories.Interfaces;
using MusicApp.Services.Interfaces;

namespace MusicApp.Controllers
{
    
    public class ArtistController : Controller
    {
        private readonly IArtistService _artistService;
        private readonly IUnitOfWork _unitOfWork;

        public ArtistController(IArtistService artistService, IUnitOfWork unitOfWork)
        {
            _artistService = artistService;
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = $"{Constants.Roles.Manager},{Constants.Roles.Administrator}")]
        public IActionResult Index()
        {
            List<Artist> artistsList = _artistService.GetAllArtists();

            return View(artistsList);
        }

        //public static bool UpdateOrCreate;
        //[Authorize(Roles = $"{Constants.Roles.Manager},{Constants.Roles.Administrator}")]
        //public IActionResult Upsert(Guid? id)
        //{
        //    ArtistViewModel artistViewModel = new()
        //    {
        //        AlbumList = _unitOfWork.Album.GetAll().Select(u => new SelectListItem
        //        {
        //            Text = u.AlbumTitle,
        //            Value = u.Id.ToString()
        //        }),

        //        Artist = new Artist()
        //    };

        //    if (id == null)
        //    {
        //        UpdateOrCreate = false;
        //        return View(artistViewModel); // create new artist
        //    }
        //    else
        //    {
        //        artistViewModel.Artist = _artistService.GetArtist(id); // edit artist
        //        UpdateOrCreate = true;
        //        return View(artistViewModel);
        //    }
        //}
        //[HttpPost]
        //public IActionResult Upsert(ArtistViewModel artistViewModel, IFormFile? file)
        //{
        //    if (UpdateOrCreate == true)
        //    {
        //        artistViewModel.UpdateOrCreate = true;
        //    }

        //    if(ModelState.IsValid)
        //    {
        //       _artistService.UpsertArtist(artistViewModel, file);

        //        TempData["success"] = "Artist created successfully";
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        artistViewModel.AlbumList = _unitOfWork.Album.GetAll().Select(u => new SelectListItem
        //        {
        //            Text = u.AlbumTitle,
        //            Value = u.Id.ToString()
        //        });
        //        return View(artistViewModel);
        //    }
        //}


        [Authorize(Roles = $"{Constants.Roles.Manager},{Constants.Roles.Administrator}")]
        public IActionResult Create(Guid id)
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

            return View(artistViewModel);
        }
        [HttpPost]
        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,
        ValueLengthLimit = int.MaxValue)]
        public IActionResult Create(ArtistViewModel artistViewModel, IFormFile? file)
        {
            if(ModelState.IsValid)
            {
                _artistService.CreateArtist(artistViewModel, file);
                TempData["success"] = "Category was created successfully";

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = $"{Constants.Roles.Manager},{Constants.Roles.Administrator}")]
        public IActionResult Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            ArtistViewModel artistFromDb = new()
            {
                AlbumList = _unitOfWork.Album.GetAll().Select(u => new SelectListItem
                {
                    Text = u.AlbumTitle,
                    Value = u.Id.ToString()
                }),

                Artist = new Artist()
            };

            artistFromDb.Artist = _artistService.GetArtist(id);

            if (artistFromDb == null)
            {
                return NotFound();
            }

            return View(artistFromDb);

        }
        [HttpPost]
        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,
        ValueLengthLimit = int.MaxValue)]
        public IActionResult Edit(ArtistViewModel artistViewModel, IFormFile? file)
        {
            if(ModelState.IsValid)
            {
                _artistService.UpdateArtist(artistViewModel, file);

                TempData["success"] = "Album updated successfully";
                return RedirectToAction("Index");
            }

            return View("Index");
        }

        [Authorize(Roles = $"{Constants.Roles.Manager},{Constants.Roles.Administrator}")]
        public IActionResult Delete(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }
            Artist? artistFromDb = _artistService.GetArtist(id);

            if (artistFromDb == null)
            {
                return NotFound();
            }
            return View(artistFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(Guid? id)
        {
            _artistService.DeleteArtist(id);

            TempData["success"] = "Artist was deleted successfully";
            return RedirectToAction("Index");
        }

    }
}
