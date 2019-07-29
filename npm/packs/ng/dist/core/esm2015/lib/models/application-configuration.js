/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
    function Setting() { }
    ApplicationConfiguration.Setting = Setting;
    if (false) {
        /** @type {?} */
        Setting.prototype.values;
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
    /**
     * @record
     */
    function Features() { }
    ApplicationConfiguration.Features = Features;
    if (false) {
        /** @type {?} */
        Features.prototype.values;
    }
})(ApplicationConfiguration || (ApplicationConfiguration = {}));
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBwbGljYXRpb24tY29uZmlndXJhdGlvbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9tb2RlbHMvYXBwbGljYXRpb24tY29uZmlndXJhdGlvbi50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsTUFBTSxLQUFXLHdCQUF3QixDQWdEeEM7QUFoREQsV0FBaUIsd0JBQXdCOzs7O0lBQ3ZDLHVCQU1DOzs7O1FBTEMsZ0NBQTJCOztRQUMzQix3QkFBVzs7UUFDWCwyQkFBaUI7O1FBQ2pCLCtCQUF5Qjs7UUFDekIsNEJBQW1COzs7OztJQUdyQiwyQkFHQzs7OztRQUZDLDhCQUEwQjs7UUFDMUIsaUNBQXNCOzs7OztJQUd4QixnQ0FFQzs7Ozs7SUFFRCx1QkFLQzs7OztRQUpDLCtCQUFvQjs7UUFDcEIsaUNBQXNCOztRQUN0QiwrQkFBb0I7O1FBQ3BCLDRCQUFpQjs7Ozs7SUFHbkIsbUJBR0M7Ozs7UUFGQyx3QkFBaUI7O1FBQ2pCLCtCQUF3Qjs7Ozs7SUFHMUIscUJBRUM7Ozs7O0lBRUQsc0JBRUM7Ozs7UUFEQyx5QkFBOEQ7Ozs7O0lBR2hFLDBCQUtDOzs7O1FBSkMsc0NBQXlCOztRQUN6Qix5QkFBVzs7UUFDWCwrQkFBaUI7O1FBQ2pCLCtCQUFpQjs7Ozs7SUFHbkIsdUJBRUM7Ozs7UUFEQywwQkFBZ0I7O0FBRXBCLENBQUMsRUFoRGdCLHdCQUF3QixLQUF4Qix3QkFBd0IsUUFnRHhDIiwic291cmNlc0NvbnRlbnQiOlsiZXhwb3J0IG5hbWVzcGFjZSBBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24ge1xuICBleHBvcnQgaW50ZXJmYWNlIFJlc3BvbnNlIHtcbiAgICBsb2NhbGl6YXRpb246IExvY2FsaXphdGlvbjtcbiAgICBhdXRoOiBBdXRoO1xuICAgIHNldHRpbmc6IFNldHRpbmc7XG4gICAgY3VycmVudFVzZXI6IEN1cnJlbnRVc2VyO1xuICAgIGZlYXR1cmVzOiBGZWF0dXJlcztcbiAgfVxuXG4gIGV4cG9ydCBpbnRlcmZhY2UgTG9jYWxpemF0aW9uIHtcbiAgICB2YWx1ZXM6IExvY2FsaXphdGlvblZhbHVlO1xuICAgIGxhbmd1YWdlczogTGFuZ3VhZ2VbXTtcbiAgfVxuXG4gIGV4cG9ydCBpbnRlcmZhY2UgTG9jYWxpemF0aW9uVmFsdWUge1xuICAgIFtrZXk6IHN0cmluZ106IHsgW2tleTogc3RyaW5nXTogc3RyaW5nIH07XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIExhbmd1YWdlIHtcbiAgICBjdWx0dXJlTmFtZTogc3RyaW5nO1xuICAgIHVpQ3VsdHVyZU5hbWU6IHN0cmluZztcbiAgICBkaXNwbGF5TmFtZTogc3RyaW5nO1xuICAgIGZsYWdJY29uOiBzdHJpbmc7XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIEF1dGgge1xuICAgIHBvbGljaWVzOiBQb2xpY3k7XG4gICAgZ3JhbnRlZFBvbGljaWVzOiBQb2xpY3k7XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIFBvbGljeSB7XG4gICAgW2tleTogc3RyaW5nXTogYm9vbGVhbjtcbiAgfVxuXG4gIGV4cG9ydCBpbnRlcmZhY2UgU2V0dGluZyB7XG4gICAgdmFsdWVzOiB7IFtrZXk6IHN0cmluZ106ICdBYnAuTG9jYWxpemF0aW9uLkRlZmF1bHRMYW5ndWFnZScgfTtcbiAgfVxuXG4gIGV4cG9ydCBpbnRlcmZhY2UgQ3VycmVudFVzZXIge1xuICAgIGlzQXV0aGVudGljYXRlZDogYm9vbGVhbjtcbiAgICBpZDogc3RyaW5nO1xuICAgIHRlbmFudElkOiBzdHJpbmc7XG4gICAgdXNlck5hbWU6IHN0cmluZztcbiAgfVxuXG4gIGV4cG9ydCBpbnRlcmZhY2UgRmVhdHVyZXMge1xuICAgIHZhbHVlczogU2V0dGluZztcbiAgfVxufVxuIl19