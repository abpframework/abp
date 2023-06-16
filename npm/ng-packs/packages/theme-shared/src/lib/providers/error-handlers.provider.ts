import { Provider } from '@angular/core';
import { CUSTOM_ERROR_HANDLERS } from '../tokens';
import { TenantResolveErrorHandlerService } from '../services/tenant-resolve-error-handler.service';
import { AbpFormatErrorHandlerService } from '../services/abp-format-error-handler.service';
import { StatusCodeErrorHandlerService } from '../services/status-code-error-handler.service';
import { UnknownStatusCodeErrorHandlerService } from '../services/unknown-status-code-error-handler.service';

export const ERROR_HANDLERS_PROVIDERS: Provider[] = [
  {
    provide: CUSTOM_ERROR_HANDLERS,
    multi: true,
    useClass: TenantResolveErrorHandlerService,
  },
  {
    provide: CUSTOM_ERROR_HANDLERS,
    multi: true,
    useClass: AbpFormatErrorHandlerService,
  },
  {
    provide: CUSTOM_ERROR_HANDLERS,
    multi: true,
    useClass: StatusCodeErrorHandlerService,
  },
  {
    provide: CUSTOM_ERROR_HANDLERS,
    multi: true,
    useClass: UnknownStatusCodeErrorHandlerService,
  },
];
