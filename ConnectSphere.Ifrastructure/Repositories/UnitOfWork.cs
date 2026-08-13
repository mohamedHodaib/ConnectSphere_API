using AutoMapper;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Infrastructure.Persistence;
using ConnectSphere.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Infrastructure.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IPostRepository> _postRepository;
        private readonly Lazy<IReactionRepository> _reactionRepository;
        private readonly Lazy<IFollowRepository> _followRepository;
        private readonly Lazy<ITagRepository> _tagRepository;
        private readonly Lazy<ICommentRepository> _commentRepository;
        private readonly Lazy<INotificationRepository> _notificationRepository;
        private readonly Lazy<IBookmarkRepository> _bookmarkRepository;

        private readonly ApplicationDbContext _appDbContext;

        public UnitOfWork(ApplicationDbContext appDbContext,IMapper mapper)
        {
            _appDbContext = appDbContext;
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(appDbContext,mapper));
            _postRepository = new Lazy<IPostRepository>(() => new PostRepository(appDbContext,mapper));
            _reactionRepository = new Lazy<IReactionRepository>(() => new ReactionRepository(appDbContext,mapper));
            _followRepository = new Lazy<IFollowRepository>(() => new FollowRepository(appDbContext));
            _tagRepository = new Lazy<ITagRepository>(() => new TagRepository(appDbContext));
            _commentRepository = new Lazy<ICommentRepository>(() => new CommentRepository(appDbContext, mapper));
            _notificationRepository = new Lazy<INotificationRepository>(() => new NotificationRepository(appDbContext, mapper));
            _bookmarkRepository = new Lazy<IBookmarkRepository>(() => new BookmarkRepository(appDbContext,mapper));
        }


        public IUserRepository UserRepository => _userRepository.Value;
        public IPostRepository PostRepository => _postRepository.Value;
        public IReactionRepository ReactionRepository => _reactionRepository.Value;
        public IFollowRepository FollowRepository => _followRepository.Value;
        public ITagRepository TagRepository => _tagRepository.Value;
        public ICommentRepository CommentRepository => _commentRepository.Value;
        public INotificationRepository NotificationRepository => _notificationRepository.Value;
        public IBookmarkRepository BookmarkRepository => _bookmarkRepository.Value;


        public async Task<int> CompleteAsync() => await _appDbContext.SaveChangesAsync();

        public void Dispose() => _appDbContext.Dispose();
    }
}
