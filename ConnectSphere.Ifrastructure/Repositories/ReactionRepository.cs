using AutoMapper;
using AutoMapper.QueryableExtensions;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Pagination;
using ConnectSphere.Domain.Entities;
using ConnectSphere.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Infrastructure.Repositories
{
    internal class ReactionRepository : RepositoryBase<Reaction>, IReactionRepository
    {
        private readonly IMapper _mapper;

        public ReactionRepository(ApplicationDbContext context, IMapper mapper)
          : base(context)
        {
            _mapper = mapper;
        }

        public void CreateReaction(Reaction reaction) => CreateReaction(reaction);

        public async Task<Reaction?> GetReactionByPostIdAndUserIdAsync(Guid postId, Guid userId, bool trackChanges = true) =>
            await FindByCondition(x => x.ReactedUserId == userId && x.PostId == postId, trackChanges)
            .FirstOrDefaultAsync();

        public async Task<PagedList<PostDetailsDto>> GetUserReactedPostsAsync(
            Guid userId, PaginationParameters paginationParameters, bool trackChanges = false)
        {
            var postsQuery = FindByCondition(r => r.ReactedUserId == userId, trackChanges)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => r.Post);

            var projectedQuery = postsQuery.ProjectTo<PostDetailsDto>(_mapper.ConfigurationProvider);

            return await PagedList<PostDetailsDto>
                .ToPagedList(projectedQuery, paginationParameters.PageNumber, paginationParameters.PageSize);
        }

        public void DeleteReaction(Reaction reaction) => Delete(reaction);
    }
}
