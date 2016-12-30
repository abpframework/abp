using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace AbpDesk.Blogging
{
    public class BlogPostComment : Entity
    {
        [NotNull]
        public virtual string Name { get; protected set; }

        [CanBeNull]
        public virtual string Email { get; protected set; }

        public virtual byte? Star { get; protected set; }

        [NotNull]
        public virtual string Message { get; protected set; }

        public BlogPostComment()
        {
            
        }

        public BlogPostComment(string name, string message, string email = null, byte? star = null)
        {
            Name = name;
            Email = email;
            Message = message;
            Star = star;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, " +
                   $"Name = {Name}, " +
                   $"Email = {Email}, " +
                   $"Message = {Message}, " +
                   $"Star = {(Star.HasValue ? Star.Value.ToString() : "none")}";
        }
    }
}