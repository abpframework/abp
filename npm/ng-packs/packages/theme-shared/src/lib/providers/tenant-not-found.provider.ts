import { TENANT_NOT_FOUND_BY_NAME } from '@abp/ng.core';
import { inject, Provider } from '@angular/core';
import { ConfirmationService } from '../services';
import { HttpErrorResponse } from '@angular/common/http';

export const tenantNotFoundProvider: Provider = {
  provide: TENANT_NOT_FOUND_BY_NAME,
  useFactory: function () {
    const confirm = inject(ConfirmationService);
    return (response: HttpErrorResponse) => {
      const { error } = response.error;
      // hide loading donut
      const appRoot = document.querySelector('app-root div.donut');
      if (appRoot) {
        appRoot.remove();
      }
      confirm.error(error.details, error.message, { hideCancelBtn: true, hideYesBtn: true });
    };
  },
};
