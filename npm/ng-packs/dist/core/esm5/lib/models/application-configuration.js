/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
export var ApplicationConfiguration;
(function(ApplicationConfiguration) {
  /**
   * @record
   */
  function Response() {}
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
  function Localization() {}
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
  function LocalizationValue() {}
  ApplicationConfiguration.LocalizationValue = LocalizationValue;
  /**
   * @record
   */
  function Language() {}
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
  function Auth() {}
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
  function Policy() {}
  ApplicationConfiguration.Policy = Policy;
  /**
   * @record
   */
  function Value() {}
  ApplicationConfiguration.Value = Value;
  if (false) {
    /** @type {?} */
    Value.prototype.values;
  }
  /**
   * @record
   */
  function CurrentUser() {}
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBwbGljYXRpb24tY29uZmlndXJhdGlvbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9tb2RlbHMvYXBwbGljYXRpb24tY29uZmlndXJhdGlvbi50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBRUEsTUFBTSxLQUFXLHdCQUF3QixDQTRDeEM7QUE1Q0QsV0FBaUIsd0JBQXdCOzs7O0lBQ3ZDLHVCQU1DOzs7O1FBTEMsZ0NBQTJCOztRQUMzQix3QkFBVzs7UUFDWCwyQkFBZTs7UUFDZiwrQkFBeUI7O1FBQ3pCLDRCQUFnQjs7Ozs7SUFHbEIsMkJBR0M7Ozs7UUFGQyw4QkFBMEI7O1FBQzFCLGlDQUFzQjs7Ozs7SUFHeEIsZ0NBRUM7Ozs7O0lBRUQsdUJBS0M7Ozs7UUFKQywrQkFBb0I7O1FBQ3BCLGlDQUFzQjs7UUFDdEIsK0JBQW9COztRQUNwQiw0QkFBaUI7Ozs7O0lBR25CLG1CQUdDOzs7O1FBRkMsd0JBQWlCOztRQUNqQiwrQkFBd0I7Ozs7O0lBRzFCLHFCQUVDOzs7OztJQUVELG9CQUVDOzs7O1FBREMsdUJBQStCOzs7OztJQUdqQywwQkFLQzs7OztRQUpDLHNDQUF5Qjs7UUFDekIseUJBQVc7O1FBQ1gsK0JBQWlCOztRQUNqQiwrQkFBaUI7O0FBRXJCLENBQUMsRUE1Q2dCLHdCQUF3QixLQUF4Qix3QkFBd0IsUUE0Q3hDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQUJQIH0gZnJvbSAnLi9jb21tb24nO1xuXG5leHBvcnQgbmFtZXNwYWNlIEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbiB7XG4gIGV4cG9ydCBpbnRlcmZhY2UgUmVzcG9uc2Uge1xuICAgIGxvY2FsaXphdGlvbjogTG9jYWxpemF0aW9uO1xuICAgIGF1dGg6IEF1dGg7XG4gICAgc2V0dGluZzogVmFsdWU7XG4gICAgY3VycmVudFVzZXI6IEN1cnJlbnRVc2VyO1xuICAgIGZlYXR1cmVzOiBWYWx1ZTtcbiAgfVxuXG4gIGV4cG9ydCBpbnRlcmZhY2UgTG9jYWxpemF0aW9uIHtcbiAgICB2YWx1ZXM6IExvY2FsaXphdGlvblZhbHVlO1xuICAgIGxhbmd1YWdlczogTGFuZ3VhZ2VbXTtcbiAgfVxuXG4gIGV4cG9ydCBpbnRlcmZhY2UgTG9jYWxpemF0aW9uVmFsdWUge1xuICAgIFtrZXk6IHN0cmluZ106IHsgW2tleTogc3RyaW5nXTogc3RyaW5nIH07XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIExhbmd1YWdlIHtcbiAgICBjdWx0dXJlTmFtZTogc3RyaW5nO1xuICAgIHVpQ3VsdHVyZU5hbWU6IHN0cmluZztcbiAgICBkaXNwbGF5TmFtZTogc3RyaW5nO1xuICAgIGZsYWdJY29uOiBzdHJpbmc7XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIEF1dGgge1xuICAgIHBvbGljaWVzOiBQb2xpY3k7XG4gICAgZ3JhbnRlZFBvbGljaWVzOiBQb2xpY3k7XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIFBvbGljeSB7XG4gICAgW2tleTogc3RyaW5nXTogYm9vbGVhbjtcbiAgfVxuXG4gIGV4cG9ydCBpbnRlcmZhY2UgVmFsdWUge1xuICAgIHZhbHVlczogQUJQLkRpY3Rpb25hcnk8c3RyaW5nPjtcbiAgfVxuXG4gIGV4cG9ydCBpbnRlcmZhY2UgQ3VycmVudFVzZXIge1xuICAgIGlzQXV0aGVudGljYXRlZDogYm9vbGVhbjtcbiAgICBpZDogc3RyaW5nO1xuICAgIHRlbmFudElkOiBzdHJpbmc7XG4gICAgdXNlck5hbWU6IHN0cmluZztcbiAgfVxufVxuIl19
