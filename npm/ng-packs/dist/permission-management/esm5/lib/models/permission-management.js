/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5wZXJtaXNzaW9uLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvbW9kZWxzL3Blcm1pc3Npb24tbWFuYWdlbWVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsTUFBTSxLQUFXLG9CQUFvQixDQW9DcEM7QUFwQ0QsV0FBaUIsb0JBQW9COzs7O0lBQ25DLG9CQUVDOzs7O1FBREMsOEJBQXdCOzs7OztJQUcxQix1QkFHQzs7OztRQUZDLHFDQUEwQjs7UUFDMUIsMEJBQWdCOzs7OztJQUdsQixvQkFJQzs7OztRQUhDLHFCQUFhOztRQUNiLDRCQUFvQjs7UUFDcEIsNEJBQTBCOzs7OztJQUc1QixnQ0FHQzs7OztRQUZDLGlDQUFhOztRQUNiLHNDQUFtQjs7Ozs7SUFHckIseUJBS0M7Ozs7UUFKQyxpQ0FBb0I7O1FBQ3BCLGdDQUFtQjs7UUFDbkIsc0NBQTJCOztRQUMzQixzQ0FBb0M7Ozs7O0lBR3RDLDhCQUdDOzs7O1FBRkMsdUNBQXFCOztRQUNyQixzQ0FBb0I7Ozs7O0lBR3RCLDRCQUVDOzs7O1FBREMsb0NBQWlDOztBQUVyQyxDQUFDLEVBcENnQixvQkFBb0IsS0FBcEIsb0JBQW9CLFFBb0NwQyIsInNvdXJjZXNDb250ZW50IjpbImV4cG9ydCBuYW1lc3BhY2UgUGVybWlzc2lvbk1hbmFnZW1lbnQge1xyXG4gIGV4cG9ydCBpbnRlcmZhY2UgU3RhdGUge1xyXG4gICAgcGVybWlzc2lvblJlczogUmVzcG9uc2U7XHJcbiAgfVxyXG5cclxuICBleHBvcnQgaW50ZXJmYWNlIFJlc3BvbnNlIHtcclxuICAgIGVudGl0eURpc3BsYXlOYW1lOiBzdHJpbmc7XHJcbiAgICBncm91cHM6IEdyb3VwW107XHJcbiAgfVxyXG5cclxuICBleHBvcnQgaW50ZXJmYWNlIEdyb3VwIHtcclxuICAgIG5hbWU6IHN0cmluZztcclxuICAgIGRpc3BsYXlOYW1lOiBzdHJpbmc7XHJcbiAgICBwZXJtaXNzaW9uczogUGVybWlzc2lvbltdO1xyXG4gIH1cclxuXHJcbiAgZXhwb3J0IGludGVyZmFjZSBNaW5pbXVtUGVybWlzc2lvbiB7XHJcbiAgICBuYW1lOiBzdHJpbmc7XHJcbiAgICBpc0dyYW50ZWQ6IGJvb2xlYW47XHJcbiAgfVxyXG5cclxuICBleHBvcnQgaW50ZXJmYWNlIFBlcm1pc3Npb24gZXh0ZW5kcyBNaW5pbXVtUGVybWlzc2lvbiB7XHJcbiAgICBkaXNwbGF5TmFtZTogc3RyaW5nO1xyXG4gICAgcGFyZW50TmFtZTogc3RyaW5nO1xyXG4gICAgYWxsb3dlZFByb3ZpZGVyczogc3RyaW5nW107XHJcbiAgICBncmFudGVkUHJvdmlkZXJzOiBHcmFudGVkUHJvdmlkZXJbXTtcclxuICB9XHJcblxyXG4gIGV4cG9ydCBpbnRlcmZhY2UgR3JhbnRlZFByb3ZpZGVyIHtcclxuICAgIHByb3ZpZGVyTmFtZTogc3RyaW5nO1xyXG4gICAgcHJvdmlkZXJLZXk6IHN0cmluZztcclxuICB9XHJcblxyXG4gIGV4cG9ydCBpbnRlcmZhY2UgVXBkYXRlUmVxdWVzdCB7XHJcbiAgICBwZXJtaXNzaW9uczogTWluaW11bVBlcm1pc3Npb25bXTtcclxuICB9XHJcbn1cclxuIl19