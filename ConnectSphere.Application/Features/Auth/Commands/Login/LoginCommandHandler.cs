using ConnectSphere.Application.Auth.Commands.Register;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Common.Models.Errors;
using ConnectSphere.Application.Common.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Auth.Commands.Login
{
    internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand,Result<TokenDto>>
    {
        private readonly IAuthService _authService;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(IAuthService authService, ILogger<LoginCommandHandler> logger,IOptions<JwtSettings> jwtSettings)
        {
            _authService = authService;
            _logger = logger;
        }

        public async Task<Result<TokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            //log use case start
            _logger.LogInformation(
            "Starting user Login for email: {Email}",
            request.LoginDto.Email);

            var validationResult = await _authService.ValidateAsync(request.LoginDto);

            if(validationResult.IsSuccess)
            {
                _logger.LogInformation("Login Successfully to email: {Email}", request.LoginDto.Email);

                var tokenDto = await _authService.GenerateTokens(true);

                return Result<TokenDto>.Success(tokenDto);
            }
            else
            {
                _logger.LogWarning("Authentication Failed: Wrong Credentials for email : {Email}", request.LoginDto.Email);

                return Result<TokenDto>.Failure(validationResult.Errors);
            }

        }
    }
}
