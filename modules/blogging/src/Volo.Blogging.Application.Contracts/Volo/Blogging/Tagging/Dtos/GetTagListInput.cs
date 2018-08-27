using System;
using System.Collections.Generic;

namespace Volo.Blogging.Tagging.Dtos
{
    public class GetTagListInput
    {
        public IEnumerable<Guid> Ids { get; set; }
    }
}