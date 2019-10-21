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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHktY29uZmlnLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LmNvbmZpZy8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9pZGVudGl0eS1jb25maWcuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFlBQVksRUFBZSxXQUFXLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDdEUsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0saUJBQWlCLENBQUM7Ozs7QUFHekM7SUFJRSwrQkFBb0IsTUFBYyxFQUFVLFdBQXdCO1FBQWhELFdBQU0sR0FBTixNQUFNLENBQVE7UUFBVSxnQkFBVyxHQUFYLFdBQVcsQ0FBYTtRQUNsRSxZQUFZLENBQUM7WUFDWDtnQkFDRSxJQUFJLEVBQUUsc0NBQXNDO2dCQUM1QyxJQUFJLEVBQUUsRUFBRTtnQkFDUixLQUFLLEVBQUUsQ0FBQztnQkFDUixPQUFPLEVBQUUsSUFBSTthQUNkO1lBQ0Q7Z0JBQ0UsSUFBSSxFQUFFLHNDQUFzQztnQkFDNUMsSUFBSSxFQUFFLFVBQVU7Z0JBQ2hCLEtBQUssRUFBRSxDQUFDO2dCQUNSLFVBQVUsRUFBRSxzQ0FBc0M7Z0JBQ2xELE1BQU0saUNBQXlCO2dCQUMvQixTQUFTLEVBQUUsaUJBQWlCO2dCQUM1QixRQUFRLEVBQUU7b0JBQ1IsRUFBRSxJQUFJLEVBQUUsT0FBTyxFQUFFLElBQUksRUFBRSxvQkFBb0IsRUFBRSxLQUFLLEVBQUUsQ0FBQyxFQUFFLGNBQWMsRUFBRSxtQkFBbUIsRUFBRTtvQkFDNUYsRUFBRSxJQUFJLEVBQUUsT0FBTyxFQUFFLElBQUksRUFBRSxvQkFBb0IsRUFBRSxLQUFLLEVBQUUsQ0FBQyxFQUFFLGNBQWMsRUFBRSxtQkFBbUIsRUFBRTtpQkFDN0Y7YUFDRjtTQUNGLENBQUMsQ0FBQztJQUNMLENBQUM7O2dCQXpCRixVQUFVLFNBQUM7b0JBQ1YsVUFBVSxFQUFFLE1BQU07aUJBQ25COzs7O2dCQUxRLE1BQU07Z0JBRnFCLFdBQVc7OztnQ0FBL0M7Q0ErQkMsQUExQkQsSUEwQkM7U0F2QlkscUJBQXFCOzs7Ozs7SUFDcEIsdUNBQXNCOzs7OztJQUFFLDRDQUFnQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IGFkZEFicFJvdXRlcywgZUxheW91dFR5cGUsIFJlc3RTZXJ2aWNlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBJZGVudGl0eUNvbmZpZ1NlcnZpY2Uge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJvdXRlcjogUm91dGVyLCBwcml2YXRlIHJlc3RTZXJ2aWNlOiBSZXN0U2VydmljZSkge1xuICAgIGFkZEFicFJvdXRlcyhbXG4gICAgICB7XG4gICAgICAgIG5hbWU6ICdBYnBVaU5hdmlnYXRpb246Ok1lbnU6QWRtaW5pc3RyYXRpb24nLFxuICAgICAgICBwYXRoOiAnJyxcbiAgICAgICAgb3JkZXI6IDEsXG4gICAgICAgIHdyYXBwZXI6IHRydWUsXG4gICAgICB9LFxuICAgICAge1xuICAgICAgICBuYW1lOiAnQWJwSWRlbnRpdHk6Ok1lbnU6SWRlbnRpdHlNYW5hZ2VtZW50JyxcbiAgICAgICAgcGF0aDogJ2lkZW50aXR5JyxcbiAgICAgICAgb3JkZXI6IDEsXG4gICAgICAgIHBhcmVudE5hbWU6ICdBYnBVaU5hdmlnYXRpb246Ok1lbnU6QWRtaW5pc3RyYXRpb24nLFxuICAgICAgICBsYXlvdXQ6IGVMYXlvdXRUeXBlLmFwcGxpY2F0aW9uLFxuICAgICAgICBpY29uQ2xhc3M6ICdmYSBmYS1pZC1jYXJkLW8nLFxuICAgICAgICBjaGlsZHJlbjogW1xuICAgICAgICAgIHsgcGF0aDogJ3JvbGVzJywgbmFtZTogJ0FicElkZW50aXR5OjpSb2xlcycsIG9yZGVyOiAyLCByZXF1aXJlZFBvbGljeTogJ0FicElkZW50aXR5LlJvbGVzJyB9LFxuICAgICAgICAgIHsgcGF0aDogJ3VzZXJzJywgbmFtZTogJ0FicElkZW50aXR5OjpVc2VycycsIG9yZGVyOiAxLCByZXF1aXJlZFBvbGljeTogJ0FicElkZW50aXR5LlVzZXJzJyB9LFxuICAgICAgICBdLFxuICAgICAgfSxcbiAgICBdKTtcbiAgfVxufVxuIl19