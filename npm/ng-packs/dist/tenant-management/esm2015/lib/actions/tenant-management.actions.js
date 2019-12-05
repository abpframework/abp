/**
 * @fileoverview added by tsickle
 * Generated from: lib/actions/tenant-management.actions.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQuYWN0aW9ucy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGVuYW50LW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvYWN0aW9ucy90ZW5hbnQtbWFuYWdlbWVudC5hY3Rpb25zLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBR0EsTUFBTSxPQUFPLFVBQVU7Ozs7SUFFckIsWUFBbUIsT0FBNkI7UUFBN0IsWUFBTyxHQUFQLE9BQU8sQ0FBc0I7SUFBRyxDQUFDOztBQURwQyxlQUFJLEdBQUcsK0JBQStCLENBQUM7OztJQUF2RCxnQkFBdUQ7O0lBQzNDLDZCQUFvQzs7QUFHbEQsTUFBTSxPQUFPLGFBQWE7Ozs7SUFFeEIsWUFBbUIsT0FBZTtRQUFmLFlBQU8sR0FBUCxPQUFPLENBQVE7SUFBRyxDQUFDOztBQUR0QixrQkFBSSxHQUFHLHFDQUFxQyxDQUFDOzs7SUFBN0QsbUJBQTZEOztJQUNqRCxnQ0FBc0I7O0FBR3BDLE1BQU0sT0FBTyxZQUFZOzs7O0lBRXZCLFlBQW1CLE9BQW9DO1FBQXBDLFlBQU8sR0FBUCxPQUFPLENBQTZCO0lBQUcsQ0FBQzs7QUFEM0MsaUJBQUksR0FBRyxrQ0FBa0MsQ0FBQzs7O0lBQTFELGtCQUEwRDs7SUFDOUMsK0JBQTJDOztBQUd6RCxNQUFNLE9BQU8sWUFBWTs7OztJQUV2QixZQUFtQixPQUF1QztRQUF2QyxZQUFPLEdBQVAsT0FBTyxDQUFnQztJQUFHLENBQUM7O0FBRDlDLGlCQUFJLEdBQUcsa0NBQWtDLENBQUM7OztJQUExRCxrQkFBMEQ7O0lBQzlDLCtCQUE4Qzs7QUFHNUQsTUFBTSxPQUFPLFlBQVk7Ozs7SUFFdkIsWUFBbUIsT0FBZTtRQUFmLFlBQU8sR0FBUCxPQUFPLENBQVE7SUFBRyxDQUFDOztBQUR0QixpQkFBSSxHQUFHLGtDQUFrQyxDQUFDOzs7SUFBMUQsa0JBQTBEOztJQUM5QywrQkFBc0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzL3RlbmFudC1tYW5hZ2VtZW50JztcbmltcG9ydCB7IEFCUCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5cbmV4cG9ydCBjbGFzcyBHZXRUZW5hbnRzIHtcbiAgc3RhdGljIHJlYWRvbmx5IHR5cGUgPSAnW1RlbmFudE1hbmFnZW1lbnRdIEdldCBUZW5hbnQnO1xuICBjb25zdHJ1Y3RvcihwdWJsaWMgcGF5bG9hZD86IEFCUC5QYWdlUXVlcnlQYXJhbXMpIHt9XG59XG5cbmV4cG9ydCBjbGFzcyBHZXRUZW5hbnRCeUlkIHtcbiAgc3RhdGljIHJlYWRvbmx5IHR5cGUgPSAnW1RlbmFudE1hbmFnZW1lbnRdIEdldCBUZW5hbnQgQnkgSWQnO1xuICBjb25zdHJ1Y3RvcihwdWJsaWMgcGF5bG9hZDogc3RyaW5nKSB7fVxufVxuXG5leHBvcnQgY2xhc3MgQ3JlYXRlVGVuYW50IHtcbiAgc3RhdGljIHJlYWRvbmx5IHR5cGUgPSAnW1RlbmFudE1hbmFnZW1lbnRdIENyZWF0ZSBUZW5hbnQnO1xuICBjb25zdHJ1Y3RvcihwdWJsaWMgcGF5bG9hZDogVGVuYW50TWFuYWdlbWVudC5BZGRSZXF1ZXN0KSB7fVxufVxuXG5leHBvcnQgY2xhc3MgVXBkYXRlVGVuYW50IHtcbiAgc3RhdGljIHJlYWRvbmx5IHR5cGUgPSAnW1RlbmFudE1hbmFnZW1lbnRdIFVwZGF0ZSBUZW5hbnQnO1xuICBjb25zdHJ1Y3RvcihwdWJsaWMgcGF5bG9hZDogVGVuYW50TWFuYWdlbWVudC5VcGRhdGVSZXF1ZXN0KSB7fVxufVxuXG5leHBvcnQgY2xhc3MgRGVsZXRlVGVuYW50IHtcbiAgc3RhdGljIHJlYWRvbmx5IHR5cGUgPSAnW1RlbmFudE1hbmFnZW1lbnRdIERlbGV0ZSBUZW5hbnQnO1xuICBjb25zdHJ1Y3RvcihwdWJsaWMgcGF5bG9hZDogc3RyaW5nKSB7fVxufVxuIl19