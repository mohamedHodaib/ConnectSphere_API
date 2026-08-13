using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Features.Auth.Commands.ResetPassword
{
    public record ResetPasswordCommand(
     string Email,
     string Token,
     string NewPassword,
     string ConfirmPassword
    ) : IRequest<Result>;
}
