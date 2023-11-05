using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using MusicApp.Extensions;
using MusicApp.Models;
using MusicApp.Models.ViewModels;
using MusicApp.Repositories.Interfaces;
using MusicApp.Services.Interfaces;
using System.Web;

namespace MusicApp.Controllers
{
    public class SongController : Controller
    {
        private readonly ISongService _songService;
        private readonly IUnitOfWork _unitOfWork;

        public SongController(ISongService songService, IUnitOfWork unitOfWork)
        {
            _songService = songService;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {


            return View();
        }

        public IActionResult Create(Guid id)
        {
            SongViewModel songViewModel = new SongViewModel()
            {
                Song = new Song()
                {
                    AlbumId = id,
                }
            };

            return View(songViewModel);
        }
        [HttpPost]
        public IActionResult Create(SongViewModel songViewModel, IFormFile? file)
        {
            if(ModelState.IsValid)
            {
                _songService.CreateSong(songViewModel, file);

                TempData["success"] = "Song was created successfully";

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

            SongViewModel songFromDb = new()
            {
                Song = new Song()
                {

                }
            };
            songFromDb.Song = _songService.GetSong(id);

            if (songFromDb == null)
            {
                return NotFound();
            }

            return View(songFromDb);
        }
        [HttpPost]
        public IActionResult Edit(SongViewModel songViewModel, IFormFile? file)
        {
            if(ModelState.IsValid)
            {
                _songService.EditSong(songViewModel, file);

                TempData["success"] = "Song was created successfully";

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

            Song? songFromDb = _songService.GetSong(id);

            if (songFromDb == null)
            {
                return NotFound();
            }

            return View(songFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(Guid? id)
        {
            _songService.DeleteSong(id);

            TempData["success"] = "Song was deleted successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Details(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            SongViewModel songFromDb = new()
            {
                Song = new Song()
                {

                }
            };
            songFromDb.Song = _songService.GetSong(id);

            if (songFromDb == null)
            {
                return NotFound();
            }

            return View(songFromDb);
        }
    }
}
