using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Http.Client.ClientProxying;

namespace Volo.Abp.Http.Client.DynamicProxying;

public class DynamicHttpProxyInterceptorClientProxy<TService> : ClientProxyBase<TService>
{
    public virtual async Task<T> CallRequestAsync<T>(ClientProxyRequestContext requestContext)
    {
        return await base.RequestAsync<T>(requestContext);
    }

    public virtual async Task<HttpContent> CallRequestAsync(ClientProxyRequestContext requestContext)
    {
        return await base.RequestAsync(requestContext);
    }

    public virtual IAsyncEnumerable<T> CallRequestAsyncEnumerable<T>(ClientProxyRequestContext requestContext)
    {
        return base.RequestAsyncEnumerable<T>(requestContext);
    }

    public virtual IDisposable UseCancellationToken(CancellationToken cancellationToken)
    {
        if (!cancellationToken.CanBeCanceled)
        {
            return NullDisposable.Instance;
        }

        var linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(CancellationTokenProvider.Token, cancellationToken);
        var scope = CancellationTokenProvider.Use(linkedCancellationTokenSource.Token);
        return new DisposeAction(() =>
        {
            scope.Dispose();
            linkedCancellationTokenSource.Dispose();
        });
    }
}
