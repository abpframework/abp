namespace Volo.Abp.Auditing
{
    public interface IAuditPropertySetter
    {
        void SetCreationAuditProperties(object targetObject);

        void SetModificationAuditProperties(object auditedObject);
    }
}