/**
 * @fileoverview added by tsickle
 * Generated from: lib/actions/tenant-management.actions.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var GetTenants = /** @class */ (function () {
    function GetTenants(payload) {
        this.payload = payload;
    }
    GetTenants.type = '[TenantManagement] Get Tenant';
    return GetTenants;
}());
export { GetTenants };
if (false) {
    /** @type {?} */
    GetTenants.type;
    /** @type {?} */
    GetTenants.prototype.payload;
}
var GetTenantById = /** @class */ (function () {
    function GetTenantById(payload) {
        this.payload = payload;
    }
    GetTenantById.type = '[TenantManagement] Get Tenant By Id';
    return GetTenantById;
}());
export { GetTenantById };
if (false) {
    /** @type {?} */
    GetTenantById.type;
    /** @type {?} */
    GetTenantById.prototype.payload;
}
var CreateTenant = /** @class */ (function () {
    function CreateTenant(payload) {
        this.payload = payload;
    }
    CreateTenant.type = '[TenantManagement] Create Tenant';
    return CreateTenant;
}());
export { CreateTenant };
if (false) {
    /** @type {?} */
    CreateTenant.type;
    /** @type {?} */
    CreateTenant.prototype.payload;
}
var UpdateTenant = /** @class */ (function () {
    function UpdateTenant(payload) {
        this.payload = payload;
    }
    UpdateTenant.type = '[TenantManagement] Update Tenant';
    return UpdateTenant;
}());
export { UpdateTenant };
if (false) {
    /** @type {?} */
    UpdateTenant.type;
    /** @type {?} */
    UpdateTenant.prototype.payload;
}
var DeleteTenant = /** @class */ (function () {
    function DeleteTenant(payload) {
        this.payload = payload;
    }
    DeleteTenant.type = '[TenantManagement] Delete Tenant';
    return DeleteTenant;
}());
export { DeleteTenant };
if (false) {
    /** @type {?} */
    DeleteTenant.type;
    /** @type {?} */
    DeleteTenant.prototype.payload;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQuYWN0aW9ucy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGVuYW50LW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvYWN0aW9ucy90ZW5hbnQtbWFuYWdlbWVudC5hY3Rpb25zLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBR0E7SUFFRSxvQkFBbUIsT0FBNkI7UUFBN0IsWUFBTyxHQUFQLE9BQU8sQ0FBc0I7SUFBRyxDQUFDO0lBRHBDLGVBQUksR0FBRywrQkFBK0IsQ0FBQztJQUV6RCxpQkFBQztDQUFBLEFBSEQsSUFHQztTQUhZLFVBQVU7OztJQUNyQixnQkFBdUQ7O0lBQzNDLDZCQUFvQzs7QUFHbEQ7SUFFRSx1QkFBbUIsT0FBZTtRQUFmLFlBQU8sR0FBUCxPQUFPLENBQVE7SUFBRyxDQUFDO0lBRHRCLGtCQUFJLEdBQUcscUNBQXFDLENBQUM7SUFFL0Qsb0JBQUM7Q0FBQSxBQUhELElBR0M7U0FIWSxhQUFhOzs7SUFDeEIsbUJBQTZEOztJQUNqRCxnQ0FBc0I7O0FBR3BDO0lBRUUsc0JBQW1CLE9BQW9DO1FBQXBDLFlBQU8sR0FBUCxPQUFPLENBQTZCO0lBQUcsQ0FBQztJQUQzQyxpQkFBSSxHQUFHLGtDQUFrQyxDQUFDO0lBRTVELG1CQUFDO0NBQUEsQUFIRCxJQUdDO1NBSFksWUFBWTs7O0lBQ3ZCLGtCQUEwRDs7SUFDOUMsK0JBQTJDOztBQUd6RDtJQUVFLHNCQUFtQixPQUF1QztRQUF2QyxZQUFPLEdBQVAsT0FBTyxDQUFnQztJQUFHLENBQUM7SUFEOUMsaUJBQUksR0FBRyxrQ0FBa0MsQ0FBQztJQUU1RCxtQkFBQztDQUFBLEFBSEQsSUFHQztTQUhZLFlBQVk7OztJQUN2QixrQkFBMEQ7O0lBQzlDLCtCQUE4Qzs7QUFHNUQ7SUFFRSxzQkFBbUIsT0FBZTtRQUFmLFlBQU8sR0FBUCxPQUFPLENBQVE7SUFBRyxDQUFDO0lBRHRCLGlCQUFJLEdBQUcsa0NBQWtDLENBQUM7SUFFNUQsbUJBQUM7Q0FBQSxBQUhELElBR0M7U0FIWSxZQUFZOzs7SUFDdkIsa0JBQTBEOztJQUM5QywrQkFBc0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzL3RlbmFudC1tYW5hZ2VtZW50JztcbmltcG9ydCB7IEFCUCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5cbmV4cG9ydCBjbGFzcyBHZXRUZW5hbnRzIHtcbiAgc3RhdGljIHJlYWRvbmx5IHR5cGUgPSAnW1RlbmFudE1hbmFnZW1lbnRdIEdldCBUZW5hbnQnO1xuICBjb25zdHJ1Y3RvcihwdWJsaWMgcGF5bG9hZD86IEFCUC5QYWdlUXVlcnlQYXJhbXMpIHt9XG59XG5cbmV4cG9ydCBjbGFzcyBHZXRUZW5hbnRCeUlkIHtcbiAgc3RhdGljIHJlYWRvbmx5IHR5cGUgPSAnW1RlbmFudE1hbmFnZW1lbnRdIEdldCBUZW5hbnQgQnkgSWQnO1xuICBjb25zdHJ1Y3RvcihwdWJsaWMgcGF5bG9hZDogc3RyaW5nKSB7fVxufVxuXG5leHBvcnQgY2xhc3MgQ3JlYXRlVGVuYW50IHtcbiAgc3RhdGljIHJlYWRvbmx5IHR5cGUgPSAnW1RlbmFudE1hbmFnZW1lbnRdIENyZWF0ZSBUZW5hbnQnO1xuICBjb25zdHJ1Y3RvcihwdWJsaWMgcGF5bG9hZDogVGVuYW50TWFuYWdlbWVudC5BZGRSZXF1ZXN0KSB7fVxufVxuXG5leHBvcnQgY2xhc3MgVXBkYXRlVGVuYW50IHtcbiAgc3RhdGljIHJlYWRvbmx5IHR5cGUgPSAnW1RlbmFudE1hbmFnZW1lbnRdIFVwZGF0ZSBUZW5hbnQnO1xuICBjb25zdHJ1Y3RvcihwdWJsaWMgcGF5bG9hZDogVGVuYW50TWFuYWdlbWVudC5VcGRhdGVSZXF1ZXN0KSB7fVxufVxuXG5leHBvcnQgY2xhc3MgRGVsZXRlVGVuYW50IHtcbiAgc3RhdGljIHJlYWRvbmx5IHR5cGUgPSAnW1RlbmFudE1hbmFnZW1lbnRdIERlbGV0ZSBUZW5hbnQnO1xuICBjb25zdHJ1Y3RvcihwdWJsaWMgcGF5bG9hZDogc3RyaW5nKSB7fVxufVxuIl19