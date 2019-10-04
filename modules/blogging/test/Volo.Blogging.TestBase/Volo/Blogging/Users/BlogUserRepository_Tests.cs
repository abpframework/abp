using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Modularity;

namespace Volo.Blogging.Users
{
    public abstract class BlogUserRepository_Tests<TStartupModule> : BloggingTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {

    }
}
