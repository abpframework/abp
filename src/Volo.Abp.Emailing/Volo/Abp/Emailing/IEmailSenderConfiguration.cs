namespace Volo.Abp.Emailing
{
    /// <summary>
    /// Defines configurations used while sending emails.
    /// </summary>
    public interface IEmailSenderConfiguration
    {
        /// <summary>
        /// Default from address.
        /// </summary>
        string DefaultFromAddress { get; }
        
        /// <summary>
        /// Default display name.
        /// </summary>
        string DefaultFromDisplayName { get; }
    }
}