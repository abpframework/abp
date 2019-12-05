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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL21vZGVscy9jb25maWcudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFLQSxNQUFNLEtBQVcsTUFBTSxDQWtDdEI7QUFsQ0QsV0FBaUIsTUFBTTs7OztJQU9yQiwwQkFNQzs7OztRQUxDLGtDQUF5Qjs7UUFDekIsaUNBQW9COztRQUNwQixrQ0FBd0I7O1FBQ3hCLDJCQUFXOztRQUNYLG1DQUE4Qzs7Ozs7SUFHaEQsMEJBR0M7Ozs7UUFGQywyQkFBYTs7UUFDYiw4QkFBaUI7Ozs7O0lBR25CLG1CQUVDOzs7OztJQUVELDJCQUVDOzs7O1FBREMsK0JBQXFCOzs7OztJQUd2QixzQ0FHQzs7OztRQUZDLHNDQUFZOztRQUNaLCtDQUFxQjs7QUFJekIsQ0FBQyxFQWxDZ0IsTUFBTSxLQUFOLE1BQU0sUUFrQ3RCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQXV0aENvbmZpZyB9IGZyb20gJ2FuZ3VsYXItb2F1dGgyLW9pZGMnO1xuaW1wb3J0IHsgVHlwZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgQXBwbGljYXRpb25Db25maWd1cmF0aW9uIH0gZnJvbSAnLi9hcHBsaWNhdGlvbi1jb25maWd1cmF0aW9uJztcbmltcG9ydCB7IEFCUCB9IGZyb20gJy4vY29tbW9uJztcblxuZXhwb3J0IG5hbWVzcGFjZSBDb25maWcge1xuICBleHBvcnQgdHlwZSBTdGF0ZSA9IEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbi5SZXNwb25zZSAmXG4gICAgQUJQLlJvb3QgJiB7IGVudmlyb25tZW50OiBFbnZpcm9ubWVudCB9ICYge1xuICAgICAgcm91dGVzOiBBQlAuRnVsbFJvdXRlW107XG4gICAgICBmbGF0dGVkUm91dGVzOiBBQlAuRnVsbFJvdXRlW107XG4gICAgfTtcblxuICBleHBvcnQgaW50ZXJmYWNlIEVudmlyb25tZW50IHtcbiAgICBhcHBsaWNhdGlvbjogQXBwbGljYXRpb247XG4gICAgcHJvZHVjdGlvbjogYm9vbGVhbjtcbiAgICBvQXV0aENvbmZpZzogQXV0aENvbmZpZztcbiAgICBhcGlzOiBBcGlzO1xuICAgIGxvY2FsaXphdGlvbjogeyBkZWZhdWx0UmVzb3VyY2VOYW1lOiBzdHJpbmcgfTtcbiAgfVxuXG4gIGV4cG9ydCBpbnRlcmZhY2UgQXBwbGljYXRpb24ge1xuICAgIG5hbWU6IHN0cmluZztcbiAgICBsb2dvVXJsPzogc3RyaW5nO1xuICB9XG5cbiAgZXhwb3J0IGludGVyZmFjZSBBcGlzIHtcbiAgICBba2V5OiBzdHJpbmddOiB7IFtrZXk6IHN0cmluZ106IHN0cmluZyB9O1xuICB9XG5cbiAgZXhwb3J0IGludGVyZmFjZSBSZXF1aXJlbWVudHMge1xuICAgIGxheW91dHM6IFR5cGU8YW55PltdO1xuICB9XG5cbiAgZXhwb3J0IGludGVyZmFjZSBMb2NhbGl6YXRpb25XaXRoRGVmYXVsdCB7XG4gICAga2V5OiBzdHJpbmc7XG4gICAgZGVmYXVsdFZhbHVlOiBzdHJpbmc7XG4gIH1cblxuICBleHBvcnQgdHlwZSBMb2NhbGl6YXRpb25QYXJhbSA9IHN0cmluZyB8IExvY2FsaXphdGlvbldpdGhEZWZhdWx0O1xufVxuIl19