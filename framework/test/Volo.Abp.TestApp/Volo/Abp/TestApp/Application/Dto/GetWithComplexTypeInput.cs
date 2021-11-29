namespace Volo.Abp.TestApp.Application.Dto
{
    public class GetWithComplexTypeInput
    {
        public string Value1 { get; set; }

        public GetWithComplexTypeInner Inner1 { get; set; }

        public class GetWithComplexTypeInner
        {
            public string Value2 { get; set; }
            public GetWithComplexTypeInnerInner Inner2 { get; set; }
        }

        public class GetWithComplexTypeInnerInner
        {
            public string Value3 { get; set; }
        }
    }
}