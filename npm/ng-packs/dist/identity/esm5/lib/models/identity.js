/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL21vZGVscy9pZGVudGl0eS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBRUEsTUFBTSxLQUFXLFFBQVEsQ0FnRHhCO0FBaERELFdBQWlCLFFBQVE7Ozs7SUFDdkIsb0JBTUM7Ozs7UUFMQyxzQkFBb0I7O1FBQ3BCLHNCQUFvQjs7UUFDcEIsNkJBQXVCOztRQUN2Qiw2QkFBdUI7O1FBQ3ZCLGtDQUE4Qjs7Ozs7SUFLaEMsOEJBSUM7Ozs7UUFIQywrQkFBYTs7UUFDYixvQ0FBbUI7O1FBQ25CLG1DQUFrQjs7Ozs7SUFHcEIsdUJBSUM7Ozs7UUFIQyw0QkFBa0I7O1FBQ2xCLG9DQUF5Qjs7UUFDekIsc0JBQVc7Ozs7O0lBS2IsdUJBT0M7Ozs7UUFOQyw0QkFBaUI7O1FBQ2pCLGtDQUF3Qjs7UUFDeEIsd0NBQThCOztRQUM5QiwrQkFBcUI7O1FBQ3JCLG9DQUF5Qjs7UUFDekIsc0JBQVc7Ozs7O0lBR2IsbUJBUUM7Ozs7UUFQQyx3QkFBaUI7O1FBQ2pCLG9CQUFhOztRQUNiLHVCQUFnQjs7UUFDaEIscUJBQWM7O1FBQ2QsMkJBQW9COztRQUNwQixnQ0FBdUI7O1FBQ3ZCLDhCQUFxQjs7Ozs7SUFHdkIsOEJBR0M7Ozs7UUFGQyxtQ0FBaUI7O1FBQ2pCLG9DQUFvQjs7QUFFeEIsQ0FBQyxFQWhEZ0IsUUFBUSxLQUFSLFFBQVEsUUFnRHhCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuXHJcbmV4cG9ydCBuYW1lc3BhY2UgSWRlbnRpdHkge1xyXG4gIGV4cG9ydCBpbnRlcmZhY2UgU3RhdGUge1xyXG4gICAgcm9sZXM6IFJvbGVSZXNwb25zZTtcclxuICAgIHVzZXJzOiBVc2VyUmVzcG9uc2U7XHJcbiAgICBzZWxlY3RlZFJvbGU6IFJvbGVJdGVtO1xyXG4gICAgc2VsZWN0ZWRVc2VyOiBVc2VySXRlbTtcclxuICAgIHNlbGVjdGVkVXNlclJvbGVzOiBSb2xlSXRlbVtdO1xyXG4gIH1cclxuXHJcbiAgZXhwb3J0IHR5cGUgUm9sZVJlc3BvbnNlID0gQUJQLlBhZ2VkUmVzcG9uc2U8Um9sZUl0ZW0+O1xyXG5cclxuICBleHBvcnQgaW50ZXJmYWNlIFJvbGVTYXZlUmVxdWVzdCB7XHJcbiAgICBuYW1lOiBzdHJpbmc7XHJcbiAgICBpc0RlZmF1bHQ6IGJvb2xlYW47XHJcbiAgICBpc1B1YmxpYzogYm9vbGVhbjtcclxuICB9XHJcblxyXG4gIGV4cG9ydCBpbnRlcmZhY2UgUm9sZUl0ZW0gZXh0ZW5kcyBSb2xlU2F2ZVJlcXVlc3Qge1xyXG4gICAgaXNTdGF0aWM6IGJvb2xlYW47XHJcbiAgICBjb25jdXJyZW5jeVN0YW1wOiBzdHJpbmc7XHJcbiAgICBpZDogc3RyaW5nO1xyXG4gIH1cclxuXHJcbiAgZXhwb3J0IHR5cGUgVXNlclJlc3BvbnNlID0gQUJQLlBhZ2VkUmVzcG9uc2U8VXNlckl0ZW0+O1xyXG5cclxuICBleHBvcnQgaW50ZXJmYWNlIFVzZXJJdGVtIGV4dGVuZHMgVXNlciB7XHJcbiAgICB0ZW5hbnRJZDogc3RyaW5nO1xyXG4gICAgZW1haWxDb25maXJtZWQ6IGJvb2xlYW47XHJcbiAgICBwaG9uZU51bWJlckNvbmZpcm1lZDogYm9vbGVhbjtcclxuICAgIGlzTG9ja2VkT3V0OiBib29sZWFuO1xyXG4gICAgY29uY3VycmVuY3lTdGFtcDogc3RyaW5nO1xyXG4gICAgaWQ6IHN0cmluZztcclxuICB9XHJcblxyXG4gIGV4cG9ydCBpbnRlcmZhY2UgVXNlciB7XHJcbiAgICB1c2VyTmFtZTogc3RyaW5nO1xyXG4gICAgbmFtZTogc3RyaW5nO1xyXG4gICAgc3VybmFtZTogc3RyaW5nO1xyXG4gICAgZW1haWw6IHN0cmluZztcclxuICAgIHBob25lTnVtYmVyOiBzdHJpbmc7XHJcbiAgICB0d29GYWN0b3JFbmFibGVkOiB0cnVlO1xyXG4gICAgbG9ja291dEVuYWJsZWQ6IHRydWU7XHJcbiAgfVxyXG5cclxuICBleHBvcnQgaW50ZXJmYWNlIFVzZXJTYXZlUmVxdWVzdCBleHRlbmRzIFVzZXIge1xyXG4gICAgcGFzc3dvcmQ6IHN0cmluZztcclxuICAgIHJvbGVOYW1lczogc3RyaW5nW107XHJcbiAgfVxyXG59XHJcbiJdfQ==