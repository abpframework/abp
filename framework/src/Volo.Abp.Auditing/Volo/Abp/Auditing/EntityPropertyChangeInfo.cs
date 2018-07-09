namespace Volo.Abp.Auditing
{
    public class EntityPropertyChangeInfo
    {
        public virtual string NewValue { get; set; }

        public virtual string OriginalValue { get; set; }

        public virtual string PropertyName { get; set; }

        public virtual string PropertyTypeFullName { get; set; }
    }
}
