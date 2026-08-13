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
    internal class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        private readonly IMapper _mapper;
        public CommentRepository(ApplicationDbContext context, IMapper mapper)
         : base(context)
        {
            _mapper = mapper;
        }

        public void CreateComment(Comment comment) => Create(comment);

        public async Task<PagedList<CommentDetailsDto>> GetCommentsAsync(Guid postId, PaginationParameters paginationParameters)
        {
            var query = FindByCondition(c => c.PostId == postId, false
                , q => q.Include(p => p.User))
                .OrderByDescending(p => p.CreatedAt);

            var projectedQuery = query.ProjectTo<CommentDetailsDto>(_mapper.ConfigurationProvider);

            return await PagedList<CommentDetailsDto>
                 .ToPagedList(projectedQuery, paginationParameters.PageNumber, paginationParameters.PageSize);
        }

        public async Task<Comment?> GetCommentByIdAsync(Guid commentId, bool trackChanges = true) =>
            await FindByCondition(c => c.Id == commentId, trackChanges)
            .FirstOrDefaultAsync();

        public void DeleteComment(Comment comment) => Delete(comment);
    }
}