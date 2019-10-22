/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { addAbpRoutes, RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import * as i0 from "@angular/core";
import * as i1 from "@angular/router";
import * as i2 from "@abp/ng.core";
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
            },
            {
                name: 'AbpIdentity::Menu:IdentityManagement',
                path: 'identity',
                order: 1,
                parentName: 'AbpUiNavigation::Menu:Administration',
                layout: "application" /* application */,
                iconClass: 'fa fa-id-card-o',
                children: [
                    { path: 'roles', name: 'AbpIdentity::Roles', order: 2, requiredPolicy: 'AbpIdentity.Roles' },
                    { path: 'users', name: 'AbpIdentity::Users', order: 1, requiredPolicy: 'AbpIdentity.Users' },
                ],
            },
        ]);
    }
}
IdentityConfigService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
IdentityConfigService.ctorParameters = () => [
    { type: Router },
    { type: RestService }
];
/** @nocollapse */ IdentityConfigService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function IdentityConfigService_Factory() { return new IdentityConfigService(i0.ɵɵinject(i1.Router), i0.ɵɵinject(i2.RestService)); }, token: IdentityConfigService, providedIn: "root" });
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHktY29uZmlnLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LmNvbmZpZy8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9pZGVudGl0eS1jb25maWcuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFlBQVksRUFBZSxXQUFXLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDdEUsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0saUJBQWlCLENBQUM7Ozs7QUFNekMsTUFBTSxPQUFPLHFCQUFxQjs7Ozs7SUFDaEMsWUFBb0IsTUFBYyxFQUFVLFdBQXdCO1FBQWhELFdBQU0sR0FBTixNQUFNLENBQVE7UUFBVSxnQkFBVyxHQUFYLFdBQVcsQ0FBYTtRQUNsRSxZQUFZLENBQUM7WUFDWDtnQkFDRSxJQUFJLEVBQUUsc0NBQXNDO2dCQUM1QyxJQUFJLEVBQUUsRUFBRTtnQkFDUixLQUFLLEVBQUUsQ0FBQztnQkFDUixPQUFPLEVBQUUsSUFBSTthQUNkO1lBQ0Q7Z0JBQ0UsSUFBSSxFQUFFLHNDQUFzQztnQkFDNUMsSUFBSSxFQUFFLFVBQVU7Z0JBQ2hCLEtBQUssRUFBRSxDQUFDO2dCQUNSLFVBQVUsRUFBRSxzQ0FBc0M7Z0JBQ2xELE1BQU0saUNBQXlCO2dCQUMvQixTQUFTLEVBQUUsaUJBQWlCO2dCQUM1QixRQUFRLEVBQUU7b0JBQ1IsRUFBRSxJQUFJLEVBQUUsT0FBTyxFQUFFLElBQUksRUFBRSxvQkFBb0IsRUFBRSxLQUFLLEVBQUUsQ0FBQyxFQUFFLGNBQWMsRUFBRSxtQkFBbUIsRUFBRTtvQkFDNUYsRUFBRSxJQUFJLEVBQUUsT0FBTyxFQUFFLElBQUksRUFBRSxvQkFBb0IsRUFBRSxLQUFLLEVBQUUsQ0FBQyxFQUFFLGNBQWMsRUFBRSxtQkFBbUIsRUFBRTtpQkFDN0Y7YUFDRjtTQUNGLENBQUMsQ0FBQztJQUNMLENBQUM7OztZQXpCRixVQUFVLFNBQUM7Z0JBQ1YsVUFBVSxFQUFFLE1BQU07YUFDbkI7Ozs7WUFMUSxNQUFNO1lBRnFCLFdBQVc7Ozs7Ozs7O0lBU2pDLHVDQUFzQjs7Ozs7SUFBRSw0Q0FBZ0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBhZGRBYnBSb3V0ZXMsIGVMYXlvdXRUeXBlLCBSZXN0U2VydmljZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcbmltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgUm91dGVyIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcclxuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xyXG5cclxuQEluamVjdGFibGUoe1xyXG4gIHByb3ZpZGVkSW46ICdyb290JyxcclxufSlcclxuZXhwb3J0IGNsYXNzIElkZW50aXR5Q29uZmlnU2VydmljZSB7XHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSByb3V0ZXI6IFJvdXRlciwgcHJpdmF0ZSByZXN0U2VydmljZTogUmVzdFNlcnZpY2UpIHtcclxuICAgIGFkZEFicFJvdXRlcyhbXHJcbiAgICAgIHtcclxuICAgICAgICBuYW1lOiAnQWJwVWlOYXZpZ2F0aW9uOjpNZW51OkFkbWluaXN0cmF0aW9uJyxcclxuICAgICAgICBwYXRoOiAnJyxcclxuICAgICAgICBvcmRlcjogMSxcclxuICAgICAgICB3cmFwcGVyOiB0cnVlLFxyXG4gICAgICB9LFxyXG4gICAgICB7XHJcbiAgICAgICAgbmFtZTogJ0FicElkZW50aXR5OjpNZW51OklkZW50aXR5TWFuYWdlbWVudCcsXHJcbiAgICAgICAgcGF0aDogJ2lkZW50aXR5JyxcclxuICAgICAgICBvcmRlcjogMSxcclxuICAgICAgICBwYXJlbnROYW1lOiAnQWJwVWlOYXZpZ2F0aW9uOjpNZW51OkFkbWluaXN0cmF0aW9uJyxcclxuICAgICAgICBsYXlvdXQ6IGVMYXlvdXRUeXBlLmFwcGxpY2F0aW9uLFxyXG4gICAgICAgIGljb25DbGFzczogJ2ZhIGZhLWlkLWNhcmQtbycsXHJcbiAgICAgICAgY2hpbGRyZW46IFtcclxuICAgICAgICAgIHsgcGF0aDogJ3JvbGVzJywgbmFtZTogJ0FicElkZW50aXR5OjpSb2xlcycsIG9yZGVyOiAyLCByZXF1aXJlZFBvbGljeTogJ0FicElkZW50aXR5LlJvbGVzJyB9LFxyXG4gICAgICAgICAgeyBwYXRoOiAndXNlcnMnLCBuYW1lOiAnQWJwSWRlbnRpdHk6OlVzZXJzJywgb3JkZXI6IDEsIHJlcXVpcmVkUG9saWN5OiAnQWJwSWRlbnRpdHkuVXNlcnMnIH0sXHJcbiAgICAgICAgXSxcclxuICAgICAgfSxcclxuICAgIF0pO1xyXG4gIH1cclxufVxyXG4iXX0=