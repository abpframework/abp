using System;

namespace Volo.Abp.Modularity
{
    /// <summary>
    /// Used to define dependencies of an ABP module to other modules.
    /// It should be used for a class implements <see cref="IAbpModule"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependsOnAttribute : Attribute, IDependedModuleTypesProvider
    {
        /// <summary>
        /// Types of depended modules.
        /// </summary>
        public Type[] DependedModuleTypes { get; }

        /// <summary>
        /// Used to define dependencies of an ABP module to other modules.
        /// </summary>
        /// <param name="dependedModuleTypes">Types of depended modules</param>
        public DependsOnAttribute(params Type[] dependedModuleTypes)
        {
            DependedModuleTypes = dependedModuleTypes;
        }

        public virtual Type[] GetDependedModuleTypes()
        {
            return DependedModuleTypes;
        }
    }
}