using ConnectSphere.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.DTOs
{
    public record NotificationDto
    {
        public Guid Id { get; init; }
        public NotificationType Type { get; init; }
        public string Title { get; init; } = null!;
        public string Message { get; init; } = null!;
        public Guid? ActorId { get; init; }
        public string? ActorName { get; init; }
        public string? ActorProfilePictureUrl { get; init; }
        public Guid? EntityId { get; init; }
        public bool IsRead { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
