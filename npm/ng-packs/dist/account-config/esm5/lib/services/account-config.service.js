/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { RestService, addAbpRoutes } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import * as i0 from '@angular/core';
import * as i1 from '@angular/router';
import * as i2 from '@abp/ng.core';
var AccountConfigService = /** @class */ (function() {
  function AccountConfigService(router, restService) {
    this.router = router;
    this.restService = restService;
    addAbpRoutes({
      name: 'AbpAccount::Menu:Account',
      path: 'account',
      invisible: true,
      layout: 'application' /* application */,
      children: [
        { path: 'login', name: 'AbpAccount::Login', order: 1 },
        { path: 'register', name: 'AbpAccount::Register', order: 2 },
        { path: 'manage-profile', name: 'AbpAccount::ManageYourProfile', order: 3 },
      ],
    });
  }
  AccountConfigService.decorators = [
    {
      type: Injectable,
      args: [
        {
          providedIn: 'root',
        },
      ],
    },
  ];
  /** @nocollapse */
  AccountConfigService.ctorParameters = function() {
    return [{ type: Router }, { type: RestService }];
  };
  /** @nocollapse */ AccountConfigService.ngInjectableDef = i0.ɵɵdefineInjectable({
    factory: function AccountConfigService_Factory() {
      return new AccountConfigService(i0.ɵɵinject(i1.Router), i0.ɵɵinject(i2.RestService));
    },
    token: AccountConfigService,
    providedIn: 'root',
  });
  return AccountConfigService;
})();
export { AccountConfigService };
if (false) {
  /**
   * @type {?}
   * @private
   */
  AccountConfigService.prototype.router;
  /**
   * @type {?}
   * @private
   */
  AccountConfigService.prototype.restService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC1jb25maWcuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuYWNjb3VudC5jb25maWcvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvYWNjb3VudC1jb25maWcuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFlLFdBQVcsRUFBRSxZQUFZLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDdEUsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0saUJBQWlCLENBQUM7Ozs7QUFFekM7SUFJRSw4QkFBb0IsTUFBYyxFQUFVLFdBQXdCO1FBQWhELFdBQU0sR0FBTixNQUFNLENBQVE7UUFBVSxnQkFBVyxHQUFYLFdBQVcsQ0FBYTtRQUNsRSxZQUFZLENBQUM7WUFDWCxJQUFJLEVBQUUsMEJBQTBCO1lBQ2hDLElBQUksRUFBRSxTQUFTO1lBQ2YsU0FBUyxFQUFFLElBQUk7WUFDZixNQUFNLGlDQUF5QjtZQUMvQixRQUFRLEVBQUU7Z0JBQ1IsRUFBRSxJQUFJLEVBQUUsT0FBTyxFQUFFLElBQUksRUFBRSxtQkFBbUIsRUFBRSxLQUFLLEVBQUUsQ0FBQyxFQUFFO2dCQUN0RCxFQUFFLElBQUksRUFBRSxVQUFVLEVBQUUsSUFBSSxFQUFFLHNCQUFzQixFQUFFLEtBQUssRUFBRSxDQUFDLEVBQUU7Z0JBQzVELEVBQUUsSUFBSSxFQUFFLGdCQUFnQixFQUFFLElBQUksRUFBRSwrQkFBK0IsRUFBRSxLQUFLLEVBQUUsQ0FBQyxFQUFFO2FBQzVFO1NBQ0YsQ0FBQyxDQUFDO0lBQ0wsQ0FBQzs7Z0JBaEJGLFVBQVUsU0FBQztvQkFDVixVQUFVLEVBQUUsTUFBTTtpQkFDbkI7Ozs7Z0JBSlEsTUFBTTtnQkFGTyxXQUFXOzs7K0JBQWpDO0NBcUJDLEFBakJELElBaUJDO1NBZFksb0JBQW9COzs7Ozs7SUFDbkIsc0NBQXNCOzs7OztJQUFFLDJDQUFnQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IGVMYXlvdXRUeXBlLCBSZXN0U2VydmljZSwgYWRkQWJwUm91dGVzIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBBY2NvdW50Q29uZmlnU2VydmljZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcm91dGVyOiBSb3V0ZXIsIHByaXZhdGUgcmVzdFNlcnZpY2U6IFJlc3RTZXJ2aWNlKSB7XG4gICAgYWRkQWJwUm91dGVzKHtcbiAgICAgIG5hbWU6ICdBYnBBY2NvdW50OjpNZW51OkFjY291bnQnLFxuICAgICAgcGF0aDogJ2FjY291bnQnLFxuICAgICAgaW52aXNpYmxlOiB0cnVlLFxuICAgICAgbGF5b3V0OiBlTGF5b3V0VHlwZS5hcHBsaWNhdGlvbixcbiAgICAgIGNoaWxkcmVuOiBbXG4gICAgICAgIHsgcGF0aDogJ2xvZ2luJywgbmFtZTogJ0FicEFjY291bnQ6OkxvZ2luJywgb3JkZXI6IDEgfSxcbiAgICAgICAgeyBwYXRoOiAncmVnaXN0ZXInLCBuYW1lOiAnQWJwQWNjb3VudDo6UmVnaXN0ZXInLCBvcmRlcjogMiB9LFxuICAgICAgICB7IHBhdGg6ICdtYW5hZ2UtcHJvZmlsZScsIG5hbWU6ICdBYnBBY2NvdW50OjpNYW5hZ2VZb3VyUHJvZmlsZScsIG9yZGVyOiAzIH0sXG4gICAgICBdLFxuICAgIH0pO1xuICB9XG59XG4iXX0=
