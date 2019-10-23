/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
export var Config;
(function(Config) {
  /**
   * @record
   */
  function Environment() {}
  Config.Environment = Environment;
  if (false) {
    /** @type {?} */
    Environment.prototype.application;
    /** @type {?} */
    Environment.prototype.production;
    /** @type {?} */
    Environment.prototype.oAuthConfig;
    /** @type {?} */
    Environment.prototype.apis;
    /** @type {?} */
    Environment.prototype.localization;
  }
  /**
   * @record
   */
  function Application() {}
  Config.Application = Application;
  if (false) {
    /** @type {?} */
    Application.prototype.name;
    /** @type {?|undefined} */
    Application.prototype.logoUrl;
  }
  /**
   * @record
   */
  function Apis() {}
  Config.Apis = Apis;
  /**
   * @record
   */
  function Requirements() {}
  Config.Requirements = Requirements;
  if (false) {
    /** @type {?} */
    Requirements.prototype.layouts;
  }
  /**
   * @record
   */
  function LocalizationWithDefault() {}
  Config.LocalizationWithDefault = LocalizationWithDefault;
  if (false) {
    /** @type {?} */
    LocalizationWithDefault.prototype.key;
    /** @type {?} */
    LocalizationWithDefault.prototype.defaultValue;
  }
})(Config || (Config = {}));
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL21vZGVscy9jb25maWcudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUtBLE1BQU0sS0FBVyxNQUFNLENBZ0N0QjtBQWhDRCxXQUFpQixNQUFNOzs7O0lBT3JCLDBCQU1DOzs7O1FBTEMsa0NBQXlCOztRQUN6QixpQ0FBb0I7O1FBQ3BCLGtDQUF3Qjs7UUFDeEIsMkJBQVc7O1FBQ1gsbUNBQThDOzs7OztJQUdoRCwwQkFHQzs7OztRQUZDLDJCQUFhOztRQUNiLDhCQUFpQjs7Ozs7SUFHbkIsbUJBRUM7Ozs7O0lBRUQsMkJBRUM7Ozs7UUFEQywrQkFBcUI7Ozs7O0lBR3ZCLHNDQUdDOzs7O1FBRkMsc0NBQVk7O1FBQ1osK0NBQXFCOztBQUV6QixDQUFDLEVBaENnQixNQUFNLEtBQU4sTUFBTSxRQWdDdEIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBdXRoQ29uZmlnIH0gZnJvbSAnYW5ndWxhci1vYXV0aDItb2lkYyc7XG5pbXBvcnQgeyBUeXBlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24gfSBmcm9tICcuL2FwcGxpY2F0aW9uLWNvbmZpZ3VyYXRpb24nO1xuaW1wb3J0IHsgQUJQIH0gZnJvbSAnLi9jb21tb24nO1xuXG5leHBvcnQgbmFtZXNwYWNlIENvbmZpZyB7XG4gIGV4cG9ydCB0eXBlIFN0YXRlID0gQXBwbGljYXRpb25Db25maWd1cmF0aW9uLlJlc3BvbnNlICZcbiAgICBBQlAuUm9vdCAmIHsgZW52aXJvbm1lbnQ6IEVudmlyb25tZW50IH0gJiB7XG4gICAgICByb3V0ZXM6IEFCUC5GdWxsUm91dGVbXTtcbiAgICAgIGZsYXR0ZWRSb3V0ZXM6IEFCUC5GdWxsUm91dGVbXTtcbiAgICB9O1xuXG4gIGV4cG9ydCBpbnRlcmZhY2UgRW52aXJvbm1lbnQge1xuICAgIGFwcGxpY2F0aW9uOiBBcHBsaWNhdGlvbjtcbiAgICBwcm9kdWN0aW9uOiBib29sZWFuO1xuICAgIG9BdXRoQ29uZmlnOiBBdXRoQ29uZmlnO1xuICAgIGFwaXM6IEFwaXM7XG4gICAgbG9jYWxpemF0aW9uOiB7IGRlZmF1bHRSZXNvdXJjZU5hbWU6IHN0cmluZyB9O1xuICB9XG5cbiAgZXhwb3J0IGludGVyZmFjZSBBcHBsaWNhdGlvbiB7XG4gICAgbmFtZTogc3RyaW5nO1xuICAgIGxvZ29Vcmw/OiBzdHJpbmc7XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIEFwaXMge1xuICAgIFtrZXk6IHN0cmluZ106IHsgW2tleTogc3RyaW5nXTogc3RyaW5nIH07XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIFJlcXVpcmVtZW50cyB7XG4gICAgbGF5b3V0czogVHlwZTxhbnk+W107XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIExvY2FsaXphdGlvbldpdGhEZWZhdWx0IHtcbiAgICBrZXk6IHN0cmluZztcbiAgICBkZWZhdWx0VmFsdWU6IHN0cmluZztcbiAgfVxufVxuIl19
