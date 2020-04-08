namespace Volo.Abp.AuditLogging
{
    public class EntityChangeWithUsername
    {
        public EntityChange EntityChange { get; set; }
        
        public string UserName { get; set; }
    }
}