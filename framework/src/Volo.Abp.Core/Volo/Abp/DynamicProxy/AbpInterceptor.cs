using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.DynamicProxy
{
    public abstract class AbpInterceptor : IAbpInterceptor
    {
        private readonly ILogger<AbpInterceptor> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public AbpInterceptor(ILogger<AbpInterceptor> logger)
        {
            _logger = logger;
        }
        public virtual Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            _logger.LogInformation($"Volo.Abp.DynamicProxy-AbpInterceptor->InterceptAsync  Method:{invocation.Method.Name} Arguments:{invocation.Arguments}");
            return Task.CompletedTask;
        }
    }
}