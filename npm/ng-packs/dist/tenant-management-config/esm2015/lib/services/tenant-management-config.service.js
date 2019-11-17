/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/tenant-management-config.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { addAbpRoutes } from '@abp/ng.core';
import * as i0 from "@angular/core";
export class TenantManagementConfigService {
    constructor() {
        addAbpRoutes({
            name: 'AbpTenantManagement::Menu:TenantManagement',
            path: 'tenant-management',
            parentName: 'AbpUiNavigation::Menu:Administration',
            layout: "application" /* application */,
            iconClass: 'fa fa-users',
            children: [
                {
                    path: 'tenants',
                    name: 'AbpTenantManagement::Tenants',
                    order: 1,
                    requiredPolicy: 'AbpTenantManagement.Tenants',
                },
            ],
        });
    }
}
TenantManagementConfigService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
TenantManagementConfigService.ctorParameters = () => [];
/** @nocollapse */ TenantManagementConfigService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function TenantManagementConfigService_Factory() { return new TenantManagementConfigService(); }, token: TenantManagementConfigService, providedIn: "root" });
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQtY29uZmlnLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LmNvbmZpZy8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy90ZW5hbnQtbWFuYWdlbWVudC1jb25maWcuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLFlBQVksRUFBZSxNQUFNLGNBQWMsQ0FBQzs7QUFLekQsTUFBTSxPQUFPLDZCQUE2QjtJQUN4QztRQUNFLFlBQVksQ0FBQztZQUNYLElBQUksRUFBRSw0Q0FBNEM7WUFDbEQsSUFBSSxFQUFFLG1CQUFtQjtZQUN6QixVQUFVLEVBQUUsc0NBQXNDO1lBQ2xELE1BQU0saUNBQXlCO1lBQy9CLFNBQVMsRUFBRSxhQUFhO1lBQ3hCLFFBQVEsRUFBRTtnQkFDUjtvQkFDRSxJQUFJLEVBQUUsU0FBUztvQkFDZixJQUFJLEVBQUUsOEJBQThCO29CQUNwQyxLQUFLLEVBQUUsQ0FBQztvQkFDUixjQUFjLEVBQUUsNkJBQTZCO2lCQUM5QzthQUNGO1NBQ0YsQ0FBQyxDQUFDO0lBQ0wsQ0FBQzs7O1lBcEJGLFVBQVUsU0FBQztnQkFDVixVQUFVLEVBQUUsTUFBTTthQUNuQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgYWRkQWJwUm91dGVzLCBlTGF5b3V0VHlwZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcblxyXG5ASW5qZWN0YWJsZSh7XHJcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgVGVuYW50TWFuYWdlbWVudENvbmZpZ1NlcnZpY2Uge1xyXG4gIGNvbnN0cnVjdG9yKCkge1xyXG4gICAgYWRkQWJwUm91dGVzKHtcclxuICAgICAgbmFtZTogJ0FicFRlbmFudE1hbmFnZW1lbnQ6Ok1lbnU6VGVuYW50TWFuYWdlbWVudCcsXHJcbiAgICAgIHBhdGg6ICd0ZW5hbnQtbWFuYWdlbWVudCcsXHJcbiAgICAgIHBhcmVudE5hbWU6ICdBYnBVaU5hdmlnYXRpb246Ok1lbnU6QWRtaW5pc3RyYXRpb24nLFxyXG4gICAgICBsYXlvdXQ6IGVMYXlvdXRUeXBlLmFwcGxpY2F0aW9uLFxyXG4gICAgICBpY29uQ2xhc3M6ICdmYSBmYS11c2VycycsXHJcbiAgICAgIGNoaWxkcmVuOiBbXHJcbiAgICAgICAge1xyXG4gICAgICAgICAgcGF0aDogJ3RlbmFudHMnLFxyXG4gICAgICAgICAgbmFtZTogJ0FicFRlbmFudE1hbmFnZW1lbnQ6OlRlbmFudHMnLFxyXG4gICAgICAgICAgb3JkZXI6IDEsXHJcbiAgICAgICAgICByZXF1aXJlZFBvbGljeTogJ0FicFRlbmFudE1hbmFnZW1lbnQuVGVuYW50cycsXHJcbiAgICAgICAgfSxcclxuICAgICAgXSxcclxuICAgIH0pO1xyXG4gIH1cclxufVxyXG4iXX0=