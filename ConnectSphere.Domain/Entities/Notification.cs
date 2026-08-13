using ConnectSphere.Domain.Enums;

namespace ConnectSphere.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }          // recipient
        public NotificationType Type { get; private set; }
        public string Title { get; private set; } = null!;
        public string Message { get; private set; } = null!;
        public Guid? ActorId { get; private set; }         // who triggered it
        public Guid? EntityId { get; private set; }        // loose ref: postId, commentId, etc. (not an FK)
        public bool IsRead { get; private set; } = false;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        //For RelationShips
        public User User { get; set; } = null!;            // recipient nav
        public User? Actor { get; set; }                   // actor nav (optional)

        private Notification() { }

        public static Notification Create(
            Guid userId,
            NotificationType type,
            string title,
            string message,
            Guid? actorId = null,
            Guid? entityId = null)
        {
            return new Notification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Type = type,
                Title = title,
                Message = message,
                ActorId = actorId,
                EntityId = entityId,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void MarkAsRead() => IsRead = true;
    }
}