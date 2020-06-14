namespace Volo.Abp.Guids
{
    public class AbpSequentialGuidGeneratorOptions
    {
        /// <summary>
        /// Default value: null (unspecified).
        /// Use <see cref="GetDefaultSequentialGuidType"/> method
        /// to get the value on use, since it fall backs to a default value.
        /// </summary>
        public SequentialGuidType? DefaultSequentialGuidType { get; set; }
        
        /// <summary>
        /// Get the <see cref="DefaultSequentialGuidType"/> value
        /// or returns <see cref="SequentialGuidType.SequentialAtEnd"/>
        /// if <see cref="DefaultSequentialGuidType"/> was null.
        /// </summary>
        public SequentialGuidType GetDefaultSequentialGuidType()
        {
            return DefaultSequentialGuidType ??
                   SequentialGuidType.SequentialAtEnd;
        }
    }
}