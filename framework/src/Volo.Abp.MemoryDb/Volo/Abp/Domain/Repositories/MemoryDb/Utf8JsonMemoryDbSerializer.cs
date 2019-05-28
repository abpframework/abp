using System;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.Domain.Repositories.MemoryDb
{
    public class Utf8JsonMemoryDbSerializer : IMemoryDbSerializer, ITransientDependency
    {
        private readonly IJsonSerializer _jsonSerializer;

        public Utf8JsonMemoryDbSerializer(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        byte[] IMemoryDbSerializer.Serialize(object obj)
        {
            return Encoding.UTF8.GetBytes(_jsonSerializer.Serialize(obj));
        }

        public object Deserialize(byte[] value, Type type)
        {
            return _jsonSerializer.Deserialize(type, Encoding.UTF8.GetString(value));
        }
    }
}