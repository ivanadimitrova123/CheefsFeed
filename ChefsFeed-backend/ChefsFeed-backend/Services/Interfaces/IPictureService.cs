namespace ChefsFeed_backend.Services.Interfaces
{
    public interface IPictureService
    {
        Task<string> UploadImageAsync(IFormFile file, long userId);
        Task<byte[]> GetImageAsync(long id);
    }
}
