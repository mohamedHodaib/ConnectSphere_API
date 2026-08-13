using ConnectSphere.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ConnectSphere.Infrastructure.Services
{
    public class SaveFile : ISaveFile
    {
        private readonly IWebHostEnvironment _environment;

        public SaveFile(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveAsync(
            IFormFile file,
            string folderName,
            CancellationToken cancellationToken = default)
        {
            
            var uploadsFolder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                folderName);

            if(!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName =
                $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(
                filePath,
                FileMode.Create);

            await file.CopyToAsync(stream, cancellationToken);

            return $"/uploads/{folderName}/{fileName}";
        }
    }
}