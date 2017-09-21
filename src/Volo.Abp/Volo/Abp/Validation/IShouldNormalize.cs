namespace Volo.Abp.Validation
{
    /// <summary>
    /// This interface is used to normalize inputs before method execution.
    /// </summary>
    public interface IShouldNormalize
    {
        /// <summary>
        /// This method is called lastly before method execution (after validation if exists).
        /// </summary>
        void Normalize();
    }
}