using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectSphere.Application.Common.Pagination;

namespace ConnectSphere.Application.Common.Interfaces.Repositories
{
    public interface IPostRepository : IRepositoryBase<Post>
    {
        void CreatePost(Post post);
        Task<PagedList<PostDetailsDto>> GetFeedAsync(Guid userId,PaginationParameters paginationParameters);
        Task<Post?> GetPostByIdAsync(Guid id, bool trackChanges = true);
        Task<Post?> GetPostByIdIncludeTagsAsync(Guid id, bool trackChanges = true);
        Task<Post?> GetPostByIdIncludeUserAndImagesAsync(Guid id, bool trackChanges = true);
        void DeletePost(Post post);
        Task<bool> PostExistsAsync(Guid id);

    }
}
