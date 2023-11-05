using MusicApp.Models;
using MusicApp.Models.ViewModels;
using MusicApp.Repositories.Interfaces;
using MusicApp.Services.Interfaces;

namespace MusicApp.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArtistService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
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

        public void UpsertArtist(ArtistViewModel artistViewModel, IFormFile? file)
        {
            if (file != null)
            {
                artistViewModel.Artist.ImageUrl = UploadPicture(artistViewModel, file);
            }

            if (artistViewModel.Artist.Id == Guid.Empty)
            {
                _unitOfWork.Artist.Add(artistViewModel.Artist);
            }
            else
            {
                _unitOfWork.Artist.Update(artistViewModel.Artist);
            }

            _unitOfWork.Save();
        }

        public void DeleteArtist(Guid? id)
        {
            Artist? obj = _unitOfWork.Artist.Get(u => u.Id == id);

            if (obj.ImageUrl != null)
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                               obj.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _unitOfWork.Artist.Delete(obj);
            _unitOfWork.Save();
        }


        private string UploadPicture(ArtistViewModel artistViewModel, IFormFile? picture)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            if (picture != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\albums");

                if (!string.IsNullOrEmpty(artistViewModel.Artist.ImageUrl))
                {
                    //delete the old image
                    var oldImagePath =
                        Path.Combine(wwwRootPath, artistViewModel.Artist.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    picture.CopyTo(fileStream);
                }

                return @"\images\albums\" + fileName;
            }

            return null!;
        }
    }
}
