using AutoMapper;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Interfaces;
using ConnectSphere.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConnectSphere.Application.Posts.Commands.CreatePost
{
    internal sealed class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Result<PostDetailsDto>>
    {
        private readonly ILogger<AddCommentCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITagService _tagService;
        private readonly ISaveFile _saveFile;
        private readonly IUrlService _urlService;

        public AddCommentCommandHandler(
            ILogger<AddCommentCommandHandler> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ITagService tagService,
            ISaveFile saveFile,
            IUrlService urlService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tagService = tagService;
            _saveFile = saveFile;
            _urlService = urlService;
        }

        public async Task<Result<PostDetailsDto>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start creating post by user with Id:{userId}", request.UserId);

            //Create The post
            var newPost = Post.Create(
                request.UserId,
                request.CreatePostDto.Content
            );

            //  Handle Tags (In-memory, attached to context)
            newPost.Tags = await _tagService.PrepareTagsAsync(request.CreatePostDto.Tags, cancellationToken);

            foreach (var image in request.CreatePostDto.Images ?? [])
            {
                var imageUrl = await _saveFile.SaveAsync(image, "posts");

                newPost.Images.Add(new PostImage
                {
                    ImageUrl = imageUrl
                });
            }

            //  Add Post to Repository
            _unitOfWork.PostRepository.CreatePost(newPost);

            await _unitOfWork.CompleteAsync();

            //Re-fetch only when we need user data ,If the client not need user
            //data this line will be deleted to optimize the performance.
            var createdPost = await _unitOfWork.PostRepository.GetPostByIdIncludeUserAndImagesAsync(newPost.Id, false);

            var newPostDetailsDto = _mapper.Map<PostDetailsDto>(createdPost);

            newPostDetailsDto.ImagesUrls = 
                newPostDetailsDto.ImagesUrls
                .Select(imageUrl => _urlService.GetAbsoluteUrl(imageUrl))
                .ToList();

            _logger.LogInformation("post created by user with Id:{userId}", request.UserId);

            return Result<PostDetailsDto>.Success(newPostDetailsDto);
        }
    }
}