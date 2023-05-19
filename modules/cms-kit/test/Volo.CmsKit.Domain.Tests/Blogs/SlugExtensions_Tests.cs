using Shouldly;
using Xunit;

namespace Volo.CmsKit.Blogs;

public class SlugExtensions_Tests
{
    [Fact]
    public void NormalizeSlug_ShouldWorkProperly()
    {
        // Arrange
        var name = "My awesome name";
        var expected = "my-awesome-name";

        // Act
        var actual = SlugNormalizer.Normalize(name);

        // Assert
        actual.ShouldBe(expected);
    }
    [Fact]
    public void NormalizeSlug_ShouldWorkProperly_WithDot()
    {
        // Arrange
        var name = "My Perfect Title v.2";
        var expected = "my-perfect-title-v.2";

        // Act
        var actual = SlugNormalizer.Normalize(name);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void NormalizeSlug_ShouldWorkProperly_WithQuestionMark()
    {
        // Arrange
        var name = "Are you gonna die?";
        var expected = "are-you-gonna-die";

        // Act
        var actual = SlugNormalizer.Normalize(name);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void NormalizeSlug_ShouldWorkProperly_WithAmpersand()
    {
        // Arrange
        var name = "We & Machines Challenge";
        var expected = "we-machines-challenge";

        // Act
        var actual = SlugNormalizer.Normalize(name);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void NormalizeSlug_ShouldWorkProperly_WithDoubleDash()
    {
        // Arrange
        var name = "Go and Code --part 2";
        var expected = "go-and-code-part-2";

        // Act
        var actual = SlugNormalizer.Normalize(name);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void NormalizeSlug_ShouldWorkProperly_WithCyrillicChars()
    {
        // Arrange
        var name = "Мое классное название";
        var expected = "moe-klassnoe-nazvanie";

        // Act
        var actual = SlugNormalizer.Normalize(name);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void NormalizeSlug_ShouldWorkProperly_WithTurkishChars()
    {
        // Arrange
        var name = "Özel Türkçe karakterler: ğüşiöç";
        var expected = "ozel-turkce-karakterler-gusioc";

        // Act
        var actual = SlugNormalizer.Normalize(name);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void NormalizeSlug_ShouldWorkProperly_WithChineseChars()
    {
        // Arrange
        var name = "我的真棒头衔";
        var expected = "wo-de-zhen-bang-tou-xian";

        // Act
        var actual = SlugNormalizer.Normalize(name);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void NormalizeSlug_ShouldWorkProperly_WithEmoji()
    {
        // Arrange
        var name = "Let's Rock 👊";
        var expected = "lets-rock";

        // Act
        var actual = SlugNormalizer.Normalize(name);

        // Assert
        actual.ShouldBe(expected);
    }
}
