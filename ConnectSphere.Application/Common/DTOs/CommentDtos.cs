using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.DTOs
{
    public record AddCommentDto(Guid PostId,string Content);

    public record CommentDetailsDto
    {
        public Guid Id { get; init; }
        public Guid PostId { get; init; }
        public Guid CommentedUserId { get; init; }
        public string CommentedUserDisplayName { get; init; } = null!;
        public string Content { get; init; } = null!;
    }
}
