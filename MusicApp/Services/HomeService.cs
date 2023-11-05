using MusicApp.Models;
using MusicApp.Repositories.Interfaces;
using MusicApp.Services.Interfaces;

namespace MusicApp.Services
{
    public class HomeService : IHomeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<Artist> GetAllArtists()
        {
            List<Artist> artistsList = _unitOfWork.Artist.GetAll(includeProperties: "Albums").ToList();

            return artistsList;
        }

        public Artist GetArtist(Guid? id)
        {
            Artist artist = _unitOfWork.Artist.Get(u => u.Id == id, includeProperties: "Albums");

            return artist;
        }
    }
}
