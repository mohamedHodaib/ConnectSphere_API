using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConnectSphere.Application.Features.Auth.Commands.ConfirmEmail
{
    internal sealed class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result>
    {
        private readonly IAuthService _authService;
        private readonly ILogger<ConfirmEmailCommandHandler> _logger;

        public ConfirmEmailCommandHandler(IAuthService authService, ILogger<ConfirmEmailCommandHandler> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Email confirmation attempt for UserId: {UserId}", request.UserId);

            var result = await _authService.ConfirmEmailAsync(request.UserId, request.Token);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Email confirmed successfully for UserId: {UserId}", request.UserId);
            }
            else
            {
                _logger.LogWarning("Email confirmation failed for UserId: {UserId}", request.UserId);
            }

            return result;
        }
    }
}
