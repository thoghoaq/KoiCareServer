using Microsoft.AspNetCore.Http;

namespace KoiCare.Application.Abtractions.Storage
{
    public interface IStorageService
    {
        public Task<string> UploadImageAsync(IFormFile file, string folderPath);
        public Task<List<string>> UploadMultipleImagesAsync(List<IFormFile> files, string folderPath);
        public Task DeleteImageAsync(string imageUrl);
    }
}
