using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace Volo.Abp.Autofac.Interception;

public class AbpShardedProxy_Tests
{
    [Theory]
    [InlineData(1)]
    [InlineData(16)]
    public void The_Same_Key_Should_Get_The_Same_Proxy_Type(int shardCount)
    {
        var builder = new AbpShardedProxyBuilder(shardCount);

        var proxyType = builder.CreateClassProxyType(typeof(Calculator), Type.EmptyTypes, ProxyGenerationOptions.Default);

        builder.CreateClassProxyType(typeof(Calculator), Type.EmptyTypes, ProxyGenerationOptions.Default).ShouldBeSameAs(proxyType);
        builder.CreateClassProxyType(typeof(Calculator), new[] { typeof(IExtra) }, ProxyGenerationOptions.Default).ShouldNotBeSameAs(proxyType);
    }

    [Fact]
    public void Interface_Proxy_Keys_Should_Be_Reused_And_Kept_Apart()
    {
        var builder = new AbpShardedProxyBuilder(16);
        var options = ProxyGenerationOptions.Default;

        var withTarget = builder.CreateInterfaceProxyTypeWithTarget(typeof(ICalculator), Type.EmptyTypes, typeof(Calculator), options);
        builder.CreateInterfaceProxyTypeWithTarget(typeof(ICalculator), Type.EmptyTypes, typeof(Calculator), options).ShouldBeSameAs(withTarget);
        builder.CreateInterfaceProxyTypeWithTarget(typeof(ICalculator), new[] { typeof(IExtra) }, typeof(Calculator), options).ShouldNotBeSameAs(withTarget);

        var withTargetInterface = builder.CreateInterfaceProxyTypeWithTargetInterface(typeof(ICalculator), Type.EmptyTypes, options);
        builder.CreateInterfaceProxyTypeWithTargetInterface(typeof(ICalculator), Type.EmptyTypes, options).ShouldBeSameAs(withTargetInterface);

        var withoutTarget = builder.CreateInterfaceProxyTypeWithoutTarget(typeof(ICalculator), Type.EmptyTypes, options);
        builder.CreateInterfaceProxyTypeWithoutTarget(typeof(ICalculator), Type.EmptyTypes, options).ShouldBeSameAs(withoutTarget);

        new[] { withTarget, withTargetInterface, withoutTarget }.Select(proxyType => proxyType.FullName).Distinct().Count().ShouldBe(3);
    }

    [Fact]
    public async Task Concurrent_First_Requests_Should_Get_One_Proxy_Type_Per_Key()
    {
        var builder = new AbpShardedProxyBuilder(4);
        var targets = DistinctlyNamedTypes();

        var results = await Task.WhenAll(Enumerable.Range(0, 8).Select(_ => Task.Run(() =>
            targets.Select(target => builder.CreateClassProxyType(target, Type.EmptyTypes, ProxyGenerationOptions.Default)).ToArray())));

        for (var i = 0; i < targets.Length; i++)
        {
            results.Select(result => result[i]).Distinct().ShouldHaveSingleItem();
        }

        results[0].Select(proxyType => proxyType.Module).Distinct().Count().ShouldBeGreaterThan(1);
    }

    [Fact]
    public void Proxies_Of_Types_Sharing_A_Simple_Name_Should_Get_Distinct_Names()
    {
        var builder = new AbpShardedProxyBuilder(16);
        var targets = SameNamedTypes().Concat(new[] { typeof(FirstOwner.Handler), typeof(SecondOwner.Handler) }).ToArray();

        var proxyTypes = targets
            .Select(target => builder.CreateClassProxyType(target, Type.EmptyTypes, ProxyGenerationOptions.Default))
            .ToArray();

        proxyTypes.Select(proxyType => proxyType.FullName).Distinct().Count().ShouldBe(targets.Length);
        proxyTypes.Select(proxyType => proxyType.AssemblyQualifiedName).Distinct().Count().ShouldBe(targets.Length);
    }

    [Fact]
    public void Interface_Proxies_Of_One_Interface_With_Different_Targets_Should_Get_Distinct_Names()
    {
        var builder = new AbpShardedProxyBuilder(16);
        var targets = CalculatorTargets();

        var proxyTypes = targets
            .Select(target => builder.CreateInterfaceProxyTypeWithTarget(typeof(ICalculator), Type.EmptyTypes, target, ProxyGenerationOptions.Default))
            .ToArray();

        proxyTypes.Distinct().Count().ShouldBe(targets.Length);
        proxyTypes.Select(proxyType => proxyType.FullName).Distinct().Count().ShouldBe(targets.Length);
    }

