import { InjectionToken, inject } from '@angular/core';
import { Router } from '@angular/router';

export const HAS_ACCOUNT_MODULE = new InjectionToken('HAS_ACCOUNT_MODULE', {
  providedIn: 'root',
  factory: () => {
    const router = inject(Router);
    return router.config.findIndex(route => route.path === 'account') > -1;
  },
});
