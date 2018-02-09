namespace Volo.Abp.Settings
{
    public class SettingDefinition
    {
        /// <summary>
        /// Unique name of the setting.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Default value of the setting.
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Can clients see this setting and it's value.
        /// It maybe dangerous for some settings to be visible to clients (such as an email server password).
        /// Default: false.
        /// </summary>
        public bool IsVisibleToClients { get; set; }

        /// <summary>
        /// Is this setting inherited from parent scopes.
        /// Default: True.
        /// </summary>
        public bool IsInherited { get; set; }

        /// <summary>
        /// Can be used to store a custom object related to this setting.
        /// </summary>
        public object CustomData { get; set; }

        public SettingDefinition(
            string name,
            string defaultValue = null,
            bool isVisibleToClients = false,
            bool isInherited = true,
            object customData = null)
        {
            Name = name;
            DefaultValue = defaultValue;
            IsVisibleToClients = isVisibleToClients;
            IsInherited = isInherited;
            CustomData = customData;
        }
    }
}