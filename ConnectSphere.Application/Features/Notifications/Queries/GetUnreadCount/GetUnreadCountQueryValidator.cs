using FluentValidation;

namespace ConnectSphere.Application.Notifications.Queries.GetUnreadCount
{
    public class GetUnreadNotificationsCountQueryValidator 
        : AbstractValidator<GetUnreadNotificationsCountQuery>
    {
        public GetUnreadNotificationsCountQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}