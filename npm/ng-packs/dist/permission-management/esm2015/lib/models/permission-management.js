/**
 * @fileoverview added by tsickle
 * Generated from: lib/models/permission-management.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
export var PermissionManagement;
(function (PermissionManagement) {
    /**
     * @record
     */
    function State() { }
    PermissionManagement.State = State;
    if (false) {
        /** @type {?} */
        State.prototype.permissionRes;
    }
    /**
     * @record
     */
    function Response() { }
    PermissionManagement.Response = Response;
    if (false) {
        /** @type {?} */
        Response.prototype.entityDisplayName;
        /** @type {?} */
        Response.prototype.groups;
    }
    /**
     * @record
     */
    function Group() { }
    PermissionManagement.Group = Group;
    if (false) {
        /** @type {?} */
        Group.prototype.name;
        /** @type {?} */
        Group.prototype.displayName;
        /** @type {?} */
        Group.prototype.permissions;
    }
    /**
     * @record
     */
    function MinimumPermission() { }
    PermissionManagement.MinimumPermission = MinimumPermission;
    if (false) {
        /** @type {?} */
        MinimumPermission.prototype.name;
        /** @type {?} */
        MinimumPermission.prototype.isGranted;
    }
    /**
     * @record
     */
    function Permission() { }
    PermissionManagement.Permission = Permission;
    if (false) {
        /** @type {?} */
        Permission.prototype.displayName;
        /** @type {?} */
        Permission.prototype.parentName;
        /** @type {?} */
        Permission.prototype.allowedProviders;
        /** @type {?} */
        Permission.prototype.grantedProviders;
    }
    /**
     * @record
     */
    function GrantedProvider() { }
    PermissionManagement.GrantedProvider = GrantedProvider;
    if (false) {
        /** @type {?} */
        GrantedProvider.prototype.providerName;
        /** @type {?} */
        GrantedProvider.prototype.providerKey;
    }
    /**
     * @record
     */
    function UpdateRequest() { }
    PermissionManagement.UpdateRequest = UpdateRequest;
    if (false) {
        /** @type {?} */
        UpdateRequest.prototype.permissions;
    }
})(PermissionManagement || (PermissionManagement = {}));
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5wZXJtaXNzaW9uLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvbW9kZWxzL3Blcm1pc3Npb24tbWFuYWdlbWVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE1BQU0sS0FBVyxvQkFBb0IsQ0FvQ3BDO0FBcENELFdBQWlCLG9CQUFvQjs7OztJQUNuQyxvQkFFQzs7OztRQURDLDhCQUF3Qjs7Ozs7SUFHMUIsdUJBR0M7Ozs7UUFGQyxxQ0FBMEI7O1FBQzFCLDBCQUFnQjs7Ozs7SUFHbEIsb0JBSUM7Ozs7UUFIQyxxQkFBYTs7UUFDYiw0QkFBb0I7O1FBQ3BCLDRCQUEwQjs7Ozs7SUFHNUIsZ0NBR0M7Ozs7UUFGQyxpQ0FBYTs7UUFDYixzQ0FBbUI7Ozs7O0lBR3JCLHlCQUtDOzs7O1FBSkMsaUNBQW9COztRQUNwQixnQ0FBbUI7O1FBQ25CLHNDQUEyQjs7UUFDM0Isc0NBQW9DOzs7OztJQUd0Qyw4QkFHQzs7OztRQUZDLHVDQUFxQjs7UUFDckIsc0NBQW9COzs7OztJQUd0Qiw0QkFFQzs7OztRQURDLG9DQUFpQzs7QUFFckMsQ0FBQyxFQXBDZ0Isb0JBQW9CLEtBQXBCLG9CQUFvQixRQW9DcEMiLCJzb3VyY2VzQ29udGVudCI6WyJleHBvcnQgbmFtZXNwYWNlIFBlcm1pc3Npb25NYW5hZ2VtZW50IHtcclxuICBleHBvcnQgaW50ZXJmYWNlIFN0YXRlIHtcclxuICAgIHBlcm1pc3Npb25SZXM6IFJlc3BvbnNlO1xyXG4gIH1cclxuXHJcbiAgZXhwb3J0IGludGVyZmFjZSBSZXNwb25zZSB7XHJcbiAgICBlbnRpdHlEaXNwbGF5TmFtZTogc3RyaW5nO1xyXG4gICAgZ3JvdXBzOiBHcm91cFtdO1xyXG4gIH1cclxuXHJcbiAgZXhwb3J0IGludGVyZmFjZSBHcm91cCB7XHJcbiAgICBuYW1lOiBzdHJpbmc7XHJcbiAgICBkaXNwbGF5TmFtZTogc3RyaW5nO1xyXG4gICAgcGVybWlzc2lvbnM6IFBlcm1pc3Npb25bXTtcclxuICB9XHJcblxyXG4gIGV4cG9ydCBpbnRlcmZhY2UgTWluaW11bVBlcm1pc3Npb24ge1xyXG4gICAgbmFtZTogc3RyaW5nO1xyXG4gICAgaXNHcmFudGVkOiBib29sZWFuO1xyXG4gIH1cclxuXHJcbiAgZXhwb3J0IGludGVyZmFjZSBQZXJtaXNzaW9uIGV4dGVuZHMgTWluaW11bVBlcm1pc3Npb24ge1xyXG4gICAgZGlzcGxheU5hbWU6IHN0cmluZztcclxuICAgIHBhcmVudE5hbWU6IHN0cmluZztcclxuICAgIGFsbG93ZWRQcm92aWRlcnM6IHN0cmluZ1tdO1xyXG4gICAgZ3JhbnRlZFByb3ZpZGVyczogR3JhbnRlZFByb3ZpZGVyW107XHJcbiAgfVxyXG5cclxuICBleHBvcnQgaW50ZXJmYWNlIEdyYW50ZWRQcm92aWRlciB7XHJcbiAgICBwcm92aWRlck5hbWU6IHN0cmluZztcclxuICAgIHByb3ZpZGVyS2V5OiBzdHJpbmc7XHJcbiAgfVxyXG5cclxuICBleHBvcnQgaW50ZXJmYWNlIFVwZGF0ZVJlcXVlc3Qge1xyXG4gICAgcGVybWlzc2lvbnM6IE1pbmltdW1QZXJtaXNzaW9uW107XHJcbiAgfVxyXG59XHJcbiJdfQ==