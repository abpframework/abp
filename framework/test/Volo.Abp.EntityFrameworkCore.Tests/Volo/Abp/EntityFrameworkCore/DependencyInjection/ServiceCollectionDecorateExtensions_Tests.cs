using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DependencyInjection;

public class ServiceCollectionDecorateExtensions_Tests
{
    [Fact]
    public void Should_Decorate_Type_Registration()
    {
        var services = new ServiceCollection();
        services.AddScoped<IDecorateTestService, DecorateTestService>();

        services.Decorate<IDecorateTestService>((_, inner) => new DecorateTestServiceDecorator(inner));

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IDecorateTestService>();
        service.ShouldBeOfType<DecorateTestServiceDecorator>().Inner.ShouldBeOfType<DecorateTestService>();
        scope.ServiceProvider.GetRequiredService<IDecorateTestService>().ShouldBeSameAs(service);
    }

    [Fact]
    public void Should_Decorate_The_Last_Registration()
    {
        var services = new ServiceCollection();
        services.AddScoped<IDecorateTestService, DecorateTestService>();
        services.AddScoped<IDecorateTestService, OtherDecorateTestService>();

        services.Decorate<IDecorateTestService>((_, inner) => new DecorateTestServiceDecorator(inner));

        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<IDecorateTestService>()
            .ShouldBeOfType<DecorateTestServiceDecorator>().Inner.ShouldBeOfType<OtherDecorateTestService>();
    }

    [Fact]
    public void Should_Dispose_Inner_Service_Created_By_Factory()
    {
        var services = new ServiceCollection();
        services.AddScoped<IDecorateTestService>(_ => new DisposableDecorateTestService());

        services.Decorate<IDecorateTestService>((_, inner) => new DecorateTestServiceDecorator(inner));

        using var provider = services.BuildServiceProvider();
        DisposableDecorateTestService inner;
        using (var scope = provider.CreateScope())
        {
            inner = scope.ServiceProvider.GetRequiredService<IDecorateTestService>()
                .ShouldBeOfType<DecorateTestServiceDecorator>().Inner.ShouldBeOfType<DisposableDecorateTestService>();
            inner.DisposeCount.ShouldBe(0);
        }

        inner.DisposeCount.ShouldBe(1);
    }

    [Fact]
    public void Should_Not_Dispose_Inner_Service_Registered_As_Instance()
    {
        var instance = new DisposableDecorateTestService();
        var services = new ServiceCollection();
        services.AddSingleton<IDecorateTestService>(instance);

        services.Decorate<IDecorateTestService>((_, inner) => new DecorateTestServiceDecorator(inner));

        using (var provider = services.BuildServiceProvider())
        {
            provider.GetRequiredService<IDecorateTestService>()
                .ShouldBeOfType<DecorateTestServiceDecorator>().Inner.ShouldBeSameAs(instance);
        }

        instance.DisposeCount.ShouldBe(0);
    }

    [Fact]
    public void Should_Not_Allow_Returning_The_Decorated_Service()
    {
        var services = new ServiceCollection();
        services.AddScoped<IDecorateTestService, DisposableDecorateTestService>();

        services.Decorate<IDecorateTestService>((_, inner) => inner);

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        Should.Throw<AbpException>(() => scope.ServiceProvider.GetRequiredService<IDecorateTestService>());
    }

    [Fact]
    public void Should_Decorate_Once()
    {
        var services = new ServiceCollection();
        services.AddScoped<IDecorateTestService, DecorateTestService>();

        services.Decorate<IDecorateTestService>((_, inner) => new DecorateTestServiceDecorator(inner));
        services.Decorate<IDecorateTestService>((_, inner) => new DecorateTestServiceDecorator(inner));

        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<IDecorateTestService>()
            .ShouldBeOfType<DecorateTestServiceDecorator>().Inner.ShouldBeOfType<DecorateTestService>();
    }

    [Fact]
    public void Should_Decorate_Registration_Added_After_Decoration()
    {
        var services = new ServiceCollection();
        services.AddScoped<IDecorateTestService, DecorateTestService>();
        services.Decorate<IDecorateTestService>((_, inner) => new DecorateTestServiceDecorator(inner));
        services.AddScoped<IDecorateTestService, OtherDecorateTestService>();

        services.Decorate<IDecorateTestService>((_, inner) => new DecorateTestServiceDecorator(inner));

        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<IDecorateTestService>()
            .ShouldBeOfType<DecorateTestServiceDecorator>().Inner.ShouldBeOfType<OtherDecorateTestService>();
    }

    [Fact]
    public void Should_Not_Change_Services_Without_Registration()
    {
        var services = new ServiceCollection();

        services.Decorate<IDecorateTestService>((_, inner) => new DecorateTestServiceDecorator(inner));

        services.ShouldBeEmpty();
    }

    public interface IDecorateTestService
    {
    }

    public class DecorateTestService : IDecorateTestService
    {
    }

    public class OtherDecorateTestService : IDecorateTestService
    {
    }

    public class DisposableDecorateTestService : IDecorateTestService, IDisposable
    {
        public int DisposeCount { get; private set; }

        public void Dispose()
        {
            DisposeCount++;
        }
    }

    public class DecorateTestServiceDecorator : IDecorateTestService
    {
        public IDecorateTestService Inner { get; }

        public DecorateTestServiceDecorator(IDecorateTestService inner)
        {
            Inner = inner;
        }
    }
}
