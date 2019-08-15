/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
export class GetTenants {
    /**
     * @param {?=} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
GetTenants.type = '[TenantManagement] Get Tenant';
if (false) {
    /** @type {?} */
    GetTenants.type;
    /** @type {?} */
    GetTenants.prototype.payload;
}
export class GetTenantById {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
GetTenantById.type = '[TenantManagement] Get Tenant By Id';
if (false) {
    /** @type {?} */
    GetTenantById.type;
    /** @type {?} */
    GetTenantById.prototype.payload;
}
export class CreateTenant {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
CreateTenant.type = '[TenantManagement] Create Tenant';
if (false) {
    /** @type {?} */
    CreateTenant.type;
    /** @type {?} */
    CreateTenant.prototype.payload;
}
export class UpdateTenant {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
UpdateTenant.type = '[TenantManagement] Update Tenant';
if (false) {
    /** @type {?} */
    UpdateTenant.type;
    /** @type {?} */
    UpdateTenant.prototype.payload;
}
export class DeleteTenant {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
DeleteTenant.type = '[TenantManagement] Delete Tenant';
if (false) {
    /** @type {?} */
    DeleteTenant.type;
    /** @type {?} */
    DeleteTenant.prototype.payload;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQuYWN0aW9ucy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGVuYW50LW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvYWN0aW9ucy90ZW5hbnQtbWFuYWdlbWVudC5hY3Rpb25zLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFHQSxNQUFNLE9BQU8sVUFBVTs7OztJQUVyQixZQUFtQixPQUE2QjtRQUE3QixZQUFPLEdBQVAsT0FBTyxDQUFzQjtJQUFHLENBQUM7O0FBRHBDLGVBQUksR0FBRywrQkFBK0IsQ0FBQzs7O0lBQXZELGdCQUF1RDs7SUFDM0MsNkJBQW9DOztBQUdsRCxNQUFNLE9BQU8sYUFBYTs7OztJQUV4QixZQUFtQixPQUFlO1FBQWYsWUFBTyxHQUFQLE9BQU8sQ0FBUTtJQUFHLENBQUM7O0FBRHRCLGtCQUFJLEdBQUcscUNBQXFDLENBQUM7OztJQUE3RCxtQkFBNkQ7O0lBQ2pELGdDQUFzQjs7QUFHcEMsTUFBTSxPQUFPLFlBQVk7Ozs7SUFFdkIsWUFBbUIsT0FBb0M7UUFBcEMsWUFBTyxHQUFQLE9BQU8sQ0FBNkI7SUFBRyxDQUFDOztBQUQzQyxpQkFBSSxHQUFHLGtDQUFrQyxDQUFDOzs7SUFBMUQsa0JBQTBEOztJQUM5QywrQkFBMkM7O0FBR3pELE1BQU0sT0FBTyxZQUFZOzs7O0lBRXZCLFlBQW1CLE9BQXVDO1FBQXZDLFlBQU8sR0FBUCxPQUFPLENBQWdDO0lBQUcsQ0FBQzs7QUFEOUMsaUJBQUksR0FBRyxrQ0FBa0MsQ0FBQzs7O0lBQTFELGtCQUEwRDs7SUFDOUMsK0JBQThDOztBQUc1RCxNQUFNLE9BQU8sWUFBWTs7OztJQUV2QixZQUFtQixPQUFlO1FBQWYsWUFBTyxHQUFQLE9BQU8sQ0FBUTtJQUFHLENBQUM7O0FBRHRCLGlCQUFJLEdBQUcsa0NBQWtDLENBQUM7OztJQUExRCxrQkFBMEQ7O0lBQzlDLCtCQUFzQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvdGVuYW50LW1hbmFnZW1lbnQnO1xuaW1wb3J0IHsgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcblxuZXhwb3J0IGNsYXNzIEdldFRlbmFudHMge1xuICBzdGF0aWMgcmVhZG9ubHkgdHlwZSA9ICdbVGVuYW50TWFuYWdlbWVudF0gR2V0IFRlbmFudCc7XG4gIGNvbnN0cnVjdG9yKHB1YmxpYyBwYXlsb2FkPzogQUJQLlBhZ2VRdWVyeVBhcmFtcykge31cbn1cblxuZXhwb3J0IGNsYXNzIEdldFRlbmFudEJ5SWQge1xuICBzdGF0aWMgcmVhZG9ubHkgdHlwZSA9ICdbVGVuYW50TWFuYWdlbWVudF0gR2V0IFRlbmFudCBCeSBJZCc7XG4gIGNvbnN0cnVjdG9yKHB1YmxpYyBwYXlsb2FkOiBzdHJpbmcpIHt9XG59XG5cbmV4cG9ydCBjbGFzcyBDcmVhdGVUZW5hbnQge1xuICBzdGF0aWMgcmVhZG9ubHkgdHlwZSA9ICdbVGVuYW50TWFuYWdlbWVudF0gQ3JlYXRlIFRlbmFudCc7XG4gIGNvbnN0cnVjdG9yKHB1YmxpYyBwYXlsb2FkOiBUZW5hbnRNYW5hZ2VtZW50LkFkZFJlcXVlc3QpIHt9XG59XG5cbmV4cG9ydCBjbGFzcyBVcGRhdGVUZW5hbnQge1xuICBzdGF0aWMgcmVhZG9ubHkgdHlwZSA9ICdbVGVuYW50TWFuYWdlbWVudF0gVXBkYXRlIFRlbmFudCc7XG4gIGNvbnN0cnVjdG9yKHB1YmxpYyBwYXlsb2FkOiBUZW5hbnRNYW5hZ2VtZW50LlVwZGF0ZVJlcXVlc3QpIHt9XG59XG5cbmV4cG9ydCBjbGFzcyBEZWxldGVUZW5hbnQge1xuICBzdGF0aWMgcmVhZG9ubHkgdHlwZSA9ICdbVGVuYW50TWFuYWdlbWVudF0gRGVsZXRlIFRlbmFudCc7XG4gIGNvbnN0cnVjdG9yKHB1YmxpYyBwYXlsb2FkOiBzdHJpbmcpIHt9XG59XG4iXX0=