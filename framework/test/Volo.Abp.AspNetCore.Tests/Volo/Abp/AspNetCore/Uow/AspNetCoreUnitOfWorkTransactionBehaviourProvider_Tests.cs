using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Uow;

public class AspNetCoreUnitOfWorkTransactionBehaviourProvider_Tests
{
    private static AspNetCoreUnitOfWorkTransactionBehaviourProvider CreateProvider(string method)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Method = method;

        return CreateProvider(httpContext);
    }

    private static AspNetCoreUnitOfWorkTransactionBehaviourProvider CreateProvider(HttpContext httpContext)
    {
        return new AspNetCoreUnitOfWorkTransactionBehaviourProvider(
            new HttpContextAccessor { HttpContext = httpContext },
            Microsoft.Extensions.Options.Options.Create(new AspNetCoreUnitOfWorkTransactionBehaviourProviderOptions()));
    }

    [Theory]
    [InlineData("GET", false)]
    [InlineData("QUERY", false)]
    [InlineData("query", false)]
    [InlineData("HEAD", true)]
    [InlineData("POST", true)]
    [InlineData("PUT", true)]
    [InlineData("DELETE", true)]
    public void IsTransactional_Should_Treat_Get_And_Query_As_Non_Transactional(string method, bool expected)
    {
        CreateProvider(method).IsTransactional.ShouldBe(expected);
    }

    [Fact]
    public void IsTransactional_Should_Not_Change_The_Feature_Collection()
    {
        var features = new KestrelLikeFeatureCollection();
        features.Set<IHttpRequestFeature>(new HttpRequestFeature { Method = "GET", Path = "/" });
        features.Set<IHttpResponseFeature>(new HttpResponseFeature());
        features.Set<IHttpResponseBodyFeature>(new StreamResponseBodyFeature(Stream.Null));
        var httpContext = new DefaultHttpContext(features);
        var provider = CreateProvider(httpContext);
        var revision = httpContext.Features.Revision;

        provider.IsTransactional.ShouldBe(false);
        provider.IsTransactional.ShouldBe(false);

        httpContext.Features.Revision.ShouldBe(revision);
    }

    [Fact]
    public void IsTransactional_Should_Return_Null_For_WebSocket_Requests()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Method = "GET";
        httpContext.Features.Set<IHttpWebSocketFeature>(new TestWebSocketFeature());

        CreateProvider(httpContext).IsTransactional.ShouldBeNull();
    }

    // Kestrel's feature collection increments the revision on every Set, even when the value is null.
    private sealed class KestrelLikeFeatureCollection : IFeatureCollection
    {
        private readonly FeatureCollection _inner = new();

        public int Revision { get; private set; }

        public bool IsReadOnly => false;

        public object? this[Type key]
        {
            get => _inner[key];
            set
            {
                Revision++;
                _inner[key] = value;
            }
        }

        public TFeature? Get<TFeature>()
        {
            return _inner.Get<TFeature>();
        }

        public void Set<TFeature>(TFeature? instance)
        {
            Revision++;
            _inner.Set(instance);
        }

        public IEnumerator<KeyValuePair<Type, object>> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    private sealed class TestWebSocketFeature : IHttpWebSocketFeature
    {
        public bool IsWebSocketRequest => true;

        public Task<WebSocket> AcceptAsync(WebSocketAcceptContext context)
        {
            throw new System.NotSupportedException();
        }
    }
}
