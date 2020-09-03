namespace Volo.Abp.MultiLingualObject
{
    public interface IMultiLingualTranslation
    {
        string Language { get; set; }
    }

    public interface IMultiLingualTranslation<T, TPrimaryKeyOfMultiLingualObject> : IMultiLingualTranslation
        where T : class
    {
        T Core { get; set; }

        TPrimaryKeyOfMultiLingualObject CoreId { get; set; }
    }
}
