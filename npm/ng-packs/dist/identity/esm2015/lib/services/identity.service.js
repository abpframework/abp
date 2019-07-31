/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import * as i0 from "@angular/core";
import * as i1 from "@abp/ng.core";
export class IdentityService {
    /**
     * @param {?} rest
     */
    constructor(rest) {
        this.rest = rest;
    }
    /**
     * @return {?}
     */
    getRoles() {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: '/api/identity/roles',
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} id
     * @return {?}
     */
    getRoleById(id) {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: `/api/identity/roles/${id}`,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} id
     * @return {?}
     */
    deleteRole(id) {
        /** @type {?} */
        const request = {
            method: 'DELETE',
            url: `/api/identity/roles/${id}`,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} body
     * @return {?}
     */
    addRole(body) {
        /** @type {?} */
        const request = {
            method: 'POST',
            url: '/api/identity/roles',
            body,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} body
     * @return {?}
     */
    updateRole(body) {
        /** @type {?} */
        const url = `/api/identity/roles/${body.id}`;
        delete body.id;
        /** @type {?} */
        const request = {
            method: 'PUT',
            url,
            body,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?=} params
     * @return {?}
     */
    getUsers(params = (/** @type {?} */ ({}))) {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: '/api/identity/users',
            params,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} id
     * @return {?}
     */
    getUserById(id) {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: `/api/identity/users/${id}`,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} id
     * @return {?}
     */
    getUserRoles(id) {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: `/api/identity/users/${id}/roles`,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} id
     * @return {?}
     */
    deleteUser(id) {
        /** @type {?} */
        const request = {
            method: 'DELETE',
            url: `/api/identity/users/${id}`,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} body
     * @return {?}
     */
    addUser(body) {
        /** @type {?} */
        const request = {
            method: 'POST',
            url: '/api/identity/users',
            body,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} body
     * @return {?}
     */
    updateUser(body) {
        /** @type {?} */
        const url = `/identity/users/${body.id}`;
        delete body.id;
        /** @type {?} */
        const request = {
            method: 'PUT',
            url,
            body,
        };
        return this.rest.request(request);
    }
}
IdentityService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
IdentityService.ctorParameters = () => [
    { type: RestService }
];
/** @nocollapse */ IdentityService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function IdentityService_Factory() { return new IdentityService(i0.ɵɵinject(i1.RestService)); }, token: IdentityService, providedIn: "root" });
if (false) {
    /**
     * @type {?}
     * @private
     */
    IdentityService.prototype.rest;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuaWRlbnRpdHkvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvaWRlbnRpdHkuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzQyxPQUFPLEVBQUUsV0FBVyxFQUFhLE1BQU0sY0FBYyxDQUFDOzs7QUFNdEQsTUFBTSxPQUFPLGVBQWU7Ozs7SUFDMUIsWUFBb0IsSUFBaUI7UUFBakIsU0FBSSxHQUFKLElBQUksQ0FBYTtJQUFHLENBQUM7Ozs7SUFFekMsUUFBUTs7Y0FDQSxPQUFPLEdBQXVCO1lBQ2xDLE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRyxFQUFFLHFCQUFxQjtTQUMzQjtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQThCLE9BQU8sQ0FBQyxDQUFDO0lBQ2pFLENBQUM7Ozs7O0lBRUQsV0FBVyxDQUFDLEVBQVU7O2NBQ2QsT0FBTyxHQUF1QjtZQUNsQyxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSx1QkFBdUIsRUFBRSxFQUFFO1NBQ2pDO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBMEIsT0FBTyxDQUFDLENBQUM7SUFDN0QsQ0FBQzs7Ozs7SUFFRCxVQUFVLENBQUMsRUFBVTs7Y0FDYixPQUFPLEdBQXVCO1lBQ2xDLE1BQU0sRUFBRSxRQUFRO1lBQ2hCLEdBQUcsRUFBRSx1QkFBdUIsRUFBRSxFQUFFO1NBQ2pDO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBMEIsT0FBTyxDQUFDLENBQUM7SUFDN0QsQ0FBQzs7Ozs7SUFFRCxPQUFPLENBQUMsSUFBOEI7O2NBQzlCLE9BQU8sR0FBMkM7WUFDdEQsTUFBTSxFQUFFLE1BQU07WUFDZCxHQUFHLEVBQUUscUJBQXFCO1lBQzFCLElBQUk7U0FDTDtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQThDLE9BQU8sQ0FBQyxDQUFDO0lBQ2pGLENBQUM7Ozs7O0lBRUQsVUFBVSxDQUFDLElBQXVCOztjQUMxQixHQUFHLEdBQUcsdUJBQXVCLElBQUksQ0FBQyxFQUFFLEVBQUU7UUFDNUMsT0FBTyxJQUFJLENBQUMsRUFBRSxDQUFDOztjQUVULE9BQU8sR0FBb0M7WUFDL0MsTUFBTSxFQUFFLEtBQUs7WUFDYixHQUFHO1lBQ0gsSUFBSTtTQUNMO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBdUMsT0FBTyxDQUFDLENBQUM7SUFDMUUsQ0FBQzs7Ozs7SUFFRCxRQUFRLENBQUMsTUFBTSxHQUFHLG1CQUFBLEVBQUUsRUFBdUI7O2NBQ25DLE9BQU8sR0FBdUI7WUFDbEMsTUFBTSxFQUFFLEtBQUs7WUFDYixHQUFHLEVBQUUscUJBQXFCO1lBQzFCLE1BQU07U0FDUDtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQThCLE9BQU8sQ0FBQyxDQUFDO0lBQ2pFLENBQUM7Ozs7O0lBRUQsV0FBVyxDQUFDLEVBQVU7O2NBQ2QsT0FBTyxHQUF1QjtZQUNsQyxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSx1QkFBdUIsRUFBRSxFQUFFO1NBQ2pDO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBMEIsT0FBTyxDQUFDLENBQUM7SUFDN0QsQ0FBQzs7Ozs7SUFFRCxZQUFZLENBQUMsRUFBVTs7Y0FDZixPQUFPLEdBQXVCO1lBQ2xDLE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRyxFQUFFLHVCQUF1QixFQUFFLFFBQVE7U0FDdkM7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUE4QixPQUFPLENBQUMsQ0FBQztJQUNqRSxDQUFDOzs7OztJQUVELFVBQVUsQ0FBQyxFQUFVOztjQUNiLE9BQU8sR0FBdUI7WUFDbEMsTUFBTSxFQUFFLFFBQVE7WUFDaEIsR0FBRyxFQUFFLHVCQUF1QixFQUFFLEVBQUU7U0FDakM7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFhLE9BQU8sQ0FBQyxDQUFDO0lBQ2hELENBQUM7Ozs7O0lBRUQsT0FBTyxDQUFDLElBQThCOztjQUM5QixPQUFPLEdBQTJDO1lBQ3RELE1BQU0sRUFBRSxNQUFNO1lBQ2QsR0FBRyxFQUFFLHFCQUFxQjtZQUMxQixJQUFJO1NBQ0w7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUE4QyxPQUFPLENBQUMsQ0FBQztJQUNqRixDQUFDOzs7OztJQUVELFVBQVUsQ0FBQyxJQUF1Qjs7Y0FDMUIsR0FBRyxHQUFHLG1CQUFtQixJQUFJLENBQUMsRUFBRSxFQUFFO1FBQ3hDLE9BQU8sSUFBSSxDQUFDLEVBQUUsQ0FBQzs7Y0FFVCxPQUFPLEdBQW9DO1lBQy9DLE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRztZQUNILElBQUk7U0FDTDtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQXVDLE9BQU8sQ0FBQyxDQUFDO0lBQzFFLENBQUM7OztZQWxIRixVQUFVLFNBQUM7Z0JBQ1YsVUFBVSxFQUFFLE1BQU07YUFDbkI7Ozs7WUFMUSxXQUFXOzs7Ozs7OztJQU9OLCtCQUF5QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IFJlc3RTZXJ2aWNlLCBSZXN0LCBBQlAgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgSWRlbnRpdHkgfSBmcm9tICcuLi9tb2RlbHMvaWRlbnRpdHknO1xuXG5ASW5qZWN0YWJsZSh7XG4gIHByb3ZpZGVkSW46ICdyb290Jyxcbn0pXG5leHBvcnQgY2xhc3MgSWRlbnRpdHlTZXJ2aWNlIHtcbiAgY29uc3RydWN0b3IocHJpdmF0ZSByZXN0OiBSZXN0U2VydmljZSkge31cblxuICBnZXRSb2xlcygpOiBPYnNlcnZhYmxlPElkZW50aXR5LlJvbGVSZXNwb25zZT4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxudWxsPiA9IHtcbiAgICAgIG1ldGhvZDogJ0dFVCcsXG4gICAgICB1cmw6ICcvYXBpL2lkZW50aXR5L3JvbGVzJyxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PG51bGwsIElkZW50aXR5LlJvbGVSZXNwb25zZT4ocmVxdWVzdCk7XG4gIH1cblxuICBnZXRSb2xlQnlJZChpZDogc3RyaW5nKTogT2JzZXJ2YWJsZTxJZGVudGl0eS5Sb2xlSXRlbT4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxudWxsPiA9IHtcbiAgICAgIG1ldGhvZDogJ0dFVCcsXG4gICAgICB1cmw6IGAvYXBpL2lkZW50aXR5L3JvbGVzLyR7aWR9YCxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PG51bGwsIElkZW50aXR5LlJvbGVJdGVtPihyZXF1ZXN0KTtcbiAgfVxuXG4gIGRlbGV0ZVJvbGUoaWQ6IHN0cmluZyk6IE9ic2VydmFibGU8SWRlbnRpdHkuUm9sZUl0ZW0+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8bnVsbD4gPSB7XG4gICAgICBtZXRob2Q6ICdERUxFVEUnLFxuICAgICAgdXJsOiBgL2FwaS9pZGVudGl0eS9yb2xlcy8ke2lkfWAsXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxudWxsLCBJZGVudGl0eS5Sb2xlSXRlbT4ocmVxdWVzdCk7XG4gIH1cblxuICBhZGRSb2xlKGJvZHk6IElkZW50aXR5LlJvbGVTYXZlUmVxdWVzdCk6IE9ic2VydmFibGU8SWRlbnRpdHkuUm9sZUl0ZW0+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8SWRlbnRpdHkuUm9sZVNhdmVSZXF1ZXN0PiA9IHtcbiAgICAgIG1ldGhvZDogJ1BPU1QnLFxuICAgICAgdXJsOiAnL2FwaS9pZGVudGl0eS9yb2xlcycsXG4gICAgICBib2R5LFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8SWRlbnRpdHkuUm9sZVNhdmVSZXF1ZXN0LCBJZGVudGl0eS5Sb2xlSXRlbT4ocmVxdWVzdCk7XG4gIH1cblxuICB1cGRhdGVSb2xlKGJvZHk6IElkZW50aXR5LlJvbGVJdGVtKTogT2JzZXJ2YWJsZTxJZGVudGl0eS5Sb2xlSXRlbT4ge1xuICAgIGNvbnN0IHVybCA9IGAvYXBpL2lkZW50aXR5L3JvbGVzLyR7Ym9keS5pZH1gO1xuICAgIGRlbGV0ZSBib2R5LmlkO1xuXG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PElkZW50aXR5LlJvbGVJdGVtPiA9IHtcbiAgICAgIG1ldGhvZDogJ1BVVCcsXG4gICAgICB1cmwsXG4gICAgICBib2R5LFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8SWRlbnRpdHkuUm9sZUl0ZW0sIElkZW50aXR5LlJvbGVJdGVtPihyZXF1ZXN0KTtcbiAgfVxuXG4gIGdldFVzZXJzKHBhcmFtcyA9IHt9IGFzIEFCUC5QYWdlUXVlcnlQYXJhbXMpOiBPYnNlcnZhYmxlPElkZW50aXR5LlVzZXJSZXNwb25zZT4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxudWxsPiA9IHtcbiAgICAgIG1ldGhvZDogJ0dFVCcsXG4gICAgICB1cmw6ICcvYXBpL2lkZW50aXR5L3VzZXJzJyxcbiAgICAgIHBhcmFtcyxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PG51bGwsIElkZW50aXR5LlVzZXJSZXNwb25zZT4ocmVxdWVzdCk7XG4gIH1cblxuICBnZXRVc2VyQnlJZChpZDogc3RyaW5nKTogT2JzZXJ2YWJsZTxJZGVudGl0eS5Vc2VySXRlbT4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxudWxsPiA9IHtcbiAgICAgIG1ldGhvZDogJ0dFVCcsXG4gICAgICB1cmw6IGAvYXBpL2lkZW50aXR5L3VzZXJzLyR7aWR9YCxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PG51bGwsIElkZW50aXR5LlVzZXJJdGVtPihyZXF1ZXN0KTtcbiAgfVxuXG4gIGdldFVzZXJSb2xlcyhpZDogc3RyaW5nKTogT2JzZXJ2YWJsZTxJZGVudGl0eS5Sb2xlUmVzcG9uc2U+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8bnVsbD4gPSB7XG4gICAgICBtZXRob2Q6ICdHRVQnLFxuICAgICAgdXJsOiBgL2FwaS9pZGVudGl0eS91c2Vycy8ke2lkfS9yb2xlc2AsXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxudWxsLCBJZGVudGl0eS5Sb2xlUmVzcG9uc2U+KHJlcXVlc3QpO1xuICB9XG5cbiAgZGVsZXRlVXNlcihpZDogc3RyaW5nKTogT2JzZXJ2YWJsZTxudWxsPiB7XG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PG51bGw+ID0ge1xuICAgICAgbWV0aG9kOiAnREVMRVRFJyxcbiAgICAgIHVybDogYC9hcGkvaWRlbnRpdHkvdXNlcnMvJHtpZH1gLFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8bnVsbCwgbnVsbD4ocmVxdWVzdCk7XG4gIH1cblxuICBhZGRVc2VyKGJvZHk6IElkZW50aXR5LlVzZXJTYXZlUmVxdWVzdCk6IE9ic2VydmFibGU8SWRlbnRpdHkuVXNlckl0ZW0+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8SWRlbnRpdHkuVXNlclNhdmVSZXF1ZXN0PiA9IHtcbiAgICAgIG1ldGhvZDogJ1BPU1QnLFxuICAgICAgdXJsOiAnL2FwaS9pZGVudGl0eS91c2VycycsXG4gICAgICBib2R5LFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8SWRlbnRpdHkuVXNlclNhdmVSZXF1ZXN0LCBJZGVudGl0eS5Vc2VySXRlbT4ocmVxdWVzdCk7XG4gIH1cblxuICB1cGRhdGVVc2VyKGJvZHk6IElkZW50aXR5LlVzZXJJdGVtKTogT2JzZXJ2YWJsZTxJZGVudGl0eS5Vc2VySXRlbT4ge1xuICAgIGNvbnN0IHVybCA9IGAvaWRlbnRpdHkvdXNlcnMvJHtib2R5LmlkfWA7XG4gICAgZGVsZXRlIGJvZHkuaWQ7XG5cbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8SWRlbnRpdHkuVXNlckl0ZW0+ID0ge1xuICAgICAgbWV0aG9kOiAnUFVUJyxcbiAgICAgIHVybCxcbiAgICAgIGJvZHksXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxJZGVudGl0eS5Vc2VySXRlbSwgSWRlbnRpdHkuVXNlckl0ZW0+KHJlcXVlc3QpO1xuICB9XG59XG4iXX0=