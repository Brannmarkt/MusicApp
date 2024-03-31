﻿using MusicApp.Data;
using MusicApp.Repositories.Interfaces;

namespace MusicApp.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _context;
        public IArtistRepository Artist { get; private set; }
        public IAlbumRepository Album { get; private set; }
        public ISongRepository Song { get; private set; }
        public IUserRepository User { get; private set; }
        public IRoleRepository Role { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
           _context = context;

            Artist = new ArtistRepository(_context);
            Album = new AlbumRepository(_context);
            Song = new SongRepository(_context);
            User = new UserRepository(_context);
            Role = new RoleRepository(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
