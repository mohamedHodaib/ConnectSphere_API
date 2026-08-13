using AutoMapper;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Posts.Commands.UpdatePost;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Users.Queries.DeleteUser
{
    internal sealed class DeleteUserCommandHandler
        : IRequestHandler<DeleteUserCommand, Result<Unit>>
    {
        private readonly ILogger<DeleteUserCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteUserCommandHandler(
            ILogger<DeleteUserCommandHandler> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle
            (DeleteUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempt deleting user {userId}", request.UserId);

            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);


            //Ensure user actually exists before mapping
            if (user is null)
            {
                _logger.LogWarning("User {userId} not found in database", request.UserId);
                return Result<Unit>.Failure(Error.NotFound("User.NotFound", "User profile not found."));
            }


            _unitOfWork.UserRepository.DeleteUser(user);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("User {userId} optained successfully.", request.UserId);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
