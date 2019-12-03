/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { addAbpRoutes } from '@abp/ng.core';
import * as i0 from "@angular/core";
var TenantManagementConfigService = /** @class */ (function () {
    function TenantManagementConfigService() {
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
    TenantManagementConfigService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    TenantManagementConfigService.ctorParameters = function () { return []; };
    /** @nocollapse */ TenantManagementConfigService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function TenantManagementConfigService_Factory() { return new TenantManagementConfigService(); }, token: TenantManagementConfigService, providedIn: "root" });
    return TenantManagementConfigService;
}());
export { TenantManagementConfigService };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQtY29uZmlnLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LmNvbmZpZy8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy90ZW5hbnQtbWFuYWdlbWVudC1jb25maWcuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsWUFBWSxFQUFlLE1BQU0sY0FBYyxDQUFDOztBQUV6RDtJQUlFO1FBQ0UsWUFBWSxDQUFDO1lBQ1gsSUFBSSxFQUFFLDRDQUE0QztZQUNsRCxJQUFJLEVBQUUsbUJBQW1CO1lBQ3pCLFVBQVUsRUFBRSxzQ0FBc0M7WUFDbEQsTUFBTSxpQ0FBeUI7WUFDL0IsU0FBUyxFQUFFLGFBQWE7WUFDeEIsUUFBUSxFQUFFO2dCQUNSO29CQUNFLElBQUksRUFBRSxTQUFTO29CQUNmLElBQUksRUFBRSw4QkFBOEI7b0JBQ3BDLEtBQUssRUFBRSxDQUFDO29CQUNSLGNBQWMsRUFBRSw2QkFBNkI7aUJBQzlDO2FBQ0Y7U0FDRixDQUFDLENBQUM7SUFDTCxDQUFDOztnQkFwQkYsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7Ozs7d0NBTEQ7Q0F3QkMsQUFyQkQsSUFxQkM7U0FsQlksNkJBQTZCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgYWRkQWJwUm91dGVzLCBlTGF5b3V0VHlwZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBUZW5hbnRNYW5hZ2VtZW50Q29uZmlnU2VydmljZSB7XG4gIGNvbnN0cnVjdG9yKCkge1xuICAgIGFkZEFicFJvdXRlcyh7XG4gICAgICBuYW1lOiAnQWJwVGVuYW50TWFuYWdlbWVudDo6TWVudTpUZW5hbnRNYW5hZ2VtZW50JyxcbiAgICAgIHBhdGg6ICd0ZW5hbnQtbWFuYWdlbWVudCcsXG4gICAgICBwYXJlbnROYW1lOiAnQWJwVWlOYXZpZ2F0aW9uOjpNZW51OkFkbWluaXN0cmF0aW9uJyxcbiAgICAgIGxheW91dDogZUxheW91dFR5cGUuYXBwbGljYXRpb24sXG4gICAgICBpY29uQ2xhc3M6ICdmYSBmYS11c2VycycsXG4gICAgICBjaGlsZHJlbjogW1xuICAgICAgICB7XG4gICAgICAgICAgcGF0aDogJ3RlbmFudHMnLFxuICAgICAgICAgIG5hbWU6ICdBYnBUZW5hbnRNYW5hZ2VtZW50OjpUZW5hbnRzJyxcbiAgICAgICAgICBvcmRlcjogMSxcbiAgICAgICAgICByZXF1aXJlZFBvbGljeTogJ0FicFRlbmFudE1hbmFnZW1lbnQuVGVuYW50cycsXG4gICAgICAgIH0sXG4gICAgICBdLFxuICAgIH0pO1xuICB9XG59XG4iXX0=