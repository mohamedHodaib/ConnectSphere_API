using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConnectSphere.Application.Features.Auth.Commands.ResetPasswrd
{

   internal sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
   {
       private readonly IAuthService _authService;
       private readonly ILogger<ResetPasswordCommandHandler> _logger;

       public ResetPasswordCommandHandler(IAuthService authService, ILogger<ResetPasswordCommandHandler> logger)
       {
           _authService = authService;
           _logger = logger;
       }

       public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
       {
           _logger.LogInformation("Password reset attempt for Email: {Email}", request.Email);

           var dto = new ResetPasswordDto(
               request.Email,
               request.Token,
               request.NewPassword,
               request.ConfirmPassword
           );

           var result = await _authService.ResetPasswordAsync(dto);

           if (result.IsSuccess)
           {
               _logger.LogInformation("Password reset successfully for Email: {Email}", request.Email);
           }
           else
           {
               _logger.LogWarning("Password reset failed for Email: {Email}", request.Email);
           }

           return result;
       }
   }
}
