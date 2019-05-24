using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Domain.Repositories.MemoryDb
{
    public class MemoryDbBinarySerializer : IMemoryDbSerializer, ITransientDependency
    {
        public object Serialize(object obj)
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            return stream;
        }

        public object Deserialize(object obj)
        {
            if (!(obj is MemoryStream stream))
            {
                return null;
            }

            var formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            return formatter.Deserialize(stream);
        }
    }
}