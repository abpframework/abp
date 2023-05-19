using System;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Timing;

namespace Volo.Abp.AspNetCore.Mvc.ModelBinding;

[Route("api/model-Binding-test")]
public class ModelBindingController : AbpController
{
    [HttpGet("DateTimeKind")]
    public string DateTimeKind(DateTime input)
    {
        return input.Kind.ToString().ToLower();
    }

    [HttpGet("NullableDateTimeKind")]
    public string NullableDateTimeKind(DateTime? input)
    {
        return input.Value.Kind.ToString().ToLower();
    }

    [HttpGet("DisableDateTimeNormalizationDateTimeKind")]
    public string DisableDateTimeNormalizationDateTimeKind([DisableDateTimeNormalization] DateTime input)
    {
        return input.Kind.ToString().ToLower();
    }

    [HttpGet("DisableDateTimeNormalizationNullableDateTimeKind")]
    public string DisableDateTimeNormalizationNullableDateTimeKind([DisableDateTimeNormalization] DateTime? input)
    {
        return input.Value.Kind.ToString().ToLower();
    }

    [HttpGet("ComplexTypeDateTimeKind")]
    public string ComplexTypeDateTimeKind(GetDateTimeKindModel input)
    {
        return input.Time1.Kind.ToString().ToLower() + "_" +
               input.Time2.Kind.ToString().ToLower() + "_" +
               input.Time3.Value.Kind.ToString().ToLower() + "_" +
               input.InnerModel.Time4.Kind.ToString().ToLower();
    }

    //JSON input and output.
    [HttpPost("ComplexTypeDateTimeKind_JSON")]
    public string ComplexTypeDateTimeKind_JSON([FromBody] GetDateTimeKindModel input)
    {
        return input.Time1.Kind.ToString().ToLower() + "_" +
               input.Time2.Kind.ToString().ToLower() + "_" +
               input.Time3.Value.Kind.ToString().ToLower() + "_" +
               input.InnerModel.Time4.Kind.ToString().ToLower();
    }

    [HttpPost("Guid_Json_Test")]
    public GuidJsonModel Guid_Json_Test([FromBody] GuidJsonModel input)
    {
        return input;
    }
}

public class GetDateTimeKindModel
{
    [DisableDateTimeNormalization]
    public DateTime Time1 { get; set; }

    public DateTime Time2 { get; set; }

    public DateTime? Time3 { get; set; }

    public GetDateTimeKindInnerModel InnerModel { get; set; }

    [DisableDateTimeNormalization]
    public class GetDateTimeKindInnerModel
    {
        public DateTime Time4 { get; set; }
    }
}

public class GuidJsonModel
{
    public Guid UserId { get; set; }

    public Guid UserId2 { get; set; }

    public Guid UserId3 { get; set; }

    public Guid UserId4 { get; set; }

    public Guid UserId5 { get; set; }

    public Guid? TenantId { get; set; }

    public Guid? TenantId2 { get; set; }
}
