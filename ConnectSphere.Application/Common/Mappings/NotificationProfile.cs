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
    public class NotificationProfile : Profile
    {
        public NotificationProfile ()
        {
            CreateMap<Notification, NotificationDto>();
        }
    }
}
