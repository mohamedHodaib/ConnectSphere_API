using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Pagination;
using ConnectSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.Interfaces.Repositories
{
    public interface IReactionRepository : IRepositoryBase<Reaction>
    {
        void CreateReaction(Reaction reaction);
        Task<Reaction?> GetReactionByPostIdAndUserIdAsync(Guid postId,Guid userId, bool trackChanges = true);

        Task<PagedList<PostDetailsDto>> GetUserReactedPostsAsync(
            Guid userId, PaginationParameters paginationParameters, bool trackChanges = false);
        void DeleteReaction(Reaction reaction);
    }
}
