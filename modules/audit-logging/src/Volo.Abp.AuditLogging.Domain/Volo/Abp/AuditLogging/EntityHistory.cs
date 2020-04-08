namespace Volo.Abp.AuditLogging
{
    public class EntityHistory
    {
        public EntityChange EntityChange { get; set; }
        
        public string UserName { get; set; }
    }
}