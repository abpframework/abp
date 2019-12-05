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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQtY29uZmlnLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LmNvbmZpZy8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy90ZW5hbnQtbWFuYWdlbWVudC1jb25maWcuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLFlBQVksRUFBZSxNQUFNLGNBQWMsQ0FBQzs7QUFLekQsTUFBTSxPQUFPLDZCQUE2QjtJQUN4QztRQUNFLFlBQVksQ0FBQztZQUNYLElBQUksRUFBRSw0Q0FBNEM7WUFDbEQsSUFBSSxFQUFFLG1CQUFtQjtZQUN6QixVQUFVLEVBQUUsc0NBQXNDO1lBQ2xELE1BQU0saUNBQXlCO1lBQy9CLFNBQVMsRUFBRSxhQUFhO1lBQ3hCLFFBQVEsRUFBRTtnQkFDUjtvQkFDRSxJQUFJLEVBQUUsU0FBUztvQkFDZixJQUFJLEVBQUUsOEJBQThCO29CQUNwQyxLQUFLLEVBQUUsQ0FBQztvQkFDUixjQUFjLEVBQUUsNkJBQTZCO2lCQUM5QzthQUNGO1NBQ0YsQ0FBQyxDQUFDO0lBQ0wsQ0FBQzs7O1lBcEJGLFVBQVUsU0FBQztnQkFDVixVQUFVLEVBQUUsTUFBTTthQUNuQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IGFkZEFicFJvdXRlcywgZUxheW91dFR5cGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuXG5ASW5qZWN0YWJsZSh7XG4gIHByb3ZpZGVkSW46ICdyb290Jyxcbn0pXG5leHBvcnQgY2xhc3MgVGVuYW50TWFuYWdlbWVudENvbmZpZ1NlcnZpY2Uge1xuICBjb25zdHJ1Y3RvcigpIHtcbiAgICBhZGRBYnBSb3V0ZXMoe1xuICAgICAgbmFtZTogJ0FicFRlbmFudE1hbmFnZW1lbnQ6Ok1lbnU6VGVuYW50TWFuYWdlbWVudCcsXG4gICAgICBwYXRoOiAndGVuYW50LW1hbmFnZW1lbnQnLFxuICAgICAgcGFyZW50TmFtZTogJ0FicFVpTmF2aWdhdGlvbjo6TWVudTpBZG1pbmlzdHJhdGlvbicsXG4gICAgICBsYXlvdXQ6IGVMYXlvdXRUeXBlLmFwcGxpY2F0aW9uLFxuICAgICAgaWNvbkNsYXNzOiAnZmEgZmEtdXNlcnMnLFxuICAgICAgY2hpbGRyZW46IFtcbiAgICAgICAge1xuICAgICAgICAgIHBhdGg6ICd0ZW5hbnRzJyxcbiAgICAgICAgICBuYW1lOiAnQWJwVGVuYW50TWFuYWdlbWVudDo6VGVuYW50cycsXG4gICAgICAgICAgb3JkZXI6IDEsXG4gICAgICAgICAgcmVxdWlyZWRQb2xpY3k6ICdBYnBUZW5hbnRNYW5hZ2VtZW50LlRlbmFudHMnLFxuICAgICAgICB9LFxuICAgICAgXSxcbiAgICB9KTtcbiAgfVxufVxuIl19