using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Repositories.Interfaces;
using ChefsFeed_backend.Services.Interfaces;

namespace ChefsFeed_backend.Services.Implementation
{
    public class PictureService : IPictureService
    {
        private readonly IPictureRepository _pictureRepository;
        private readonly IUserRepository _userRepository;

        public PictureService(IPictureRepository pictureRepository, IUserRepository userRepository)
        {
            _pictureRepository = pictureRepository;
            _userRepository = userRepository;
        }

        public async Task<string> UploadImageAsync(IFormFile file, long userId)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Invalid file");
            }

            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var image = new Picture
                {
                    FileName = file.FileName,
                    ImageData = memoryStream.ToArray(),
                    ContentType = file.ContentType
                };

                await _pictureRepository.AddAsync(image);
                await _pictureRepository.SaveChangesAsync();

                user.ProfilePictureId = image.Id;
                await _userRepository.SaveChangesAsync();

                // Construct the image URL
                string imageUrl = $"https://yourdomain.com/api/image/{image.Id}";
                return imageUrl;
            }
        }

        public async Task<byte[]> GetImageAsync(long id)
        {
            var image = await _pictureRepository.GetByIdAsync(id);
            return image?.ImageData;
        }
    }
}
