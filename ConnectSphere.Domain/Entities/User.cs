using ConnectSphere.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ConnectSphere.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Biography { get; set; }
        public string? ProfilePictureUrl { get; set; }

        public DateTime JointDate { get; set; }

        //Refresh Token properties
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public bool IsRefreshTokenRevoked { get; set; } = false;


        // Domain-specific properties
        public string DisplayName => $"{FirstName} {LastName}";

        //Privacy & notification settings
        public bool? IsPrivate { get; set; } = false;
        public bool? ShowOnlineStatus { get; set; } = true;
        public bool? EmailNotificationsEnabled { get; set; } = true;


        public ICollection<Post> Posts { get; set; }  = new List<Post>();
        public ICollection<Comment> Comments { get; set; }  = new List<Comment>();
        public ICollection<Follow> Followers { get; set; }  = new List<Follow>();
        public ICollection<Follow> Following { get; set; }  = new List<Follow>();

        public void Update(string firstName,
            string lastName,
            string username,
            string? biography,
            bool? isPrivate,
            bool? showOnlineStatus,
            bool? emailNotificationsEnabled,
            string? profilePictureUrl)
        {
            FirstName = firstName;
            LastName = lastName;
            Biography = biography;
            UserName = username;
            IsPrivate = isPrivate;
            ShowOnlineStatus = showOnlineStatus;
            EmailNotificationsEnabled = emailNotificationsEnabled;
            ProfilePictureUrl = profilePictureUrl;
        }
    }
}
