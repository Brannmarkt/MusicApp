namespace MusicApp.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IArtistRepository Artist { get; }
        IAlbumRepository Album { get; }
        ISongRepository Song { get; }

        void Save();
    }
}
