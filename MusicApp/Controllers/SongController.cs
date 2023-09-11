using Microsoft.AspNetCore.Mvc;
using MusicApp.Models;
using MusicApp.Models.ViewModels;
using MusicApp.Repositories.Interfaces;

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

        //public IActionResult Upsert(Guid? id)
        //{
        //    SongViewModel songViewModel = new()
        //    {
        //        Song = new Song()
        //    };

        //    if (id == null)
        //    {
        //        return View(songViewModel); // create new artist
        //    }
        //    else
        //    {
        //        songViewModel.Song = _unitOfWork.Song.Get(u => u.Id == id);
        //        return View(songViewModel);
        //    }
        //}
        //[HttpPost]
        //public IActionResult Upsert(SongViewModel songViewModel, IFormFile file)
        //{
        //    if(ModelState.IsValid)
        //    {

        //    }
        //    else
        //    {
        //        return View(songViewModel);
        //    }
        //}
    }
}