    [Fact]
    public void Interface_Interception_Should_Name_The_Proxy_After_The_Class_Own_Interface()
    {
        using var container = Build(builder => builder.RegisterType<OwnInterfaceCalculator>()
            .As<IOwnInterfaceCalculator>()
            .As<IExtra>()
            .EnableAbpInterfaceInterceptors()
            .InterceptedBy(typeof(RecordingInterceptor)));

        var calculator = container.Resolve<IOwnInterfaceCalculator>();

        calculator.GetType().FullName.ShouldStartWith("Castle.Proxies.IOwnInterfaceCalculatorProxy");
        calculator.Add(1, 2).ShouldBe(3);
        ((IExtra)calculator).Seven().ShouldBe(7);
        container.Resolve<RecordingInterceptor>().Calls.ShouldBe(new[] { "counted:Add", "counted:Seven" });
    }

    [Fact]
    public void Interface_Interception_Should_Keep_The_Attributes_Autofac_Copies_To_The_Proxy()
    {
        using var container = Build(builder =>
        {
            builder.RegisterType<TaggedCalculator>().Keyed<ITaggedCalculator>("autofac")
                .EnableInterfaceInterceptors().InterceptedBy(typeof(RecordingInterceptor));
            builder.RegisterType<TaggedCalculator>().Keyed<ITaggedCalculator>("abp")
                .EnableAbpInterfaceInterceptors().InterceptedBy(typeof(RecordingInterceptor));
        });

        // The class's own interface is not the first one, and the first one has an attribute Castle copies.
        typeof(TaggedCalculator).GetInterfaces().First().ShouldBe(typeof(ITaggedExtra));
        var fromAutofac = container.ResolveKeyed<ITaggedCalculator>("autofac").GetType();
        var fromAbp = container.ResolveKeyed<ITaggedCalculator>("abp").GetType();

        fromAutofac.GetCustomAttribute<ProxyTagAttribute>().ShouldNotBeNull().Tag.ShouldBe("extra");
        fromAbp.GetCustomAttribute<ProxyTagAttribute>().ShouldNotBeNull().Tag.ShouldBe("extra");
        fromAbp.GetInterfaces().OrderBy(i => i.FullName).ShouldBe(fromAutofac.GetInterfaces().OrderBy(i => i.FullName));
    }

    [Fact]
    public void Interface_Interception_Should_Keep_The_Attributes_Castle_Is_Told_To_Copy_To_The_Proxy()
    {
        global::Castle.DynamicProxy.Generators.AttributesToAlwaysReplicate.Add(typeof(InheritedProxyTagAttribute));
        using var container = Build(builder =>
        {
            builder.RegisterType<ReplicatedCalculator>().Keyed<IReplicatedCalculator>("autofac")
                .EnableInterfaceInterceptors().InterceptedBy(typeof(RecordingInterceptor));
            builder.RegisterType<ReplicatedCalculator>().Keyed<IReplicatedCalculator>("abp")
                .EnableAbpInterfaceInterceptors().InterceptedBy(typeof(RecordingInterceptor));
        });

        typeof(ReplicatedCalculator).GetInterfaces().First().ShouldBe(typeof(IReplicatedExtra));
        var fromAutofac = container.ResolveKeyed<IReplicatedCalculator>("autofac").GetType();
        var fromAbp = container.ResolveKeyed<IReplicatedCalculator>("abp").GetType();

        fromAutofac.GetCustomAttribute<InheritedProxyTagAttribute>().ShouldNotBeNull().Tag.ShouldBe("replicated");
        fromAbp.GetCustomAttribute<InheritedProxyTagAttribute>().ShouldNotBeNull().Tag.ShouldBe("replicated");
    }

