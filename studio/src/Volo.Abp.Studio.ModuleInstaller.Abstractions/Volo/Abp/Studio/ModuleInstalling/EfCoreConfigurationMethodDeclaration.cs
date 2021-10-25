using JetBrains.Annotations;

namespace Volo.Abp.Studio.ModuleInstalling
{
    public class EfCoreConfigurationMethodDeclaration
    {
        public string Namespace { get; }

        public string MethodName { get; }

        public EfCoreConfigurationMethodDeclaration([NotNull] string nameSpace, [NotNull] string methodName)
        {
            Namespace = Check.NotNullOrEmpty(nameSpace, nameof(nameSpace));
            MethodName = Check.NotNullOrEmpty(methodName, nameof(methodName));
        }

    }
}
