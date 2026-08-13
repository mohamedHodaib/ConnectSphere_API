using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Comments.Queries.ViewComments
{
    public sealed record ViewCommentsQuery(Guid PostId,PaginationParameters PaginationDto)
        : IRequest<Result<PagedResult<CommentDetailsDto>>>;
}
