using System;
using Volo.Abp.Settings;

namespace Volo.Abp.Emailing
{
    /// <summary>
    /// Implementation of <see cref="IEmailSenderConfiguration"/> that reads settings
    /// from <see cref="ISettingManager"/>.
    /// </summary>
    public abstract class EmailSenderConfiguration : IEmailSenderConfiguration
    {
        public virtual string DefaultFromAddress => GetNotEmptySettingValue(EmailSettingNames.DefaultFromAddress);

        public virtual string DefaultFromDisplayName => SettingProvider.GetOrNull(EmailSettingNames.DefaultFromDisplayName);

        protected readonly ISettingProvider SettingProvider;

        /// <summary>
        /// Creates a new <see cref="EmailSenderConfiguration"/>.
        /// </summary>
        protected EmailSenderConfiguration(ISettingProvider settingProvider)
        {
            SettingProvider = settingProvider;
        }

        /// <summary>
        /// Gets a setting value by checking. Throws <see cref="AbpException"/> if it's null or empty.
        /// </summary>
        /// <param name="name">Name of the setting</param>
        /// <returns>Value of the setting</returns>
        protected string GetNotEmptySettingValue(string name)
        {
            var value = SettingProvider.GetOrNull(name);

            if (value.IsNullOrEmpty())
            {
                throw new AbpException($"Setting value for '{name}' is null or empty!");
            }

            return value;
        }
    }
}