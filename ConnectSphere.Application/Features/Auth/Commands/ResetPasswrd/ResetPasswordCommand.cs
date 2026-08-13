using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectSphere.Application.Common.Models;
using MediatR;

namespace ConnectSphere.Application.Features.Auth.Commands.ResetPasswrd
{
    public record ResetPasswordCommand(
    string Email,
    string Token,
    string NewPassword,
    string ConfirmPassword
    ) : IRequest<Result>;
}
