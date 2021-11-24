using System;

namespace Volo.Abp.Validation;

/// <summary>
/// Can be added to a method to enable auto validation if validation is disabled for it's class.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class EnableValidationAttribute : Attribute
{

}
