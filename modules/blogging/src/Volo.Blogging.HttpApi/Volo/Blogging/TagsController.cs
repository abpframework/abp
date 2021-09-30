using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Blogging.Tagging;
using Volo.Blogging.Tagging.Dtos;

namespace Volo.Blogging
{
    [RemoteService(Name = BloggingRemoteServiceConsts.RemoteServiceName)]
    [Area("blogging")]
    [Route("api/blogging/tags")]
    public class TagsController : AbpController, ITagAppService
    {
        private readonly ITagAppService _tagAppService;

        public TagsController(ITagAppService tagAppService)
        {
            _tagAppService = tagAppService;
        }

        [HttpGet]
        [Route("popular/{blogId}")]
        public Task<List<TagDto>> GetPopularTagsAsync(Guid blogId, GetPopularTagsInput input)
        {
            return _tagAppService.GetPopularTagsAsync(blogId, input);
        }
    }
}
