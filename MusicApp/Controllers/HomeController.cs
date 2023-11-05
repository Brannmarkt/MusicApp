using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using MusicApp.Models;
using MusicApp.Repositories.Interfaces;
using MusicApp.Services.Interfaces;
using System.Diagnostics;

namespace MusicApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IHomeService homeService, ILogger<HomeController> logger)
        {
            _homeService = homeService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Artist> artistList = _homeService.GetAllArtists();
            return View(artistList);
        }
        [HttpPost]
        public IActionResult Index(string ArtistName)
        {
            List<Artist> artistList = _homeService.GetAllArtists();

            if (!String.IsNullOrEmpty(ArtistName))
            {
                artistList = artistList.Where(u => u.ArtistName.ToLower().Contains(ArtistName)).ToList();
            }
            
            return View(artistList);
        }

        public IActionResult Details(Guid id)
        {
            Artist artist = _homeService.GetArtist(id);

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