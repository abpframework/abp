using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.CmsKit.Tags
{
    public class EntityTagManager_Tests : CmsKitDomainTestBase
    {
        private readonly CmsKitTestData _cmsKitTestData;
        private readonly IEntityTagManager _entityTagManager;
        private readonly IEntityTagManager _entityTagManager;

        public EntityTagManager_Tests()
        {
            _cmsKitTestData = GetRequiredService<CmsKitTestData>();
            _entityTagManager = GetRequiredService<IEntityTagManager>();
        }

        public async Task AddTagToEntityAsync_ShouldAdd_WhenEverythingCorrect()
        {
            var tag = await 
            await _entityTagManager.AddTagToEntityAsync(_cmsKitTestData.tag)
        }
    }
}
