using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using MusicApp.Models;
using MusicApp.Repositories.Interfaces;
using System.Diagnostics;

namespace MusicApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Artist> artistList = _unitOfWork.Artist.GetAll().ToList();
            return View(artistList);
        }
        [HttpPost]
        public IActionResult Index(string ArtistName)
        {
            List<Artist> artistList = _unitOfWork.Artist.GetAll().ToList();

            if (!String.IsNullOrEmpty(ArtistName))
            {
                artistList = artistList.Where(u => u.ArtistName.Contains(ArtistName)).ToList();
            }
            
            return View(artistList);
        }

        public IActionResult Details(Guid id)
        {
            Artist artist = _unitOfWork.Artist.Get(u => u.Id == id, includeProperties: "Albums");
            return View(artist);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}