    [Fact]
    public void Properties_Should_Be_Set_On_Same_Named_Proxies_From_Different_Generators()
    {
        // Two generators name a proxy of the same interface the same way, as Autofac's own and ABP's do side by side.
        var first = (IPropertyTarget)new ProxyGenerator(new DefaultProxyBuilder())
            .CreateInterfaceProxyWithTarget(typeof(IPropertyTarget), new PropertyTarget01());
        var second = (IPropertyTarget)new ProxyGenerator(new DefaultProxyBuilder())
            .CreateInterfaceProxyWithTarget(typeof(IPropertyTarget), new PropertyTarget02());
        first.GetType().FullName.ShouldBe(second.GetType().FullName);
        first.GetType().ShouldNotBe(second.GetType());

        ObjectHelper.TrySetProperty(first, x => x.Value, () => 7);
        ObjectHelper.TrySetProperty(second, x => x.Value, () => 42);

        ((IPropertyTarget)ProxyUtil.GetUnproxiedInstance(first)).Value.ShouldBe(7);
        ((IPropertyTarget)ProxyUtil.GetUnproxiedInstance(second)).Value.ShouldBe(42);
    }

    [Fact]
    public void Interface_Interception_Should_Name_The_Proxy_After_A_Nullable_Annotated_Own_Interface()
    {
        using var container = Build(builder => builder.RegisterType<NullableCalculator>()
            .As<INullableCalculator>()
            .EnableAbpInterfaceInterceptors()
            .InterceptedBy(typeof(RecordingInterceptor)));

        typeof(INullableCalculator).CustomAttributes.ShouldContain(a => a.AttributeType.Name == "NullableContextAttribute");
        container.Resolve<INullableCalculator>().GetType().FullName.ShouldStartWith("Castle.Proxies.INullableCalculatorProxy");
    }

    [Fact]
    public void Interface_Interception_Should_Keep_A_Runtime_Attribute_Of_The_Compiler_Namespace_On_The_Proxy()
    {
        using var container = Build(builder => builder.RegisterType<ForwardedCalculator>()
            .As<IForwardedCalculator>()
            .EnableAbpInterfaceInterceptors()
            .InterceptedBy(typeof(RecordingInterceptor)));

        typeof(ForwardedCalculator).GetInterfaces().First().ShouldBe(typeof(IForwardedExtra));
        var proxyType = container.Resolve<IForwardedCalculator>().GetType();
        proxyType.FullName.ShouldStartWith("Castle.Proxies.IForwardedExtraProxy");
        proxyType.GetCustomAttributes(typeof(System.Runtime.CompilerServices.TypeForwardedFromAttribute), false).ShouldNotBeEmpty();
    }

    [Fact]
    public void Interface_Proxies_Of_A_Shared_Interface_Should_Keep_Distinct_Names_For_Caches_Keyed_By_Name()
    {
        var targets = PropertyTargets();
        using var container = Build(builder =>
        {
            for (var i = 0; i < targets.Length; i++)
            {
                builder.RegisterType(targets[i]).Keyed<IPropertyTarget>(i).EnableAbpInterfaceInterceptors().InterceptedBy(typeof(RecordingInterceptor));
            }
        });

        var proxies = Enumerable.Range(0, targets.Length).Select(i => container.ResolveKeyed<IPropertyTarget>(i)).ToArray();

        proxies.Select(proxy => proxy.GetType().FullName).Distinct().Count().ShouldBe(targets.Length);
        for (var i = 0; i < proxies.Length; i++)
        {
            var value = i;
            ObjectHelper.TrySetProperty(proxies[i], x => x.Value, () => value);
            ((IPropertyTarget)ProxyUtil.GetUnproxiedInstance(proxies[i])).Value.ShouldBe(i);
        }
    }

    [Fact]
    public async Task Every_Proxy_Kind_Should_Intercept_Through_The_Sharded_Builder()
    {
        var generator = new ProxyGenerator(new AbpShardedProxyBuilder(4));
        var interceptor = new RecordingInterceptor("counted");

        generator.CreateClassProxy<Calculator>(interceptor).Add(1, 2).ShouldBe(3);
        generator.CreateClassProxyWithTarget(new Calculator(), interceptor).Add(1, 2).ShouldBe(3);
        generator.CreateInterfaceProxyWithTarget<ICalculator>(new Calculator(), interceptor).Add(1, 2).ShouldBe(3);
        generator.CreateInterfaceProxyWithTargetInterface<ICalculator>(new Calculator(), interceptor).Add(1, 2).ShouldBe(3);
        generator.CreateInterfaceProxyWithoutTarget<ICalculator>(new ReturningInterceptor(7)).Add(1, 2).ShouldBe(7);
        (await generator.CreateClassProxy<Calculator>(interceptor).AddAsync(2, 3)).ShouldBe(5);
        Should.Throw<InvalidOperationException>(() => generator.CreateClassProxy<Calculator>(interceptor).Fail());

        interceptor.Calls.Count.ShouldBe(6);
    }

