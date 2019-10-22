/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
export var Config;
(function (Config) {
    /**
     * @record
     */
    function Environment() { }
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
    function Application() { }
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
    function Apis() { }
    Config.Apis = Apis;
    /**
     * @record
     */
    function Requirements() { }
    Config.Requirements = Requirements;
    if (false) {
        /** @type {?} */
        Requirements.prototype.layouts;
    }
    /**
     * @record
     */
    function LocalizationWithDefault() { }
    Config.LocalizationWithDefault = LocalizationWithDefault;
    if (false) {
        /** @type {?} */
        LocalizationWithDefault.prototype.key;
        /** @type {?} */
        LocalizationWithDefault.prototype.defaultValue;
    }
})(Config || (Config = {}));
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL21vZGVscy9jb25maWcudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUtBLE1BQU0sS0FBVyxNQUFNLENBZ0N0QjtBQWhDRCxXQUFpQixNQUFNOzs7O0lBT3JCLDBCQU1DOzs7O1FBTEMsa0NBQXlCOztRQUN6QixpQ0FBb0I7O1FBQ3BCLGtDQUF3Qjs7UUFDeEIsMkJBQVc7O1FBQ1gsbUNBQThDOzs7OztJQUdoRCwwQkFHQzs7OztRQUZDLDJCQUFhOztRQUNiLDhCQUFpQjs7Ozs7SUFHbkIsbUJBRUM7Ozs7O0lBRUQsMkJBRUM7Ozs7UUFEQywrQkFBcUI7Ozs7O0lBR3ZCLHNDQUdDOzs7O1FBRkMsc0NBQVk7O1FBQ1osK0NBQXFCOztBQUV6QixDQUFDLEVBaENnQixNQUFNLEtBQU4sTUFBTSxRQWdDdEIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBdXRoQ29uZmlnIH0gZnJvbSAnYW5ndWxhci1vYXV0aDItb2lkYyc7XHJcbmltcG9ydCB7IFR5cGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgQXBwbGljYXRpb25Db25maWd1cmF0aW9uIH0gZnJvbSAnLi9hcHBsaWNhdGlvbi1jb25maWd1cmF0aW9uJztcclxuaW1wb3J0IHsgQUJQIH0gZnJvbSAnLi9jb21tb24nO1xyXG5cclxuZXhwb3J0IG5hbWVzcGFjZSBDb25maWcge1xyXG4gIGV4cG9ydCB0eXBlIFN0YXRlID0gQXBwbGljYXRpb25Db25maWd1cmF0aW9uLlJlc3BvbnNlICZcclxuICAgIEFCUC5Sb290ICYgeyBlbnZpcm9ubWVudDogRW52aXJvbm1lbnQgfSAmIHtcclxuICAgICAgcm91dGVzOiBBQlAuRnVsbFJvdXRlW107XHJcbiAgICAgIGZsYXR0ZWRSb3V0ZXM6IEFCUC5GdWxsUm91dGVbXTtcclxuICAgIH07XHJcblxyXG4gIGV4cG9ydCBpbnRlcmZhY2UgRW52aXJvbm1lbnQge1xyXG4gICAgYXBwbGljYXRpb246IEFwcGxpY2F0aW9uO1xyXG4gICAgcHJvZHVjdGlvbjogYm9vbGVhbjtcclxuICAgIG9BdXRoQ29uZmlnOiBBdXRoQ29uZmlnO1xyXG4gICAgYXBpczogQXBpcztcclxuICAgIGxvY2FsaXphdGlvbjogeyBkZWZhdWx0UmVzb3VyY2VOYW1lOiBzdHJpbmcgfTtcclxuICB9XHJcblxyXG4gIGV4cG9ydCBpbnRlcmZhY2UgQXBwbGljYXRpb24ge1xyXG4gICAgbmFtZTogc3RyaW5nO1xyXG4gICAgbG9nb1VybD86IHN0cmluZztcclxuICB9XHJcblxyXG4gIGV4cG9ydCBpbnRlcmZhY2UgQXBpcyB7XHJcbiAgICBba2V5OiBzdHJpbmddOiB7IFtrZXk6IHN0cmluZ106IHN0cmluZyB9O1xyXG4gIH1cclxuXHJcbiAgZXhwb3J0IGludGVyZmFjZSBSZXF1aXJlbWVudHMge1xyXG4gICAgbGF5b3V0czogVHlwZTxhbnk+W107XHJcbiAgfVxyXG5cclxuICBleHBvcnQgaW50ZXJmYWNlIExvY2FsaXphdGlvbldpdGhEZWZhdWx0IHtcclxuICAgIGtleTogc3RyaW5nO1xyXG4gICAgZGVmYXVsdFZhbHVlOiBzdHJpbmc7XHJcbiAgfVxyXG59XHJcbiJdfQ==