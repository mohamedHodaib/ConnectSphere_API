using ConnectSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.Interfaces.Repositories
{
    public interface IFollowRepository : IRepositoryBase<Follow>
    {
        void CreateFollow(Follow follow);
        Task<Follow?> GetFollow(Guid followerId, Guid followingId,bool trackChanges = true);
        void DeleteFollow(Follow follow);
        Task<bool> FollowExistsAsync(Guid followerId, Guid followingId);
    }
}
