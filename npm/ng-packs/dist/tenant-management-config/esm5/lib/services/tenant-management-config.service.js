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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQtY29uZmlnLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LmNvbmZpZy8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy90ZW5hbnQtbWFuYWdlbWVudC1jb25maWcuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsWUFBWSxFQUFlLE1BQU0sY0FBYyxDQUFDOztBQUV6RDtJQUlFO1FBQ0UsWUFBWSxDQUFDO1lBQ1gsSUFBSSxFQUFFLDRDQUE0QztZQUNsRCxJQUFJLEVBQUUsbUJBQW1CO1lBQ3pCLFVBQVUsRUFBRSxzQ0FBc0M7WUFDbEQsTUFBTSxpQ0FBeUI7WUFDL0IsU0FBUyxFQUFFLGFBQWE7WUFDeEIsUUFBUSxFQUFFO2dCQUNSO29CQUNFLElBQUksRUFBRSxTQUFTO29CQUNmLElBQUksRUFBRSw4QkFBOEI7b0JBQ3BDLEtBQUssRUFBRSxDQUFDO29CQUNSLGNBQWMsRUFBRSw2QkFBNkI7aUJBQzlDO2FBQ0Y7U0FDRixDQUFDLENBQUM7SUFDTCxDQUFDOztnQkFwQkYsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7Ozs7d0NBTEQ7Q0F3QkMsQUFyQkQsSUFxQkM7U0FsQlksNkJBQTZCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBhZGRBYnBSb3V0ZXMsIGVMYXlvdXRUeXBlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuXHJcbkBJbmplY3RhYmxlKHtcclxuICBwcm92aWRlZEluOiAncm9vdCcsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBUZW5hbnRNYW5hZ2VtZW50Q29uZmlnU2VydmljZSB7XHJcbiAgY29uc3RydWN0b3IoKSB7XHJcbiAgICBhZGRBYnBSb3V0ZXMoe1xyXG4gICAgICBuYW1lOiAnQWJwVGVuYW50TWFuYWdlbWVudDo6TWVudTpUZW5hbnRNYW5hZ2VtZW50JyxcclxuICAgICAgcGF0aDogJ3RlbmFudC1tYW5hZ2VtZW50JyxcclxuICAgICAgcGFyZW50TmFtZTogJ0FicFVpTmF2aWdhdGlvbjo6TWVudTpBZG1pbmlzdHJhdGlvbicsXHJcbiAgICAgIGxheW91dDogZUxheW91dFR5cGUuYXBwbGljYXRpb24sXHJcbiAgICAgIGljb25DbGFzczogJ2ZhIGZhLXVzZXJzJyxcclxuICAgICAgY2hpbGRyZW46IFtcclxuICAgICAgICB7XHJcbiAgICAgICAgICBwYXRoOiAndGVuYW50cycsXHJcbiAgICAgICAgICBuYW1lOiAnQWJwVGVuYW50TWFuYWdlbWVudDo6VGVuYW50cycsXHJcbiAgICAgICAgICBvcmRlcjogMSxcclxuICAgICAgICAgIHJlcXVpcmVkUG9saWN5OiAnQWJwVGVuYW50TWFuYWdlbWVudC5UZW5hbnRzJyxcclxuICAgICAgICB9LFxyXG4gICAgICBdLFxyXG4gICAgfSk7XHJcbiAgfVxyXG59XHJcbiJdfQ==