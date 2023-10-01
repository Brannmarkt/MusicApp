namespace MusicApp.Extensions
{
    public static class IFormFileExtensions
    {
        public static byte[] GetBytes(this IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            formFile.CopyToAsync(memoryStream);

            return memoryStream.ToArray();
        }
    }
}
