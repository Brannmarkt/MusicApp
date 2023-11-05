using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MusicApp.Models;
using MusicApp.Models.ViewModels;
using MusicApp.Repositories.Interfaces;
using MusicApp.Services.Interfaces;

namespace MusicApp.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AlbumService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<Album> GetAllAlbums()
        {
            List<Album> albumsList = _unitOfWork.Album.GetAll(includeProperties: "Songs").ToList();

            return albumsList;
        }

        public Album GetAlbum(Guid? id)
        {
            Album album = _unitOfWork.Album.Get(u => u.Id == id);

            return album;
        }

        public void CreateAlbum(AlbumViewModel albumViewModel, IFormFile? file, IFormFile? archive)
        {
            if (file != null)
            {
                albumViewModel.Album.ImageUrl = UploadPicture(albumViewModel, file);
            }

            if (archive != null)
            {
                albumViewModel.Album.Archive = UploadArchive(archive);
            }

            _unitOfWork.Album.Add(albumViewModel.Album);
            _unitOfWork.Save();
        }

        public void EditAlbum(AlbumViewModel albumViewModel, IFormFile? file, IFormFile? archive)
        {
            var existingAlbum = _unitOfWork.Album.Get(u => u.Id == albumViewModel.Album.Id);
            byte[] existingArchive = existingAlbum.Archive;

            _unitOfWork.Album.Detach(existingAlbum);

            existingAlbum = albumViewModel.Album;
            _unitOfWork.Album.Attach(existingAlbum);

            if (file != null)
            {
                existingAlbum.ImageUrl = UploadPicture(albumViewModel, file);
            }

            if (archive != null)
            {
                existingAlbum.Archive = UploadArchive(archive);
            }
            else
            {
                existingAlbum.Archive = existingArchive;
            }

            _unitOfWork.Album.Update(existingAlbum);
            _unitOfWork.Save();
        }

        public void DeleteAlbum(Guid? id)
        {
            Album? obj = _unitOfWork.Album.Get(u => u.Id == id);

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                               obj.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Album.Delete(obj);
            _unitOfWork.Save();
        }

        public byte[] DownloadArchive(Album album)
        {
            byte[] fileData = album.Archive;
            return fileData;
        }

        private string UploadPicture(AlbumViewModel albumViewModel, IFormFile? picture)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            if (picture != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\albums");

                if (!string.IsNullOrEmpty(albumViewModel.Album.ImageUrl))
                {
                    //delete the old image
                    var oldImagePath =
                        Path.Combine(wwwRootPath, albumViewModel.Album.ImageUrl.TrimStart('\\'));

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

        private byte[] UploadArchive(IFormFile? archive)
        {
            byte[] fileData;
            using (var binaryReader = new BinaryReader(archive.OpenReadStream()))
            {
                fileData = binaryReader.ReadBytes((int)archive.Length);
            }

            return fileData;
        }
    }
}
