using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Volo.Blogging.Comments;
using Volo.Blogging.Posts;
using Xunit;

namespace Volo.Blogging
{
    public class Post_Tests
    {
        [Fact]
        public void IncreaseReadCount()
        {
            var post = new Post(Guid.NewGuid(), Guid.NewGuid(), "abp", "⊙o⊙", "abp.io");
            post.IncreaseReadCount();
            post.ReadCount.ShouldBe(1);
        }

        [Theory]
        [InlineData("aaa")]
        [InlineData("bbb")]
        public void SetTitle(string title)
        {
            var post = new Post(Guid.NewGuid(), Guid.NewGuid(), "abp", "⊙o⊙", "abp.io");
            post.SetTitle(title);
            post.Title.ShouldBe(title);
        }

        [Theory]
        [InlineData("aaa")]
        [InlineData("bbb")]
        public void SetUrl(string url)
        {
            var post = new Post(Guid.NewGuid(), Guid.NewGuid(), "abp", "⊙o⊙", "abp.io");
            post.SetUrl(url);
            post.Url.ShouldBe(url);
        }

        [Fact]
        public void AddTag()
        {
            var post = new Post(Guid.NewGuid(), Guid.NewGuid(), "abp", "⊙o⊙", "abp.io");
            var tagId = Guid.NewGuid();
            post.AddTag(tagId);
            post.Tags.ShouldContain(x => x.TagId == tagId);
        }


        [Fact]
        public void RemoveTag()
        {
            var post = new Post(Guid.NewGuid(), Guid.NewGuid(), "abp", "⊙o⊙", "abp.io");
            var tagId = Guid.NewGuid();
            post.AddTag(tagId);

            post.Tags.ShouldContain(x => x.TagId == tagId);

            post.RemoveTag(tagId);
            post.Tags.ShouldNotContain(x => x.TagId == tagId);
        }

    }
}
