using ConnectSphere.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ConnectSphere.Infrastructure.Services
{
    public class UrlService : IUrlService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UrlService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetAbsoluteUrl(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return null;

            var request = _httpContextAccessor.HttpContext!.Request;

            return $"{request.Scheme}://{request.Host}{relativePath}";
        }
    }
}
