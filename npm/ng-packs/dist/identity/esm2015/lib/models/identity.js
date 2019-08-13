/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL21vZGVscy9pZGVudGl0eS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBRUEsTUFBTSxLQUFXLFFBQVEsQ0FnRHhCO0FBaERELFdBQWlCLFFBQVE7Ozs7SUFDdkIsb0JBTUM7Ozs7UUFMQyxzQkFBb0I7O1FBQ3BCLHNCQUFvQjs7UUFDcEIsNkJBQXVCOztRQUN2Qiw2QkFBdUI7O1FBQ3ZCLGtDQUE4Qjs7Ozs7SUFLaEMsOEJBSUM7Ozs7UUFIQywrQkFBYTs7UUFDYixvQ0FBbUI7O1FBQ25CLG1DQUFrQjs7Ozs7SUFHcEIsdUJBSUM7Ozs7UUFIQyw0QkFBa0I7O1FBQ2xCLG9DQUF5Qjs7UUFDekIsc0JBQVc7Ozs7O0lBS2IsdUJBT0M7Ozs7UUFOQyw0QkFBaUI7O1FBQ2pCLGtDQUF3Qjs7UUFDeEIsd0NBQThCOztRQUM5QiwrQkFBcUI7O1FBQ3JCLG9DQUF5Qjs7UUFDekIsc0JBQVc7Ozs7O0lBR2IsbUJBUUM7Ozs7UUFQQyx3QkFBaUI7O1FBQ2pCLG9CQUFhOztRQUNiLHVCQUFnQjs7UUFDaEIscUJBQWM7O1FBQ2QsMkJBQW9COztRQUNwQixnQ0FBdUI7O1FBQ3ZCLDhCQUFxQjs7Ozs7SUFHdkIsOEJBR0M7Ozs7UUFGQyxtQ0FBaUI7O1FBQ2pCLG9DQUFvQjs7QUFFeEIsQ0FBQyxFQWhEZ0IsUUFBUSxLQUFSLFFBQVEsUUFnRHhCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcblxuZXhwb3J0IG5hbWVzcGFjZSBJZGVudGl0eSB7XG4gIGV4cG9ydCBpbnRlcmZhY2UgU3RhdGUge1xuICAgIHJvbGVzOiBSb2xlUmVzcG9uc2U7XG4gICAgdXNlcnM6IFVzZXJSZXNwb25zZTtcbiAgICBzZWxlY3RlZFJvbGU6IFJvbGVJdGVtO1xuICAgIHNlbGVjdGVkVXNlcjogVXNlckl0ZW07XG4gICAgc2VsZWN0ZWRVc2VyUm9sZXM6IFJvbGVJdGVtW107XG4gIH1cblxuICBleHBvcnQgdHlwZSBSb2xlUmVzcG9uc2UgPSBBQlAuUGFnZWRSZXNwb25zZTxSb2xlSXRlbT47XG5cbiAgZXhwb3J0IGludGVyZmFjZSBSb2xlU2F2ZVJlcXVlc3Qge1xuICAgIG5hbWU6IHN0cmluZztcbiAgICBpc0RlZmF1bHQ6IGJvb2xlYW47XG4gICAgaXNQdWJsaWM6IGJvb2xlYW47XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIFJvbGVJdGVtIGV4dGVuZHMgUm9sZVNhdmVSZXF1ZXN0IHtcbiAgICBpc1N0YXRpYzogYm9vbGVhbjtcbiAgICBjb25jdXJyZW5jeVN0YW1wOiBzdHJpbmc7XG4gICAgaWQ6IHN0cmluZztcbiAgfVxuXG4gIGV4cG9ydCB0eXBlIFVzZXJSZXNwb25zZSA9IEFCUC5QYWdlZFJlc3BvbnNlPFVzZXJJdGVtPjtcblxuICBleHBvcnQgaW50ZXJmYWNlIFVzZXJJdGVtIGV4dGVuZHMgVXNlciB7XG4gICAgdGVuYW50SWQ6IHN0cmluZztcbiAgICBlbWFpbENvbmZpcm1lZDogYm9vbGVhbjtcbiAgICBwaG9uZU51bWJlckNvbmZpcm1lZDogYm9vbGVhbjtcbiAgICBpc0xvY2tlZE91dDogYm9vbGVhbjtcbiAgICBjb25jdXJyZW5jeVN0YW1wOiBzdHJpbmc7XG4gICAgaWQ6IHN0cmluZztcbiAgfVxuXG4gIGV4cG9ydCBpbnRlcmZhY2UgVXNlciB7XG4gICAgdXNlck5hbWU6IHN0cmluZztcbiAgICBuYW1lOiBzdHJpbmc7XG4gICAgc3VybmFtZTogc3RyaW5nO1xuICAgIGVtYWlsOiBzdHJpbmc7XG4gICAgcGhvbmVOdW1iZXI6IHN0cmluZztcbiAgICB0d29GYWN0b3JFbmFibGVkOiB0cnVlO1xuICAgIGxvY2tvdXRFbmFibGVkOiB0cnVlO1xuICB9XG5cbiAgZXhwb3J0IGludGVyZmFjZSBVc2VyU2F2ZVJlcXVlc3QgZXh0ZW5kcyBVc2VyIHtcbiAgICBwYXNzd29yZDogc3RyaW5nO1xuICAgIHJvbGVOYW1lczogc3RyaW5nW107XG4gIH1cbn1cbiJdfQ==