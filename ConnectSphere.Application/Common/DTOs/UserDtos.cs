using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.DTOs
{
    //For registeration

    public record UserProfileDetailsDto
    {
        public Guid Id { get; init; }
        public string? DisplayName { get; init; }
        public string Email { get; init; } = null!;
        public string? PhoneNumber { get; init; }
        public bool EmailConfirmed { get; init; }
    }
    public record PublicProfileInfoDto
    {
        public string Username { get; init; } = null!;
        public string DisplayName { get; init; } = null!;
        public string? Biography { get; init; } 
        public string? ProfilePictureUrl { get; set; }
        public DateTime JointDate { get; init; }
        public int PostsCount { get; init; }
        public int FollowersCount { get; init; }
        public int FollowingCount { get; init; }
    }

    public record OwnProfileInfoDto
    {
        public string Username { get; init; } = null!;

        public string Email { get; init; } = null!;

        public string DisplayName { get; init; } = null!;

        public string? Biography { get; init; } 

        public string? ProfilePictureUrl { get; set; }

        public DateTime JointDate { get; init; }
    }


    public record SettingsDto
    {
        public string FirstName { get; init; } 
        public string LastName { get; init; } 
        public string? Biography { get; init; }
        public string Username { get; init; }
        public IFormFile? ProfilePicture { get; init; }
            
               
        //privacy and notifications properties
        public bool? IsPrivate { get; set; }
        public bool? ShowOnlineStatus { get; set; }
        public bool? EmailNotificationsEnabled { get; set; }
    }


    public record OwnSettingsDto
    {
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string? Biography { get; init; }
        public string Username { get; init; } = null!;
        public string? ProfilePictureUrl { get; set; }

        public bool IsPrivate { get; init; }
        public bool ShowOnlineStatus { get; init; }
        public bool EmailNotificationsEnabled { get; init; }
    }

    public record ProfilePictureUrlDto
    {
        public string? ProfilePictureUrl { get; init; }    }
}
