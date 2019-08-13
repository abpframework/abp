/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
export class TenantManagementGet {
}
TenantManagementGet.type = '[TenantManagement] Get';
if (false) {
    /** @type {?} */
    TenantManagementGet.type;
}
export class TenantManagementGetById {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
TenantManagementGetById.type = '[TenantManagement] Get By Id';
if (false) {
    /** @type {?} */
    TenantManagementGetById.type;
    /** @type {?} */
    TenantManagementGetById.prototype.payload;
}
export class TenantManagementAdd {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
TenantManagementAdd.type = '[TenantManagement] Add';
if (false) {
    /** @type {?} */
    TenantManagementAdd.type;
    /** @type {?} */
    TenantManagementAdd.prototype.payload;
}
export class TenantManagementUpdate {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
TenantManagementUpdate.type = '[TenantManagement] Update';
if (false) {
    /** @type {?} */
    TenantManagementUpdate.type;
    /** @type {?} */
    TenantManagementUpdate.prototype.payload;
}
export class TenantManagementDelete {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
TenantManagementDelete.type = '[TenantManagement] Delete';
if (false) {
    /** @type {?} */
    TenantManagementDelete.type;
    /** @type {?} */
    TenantManagementDelete.prototype.payload;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQuYWN0aW9ucy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGVuYW50LW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvYWN0aW9ucy90ZW5hbnQtbWFuYWdlbWVudC5hY3Rpb25zLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFFQSxNQUFNLE9BQU8sbUJBQW1COztBQUNkLHdCQUFJLEdBQUcsd0JBQXdCLENBQUM7OztJQUFoRCx5QkFBZ0Q7O0FBR2xELE1BQU0sT0FBTyx1QkFBdUI7Ozs7SUFFbEMsWUFBbUIsT0FBZTtRQUFmLFlBQU8sR0FBUCxPQUFPLENBQVE7SUFBRyxDQUFDOztBQUR0Qiw0QkFBSSxHQUFHLDhCQUE4QixDQUFDOzs7SUFBdEQsNkJBQXNEOztJQUMxQywwQ0FBc0I7O0FBR3BDLE1BQU0sT0FBTyxtQkFBbUI7Ozs7SUFFOUIsWUFBbUIsT0FBb0M7UUFBcEMsWUFBTyxHQUFQLE9BQU8sQ0FBNkI7SUFBRyxDQUFDOztBQUQzQyx3QkFBSSxHQUFHLHdCQUF3QixDQUFDOzs7SUFBaEQseUJBQWdEOztJQUNwQyxzQ0FBMkM7O0FBR3pELE1BQU0sT0FBTyxzQkFBc0I7Ozs7SUFFakMsWUFBbUIsT0FBdUM7UUFBdkMsWUFBTyxHQUFQLE9BQU8sQ0FBZ0M7SUFBRyxDQUFDOztBQUQ5QywyQkFBSSxHQUFHLDJCQUEyQixDQUFDOzs7SUFBbkQsNEJBQW1EOztJQUN2Qyx5Q0FBOEM7O0FBRzVELE1BQU0sT0FBTyxzQkFBc0I7Ozs7SUFFakMsWUFBbUIsT0FBZTtRQUFmLFlBQU8sR0FBUCxPQUFPLENBQVE7SUFBRyxDQUFDOztBQUR0QiwyQkFBSSxHQUFHLDJCQUEyQixDQUFDOzs7SUFBbkQsNEJBQW1EOztJQUN2Qyx5Q0FBc0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzL3RlbmFudC1tYW5hZ2VtZW50JztcblxuZXhwb3J0IGNsYXNzIFRlbmFudE1hbmFnZW1lbnRHZXQge1xuICBzdGF0aWMgcmVhZG9ubHkgdHlwZSA9ICdbVGVuYW50TWFuYWdlbWVudF0gR2V0Jztcbn1cblxuZXhwb3J0IGNsYXNzIFRlbmFudE1hbmFnZW1lbnRHZXRCeUlkIHtcbiAgc3RhdGljIHJlYWRvbmx5IHR5cGUgPSAnW1RlbmFudE1hbmFnZW1lbnRdIEdldCBCeSBJZCc7XG4gIGNvbnN0cnVjdG9yKHB1YmxpYyBwYXlsb2FkOiBzdHJpbmcpIHt9XG59XG5cbmV4cG9ydCBjbGFzcyBUZW5hbnRNYW5hZ2VtZW50QWRkIHtcbiAgc3RhdGljIHJlYWRvbmx5IHR5cGUgPSAnW1RlbmFudE1hbmFnZW1lbnRdIEFkZCc7XG4gIGNvbnN0cnVjdG9yKHB1YmxpYyBwYXlsb2FkOiBUZW5hbnRNYW5hZ2VtZW50LkFkZFJlcXVlc3QpIHt9XG59XG5cbmV4cG9ydCBjbGFzcyBUZW5hbnRNYW5hZ2VtZW50VXBkYXRlIHtcbiAgc3RhdGljIHJlYWRvbmx5IHR5cGUgPSAnW1RlbmFudE1hbmFnZW1lbnRdIFVwZGF0ZSc7XG4gIGNvbnN0cnVjdG9yKHB1YmxpYyBwYXlsb2FkOiBUZW5hbnRNYW5hZ2VtZW50LlVwZGF0ZVJlcXVlc3QpIHt9XG59XG5cbmV4cG9ydCBjbGFzcyBUZW5hbnRNYW5hZ2VtZW50RGVsZXRlIHtcbiAgc3RhdGljIHJlYWRvbmx5IHR5cGUgPSAnW1RlbmFudE1hbmFnZW1lbnRdIERlbGV0ZSc7XG4gIGNvbnN0cnVjdG9yKHB1YmxpYyBwYXlsb2FkOiBzdHJpbmcpIHt9XG59XG4iXX0=