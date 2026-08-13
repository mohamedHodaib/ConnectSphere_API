using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Domain.Entities
{
    public class Follow
    {
        //The follower in this follow record
        public Guid FollowerId { get; set; }
        public User Follower { get; set; } = null!;

        //the followedUser in this follow record
        public Guid FollowedUserId {  get; set; }
        public User FollowedUser { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public static Follow Create(Guid followerId, Guid followedUserId)
        {
            return new Follow
            {
                FollowerId = followerId,
                FollowedUserId = followedUserId,
                CreatedAt = DateTime.UtcNow
            };
        }
            
    }
}
