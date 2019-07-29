/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5wZXJtaXNzaW9uLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvbW9kZWxzL3Blcm1pc3Npb24tbWFuYWdlbWVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsTUFBTSxLQUFXLG9CQUFvQixDQW9DcEM7QUFwQ0QsV0FBaUIsb0JBQW9COzs7O0lBQ25DLG9CQUVDOzs7O1FBREMsOEJBQXdCOzs7OztJQUcxQix1QkFHQzs7OztRQUZDLHFDQUEwQjs7UUFDMUIsMEJBQWdCOzs7OztJQUdsQixvQkFJQzs7OztRQUhDLHFCQUFhOztRQUNiLDRCQUFvQjs7UUFDcEIsNEJBQTBCOzs7OztJQUc1QixnQ0FHQzs7OztRQUZDLGlDQUFhOztRQUNiLHNDQUFtQjs7Ozs7SUFHckIseUJBS0M7Ozs7UUFKQyxpQ0FBb0I7O1FBQ3BCLGdDQUFtQjs7UUFDbkIsc0NBQTJCOztRQUMzQixzQ0FBb0M7Ozs7O0lBR3RDLDhCQUdDOzs7O1FBRkMsdUNBQXFCOztRQUNyQixzQ0FBb0I7Ozs7O0lBR3RCLDRCQUVDOzs7O1FBREMsb0NBQWlDOztBQUVyQyxDQUFDLEVBcENnQixvQkFBb0IsS0FBcEIsb0JBQW9CLFFBb0NwQyIsInNvdXJjZXNDb250ZW50IjpbImV4cG9ydCBuYW1lc3BhY2UgUGVybWlzc2lvbk1hbmFnZW1lbnQge1xuICBleHBvcnQgaW50ZXJmYWNlIFN0YXRlIHtcbiAgICBwZXJtaXNzaW9uUmVzOiBSZXNwb25zZTtcbiAgfVxuXG4gIGV4cG9ydCBpbnRlcmZhY2UgUmVzcG9uc2Uge1xuICAgIGVudGl0eURpc3BsYXlOYW1lOiBzdHJpbmc7XG4gICAgZ3JvdXBzOiBHcm91cFtdO1xuICB9XG5cbiAgZXhwb3J0IGludGVyZmFjZSBHcm91cCB7XG4gICAgbmFtZTogc3RyaW5nO1xuICAgIGRpc3BsYXlOYW1lOiBzdHJpbmc7XG4gICAgcGVybWlzc2lvbnM6IFBlcm1pc3Npb25bXTtcbiAgfVxuXG4gIGV4cG9ydCBpbnRlcmZhY2UgTWluaW11bVBlcm1pc3Npb24ge1xuICAgIG5hbWU6IHN0cmluZztcbiAgICBpc0dyYW50ZWQ6IGJvb2xlYW47XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIFBlcm1pc3Npb24gZXh0ZW5kcyBNaW5pbXVtUGVybWlzc2lvbiB7XG4gICAgZGlzcGxheU5hbWU6IHN0cmluZztcbiAgICBwYXJlbnROYW1lOiBzdHJpbmc7XG4gICAgYWxsb3dlZFByb3ZpZGVyczogc3RyaW5nW107XG4gICAgZ3JhbnRlZFByb3ZpZGVyczogR3JhbnRlZFByb3ZpZGVyW107XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIEdyYW50ZWRQcm92aWRlciB7XG4gICAgcHJvdmlkZXJOYW1lOiBzdHJpbmc7XG4gICAgcHJvdmlkZXJLZXk6IHN0cmluZztcbiAgfVxuXG4gIGV4cG9ydCBpbnRlcmZhY2UgVXBkYXRlUmVxdWVzdCB7XG4gICAgcGVybWlzc2lvbnM6IE1pbmltdW1QZXJtaXNzaW9uW107XG4gIH1cbn1cbiJdfQ==