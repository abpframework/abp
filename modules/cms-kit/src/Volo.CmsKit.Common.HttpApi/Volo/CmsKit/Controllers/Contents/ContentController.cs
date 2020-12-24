using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.CmsKit.Common.Application.Contracts.Volo.CmsKit.Contents;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Controllers.Contents
{
    public class ContentController : CmsKitControllerBase, IContentAppService
    {
        protected readonly IContentAppService _contentAppService;

        public ContentController(IContentAppService contentAppService)
        {
            _contentAppService = contentAppService;
        }

        public virtual Task<ContentDto> GetAsync(GetContentInput input)
        {
            return _contentAppService.GetAsync(input);
        }
    }
}
