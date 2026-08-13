using AutoMapper;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Domain.Entities;

namespace ConnectSphere.Application.Common.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, OwnProfileInfoDto>();

            CreateMap<User, UserProfileDetailsDto>();

            CreateMap<User, PublicProfileInfoDto>()
                .ForMember(dest => dest.PostsCount, opt => opt.MapFrom(src => src.Posts.Count))
                .ForMember(dest => dest.FollowersCount, opt => opt.MapFrom(src => src.Followers.Count))
                .ForMember(dest => dest.FollowingCount, opt => opt.MapFrom(src => src.Following.Count));

            CreateMap<User, OwnSettingsDto>();
        }
    }
}
