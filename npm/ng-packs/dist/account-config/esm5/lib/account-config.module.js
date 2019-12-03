/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { CoreModule, noop } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { APP_INITIALIZER, InjectionToken, NgModule } from '@angular/core';
import { AccountConfigService } from './services/account-config.service';
/**
 * @record
 */
export function AccountConfigOptions() { }
if (false) {
    /** @type {?|undefined} */
    AccountConfigOptions.prototype.redirectUrl;
}
/**
 * @param {?} options
 * @return {?}
 */
export function accountOptionsFactory(options) {
    return tslib_1.__assign({ redirectUrl: '/' }, options);
}
/** @type {?} */
export var ACCOUNT_OPTIONS = new InjectionToken('ACCOUNT_OPTIONS');
var ɵ0 = noop;
var AccountConfigModule = /** @class */ (function () {
    function AccountConfigModule() {
    }
    /**
     * @param {?=} options
     * @return {?}
     */
    AccountConfigModule.forRoot = /**
     * @param {?=} options
     * @return {?}
     */
    function (options) {
        if (options === void 0) { options = (/** @type {?} */ ({})); }
        return {
            ngModule: AccountConfigModule,
            providers: [
                { provide: ACCOUNT_OPTIONS, useValue: options },
                {
                    provide: 'ACCOUNT_OPTIONS',
                    useFactory: accountOptionsFactory,
                    deps: [ACCOUNT_OPTIONS],
                },
            ],
        };
    };
    AccountConfigModule.decorators = [
        { type: NgModule, args: [{
                    imports: [CoreModule, ThemeSharedModule],
                    providers: [{ provide: APP_INITIALIZER, multi: true, deps: [AccountConfigService], useFactory: ɵ0 }],
                },] }
    ];
    return AccountConfigModule;
}());
export { AccountConfigModule };
export { ɵ0 };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC1jb25maWcubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LmNvbmZpZy8iLCJzb3VyY2VzIjpbImxpYi9hY2NvdW50LWNvbmZpZy5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLElBQUksRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUNoRCxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUN6RCxPQUFPLEVBQUUsZUFBZSxFQUFFLGNBQWMsRUFBdUIsUUFBUSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQy9GLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLG1DQUFtQyxDQUFDOzs7O0FBRXpFLDBDQUVDOzs7SUFEQywyQ0FBcUI7Ozs7OztBQUd2QixNQUFNLFVBQVUscUJBQXFCLENBQUMsT0FBNkI7SUFDakUsMEJBQ0UsV0FBVyxFQUFFLEdBQUcsSUFDYixPQUFPLEVBQ1Y7QUFDSixDQUFDOztBQUVELE1BQU0sS0FBTyxlQUFlLEdBQUcsSUFBSSxjQUFjLENBQUMsaUJBQWlCLENBQUM7U0FJNkIsSUFBSTtBQUZyRztJQUFBO0lBa0JBLENBQUM7Ozs7O0lBYlEsMkJBQU87Ozs7SUFBZCxVQUFlLE9BQW9DO1FBQXBDLHdCQUFBLEVBQUEsNkJBQVUsRUFBRSxFQUF3QjtRQUNqRCxPQUFPO1lBQ0wsUUFBUSxFQUFFLG1CQUFtQjtZQUM3QixTQUFTLEVBQUU7Z0JBQ1QsRUFBRSxPQUFPLEVBQUUsZUFBZSxFQUFFLFFBQVEsRUFBRSxPQUFPLEVBQUU7Z0JBQy9DO29CQUNFLE9BQU8sRUFBRSxpQkFBaUI7b0JBQzFCLFVBQVUsRUFBRSxxQkFBcUI7b0JBQ2pDLElBQUksRUFBRSxDQUFDLGVBQWUsQ0FBQztpQkFDeEI7YUFDRjtTQUNGLENBQUM7SUFDSixDQUFDOztnQkFqQkYsUUFBUSxTQUFDO29CQUNSLE9BQU8sRUFBRSxDQUFDLFVBQVUsRUFBRSxpQkFBaUIsQ0FBQztvQkFDeEMsU0FBUyxFQUFFLENBQUMsRUFBRSxPQUFPLEVBQUUsZUFBZSxFQUFFLEtBQUssRUFBRSxJQUFJLEVBQUUsSUFBSSxFQUFFLENBQUMsb0JBQW9CLENBQUMsRUFBRSxVQUFVLElBQU0sRUFBRSxDQUFDO2lCQUN2Rzs7SUFlRCwwQkFBQztDQUFBLEFBbEJELElBa0JDO1NBZFksbUJBQW1CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29yZU1vZHVsZSwgbm9vcCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBUaGVtZVNoYXJlZE1vZHVsZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IEFQUF9JTklUSUFMSVpFUiwgSW5qZWN0aW9uVG9rZW4sIE1vZHVsZVdpdGhQcm92aWRlcnMsIE5nTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBBY2NvdW50Q29uZmlnU2VydmljZSB9IGZyb20gJy4vc2VydmljZXMvYWNjb3VudC1jb25maWcuc2VydmljZSc7XG5cbmV4cG9ydCBpbnRlcmZhY2UgQWNjb3VudENvbmZpZ09wdGlvbnMge1xuICByZWRpcmVjdFVybD86IHN0cmluZztcbn1cblxuZXhwb3J0IGZ1bmN0aW9uIGFjY291bnRPcHRpb25zRmFjdG9yeShvcHRpb25zOiBBY2NvdW50Q29uZmlnT3B0aW9ucykge1xuICByZXR1cm4ge1xuICAgIHJlZGlyZWN0VXJsOiAnLycsXG4gICAgLi4ub3B0aW9ucyxcbiAgfTtcbn1cblxuZXhwb3J0IGNvbnN0IEFDQ09VTlRfT1BUSU9OUyA9IG5ldyBJbmplY3Rpb25Ub2tlbignQUNDT1VOVF9PUFRJT05TJyk7XG5cbkBOZ01vZHVsZSh7XG4gIGltcG9ydHM6IFtDb3JlTW9kdWxlLCBUaGVtZVNoYXJlZE1vZHVsZV0sXG4gIHByb3ZpZGVyczogW3sgcHJvdmlkZTogQVBQX0lOSVRJQUxJWkVSLCBtdWx0aTogdHJ1ZSwgZGVwczogW0FjY291bnRDb25maWdTZXJ2aWNlXSwgdXNlRmFjdG9yeTogbm9vcCB9XSxcbn0pXG5leHBvcnQgY2xhc3MgQWNjb3VudENvbmZpZ01vZHVsZSB7XG4gIHN0YXRpYyBmb3JSb290KG9wdGlvbnMgPSB7fSBhcyBBY2NvdW50Q29uZmlnT3B0aW9ucyk6IE1vZHVsZVdpdGhQcm92aWRlcnMge1xuICAgIHJldHVybiB7XG4gICAgICBuZ01vZHVsZTogQWNjb3VudENvbmZpZ01vZHVsZSxcbiAgICAgIHByb3ZpZGVyczogW1xuICAgICAgICB7IHByb3ZpZGU6IEFDQ09VTlRfT1BUSU9OUywgdXNlVmFsdWU6IG9wdGlvbnMgfSxcbiAgICAgICAge1xuICAgICAgICAgIHByb3ZpZGU6ICdBQ0NPVU5UX09QVElPTlMnLFxuICAgICAgICAgIHVzZUZhY3Rvcnk6IGFjY291bnRPcHRpb25zRmFjdG9yeSxcbiAgICAgICAgICBkZXBzOiBbQUNDT1VOVF9PUFRJT05TXSxcbiAgICAgICAgfSxcbiAgICAgIF0sXG4gICAgfTtcbiAgfVxufVxuIl19