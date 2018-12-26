using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Volo.Blogging.MongoDB
{
    public class BloggingMongoModelBuilderConfigurationOptions : MongoModelBuilderConfigurationOptions
    {
        public BloggingMongoModelBuilderConfigurationOptions([NotNull] string tablePrefix = BloggingConsts.DefaultDbTablePrefix)
            : base(tablePrefix)
        {
        }
    }
}
