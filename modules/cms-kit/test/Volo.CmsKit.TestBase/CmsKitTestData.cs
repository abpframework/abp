using System;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit
{
    public class CmsKitTestData : ISingletonDependency
    {
        public Guid User1Id { get; } = Guid.NewGuid();

        public Guid User2Id { get; } = Guid.NewGuid();

        public Guid CommentWithChildId { get; } = Guid.NewGuid();

        public string EntityType1 { get; } = "EntityName1";

        public string EntityType2 { get; } = "EntityName2";

        public string EntityId1 { get; } = "1";

        public string EntityId2 { get; } = "2";

        public string Content_1_EntityType = "Lyrics";
        
        public string Content_1 { get; } = "First things first\nI'ma say all the words inside my head\nI'm fired up and tired of the way that things have been, oh-ooh\nThe way that things have been, oh-ooh";

        public string Content_1_Id = "1";

        public string[] Content_1_Tags => new string[] {"Imagine Dragons", "Music"};
        
        public string Content_2_EntityType = "LyricsAlso";
        
        public string Content_2 { get; } = "Second thing second\nDon't you tell me what you think that I could be\nI'm the one at the sail, I'm the master of my sea, oh-ooh\nThe master of my sea, oh-ooh";
        
        public string Content_2_Id = "2";
        
        public string[] Content_2_Tags => new string[] {"Imagine Dragons", "Music", "Most Loved Part"};
    }
}
