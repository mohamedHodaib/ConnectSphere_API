using ConnectSphere.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Domain.Entities
{
    public class Reaction
    {
        public Guid PostId { get; set; }
        public Guid ReactedUserId { get; set; }
        public ReactionType Type { get; set; } 
        public DateTime CreatedAt { get; set; }

        //For RelationShips
        public User User { get; set; } = null!;
        public Post Post { get; set; } = null!;

        private Reaction() { }

        public static Reaction Create(Guid userId,Guid postId, ReactionType reactionType)
        {
            // Create the instance
            var reaction = new Reaction
            {
                PostId = postId,
                ReactedUserId = userId,
                Type = reactionType,
                CreatedAt = DateTime.UtcNow
            };

            return reaction;
        }

        public void Update(ReactionType reactionType) => Type = reactionType;
    }
}
