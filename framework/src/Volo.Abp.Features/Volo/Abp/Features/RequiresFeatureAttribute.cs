using System;

namespace Volo.Abp.Features
{
    /// <summary>
    /// This attribute can be used on a class/method to declare that given class/method is available
    /// only if required feature(s) are enabled.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequiresFeatureAttribute : Attribute
    {
        /// <summary>
        /// A list of features to be checked if they are enabled.
        /// </summary>
        public string[] Features { get; }

        /// <summary>
        /// If this property is set to true, all of the <see cref="Features"/> must be enabled.
        /// If it's false, at least one of the <see cref="Features"/> must be enabled.
        /// Default: false.
        /// </summary>
        public bool RequiresAll { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="RequiresFeatureAttribute"/> class.
        /// </summary>
        /// <param name="features">A list of features to be checked if they are enabled</param>
        public RequiresFeatureAttribute(params string[] features)
        {
            Features = features ?? Array.Empty<string>();
        }
    }
}