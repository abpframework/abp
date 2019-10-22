/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
export var ApplicationConfiguration;
(function (ApplicationConfiguration) {
    /**
     * @record
     */
    function Response() { }
    ApplicationConfiguration.Response = Response;
    if (false) {
        /** @type {?} */
        Response.prototype.localization;
        /** @type {?} */
        Response.prototype.auth;
        /** @type {?} */
        Response.prototype.setting;
        /** @type {?} */
        Response.prototype.currentUser;
        /** @type {?} */
        Response.prototype.features;
    }
    /**
     * @record
     */
    function Localization() { }
    ApplicationConfiguration.Localization = Localization;
    if (false) {
        /** @type {?} */
        Localization.prototype.values;
        /** @type {?} */
        Localization.prototype.languages;
    }
    /**
     * @record
     */
    function LocalizationValue() { }
    ApplicationConfiguration.LocalizationValue = LocalizationValue;
    /**
     * @record
     */
    function Language() { }
    ApplicationConfiguration.Language = Language;
    if (false) {
        /** @type {?} */
        Language.prototype.cultureName;
        /** @type {?} */
        Language.prototype.uiCultureName;
        /** @type {?} */
        Language.prototype.displayName;
        /** @type {?} */
        Language.prototype.flagIcon;
    }
    /**
     * @record
     */
    function Auth() { }
    ApplicationConfiguration.Auth = Auth;
    if (false) {
        /** @type {?} */
        Auth.prototype.policies;
        /** @type {?} */
        Auth.prototype.grantedPolicies;
    }
    /**
     * @record
     */
    function Policy() { }
    ApplicationConfiguration.Policy = Policy;
    /**
     * @record
     */
    function Value() { }
    ApplicationConfiguration.Value = Value;
    if (false) {
        /** @type {?} */
        Value.prototype.values;
    }
    /**
     * @record
     */
    function CurrentUser() { }
    ApplicationConfiguration.CurrentUser = CurrentUser;
    if (false) {
        /** @type {?} */
        CurrentUser.prototype.isAuthenticated;
        /** @type {?} */
        CurrentUser.prototype.id;
        /** @type {?} */
        CurrentUser.prototype.tenantId;
        /** @type {?} */
        CurrentUser.prototype.userName;
    }
})(ApplicationConfiguration || (ApplicationConfiguration = {}));
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBwbGljYXRpb24tY29uZmlndXJhdGlvbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9tb2RlbHMvYXBwbGljYXRpb24tY29uZmlndXJhdGlvbi50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBRUEsTUFBTSxLQUFXLHdCQUF3QixDQTRDeEM7QUE1Q0QsV0FBaUIsd0JBQXdCOzs7O0lBQ3ZDLHVCQU1DOzs7O1FBTEMsZ0NBQTJCOztRQUMzQix3QkFBVzs7UUFDWCwyQkFBZTs7UUFDZiwrQkFBeUI7O1FBQ3pCLDRCQUFnQjs7Ozs7SUFHbEIsMkJBR0M7Ozs7UUFGQyw4QkFBMEI7O1FBQzFCLGlDQUFzQjs7Ozs7SUFHeEIsZ0NBRUM7Ozs7O0lBRUQsdUJBS0M7Ozs7UUFKQywrQkFBb0I7O1FBQ3BCLGlDQUFzQjs7UUFDdEIsK0JBQW9COztRQUNwQiw0QkFBaUI7Ozs7O0lBR25CLG1CQUdDOzs7O1FBRkMsd0JBQWlCOztRQUNqQiwrQkFBd0I7Ozs7O0lBRzFCLHFCQUVDOzs7OztJQUVELG9CQUVDOzs7O1FBREMsdUJBQStCOzs7OztJQUdqQywwQkFLQzs7OztRQUpDLHNDQUF5Qjs7UUFDekIseUJBQVc7O1FBQ1gsK0JBQWlCOztRQUNqQiwrQkFBaUI7O0FBRXJCLENBQUMsRUE1Q2dCLHdCQUF3QixLQUF4Qix3QkFBd0IsUUE0Q3hDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQUJQIH0gZnJvbSAnLi9jb21tb24nO1xyXG5cclxuZXhwb3J0IG5hbWVzcGFjZSBBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24ge1xyXG4gIGV4cG9ydCBpbnRlcmZhY2UgUmVzcG9uc2Uge1xyXG4gICAgbG9jYWxpemF0aW9uOiBMb2NhbGl6YXRpb247XHJcbiAgICBhdXRoOiBBdXRoO1xyXG4gICAgc2V0dGluZzogVmFsdWU7XHJcbiAgICBjdXJyZW50VXNlcjogQ3VycmVudFVzZXI7XHJcbiAgICBmZWF0dXJlczogVmFsdWU7XHJcbiAgfVxyXG5cclxuICBleHBvcnQgaW50ZXJmYWNlIExvY2FsaXphdGlvbiB7XHJcbiAgICB2YWx1ZXM6IExvY2FsaXphdGlvblZhbHVlO1xyXG4gICAgbGFuZ3VhZ2VzOiBMYW5ndWFnZVtdO1xyXG4gIH1cclxuXHJcbiAgZXhwb3J0IGludGVyZmFjZSBMb2NhbGl6YXRpb25WYWx1ZSB7XHJcbiAgICBba2V5OiBzdHJpbmddOiB7IFtrZXk6IHN0cmluZ106IHN0cmluZyB9O1xyXG4gIH1cclxuXHJcbiAgZXhwb3J0IGludGVyZmFjZSBMYW5ndWFnZSB7XHJcbiAgICBjdWx0dXJlTmFtZTogc3RyaW5nO1xyXG4gICAgdWlDdWx0dXJlTmFtZTogc3RyaW5nO1xyXG4gICAgZGlzcGxheU5hbWU6IHN0cmluZztcclxuICAgIGZsYWdJY29uOiBzdHJpbmc7XHJcbiAgfVxyXG5cclxuICBleHBvcnQgaW50ZXJmYWNlIEF1dGgge1xyXG4gICAgcG9saWNpZXM6IFBvbGljeTtcclxuICAgIGdyYW50ZWRQb2xpY2llczogUG9saWN5O1xyXG4gIH1cclxuXHJcbiAgZXhwb3J0IGludGVyZmFjZSBQb2xpY3kge1xyXG4gICAgW2tleTogc3RyaW5nXTogYm9vbGVhbjtcclxuICB9XHJcblxyXG4gIGV4cG9ydCBpbnRlcmZhY2UgVmFsdWUge1xyXG4gICAgdmFsdWVzOiBBQlAuRGljdGlvbmFyeTxzdHJpbmc+O1xyXG4gIH1cclxuXHJcbiAgZXhwb3J0IGludGVyZmFjZSBDdXJyZW50VXNlciB7XHJcbiAgICBpc0F1dGhlbnRpY2F0ZWQ6IGJvb2xlYW47XHJcbiAgICBpZDogc3RyaW5nO1xyXG4gICAgdGVuYW50SWQ6IHN0cmluZztcclxuICAgIHVzZXJOYW1lOiBzdHJpbmc7XHJcbiAgfVxyXG59XHJcbiJdfQ==