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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL21vZGVscy9pZGVudGl0eS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUVBLE1BQU0sS0FBVyxRQUFRLENBZ0R4QjtBQWhERCxXQUFpQixRQUFROzs7O0lBQ3ZCLG9CQU1DOzs7O1FBTEMsc0JBQW9COztRQUNwQixzQkFBb0I7O1FBQ3BCLDZCQUF1Qjs7UUFDdkIsNkJBQXVCOztRQUN2QixrQ0FBOEI7Ozs7O0lBS2hDLDhCQUlDOzs7O1FBSEMsK0JBQWE7O1FBQ2Isb0NBQW1COztRQUNuQixtQ0FBa0I7Ozs7O0lBR3BCLHVCQUlDOzs7O1FBSEMsNEJBQWtCOztRQUNsQixvQ0FBeUI7O1FBQ3pCLHNCQUFXOzs7OztJQUtiLHVCQU9DOzs7O1FBTkMsNEJBQWlCOztRQUNqQixrQ0FBd0I7O1FBQ3hCLHdDQUE4Qjs7UUFDOUIsK0JBQXFCOztRQUNyQixvQ0FBeUI7O1FBQ3pCLHNCQUFXOzs7OztJQUdiLG1CQVFDOzs7O1FBUEMsd0JBQWlCOztRQUNqQixvQkFBYTs7UUFDYix1QkFBZ0I7O1FBQ2hCLHFCQUFjOztRQUNkLDJCQUFvQjs7UUFDcEIsZ0NBQXVCOztRQUN2Qiw4QkFBcUI7Ozs7O0lBR3ZCLDhCQUdDOzs7O1FBRkMsbUNBQWlCOztRQUNqQixvQ0FBb0I7O0FBRXhCLENBQUMsRUFoRGdCLFFBQVEsS0FBUixRQUFRLFFBZ0R4QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFCUCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcblxyXG5leHBvcnQgbmFtZXNwYWNlIElkZW50aXR5IHtcclxuICBleHBvcnQgaW50ZXJmYWNlIFN0YXRlIHtcclxuICAgIHJvbGVzOiBSb2xlUmVzcG9uc2U7XHJcbiAgICB1c2VyczogVXNlclJlc3BvbnNlO1xyXG4gICAgc2VsZWN0ZWRSb2xlOiBSb2xlSXRlbTtcclxuICAgIHNlbGVjdGVkVXNlcjogVXNlckl0ZW07XHJcbiAgICBzZWxlY3RlZFVzZXJSb2xlczogUm9sZUl0ZW1bXTtcclxuICB9XHJcblxyXG4gIGV4cG9ydCB0eXBlIFJvbGVSZXNwb25zZSA9IEFCUC5QYWdlZFJlc3BvbnNlPFJvbGVJdGVtPjtcclxuXHJcbiAgZXhwb3J0IGludGVyZmFjZSBSb2xlU2F2ZVJlcXVlc3Qge1xyXG4gICAgbmFtZTogc3RyaW5nO1xyXG4gICAgaXNEZWZhdWx0OiBib29sZWFuO1xyXG4gICAgaXNQdWJsaWM6IGJvb2xlYW47XHJcbiAgfVxyXG5cclxuICBleHBvcnQgaW50ZXJmYWNlIFJvbGVJdGVtIGV4dGVuZHMgUm9sZVNhdmVSZXF1ZXN0IHtcclxuICAgIGlzU3RhdGljOiBib29sZWFuO1xyXG4gICAgY29uY3VycmVuY3lTdGFtcDogc3RyaW5nO1xyXG4gICAgaWQ6IHN0cmluZztcclxuICB9XHJcblxyXG4gIGV4cG9ydCB0eXBlIFVzZXJSZXNwb25zZSA9IEFCUC5QYWdlZFJlc3BvbnNlPFVzZXJJdGVtPjtcclxuXHJcbiAgZXhwb3J0IGludGVyZmFjZSBVc2VySXRlbSBleHRlbmRzIFVzZXIge1xyXG4gICAgdGVuYW50SWQ6IHN0cmluZztcclxuICAgIGVtYWlsQ29uZmlybWVkOiBib29sZWFuO1xyXG4gICAgcGhvbmVOdW1iZXJDb25maXJtZWQ6IGJvb2xlYW47XHJcbiAgICBpc0xvY2tlZE91dDogYm9vbGVhbjtcclxuICAgIGNvbmN1cnJlbmN5U3RhbXA6IHN0cmluZztcclxuICAgIGlkOiBzdHJpbmc7XHJcbiAgfVxyXG5cclxuICBleHBvcnQgaW50ZXJmYWNlIFVzZXIge1xyXG4gICAgdXNlck5hbWU6IHN0cmluZztcclxuICAgIG5hbWU6IHN0cmluZztcclxuICAgIHN1cm5hbWU6IHN0cmluZztcclxuICAgIGVtYWlsOiBzdHJpbmc7XHJcbiAgICBwaG9uZU51bWJlcjogc3RyaW5nO1xyXG4gICAgdHdvRmFjdG9yRW5hYmxlZDogdHJ1ZTtcclxuICAgIGxvY2tvdXRFbmFibGVkOiB0cnVlO1xyXG4gIH1cclxuXHJcbiAgZXhwb3J0IGludGVyZmFjZSBVc2VyU2F2ZVJlcXVlc3QgZXh0ZW5kcyBVc2VyIHtcclxuICAgIHBhc3N3b3JkOiBzdHJpbmc7XHJcbiAgICByb2xlTmFtZXM6IHN0cmluZ1tdO1xyXG4gIH1cclxufVxyXG4iXX0=