    [Fact]
    public void Class_Interception_Should_Pass_The_Options_Selector_To_The_Proxy()
    {
        var options = new ProxyGenerationOptions { Selector = new NoInterceptorsSelector() };
        using var container = Build(builder => builder.RegisterType<Calculator>()
            .EnableAbpClassInterceptors(options)
            .InterceptedBy(typeof(RecordingInterceptor)));

        container.Resolve<Calculator>().Add(1, 2).ShouldBe(3);

        container.Resolve<RecordingInterceptor>().Calls.ShouldBeEmpty();
    }

    [Fact]
    public void Class_Interception_Should_Pass_The_Options_Mixins_To_The_Proxy()
    {
        var options = new ProxyGenerationOptions();
        options.AddMixinInstance(new Extra());
        using var container = Build(builder => builder.RegisterType<Calculator>()
            .EnableAbpClassInterceptors(options)
            .InterceptedBy(typeof(RecordingInterceptor)));

        var calculator = container.Resolve<Calculator>();

        calculator.Add(1, 2).ShouldBe(3);
        ((IExtra)calculator).Seven().ShouldBe(7);
        container.Resolve<RecordingInterceptor>().Calls.ShouldBe(new[] { "counted:Add", "counted:Seven" });
    }

    [Fact]
    public void Class_Interception_Should_Run_Explicit_Then_Attribute_Interceptors()
    {
        using var container = Build(builder => builder.RegisterType<AttributedCalculator>()
            .EnableAbpClassInterceptors()
            .InterceptedBy(typeof(RecordingInterceptor)));

        container.Resolve<AttributedCalculator>().Add(1, 2).ShouldBe(3);

        CallsOf(container).ShouldBe(new[] { "counted:Add", "attribute:Add" });
    }

    [Fact]
    public void Interface_Interception_Should_Run_Explicit_Named_And_Attribute_Interceptors()
    {
        using var container = Build(builder => builder.RegisterType<AttributedCalculator>()
            .As<ICalculator>()
            .EnableAbpInterfaceInterceptors()
            .InterceptedBy(typeof(RecordingInterceptor))
            .InterceptedBy("named"));

        container.Resolve<ICalculator>().Add(1, 2).ShouldBe(3);

        CallsOf(container).ShouldBe(new[] { "counted:Add", "named:Add", "attribute:Add" });
    }

    [Fact]
    public void Interface_Interception_Should_Pass_The_Options_To_The_Proxy()
    {
        var options = new ProxyGenerationOptions { Selector = new NoInterceptorsSelector() };
        using var container = Build(builder => builder.RegisterType<Calculator>()
            .As<ICalculator>()
            .EnableAbpInterfaceInterceptors(options)
            .InterceptedBy(typeof(RecordingInterceptor)));

        container.Resolve<ICalculator>().Add(1, 2).ShouldBe(3);

        container.Resolve<RecordingInterceptor>().Calls.ShouldBeEmpty();
    }

    [Fact]
    public async Task The_Shard_Count_Should_Be_Fixed_Once_The_Generator_Exists()
    {
        var generators = await Task.WhenAll(Enumerable.Range(0, 8)
            .Select(_ => Task.Run(() => AbpInterceptionRegistrationExtensions.ProxyGenerator)));
        var generator = generators.Distinct().ShouldHaveSingleItem();
        var shardCount = AbpInterceptionRegistrationExtensions.ShardCount;

        generator.ProxyBuilder.ShouldBeOfType<AbpShardedProxyBuilder>().ShardCount.ShouldBe(shardCount);
        AbpInterceptionRegistrationExtensions.ShardCount = shardCount;
        Should.Throw<AbpException>(() => AbpInterceptionRegistrationExtensions.ShardCount = shardCount + 1);
        Should.Throw<ArgumentException>(() => AbpInterceptionRegistrationExtensions.ShardCount = 0);
        AbpInterceptionRegistrationExtensions.ShardCount.ShouldBe(shardCount);
    }

