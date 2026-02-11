import { inject } from '@angular/core';
import { HttpHandlerFn, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { TimezoneService } from '../services';

export const timezoneInterceptor: HttpInterceptorFn = (
  req: HttpRequest<any>,
  next: HttpHandlerFn,
) => {
  const timezoneService = inject(TimezoneService);

  if (!timezoneService.isUtcClockEnabled) {
    return next(req);
  }
  const timezone = timezoneService.timezone;
  if (timezone) {
    req = req.clone({
      setHeaders: {
        __timezone: timezone,
      },
    });
  }
  return next(req);
};
