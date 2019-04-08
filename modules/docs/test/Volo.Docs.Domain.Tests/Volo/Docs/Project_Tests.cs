using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Volo.Docs.Projects;
using Xunit;

namespace Volo.Docs
{
    public class Project_Tests : DocsDomainTestBase
    {
        [Theory]
        [InlineData("aaa")]
        [InlineData("bbb")]
        public void SetName(string name)
        {
            var project = new Project(Guid.NewGuid(), "ABP vNext", "ABP", "Github", "md", "index",
                "docs-nav.json");
            project.SetName(name);
            project.Name.ShouldBe(name);
        }

        [Theory]
        [InlineData("aaa")]
        [InlineData("bbb")]
        public void SetFormat(string format)
        {
            var project = new Project(Guid.NewGuid(), "ABP vNext", "ABP", "Github", "md", "index",
                "docs-nav.json");
            project.SetFormat(format);
            project.Format.ShouldBe(format);
        }

        [Theory]
        [InlineData("aaa")]
        [InlineData("bbb")]
        public void SetNavigationDocumentName(string navigationDocumentName)
        {
            var project = new Project(Guid.NewGuid(), "ABP vNext", "ABP", "Github", "md", "index",
                "docs-nav.json");
            project.SetNavigationDocumentName(navigationDocumentName);
            project.NavigationDocumentName.ShouldBe(navigationDocumentName);
        }

        [Theory]
        [InlineData("aaa")]
        [InlineData("bbb")]
        public void SetDefaultDocumentName(string defaultDocumentName)
        {
            var project = new Project(Guid.NewGuid(), "ABP vNext", "ABP", "Github", "md", "index",
                "docs-nav.json");
            project.SetDefaultDocumentName(defaultDocumentName);
            project.DefaultDocumentName.ShouldBe(defaultDocumentName);
        }
    }
}
