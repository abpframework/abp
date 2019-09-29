using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Volo.Docs.MongoDB
{
    public class DocsMongoModelBuilderConfigurationOptions : MongoModelBuilderConfigurationOptions
    {
        public DocsMongoModelBuilderConfigurationOptions([NotNull] string tablePrefix = DocsConsts.DefaultDbTablePrefix)
            : base(tablePrefix)
        {
        }
    }
}
