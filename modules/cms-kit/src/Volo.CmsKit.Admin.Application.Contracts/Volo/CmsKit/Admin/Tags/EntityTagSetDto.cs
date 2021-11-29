using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.CmsKit.Admin.Tags
{
    [Serializable]
    public class EntityTagSetDto
    {
        public string EntityId { get; set; }
        public string EntityType { get; set; }
        public List<string> Tags { get; set; }
    }
}
