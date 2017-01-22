namespace Volo.Abp
{
    /// <summary>
    /// Can be used to store Name/Value (or Key/Value) pairs.
    /// </summary>
    //[Serializable]
    public class NameValue : NameValue<string>
    {
        /// <summary>
        /// Creates a new <see cref="NameValue"/>.
        /// </summary>
        public NameValue()
        {

        }

        /// <summary>
        /// Creates a new <see cref="NameValue"/>.
        /// </summary>
        public NameValue(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

    /// <summary>
    /// Can be used to store Name/Value (or Key/Value) pairs.
    /// </summary>
    //[Serializable]
    public class NameValue<T>
    {
        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Creates a new <see cref="NameValue"/>.
        /// </summary>
        public NameValue()
        {

        }

        /// <summary>
        /// Creates a new <see cref="NameValue"/>.
        /// </summary>
        public NameValue(string name, T value)
        {
            Name = name;
            Value = value;
        }
    }
}