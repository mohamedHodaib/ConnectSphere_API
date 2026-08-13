using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Interfaces;
using ConnectSphere.Application.Posts.Commands.UpdatePost;
using ConnectSphere.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConnectSphere.Application.Profiles.Commands.UpdateProfileInfo
{
    internal sealed class UpdateProfileInfoCommandHandler
        : IRequestHandler<UpdateProfileInfoCommand, Result<Unit>>
    {
        private readonly ILogger<UpdateProfileInfoCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISaveFile _saveFile;

        public UpdateProfileInfoCommandHandler(
            ILogger<UpdateProfileInfoCommandHandler> logger,
            IUnitOfWork unitOfWork,
            ITagService tagService,
            ISaveFile saveFile)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _saveFile = saveFile;
        }

        public async Task<Result<Unit>> Handle
            (UpdateProfileInfoCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempt updateing profile info for user {userId}", request.UserId);


            var userInfo = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);


            //Ensure user actually exists before mapping
            if (userInfo is null)
            {
                _logger.LogWarning("User {userId} not found in database", request.UserId);
                return Result<Unit>.Failure(Error.NotFound("User.NotFound", "User profile not found."));
            }

            //Save profile picture
            string? imageUrl = null ;
            if(request.UpdateProfileInfoDto.ProfilePicture != null)
            {
                 imageUrl = await _saveFile.SaveAsync
                (request.UpdateProfileInfoDto.ProfilePicture,
                "profile_pictures");
            }

            //make update
            userInfo.Update(request.UpdateProfileInfoDto.FirstName
                , request.UpdateProfileInfoDto.LastName
                , request.UpdateProfileInfoDto.Username
                , request.UpdateProfileInfoDto.Biography
                , request.UpdateProfileInfoDto.IsPrivate
                , request.UpdateProfileInfoDto.ShowOnlineStatus
                , request.UpdateProfileInfoDto.EmailNotificationsEnabled
                , imageUrl);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("User {userId} updated successfully.", request.UserId);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}



