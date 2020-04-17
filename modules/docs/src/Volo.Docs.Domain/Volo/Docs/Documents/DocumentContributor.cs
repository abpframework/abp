using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Docs.Documents
{
    public class DocumentContributor : Entity
    {
        public Guid DocumentId { get; set; }

        public string Username { get; set; }

        public string UserProfileUrl { get; set; }

        public string AvatarUrl { get; set; }

        protected DocumentContributor()
        {

        }

        public virtual bool Equals(Guid documentId, string username)
        {
            return DocumentId == documentId && Username == username;
        }

        public DocumentContributor(Guid documentId, string username, string userProfileUrl, string avatarUrl)
        {
            DocumentId = documentId;
            Username = username;
            UserProfileUrl = userProfileUrl;
            AvatarUrl = avatarUrl;
        }

        public override object[] GetKeys()
        {
            return new object[] { DocumentId, Username };
        }
    }
}
