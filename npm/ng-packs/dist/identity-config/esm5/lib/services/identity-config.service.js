/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { addAbpRoutes, RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import * as i0 from '@angular/core';
import * as i1 from '@angular/router';
import * as i2 from '@abp/ng.core';
var IdentityConfigService = /** @class */ (function() {
  function IdentityConfigService(router, restService) {
    this.router = router;
    this.restService = restService;
    addAbpRoutes([
      {
        name: 'AbpUiNavigation::Menu:Administration',
        path: '',
        order: 1,
        wrapper: true,
        iconClass: 'fa fa-wrench',
      },
      {
        name: 'AbpIdentity::Menu:IdentityManagement',
        path: 'identity',
        order: 1,
        parentName: 'AbpUiNavigation::Menu:Administration',
        layout: 'application' /* application */,
        iconClass: 'fa fa-id-card-o',
        children: [
          { path: 'roles', name: 'AbpIdentity::Roles', order: 1, requiredPolicy: 'AbpIdentity.Roles' },
          { path: 'users', name: 'AbpIdentity::Users', order: 2, requiredPolicy: 'AbpIdentity.Users' },
        ],
      },
    ]);
  }
  IdentityConfigService.decorators = [
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
  IdentityConfigService.ctorParameters = function() {
    return [{ type: Router }, { type: RestService }];
  };
  /** @nocollapse */ IdentityConfigService.ngInjectableDef = i0.ɵɵdefineInjectable({
    factory: function IdentityConfigService_Factory() {
      return new IdentityConfigService(i0.ɵɵinject(i1.Router), i0.ɵɵinject(i2.RestService));
    },
    token: IdentityConfigService,
    providedIn: 'root',
  });
  return IdentityConfigService;
})();
export { IdentityConfigService };
if (false) {
  /**
   * @type {?}
   * @private
   */
  IdentityConfigService.prototype.router;
  /**
   * @type {?}
   * @private
   */
  IdentityConfigService.prototype.restService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHktY29uZmlnLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LmNvbmZpZy8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9pZGVudGl0eS1jb25maWcuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFlBQVksRUFBZSxXQUFXLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDdEUsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0saUJBQWlCLENBQUM7Ozs7QUFHekM7SUFJRSwrQkFBb0IsTUFBYyxFQUFVLFdBQXdCO1FBQWhELFdBQU0sR0FBTixNQUFNLENBQVE7UUFBVSxnQkFBVyxHQUFYLFdBQVcsQ0FBYTtRQUNsRSxZQUFZLENBQUM7WUFDWDtnQkFDRSxJQUFJLEVBQUUsc0NBQXNDO2dCQUM1QyxJQUFJLEVBQUUsRUFBRTtnQkFDUixLQUFLLEVBQUUsQ0FBQztnQkFDUixPQUFPLEVBQUUsSUFBSTtnQkFDYixTQUFTLEVBQUUsY0FBYzthQUMxQjtZQUNEO2dCQUNFLElBQUksRUFBRSxzQ0FBc0M7Z0JBQzVDLElBQUksRUFBRSxVQUFVO2dCQUNoQixLQUFLLEVBQUUsQ0FBQztnQkFDUixVQUFVLEVBQUUsc0NBQXNDO2dCQUNsRCxNQUFNLGlDQUF5QjtnQkFDL0IsU0FBUyxFQUFFLGlCQUFpQjtnQkFDNUIsUUFBUSxFQUFFO29CQUNSLEVBQUUsSUFBSSxFQUFFLE9BQU8sRUFBRSxJQUFJLEVBQUUsb0JBQW9CLEVBQUUsS0FBSyxFQUFFLENBQUMsRUFBRSxjQUFjLEVBQUUsbUJBQW1CLEVBQUU7b0JBQzVGLEVBQUUsSUFBSSxFQUFFLE9BQU8sRUFBRSxJQUFJLEVBQUUsb0JBQW9CLEVBQUUsS0FBSyxFQUFFLENBQUMsRUFBRSxjQUFjLEVBQUUsbUJBQW1CLEVBQUU7aUJBQzdGO2FBQ0Y7U0FDRixDQUFDLENBQUM7SUFDTCxDQUFDOztnQkExQkYsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7OztnQkFMUSxNQUFNO2dCQUZxQixXQUFXOzs7Z0NBQS9DO0NBZ0NDLEFBM0JELElBMkJDO1NBeEJZLHFCQUFxQjs7Ozs7O0lBQ3BCLHVDQUFzQjs7Ozs7SUFBRSw0Q0FBZ0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBhZGRBYnBSb3V0ZXMsIGVMYXlvdXRUeXBlLCBSZXN0U2VydmljZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBSb3V0ZXIgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuXG5ASW5qZWN0YWJsZSh7XG4gIHByb3ZpZGVkSW46ICdyb290Jyxcbn0pXG5leHBvcnQgY2xhc3MgSWRlbnRpdHlDb25maWdTZXJ2aWNlIHtcbiAgY29uc3RydWN0b3IocHJpdmF0ZSByb3V0ZXI6IFJvdXRlciwgcHJpdmF0ZSByZXN0U2VydmljZTogUmVzdFNlcnZpY2UpIHtcbiAgICBhZGRBYnBSb3V0ZXMoW1xuICAgICAge1xuICAgICAgICBuYW1lOiAnQWJwVWlOYXZpZ2F0aW9uOjpNZW51OkFkbWluaXN0cmF0aW9uJyxcbiAgICAgICAgcGF0aDogJycsXG4gICAgICAgIG9yZGVyOiAxLFxuICAgICAgICB3cmFwcGVyOiB0cnVlLFxuICAgICAgICBpY29uQ2xhc3M6ICdmYSBmYS13cmVuY2gnLFxuICAgICAgfSxcbiAgICAgIHtcbiAgICAgICAgbmFtZTogJ0FicElkZW50aXR5OjpNZW51OklkZW50aXR5TWFuYWdlbWVudCcsXG4gICAgICAgIHBhdGg6ICdpZGVudGl0eScsXG4gICAgICAgIG9yZGVyOiAxLFxuICAgICAgICBwYXJlbnROYW1lOiAnQWJwVWlOYXZpZ2F0aW9uOjpNZW51OkFkbWluaXN0cmF0aW9uJyxcbiAgICAgICAgbGF5b3V0OiBlTGF5b3V0VHlwZS5hcHBsaWNhdGlvbixcbiAgICAgICAgaWNvbkNsYXNzOiAnZmEgZmEtaWQtY2FyZC1vJyxcbiAgICAgICAgY2hpbGRyZW46IFtcbiAgICAgICAgICB7IHBhdGg6ICdyb2xlcycsIG5hbWU6ICdBYnBJZGVudGl0eTo6Um9sZXMnLCBvcmRlcjogMSwgcmVxdWlyZWRQb2xpY3k6ICdBYnBJZGVudGl0eS5Sb2xlcycgfSxcbiAgICAgICAgICB7IHBhdGg6ICd1c2VycycsIG5hbWU6ICdBYnBJZGVudGl0eTo6VXNlcnMnLCBvcmRlcjogMiwgcmVxdWlyZWRQb2xpY3k6ICdBYnBJZGVudGl0eS5Vc2VycycgfSxcbiAgICAgICAgXSxcbiAgICAgIH0sXG4gICAgXSk7XG4gIH1cbn1cbiJdfQ==
