// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Blogging.Comments;
using System.Collections.Generic;
using Volo.Blogging.Comments.Dtos;

// ReSharper disable once CheckNamespace
namespace Volo.Blogging.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(ICommentAppService), typeof(CommentsClientProxy))]
    public partial class CommentsClientProxy : ClientProxyBase<ICommentAppService>, ICommentAppService
    {
        public virtual async Task<List<CommentWithRepliesDto>> GetHierarchicalListOfPostAsync(Guid postId)
        {
            return await RequestAsync<List<CommentWithRepliesDto>>(nameof(GetHierarchicalListOfPostAsync), new ClientProxyRequestTypeValue
            {
                { typeof(Guid), postId }
            });
        }

        public virtual async Task<CommentWithDetailsDto> CreateAsync(CreateCommentDto input)
        {
            return await RequestAsync<CommentWithDetailsDto>(nameof(CreateAsync), new ClientProxyRequestTypeValue
            {
                { typeof(CreateCommentDto), input }
            });
        }

        public virtual async Task<CommentWithDetailsDto> UpdateAsync(Guid id, UpdateCommentDto input)
        {
            return await RequestAsync<CommentWithDetailsDto>(nameof(UpdateAsync), new ClientProxyRequestTypeValue
            {
                { typeof(Guid), id },
                { typeof(UpdateCommentDto), input }
            });
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            await RequestAsync(nameof(DeleteAsync), new ClientProxyRequestTypeValue
            {
                { typeof(Guid), id }
            });
        }
    }
}
