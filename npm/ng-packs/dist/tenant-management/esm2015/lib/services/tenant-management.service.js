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
    /**
     * @param {?} id
     * @return {?}
     */
    deleteDefaultConnectionString(id) {
        /** @type {?} */
        const url = `/api/multi-tenancy/tenant/${id}/defaultConnectionString`;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGVuYW50LW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvdGVuYW50LW1hbmFnZW1lbnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzQyxPQUFPLEVBQUUsV0FBVyxFQUFhLE1BQU0sY0FBYyxDQUFDOzs7QUFNdEQsTUFBTSxPQUFPLHVCQUF1Qjs7OztJQUNsQyxZQUFvQixJQUFpQjtRQUFqQixTQUFJLEdBQUosSUFBSSxDQUFhO0lBQUcsQ0FBQzs7OztJQUV6QyxHQUFHOztjQUNLLE9BQU8sR0FBdUI7WUFDbEMsTUFBTSxFQUFFLEtBQUs7WUFDYixHQUFHLEVBQUUsNEJBQTRCO1NBQ2xDO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBa0MsT0FBTyxDQUFDLENBQUM7SUFDckUsQ0FBQzs7Ozs7SUFFRCxPQUFPLENBQUMsRUFBVTs7Y0FDVixPQUFPLEdBQXVCO1lBQ2xDLE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRyxFQUFFLDhCQUE4QixFQUFFLEVBQUU7U0FDeEM7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFzQixPQUFPLENBQUMsQ0FBQztJQUN6RCxDQUFDOzs7OztJQUVELE1BQU0sQ0FBQyxFQUFVOztjQUNULE9BQU8sR0FBdUI7WUFDbEMsTUFBTSxFQUFFLFFBQVE7WUFDaEIsR0FBRyxFQUFFLDhCQUE4QixFQUFFLEVBQUU7U0FDeEM7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFhLE9BQU8sQ0FBQyxDQUFDO0lBQ2hELENBQUM7Ozs7O0lBRUQsR0FBRyxDQUFDLElBQWlDOztjQUM3QixPQUFPLEdBQThDO1lBQ3pELE1BQU0sRUFBRSxNQUFNO1lBQ2QsR0FBRyxFQUFFLDRCQUE0QjtZQUNqQyxJQUFJO1NBQ0w7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUE2QyxPQUFPLENBQUMsQ0FBQztJQUNoRixDQUFDOzs7OztJQUVELE1BQU0sQ0FBQyxJQUFvQzs7Y0FDbkMsR0FBRyxHQUFHLDhCQUE4QixJQUFJLENBQUMsRUFBRSxFQUFFO1FBQ25ELE9BQU8sSUFBSSxDQUFDLEVBQUUsQ0FBQzs7Y0FFVCxPQUFPLEdBQWlEO1lBQzVELE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRztZQUNILElBQUk7U0FDTDtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQTZDLE9BQU8sQ0FBQyxDQUFDO0lBQ2hGLENBQUM7Ozs7O0lBRUQsMEJBQTBCLENBQUMsRUFBVTs7Y0FDN0IsR0FBRyxHQUFHLDhCQUE4QixFQUFFLDBCQUEwQjs7Y0FFaEUsT0FBTyxHQUFrRTtZQUM3RSxNQUFNLEVBQUUsS0FBSztZQUNiLFlBQVksbUJBQXdCO1lBQ3BDLEdBQUc7U0FDSjtRQUNELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQTBELE9BQU8sQ0FBQyxDQUFDO0lBQzdGLENBQUM7Ozs7O0lBRUQsNkJBQTZCLENBQUMsT0FBd0Q7O2NBQzlFLEdBQUcsR0FBRyw4QkFBOEIsT0FBTyxDQUFDLEVBQUUsMEJBQTBCOztjQUV4RSxPQUFPLEdBQWtFO1lBQzdFLE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRztZQUNILE1BQU0sRUFBRSxFQUFFLHVCQUF1QixFQUFFLE9BQU8sQ0FBQyx1QkFBdUIsRUFBRTtTQUNyRTtRQUNELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQXVELE9BQU8sQ0FBQyxDQUFDO0lBQzFGLENBQUM7Ozs7O0lBRUQsNkJBQTZCLENBQUMsRUFBVTs7Y0FDaEMsR0FBRyxHQUFHLDZCQUE2QixFQUFFLDBCQUEwQjs7Y0FFL0QsT0FBTyxHQUFrRTtZQUM3RSxNQUFNLEVBQUUsUUFBUTtZQUNoQixHQUFHO1NBQ0o7UUFDRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUF1RCxPQUFPLENBQUMsQ0FBQztJQUMxRixDQUFDOzs7WUF0RkYsVUFBVSxTQUFDO2dCQUNWLFVBQVUsRUFBRSxNQUFNO2FBQ25COzs7O1lBTFEsV0FBVzs7Ozs7Ozs7SUFPTix1Q0FBeUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBSZXN0U2VydmljZSwgUmVzdCwgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvdGVuYW50LW1hbmFnZW1lbnQnO1xuXG5ASW5qZWN0YWJsZSh7XG4gIHByb3ZpZGVkSW46ICdyb290Jyxcbn0pXG5leHBvcnQgY2xhc3MgVGVuYW50TWFuYWdlbWVudFNlcnZpY2Uge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJlc3Q6IFJlc3RTZXJ2aWNlKSB7fVxuXG4gIGdldCgpOiBPYnNlcnZhYmxlPFRlbmFudE1hbmFnZW1lbnQuUmVzcG9uc2U+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8bnVsbD4gPSB7XG4gICAgICBtZXRob2Q6ICdHRVQnLFxuICAgICAgdXJsOiAnL2FwaS9tdWx0aS10ZW5hbmN5L3RlbmFudHMnLFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8bnVsbCwgVGVuYW50TWFuYWdlbWVudC5SZXNwb25zZT4ocmVxdWVzdCk7XG4gIH1cblxuICBnZXRCeUlkKGlkOiBzdHJpbmcpOiBPYnNlcnZhYmxlPEFCUC5CYXNpY0l0ZW0+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8bnVsbD4gPSB7XG4gICAgICBtZXRob2Q6ICdHRVQnLFxuICAgICAgdXJsOiBgL2FwaS9tdWx0aS10ZW5hbmN5L3RlbmFudHMvJHtpZH1gLFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8bnVsbCwgQUJQLkJhc2ljSXRlbT4ocmVxdWVzdCk7XG4gIH1cblxuICBkZWxldGUoaWQ6IHN0cmluZyk6IE9ic2VydmFibGU8bnVsbD4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxudWxsPiA9IHtcbiAgICAgIG1ldGhvZDogJ0RFTEVURScsXG4gICAgICB1cmw6IGAvYXBpL211bHRpLXRlbmFuY3kvdGVuYW50cy8ke2lkfWAsXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxudWxsLCBudWxsPihyZXF1ZXN0KTtcbiAgfVxuXG4gIGFkZChib2R5OiBUZW5hbnRNYW5hZ2VtZW50LkFkZFJlcXVlc3QpOiBPYnNlcnZhYmxlPEFCUC5CYXNpY0l0ZW0+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8VGVuYW50TWFuYWdlbWVudC5BZGRSZXF1ZXN0PiA9IHtcbiAgICAgIG1ldGhvZDogJ1BPU1QnLFxuICAgICAgdXJsOiBgL2FwaS9tdWx0aS10ZW5hbmN5L3RlbmFudHNgLFxuICAgICAgYm9keSxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PFRlbmFudE1hbmFnZW1lbnQuQWRkUmVxdWVzdCwgQUJQLkJhc2ljSXRlbT4ocmVxdWVzdCk7XG4gIH1cblxuICB1cGRhdGUoYm9keTogVGVuYW50TWFuYWdlbWVudC5VcGRhdGVSZXF1ZXN0KTogT2JzZXJ2YWJsZTxBQlAuQmFzaWNJdGVtPiB7XG4gICAgY29uc3QgdXJsID0gYC9hcGkvbXVsdGktdGVuYW5jeS90ZW5hbnRzLyR7Ym9keS5pZH1gO1xuICAgIGRlbGV0ZSBib2R5LmlkO1xuXG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PFRlbmFudE1hbmFnZW1lbnQuVXBkYXRlUmVxdWVzdD4gPSB7XG4gICAgICBtZXRob2Q6ICdQVVQnLFxuICAgICAgdXJsLFxuICAgICAgYm9keSxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PFRlbmFudE1hbmFnZW1lbnQuQWRkUmVxdWVzdCwgQUJQLkJhc2ljSXRlbT4ocmVxdWVzdCk7XG4gIH1cblxuICBnZXREZWZhdWx0Q29ubmVjdGlvblN0cmluZyhpZDogc3RyaW5nKTogT2JzZXJ2YWJsZTxzdHJpbmc+IHtcbiAgICBjb25zdCB1cmwgPSBgL2FwaS9tdWx0aS10ZW5hbmN5L3RlbmFudHMvJHtpZH0vZGVmYXVsdENvbm5lY3Rpb25TdHJpbmdgO1xuXG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PFRlbmFudE1hbmFnZW1lbnQuRGVmYXVsdENvbm5lY3Rpb25TdHJpbmdSZXF1ZXN0PiA9IHtcbiAgICAgIG1ldGhvZDogJ0dFVCcsXG4gICAgICByZXNwb25zZVR5cGU6IFJlc3QuUmVzcG9uc2VUeXBlLlRleHQsXG4gICAgICB1cmwsXG4gICAgfTtcbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8VGVuYW50TWFuYWdlbWVudC5EZWZhdWx0Q29ubmVjdGlvblN0cmluZ1JlcXVlc3QsIHN0cmluZz4ocmVxdWVzdCk7XG4gIH1cblxuICB1cGRhdGVEZWZhdWx0Q29ubmVjdGlvblN0cmluZyhwYXlsb2FkOiBUZW5hbnRNYW5hZ2VtZW50LkRlZmF1bHRDb25uZWN0aW9uU3RyaW5nUmVxdWVzdCk6IE9ic2VydmFibGU8YW55PiB7XG4gICAgY29uc3QgdXJsID0gYC9hcGkvbXVsdGktdGVuYW5jeS90ZW5hbnRzLyR7cGF5bG9hZC5pZH0vZGVmYXVsdENvbm5lY3Rpb25TdHJpbmdgO1xuXG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PFRlbmFudE1hbmFnZW1lbnQuRGVmYXVsdENvbm5lY3Rpb25TdHJpbmdSZXF1ZXN0PiA9IHtcbiAgICAgIG1ldGhvZDogJ1BVVCcsXG4gICAgICB1cmwsXG4gICAgICBwYXJhbXM6IHsgZGVmYXVsdENvbm5lY3Rpb25TdHJpbmc6IHBheWxvYWQuZGVmYXVsdENvbm5lY3Rpb25TdHJpbmcgfSxcbiAgICB9O1xuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxUZW5hbnRNYW5hZ2VtZW50LkRlZmF1bHRDb25uZWN0aW9uU3RyaW5nUmVxdWVzdCwgYW55PihyZXF1ZXN0KTtcbiAgfVxuXG4gIGRlbGV0ZURlZmF1bHRDb25uZWN0aW9uU3RyaW5nKGlkOiBzdHJpbmcpOiBPYnNlcnZhYmxlPHN0cmluZz4ge1xuICAgIGNvbnN0IHVybCA9IGAvYXBpL211bHRpLXRlbmFuY3kvdGVuYW50LyR7aWR9L2RlZmF1bHRDb25uZWN0aW9uU3RyaW5nYDtcblxuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxUZW5hbnRNYW5hZ2VtZW50LkRlZmF1bHRDb25uZWN0aW9uU3RyaW5nUmVxdWVzdD4gPSB7XG4gICAgICBtZXRob2Q6ICdERUxFVEUnLFxuICAgICAgdXJsLFxuICAgIH07XG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PFRlbmFudE1hbmFnZW1lbnQuRGVmYXVsdENvbm5lY3Rpb25TdHJpbmdSZXF1ZXN0LCBhbnk+KHJlcXVlc3QpO1xuICB9XG59XG4iXX0=