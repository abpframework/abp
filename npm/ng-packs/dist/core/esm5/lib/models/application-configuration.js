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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBwbGljYXRpb24tY29uZmlndXJhdGlvbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9tb2RlbHMvYXBwbGljYXRpb24tY29uZmlndXJhdGlvbi50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUVBLE1BQU0sS0FBVyx3QkFBd0IsQ0E0Q3hDO0FBNUNELFdBQWlCLHdCQUF3Qjs7OztJQUN2Qyx1QkFNQzs7OztRQUxDLGdDQUEyQjs7UUFDM0Isd0JBQVc7O1FBQ1gsMkJBQWU7O1FBQ2YsK0JBQXlCOztRQUN6Qiw0QkFBZ0I7Ozs7O0lBR2xCLDJCQUdDOzs7O1FBRkMsOEJBQTBCOztRQUMxQixpQ0FBc0I7Ozs7O0lBR3hCLGdDQUVDOzs7OztJQUVELHVCQUtDOzs7O1FBSkMsK0JBQW9COztRQUNwQixpQ0FBc0I7O1FBQ3RCLCtCQUFvQjs7UUFDcEIsNEJBQWlCOzs7OztJQUduQixtQkFHQzs7OztRQUZDLHdCQUFpQjs7UUFDakIsK0JBQXdCOzs7OztJQUcxQixxQkFFQzs7Ozs7SUFFRCxvQkFFQzs7OztRQURDLHVCQUErQjs7Ozs7SUFHakMsMEJBS0M7Ozs7UUFKQyxzQ0FBeUI7O1FBQ3pCLHlCQUFXOztRQUNYLCtCQUFpQjs7UUFDakIsK0JBQWlCOztBQUVyQixDQUFDLEVBNUNnQix3QkFBd0IsS0FBeEIsd0JBQXdCLFFBNEN4QyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFCUCB9IGZyb20gJy4vY29tbW9uJztcclxuXHJcbmV4cG9ydCBuYW1lc3BhY2UgQXBwbGljYXRpb25Db25maWd1cmF0aW9uIHtcclxuICBleHBvcnQgaW50ZXJmYWNlIFJlc3BvbnNlIHtcclxuICAgIGxvY2FsaXphdGlvbjogTG9jYWxpemF0aW9uO1xyXG4gICAgYXV0aDogQXV0aDtcclxuICAgIHNldHRpbmc6IFZhbHVlO1xyXG4gICAgY3VycmVudFVzZXI6IEN1cnJlbnRVc2VyO1xyXG4gICAgZmVhdHVyZXM6IFZhbHVlO1xyXG4gIH1cclxuXHJcbiAgZXhwb3J0IGludGVyZmFjZSBMb2NhbGl6YXRpb24ge1xyXG4gICAgdmFsdWVzOiBMb2NhbGl6YXRpb25WYWx1ZTtcclxuICAgIGxhbmd1YWdlczogTGFuZ3VhZ2VbXTtcclxuICB9XHJcblxyXG4gIGV4cG9ydCBpbnRlcmZhY2UgTG9jYWxpemF0aW9uVmFsdWUge1xyXG4gICAgW2tleTogc3RyaW5nXTogeyBba2V5OiBzdHJpbmddOiBzdHJpbmcgfTtcclxuICB9XHJcblxyXG4gIGV4cG9ydCBpbnRlcmZhY2UgTGFuZ3VhZ2Uge1xyXG4gICAgY3VsdHVyZU5hbWU6IHN0cmluZztcclxuICAgIHVpQ3VsdHVyZU5hbWU6IHN0cmluZztcclxuICAgIGRpc3BsYXlOYW1lOiBzdHJpbmc7XHJcbiAgICBmbGFnSWNvbjogc3RyaW5nO1xyXG4gIH1cclxuXHJcbiAgZXhwb3J0IGludGVyZmFjZSBBdXRoIHtcclxuICAgIHBvbGljaWVzOiBQb2xpY3k7XHJcbiAgICBncmFudGVkUG9saWNpZXM6IFBvbGljeTtcclxuICB9XHJcblxyXG4gIGV4cG9ydCBpbnRlcmZhY2UgUG9saWN5IHtcclxuICAgIFtrZXk6IHN0cmluZ106IGJvb2xlYW47XHJcbiAgfVxyXG5cclxuICBleHBvcnQgaW50ZXJmYWNlIFZhbHVlIHtcclxuICAgIHZhbHVlczogQUJQLkRpY3Rpb25hcnk8c3RyaW5nPjtcclxuICB9XHJcblxyXG4gIGV4cG9ydCBpbnRlcmZhY2UgQ3VycmVudFVzZXIge1xyXG4gICAgaXNBdXRoZW50aWNhdGVkOiBib29sZWFuO1xyXG4gICAgaWQ6IHN0cmluZztcclxuICAgIHRlbmFudElkOiBzdHJpbmc7XHJcbiAgICB1c2VyTmFtZTogc3RyaW5nO1xyXG4gIH1cclxufVxyXG4iXX0=