/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/identity.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import * as i0 from "@angular/core";
import * as i1 from "@abp/ng.core";
var IdentityService = /** @class */ (function () {
    function IdentityService(rest) {
        this.rest = rest;
    }
    /**
     * @param {?=} params
     * @return {?}
     */
    IdentityService.prototype.getRoles = /**
     * @param {?=} params
     * @return {?}
     */
    function (params) {
        if (params === void 0) { params = (/** @type {?} */ ({})); }
        /** @type {?} */
        var request = {
            method: 'GET',
            url: '/api/identity/roles',
            params: params,
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} id
     * @return {?}
     */
    IdentityService.prototype.getRoleById = /**
     * @param {?} id
     * @return {?}
     */
    function (id) {
        /** @type {?} */
        var request = {
            method: 'GET',
            url: "/api/identity/roles/" + id,
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} id
     * @return {?}
     */
    IdentityService.prototype.deleteRole = /**
     * @param {?} id
     * @return {?}
     */
    function (id) {
        /** @type {?} */
        var request = {
            method: 'DELETE',
            url: "/api/identity/roles/" + id,
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} body
     * @return {?}
     */
    IdentityService.prototype.createRole = /**
     * @param {?} body
     * @return {?}
     */
    function (body) {
        /** @type {?} */
        var request = {
            method: 'POST',
            url: '/api/identity/roles',
            body: body,
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} body
     * @return {?}
     */
    IdentityService.prototype.updateRole = /**
     * @param {?} body
     * @return {?}
     */
    function (body) {
        /** @type {?} */
        var url = "/api/identity/roles/" + body.id;
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
     * @param {?=} params
     * @return {?}
     */
    IdentityService.prototype.getUsers = /**
     * @param {?=} params
     * @return {?}
     */
    function (params) {
        if (params === void 0) { params = (/** @type {?} */ ({})); }
        /** @type {?} */
        var request = {
            method: 'GET',
            url: '/api/identity/users',
            params: params,
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} id
     * @return {?}
     */
    IdentityService.prototype.getUserById = /**
     * @param {?} id
     * @return {?}
     */
    function (id) {
        /** @type {?} */
        var request = {
            method: 'GET',
            url: "/api/identity/users/" + id,
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} id
     * @return {?}
     */
    IdentityService.prototype.getUserRoles = /**
     * @param {?} id
     * @return {?}
     */
    function (id) {
        /** @type {?} */
        var request = {
            method: 'GET',
            url: "/api/identity/users/" + id + "/roles",
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} id
     * @return {?}
     */
    IdentityService.prototype.deleteUser = /**
     * @param {?} id
     * @return {?}
     */
    function (id) {
        /** @type {?} */
        var request = {
            method: 'DELETE',
            url: "/api/identity/users/" + id,
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} body
     * @return {?}
     */
    IdentityService.prototype.createUser = /**
     * @param {?} body
     * @return {?}
     */
    function (body) {
        /** @type {?} */
        var request = {
            method: 'POST',
            url: '/api/identity/users',
            body: body,
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} body
     * @return {?}
     */
    IdentityService.prototype.updateUser = /**
     * @param {?} body
     * @return {?}
     */
    function (body) {
        /** @type {?} */
        var url = "/api/identity/users/" + body.id;
        delete body.id;
        /** @type {?} */
        var request = {
            method: 'PUT',
            url: url,
            body: body,
        };
        return this.rest.request(request);
    };
    IdentityService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    IdentityService.ctorParameters = function () { return [
        { type: RestService }
    ]; };
    /** @nocollapse */ IdentityService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function IdentityService_Factory() { return new IdentityService(i0.ɵɵinject(i1.RestService)); }, token: IdentityService, providedIn: "root" });
    return IdentityService;
}());
export { IdentityService };
if (false) {
    /**
     * @type {?}
     * @private
     */
    IdentityService.prototype.rest;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuaWRlbnRpdHkvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvaWRlbnRpdHkuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFM0MsT0FBTyxFQUFFLFdBQVcsRUFBYSxNQUFNLGNBQWMsQ0FBQzs7O0FBR3REO0lBSUUseUJBQW9CLElBQWlCO1FBQWpCLFNBQUksR0FBSixJQUFJLENBQWE7SUFBRyxDQUFDOzs7OztJQUV6QyxrQ0FBUTs7OztJQUFSLFVBQVMsTUFBa0M7UUFBbEMsdUJBQUEsRUFBQSw0QkFBUyxFQUFFLEVBQXVCOztZQUNuQyxPQUFPLEdBQXVCO1lBQ2xDLE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRyxFQUFFLHFCQUFxQjtZQUMxQixNQUFNLFFBQUE7U0FDUDtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQThCLE9BQU8sQ0FBQyxDQUFDO0lBQ2pFLENBQUM7Ozs7O0lBRUQscUNBQVc7Ozs7SUFBWCxVQUFZLEVBQVU7O1lBQ2QsT0FBTyxHQUF1QjtZQUNsQyxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSx5QkFBdUIsRUFBSTtTQUNqQztRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQTBCLE9BQU8sQ0FBQyxDQUFDO0lBQzdELENBQUM7Ozs7O0lBRUQsb0NBQVU7Ozs7SUFBVixVQUFXLEVBQVU7O1lBQ2IsT0FBTyxHQUF1QjtZQUNsQyxNQUFNLEVBQUUsUUFBUTtZQUNoQixHQUFHLEVBQUUseUJBQXVCLEVBQUk7U0FDakM7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUEwQixPQUFPLENBQUMsQ0FBQztJQUM3RCxDQUFDOzs7OztJQUVELG9DQUFVOzs7O0lBQVYsVUFBVyxJQUE4Qjs7WUFDakMsT0FBTyxHQUEyQztZQUN0RCxNQUFNLEVBQUUsTUFBTTtZQUNkLEdBQUcsRUFBRSxxQkFBcUI7WUFDMUIsSUFBSSxNQUFBO1NBQ0w7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUE4QyxPQUFPLENBQUMsQ0FBQztJQUNqRixDQUFDOzs7OztJQUVELG9DQUFVOzs7O0lBQVYsVUFBVyxJQUF1Qjs7WUFDMUIsR0FBRyxHQUFHLHlCQUF1QixJQUFJLENBQUMsRUFBSTtRQUM1QyxPQUFPLElBQUksQ0FBQyxFQUFFLENBQUM7O1lBRVQsT0FBTyxHQUFvQztZQUMvQyxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsS0FBQTtZQUNILElBQUksTUFBQTtTQUNMO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBdUMsT0FBTyxDQUFDLENBQUM7SUFDMUUsQ0FBQzs7Ozs7SUFFRCxrQ0FBUTs7OztJQUFSLFVBQVMsTUFBa0M7UUFBbEMsdUJBQUEsRUFBQSw0QkFBUyxFQUFFLEVBQXVCOztZQUNuQyxPQUFPLEdBQXVCO1lBQ2xDLE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRyxFQUFFLHFCQUFxQjtZQUMxQixNQUFNLFFBQUE7U0FDUDtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQThCLE9BQU8sQ0FBQyxDQUFDO0lBQ2pFLENBQUM7Ozs7O0lBRUQscUNBQVc7Ozs7SUFBWCxVQUFZLEVBQVU7O1lBQ2QsT0FBTyxHQUF1QjtZQUNsQyxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSx5QkFBdUIsRUFBSTtTQUNqQztRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQTBCLE9BQU8sQ0FBQyxDQUFDO0lBQzdELENBQUM7Ozs7O0lBRUQsc0NBQVk7Ozs7SUFBWixVQUFhLEVBQVU7O1lBQ2YsT0FBTyxHQUF1QjtZQUNsQyxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSx5QkFBdUIsRUFBRSxXQUFRO1NBQ3ZDO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBOEIsT0FBTyxDQUFDLENBQUM7SUFDakUsQ0FBQzs7Ozs7SUFFRCxvQ0FBVTs7OztJQUFWLFVBQVcsRUFBVTs7WUFDYixPQUFPLEdBQXVCO1lBQ2xDLE1BQU0sRUFBRSxRQUFRO1lBQ2hCLEdBQUcsRUFBRSx5QkFBdUIsRUFBSTtTQUNqQztRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQWEsT0FBTyxDQUFDLENBQUM7SUFDaEQsQ0FBQzs7Ozs7SUFFRCxvQ0FBVTs7OztJQUFWLFVBQVcsSUFBOEI7O1lBQ2pDLE9BQU8sR0FBMkM7WUFDdEQsTUFBTSxFQUFFLE1BQU07WUFDZCxHQUFHLEVBQUUscUJBQXFCO1lBQzFCLElBQUksTUFBQTtTQUNMO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBOEMsT0FBTyxDQUFDLENBQUM7SUFDakYsQ0FBQzs7Ozs7SUFFRCxvQ0FBVTs7OztJQUFWLFVBQVcsSUFBdUI7O1lBQzFCLEdBQUcsR0FBRyx5QkFBdUIsSUFBSSxDQUFDLEVBQUk7UUFDNUMsT0FBTyxJQUFJLENBQUMsRUFBRSxDQUFDOztZQUVULE9BQU8sR0FBb0M7WUFDL0MsTUFBTSxFQUFFLEtBQUs7WUFDYixHQUFHLEtBQUE7WUFDSCxJQUFJLE1BQUE7U0FDTDtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQXVDLE9BQU8sQ0FBQyxDQUFDO0lBQzFFLENBQUM7O2dCQW5IRixVQUFVLFNBQUM7b0JBQ1YsVUFBVSxFQUFFLE1BQU07aUJBQ25COzs7O2dCQUxRLFdBQVc7OzswQkFGcEI7Q0F5SEMsQUFwSEQsSUFvSEM7U0FqSFksZUFBZTs7Ozs7O0lBQ2QsK0JBQXlCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgUmVzdFNlcnZpY2UsIFJlc3QsIEFCUCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBJZGVudGl0eSB9IGZyb20gJy4uL21vZGVscy9pZGVudGl0eSc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBJZGVudGl0eVNlcnZpY2Uge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJlc3Q6IFJlc3RTZXJ2aWNlKSB7fVxuXG4gIGdldFJvbGVzKHBhcmFtcyA9IHt9IGFzIEFCUC5QYWdlUXVlcnlQYXJhbXMpOiBPYnNlcnZhYmxlPElkZW50aXR5LlJvbGVSZXNwb25zZT4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxudWxsPiA9IHtcbiAgICAgIG1ldGhvZDogJ0dFVCcsXG4gICAgICB1cmw6ICcvYXBpL2lkZW50aXR5L3JvbGVzJyxcbiAgICAgIHBhcmFtcyxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PG51bGwsIElkZW50aXR5LlJvbGVSZXNwb25zZT4ocmVxdWVzdCk7XG4gIH1cblxuICBnZXRSb2xlQnlJZChpZDogc3RyaW5nKTogT2JzZXJ2YWJsZTxJZGVudGl0eS5Sb2xlSXRlbT4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxudWxsPiA9IHtcbiAgICAgIG1ldGhvZDogJ0dFVCcsXG4gICAgICB1cmw6IGAvYXBpL2lkZW50aXR5L3JvbGVzLyR7aWR9YCxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PG51bGwsIElkZW50aXR5LlJvbGVJdGVtPihyZXF1ZXN0KTtcbiAgfVxuXG4gIGRlbGV0ZVJvbGUoaWQ6IHN0cmluZyk6IE9ic2VydmFibGU8SWRlbnRpdHkuUm9sZUl0ZW0+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8bnVsbD4gPSB7XG4gICAgICBtZXRob2Q6ICdERUxFVEUnLFxuICAgICAgdXJsOiBgL2FwaS9pZGVudGl0eS9yb2xlcy8ke2lkfWAsXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxudWxsLCBJZGVudGl0eS5Sb2xlSXRlbT4ocmVxdWVzdCk7XG4gIH1cblxuICBjcmVhdGVSb2xlKGJvZHk6IElkZW50aXR5LlJvbGVTYXZlUmVxdWVzdCk6IE9ic2VydmFibGU8SWRlbnRpdHkuUm9sZUl0ZW0+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8SWRlbnRpdHkuUm9sZVNhdmVSZXF1ZXN0PiA9IHtcbiAgICAgIG1ldGhvZDogJ1BPU1QnLFxuICAgICAgdXJsOiAnL2FwaS9pZGVudGl0eS9yb2xlcycsXG4gICAgICBib2R5LFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8SWRlbnRpdHkuUm9sZVNhdmVSZXF1ZXN0LCBJZGVudGl0eS5Sb2xlSXRlbT4ocmVxdWVzdCk7XG4gIH1cblxuICB1cGRhdGVSb2xlKGJvZHk6IElkZW50aXR5LlJvbGVJdGVtKTogT2JzZXJ2YWJsZTxJZGVudGl0eS5Sb2xlSXRlbT4ge1xuICAgIGNvbnN0IHVybCA9IGAvYXBpL2lkZW50aXR5L3JvbGVzLyR7Ym9keS5pZH1gO1xuICAgIGRlbGV0ZSBib2R5LmlkO1xuXG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PElkZW50aXR5LlJvbGVJdGVtPiA9IHtcbiAgICAgIG1ldGhvZDogJ1BVVCcsXG4gICAgICB1cmwsXG4gICAgICBib2R5LFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8SWRlbnRpdHkuUm9sZUl0ZW0sIElkZW50aXR5LlJvbGVJdGVtPihyZXF1ZXN0KTtcbiAgfVxuXG4gIGdldFVzZXJzKHBhcmFtcyA9IHt9IGFzIEFCUC5QYWdlUXVlcnlQYXJhbXMpOiBPYnNlcnZhYmxlPElkZW50aXR5LlVzZXJSZXNwb25zZT4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxudWxsPiA9IHtcbiAgICAgIG1ldGhvZDogJ0dFVCcsXG4gICAgICB1cmw6ICcvYXBpL2lkZW50aXR5L3VzZXJzJyxcbiAgICAgIHBhcmFtcyxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PG51bGwsIElkZW50aXR5LlVzZXJSZXNwb25zZT4ocmVxdWVzdCk7XG4gIH1cblxuICBnZXRVc2VyQnlJZChpZDogc3RyaW5nKTogT2JzZXJ2YWJsZTxJZGVudGl0eS5Vc2VySXRlbT4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxudWxsPiA9IHtcbiAgICAgIG1ldGhvZDogJ0dFVCcsXG4gICAgICB1cmw6IGAvYXBpL2lkZW50aXR5L3VzZXJzLyR7aWR9YCxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PG51bGwsIElkZW50aXR5LlVzZXJJdGVtPihyZXF1ZXN0KTtcbiAgfVxuXG4gIGdldFVzZXJSb2xlcyhpZDogc3RyaW5nKTogT2JzZXJ2YWJsZTxJZGVudGl0eS5Sb2xlUmVzcG9uc2U+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8bnVsbD4gPSB7XG4gICAgICBtZXRob2Q6ICdHRVQnLFxuICAgICAgdXJsOiBgL2FwaS9pZGVudGl0eS91c2Vycy8ke2lkfS9yb2xlc2AsXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxudWxsLCBJZGVudGl0eS5Sb2xlUmVzcG9uc2U+KHJlcXVlc3QpO1xuICB9XG5cbiAgZGVsZXRlVXNlcihpZDogc3RyaW5nKTogT2JzZXJ2YWJsZTxudWxsPiB7XG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PG51bGw+ID0ge1xuICAgICAgbWV0aG9kOiAnREVMRVRFJyxcbiAgICAgIHVybDogYC9hcGkvaWRlbnRpdHkvdXNlcnMvJHtpZH1gLFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8bnVsbCwgbnVsbD4ocmVxdWVzdCk7XG4gIH1cblxuICBjcmVhdGVVc2VyKGJvZHk6IElkZW50aXR5LlVzZXJTYXZlUmVxdWVzdCk6IE9ic2VydmFibGU8SWRlbnRpdHkuVXNlckl0ZW0+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8SWRlbnRpdHkuVXNlclNhdmVSZXF1ZXN0PiA9IHtcbiAgICAgIG1ldGhvZDogJ1BPU1QnLFxuICAgICAgdXJsOiAnL2FwaS9pZGVudGl0eS91c2VycycsXG4gICAgICBib2R5LFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8SWRlbnRpdHkuVXNlclNhdmVSZXF1ZXN0LCBJZGVudGl0eS5Vc2VySXRlbT4ocmVxdWVzdCk7XG4gIH1cblxuICB1cGRhdGVVc2VyKGJvZHk6IElkZW50aXR5LlVzZXJJdGVtKTogT2JzZXJ2YWJsZTxJZGVudGl0eS5Vc2VySXRlbT4ge1xuICAgIGNvbnN0IHVybCA9IGAvYXBpL2lkZW50aXR5L3VzZXJzLyR7Ym9keS5pZH1gO1xuICAgIGRlbGV0ZSBib2R5LmlkO1xuXG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PElkZW50aXR5LlVzZXJJdGVtPiA9IHtcbiAgICAgIG1ldGhvZDogJ1BVVCcsXG4gICAgICB1cmwsXG4gICAgICBib2R5LFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8SWRlbnRpdHkuVXNlckl0ZW0sIElkZW50aXR5LlVzZXJJdGVtPihyZXF1ZXN0KTtcbiAgfVxufVxuIl19