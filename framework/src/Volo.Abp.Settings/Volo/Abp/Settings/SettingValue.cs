namespace Volo.Abp.Settings
{
    public class SettingValue : NameValue
    {
        /// <summary>
        /// Creates a new <see cref="NameValue"/>.
        /// </summary>
        public SettingValue()
        {

        }

        /// <summary>
        /// Creates a new <see cref="NameValue"/>.
        /// </summary>
        public SettingValue(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}