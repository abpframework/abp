import { Provider } from '@angular/core';
import { CUSTOM_ERROR_HANDLERS } from '../tokens';
import {
  TenantResolveErrorHandlerService,
  AbpFormatErrorHandlerService,
  StatusCodeErrorHandlerService,
  UnknownStatusCodeErrorHandlerService,
  AbpAuthenticationErrorHandler,
} from '../services';

export const DEFAULT_HANDLERS_PROVIDERS: Provider[] = [
  {
    provide: CUSTOM_ERROR_HANDLERS,
    multi: true,
    useExisting: TenantResolveErrorHandlerService,
  },
  {
    provide: CUSTOM_ERROR_HANDLERS,
    multi: true,
    useExisting: AbpFormatErrorHandlerService,
  },
  {
    provide: CUSTOM_ERROR_HANDLERS,
    multi: true,
    useExisting: StatusCodeErrorHandlerService,
  },
  {
    provide: CUSTOM_ERROR_HANDLERS,
    multi: true,
    useExisting: UnknownStatusCodeErrorHandlerService,
  },
  {
    provide: CUSTOM_ERROR_HANDLERS,
    multi: true,
    useExisting: AbpAuthenticationErrorHandler,
  },
];
