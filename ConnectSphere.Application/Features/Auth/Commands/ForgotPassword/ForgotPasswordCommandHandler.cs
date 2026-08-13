using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConnectSphere.Application.Features.Auth.Commands.ForgotPassword
{
    internal sealed class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result>
    {
        private readonly IAuthService _authService;
        private readonly ILogger<ForgotPasswordCommandHandler> _logger;

        public ForgotPasswordCommandHandler(IAuthService authService, ILogger<ForgotPasswordCommandHandler> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Password reset request received for Email: {Email}", request.Email);

            var result = await _authService.ForgotPasswordAsync(request.Email);

            _logger.LogInformation("Password reset request processed for Email: {Email}", request.Email);

            return result;
        }
    }
}