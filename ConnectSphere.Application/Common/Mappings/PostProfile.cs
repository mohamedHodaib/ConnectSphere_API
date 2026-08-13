using AutoMapper;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.Mappings
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostDetailsDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Content, opt => opt.MapFrom(s => s.Content))
            .ForMember(d => d.Title, opt => opt.MapFrom(s =>
                                                         string.IsNullOrEmpty(s.Content)
                                                            ? null
                                                            : (s.Content.Contains('\n')
                                                                ? s.Content.Substring(0, s.Content.IndexOf('\n')).Trim()
                                                                : (s.Content.Length <= 120 ? s.Content : s.Content.Substring(0, 120)).Trim())))


            .ForMember(d => d.Content, opt => opt.MapFrom(s => s.Content))
            .ForMember(d => d.ImagesUrls, opt => opt.MapFrom(s => s.Images != null
                                                            ? s.Images.Select(i => i.ImageUrl).ToList()
                                                            : new List<string>()))

            .ForMember(d => d.Tags, opt => opt.MapFrom(s => s.Tags != null
                                                            ? s.Tags.Select(t => t.Name).ToList()
                                                            : new List<string>()))

            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt))

            .ForMember(d => d.AuthorId, opt => opt.MapFrom(s => s.UserId))

            .ForMember(d => d.AuthorName, opt => opt.MapFrom(s => s.User != null ? s.User.DisplayName : string.Empty))
            .ForMember(d => d.AuthorUsername, opt => opt.MapFrom(s => s.User != null ? s.User.UserName : null))
            .ForMember(d => d.AuthorProfilePictureUrl,
                                               opt => opt.MapFrom(s => s.User != null ? s.User.ProfilePictureUrl : null))
            .ForMember(d => d.OriginalUrl, opt => opt.Ignore()) // map if you add a Source/OriginalUrl to Post
            .ForMember(d => d.ReactionsCount, opt => opt.MapFrom(s => s.Reactions != null ? s.Reactions.Count : 0));
        }
    }
}
