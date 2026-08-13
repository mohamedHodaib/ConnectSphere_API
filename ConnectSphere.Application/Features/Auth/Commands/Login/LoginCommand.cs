using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Auth.Commands.Login
{
    public sealed record LoginCommand (LoginDto LoginDto) : IRequest<Result<TokenDto>>;
}
