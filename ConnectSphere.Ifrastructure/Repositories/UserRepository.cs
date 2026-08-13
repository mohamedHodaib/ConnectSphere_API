using AutoMapper;
using AutoMapper.QueryableExtensions;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Pagination;
using ConnectSphere.Domain.Entities;
using ConnectSphere.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Infrastructure.Repositories
{
    internal class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext appDbContext,IMapper mapper)
            : base(appDbContext)
        {
            _mapper = mapper;
        }

        public async Task<bool> UserExistsAsync(Guid id) => await IsExist(x => x.Id == id);

        public async Task<User?> GetByIdAsync(Guid id, bool trackChanges = true) =>
            await FindByCondition(u => u.Id == id, trackChanges)
            .FirstOrDefaultAsync();

        public async Task<PublicProfileInfoDto?> GetPublicProfileinfoByIdAsync(Guid id,bool trackChanges = true) =>
            await FindByCondition(u => u.Id == id,trackChanges)
            .ProjectTo<PublicProfileInfoDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();


        public async Task<OwnProfileInfoDto?> GetOwnProfileinfoByIdAsync(Guid id, bool trackChanges = true) =>
            await FindByCondition(u => u.Id == id, trackChanges)
            .ProjectTo<OwnProfileInfoDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        public async Task<OwnSettingsDto?> GetOwnSettingsByIdAsync(Guid id, bool trackChanges = true) =>
            await FindByCondition(u => u.Id == id, trackChanges)
            .ProjectTo<OwnSettingsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        public async Task<PublicProfileInfoDto?> GetPublicProfileInfoByUsernameAsync(string username, bool trackChanges = false) => 
            await FindByCondition(u => u.NormalizedUserName == username.ToUpperInvariant(), trackChanges
                ,q => q.Include(u => u.Followers)
                .Include(u => u.Following)
                .Include(u => u.Posts))
            .ProjectTo<PublicProfileInfoDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();

        public async Task<PagedList<PublicProfileInfoDto>> GetSuggestionsAsync(
            Guid userId,
            IReadOnlySet<Guid> execludedUserIds,
            PaginationParameters paginationParameters)
        {
            // Users that the current user already follows
            var followingIds = await FindByCondition(u => u.Id == userId, false)
                .SelectMany(u => u.Following.Select(f => f.FollowedUserId))
                .ToListAsync();


            var query = FindByCondition(u =>
                    u.Id != userId &&
                    !followingIds.Contains(u.Id) &&
                    !execludedUserIds.Contains(u.Id),
                    false)
                .OrderByDescending(u => u.Followers.Count(f => followingIds.Contains(f.FollowerId)))
                .ThenByDescending(u => u.Followers.Count())
                .ThenBy(u => u.Id);

            var projectedQuery = query.ProjectTo<PublicProfileInfoDto>(_mapper.ConfigurationProvider);

            return await PagedList<PublicProfileInfoDto>.ToPagedList(
                projectedQuery,
                paginationParameters.PageNumber,
                paginationParameters.PageSize);
        }


        public void DeleteUser(User user) => Delete(user);
    }
}
