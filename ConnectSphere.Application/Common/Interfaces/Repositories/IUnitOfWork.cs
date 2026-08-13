using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IPostRepository PostRepository { get; }
        IReactionRepository ReactionRepository { get; }
        ITagRepository TagRepository { get; }
        IFollowRepository FollowRepository { get; }
        ICommentRepository CommentRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IBookmarkRepository BookmarkRepository { get; }


        Task<int> CompleteAsync();
    }
}
