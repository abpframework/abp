/**
 * @fileoverview added by tsickle
 * Generated from: lib/models/application-configuration.ts
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBwbGljYXRpb24tY29uZmlndXJhdGlvbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9tb2RlbHMvYXBwbGljYXRpb24tY29uZmlndXJhdGlvbi50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUVBLE1BQU0sS0FBVyx3QkFBd0IsQ0E0Q3hDO0FBNUNELFdBQWlCLHdCQUF3Qjs7OztJQUN2Qyx1QkFNQzs7OztRQUxDLGdDQUEyQjs7UUFDM0Isd0JBQVc7O1FBQ1gsMkJBQWU7O1FBQ2YsK0JBQXlCOztRQUN6Qiw0QkFBZ0I7Ozs7O0lBR2xCLDJCQUdDOzs7O1FBRkMsOEJBQTBCOztRQUMxQixpQ0FBc0I7Ozs7O0lBR3hCLGdDQUVDOzs7OztJQUVELHVCQUtDOzs7O1FBSkMsK0JBQW9COztRQUNwQixpQ0FBc0I7O1FBQ3RCLCtCQUFvQjs7UUFDcEIsNEJBQWlCOzs7OztJQUduQixtQkFHQzs7OztRQUZDLHdCQUFpQjs7UUFDakIsK0JBQXdCOzs7OztJQUcxQixxQkFFQzs7Ozs7SUFFRCxvQkFFQzs7OztRQURDLHVCQUErQjs7Ozs7SUFHakMsMEJBS0M7Ozs7UUFKQyxzQ0FBeUI7O1FBQ3pCLHlCQUFXOztRQUNYLCtCQUFpQjs7UUFDakIsK0JBQWlCOztBQUVyQixDQUFDLEVBNUNnQix3QkFBd0IsS0FBeEIsd0JBQXdCLFFBNEN4QyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFCUCB9IGZyb20gJy4vY29tbW9uJztcblxuZXhwb3J0IG5hbWVzcGFjZSBBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24ge1xuICBleHBvcnQgaW50ZXJmYWNlIFJlc3BvbnNlIHtcbiAgICBsb2NhbGl6YXRpb246IExvY2FsaXphdGlvbjtcbiAgICBhdXRoOiBBdXRoO1xuICAgIHNldHRpbmc6IFZhbHVlO1xuICAgIGN1cnJlbnRVc2VyOiBDdXJyZW50VXNlcjtcbiAgICBmZWF0dXJlczogVmFsdWU7XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIExvY2FsaXphdGlvbiB7XG4gICAgdmFsdWVzOiBMb2NhbGl6YXRpb25WYWx1ZTtcbiAgICBsYW5ndWFnZXM6IExhbmd1YWdlW107XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIExvY2FsaXphdGlvblZhbHVlIHtcbiAgICBba2V5OiBzdHJpbmddOiB7IFtrZXk6IHN0cmluZ106IHN0cmluZyB9O1xuICB9XG5cbiAgZXhwb3J0IGludGVyZmFjZSBMYW5ndWFnZSB7XG4gICAgY3VsdHVyZU5hbWU6IHN0cmluZztcbiAgICB1aUN1bHR1cmVOYW1lOiBzdHJpbmc7XG4gICAgZGlzcGxheU5hbWU6IHN0cmluZztcbiAgICBmbGFnSWNvbjogc3RyaW5nO1xuICB9XG5cbiAgZXhwb3J0IGludGVyZmFjZSBBdXRoIHtcbiAgICBwb2xpY2llczogUG9saWN5O1xuICAgIGdyYW50ZWRQb2xpY2llczogUG9saWN5O1xuICB9XG5cbiAgZXhwb3J0IGludGVyZmFjZSBQb2xpY3kge1xuICAgIFtrZXk6IHN0cmluZ106IGJvb2xlYW47XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIFZhbHVlIHtcbiAgICB2YWx1ZXM6IEFCUC5EaWN0aW9uYXJ5PHN0cmluZz47XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIEN1cnJlbnRVc2VyIHtcbiAgICBpc0F1dGhlbnRpY2F0ZWQ6IGJvb2xlYW47XG4gICAgaWQ6IHN0cmluZztcbiAgICB0ZW5hbnRJZDogc3RyaW5nO1xuICAgIHVzZXJOYW1lOiBzdHJpbmc7XG4gIH1cbn1cbiJdfQ==