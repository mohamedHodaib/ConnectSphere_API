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
    public interface ICommentRepository : IRepositoryBase<Comment>
    {
        void CreateComment(Comment comment);

        Task<PagedList<CommentDetailsDto>> GetCommentsAsync(Guid postId, PaginationParameters paginationParameters);
        Task<Comment?> GetCommentByIdAsync(Guid commentId, bool trackChanges = true);
        void DeleteComment(Comment comment);
    }
}
