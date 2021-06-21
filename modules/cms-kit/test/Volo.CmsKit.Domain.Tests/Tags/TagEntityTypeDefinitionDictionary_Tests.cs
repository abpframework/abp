using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Localization;
using Volo.CmsKit.Localization;
using Xunit;

namespace Volo.CmsKit.Tags
{
    public class TagEntityTypeDefinitionDictionary_Tests : CmsKitDomainTestBase
    {
        private readonly CmsKitTagOptions cmsKitTagOptions;

        public TagEntityTypeDefinitionDictionary_Tests()
        {
            var options = GetRequiredService<IOptions<CmsKitTagOptions>>();
            cmsKitTagOptions = options.Value;
        }

        [Fact]
        public void ShouldAddEntityTypeProperly_WithOnlyEntityType()
        {
            cmsKitTagOptions.EntityTypes.Add(new TagEntityTypeDefiniton("My.Entity.Type"));
        }

        [Fact]
        public void ShouldAddEntityTypeProperly_WithEntityTypeAndDisplayName()
        {
            cmsKitTagOptions.EntityTypes.Add(
                new TagEntityTypeDefiniton(
                    "My.Entity.Type",
                    LocalizableString.Create<CmsKitResource>("MyEntity")));
        }

        [Fact]
        public void ShouldAddEntityType_WithAllParameters()
        {
            cmsKitTagOptions.EntityTypes.Add(
                new TagEntityTypeDefiniton(
                    "My.Entity.Type",
                    LocalizableString.Create<CmsKitResource>("MyEntity"),
                    new[] { "SomeCreatePolicy" },
                    new[] { "SomeUpdatePolicy" },
                    new[] { "SomeDeletePolicy" }
                    ));
        }

        [Fact]
        public void ShouldThrowException_WhileAddingExistingType()
        {
            var expectedCount = cmsKitTagOptions.EntityTypes.Count + 1;

            cmsKitTagOptions.EntityTypes.Add(new TagEntityTypeDefiniton("My.Entity.Type"));
            cmsKitTagOptions.EntityTypes.AddIfNotContains(new TagEntityTypeDefiniton("My.Entity.Type"));

            cmsKitTagOptions.EntityTypes.Count.ShouldBe(expectedCount);
        }
    }
}
