/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/application-configuration.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { RestService } from './rest.service';
import * as i0 from "@angular/core";
import * as i1 from "./rest.service";
var ApplicationConfigurationService = /** @class */ (function () {
    function ApplicationConfigurationService(rest) {
        this.rest = rest;
    }
    /**
     * @return {?}
     */
    ApplicationConfigurationService.prototype.getConfiguration = /**
     * @return {?}
     */
    function () {
        /** @type {?} */
        var request = {
            method: 'GET',
            url: '/api/abp/application-configuration',
        };
        return this.rest.request(request);
    };
    ApplicationConfigurationService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    ApplicationConfigurationService.ctorParameters = function () { return [
        { type: RestService }
    ]; };
    /** @nocollapse */ ApplicationConfigurationService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ApplicationConfigurationService_Factory() { return new ApplicationConfigurationService(i0.ɵɵinject(i1.RestService)); }, token: ApplicationConfigurationService, providedIn: "root" });
    return ApplicationConfigurationService;
}());
export { ApplicationConfigurationService };
if (false) {
    /**
     * @type {?}
     * @private
     */
    ApplicationConfigurationService.prototype.rest;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBwbGljYXRpb24tY29uZmlndXJhdGlvbi5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL2FwcGxpY2F0aW9uLWNvbmZpZ3VyYXRpb24uc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFJM0MsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLGdCQUFnQixDQUFDOzs7QUFFN0M7SUFJRSx5Q0FBb0IsSUFBaUI7UUFBakIsU0FBSSxHQUFKLElBQUksQ0FBYTtJQUFHLENBQUM7Ozs7SUFFekMsMERBQWdCOzs7SUFBaEI7O1lBQ1EsT0FBTyxHQUF1QjtZQUNsQyxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSxvQ0FBb0M7U0FDMUM7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUEwQyxPQUFPLENBQUMsQ0FBQztJQUM3RSxDQUFDOztnQkFiRixVQUFVLFNBQUM7b0JBQ1YsVUFBVSxFQUFFLE1BQU07aUJBQ25COzs7O2dCQUpRLFdBQVc7OzswQ0FKcEI7Q0FvQkMsQUFkRCxJQWNDO1NBWFksK0JBQStCOzs7Ozs7SUFDOUIsK0NBQXlCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IFJlc3QgfSBmcm9tICcuLi9tb2RlbHMvcmVzdCc7XHJcbmltcG9ydCB7IEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbiB9IGZyb20gJy4uL21vZGVscy9hcHBsaWNhdGlvbi1jb25maWd1cmF0aW9uJztcclxuaW1wb3J0IHsgUmVzdFNlcnZpY2UgfSBmcm9tICcuL3Jlc3Quc2VydmljZSc7XHJcblxyXG5ASW5qZWN0YWJsZSh7XHJcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgQXBwbGljYXRpb25Db25maWd1cmF0aW9uU2VydmljZSB7XHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSByZXN0OiBSZXN0U2VydmljZSkge31cclxuXHJcbiAgZ2V0Q29uZmlndXJhdGlvbigpOiBPYnNlcnZhYmxlPEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbi5SZXNwb25zZT4ge1xyXG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PG51bGw+ID0ge1xyXG4gICAgICBtZXRob2Q6ICdHRVQnLFxyXG4gICAgICB1cmw6ICcvYXBpL2FicC9hcHBsaWNhdGlvbi1jb25maWd1cmF0aW9uJyxcclxuICAgIH07XHJcblxyXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PG51bGwsIEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbi5SZXNwb25zZT4ocmVxdWVzdCk7XHJcbiAgfVxyXG59XHJcbiJdfQ==