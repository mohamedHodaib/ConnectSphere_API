using AutoMapper;
using AutoMapper.QueryableExtensions;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Common.Pagination;
using ConnectSphere.Domain.Entities;
using ConnectSphere.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ConnectSphere.Infrastructure.Persistence.Repositories
{
    public class BookmarkRepository : RepositoryBase<Bookmark>, IBookmarkRepository
    {
        private readonly IMapper _mapper;

        public BookmarkRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public void CreateBookmark(Bookmark bookmark) => Create(bookmark);

        public void DeleteBookmark(Bookmark bookmark) => Delete(bookmark);

        public async Task<Bookmark?> GetBookmark(Guid userId, Guid postId, bool trackChanges = true)
            => await FindByCondition(b => b.BookmarkedUserId == userId && b.PostId == postId, trackChanges)
                .SingleOrDefaultAsync();

        public async Task<bool> BookmarkExistsAsync(Guid userId, Guid postId)
            => await FindByCondition(b => b.BookmarkedUserId == userId && b.PostId == postId, trackChanges: false)
                .AnyAsync();

        public async Task<PagedList<PostDetailsDto>> GetUserBookmarksAsync(Guid userId, PaginationParameters paginationParameters)
        {
            var query = FindByCondition(b => b.BookmarkedUserId == userId, trackChanges: false)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => b.Post);

            var projectedQuery = query.ProjectTo<PostDetailsDto>(_mapper.ConfigurationProvider);

            return await PagedList<PostDetailsDto>
                .ToPagedList(projectedQuery, paginationParameters.PageNumber, paginationParameters.PageSize);
        }
    }
}