/**
 * @fileoverview added by tsickle
 * Generated from: lib/models/identity.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
export var Identity;
(function (Identity) {
    /**
     * @record
     */
    function State() { }
    Identity.State = State;
    if (false) {
        /** @type {?} */
        State.prototype.roles;
        /** @type {?} */
        State.prototype.users;
        /** @type {?} */
        State.prototype.selectedRole;
        /** @type {?} */
        State.prototype.selectedUser;
        /** @type {?} */
        State.prototype.selectedUserRoles;
    }
    /**
     * @record
     */
    function RoleSaveRequest() { }
    Identity.RoleSaveRequest = RoleSaveRequest;
    if (false) {
        /** @type {?} */
        RoleSaveRequest.prototype.name;
        /** @type {?} */
        RoleSaveRequest.prototype.isDefault;
        /** @type {?} */
        RoleSaveRequest.prototype.isPublic;
    }
    /**
     * @record
     */
    function RoleItem() { }
    Identity.RoleItem = RoleItem;
    if (false) {
        /** @type {?} */
        RoleItem.prototype.isStatic;
        /** @type {?} */
        RoleItem.prototype.concurrencyStamp;
        /** @type {?} */
        RoleItem.prototype.id;
    }
    /**
     * @record
     */
    function UserItem() { }
    Identity.UserItem = UserItem;
    if (false) {
        /** @type {?} */
        UserItem.prototype.tenantId;
        /** @type {?} */
        UserItem.prototype.emailConfirmed;
        /** @type {?} */
        UserItem.prototype.phoneNumberConfirmed;
        /** @type {?} */
        UserItem.prototype.isLockedOut;
        /** @type {?} */
        UserItem.prototype.concurrencyStamp;
        /** @type {?} */
        UserItem.prototype.id;
    }
    /**
     * @record
     */
    function User() { }
    Identity.User = User;
    if (false) {
        /** @type {?} */
        User.prototype.userName;
        /** @type {?} */
        User.prototype.name;
        /** @type {?} */
        User.prototype.surname;
        /** @type {?} */
        User.prototype.email;
        /** @type {?} */
        User.prototype.phoneNumber;
        /** @type {?} */
        User.prototype.twoFactorEnabled;
        /** @type {?} */
        User.prototype.lockoutEnabled;
    }
    /**
     * @record
     */
    function UserSaveRequest() { }
    Identity.UserSaveRequest = UserSaveRequest;
    if (false) {
        /** @type {?} */
        UserSaveRequest.prototype.password;
        /** @type {?} */
        UserSaveRequest.prototype.roleNames;
    }
})(Identity || (Identity = {}));
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL21vZGVscy9pZGVudGl0eS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUVBLE1BQU0sS0FBVyxRQUFRLENBZ0R4QjtBQWhERCxXQUFpQixRQUFROzs7O0lBQ3ZCLG9CQU1DOzs7O1FBTEMsc0JBQW9COztRQUNwQixzQkFBb0I7O1FBQ3BCLDZCQUF1Qjs7UUFDdkIsNkJBQXVCOztRQUN2QixrQ0FBOEI7Ozs7O0lBS2hDLDhCQUlDOzs7O1FBSEMsK0JBQWE7O1FBQ2Isb0NBQW1COztRQUNuQixtQ0FBa0I7Ozs7O0lBR3BCLHVCQUlDOzs7O1FBSEMsNEJBQWtCOztRQUNsQixvQ0FBeUI7O1FBQ3pCLHNCQUFXOzs7OztJQUtiLHVCQU9DOzs7O1FBTkMsNEJBQWlCOztRQUNqQixrQ0FBd0I7O1FBQ3hCLHdDQUE4Qjs7UUFDOUIsK0JBQXFCOztRQUNyQixvQ0FBeUI7O1FBQ3pCLHNCQUFXOzs7OztJQUdiLG1CQVFDOzs7O1FBUEMsd0JBQWlCOztRQUNqQixvQkFBYTs7UUFDYix1QkFBZ0I7O1FBQ2hCLHFCQUFjOztRQUNkLDJCQUFvQjs7UUFDcEIsZ0NBQXVCOztRQUN2Qiw4QkFBcUI7Ozs7O0lBR3ZCLDhCQUdDOzs7O1FBRkMsbUNBQWlCOztRQUNqQixvQ0FBb0I7O0FBRXhCLENBQUMsRUFoRGdCLFFBQVEsS0FBUixRQUFRLFFBZ0R4QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFCUCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5cbmV4cG9ydCBuYW1lc3BhY2UgSWRlbnRpdHkge1xuICBleHBvcnQgaW50ZXJmYWNlIFN0YXRlIHtcbiAgICByb2xlczogUm9sZVJlc3BvbnNlO1xuICAgIHVzZXJzOiBVc2VyUmVzcG9uc2U7XG4gICAgc2VsZWN0ZWRSb2xlOiBSb2xlSXRlbTtcbiAgICBzZWxlY3RlZFVzZXI6IFVzZXJJdGVtO1xuICAgIHNlbGVjdGVkVXNlclJvbGVzOiBSb2xlSXRlbVtdO1xuICB9XG5cbiAgZXhwb3J0IHR5cGUgUm9sZVJlc3BvbnNlID0gQUJQLlBhZ2VkUmVzcG9uc2U8Um9sZUl0ZW0+O1xuXG4gIGV4cG9ydCBpbnRlcmZhY2UgUm9sZVNhdmVSZXF1ZXN0IHtcbiAgICBuYW1lOiBzdHJpbmc7XG4gICAgaXNEZWZhdWx0OiBib29sZWFuO1xuICAgIGlzUHVibGljOiBib29sZWFuO1xuICB9XG5cbiAgZXhwb3J0IGludGVyZmFjZSBSb2xlSXRlbSBleHRlbmRzIFJvbGVTYXZlUmVxdWVzdCB7XG4gICAgaXNTdGF0aWM6IGJvb2xlYW47XG4gICAgY29uY3VycmVuY3lTdGFtcDogc3RyaW5nO1xuICAgIGlkOiBzdHJpbmc7XG4gIH1cblxuICBleHBvcnQgdHlwZSBVc2VyUmVzcG9uc2UgPSBBQlAuUGFnZWRSZXNwb25zZTxVc2VySXRlbT47XG5cbiAgZXhwb3J0IGludGVyZmFjZSBVc2VySXRlbSBleHRlbmRzIFVzZXIge1xuICAgIHRlbmFudElkOiBzdHJpbmc7XG4gICAgZW1haWxDb25maXJtZWQ6IGJvb2xlYW47XG4gICAgcGhvbmVOdW1iZXJDb25maXJtZWQ6IGJvb2xlYW47XG4gICAgaXNMb2NrZWRPdXQ6IGJvb2xlYW47XG4gICAgY29uY3VycmVuY3lTdGFtcDogc3RyaW5nO1xuICAgIGlkOiBzdHJpbmc7XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIFVzZXIge1xuICAgIHVzZXJOYW1lOiBzdHJpbmc7XG4gICAgbmFtZTogc3RyaW5nO1xuICAgIHN1cm5hbWU6IHN0cmluZztcbiAgICBlbWFpbDogc3RyaW5nO1xuICAgIHBob25lTnVtYmVyOiBzdHJpbmc7XG4gICAgdHdvRmFjdG9yRW5hYmxlZDogdHJ1ZTtcbiAgICBsb2Nrb3V0RW5hYmxlZDogdHJ1ZTtcbiAgfVxuXG4gIGV4cG9ydCBpbnRlcmZhY2UgVXNlclNhdmVSZXF1ZXN0IGV4dGVuZHMgVXNlciB7XG4gICAgcGFzc3dvcmQ6IHN0cmluZztcbiAgICByb2xlTmFtZXM6IHN0cmluZ1tdO1xuICB9XG59XG4iXX0=