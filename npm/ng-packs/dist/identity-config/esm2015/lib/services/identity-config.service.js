/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/identity-config.service.ts
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
                iconClass: 'fa fa-wrench',
            },
            {
                name: 'AbpIdentity::Menu:IdentityManagement',
                path: 'identity',
                order: 1,
                parentName: 'AbpUiNavigation::Menu:Administration',
                layout: "application" /* application */,
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHktY29uZmlnLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LmNvbmZpZy8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9pZGVudGl0eS1jb25maWcuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxZQUFZLEVBQWUsV0FBVyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ3RFLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLE1BQU0sRUFBRSxNQUFNLGlCQUFpQixDQUFDOzs7O0FBTXpDLE1BQU0sT0FBTyxxQkFBcUI7Ozs7O0lBQ2hDLFlBQW9CLE1BQWMsRUFBVSxXQUF3QjtRQUFoRCxXQUFNLEdBQU4sTUFBTSxDQUFRO1FBQVUsZ0JBQVcsR0FBWCxXQUFXLENBQWE7UUFDbEUsWUFBWSxDQUFDO1lBQ1g7Z0JBQ0UsSUFBSSxFQUFFLHNDQUFzQztnQkFDNUMsSUFBSSxFQUFFLEVBQUU7Z0JBQ1IsS0FBSyxFQUFFLENBQUM7Z0JBQ1IsT0FBTyxFQUFFLElBQUk7Z0JBQ2IsU0FBUyxFQUFFLGNBQWM7YUFDMUI7WUFDRDtnQkFDRSxJQUFJLEVBQUUsc0NBQXNDO2dCQUM1QyxJQUFJLEVBQUUsVUFBVTtnQkFDaEIsS0FBSyxFQUFFLENBQUM7Z0JBQ1IsVUFBVSxFQUFFLHNDQUFzQztnQkFDbEQsTUFBTSxpQ0FBeUI7Z0JBQy9CLFNBQVMsRUFBRSxpQkFBaUI7Z0JBQzVCLFFBQVEsRUFBRTtvQkFDUixFQUFFLElBQUksRUFBRSxPQUFPLEVBQUUsSUFBSSxFQUFFLG9CQUFvQixFQUFFLEtBQUssRUFBRSxDQUFDLEVBQUUsY0FBYyxFQUFFLG1CQUFtQixFQUFFO29CQUM1RixFQUFFLElBQUksRUFBRSxPQUFPLEVBQUUsSUFBSSxFQUFFLG9CQUFvQixFQUFFLEtBQUssRUFBRSxDQUFDLEVBQUUsY0FBYyxFQUFFLG1CQUFtQixFQUFFO2lCQUM3RjthQUNGO1NBQ0YsQ0FBQyxDQUFDO0lBQ0wsQ0FBQzs7O1lBMUJGLFVBQVUsU0FBQztnQkFDVixVQUFVLEVBQUUsTUFBTTthQUNuQjs7OztZQUxRLE1BQU07WUFGcUIsV0FBVzs7Ozs7Ozs7SUFTakMsdUNBQXNCOzs7OztJQUFFLDRDQUFnQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IGFkZEFicFJvdXRlcywgZUxheW91dFR5cGUsIFJlc3RTZXJ2aWNlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBJZGVudGl0eUNvbmZpZ1NlcnZpY2Uge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJvdXRlcjogUm91dGVyLCBwcml2YXRlIHJlc3RTZXJ2aWNlOiBSZXN0U2VydmljZSkge1xuICAgIGFkZEFicFJvdXRlcyhbXG4gICAgICB7XG4gICAgICAgIG5hbWU6ICdBYnBVaU5hdmlnYXRpb246Ok1lbnU6QWRtaW5pc3RyYXRpb24nLFxuICAgICAgICBwYXRoOiAnJyxcbiAgICAgICAgb3JkZXI6IDEsXG4gICAgICAgIHdyYXBwZXI6IHRydWUsXG4gICAgICAgIGljb25DbGFzczogJ2ZhIGZhLXdyZW5jaCcsXG4gICAgICB9LFxuICAgICAge1xuICAgICAgICBuYW1lOiAnQWJwSWRlbnRpdHk6Ok1lbnU6SWRlbnRpdHlNYW5hZ2VtZW50JyxcbiAgICAgICAgcGF0aDogJ2lkZW50aXR5JyxcbiAgICAgICAgb3JkZXI6IDEsXG4gICAgICAgIHBhcmVudE5hbWU6ICdBYnBVaU5hdmlnYXRpb246Ok1lbnU6QWRtaW5pc3RyYXRpb24nLFxuICAgICAgICBsYXlvdXQ6IGVMYXlvdXRUeXBlLmFwcGxpY2F0aW9uLFxuICAgICAgICBpY29uQ2xhc3M6ICdmYSBmYS1pZC1jYXJkLW8nLFxuICAgICAgICBjaGlsZHJlbjogW1xuICAgICAgICAgIHsgcGF0aDogJ3JvbGVzJywgbmFtZTogJ0FicElkZW50aXR5OjpSb2xlcycsIG9yZGVyOiAxLCByZXF1aXJlZFBvbGljeTogJ0FicElkZW50aXR5LlJvbGVzJyB9LFxuICAgICAgICAgIHsgcGF0aDogJ3VzZXJzJywgbmFtZTogJ0FicElkZW50aXR5OjpVc2VycycsIG9yZGVyOiAyLCByZXF1aXJlZFBvbGljeTogJ0FicElkZW50aXR5LlVzZXJzJyB9LFxuICAgICAgICBdLFxuICAgICAgfSxcbiAgICBdKTtcbiAgfVxufVxuIl19