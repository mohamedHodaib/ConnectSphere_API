using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.DTOs
{
    public record CreatePostDto(string? Content,List<string>?Tags, List<IFormFile>? Images);

    public record UpdatePostDto(string? Content,List<string>?Tags);

    public record PostDetailsDto
    {
        public Guid Id { get; init; }
        public string? Content { get; init; }
        public string? Title { get; init; }            // short title or headline
        public List<string>? ImagesUrls { get; set; } // fixed spelling (URLs)
        public List<string>? Tags { get; init; }
        public DateTime CreatedAt { get; init; }

        public Guid AuthorId { get; init; }
        public string AuthorName { get; init; } = null!;
        public string? AuthorUsername { get; init; }          // e.g. "miacarter"
        public string? AuthorProfilePictureUrl { get; init; } // avatar URL

        public string? OriginalUrl { get; init; } // where "View original" should point
        public int ReactionsCount { get; init; }
    }


}
