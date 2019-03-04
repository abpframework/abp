using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Features
{
    public class MethodInvocationFeatureCheckerService : IMethodInvocationFeatureCheckerService, ITransientDependency
    {
        private readonly IFeatureChecker _featureChecker;

        public MethodInvocationFeatureCheckerService(
            IFeatureChecker featureChecker)
        {
            _featureChecker = featureChecker;
        }

        public async Task CheckAsync(MethodInvocationFeatureCheckerContext context)
        {
            if (IsFeatureCheckDisabled(context))
            {
                return;
            }

            foreach (var requiresFeatureAttribute in GetRequiredFeatureAttributes(context))
            {
                await _featureChecker.CheckEnabledAsync(requiresFeatureAttribute.RequiresAll, requiresFeatureAttribute.Features);
            }
        }

        protected virtual bool IsFeatureCheckDisabled(MethodInvocationFeatureCheckerContext context)
        {
            return context.Method
                .GetCustomAttributes(true)
                .OfType<DisableFeatureCheckAttribute>()
                .Any();
        }

        protected virtual RequiresFeatureAttribute[] GetRequiredFeatureAttributes(MethodInvocationFeatureCheckerContext context)
        {
            var classAttributes = context.Method.DeclaringType
                .GetCustomAttributes(true)
                .OfType<RequiresFeatureAttribute>();

            var methodAttributes = context.Method
                .GetCustomAttributes(true)
                .OfType<RequiresFeatureAttribute>();

            return classAttributes.Union(methodAttributes).ToArray();
        }
    }
}