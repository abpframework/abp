using System;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit
{
    public class CmsKitTestData : ISingletonDependency
    {
        public Guid User1Id { get; } = Guid.NewGuid();

        public string User1UserName => "fake.user";

        public Guid User2Id { get; } = Guid.NewGuid();

        public Guid CommentWithChildId { get; } = Guid.NewGuid();


        public string EntityType1 { get; } = "EntityName1";

        public string EntityType2 { get; } = "EntityName2";

        public string EntityId1 { get; } = "1";

        public string EntityId2 { get; } = "2";

        public string Content_1_EntityType { get; } = "Lyrics";

        public string Content_1 { get; } = "First things first\nI'ma say all the words inside my head\nI'm fired up and tired of the way that things have been, oh-ooh\nThe way that things have been, oh-ooh";

        public Guid Content_1_Id { get; } = Guid.NewGuid();

        public string Content_1_EntityId { get; } = "1";

        public string[] Content_1_Tags => new string[] { "Imagine Dragons", "Music" };

        public string Content_2_EntityType { get; } = "LyricsAlso";

        public string Content_2 { get; } = "Second thing second\nDon't you tell me what you think that I could be\nI'm the one at the sail, I'm the master of my sea, oh-ooh\nThe master of my sea, oh-ooh";

        public Guid Content_2_Id { get; } = Guid.NewGuid();

        public string Content_2_EntityId { get; } = "2";

        public string[] Content_2_Tags => new string[] { "Imagine Dragons", "Music", "Most Loved Part" };

        public string Page_1_Title { get; } = "Imagine Dragons - Believer Lyrics";

        public string Page_1_Slug { get; } = "imagine-dragons-believer-lyrics";

        public Guid Page_1_Id { get; } = Guid.NewGuid();

        public string Page_1_Content => Content_1;

        public string Page_2_Title { get; } = "Imagine Dragons - Believer Lyrics (Page 2)";

        public string Page_2_Slug { get; } = "imagine-dragons-believer-lyrics-2";

        public Guid Page_2_Id { get; } = Guid.NewGuid();

        public string Page_2_Content => Content_2;

        public string TagDefinition_1_EntityType => "My.Namespace.CustomType";

        public Guid TagId_1 { get; } = Guid.NewGuid();

        public string TagName_1 => "Awesome";

        public Guid TagId_2 { get; } = Guid.NewGuid();

        public string TagName_2 => "News";

        public Guid Blog_Id { get; set; } = Guid.NewGuid();

        public string BlogName => "Cms Blog";

        public string BlogSlug => "cms-blog";

        public Guid BlogPost_1_Id { get; internal set; }

        public string BlogPost_1_Title => "How to install CmsKit?";

        public string BlogPost_1_Slug => "how-to-install-cms-kit";

        public Guid BlogPost_2_Id { get; internal set; }

        public string BlogPost_2_Title => "How to use CmsKit";

        public string BlogPost_2_Slug => "how-to-use-cms-kit";

        public Guid BlogFeature_1_Id { get; internal set; } = Guid.NewGuid();

        public string BlogFeature_1_FeatureName => "Analytics";

        public bool BlogFeature_1_Enabled => true;

        public Guid BlogFeature_2_Id { get; internal set; } = Guid.NewGuid();

        public string BlogFeature_2_FeatureName => "Hotjar";

        public bool BlogFeature_2_Enabled => false;

        public Guid Media_1_Id { get; } = Guid.NewGuid();

        public string Media_1_EntityType => nameof(Blog);

        public string Media_1_Content { get; } = "Hi, this is text file";

        public string Media_1_Name { get; } = "hello.txt";

        public string Media_1_ContentType { get; } = "text/plain";

        public Guid Menu_1_Id { get; } = Guid.NewGuid();

        public string Menu_1_Name { get; } = "MainMenu";

        public Guid MenuItem_1_Id { get; } = Guid.NewGuid();
        
        public string MenuItem_1_Name { get; } = "About Us";
        
        public string MenuItem_1_Url { get; } = "/about-us";

        public Guid MenuItem_2_Id { get; } = Guid.NewGuid();
        
        public string MenuItem_2_Name { get; } = "Our Team";
        
        public string MenuItem_2_Url { get; } = "/team";
        
        
        public Guid Menu_2_Id { get; } = Guid.NewGuid();

        public string Menu_2_Name { get; } = "DraftMenu";
        
        public Guid MenuItem_3_Id { get; } = Guid.NewGuid();
        
        public string MenuItem_3_Name { get; } = "Products";
        
        public string MenuItem_3_Url { get; } = "/products";
    }
}
