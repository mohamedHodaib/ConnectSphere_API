using FluentValidation;

namespace ConnectSphere.Application.Notifications.Commands.MarkAllAsRead
{
    public class MarkAllNotificationsAsReadCommandValidator : AbstractValidator<MarkAllNotificationsAsReadCommand>
    {
        public MarkAllNotificationsAsReadCommandValidator()
        {

            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}