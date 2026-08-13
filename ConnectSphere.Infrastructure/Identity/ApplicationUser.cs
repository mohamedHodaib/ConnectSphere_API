using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Biography { get; set; } = null!;

        public DateTimeOffset JointDate { get; set; }
        public DateOnly DateOfBirth { get; set; }

        //Refresh Token properties
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public bool IsRefreshTokenRevoked { get; set; } = false;

        // Domain-specific properties
        public string FullName => $"{FirstName} {LastName}";


        //public ICollection<Post> Posts { get; set; }  = new List<Post>();
        //public ICollection<Comment> Comments { get; set; }  = new List<Comment>();
        //public ICollection<Reaction> Reactions { get; set; }  = new List<Reaction>();
        //public ICollection<Following> FollowingsForUser { get; set; }  = new List<Following>();
        //public ICollection<Following> FollowingsByUser { get; set; }  = new List<Following>();
    }
}
