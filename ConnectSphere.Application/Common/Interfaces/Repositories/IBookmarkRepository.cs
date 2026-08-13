using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Common.Pagination;
using ConnectSphere.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.Interfaces.Repositories
{
    public interface IBookmarkRepository : IRepositoryBase<Bookmark>
    {
        void CreateBookmark(Bookmark bookmark);
        Task<Bookmark?> GetBookmark(Guid userId, Guid postId, bool trackChanges = true);
        void DeleteBookmark(Bookmark bookmark);
        Task<bool> BookmarkExistsAsync(Guid userId, Guid postId);
        Task<PagedList<PostDetailsDto>> GetUserBookmarksAsync(Guid userId, PaginationParameters paginationParameters);
    }
}