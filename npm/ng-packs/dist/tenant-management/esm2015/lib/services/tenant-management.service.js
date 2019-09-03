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
     * @param {?=} params
     * @return {?}
     */
    getTenant(params = (/** @type {?} */ ({}))) {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: '/api/multi-tenancy/tenants',
            params,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} id
     * @return {?}
     */
    getTenantById(id) {
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
    deleteTenant(id) {
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
    createTenant(body) {
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
    updateTenant(body) {
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
        const url = `/api/multi-tenancy/tenants/${id}/default-connection-string`;
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
        const url = `/api/multi-tenancy/tenants/${payload.id}/default-connection-string`;
        /** @type {?} */
        const request = {
            method: 'PUT',
            url,
            params: { defaultConnectionString: payload.defaultConnectionString },
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} id
     * @return {?}
     */
    deleteDefaultConnectionString(id) {
        /** @type {?} */
        const url = `/api/multi-tenancy/tenants/${id}/default-connection-string`;
        /** @type {?} */
        const request = {
            method: 'DELETE',
            url,
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGVuYW50LW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvdGVuYW50LW1hbmFnZW1lbnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzQyxPQUFPLEVBQUUsV0FBVyxFQUFhLE1BQU0sY0FBYyxDQUFDOzs7QUFNdEQsTUFBTSxPQUFPLHVCQUF1Qjs7OztJQUNsQyxZQUFvQixJQUFpQjtRQUFqQixTQUFJLEdBQUosSUFBSSxDQUFhO0lBQUcsQ0FBQzs7Ozs7SUFFekMsU0FBUyxDQUFDLE1BQU0sR0FBRyxtQkFBQSxFQUFFLEVBQXVCOztjQUNwQyxPQUFPLEdBQXVCO1lBQ2xDLE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRyxFQUFFLDRCQUE0QjtZQUNqQyxNQUFNO1NBQ1A7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFrQyxPQUFPLENBQUMsQ0FBQztJQUNyRSxDQUFDOzs7OztJQUVELGFBQWEsQ0FBQyxFQUFVOztjQUNoQixPQUFPLEdBQXVCO1lBQ2xDLE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRyxFQUFFLDhCQUE4QixFQUFFLEVBQUU7U0FDeEM7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFzQixPQUFPLENBQUMsQ0FBQztJQUN6RCxDQUFDOzs7OztJQUVELFlBQVksQ0FBQyxFQUFVOztjQUNmLE9BQU8sR0FBdUI7WUFDbEMsTUFBTSxFQUFFLFFBQVE7WUFDaEIsR0FBRyxFQUFFLDhCQUE4QixFQUFFLEVBQUU7U0FDeEM7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFhLE9BQU8sQ0FBQyxDQUFDO0lBQ2hELENBQUM7Ozs7O0lBRUQsWUFBWSxDQUFDLElBQWlDOztjQUN0QyxPQUFPLEdBQThDO1lBQ3pELE1BQU0sRUFBRSxNQUFNO1lBQ2QsR0FBRyxFQUFFLDRCQUE0QjtZQUNqQyxJQUFJO1NBQ0w7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUE2QyxPQUFPLENBQUMsQ0FBQztJQUNoRixDQUFDOzs7OztJQUVELFlBQVksQ0FBQyxJQUFvQzs7Y0FDekMsR0FBRyxHQUFHLDhCQUE4QixJQUFJLENBQUMsRUFBRSxFQUFFO1FBQ25ELE9BQU8sSUFBSSxDQUFDLEVBQUUsQ0FBQzs7Y0FFVCxPQUFPLEdBQWlEO1lBQzVELE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRztZQUNILElBQUk7U0FDTDtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQTZDLE9BQU8sQ0FBQyxDQUFDO0lBQ2hGLENBQUM7Ozs7O0lBRUQsMEJBQTBCLENBQUMsRUFBVTs7Y0FDN0IsR0FBRyxHQUFHLDhCQUE4QixFQUFFLDRCQUE0Qjs7Y0FFbEUsT0FBTyxHQUFrRTtZQUM3RSxNQUFNLEVBQUUsS0FBSztZQUNiLFlBQVksbUJBQXdCO1lBQ3BDLEdBQUc7U0FDSjtRQUNELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQTBELE9BQU8sQ0FBQyxDQUFDO0lBQzdGLENBQUM7Ozs7O0lBRUQsNkJBQTZCLENBQUMsT0FBd0Q7O2NBQzlFLEdBQUcsR0FBRyw4QkFBOEIsT0FBTyxDQUFDLEVBQUUsNEJBQTRCOztjQUUxRSxPQUFPLEdBQWtFO1lBQzdFLE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRztZQUNILE1BQU0sRUFBRSxFQUFFLHVCQUF1QixFQUFFLE9BQU8sQ0FBQyx1QkFBdUIsRUFBRTtTQUNyRTtRQUNELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQXVELE9BQU8sQ0FBQyxDQUFDO0lBQzFGLENBQUM7Ozs7O0lBRUQsNkJBQTZCLENBQUMsRUFBVTs7Y0FDaEMsR0FBRyxHQUFHLDhCQUE4QixFQUFFLDRCQUE0Qjs7Y0FFbEUsT0FBTyxHQUFrRTtZQUM3RSxNQUFNLEVBQUUsUUFBUTtZQUNoQixHQUFHO1NBQ0o7UUFDRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUF1RCxPQUFPLENBQUMsQ0FBQztJQUMxRixDQUFDOzs7WUF2RkYsVUFBVSxTQUFDO2dCQUNWLFVBQVUsRUFBRSxNQUFNO2FBQ25COzs7O1lBTFEsV0FBVzs7Ozs7Ozs7SUFPTix1Q0FBeUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBSZXN0U2VydmljZSwgUmVzdCwgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvdGVuYW50LW1hbmFnZW1lbnQnO1xuXG5ASW5qZWN0YWJsZSh7XG4gIHByb3ZpZGVkSW46ICdyb290Jyxcbn0pXG5leHBvcnQgY2xhc3MgVGVuYW50TWFuYWdlbWVudFNlcnZpY2Uge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJlc3Q6IFJlc3RTZXJ2aWNlKSB7fVxuXG4gIGdldFRlbmFudChwYXJhbXMgPSB7fSBhcyBBQlAuUGFnZVF1ZXJ5UGFyYW1zKTogT2JzZXJ2YWJsZTxUZW5hbnRNYW5hZ2VtZW50LlJlc3BvbnNlPiB7XG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PG51bGw+ID0ge1xuICAgICAgbWV0aG9kOiAnR0VUJyxcbiAgICAgIHVybDogJy9hcGkvbXVsdGktdGVuYW5jeS90ZW5hbnRzJyxcbiAgICAgIHBhcmFtcyxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PG51bGwsIFRlbmFudE1hbmFnZW1lbnQuUmVzcG9uc2U+KHJlcXVlc3QpO1xuICB9XG5cbiAgZ2V0VGVuYW50QnlJZChpZDogc3RyaW5nKTogT2JzZXJ2YWJsZTxBQlAuQmFzaWNJdGVtPiB7XG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PG51bGw+ID0ge1xuICAgICAgbWV0aG9kOiAnR0VUJyxcbiAgICAgIHVybDogYC9hcGkvbXVsdGktdGVuYW5jeS90ZW5hbnRzLyR7aWR9YCxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PG51bGwsIEFCUC5CYXNpY0l0ZW0+KHJlcXVlc3QpO1xuICB9XG5cbiAgZGVsZXRlVGVuYW50KGlkOiBzdHJpbmcpOiBPYnNlcnZhYmxlPG51bGw+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8bnVsbD4gPSB7XG4gICAgICBtZXRob2Q6ICdERUxFVEUnLFxuICAgICAgdXJsOiBgL2FwaS9tdWx0aS10ZW5hbmN5L3RlbmFudHMvJHtpZH1gLFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8bnVsbCwgbnVsbD4ocmVxdWVzdCk7XG4gIH1cblxuICBjcmVhdGVUZW5hbnQoYm9keTogVGVuYW50TWFuYWdlbWVudC5BZGRSZXF1ZXN0KTogT2JzZXJ2YWJsZTxBQlAuQmFzaWNJdGVtPiB7XG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PFRlbmFudE1hbmFnZW1lbnQuQWRkUmVxdWVzdD4gPSB7XG4gICAgICBtZXRob2Q6ICdQT1NUJyxcbiAgICAgIHVybDogYC9hcGkvbXVsdGktdGVuYW5jeS90ZW5hbnRzYCxcbiAgICAgIGJvZHksXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxUZW5hbnRNYW5hZ2VtZW50LkFkZFJlcXVlc3QsIEFCUC5CYXNpY0l0ZW0+KHJlcXVlc3QpO1xuICB9XG5cbiAgdXBkYXRlVGVuYW50KGJvZHk6IFRlbmFudE1hbmFnZW1lbnQuVXBkYXRlUmVxdWVzdCk6IE9ic2VydmFibGU8QUJQLkJhc2ljSXRlbT4ge1xuICAgIGNvbnN0IHVybCA9IGAvYXBpL211bHRpLXRlbmFuY3kvdGVuYW50cy8ke2JvZHkuaWR9YDtcbiAgICBkZWxldGUgYm9keS5pZDtcblxuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxUZW5hbnRNYW5hZ2VtZW50LlVwZGF0ZVJlcXVlc3Q+ID0ge1xuICAgICAgbWV0aG9kOiAnUFVUJyxcbiAgICAgIHVybCxcbiAgICAgIGJvZHksXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxUZW5hbnRNYW5hZ2VtZW50LkFkZFJlcXVlc3QsIEFCUC5CYXNpY0l0ZW0+KHJlcXVlc3QpO1xuICB9XG5cbiAgZ2V0RGVmYXVsdENvbm5lY3Rpb25TdHJpbmcoaWQ6IHN0cmluZyk6IE9ic2VydmFibGU8c3RyaW5nPiB7XG4gICAgY29uc3QgdXJsID0gYC9hcGkvbXVsdGktdGVuYW5jeS90ZW5hbnRzLyR7aWR9L2RlZmF1bHQtY29ubmVjdGlvbi1zdHJpbmdgO1xuXG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PFRlbmFudE1hbmFnZW1lbnQuRGVmYXVsdENvbm5lY3Rpb25TdHJpbmdSZXF1ZXN0PiA9IHtcbiAgICAgIG1ldGhvZDogJ0dFVCcsXG4gICAgICByZXNwb25zZVR5cGU6IFJlc3QuUmVzcG9uc2VUeXBlLlRleHQsXG4gICAgICB1cmwsXG4gICAgfTtcbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8VGVuYW50TWFuYWdlbWVudC5EZWZhdWx0Q29ubmVjdGlvblN0cmluZ1JlcXVlc3QsIHN0cmluZz4ocmVxdWVzdCk7XG4gIH1cblxuICB1cGRhdGVEZWZhdWx0Q29ubmVjdGlvblN0cmluZyhwYXlsb2FkOiBUZW5hbnRNYW5hZ2VtZW50LkRlZmF1bHRDb25uZWN0aW9uU3RyaW5nUmVxdWVzdCk6IE9ic2VydmFibGU8YW55PiB7XG4gICAgY29uc3QgdXJsID0gYC9hcGkvbXVsdGktdGVuYW5jeS90ZW5hbnRzLyR7cGF5bG9hZC5pZH0vZGVmYXVsdC1jb25uZWN0aW9uLXN0cmluZ2A7XG5cbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8VGVuYW50TWFuYWdlbWVudC5EZWZhdWx0Q29ubmVjdGlvblN0cmluZ1JlcXVlc3Q+ID0ge1xuICAgICAgbWV0aG9kOiAnUFVUJyxcbiAgICAgIHVybCxcbiAgICAgIHBhcmFtczogeyBkZWZhdWx0Q29ubmVjdGlvblN0cmluZzogcGF5bG9hZC5kZWZhdWx0Q29ubmVjdGlvblN0cmluZyB9LFxuICAgIH07XG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PFRlbmFudE1hbmFnZW1lbnQuRGVmYXVsdENvbm5lY3Rpb25TdHJpbmdSZXF1ZXN0LCBhbnk+KHJlcXVlc3QpO1xuICB9XG5cbiAgZGVsZXRlRGVmYXVsdENvbm5lY3Rpb25TdHJpbmcoaWQ6IHN0cmluZyk6IE9ic2VydmFibGU8c3RyaW5nPiB7XG4gICAgY29uc3QgdXJsID0gYC9hcGkvbXVsdGktdGVuYW5jeS90ZW5hbnRzLyR7aWR9L2RlZmF1bHQtY29ubmVjdGlvbi1zdHJpbmdgO1xuXG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PFRlbmFudE1hbmFnZW1lbnQuRGVmYXVsdENvbm5lY3Rpb25TdHJpbmdSZXF1ZXN0PiA9IHtcbiAgICAgIG1ldGhvZDogJ0RFTEVURScsXG4gICAgICB1cmwsXG4gICAgfTtcbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8VGVuYW50TWFuYWdlbWVudC5EZWZhdWx0Q29ubmVjdGlvblN0cmluZ1JlcXVlc3QsIGFueT4ocmVxdWVzdCk7XG4gIH1cbn1cbiJdfQ==