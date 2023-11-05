using MusicApp.Models.ViewModels;
using MusicApp.Models;
using MusicApp.Services.Interfaces;
using MusicApp.Repositories.Interfaces;

namespace MusicApp.Services
{
    public class SongService : ISongService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SongService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public Song GetSong(Guid? id)
        {
            Song song = _unitOfWork.Song.Get(u => u.Id == id);

            return song;
        }

        public void CreateSong(SongViewModel songViewModel, IFormFile? file)
        {
            if(file != null)
            {
                songViewModel.Song.Content = UploadSong(file);
                songViewModel.Song.ContentType = file.ContentType;
            }

            _unitOfWork.Song.Add(songViewModel.Song);
            _unitOfWork.Save();
        }

        public void EditSong(SongViewModel songViewModel, IFormFile? file)
        {
            var existingSong = _unitOfWork.Song.Get(u => u.Id == songViewModel.Song.Id);
            byte[] existingSongContent = existingSong.Content;

            _unitOfWork.Song.Detach(existingSong);

            existingSong = songViewModel.Song;
            _unitOfWork.Song.Attach(existingSong);

            if (file != null)
            {
                existingSong.Content = UploadSong(file);
                existingSong.ContentType = file.ContentType;
            }
            else
            {
                existingSong.Content = existingSongContent;
            }

            _unitOfWork.Song.Update(existingSong);
            _unitOfWork.Save();
        }

        public void DeleteSong(Guid? id)
        {
            Song? obj = _unitOfWork.Song.Get(u => u.Id == id);

            _unitOfWork.Song.Delete(obj);
            _unitOfWork.Save();
        }


        private byte[] UploadSong(IFormFile? song)
        {
            byte[] fileData;
            using (var binaryReader = new BinaryReader(song.OpenReadStream()))
            {
                fileData = binaryReader.ReadBytes((int)song.Length);
            }

            return fileData;
        }
    }
}
