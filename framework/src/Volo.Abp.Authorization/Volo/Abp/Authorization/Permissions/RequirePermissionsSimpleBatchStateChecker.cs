using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.Authorization.Permissions
{
    public class RequirePermissionsSimpleBatchStateChecker<TState> : SimpleBatchStateCheckerBase<TState>
        where TState : IHasSimpleStateCheckers<TState>
    {
        public static readonly RequirePermissionsSimpleBatchStateChecker<TState> Instance = new RequirePermissionsSimpleBatchStateChecker<TState>();

        private readonly List<RequirePermissionsSimpleBatchStateCheckerModel<TState>> _models;

        public RequirePermissionsSimpleBatchStateChecker()
        {
            _models = new List<RequirePermissionsSimpleBatchStateCheckerModel<TState>>();
        }

        public RequirePermissionsSimpleBatchStateChecker<TState> AddCheckModels(params RequirePermissionsSimpleBatchStateCheckerModel<TState>[] models)
        {
            Check.NotNullOrEmpty(models, nameof(models));

            _models.AddRange(models);
            return this;
        }

        public override async Task<SimpleStateCheckerResult<TState>> IsEnabledAsync(SimpleBatchStateCheckerContext<TState> context)
        {
            var permissionChecker = context.ServiceProvider.GetRequiredService<IPermissionChecker>();

            var result = new SimpleStateCheckerResult<TState>(context.States);

            var permissions = _models.Where(x => context.States.Any(s => s.Equals(x.State))).SelectMany(x => x.Permissions).Distinct().ToArray();
            var grantResult = await permissionChecker.IsGrantedAsync(permissions);

            foreach (var state in context.States)
            {
                var model = _models.FirstOrDefault(x => x.State.Equals(state));
                if (model != null)
                {
                    if (model.RequiresAll)
                    {
                        result[model.State] = model.Permissions.All(x => grantResult.Result.Any(y => y.Key == x && y.Value == PermissionGrantResult.Granted));
                    }
                    else
                    {
                        result[model.State] = grantResult.Result.Any(x => model.Permissions.Contains(x.Key) && x.Value == PermissionGrantResult.Granted);
                    }
                }
            }

            return result;
        }
    }
}
