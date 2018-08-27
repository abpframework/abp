using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Repositories;
using Volo.Blogging.Tagging;

namespace Volo.Blogging.Posts
{
    public interface IPostTagRepository : IBasicRepository<PostTag>
    {
    }
}
