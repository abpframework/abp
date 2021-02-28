﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Public.Comments
{
    public class CommentPublicAppService : ApplicationService, ICommentPublicAppService
    {
        protected CmsKitOptions CmsKitOptions { get; }
        protected ICommentRepository CommentRepository { get; }
        protected ICmsUserLookupService CmsUserLookupService { get; }
        public IDistributedEventBus DistributedEventBus { get; }
        public IUnitOfWorkManager UnitOfWorkManager { get; }

        public CommentPublicAppService(
            ICommentRepository commentRepository,
            ICmsUserLookupService cmsUserLookupService,
            IDistributedEventBus distributedEventBus,
            IUnitOfWorkManager unitOfWorkManager,
            IOptions<CmsKitOptions> cmsKitOptions)
        {
            CmsKitOptions = cmsKitOptions.Value;
            CommentRepository = commentRepository;
            CmsUserLookupService = cmsUserLookupService;
            DistributedEventBus = distributedEventBus;
            UnitOfWorkManager = unitOfWorkManager;
        }

        public virtual async Task<ListResultDto<CommentWithDetailsDto>> GetListAsync(string entityType, string entityId)
        {
            var commentsWithAuthor = await CommentRepository
                .GetListWithAuthorsAsync(entityType, entityId);

            return new ListResultDto<CommentWithDetailsDto>(
                ConvertCommentsToNestedStructure(commentsWithAuthor)
            );
        }

        [Authorize]
        public virtual async Task<CommentDto> CreateAsync(string entityType, string entityId, CreateCommentInput input)
        {
            var user = await CmsUserLookupService.GetByIdAsync(CurrentUser.GetId());

            if(input.RepliedCommentId.HasValue)
            {
                await CommentRepository.GetAsync(input.RepliedCommentId.Value);
            }

            var comment = await CommentRepository.InsertAsync(
                new Comment(
                    GuidGenerator.Create(),
                    entityType,
                    entityId,
                    input.Text,
                    input.RepliedCommentId,
                    user.Id,
                    CurrentTenant.Id
                )
            );


            await UnitOfWorkManager.Current.SaveChangesAsync();

            await DistributedEventBus.PublishAsync(new CreatedCommentEvent
            {
                Id = comment.Id
            });

            return ObjectMapper.Map<Comment, CommentDto>(comment);
        }

        [Authorize]
        public virtual async Task<CommentDto> UpdateAsync(Guid id, UpdateCommentInput input)
        {
            var comment = await CommentRepository.GetAsync(id);

            if (comment.CreatorId != CurrentUser.GetId())
            {
                throw new BusinessException(); //TODO: AbpAuthorizationException!
            }

            comment.SetText(input.Text);

            var updatedComment = await CommentRepository.UpdateAsync(comment);

            return ObjectMapper.Map<Comment, CommentDto>(updatedComment);
        }

        [Authorize]
        public virtual async Task DeleteAsync(Guid id)
        {
            var comment = await CommentRepository.GetAsync(id);

            if (comment.CreatorId != CurrentUser.GetId())
            {
                throw new BusinessException(); //TODO: AbpAuthorizationException!
            }

            await CommentRepository.DeleteWithRepliesAsync(comment);
        }

        private List<CommentWithDetailsDto> ConvertCommentsToNestedStructure(List<CommentWithAuthorQueryResultItem> comments)
        {
            //TODO: I think this method can be optimized if you use dictionaries instead of straight search

            var parentComments = comments
                .Where(c=> c.Comment.RepliedCommentId == null)
                .Select(c=> ObjectMapper.Map<Comment, CommentWithDetailsDto>(c.Comment))
                .ToList();

            foreach (var parentComment in parentComments)
            {
                parentComment.Author = GetAuthorAsDtoFromCommentList(comments, parentComment.Id);

                parentComment.Replies = comments
                    .Where(c => c.Comment.RepliedCommentId == parentComment.Id)
                    .Select(c => ObjectMapper.Map<Comment, CommentDto>(c.Comment))
                    .ToList();

                foreach (var reply in parentComment.Replies)
                {
                    reply.Author = GetAuthorAsDtoFromCommentList(comments, reply.Id);
                }
            }

            return parentComments;
        }

        private CmsUserDto GetAuthorAsDtoFromCommentList(List<CommentWithAuthorQueryResultItem> comments, Guid commentId)
        {
            return ObjectMapper.Map<CmsUser, CmsUserDto>(comments.Single(c => c.Comment.Id == commentId).Author);
        }
    }
}
