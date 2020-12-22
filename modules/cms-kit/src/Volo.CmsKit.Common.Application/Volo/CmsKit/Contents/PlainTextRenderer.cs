using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.CmsKit.Common.Application.Contracts.Volo.CmsKit.Contents;
using Volo.CmsKit.Domain.Shared.Volo.CmsKit.Contents;

namespace Volo.CmsKit.Common.Application.Volo.CmsKit.Contents
{
    public class PlainTextRenderer : IContentRenderer, ITransientDependency
    {
        public Task<string> RenderAsync(IContent content)
        {
            return Task.FromResult(content?.Value);
        }
    }
}
