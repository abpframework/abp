namespace Volo.Abp.Guids
{
    public class AbpSequentialGuidGeneratorOptions
    {
        /// <summary>
        /// Default value: <see cref="SequentialGuidType.SequentialAtEnd"/>.
        /// </summary>
        public SequentialGuidType DefaultSequentialGuidType { get; set; }

        public AbpSequentialGuidGeneratorOptions()
        {
            DefaultSequentialGuidType = SequentialGuidType.SequentialAtEnd;
        }
    }
}