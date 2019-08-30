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
     * @param {?=} params
     * @return {?}
     */
    TenantManagementService.prototype.getTenant = /**
     * @param {?=} params
     * @return {?}
     */
    function (params) {
        if (params === void 0) { params = (/** @type {?} */ ({})); }
        /** @type {?} */
        var request = {
            method: 'GET',
            url: '/api/multi-tenancy/tenants',
            params: params,
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} id
     * @return {?}
     */
    TenantManagementService.prototype.getTenantById = /**
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
    TenantManagementService.prototype.deleteTenant = /**
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
    TenantManagementService.prototype.createTenant = /**
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
    TenantManagementService.prototype.updateTenant = /**
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
        var url = "/api/multi-tenancy/tenants/" + id + "/default-connection-string";
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
        var url = "/api/multi-tenancy/tenants/" + payload.id + "/default-connection-string";
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
        var url = "/api/multi-tenancy/tenants/" + id + "/default-connection-string";
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGVuYW50LW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvdGVuYW50LW1hbmFnZW1lbnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzQyxPQUFPLEVBQUUsV0FBVyxFQUFhLE1BQU0sY0FBYyxDQUFDOzs7QUFHdEQ7SUFJRSxpQ0FBb0IsSUFBaUI7UUFBakIsU0FBSSxHQUFKLElBQUksQ0FBYTtJQUFHLENBQUM7Ozs7O0lBRXpDLDJDQUFTOzs7O0lBQVQsVUFBVSxNQUFrQztRQUFsQyx1QkFBQSxFQUFBLDRCQUFTLEVBQUUsRUFBdUI7O1lBQ3BDLE9BQU8sR0FBdUI7WUFDbEMsTUFBTSxFQUFFLEtBQUs7WUFDYixHQUFHLEVBQUUsNEJBQTRCO1lBQ2pDLE1BQU0sUUFBQTtTQUNQO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBa0MsT0FBTyxDQUFDLENBQUM7SUFDckUsQ0FBQzs7Ozs7SUFFRCwrQ0FBYTs7OztJQUFiLFVBQWMsRUFBVTs7WUFDaEIsT0FBTyxHQUF1QjtZQUNsQyxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSxnQ0FBOEIsRUFBSTtTQUN4QztRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQXNCLE9BQU8sQ0FBQyxDQUFDO0lBQ3pELENBQUM7Ozs7O0lBRUQsOENBQVk7Ozs7SUFBWixVQUFhLEVBQVU7O1lBQ2YsT0FBTyxHQUF1QjtZQUNsQyxNQUFNLEVBQUUsUUFBUTtZQUNoQixHQUFHLEVBQUUsZ0NBQThCLEVBQUk7U0FDeEM7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFhLE9BQU8sQ0FBQyxDQUFDO0lBQ2hELENBQUM7Ozs7O0lBRUQsOENBQVk7Ozs7SUFBWixVQUFhLElBQWlDOztZQUN0QyxPQUFPLEdBQThDO1lBQ3pELE1BQU0sRUFBRSxNQUFNO1lBQ2QsR0FBRyxFQUFFLDRCQUE0QjtZQUNqQyxJQUFJLE1BQUE7U0FDTDtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQTZDLE9BQU8sQ0FBQyxDQUFDO0lBQ2hGLENBQUM7Ozs7O0lBRUQsOENBQVk7Ozs7SUFBWixVQUFhLElBQW9DOztZQUN6QyxHQUFHLEdBQUcsZ0NBQThCLElBQUksQ0FBQyxFQUFJO1FBQ25ELE9BQU8sSUFBSSxDQUFDLEVBQUUsQ0FBQzs7WUFFVCxPQUFPLEdBQWlEO1lBQzVELE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRyxLQUFBO1lBQ0gsSUFBSSxNQUFBO1NBQ0w7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUE2QyxPQUFPLENBQUMsQ0FBQztJQUNoRixDQUFDOzs7OztJQUVELDREQUEwQjs7OztJQUExQixVQUEyQixFQUFVOztZQUM3QixHQUFHLEdBQUcsZ0NBQThCLEVBQUUsK0JBQTRCOztZQUVsRSxPQUFPLEdBQWtFO1lBQzdFLE1BQU0sRUFBRSxLQUFLO1lBQ2IsWUFBWSxtQkFBd0I7WUFDcEMsR0FBRyxLQUFBO1NBQ0o7UUFDRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUEwRCxPQUFPLENBQUMsQ0FBQztJQUM3RixDQUFDOzs7OztJQUVELCtEQUE2Qjs7OztJQUE3QixVQUE4QixPQUF3RDs7WUFDOUUsR0FBRyxHQUFHLGdDQUE4QixPQUFPLENBQUMsRUFBRSwrQkFBNEI7O1lBRTFFLE9BQU8sR0FBa0U7WUFDN0UsTUFBTSxFQUFFLEtBQUs7WUFDYixHQUFHLEtBQUE7WUFDSCxNQUFNLEVBQUUsRUFBRSx1QkFBdUIsRUFBRSxPQUFPLENBQUMsdUJBQXVCLEVBQUU7U0FDckU7UUFDRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUF1RCxPQUFPLENBQUMsQ0FBQztJQUMxRixDQUFDOzs7OztJQUVELCtEQUE2Qjs7OztJQUE3QixVQUE4QixFQUFVOztZQUNoQyxHQUFHLEdBQUcsZ0NBQThCLEVBQUUsK0JBQTRCOztZQUVsRSxPQUFPLEdBQWtFO1lBQzdFLE1BQU0sRUFBRSxRQUFRO1lBQ2hCLEdBQUcsS0FBQTtTQUNKO1FBQ0QsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBdUQsT0FBTyxDQUFDLENBQUM7SUFDMUYsQ0FBQzs7Z0JBdkZGLFVBQVUsU0FBQztvQkFDVixVQUFVLEVBQUUsTUFBTTtpQkFDbkI7Ozs7Z0JBTFEsV0FBVzs7O2tDQUZwQjtDQTZGQyxBQXhGRCxJQXdGQztTQXJGWSx1QkFBdUI7Ozs7OztJQUN0Qix1Q0FBeUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBSZXN0U2VydmljZSwgUmVzdCwgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvdGVuYW50LW1hbmFnZW1lbnQnO1xuXG5ASW5qZWN0YWJsZSh7XG4gIHByb3ZpZGVkSW46ICdyb290Jyxcbn0pXG5leHBvcnQgY2xhc3MgVGVuYW50TWFuYWdlbWVudFNlcnZpY2Uge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJlc3Q6IFJlc3RTZXJ2aWNlKSB7fVxuXG4gIGdldFRlbmFudChwYXJhbXMgPSB7fSBhcyBBQlAuUGFnZVF1ZXJ5UGFyYW1zKTogT2JzZXJ2YWJsZTxUZW5hbnRNYW5hZ2VtZW50LlJlc3BvbnNlPiB7XG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PG51bGw+ID0ge1xuICAgICAgbWV0aG9kOiAnR0VUJyxcbiAgICAgIHVybDogJy9hcGkvbXVsdGktdGVuYW5jeS90ZW5hbnRzJyxcbiAgICAgIHBhcmFtcyxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PG51bGwsIFRlbmFudE1hbmFnZW1lbnQuUmVzcG9uc2U+KHJlcXVlc3QpO1xuICB9XG5cbiAgZ2V0VGVuYW50QnlJZChpZDogc3RyaW5nKTogT2JzZXJ2YWJsZTxBQlAuQmFzaWNJdGVtPiB7XG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PG51bGw+ID0ge1xuICAgICAgbWV0aG9kOiAnR0VUJyxcbiAgICAgIHVybDogYC9hcGkvbXVsdGktdGVuYW5jeS90ZW5hbnRzLyR7aWR9YCxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PG51bGwsIEFCUC5CYXNpY0l0ZW0+KHJlcXVlc3QpO1xuICB9XG5cbiAgZGVsZXRlVGVuYW50KGlkOiBzdHJpbmcpOiBPYnNlcnZhYmxlPG51bGw+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8bnVsbD4gPSB7XG4gICAgICBtZXRob2Q6ICdERUxFVEUnLFxuICAgICAgdXJsOiBgL2FwaS9tdWx0aS10ZW5hbmN5L3RlbmFudHMvJHtpZH1gLFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8bnVsbCwgbnVsbD4ocmVxdWVzdCk7XG4gIH1cblxuICBjcmVhdGVUZW5hbnQoYm9keTogVGVuYW50TWFuYWdlbWVudC5BZGRSZXF1ZXN0KTogT2JzZXJ2YWJsZTxBQlAuQmFzaWNJdGVtPiB7XG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PFRlbmFudE1hbmFnZW1lbnQuQWRkUmVxdWVzdD4gPSB7XG4gICAgICBtZXRob2Q6ICdQT1NUJyxcbiAgICAgIHVybDogYC9hcGkvbXVsdGktdGVuYW5jeS90ZW5hbnRzYCxcbiAgICAgIGJvZHksXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxUZW5hbnRNYW5hZ2VtZW50LkFkZFJlcXVlc3QsIEFCUC5CYXNpY0l0ZW0+KHJlcXVlc3QpO1xuICB9XG5cbiAgdXBkYXRlVGVuYW50KGJvZHk6IFRlbmFudE1hbmFnZW1lbnQuVXBkYXRlUmVxdWVzdCk6IE9ic2VydmFibGU8QUJQLkJhc2ljSXRlbT4ge1xuICAgIGNvbnN0IHVybCA9IGAvYXBpL211bHRpLXRlbmFuY3kvdGVuYW50cy8ke2JvZHkuaWR9YDtcbiAgICBkZWxldGUgYm9keS5pZDtcblxuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxUZW5hbnRNYW5hZ2VtZW50LlVwZGF0ZVJlcXVlc3Q+ID0ge1xuICAgICAgbWV0aG9kOiAnUFVUJyxcbiAgICAgIHVybCxcbiAgICAgIGJvZHksXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxUZW5hbnRNYW5hZ2VtZW50LkFkZFJlcXVlc3QsIEFCUC5CYXNpY0l0ZW0+KHJlcXVlc3QpO1xuICB9XG5cbiAgZ2V0RGVmYXVsdENvbm5lY3Rpb25TdHJpbmcoaWQ6IHN0cmluZyk6IE9ic2VydmFibGU8c3RyaW5nPiB7XG4gICAgY29uc3QgdXJsID0gYC9hcGkvbXVsdGktdGVuYW5jeS90ZW5hbnRzLyR7aWR9L2RlZmF1bHQtY29ubmVjdGlvbi1zdHJpbmdgO1xuXG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PFRlbmFudE1hbmFnZW1lbnQuRGVmYXVsdENvbm5lY3Rpb25TdHJpbmdSZXF1ZXN0PiA9IHtcbiAgICAgIG1ldGhvZDogJ0dFVCcsXG4gICAgICByZXNwb25zZVR5cGU6IFJlc3QuUmVzcG9uc2VUeXBlLlRleHQsXG4gICAgICB1cmwsXG4gICAgfTtcbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8VGVuYW50TWFuYWdlbWVudC5EZWZhdWx0Q29ubmVjdGlvblN0cmluZ1JlcXVlc3QsIHN0cmluZz4ocmVxdWVzdCk7XG4gIH1cblxuICB1cGRhdGVEZWZhdWx0Q29ubmVjdGlvblN0cmluZyhwYXlsb2FkOiBUZW5hbnRNYW5hZ2VtZW50LkRlZmF1bHRDb25uZWN0aW9uU3RyaW5nUmVxdWVzdCk6IE9ic2VydmFibGU8YW55PiB7XG4gICAgY29uc3QgdXJsID0gYC9hcGkvbXVsdGktdGVuYW5jeS90ZW5hbnRzLyR7cGF5bG9hZC5pZH0vZGVmYXVsdC1jb25uZWN0aW9uLXN0cmluZ2A7XG5cbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8VGVuYW50TWFuYWdlbWVudC5EZWZhdWx0Q29ubmVjdGlvblN0cmluZ1JlcXVlc3Q+ID0ge1xuICAgICAgbWV0aG9kOiAnUFVUJyxcbiAgICAgIHVybCxcbiAgICAgIHBhcmFtczogeyBkZWZhdWx0Q29ubmVjdGlvblN0cmluZzogcGF5bG9hZC5kZWZhdWx0Q29ubmVjdGlvblN0cmluZyB9LFxuICAgIH07XG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PFRlbmFudE1hbmFnZW1lbnQuRGVmYXVsdENvbm5lY3Rpb25TdHJpbmdSZXF1ZXN0LCBhbnk+KHJlcXVlc3QpO1xuICB9XG5cbiAgZGVsZXRlRGVmYXVsdENvbm5lY3Rpb25TdHJpbmcoaWQ6IHN0cmluZyk6IE9ic2VydmFibGU8c3RyaW5nPiB7XG4gICAgY29uc3QgdXJsID0gYC9hcGkvbXVsdGktdGVuYW5jeS90ZW5hbnRzLyR7aWR9L2RlZmF1bHQtY29ubmVjdGlvbi1zdHJpbmdgO1xuXG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PFRlbmFudE1hbmFnZW1lbnQuRGVmYXVsdENvbm5lY3Rpb25TdHJpbmdSZXF1ZXN0PiA9IHtcbiAgICAgIG1ldGhvZDogJ0RFTEVURScsXG4gICAgICB1cmwsXG4gICAgfTtcbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8VGVuYW50TWFuYWdlbWVudC5EZWZhdWx0Q29ubmVjdGlvblN0cmluZ1JlcXVlc3QsIGFueT4ocmVxdWVzdCk7XG4gIH1cbn1cbiJdfQ==