    private static IContainer Build(Action<ContainerBuilder> register)
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<RecordingInterceptor>().AsSelf().SingleInstance()
            .WithParameter(new TypedParameter(typeof(string), "counted"));
        builder.Register(_ => new RecordingInterceptor("named")).Named<IInterceptor>("named").SingleInstance();
        builder.Register(_ => new AttributeInterceptor()).AsSelf().SingleInstance();
        register(builder);
        return builder.Build();
    }

    private static List<string> CallsOf(IContainer container)
    {
        return container.Resolve<RecordingInterceptor>().Recorded
            .Concat(((RecordingInterceptor)container.ResolveNamed<IInterceptor>("named")).Recorded)
            .Concat(container.Resolve<AttributeInterceptor>().Recorded)
            .OrderBy(call => call.Order)
            .Select(call => call.Name)
            .ToList();
    }

    private static Type[] DistinctlyNamedTypes()
    {
        return new[]
        {
            typeof(Spread01), typeof(Spread02), typeof(Spread03), typeof(Spread04), typeof(Spread05), typeof(Spread06),
            typeof(Spread07), typeof(Spread08), typeof(Spread09), typeof(Spread10), typeof(Spread11), typeof(Spread12)
        };
    }

    private static Type[] CalculatorTargets()
    {
        return new[]
        {
            typeof(Calculator), typeof(Calculator<int>), typeof(Calculator<string>), typeof(Calculator<long>),
            typeof(Calculator<byte>), typeof(Calculator<short>), typeof(Calculator<char>), typeof(Calculator<bool>),
            typeof(Calculator<double>), typeof(Calculator<float>), typeof(Calculator<decimal>), typeof(Calculator<Guid>)
        };
    }

    private static Type[] PropertyTargets()
    {
        return new[]
        {
            typeof(PropertyTarget01), typeof(PropertyTarget02), typeof(PropertyTarget03), typeof(PropertyTarget04),
            typeof(PropertyTarget05), typeof(PropertyTarget06), typeof(PropertyTarget07), typeof(PropertyTarget08),
            typeof(PropertyTarget09), typeof(PropertyTarget10), typeof(PropertyTarget11), typeof(PropertyTarget12)
        };
    }

    private static Type[] SameNamedTypes()
    {
        return new[]
            {
                typeof(int), typeof(long), typeof(short), typeof(byte), typeof(string), typeof(Guid), typeof(DateTime),
                typeof(decimal), typeof(double), typeof(float), typeof(char), typeof(bool), typeof(object), typeof(Uri),
                typeof(Version), typeof(TimeSpan)
            }
            .Select(argument => typeof(Target<>).MakeGenericType(argument))
            .ToArray();
    }

    public interface ICalculator
    {
        int Add(int a, int b);
    }

    public class Calculator<T> : ICalculator
    {
        public virtual int Add(int a, int b) => a + b;
    }

#nullable enable
    public interface INullableCalculator
    {
        string? Describe(string? value);

        string Name();
    }

    public class NullableCalculator : IExtra, INullableCalculator
    {
        public int Seven() => 7;

        public string? Describe(string? value) => value;

        public string Name() => "name";
    }
