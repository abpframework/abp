/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import * as i0 from "@angular/core";
import * as i1 from "@abp/ng.core";
export class TenantManagementService {
    /**
     * @param {?} rest
     */
    constructor(rest) {
        this.rest = rest;
    }
    /**
     * @return {?}
     */
    get() {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: '/api/multi-tenancy/tenants',
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} id
     * @return {?}
     */
    getById(id) {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: `/api/multi-tenancy/tenants/${id}`,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} id
     * @return {?}
     */
    delete(id) {
        /** @type {?} */
        const request = {
            method: 'DELETE',
            url: `/api/multi-tenancy/tenants/${id}`,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} body
     * @return {?}
     */
    add(body) {
        /** @type {?} */
        const request = {
            method: 'POST',
            url: `/api/multi-tenancy/tenants`,
            body,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} body
     * @return {?}
     */
    update(body) {
        /** @type {?} */
        const url = `/api/multi-tenancy/tenants/${body.id}`;
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
     * @param {?} id
     * @return {?}
     */
    getDefaultConnectionString(id) {
        /** @type {?} */
        const url = `/api/multi-tenancy/tenants/${id}/defaultConnectionString`;
        /** @type {?} */
        const request = {
            method: 'GET',
            responseType: "text" /* Text */,
            url,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} payload
     * @return {?}
     */
    updateDefaultConnectionString(payload) {
        /** @type {?} */
        const url = `/api/multi-tenancy/tenants/${payload.id}/defaultConnectionString`;
        /** @type {?} */
        const request = {
            method: 'PUT',
            url,
            params: { defaultConnectionString: payload.defaultConnectionString },
        };
        return this.rest.request(request);
    }
}
TenantManagementService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
TenantManagementService.ctorParameters = () => [
    { type: RestService }
];
/** @nocollapse */ TenantManagementService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function TenantManagementService_Factory() { return new TenantManagementService(i0.ɵɵinject(i1.RestService)); }, token: TenantManagementService, providedIn: "root" });
if (false) {
    /**
     * @type {?}
     * @private
     */
    TenantManagementService.prototype.rest;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGVuYW50LW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvdGVuYW50LW1hbmFnZW1lbnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzQyxPQUFPLEVBQUUsV0FBVyxFQUFhLE1BQU0sY0FBYyxDQUFDOzs7QUFNdEQsTUFBTSxPQUFPLHVCQUF1Qjs7OztJQUNsQyxZQUFvQixJQUFpQjtRQUFqQixTQUFJLEdBQUosSUFBSSxDQUFhO0lBQUcsQ0FBQzs7OztJQUV6QyxHQUFHOztjQUNLLE9BQU8sR0FBdUI7WUFDbEMsTUFBTSxFQUFFLEtBQUs7WUFDYixHQUFHLEVBQUUsNEJBQTRCO1NBQ2xDO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBa0MsT0FBTyxDQUFDLENBQUM7SUFDckUsQ0FBQzs7Ozs7SUFFRCxPQUFPLENBQUMsRUFBVTs7Y0FDVixPQUFPLEdBQXVCO1lBQ2xDLE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRyxFQUFFLDhCQUE4QixFQUFFLEVBQUU7U0FDeEM7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFzQixPQUFPLENBQUMsQ0FBQztJQUN6RCxDQUFDOzs7OztJQUVELE1BQU0sQ0FBQyxFQUFVOztjQUNULE9BQU8sR0FBdUI7WUFDbEMsTUFBTSxFQUFFLFFBQVE7WUFDaEIsR0FBRyxFQUFFLDhCQUE4QixFQUFFLEVBQUU7U0FDeEM7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFhLE9BQU8sQ0FBQyxDQUFDO0lBQ2hELENBQUM7Ozs7O0lBRUQsR0FBRyxDQUFDLElBQWlDOztjQUM3QixPQUFPLEdBQThDO1lBQ3pELE1BQU0sRUFBRSxNQUFNO1lBQ2QsR0FBRyxFQUFFLDRCQUE0QjtZQUNqQyxJQUFJO1NBQ0w7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUE2QyxPQUFPLENBQUMsQ0FBQztJQUNoRixDQUFDOzs7OztJQUVELE1BQU0sQ0FBQyxJQUFvQzs7Y0FDbkMsR0FBRyxHQUFHLDhCQUE4QixJQUFJLENBQUMsRUFBRSxFQUFFO1FBQ25ELE9BQU8sSUFBSSxDQUFDLEVBQUUsQ0FBQzs7Y0FFVCxPQUFPLEdBQWlEO1lBQzVELE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRztZQUNILElBQUk7U0FDTDtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQTZDLE9BQU8sQ0FBQyxDQUFDO0lBQ2hGLENBQUM7Ozs7O0lBRUQsMEJBQTBCLENBQUMsRUFBVTs7Y0FDN0IsR0FBRyxHQUFHLDhCQUE4QixFQUFFLDBCQUEwQjs7Y0FFaEUsT0FBTyxHQUFrRTtZQUM3RSxNQUFNLEVBQUUsS0FBSztZQUNiLFlBQVksbUJBQXdCO1lBQ3BDLEdBQUc7U0FDSjtRQUNELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQTBELE9BQU8sQ0FBQyxDQUFDO0lBQzdGLENBQUM7Ozs7O0lBRUQsNkJBQTZCLENBQUMsT0FBd0Q7O2NBQzlFLEdBQUcsR0FBRyw4QkFBOEIsT0FBTyxDQUFDLEVBQUUsMEJBQTBCOztjQUV4RSxPQUFPLEdBQWtFO1lBQzdFLE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRztZQUNILE1BQU0sRUFBRSxFQUFFLHVCQUF1QixFQUFFLE9BQU8sQ0FBQyx1QkFBdUIsRUFBRTtTQUNyRTtRQUNELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQXVELE9BQU8sQ0FBQyxDQUFDO0lBQzFGLENBQUM7OztZQTVFRixVQUFVLFNBQUM7Z0JBQ1YsVUFBVSxFQUFFLE1BQU07YUFDbkI7Ozs7WUFMUSxXQUFXOzs7Ozs7OztJQU9OLHVDQUF5QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IFJlc3RTZXJ2aWNlLCBSZXN0LCBBQlAgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgVGVuYW50TWFuYWdlbWVudCB9IGZyb20gJy4uL21vZGVscy90ZW5hbnQtbWFuYWdlbWVudCc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBUZW5hbnRNYW5hZ2VtZW50U2VydmljZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcmVzdDogUmVzdFNlcnZpY2UpIHt9XG5cbiAgZ2V0KCk6IE9ic2VydmFibGU8VGVuYW50TWFuYWdlbWVudC5SZXNwb25zZT4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxudWxsPiA9IHtcbiAgICAgIG1ldGhvZDogJ0dFVCcsXG4gICAgICB1cmw6ICcvYXBpL211bHRpLXRlbmFuY3kvdGVuYW50cycsXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxudWxsLCBUZW5hbnRNYW5hZ2VtZW50LlJlc3BvbnNlPihyZXF1ZXN0KTtcbiAgfVxuXG4gIGdldEJ5SWQoaWQ6IHN0cmluZyk6IE9ic2VydmFibGU8QUJQLkJhc2ljSXRlbT4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxudWxsPiA9IHtcbiAgICAgIG1ldGhvZDogJ0dFVCcsXG4gICAgICB1cmw6IGAvYXBpL211bHRpLXRlbmFuY3kvdGVuYW50cy8ke2lkfWAsXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxudWxsLCBBQlAuQmFzaWNJdGVtPihyZXF1ZXN0KTtcbiAgfVxuXG4gIGRlbGV0ZShpZDogc3RyaW5nKTogT2JzZXJ2YWJsZTxudWxsPiB7XG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PG51bGw+ID0ge1xuICAgICAgbWV0aG9kOiAnREVMRVRFJyxcbiAgICAgIHVybDogYC9hcGkvbXVsdGktdGVuYW5jeS90ZW5hbnRzLyR7aWR9YCxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PG51bGwsIG51bGw+KHJlcXVlc3QpO1xuICB9XG5cbiAgYWRkKGJvZHk6IFRlbmFudE1hbmFnZW1lbnQuQWRkUmVxdWVzdCk6IE9ic2VydmFibGU8QUJQLkJhc2ljSXRlbT4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxUZW5hbnRNYW5hZ2VtZW50LkFkZFJlcXVlc3Q+ID0ge1xuICAgICAgbWV0aG9kOiAnUE9TVCcsXG4gICAgICB1cmw6IGAvYXBpL211bHRpLXRlbmFuY3kvdGVuYW50c2AsXG4gICAgICBib2R5LFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8VGVuYW50TWFuYWdlbWVudC5BZGRSZXF1ZXN0LCBBQlAuQmFzaWNJdGVtPihyZXF1ZXN0KTtcbiAgfVxuXG4gIHVwZGF0ZShib2R5OiBUZW5hbnRNYW5hZ2VtZW50LlVwZGF0ZVJlcXVlc3QpOiBPYnNlcnZhYmxlPEFCUC5CYXNpY0l0ZW0+IHtcbiAgICBjb25zdCB1cmwgPSBgL2FwaS9tdWx0aS10ZW5hbmN5L3RlbmFudHMvJHtib2R5LmlkfWA7XG4gICAgZGVsZXRlIGJvZHkuaWQ7XG5cbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8VGVuYW50TWFuYWdlbWVudC5VcGRhdGVSZXF1ZXN0PiA9IHtcbiAgICAgIG1ldGhvZDogJ1BVVCcsXG4gICAgICB1cmwsXG4gICAgICBib2R5LFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8VGVuYW50TWFuYWdlbWVudC5BZGRSZXF1ZXN0LCBBQlAuQmFzaWNJdGVtPihyZXF1ZXN0KTtcbiAgfVxuXG4gIGdldERlZmF1bHRDb25uZWN0aW9uU3RyaW5nKGlkOiBzdHJpbmcpOiBPYnNlcnZhYmxlPHN0cmluZz4ge1xuICAgIGNvbnN0IHVybCA9IGAvYXBpL211bHRpLXRlbmFuY3kvdGVuYW50cy8ke2lkfS9kZWZhdWx0Q29ubmVjdGlvblN0cmluZ2A7XG5cbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8VGVuYW50TWFuYWdlbWVudC5EZWZhdWx0Q29ubmVjdGlvblN0cmluZ1JlcXVlc3Q+ID0ge1xuICAgICAgbWV0aG9kOiAnR0VUJyxcbiAgICAgIHJlc3BvbnNlVHlwZTogUmVzdC5SZXNwb25zZVR5cGUuVGV4dCxcbiAgICAgIHVybCxcbiAgICB9O1xuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxUZW5hbnRNYW5hZ2VtZW50LkRlZmF1bHRDb25uZWN0aW9uU3RyaW5nUmVxdWVzdCwgc3RyaW5nPihyZXF1ZXN0KTtcbiAgfVxuXG4gIHVwZGF0ZURlZmF1bHRDb25uZWN0aW9uU3RyaW5nKHBheWxvYWQ6IFRlbmFudE1hbmFnZW1lbnQuRGVmYXVsdENvbm5lY3Rpb25TdHJpbmdSZXF1ZXN0KTogT2JzZXJ2YWJsZTxhbnk+IHtcbiAgICBjb25zdCB1cmwgPSBgL2FwaS9tdWx0aS10ZW5hbmN5L3RlbmFudHMvJHtwYXlsb2FkLmlkfS9kZWZhdWx0Q29ubmVjdGlvblN0cmluZ2A7XG5cbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8VGVuYW50TWFuYWdlbWVudC5EZWZhdWx0Q29ubmVjdGlvblN0cmluZ1JlcXVlc3Q+ID0ge1xuICAgICAgbWV0aG9kOiAnUFVUJyxcbiAgICAgIHVybCxcbiAgICAgIHBhcmFtczogeyBkZWZhdWx0Q29ubmVjdGlvblN0cmluZzogcGF5bG9hZC5kZWZhdWx0Q29ubmVjdGlvblN0cmluZyB9LFxuICAgIH07XG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PFRlbmFudE1hbmFnZW1lbnQuRGVmYXVsdENvbm5lY3Rpb25TdHJpbmdSZXF1ZXN0LCBhbnk+KHJlcXVlc3QpO1xuICB9XG59XG4iXX0=