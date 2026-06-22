using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.StaticDefinitions;

public class StaticDefinitionCache_Tests : AbpIntegratedTest<AbpTestModule>
{
    protected readonly IStaticDefinitionCache<StaticDefinition1, List<StaticDefinition1>> _staticDefinitionCache1;
    protected readonly IStaticDefinitionCache<StaticDefinition2, List<StaticDefinition2>> _staticDefinitionCache2;

    public StaticDefinitionCache_Tests()
    {
        _staticDefinitionCache1 = GetRequiredService<IStaticDefinitionCache<StaticDefinition1, List<StaticDefinition1>>>();
        _staticDefinitionCache2 = GetRequiredService<IStaticDefinitionCache<StaticDefinition2, List<StaticDefinition2>>>();
    }

    [Fact]
    public async Task GetOrCreate_Test()
    {
        var definition1 = new StaticDefinition1 { Name = "Definition1", Value = 1 };
        var definition2 = new StaticDefinition1 { Name = "Definition2", Value = 2 };

        var definitionsFirstRetrieval = await _staticDefinitionCache1.GetOrCreateAsync(() =>
        {
            return Task.FromResult(new List<StaticDefinition1> { definition1, definition2 });
        });

        var definitionsSecondRetrieval = await _staticDefinitionCache1.GetOrCreateAsync(() =>
        {
            throw new AbpException("Factory should not be called on second retrieval");
        });

        definitionsFirstRetrieval.ShouldBe(definitionsSecondRetrieval);

        definitionsSecondRetrieval.Count.ShouldBe(2);

        definitionsSecondRetrieval[0].Name.ShouldBe("Definition1");
        definitionsSecondRetrieval[0].Value.ShouldBe(1);

        definitionsSecondRetrieval[1].Name.ShouldBe("Definition2");
        definitionsSecondRetrieval[1].Value.ShouldBe(2);
    }

    [Fact]
    public async Task Separate_Caches_For_Different_Types_Test()
    {
        var definitions1 = await _staticDefinitionCache1.GetOrCreateAsync(() =>
        {
            return Task.FromResult(new List<StaticDefinition1> { new StaticDefinition1 {Name = "Definition1", Value = 1} });
        });
        var definitions2 = await _staticDefinitionCache2.GetOrCreateAsync(() =>
        {
            return Task.FromResult(new List<StaticDefinition2> { new StaticDefinition2 {Name = "DefinitionA", Value = 100} });
        });

        definitions1.Count.ShouldBe(1);
        definitions1[0].Name.ShouldBe("Definition1");
        definitions1[0].Value.ShouldBe(1);

        definitions2.Count.ShouldBe(1);
        definitions2[0].Name.ShouldBe("DefinitionA");
        definitions2[0].Value.ShouldBe(100);
    }

    [Fact]
    public async Task Clear_Test()
    {
        var definitions1 = await _staticDefinitionCache1.GetOrCreateAsync(() =>
        {
            return Task.FromResult(new List<StaticDefinition1> { new StaticDefinition1 {Name = "Definition1", Value = 1} });
        });
        var definitions2 = await _staticDefinitionCache2.GetOrCreateAsync(() =>
        {
            return Task.FromResult(new List<StaticDefinition2> { new StaticDefinition2 {Name = "DefinitionA", Value = 100} });
        });

        definitions1.Count.ShouldBe(1);
        definitions1[0].Name.ShouldBe("Definition1");
        definitions1[0].Value.ShouldBe(1);

        definitions2.Count.ShouldBe(1);
        definitions2[0].Name.ShouldBe("DefinitionA");
        definitions2[0].Value.ShouldBe(100);

        await _staticDefinitionCache1.ClearAsync();
        await _staticDefinitionCache2.ClearAsync();

        var definitions1AfterClear = await _staticDefinitionCache1.GetOrCreateAsync(() =>
        {
            return Task.FromResult(new List<StaticDefinition1> { new StaticDefinition1 {Name = "DefinitionNew", Value = 10} });
        });
        var definitions2AfterClear = await _staticDefinitionCache2.GetOrCreateAsync(() =>
        {
            return Task.FromResult(new List<StaticDefinition2> {new StaticDefinition2 {Name = "DefinitionNewA", Value = 200}});
        });

        definitions1AfterClear.Count.ShouldBe(1);
        definitions1AfterClear[0].Name.ShouldBe("DefinitionNew");
        definitions1AfterClear[0].Value.ShouldBe(10);

        definitions2AfterClear.Count.ShouldBe(1);
        definitions2AfterClear[0].Name.ShouldBe("DefinitionNewA");
        definitions2AfterClear[0].Value.ShouldBe(200);
    }

    public class StaticDefinition1
    {
        public string Name { get; set; }

        public int Value { get; set; }
    }

    public class StaticDefinition2
    {
        public string Name { get; set; }

        public int Value { get; set; }
    }
}
