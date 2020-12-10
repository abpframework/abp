using System;

namespace Volo.Docs.Documents
{
    [Serializable]
    public class DocumentContributorDto
    {
        public string Username { get; set; }

        public string UserProfileUrl { get; set; }

        public string AvatarUrl { get; set; }

        public int CommitCount { get; set; }
    }
}
