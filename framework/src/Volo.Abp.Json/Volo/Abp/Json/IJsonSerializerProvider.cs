using System;
using JetBrains.Annotations;

namespace Volo.Abp.Json
{
    public interface IJsonSerializerProvider
    {
        bool CanHandle([CanBeNull]Type type);

        string Serialize(object obj, bool camelCase = true, bool indented = false);

        T Deserialize<T>(string jsonString, bool camelCase = true);

        object Deserialize(Type type, string jsonString, bool camelCase = true);
    }
}
