using Microsoft.AspNetCore.Http;

namespace ConnectSphere.Application.Common.Interfaces.Services
{
    public interface ISaveFile
    {
        Task<string> SaveAsync(
            IFormFile file,
            string folderName,
            CancellationToken cancellationToken = default);
    }
}