using System;

namespace ConnectSphere.Domain.Entities
{
    public class Bookmark
    {
        public Guid PostId { get; private set; }
        public Guid BookmarkedUserId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        //For RelationShips
        public User User { get; set; } = null!;
        public Post Post { get; set; } = null!;

        private Bookmark() { }

        public static Bookmark Create(Guid userId, Guid postId)
        {
            return new Bookmark
            {
                PostId = postId,
                BookmarkedUserId = userId,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}