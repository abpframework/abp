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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5wZXJtaXNzaW9uLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvbW9kZWxzL3Blcm1pc3Npb24tbWFuYWdlbWVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE1BQU0sS0FBVyxvQkFBb0IsQ0FvQ3BDO0FBcENELFdBQWlCLG9CQUFvQjs7OztJQUNuQyxvQkFFQzs7OztRQURDLDhCQUF3Qjs7Ozs7SUFHMUIsdUJBR0M7Ozs7UUFGQyxxQ0FBMEI7O1FBQzFCLDBCQUFnQjs7Ozs7SUFHbEIsb0JBSUM7Ozs7UUFIQyxxQkFBYTs7UUFDYiw0QkFBb0I7O1FBQ3BCLDRCQUEwQjs7Ozs7SUFHNUIsZ0NBR0M7Ozs7UUFGQyxpQ0FBYTs7UUFDYixzQ0FBbUI7Ozs7O0lBR3JCLHlCQUtDOzs7O1FBSkMsaUNBQW9COztRQUNwQixnQ0FBbUI7O1FBQ25CLHNDQUEyQjs7UUFDM0Isc0NBQW9DOzs7OztJQUd0Qyw4QkFHQzs7OztRQUZDLHVDQUFxQjs7UUFDckIsc0NBQW9COzs7OztJQUd0Qiw0QkFFQzs7OztRQURDLG9DQUFpQzs7QUFFckMsQ0FBQyxFQXBDZ0Isb0JBQW9CLEtBQXBCLG9CQUFvQixRQW9DcEMiLCJzb3VyY2VzQ29udGVudCI6WyJleHBvcnQgbmFtZXNwYWNlIFBlcm1pc3Npb25NYW5hZ2VtZW50IHtcbiAgZXhwb3J0IGludGVyZmFjZSBTdGF0ZSB7XG4gICAgcGVybWlzc2lvblJlczogUmVzcG9uc2U7XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIFJlc3BvbnNlIHtcbiAgICBlbnRpdHlEaXNwbGF5TmFtZTogc3RyaW5nO1xuICAgIGdyb3VwczogR3JvdXBbXTtcbiAgfVxuXG4gIGV4cG9ydCBpbnRlcmZhY2UgR3JvdXAge1xuICAgIG5hbWU6IHN0cmluZztcbiAgICBkaXNwbGF5TmFtZTogc3RyaW5nO1xuICAgIHBlcm1pc3Npb25zOiBQZXJtaXNzaW9uW107XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIE1pbmltdW1QZXJtaXNzaW9uIHtcbiAgICBuYW1lOiBzdHJpbmc7XG4gICAgaXNHcmFudGVkOiBib29sZWFuO1xuICB9XG5cbiAgZXhwb3J0IGludGVyZmFjZSBQZXJtaXNzaW9uIGV4dGVuZHMgTWluaW11bVBlcm1pc3Npb24ge1xuICAgIGRpc3BsYXlOYW1lOiBzdHJpbmc7XG4gICAgcGFyZW50TmFtZTogc3RyaW5nO1xuICAgIGFsbG93ZWRQcm92aWRlcnM6IHN0cmluZ1tdO1xuICAgIGdyYW50ZWRQcm92aWRlcnM6IEdyYW50ZWRQcm92aWRlcltdO1xuICB9XG5cbiAgZXhwb3J0IGludGVyZmFjZSBHcmFudGVkUHJvdmlkZXIge1xuICAgIHByb3ZpZGVyTmFtZTogc3RyaW5nO1xuICAgIHByb3ZpZGVyS2V5OiBzdHJpbmc7XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIFVwZGF0ZVJlcXVlc3Qge1xuICAgIHBlcm1pc3Npb25zOiBNaW5pbXVtUGVybWlzc2lvbltdO1xuICB9XG59XG4iXX0=