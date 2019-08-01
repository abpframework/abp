/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import * as i0 from "@angular/core";
import * as i1 from "@abp/ng.core";
var TenantManagementService = /** @class */ (function () {
    function TenantManagementService(rest) {
        this.rest = rest;
    }
    /**
     * @return {?}
     */
    TenantManagementService.prototype.get = /**
     * @return {?}
     */
    function () {
        /** @type {?} */
        var request = {
            method: 'GET',
            url: '/api/multi-tenancy/tenants',
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} id
     * @return {?}
     */
    TenantManagementService.prototype.getById = /**
     * @param {?} id
     * @return {?}
     */
    function (id) {
        /** @type {?} */
        var request = {
            method: 'GET',
            url: "/api/multi-tenancy/tenants/" + id,
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} id
     * @return {?}
     */
    TenantManagementService.prototype.delete = /**
     * @param {?} id
     * @return {?}
     */
    function (id) {
        /** @type {?} */
        var request = {
            method: 'DELETE',
            url: "/api/multi-tenancy/tenants/" + id,
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} body
     * @return {?}
     */
    TenantManagementService.prototype.add = /**
     * @param {?} body
     * @return {?}
     */
    function (body) {
        /** @type {?} */
        var request = {
            method: 'POST',
            url: "/api/multi-tenancy/tenants",
            body: body,
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} body
     * @return {?}
     */
    TenantManagementService.prototype.update = /**
     * @param {?} body
     * @return {?}
     */
    function (body) {
        /** @type {?} */
        var url = "/api/multi-tenancy/tenants/" + body.id;
        delete body.id;
        /** @type {?} */
        var request = {
            method: 'PUT',
            url: url,
            body: body,
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} id
     * @return {?}
     */
    TenantManagementService.prototype.getDefaultConnectionString = /**
     * @param {?} id
     * @return {?}
     */
    function (id) {
        /** @type {?} */
        var url = "/api/multi-tenancy/tenants/" + id + "/defaultConnectionString";
        /** @type {?} */
        var request = {
            method: 'GET',
            responseType: "text" /* Text */,
            url: url,
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} payload
     * @return {?}
     */
    TenantManagementService.prototype.updateDefaultConnectionString = /**
     * @param {?} payload
     * @return {?}
     */
    function (payload) {
        /** @type {?} */
        var url = "/api/multi-tenancy/tenants/" + payload.id + "/defaultConnectionString";
        /** @type {?} */
        var request = {
            method: 'PUT',
            url: url,
            params: { defaultConnectionString: payload.defaultConnectionString },
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} id
     * @return {?}
     */
    TenantManagementService.prototype.deleteDefaultConnectionString = /**
     * @param {?} id
     * @return {?}
     */
    function (id) {
        /** @type {?} */
        var url = "/api/multi-tenancy/tenant/" + id + "/defaultConnectionString";
        /** @type {?} */
        var request = {
            method: 'DELETE',
            url: url,
        };
        return this.rest.request(request);
    };
    TenantManagementService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    TenantManagementService.ctorParameters = function () { return [
        { type: RestService }
    ]; };
    /** @nocollapse */ TenantManagementService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function TenantManagementService_Factory() { return new TenantManagementService(i0.ɵɵinject(i1.RestService)); }, token: TenantManagementService, providedIn: "root" });
    return TenantManagementService;
}());
export { TenantManagementService };
if (false) {
    /**
     * @type {?}
     * @private
     */
    TenantManagementService.prototype.rest;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGVuYW50LW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvdGVuYW50LW1hbmFnZW1lbnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzQyxPQUFPLEVBQUUsV0FBVyxFQUFhLE1BQU0sY0FBYyxDQUFDOzs7QUFHdEQ7SUFJRSxpQ0FBb0IsSUFBaUI7UUFBakIsU0FBSSxHQUFKLElBQUksQ0FBYTtJQUFHLENBQUM7Ozs7SUFFekMscUNBQUc7OztJQUFIOztZQUNRLE9BQU8sR0FBdUI7WUFDbEMsTUFBTSxFQUFFLEtBQUs7WUFDYixHQUFHLEVBQUUsNEJBQTRCO1NBQ2xDO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBa0MsT0FBTyxDQUFDLENBQUM7SUFDckUsQ0FBQzs7Ozs7SUFFRCx5Q0FBTzs7OztJQUFQLFVBQVEsRUFBVTs7WUFDVixPQUFPLEdBQXVCO1lBQ2xDLE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRyxFQUFFLGdDQUE4QixFQUFJO1NBQ3hDO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBc0IsT0FBTyxDQUFDLENBQUM7SUFDekQsQ0FBQzs7Ozs7SUFFRCx3Q0FBTTs7OztJQUFOLFVBQU8sRUFBVTs7WUFDVCxPQUFPLEdBQXVCO1lBQ2xDLE1BQU0sRUFBRSxRQUFRO1lBQ2hCLEdBQUcsRUFBRSxnQ0FBOEIsRUFBSTtTQUN4QztRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQWEsT0FBTyxDQUFDLENBQUM7SUFDaEQsQ0FBQzs7Ozs7SUFFRCxxQ0FBRzs7OztJQUFILFVBQUksSUFBaUM7O1lBQzdCLE9BQU8sR0FBOEM7WUFDekQsTUFBTSxFQUFFLE1BQU07WUFDZCxHQUFHLEVBQUUsNEJBQTRCO1lBQ2pDLElBQUksTUFBQTtTQUNMO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBNkMsT0FBTyxDQUFDLENBQUM7SUFDaEYsQ0FBQzs7Ozs7SUFFRCx3Q0FBTTs7OztJQUFOLFVBQU8sSUFBb0M7O1lBQ25DLEdBQUcsR0FBRyxnQ0FBOEIsSUFBSSxDQUFDLEVBQUk7UUFDbkQsT0FBTyxJQUFJLENBQUMsRUFBRSxDQUFDOztZQUVULE9BQU8sR0FBaUQ7WUFDNUQsTUFBTSxFQUFFLEtBQUs7WUFDYixHQUFHLEtBQUE7WUFDSCxJQUFJLE1BQUE7U0FDTDtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQTZDLE9BQU8sQ0FBQyxDQUFDO0lBQ2hGLENBQUM7Ozs7O0lBRUQsNERBQTBCOzs7O0lBQTFCLFVBQTJCLEVBQVU7O1lBQzdCLEdBQUcsR0FBRyxnQ0FBOEIsRUFBRSw2QkFBMEI7O1lBRWhFLE9BQU8sR0FBa0U7WUFDN0UsTUFBTSxFQUFFLEtBQUs7WUFDYixZQUFZLG1CQUF3QjtZQUNwQyxHQUFHLEtBQUE7U0FDSjtRQUNELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQTBELE9BQU8sQ0FBQyxDQUFDO0lBQzdGLENBQUM7Ozs7O0lBRUQsK0RBQTZCOzs7O0lBQTdCLFVBQThCLE9BQXdEOztZQUM5RSxHQUFHLEdBQUcsZ0NBQThCLE9BQU8sQ0FBQyxFQUFFLDZCQUEwQjs7WUFFeEUsT0FBTyxHQUFrRTtZQUM3RSxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsS0FBQTtZQUNILE1BQU0sRUFBRSxFQUFFLHVCQUF1QixFQUFFLE9BQU8sQ0FBQyx1QkFBdUIsRUFBRTtTQUNyRTtRQUNELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQXVELE9BQU8sQ0FBQyxDQUFDO0lBQzFGLENBQUM7Ozs7O0lBRUQsK0RBQTZCOzs7O0lBQTdCLFVBQThCLEVBQVU7O1lBQ2hDLEdBQUcsR0FBRywrQkFBNkIsRUFBRSw2QkFBMEI7O1lBRS9ELE9BQU8sR0FBa0U7WUFDN0UsTUFBTSxFQUFFLFFBQVE7WUFDaEIsR0FBRyxLQUFBO1NBQ0o7UUFDRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUF1RCxPQUFPLENBQUMsQ0FBQztJQUMxRixDQUFDOztnQkF0RkYsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7OztnQkFMUSxXQUFXOzs7a0NBRnBCO0NBNEZDLEFBdkZELElBdUZDO1NBcEZZLHVCQUF1Qjs7Ozs7O0lBQ3RCLHVDQUF5QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IFJlc3RTZXJ2aWNlLCBSZXN0LCBBQlAgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgVGVuYW50TWFuYWdlbWVudCB9IGZyb20gJy4uL21vZGVscy90ZW5hbnQtbWFuYWdlbWVudCc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBUZW5hbnRNYW5hZ2VtZW50U2VydmljZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcmVzdDogUmVzdFNlcnZpY2UpIHt9XG5cbiAgZ2V0KCk6IE9ic2VydmFibGU8VGVuYW50TWFuYWdlbWVudC5SZXNwb25zZT4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxudWxsPiA9IHtcbiAgICAgIG1ldGhvZDogJ0dFVCcsXG4gICAgICB1cmw6ICcvYXBpL211bHRpLXRlbmFuY3kvdGVuYW50cycsXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxudWxsLCBUZW5hbnRNYW5hZ2VtZW50LlJlc3BvbnNlPihyZXF1ZXN0KTtcbiAgfVxuXG4gIGdldEJ5SWQoaWQ6IHN0cmluZyk6IE9ic2VydmFibGU8QUJQLkJhc2ljSXRlbT4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxudWxsPiA9IHtcbiAgICAgIG1ldGhvZDogJ0dFVCcsXG4gICAgICB1cmw6IGAvYXBpL211bHRpLXRlbmFuY3kvdGVuYW50cy8ke2lkfWAsXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxudWxsLCBBQlAuQmFzaWNJdGVtPihyZXF1ZXN0KTtcbiAgfVxuXG4gIGRlbGV0ZShpZDogc3RyaW5nKTogT2JzZXJ2YWJsZTxudWxsPiB7XG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PG51bGw+ID0ge1xuICAgICAgbWV0aG9kOiAnREVMRVRFJyxcbiAgICAgIHVybDogYC9hcGkvbXVsdGktdGVuYW5jeS90ZW5hbnRzLyR7aWR9YCxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PG51bGwsIG51bGw+KHJlcXVlc3QpO1xuICB9XG5cbiAgYWRkKGJvZHk6IFRlbmFudE1hbmFnZW1lbnQuQWRkUmVxdWVzdCk6IE9ic2VydmFibGU8QUJQLkJhc2ljSXRlbT4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxUZW5hbnRNYW5hZ2VtZW50LkFkZFJlcXVlc3Q+ID0ge1xuICAgICAgbWV0aG9kOiAnUE9TVCcsXG4gICAgICB1cmw6IGAvYXBpL211bHRpLXRlbmFuY3kvdGVuYW50c2AsXG4gICAgICBib2R5LFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8VGVuYW50TWFuYWdlbWVudC5BZGRSZXF1ZXN0LCBBQlAuQmFzaWNJdGVtPihyZXF1ZXN0KTtcbiAgfVxuXG4gIHVwZGF0ZShib2R5OiBUZW5hbnRNYW5hZ2VtZW50LlVwZGF0ZVJlcXVlc3QpOiBPYnNlcnZhYmxlPEFCUC5CYXNpY0l0ZW0+IHtcbiAgICBjb25zdCB1cmwgPSBgL2FwaS9tdWx0aS10ZW5hbmN5L3RlbmFudHMvJHtib2R5LmlkfWA7XG4gICAgZGVsZXRlIGJvZHkuaWQ7XG5cbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8VGVuYW50TWFuYWdlbWVudC5VcGRhdGVSZXF1ZXN0PiA9IHtcbiAgICAgIG1ldGhvZDogJ1BVVCcsXG4gICAgICB1cmwsXG4gICAgICBib2R5LFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8VGVuYW50TWFuYWdlbWVudC5BZGRSZXF1ZXN0LCBBQlAuQmFzaWNJdGVtPihyZXF1ZXN0KTtcbiAgfVxuXG4gIGdldERlZmF1bHRDb25uZWN0aW9uU3RyaW5nKGlkOiBzdHJpbmcpOiBPYnNlcnZhYmxlPHN0cmluZz4ge1xuICAgIGNvbnN0IHVybCA9IGAvYXBpL211bHRpLXRlbmFuY3kvdGVuYW50cy8ke2lkfS9kZWZhdWx0Q29ubmVjdGlvblN0cmluZ2A7XG5cbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8VGVuYW50TWFuYWdlbWVudC5EZWZhdWx0Q29ubmVjdGlvblN0cmluZ1JlcXVlc3Q+ID0ge1xuICAgICAgbWV0aG9kOiAnR0VUJyxcbiAgICAgIHJlc3BvbnNlVHlwZTogUmVzdC5SZXNwb25zZVR5cGUuVGV4dCxcbiAgICAgIHVybCxcbiAgICB9O1xuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxUZW5hbnRNYW5hZ2VtZW50LkRlZmF1bHRDb25uZWN0aW9uU3RyaW5nUmVxdWVzdCwgc3RyaW5nPihyZXF1ZXN0KTtcbiAgfVxuXG4gIHVwZGF0ZURlZmF1bHRDb25uZWN0aW9uU3RyaW5nKHBheWxvYWQ6IFRlbmFudE1hbmFnZW1lbnQuRGVmYXVsdENvbm5lY3Rpb25TdHJpbmdSZXF1ZXN0KTogT2JzZXJ2YWJsZTxhbnk+IHtcbiAgICBjb25zdCB1cmwgPSBgL2FwaS9tdWx0aS10ZW5hbmN5L3RlbmFudHMvJHtwYXlsb2FkLmlkfS9kZWZhdWx0Q29ubmVjdGlvblN0cmluZ2A7XG5cbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8VGVuYW50TWFuYWdlbWVudC5EZWZhdWx0Q29ubmVjdGlvblN0cmluZ1JlcXVlc3Q+ID0ge1xuICAgICAgbWV0aG9kOiAnUFVUJyxcbiAgICAgIHVybCxcbiAgICAgIHBhcmFtczogeyBkZWZhdWx0Q29ubmVjdGlvblN0cmluZzogcGF5bG9hZC5kZWZhdWx0Q29ubmVjdGlvblN0cmluZyB9LFxuICAgIH07XG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PFRlbmFudE1hbmFnZW1lbnQuRGVmYXVsdENvbm5lY3Rpb25TdHJpbmdSZXF1ZXN0LCBhbnk+KHJlcXVlc3QpO1xuICB9XG5cbiAgZGVsZXRlRGVmYXVsdENvbm5lY3Rpb25TdHJpbmcoaWQ6IHN0cmluZyk6IE9ic2VydmFibGU8c3RyaW5nPiB7XG4gICAgY29uc3QgdXJsID0gYC9hcGkvbXVsdGktdGVuYW5jeS90ZW5hbnQvJHtpZH0vZGVmYXVsdENvbm5lY3Rpb25TdHJpbmdgO1xuXG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PFRlbmFudE1hbmFnZW1lbnQuRGVmYXVsdENvbm5lY3Rpb25TdHJpbmdSZXF1ZXN0PiA9IHtcbiAgICAgIG1ldGhvZDogJ0RFTEVURScsXG4gICAgICB1cmwsXG4gICAgfTtcbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8VGVuYW50TWFuYWdlbWVudC5EZWZhdWx0Q29ubmVjdGlvblN0cmluZ1JlcXVlc3QsIGFueT4ocmVxdWVzdCk7XG4gIH1cbn1cbiJdfQ==