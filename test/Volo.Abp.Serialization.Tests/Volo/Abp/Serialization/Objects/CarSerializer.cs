using Volo.Abp.DependencyInjection;
using Volo.Abp.Serialization.Binary;

namespace Volo.Abp.Serialization.Objects
{
    public class CarSerializer : IObjectSerializer<Car>, ITransientDependency
    {
        public byte[] Serialize(Car obj)
        {
            obj.Name += "-serialized";
            return BinarySerializationHelper.Serialize(obj);
        }

        public Car Deserialize(byte[] bytes)
        {
            var car = (Car)BinarySerializationHelper.DeserializeExtended(bytes);
            car.Name += "-deserialized";
            return car;
        }
    }
}