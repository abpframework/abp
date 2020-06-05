namespace Volo.Abp.ObjectExtending
{
    public static class AuditLoggingModuleExtensionConsts
    {
        public const string ModuleName = "AuditLogging";

        public static class EntityNames
        {
            public const string AuditLog = "AuditLog";
            
            public const string AuditLogAction = "AuditLogAction";
            
            public const string EntityChange = "EntityChange";
        }
    }
}