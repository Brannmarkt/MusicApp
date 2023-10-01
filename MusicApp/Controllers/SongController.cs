using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using MusicApp.Extensions;
using MusicApp.Models;
using MusicApp.Models.ViewModels;
using MusicApp.Repositories.Interfaces;
using System.Web;

namespace MusicApp.Controllers
{
    public class SongController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SongController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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
                if (file != null)
                {
                    byte[] fileData;
                    using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                    {
                        fileData = binaryReader.ReadBytes((int)file.Length);
                    }

                    songViewModel.Song.Content = fileData;
                    songViewModel.Song.ContentType = file.ContentType;
                }

                _unitOfWork.Song.Add(songViewModel.Song);
                _unitOfWork.Save();
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
            songFromDb.Song = _unitOfWork.Song.Get(u => u.Id == id);

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
                if (file != null)
                {
                    byte[] fileData;
                    using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                    {
                        fileData = binaryReader.ReadBytes((int)file.Length);
                    }

                    songViewModel.Song.Content = fileData;
                    songViewModel.Song.ContentType = file.ContentType;
                }

                _unitOfWork.Song.Update(songViewModel.Song);
                _unitOfWork.Save();
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
            Song? songFromDb = _unitOfWork.Song.Get(u => u.Id == id);

            if (songFromDb == null)
            {
                return NotFound();
            }
            return View(songFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(Guid? id)
        {
            Song? obj = _unitOfWork.Song.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            
            _unitOfWork.Song.Delete(obj);
            _unitOfWork.Save();
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
            songFromDb.Song = _unitOfWork.Song.Get(u => u.Id == id);

            if (songFromDb == null)
            {
                return NotFound();
            }

            return View(songFromDb);
        }
    }
}
