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
var IdentityConfigService = /** @class */ (function () {
    function IdentityConfigService(router, restService) {
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
    IdentityConfigService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    IdentityConfigService.ctorParameters = function () { return [
        { type: Router },
        { type: RestService }
    ]; };
    /** @nocollapse */ IdentityConfigService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function IdentityConfigService_Factory() { return new IdentityConfigService(i0.ɵɵinject(i1.Router), i0.ɵɵinject(i2.RestService)); }, token: IdentityConfigService, providedIn: "root" });
    return IdentityConfigService;
}());
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHktY29uZmlnLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LmNvbmZpZy8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9pZGVudGl0eS1jb25maWcuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFlBQVksRUFBZSxXQUFXLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDdEUsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0saUJBQWlCLENBQUM7Ozs7QUFHekM7SUFJRSwrQkFBb0IsTUFBYyxFQUFVLFdBQXdCO1FBQWhELFdBQU0sR0FBTixNQUFNLENBQVE7UUFBVSxnQkFBVyxHQUFYLFdBQVcsQ0FBYTtRQUNsRSxZQUFZLENBQUM7WUFDWDtnQkFDRSxJQUFJLEVBQUUsc0NBQXNDO2dCQUM1QyxJQUFJLEVBQUUsRUFBRTtnQkFDUixLQUFLLEVBQUUsQ0FBQztnQkFDUixPQUFPLEVBQUUsSUFBSTthQUNkO1lBQ0Q7Z0JBQ0UsSUFBSSxFQUFFLHNDQUFzQztnQkFDNUMsSUFBSSxFQUFFLFVBQVU7Z0JBQ2hCLEtBQUssRUFBRSxDQUFDO2dCQUNSLFVBQVUsRUFBRSxzQ0FBc0M7Z0JBQ2xELE1BQU0saUNBQXlCO2dCQUMvQixTQUFTLEVBQUUsaUJBQWlCO2dCQUM1QixRQUFRLEVBQUU7b0JBQ1IsRUFBRSxJQUFJLEVBQUUsT0FBTyxFQUFFLElBQUksRUFBRSxvQkFBb0IsRUFBRSxLQUFLLEVBQUUsQ0FBQyxFQUFFLGNBQWMsRUFBRSxtQkFBbUIsRUFBRTtvQkFDNUYsRUFBRSxJQUFJLEVBQUUsT0FBTyxFQUFFLElBQUksRUFBRSxvQkFBb0IsRUFBRSxLQUFLLEVBQUUsQ0FBQyxFQUFFLGNBQWMsRUFBRSxtQkFBbUIsRUFBRTtpQkFDN0Y7YUFDRjtTQUNGLENBQUMsQ0FBQztJQUNMLENBQUM7O2dCQXpCRixVQUFVLFNBQUM7b0JBQ1YsVUFBVSxFQUFFLE1BQU07aUJBQ25COzs7O2dCQUxRLE1BQU07Z0JBRnFCLFdBQVc7OztnQ0FBL0M7Q0ErQkMsQUExQkQsSUEwQkM7U0F2QlkscUJBQXFCOzs7Ozs7SUFDcEIsdUNBQXNCOzs7OztJQUFFLDRDQUFnQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IGFkZEFicFJvdXRlcywgZUxheW91dFR5cGUsIFJlc3RTZXJ2aWNlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBSb3V0ZXIgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xyXG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XHJcblxyXG5ASW5qZWN0YWJsZSh7XHJcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgSWRlbnRpdHlDb25maWdTZXJ2aWNlIHtcclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJvdXRlcjogUm91dGVyLCBwcml2YXRlIHJlc3RTZXJ2aWNlOiBSZXN0U2VydmljZSkge1xyXG4gICAgYWRkQWJwUm91dGVzKFtcclxuICAgICAge1xyXG4gICAgICAgIG5hbWU6ICdBYnBVaU5hdmlnYXRpb246Ok1lbnU6QWRtaW5pc3RyYXRpb24nLFxyXG4gICAgICAgIHBhdGg6ICcnLFxyXG4gICAgICAgIG9yZGVyOiAxLFxyXG4gICAgICAgIHdyYXBwZXI6IHRydWUsXHJcbiAgICAgIH0sXHJcbiAgICAgIHtcclxuICAgICAgICBuYW1lOiAnQWJwSWRlbnRpdHk6Ok1lbnU6SWRlbnRpdHlNYW5hZ2VtZW50JyxcclxuICAgICAgICBwYXRoOiAnaWRlbnRpdHknLFxyXG4gICAgICAgIG9yZGVyOiAxLFxyXG4gICAgICAgIHBhcmVudE5hbWU6ICdBYnBVaU5hdmlnYXRpb246Ok1lbnU6QWRtaW5pc3RyYXRpb24nLFxyXG4gICAgICAgIGxheW91dDogZUxheW91dFR5cGUuYXBwbGljYXRpb24sXHJcbiAgICAgICAgaWNvbkNsYXNzOiAnZmEgZmEtaWQtY2FyZC1vJyxcclxuICAgICAgICBjaGlsZHJlbjogW1xyXG4gICAgICAgICAgeyBwYXRoOiAncm9sZXMnLCBuYW1lOiAnQWJwSWRlbnRpdHk6OlJvbGVzJywgb3JkZXI6IDIsIHJlcXVpcmVkUG9saWN5OiAnQWJwSWRlbnRpdHkuUm9sZXMnIH0sXHJcbiAgICAgICAgICB7IHBhdGg6ICd1c2VycycsIG5hbWU6ICdBYnBJZGVudGl0eTo6VXNlcnMnLCBvcmRlcjogMSwgcmVxdWlyZWRQb2xpY3k6ICdBYnBJZGVudGl0eS5Vc2VycycgfSxcclxuICAgICAgICBdLFxyXG4gICAgICB9LFxyXG4gICAgXSk7XHJcbiAgfVxyXG59XHJcbiJdfQ==