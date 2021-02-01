using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Volo.CmsKit.Blogs.Extensions;
using Shouldly;

namespace Volo.CmsKit.Blogs
{
    public class UrlSlugExtensions_Tests
    {
        [Fact]
        public void NormalizeAsUrlSlug_ShouldWorkProperly()
        {
            // Arrange
            var name = "My awesome name";
            var expected = "my-awesome-name";

            // Act
            var actual = name.NormalizeAsUrlSlug();

            // Assert
            actual.ShouldBe(expected);
        }
        [Fact]
        public void NormalizeAsUrlSlug_ShouldWorkProperly_WithDot()
        {
            // Arrange
            var name = "My Perfect Title v.2";
            var expected = "my-perfect-title-v2";

            // Act
            var actual = name.NormalizeAsUrlSlug();

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public void NormalizeAsUrlSlug_ShouldWorkProperly_WithQuestionMark()
        {
            // Arrange
            var name = "Are you gonna die?";
            var expected = "are-you-gonna-die";

            // Act
            var actual = name.NormalizeAsUrlSlug();

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public void NormalizeAsUrlSlug_ShouldWorkProperly_WithAmpersand()
        {
            // Arrange
            var name = "We & Machines Challenge";
            var expected = "we-machines-challenge";

            // Act
            var actual = name.NormalizeAsUrlSlug();

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public void NormalizeAsUrlSlug_ShouldWorkProperly_WithDoubleDash()
        {
            // Arrange
            var name = "Go and Code --part 2";
            var expected = "go-and-code-part-2";

            // Act
            var actual = name.NormalizeAsUrlSlug();

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public void NormalizeAsUrlSlug_ShouldWorkProperly_WithCyrillicChars()
        {
            // Arrange
            var name = "Мое классное название";
            var expected = "moe-klassnoe-nazvanie";

            // Act
            var actual = name.NormalizeAsUrlSlug();

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public void NormalizeAsUrlSlug_ShouldWorkProperly_WithTurkishChars()
        {
            // Arrange
            var name = "Özel Türkçe karakterler: ğüşiöç";
            var expected = "ozel-turkce-karakterler-gusioc";

            // Act
            var actual = name.NormalizeAsUrlSlug();

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public void NormalizeAsUrlSlug_ShouldWorkProperly_WithChineseChars()
        {
            // Arrange
            var name = "我的挑战";
            var expected = "o-e-iao-han";

            // Act
            var actual = name.NormalizeAsUrlSlug();

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public void NormalizeAsUrlSlug_ShouldWorkProperly_WithEmoji()
        {
            // Arrange
            var name = "Let's Rock 👊";
            var expected = "lets-rock";

            // Act
            var actual = name.NormalizeAsUrlSlug();

            // Assert
            actual.ShouldBe(expected);
        }
    }
}
