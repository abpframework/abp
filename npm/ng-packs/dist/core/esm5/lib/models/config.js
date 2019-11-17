/**
 * @fileoverview added by tsickle
 * Generated from: lib/models/config.ts
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL21vZGVscy9jb25maWcudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFLQSxNQUFNLEtBQVcsTUFBTSxDQWtDdEI7QUFsQ0QsV0FBaUIsTUFBTTs7OztJQU9yQiwwQkFNQzs7OztRQUxDLGtDQUF5Qjs7UUFDekIsaUNBQW9COztRQUNwQixrQ0FBd0I7O1FBQ3hCLDJCQUFXOztRQUNYLG1DQUE4Qzs7Ozs7SUFHaEQsMEJBR0M7Ozs7UUFGQywyQkFBYTs7UUFDYiw4QkFBaUI7Ozs7O0lBR25CLG1CQUVDOzs7OztJQUVELDJCQUVDOzs7O1FBREMsK0JBQXFCOzs7OztJQUd2QixzQ0FHQzs7OztRQUZDLHNDQUFZOztRQUNaLCtDQUFxQjs7QUFJekIsQ0FBQyxFQWxDZ0IsTUFBTSxLQUFOLE1BQU0sUUFrQ3RCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQXV0aENvbmZpZyB9IGZyb20gJ2FuZ3VsYXItb2F1dGgyLW9pZGMnO1xyXG5pbXBvcnQgeyBUeXBlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbiB9IGZyb20gJy4vYXBwbGljYXRpb24tY29uZmlndXJhdGlvbic7XHJcbmltcG9ydCB7IEFCUCB9IGZyb20gJy4vY29tbW9uJztcclxuXHJcbmV4cG9ydCBuYW1lc3BhY2UgQ29uZmlnIHtcclxuICBleHBvcnQgdHlwZSBTdGF0ZSA9IEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbi5SZXNwb25zZSAmXHJcbiAgICBBQlAuUm9vdCAmIHsgZW52aXJvbm1lbnQ6IEVudmlyb25tZW50IH0gJiB7XHJcbiAgICAgIHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdO1xyXG4gICAgICBmbGF0dGVkUm91dGVzOiBBQlAuRnVsbFJvdXRlW107XHJcbiAgICB9O1xyXG5cclxuICBleHBvcnQgaW50ZXJmYWNlIEVudmlyb25tZW50IHtcclxuICAgIGFwcGxpY2F0aW9uOiBBcHBsaWNhdGlvbjtcclxuICAgIHByb2R1Y3Rpb246IGJvb2xlYW47XHJcbiAgICBvQXV0aENvbmZpZzogQXV0aENvbmZpZztcclxuICAgIGFwaXM6IEFwaXM7XHJcbiAgICBsb2NhbGl6YXRpb246IHsgZGVmYXVsdFJlc291cmNlTmFtZTogc3RyaW5nIH07XHJcbiAgfVxyXG5cclxuICBleHBvcnQgaW50ZXJmYWNlIEFwcGxpY2F0aW9uIHtcclxuICAgIG5hbWU6IHN0cmluZztcclxuICAgIGxvZ29Vcmw/OiBzdHJpbmc7XHJcbiAgfVxyXG5cclxuICBleHBvcnQgaW50ZXJmYWNlIEFwaXMge1xyXG4gICAgW2tleTogc3RyaW5nXTogeyBba2V5OiBzdHJpbmddOiBzdHJpbmcgfTtcclxuICB9XHJcblxyXG4gIGV4cG9ydCBpbnRlcmZhY2UgUmVxdWlyZW1lbnRzIHtcclxuICAgIGxheW91dHM6IFR5cGU8YW55PltdO1xyXG4gIH1cclxuXHJcbiAgZXhwb3J0IGludGVyZmFjZSBMb2NhbGl6YXRpb25XaXRoRGVmYXVsdCB7XHJcbiAgICBrZXk6IHN0cmluZztcclxuICAgIGRlZmF1bHRWYWx1ZTogc3RyaW5nO1xyXG4gIH1cclxuXHJcbiAgZXhwb3J0IHR5cGUgTG9jYWxpemF0aW9uUGFyYW0gPSBzdHJpbmcgfCBMb2NhbGl6YXRpb25XaXRoRGVmYXVsdDtcclxufVxyXG4iXX0=