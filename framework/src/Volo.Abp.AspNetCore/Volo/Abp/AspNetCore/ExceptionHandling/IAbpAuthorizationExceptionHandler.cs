﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Authorization;

namespace Volo.Abp.AspNetCore.ExceptionHandling;

public interface IAbpAuthorizationExceptionHandler
{
    Task HandleAsync(AbpAuthorizationException exception, HttpContext httpContext);
}
