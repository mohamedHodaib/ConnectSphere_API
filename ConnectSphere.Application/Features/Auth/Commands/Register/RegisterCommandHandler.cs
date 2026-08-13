using AutoMapper;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConnectSphere.Application.Auth.Commands.Register.RegisterCommand;

namespace ConnectSphere.Application.Auth.Commands.Register
{
    internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<UserProfileDetailsDto>>
    {
       
        private readonly IAuthService _authService;
        private readonly ILogger<RegisterCommandHandler> _logger;
       
        public RegisterCommandHandler(IAuthService authService,ILogger<RegisterCommandHandler> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public async Task<Result<UserProfileDetailsDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            //log use case start
            _logger.LogInformation(
            "Starting user registration for email: {Email}",
            request.RegisterDto.Email);

            var result = await _authService.RegisterAsync(request.RegisterDto);

            if(result.IsSuccess)
            {
                //log successfull business operation
                _logger.LogInformation(
                    "User Registeration Success.User Id: {id},Email: {email}"
                    ,result.Data!.Id, result.Data.Email);
            }

            else
            {
                //log failed business operation
                _logger.LogInformation(
                    "User Registeration Failed For {email}"
                    , request.RegisterDto.Email);
            }

            return result;
        }
    }
}
