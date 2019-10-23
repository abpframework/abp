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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC1jb25maWcubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LmNvbmZpZy8iLCJzb3VyY2VzIjpbImxpYi9hY2NvdW50LWNvbmZpZy5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLElBQUksRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUNoRCxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUN6RCxPQUFPLEVBQUUsZUFBZSxFQUFFLGNBQWMsRUFBdUIsUUFBUSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQy9GLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLG1DQUFtQyxDQUFDOzs7O0FBRXpFLDBDQUVDOzs7SUFEQywyQ0FBcUI7Ozs7OztBQUd2QixNQUFNLFVBQVUscUJBQXFCLENBQUMsT0FBNkI7SUFDakUsMEJBQ0UsV0FBVyxFQUFFLEdBQUcsSUFDYixPQUFPLEVBQ1Y7QUFDSixDQUFDOztBQUVELE1BQU0sS0FBTyxlQUFlLEdBQUcsSUFBSSxjQUFjLENBQUMsaUJBQWlCLENBQUM7U0FJNkIsSUFBSTtBQUZyRztJQUFBO0lBa0JBLENBQUM7Ozs7O0lBYlEsMkJBQU87Ozs7SUFBZCxVQUFlLE9BQW9DO1FBQXBDLHdCQUFBLEVBQUEsNkJBQVUsRUFBRSxFQUF3QjtRQUNqRCxPQUFPO1lBQ0wsUUFBUSxFQUFFLG1CQUFtQjtZQUM3QixTQUFTLEVBQUU7Z0JBQ1QsRUFBRSxPQUFPLEVBQUUsZUFBZSxFQUFFLFFBQVEsRUFBRSxPQUFPLEVBQUU7Z0JBQy9DO29CQUNFLE9BQU8sRUFBRSxpQkFBaUI7b0JBQzFCLFVBQVUsRUFBRSxxQkFBcUI7b0JBQ2pDLElBQUksRUFBRSxDQUFDLGVBQWUsQ0FBQztpQkFDeEI7YUFDRjtTQUNGLENBQUM7SUFDSixDQUFDOztnQkFqQkYsUUFBUSxTQUFDO29CQUNSLE9BQU8sRUFBRSxDQUFDLFVBQVUsRUFBRSxpQkFBaUIsQ0FBQztvQkFDeEMsU0FBUyxFQUFFLENBQUMsRUFBRSxPQUFPLEVBQUUsZUFBZSxFQUFFLEtBQUssRUFBRSxJQUFJLEVBQUUsSUFBSSxFQUFFLENBQUMsb0JBQW9CLENBQUMsRUFBRSxVQUFVLElBQU0sRUFBRSxDQUFDO2lCQUN2Rzs7SUFlRCwwQkFBQztDQUFBLEFBbEJELElBa0JDO1NBZFksbUJBQW1CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29yZU1vZHVsZSwgbm9vcCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcbmltcG9ydCB7IFRoZW1lU2hhcmVkTW9kdWxlIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xyXG5pbXBvcnQgeyBBUFBfSU5JVElBTElaRVIsIEluamVjdGlvblRva2VuLCBNb2R1bGVXaXRoUHJvdmlkZXJzLCBOZ01vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBBY2NvdW50Q29uZmlnU2VydmljZSB9IGZyb20gJy4vc2VydmljZXMvYWNjb3VudC1jb25maWcuc2VydmljZSc7XHJcblxyXG5leHBvcnQgaW50ZXJmYWNlIEFjY291bnRDb25maWdPcHRpb25zIHtcclxuICByZWRpcmVjdFVybD86IHN0cmluZztcclxufVxyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIGFjY291bnRPcHRpb25zRmFjdG9yeShvcHRpb25zOiBBY2NvdW50Q29uZmlnT3B0aW9ucykge1xyXG4gIHJldHVybiB7XHJcbiAgICByZWRpcmVjdFVybDogJy8nLFxyXG4gICAgLi4ub3B0aW9ucyxcclxuICB9O1xyXG59XHJcblxyXG5leHBvcnQgY29uc3QgQUNDT1VOVF9PUFRJT05TID0gbmV3IEluamVjdGlvblRva2VuKCdBQ0NPVU5UX09QVElPTlMnKTtcclxuXHJcbkBOZ01vZHVsZSh7XHJcbiAgaW1wb3J0czogW0NvcmVNb2R1bGUsIFRoZW1lU2hhcmVkTW9kdWxlXSxcclxuICBwcm92aWRlcnM6IFt7IHByb3ZpZGU6IEFQUF9JTklUSUFMSVpFUiwgbXVsdGk6IHRydWUsIGRlcHM6IFtBY2NvdW50Q29uZmlnU2VydmljZV0sIHVzZUZhY3Rvcnk6IG5vb3AgfV0sXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBBY2NvdW50Q29uZmlnTW9kdWxlIHtcclxuICBzdGF0aWMgZm9yUm9vdChvcHRpb25zID0ge30gYXMgQWNjb3VudENvbmZpZ09wdGlvbnMpOiBNb2R1bGVXaXRoUHJvdmlkZXJzIHtcclxuICAgIHJldHVybiB7XHJcbiAgICAgIG5nTW9kdWxlOiBBY2NvdW50Q29uZmlnTW9kdWxlLFxyXG4gICAgICBwcm92aWRlcnM6IFtcclxuICAgICAgICB7IHByb3ZpZGU6IEFDQ09VTlRfT1BUSU9OUywgdXNlVmFsdWU6IG9wdGlvbnMgfSxcclxuICAgICAgICB7XHJcbiAgICAgICAgICBwcm92aWRlOiAnQUNDT1VOVF9PUFRJT05TJyxcclxuICAgICAgICAgIHVzZUZhY3Rvcnk6IGFjY291bnRPcHRpb25zRmFjdG9yeSxcclxuICAgICAgICAgIGRlcHM6IFtBQ0NPVU5UX09QVElPTlNdLFxyXG4gICAgICAgIH0sXHJcbiAgICAgIF0sXHJcbiAgICB9O1xyXG4gIH1cclxufVxyXG4iXX0=