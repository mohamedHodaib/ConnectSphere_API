using ConnectSphere.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Features.Auth.Commands.ForgotPassword
{
    public record ForgotPasswordCommand(string Email) : IRequest<Result>;
}
