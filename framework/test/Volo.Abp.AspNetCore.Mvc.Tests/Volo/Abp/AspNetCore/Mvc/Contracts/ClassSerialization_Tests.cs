using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Volo.Abp.Modularity;
using Volo.Abp.Reflection;
using Volo.Abp.Serialization;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Contracts
{
    public class ClassSerialization_Tests : AspNetCoreMvcTestBase
    {


        [Fact]
        public void Should_Have_Serializable_Attribute()
        {
            var assemblyFinder = GetRequiredService<IAssemblyFinder>();

            var assembly = assemblyFinder.Assemblies
                                         .Where(x => x.ManifestModule.Name == "Volo.Abp.AspNetCore.Mvc.Contracts.dll")
                                         .Single();

            var classes = assembly.DefinedTypes
                                  .Where(x => x.IsClass)
                                  .Where(x => !x.IsAssignableTo<AbpModule>())
                                  .ToList();

            foreach (var typeInfo in classes)
            {
                var attr = typeInfo.GetCustomAttribute<SerializableAttribute>();
                if (attr == null)
                    throw new Exception($"Class {typeInfo.FullName} does not contain [Serializable] attribute.");
            }
        }
    }
}