#nullable restore

    public interface IOwnInterfaceCalculator
    {
        int Add(int a, int b);
    }

    public class OwnInterfaceCalculator : IExtra, IOwnInterfaceCalculator
    {
        public int Add(int a, int b) => a + b;

        public int Seven() => 7;
    }

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false)]
    public class ProxyTagAttribute : Attribute
    {
        public string Tag { get; }

        public ProxyTagAttribute(string tag)
        {
            Tag = tag;
        }
    }

    [ProxyTag("extra")]
    public interface ITaggedExtra
    {
        int Eight();
    }

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class InheritedProxyTagAttribute : Attribute
    {
        public string Tag { get; }

        public InheritedProxyTagAttribute(string tag)
        {
            Tag = tag;
        }
    }

    [InheritedProxyTag("replicated")]
    public interface IReplicatedExtra
    {
        int Nine();
    }

    public interface IReplicatedCalculator
    {
        int Add(int a, int b);
    }

    public class ReplicatedCalculator : IReplicatedExtra, IReplicatedCalculator
    {
        public int Add(int a, int b) => a + b;

        public int Nine() => 9;
    }

    [System.Runtime.CompilerServices.TypeForwardedFrom("Original, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public interface IForwardedExtra
    {
        int Ten();
    }

    public interface IForwardedCalculator
    {
        int Add(int a, int b);
    }

    public class ForwardedCalculator : IForwardedExtra, IForwardedCalculator
    {
        public int Add(int a, int b) => a + b;

        public int Ten() => 10;
    }

    public interface ITaggedCalculator
    {
        int Add(int a, int b);
    }

    public class TaggedCalculator : ITaggedExtra, ITaggedCalculator
    {
        public int Add(int a, int b) => a + b;

        public int Eight() => 8;
    }

    public interface IPropertyTarget
    {
        int Value { get; set; }
    }

    public class PropertyTarget01 : IPropertyTarget
    {
        public int Value { get; set; }
    }

    public class PropertyTarget02 : IPropertyTarget
    {
        public int Value { get; set; }
    }

    public class PropertyTarget03 : IPropertyTarget
    {
        public int Value { get; set; }
    }

    public class PropertyTarget04 : IPropertyTarget
    {
        public int Value { get; set; }
    }

    public class PropertyTarget05 : IPropertyTarget
    {
        public int Value { get; set; }
    }

    public class PropertyTarget06 : IPropertyTarget
    {
        public int Value { get; set; }
    }

    public class PropertyTarget07 : IPropertyTarget
    {
        public int Value { get; set; }
    }

    public class PropertyTarget08 : IPropertyTarget
    {
        public int Value { get; set; }
    }

    public class PropertyTarget09 : IPropertyTarget
    {
        public int Value { get; set; }
    }

    public class PropertyTarget10 : IPropertyTarget
    {
        public int Value { get; set; }
    }

    public class PropertyTarget11 : IPropertyTarget
    {
        public int Value { get; set; }
    }

    public class PropertyTarget12 : IPropertyTarget
    {
        public int Value { get; set; }
    }

    public interface IExtra
    {
        int Seven();
    }

    public class Calculator : ICalculator
    {
        public virtual int Add(int a, int b) => a + b;

        public virtual Task<int> AddAsync(int a, int b) => Task.FromResult(a + b);

        public virtual int Fail() => throw new InvalidOperationException("Failed.");
    }

    [Intercept(typeof(AttributeInterceptor))]
    public class AttributedCalculator : Calculator
    {
    }

    public class Extra : IExtra
    {
        public int Seven() => 7;
    }

    public class Spread01
    {
        public virtual int Value() => 1;
    }

    public class Spread02
    {
        public virtual int Value() => 2;
    }

    public class Spread03
    {
        public virtual int Value() => 3;
    }

    public class Spread04
    {
        public virtual int Value() => 4;
    }

    public class Spread05
    {
        public virtual int Value() => 5;
    }

    public class Spread06
    {
        public virtual int Value() => 6;
    }

    public class Spread07
    {
        public virtual int Value() => 7;
    }

    public class Spread08
    {
        public virtual int Value() => 8;
    }

    public class Spread09
    {
        public virtual int Value() => 9;
    }

    public class Spread10
    {
        public virtual int Value() => 10;
    }

    public class Spread11
    {
        public virtual int Value() => 11;
    }

    public class Spread12
    {
        public virtual int Value() => 12;
    }

    public class FirstOwner
    {
        public class Handler
        {
            public virtual int Value() => 1;
        }
    }

    public class SecondOwner
    {
        public class Handler
        {
            public virtual int Value() => 2;
        }
    }

    public class Target<T>
    {
        public virtual T? Echo(T? value) => value;
    }

    public class RecordingInterceptor : IInterceptor
    {
        private static int _order;

        private readonly string _name;

        public RecordingInterceptor(string name)
        {
            _name = name;
        }

        public List<string> Calls => Recorded.Select(call => call.Name).ToList();

        public List<(int Order, string Name)> Recorded { get; } = new();

        public void Intercept(IInvocation invocation)
        {
            lock (Recorded)
            {
                Recorded.Add((System.Threading.Interlocked.Increment(ref _order), $"{_name}:{invocation.Method.Name}"));
            }

            invocation.Proceed();
        }
    }

    public class AttributeInterceptor : RecordingInterceptor
    {
        public AttributeInterceptor()
            : base("attribute")
        {
        }
    }

    public class ReturningInterceptor : IInterceptor
    {
        private readonly int _value;

        public ReturningInterceptor(int value)
        {
            _value = value;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.ReturnValue = _value;
        }
    }

    public class NoInterceptorsSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, System.Reflection.MethodInfo method, IInterceptor[] interceptors)
        {
            return Array.Empty<IInterceptor>();
        }
    }
}
