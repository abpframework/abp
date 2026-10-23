using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Autofac;

public class AutofacServiceImplementationTypeProvider_Tests : AbpIntegratedTest<AutofacTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.AddTransient<TypeProviderInterceptor>();
        services.AddTransient<TypeProviderInterceptedService>();
        services.OnRegistered(context =>
        {
            if (context.ImplementationType == typeof(TypeProviderInterceptedService))
            {
                context.Interceptors.TryAdd<TypeProviderInterceptor>();
            }
        });

        var builder = services.GetObject<ContainerBuilder>();
        builder.RegisterType<TypeProviderService>();
        builder.RegisterType<TypeProviderReplacement>().As<TypeProviderReplaced>();
        builder.Register(_ => new TypeProviderService()).As<TypeProviderCreatedByMethod>();
        builder.RegisterType<TypeProviderService>().As<ITypeProviderService>();
        builder.RegisterType<TypeProviderService>().As<ITypeProviderDecoratedService>();
        builder.RegisterDecorator<TypeProviderDecorator, ITypeProviderDecoratedService>();
    }

    [Fact]
    public void Should_Replace_The_Null_Provider()
    {
        GetRequiredService<IServiceImplementationTypeProvider>().ShouldBeOfType<AutofacServiceImplementationTypeProvider>();
    }

    [Fact]
    public void Should_Tell_The_Type_A_Registration_Creates()
    {
        var provider = GetRequiredService<IServiceImplementationTypeProvider>();

        provider.GetImplementationTypeOrNull(typeof(TypeProviderService)).ShouldBe(typeof(TypeProviderService));
        provider.GetImplementationTypeOrNull(typeof(ITypeProviderService)).ShouldBe(typeof(TypeProviderService));
        provider.GetImplementationTypeOrNull(typeof(TypeProviderReplaced)).ShouldBe(typeof(TypeProviderReplacement));
    }

    [Fact]
    public void Should_Tell_The_Proxy_Type_Of_An_Intercepted_Class()
    {
        var type = GetRequiredService<IServiceImplementationTypeProvider>().GetImplementationTypeOrNull(typeof(TypeProviderInterceptedService));

        type.ShouldNotBeNull();
        type.ShouldNotBe(typeof(TypeProviderInterceptedService));
        type.ShouldBe(GetRequiredService<TypeProviderInterceptedService>().GetType());
    }

    [Fact]
    public void Should_Not_Tell_A_Type_It_Can_Not_Know()
    {
        var provider = GetRequiredService<IServiceImplementationTypeProvider>();

        provider.GetImplementationTypeOrNull(typeof(TypeProviderCreatedByMethod)).ShouldBeNull();
        provider.GetImplementationTypeOrNull(typeof(ITypeProviderDecoratedService)).ShouldBeNull();
        provider.GetImplementationTypeOrNull(typeof(TypeProviderNotRegistered)).ShouldBeNull();
    }

    [Fact]
    public void Should_Not_Tell_A_Type_For_Another_Container()
    {
        var services = new ServiceCollection();
        services.AddTransient<TypeProviderService>();
        using var serviceProvider = services.BuildServiceProvider();

        new AutofacServiceImplementationTypeProvider(serviceProvider)
            .GetImplementationTypeOrNull(typeof(TypeProviderService))
            .ShouldBeNull();
    }

    public interface ITypeProviderService
    {
    }

    public interface ITypeProviderDecoratedService
    {
    }

    public class TypeProviderService : TypeProviderCreatedByMethod, ITypeProviderService, ITypeProviderDecoratedService
    {
    }

    public class TypeProviderCreatedByMethod
    {
    }

    public class TypeProviderReplaced
    {
    }

    public class TypeProviderReplacement : TypeProviderReplaced
    {
    }

    public class TypeProviderNotRegistered
    {
    }

    public class TypeProviderDecorator : ITypeProviderDecoratedService
    {
        public TypeProviderDecorator(ITypeProviderDecoratedService inner)
        {
        }
    }

    public class TypeProviderInterceptedService
    {
        public virtual Task DoItAsync()
        {
            return Task.CompletedTask;
        }
    }

    public class TypeProviderInterceptor : AbpInterceptor
    {
        public override Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            return invocation.ProceedAsync();
        }
    }
}
