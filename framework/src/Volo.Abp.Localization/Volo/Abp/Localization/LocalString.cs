namespace Volo.Abp.Localization
{
    public class LocalString
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public LocalString(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
