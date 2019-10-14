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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC1jb25maWcuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuYWNjb3VudC5jb25maWcvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvYWNjb3VudC1jb25maWcuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFlLFdBQVcsRUFBRSxZQUFZLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDdEUsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0saUJBQWlCLENBQUM7Ozs7QUFFekM7SUFJRSw4QkFBb0IsTUFBYyxFQUFVLFdBQXdCO1FBQWhELFdBQU0sR0FBTixNQUFNLENBQVE7UUFBVSxnQkFBVyxHQUFYLFdBQVcsQ0FBYTtRQUNsRSxZQUFZLENBQUM7WUFDWCxJQUFJLEVBQUUsMEJBQTBCO1lBQ2hDLElBQUksRUFBRSxTQUFTO1lBQ2YsU0FBUyxFQUFFLElBQUk7WUFDZixNQUFNLGlDQUF5QjtZQUMvQixRQUFRLEVBQUU7Z0JBQ1IsRUFBRSxJQUFJLEVBQUUsT0FBTyxFQUFFLElBQUksRUFBRSxtQkFBbUIsRUFBRSxLQUFLLEVBQUUsQ0FBQyxFQUFFO2dCQUN0RCxFQUFFLElBQUksRUFBRSxVQUFVLEVBQUUsSUFBSSxFQUFFLHNCQUFzQixFQUFFLEtBQUssRUFBRSxDQUFDLEVBQUU7YUFDN0Q7U0FDRixDQUFDLENBQUM7SUFDTCxDQUFDOztnQkFmRixVQUFVLFNBQUM7b0JBQ1YsVUFBVSxFQUFFLE1BQU07aUJBQ25COzs7O2dCQUpRLE1BQU07Z0JBRk8sV0FBVzs7OytCQUFqQztDQW9CQyxBQWhCRCxJQWdCQztTQWJZLG9CQUFvQjs7Ozs7O0lBQ25CLHNDQUFzQjs7Ozs7SUFBRSwyQ0FBZ0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBlTGF5b3V0VHlwZSwgUmVzdFNlcnZpY2UsIGFkZEFicFJvdXRlcyB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBSb3V0ZXIgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuXG5ASW5qZWN0YWJsZSh7XG4gIHByb3ZpZGVkSW46ICdyb290Jyxcbn0pXG5leHBvcnQgY2xhc3MgQWNjb3VudENvbmZpZ1NlcnZpY2Uge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJvdXRlcjogUm91dGVyLCBwcml2YXRlIHJlc3RTZXJ2aWNlOiBSZXN0U2VydmljZSkge1xuICAgIGFkZEFicFJvdXRlcyh7XG4gICAgICBuYW1lOiAnQWJwQWNjb3VudDo6TWVudTpBY2NvdW50JyxcbiAgICAgIHBhdGg6ICdhY2NvdW50JyxcbiAgICAgIGludmlzaWJsZTogdHJ1ZSxcbiAgICAgIGxheW91dDogZUxheW91dFR5cGUuYXBwbGljYXRpb24sXG4gICAgICBjaGlsZHJlbjogW1xuICAgICAgICB7IHBhdGg6ICdsb2dpbicsIG5hbWU6ICdBYnBBY2NvdW50OjpMb2dpbicsIG9yZGVyOiAxIH0sXG4gICAgICAgIHsgcGF0aDogJ3JlZ2lzdGVyJywgbmFtZTogJ0FicEFjY291bnQ6OlJlZ2lzdGVyJywgb3JkZXI6IDIgfSxcbiAgICAgIF0sXG4gICAgfSk7XG4gIH1cbn1cbiJdfQ==
