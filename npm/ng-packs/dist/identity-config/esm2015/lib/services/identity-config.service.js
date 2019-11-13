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
export class IdentityConfigService {
  /**
   * @param {?} router
   * @param {?} restService
   */
  constructor(router, restService) {
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
IdentityConfigService.ctorParameters = () => [{ type: Router }, { type: RestService }];
/** @nocollapse */ IdentityConfigService.ngInjectableDef = i0.ɵɵdefineInjectable({
  factory: function IdentityConfigService_Factory() {
    return new IdentityConfigService(i0.ɵɵinject(i1.Router), i0.ɵɵinject(i2.RestService));
  },
  token: IdentityConfigService,
  providedIn: 'root',
});
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHktY29uZmlnLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LmNvbmZpZy8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9pZGVudGl0eS1jb25maWcuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFlBQVksRUFBZSxXQUFXLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDdEUsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0saUJBQWlCLENBQUM7Ozs7QUFNekMsTUFBTSxPQUFPLHFCQUFxQjs7Ozs7SUFDaEMsWUFBb0IsTUFBYyxFQUFVLFdBQXdCO1FBQWhELFdBQU0sR0FBTixNQUFNLENBQVE7UUFBVSxnQkFBVyxHQUFYLFdBQVcsQ0FBYTtRQUNsRSxZQUFZLENBQUM7WUFDWDtnQkFDRSxJQUFJLEVBQUUsc0NBQXNDO2dCQUM1QyxJQUFJLEVBQUUsRUFBRTtnQkFDUixLQUFLLEVBQUUsQ0FBQztnQkFDUixPQUFPLEVBQUUsSUFBSTtnQkFDYixTQUFTLEVBQUUsY0FBYzthQUMxQjtZQUNEO2dCQUNFLElBQUksRUFBRSxzQ0FBc0M7Z0JBQzVDLElBQUksRUFBRSxVQUFVO2dCQUNoQixLQUFLLEVBQUUsQ0FBQztnQkFDUixVQUFVLEVBQUUsc0NBQXNDO2dCQUNsRCxNQUFNLGlDQUF5QjtnQkFDL0IsU0FBUyxFQUFFLGlCQUFpQjtnQkFDNUIsUUFBUSxFQUFFO29CQUNSLEVBQUUsSUFBSSxFQUFFLE9BQU8sRUFBRSxJQUFJLEVBQUUsb0JBQW9CLEVBQUUsS0FBSyxFQUFFLENBQUMsRUFBRSxjQUFjLEVBQUUsbUJBQW1CLEVBQUU7b0JBQzVGLEVBQUUsSUFBSSxFQUFFLE9BQU8sRUFBRSxJQUFJLEVBQUUsb0JBQW9CLEVBQUUsS0FBSyxFQUFFLENBQUMsRUFBRSxjQUFjLEVBQUUsbUJBQW1CLEVBQUU7aUJBQzdGO2FBQ0Y7U0FDRixDQUFDLENBQUM7SUFDTCxDQUFDOzs7WUExQkYsVUFBVSxTQUFDO2dCQUNWLFVBQVUsRUFBRSxNQUFNO2FBQ25COzs7O1lBTFEsTUFBTTtZQUZxQixXQUFXOzs7Ozs7OztJQVNqQyx1Q0FBc0I7Ozs7O0lBQUUsNENBQWdDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgYWRkQWJwUm91dGVzLCBlTGF5b3V0VHlwZSwgUmVzdFNlcnZpY2UgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgUm91dGVyIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcblxuQEluamVjdGFibGUoe1xuICBwcm92aWRlZEluOiAncm9vdCcsXG59KVxuZXhwb3J0IGNsYXNzIElkZW50aXR5Q29uZmlnU2VydmljZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcm91dGVyOiBSb3V0ZXIsIHByaXZhdGUgcmVzdFNlcnZpY2U6IFJlc3RTZXJ2aWNlKSB7XG4gICAgYWRkQWJwUm91dGVzKFtcbiAgICAgIHtcbiAgICAgICAgbmFtZTogJ0FicFVpTmF2aWdhdGlvbjo6TWVudTpBZG1pbmlzdHJhdGlvbicsXG4gICAgICAgIHBhdGg6ICcnLFxuICAgICAgICBvcmRlcjogMSxcbiAgICAgICAgd3JhcHBlcjogdHJ1ZSxcbiAgICAgICAgaWNvbkNsYXNzOiAnZmEgZmEtd3JlbmNoJyxcbiAgICAgIH0sXG4gICAgICB7XG4gICAgICAgIG5hbWU6ICdBYnBJZGVudGl0eTo6TWVudTpJZGVudGl0eU1hbmFnZW1lbnQnLFxuICAgICAgICBwYXRoOiAnaWRlbnRpdHknLFxuICAgICAgICBvcmRlcjogMSxcbiAgICAgICAgcGFyZW50TmFtZTogJ0FicFVpTmF2aWdhdGlvbjo6TWVudTpBZG1pbmlzdHJhdGlvbicsXG4gICAgICAgIGxheW91dDogZUxheW91dFR5cGUuYXBwbGljYXRpb24sXG4gICAgICAgIGljb25DbGFzczogJ2ZhIGZhLWlkLWNhcmQtbycsXG4gICAgICAgIGNoaWxkcmVuOiBbXG4gICAgICAgICAgeyBwYXRoOiAncm9sZXMnLCBuYW1lOiAnQWJwSWRlbnRpdHk6OlJvbGVzJywgb3JkZXI6IDEsIHJlcXVpcmVkUG9saWN5OiAnQWJwSWRlbnRpdHkuUm9sZXMnIH0sXG4gICAgICAgICAgeyBwYXRoOiAndXNlcnMnLCBuYW1lOiAnQWJwSWRlbnRpdHk6OlVzZXJzJywgb3JkZXI6IDIsIHJlcXVpcmVkUG9saWN5OiAnQWJwSWRlbnRpdHkuVXNlcnMnIH0sXG4gICAgICAgIF0sXG4gICAgICB9LFxuICAgIF0pO1xuICB9XG59XG4iXX0=
