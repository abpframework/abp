namespace Volo.Abp.Emailing
{
    /// <summary>
    /// Declares names of the settings defined by <see cref="EmailSettingProvider"/>.
    /// </summary>
    public static class EmailSettingNames
    {
        /// <summary>
        /// Abp.Net.Mail.DefaultFromAddress
        /// </summary>
        public const string DefaultFromAddress = "Abp.Mailing.DefaultFromAddress";

        /// <summary>
        /// Abp.Net.Mail.DefaultFromDisplayName
        /// </summary>
        public const string DefaultFromDisplayName = "Abp.Mailing.DefaultFromDisplayName";

        /// <summary>
        /// SMTP related email settings.
        /// </summary>
        public static class Smtp
        {
            /// <summary>
            /// Abp.Net.Mail.Smtp.Host
            /// </summary>
            public const string Host = "Abp.Mailing.Smtp.Host";

            /// <summary>
            /// Abp.Net.Mail.Smtp.Port
            /// </summary>
            public const string Port = "Abp.Mailing.Smtp.Port";

            /// <summary>
            /// Abp.Net.Mail.Smtp.UserName
            /// </summary>
            public const string UserName = "Abp.Mailing.Smtp.UserName";

            /// <summary>
            /// Abp.Net.Mail.Smtp.Password
            /// </summary>
            public const string Password = "Abp.Mailing.Smtp.Password";

            /// <summary>
            /// Abp.Net.Mail.Smtp.Domain
            /// </summary>
            public const string Domain = "Abp.Mailing.Smtp.Domain";

            /// <summary>
            /// Abp.Net.Mail.Smtp.EnableSsl
            /// </summary>
            public const string EnableSsl = "Abp.Mailing.Smtp.EnableSsl";

            /// <summary>
            /// Abp.Net.Mail.Smtp.UseDefaultCredentials
            /// </summary>
            public const string UseDefaultCredentials = "Abp.Mailing.Smtp.UseDefaultCredentials";
        }
    }
}