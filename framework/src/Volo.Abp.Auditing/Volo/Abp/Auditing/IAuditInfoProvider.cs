namespace Volo.Abp.Auditing
{
    /// <summary>
    /// Provides an interface to provide audit informations in the upper layers.
    /// </summary>
    public interface IAuditInfoProvider
    {
        /// <summary>
        /// Called to fill needed properties.
        /// </summary>
        /// <param name="auditInfo">Audit info that is partially filled</param>
        void Fill(AuditInfo auditInfo);
    }
}