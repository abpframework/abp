namespace Volo.DependencyInjection
{
    public class ObjectAccessor<T> : IObjectAccessor<T>
    {
        public T Object { get; set; }

        public ObjectAccessor()
        {
            
        }

        public ObjectAccessor(T obj)
        {
            Object = obj;
        }
    }
}