using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Domain.Entities;
using ConnectSphere.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using ConnectSphere.Application.Common.Pagination;

namespace ConnectSphere.Infrastructure.Repositories
{
    internal class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        private readonly IMapper _mapper;   

        public PostRepository(ApplicationDbContext context,IMapper mapper)
          : base(context)
        {
            _mapper = mapper;
        }

        public void CreatePost(Post post) => Create(post);

        public async Task<PagedList<PostDetailsDto>> GetFeedAsync(Guid userId, PaginationParameters paginationParameters)
        {
            var query = FindByCondition(
                p => p.User.Followers.Any(f => f.FollowerId == userId)
                , false)
                .OrderByDescending(p => p.CreatedAt);

            var projectedQuery = query.ProjectTo<PostDetailsDto>(_mapper.ConfigurationProvider);

           return await PagedList<PostDetailsDto>
                .ToPagedList(projectedQuery, paginationParameters.PageNumber,paginationParameters.PageSize);
        }
        public async Task<Post?> GetPostByIdAsync(Guid id, bool trackChanges = true) =>
          await FindByCondition(p => p.Id == id, trackChanges)
           .SingleOrDefaultAsync();

        public async Task<Post?> GetPostByIdIncludeUserAndImagesAsync(Guid id, bool trackChanges = true) =>
          await FindByCondition(p => p.Id == id, trackChanges, q => q.Include(p => p.User).Include(p => p.Images))
           .SingleOrDefaultAsync();

        public async Task<Post?> GetPostByIdIncludeTagsAsync(Guid id, bool trackChanges = true) =>
          await FindByCondition(p => p.Id == id, trackChanges,q => q.Include(p => p.Tags))
           .SingleOrDefaultAsync();

        public void DeletePost(Post post) => Delete(post);

        public Task<bool> PostExistsAsync(Guid id) => IsExist(p => p.Id == id);

        
    }
}
