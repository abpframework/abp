using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Volo.Blogging.Blogs;
using Xunit;

namespace Volo.Blogging
{
    public class Blog_Tests
    {
        [Theory]
        [InlineData("aaa")]
        [InlineData("bbb")]
        public void SetName(string name)
        {
            var blog = new Blog(Guid.NewGuid(), "test blog", "test");
            blog.SetName(name);
            blog.Name.ShouldBe(name);
        }

        [Theory]
        [InlineData("aaa")]
        [InlineData("bbb")]
        public void SetShortName(string name)
        {
            var blog = new Blog(Guid.NewGuid(), "test blog", "test");
            blog.SetShortName(name);
            blog.ShortName.ShouldBe(name);
        }
    }
}
