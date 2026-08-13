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
    internal class FollowRepository : RepositoryBase<Follow>, IFollowRepository
    {
        public FollowRepository(ApplicationDbContext context)
          : base(context)
        {
        }

        public void CreateFollow(Follow follow) => Create(follow);

        public async Task<Follow?> GetFollow(Guid followerId, Guid followingId,bool trackChanges = true) =>
             await FindByCondition(f => f.FollowerId == followerId && f.FollowedUserId == followingId,trackChanges)
            .FirstOrDefaultAsync();

        public void DeleteFollow(Follow follow) => Delete(follow);

        public Task<bool> FollowExistsAsync(Guid followerId, Guid followingId) => 
            IsExist(f => f.FollowerId == followerId && f.FollowedUserId == followingId);


        

    }
}
