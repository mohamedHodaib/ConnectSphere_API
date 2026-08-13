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
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDetailsDto>()
                .ForMember(dest => dest.CommentedUserDisplayName
                ,opt => opt.MapFrom(src => src.User.DisplayName));
        }
    }
}
