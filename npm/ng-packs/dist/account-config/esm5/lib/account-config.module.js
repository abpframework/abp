/**
 * @fileoverview added by tsickle
 * Generated from: lib/account-config.module.ts
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC1jb25maWcubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LmNvbmZpZy8iLCJzb3VyY2VzIjpbImxpYi9hY2NvdW50LWNvbmZpZy5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxJQUFJLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDaEQsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDekQsT0FBTyxFQUFFLGVBQWUsRUFBRSxjQUFjLEVBQXVCLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMvRixPQUFPLEVBQUUsb0JBQW9CLEVBQUUsTUFBTSxtQ0FBbUMsQ0FBQzs7OztBQUV6RSwwQ0FFQzs7O0lBREMsMkNBQXFCOzs7Ozs7QUFHdkIsTUFBTSxVQUFVLHFCQUFxQixDQUFDLE9BQTZCO0lBQ2pFLDBCQUNFLFdBQVcsRUFBRSxHQUFHLElBQ2IsT0FBTyxFQUNWO0FBQ0osQ0FBQzs7QUFFRCxNQUFNLEtBQU8sZUFBZSxHQUFHLElBQUksY0FBYyxDQUFDLGlCQUFpQixDQUFDO1NBSTZCLElBQUk7QUFGckc7SUFBQTtJQWtCQSxDQUFDOzs7OztJQWJRLDJCQUFPOzs7O0lBQWQsVUFBZSxPQUFvQztRQUFwQyx3QkFBQSxFQUFBLDZCQUFVLEVBQUUsRUFBd0I7UUFDakQsT0FBTztZQUNMLFFBQVEsRUFBRSxtQkFBbUI7WUFDN0IsU0FBUyxFQUFFO2dCQUNULEVBQUUsT0FBTyxFQUFFLGVBQWUsRUFBRSxRQUFRLEVBQUUsT0FBTyxFQUFFO2dCQUMvQztvQkFDRSxPQUFPLEVBQUUsaUJBQWlCO29CQUMxQixVQUFVLEVBQUUscUJBQXFCO29CQUNqQyxJQUFJLEVBQUUsQ0FBQyxlQUFlLENBQUM7aUJBQ3hCO2FBQ0Y7U0FDRixDQUFDO0lBQ0osQ0FBQzs7Z0JBakJGLFFBQVEsU0FBQztvQkFDUixPQUFPLEVBQUUsQ0FBQyxVQUFVLEVBQUUsaUJBQWlCLENBQUM7b0JBQ3hDLFNBQVMsRUFBRSxDQUFDLEVBQUUsT0FBTyxFQUFFLGVBQWUsRUFBRSxLQUFLLEVBQUUsSUFBSSxFQUFFLElBQUksRUFBRSxDQUFDLG9CQUFvQixDQUFDLEVBQUUsVUFBVSxJQUFNLEVBQUUsQ0FBQztpQkFDdkc7O0lBZUQsMEJBQUM7Q0FBQSxBQWxCRCxJQWtCQztTQWRZLG1CQUFtQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvcmVNb2R1bGUsIG5vb3AgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgVGhlbWVTaGFyZWRNb2R1bGUgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBBUFBfSU5JVElBTElaRVIsIEluamVjdGlvblRva2VuLCBNb2R1bGVXaXRoUHJvdmlkZXJzLCBOZ01vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgQWNjb3VudENvbmZpZ1NlcnZpY2UgfSBmcm9tICcuL3NlcnZpY2VzL2FjY291bnQtY29uZmlnLnNlcnZpY2UnO1xuXG5leHBvcnQgaW50ZXJmYWNlIEFjY291bnRDb25maWdPcHRpb25zIHtcbiAgcmVkaXJlY3RVcmw/OiBzdHJpbmc7XG59XG5cbmV4cG9ydCBmdW5jdGlvbiBhY2NvdW50T3B0aW9uc0ZhY3Rvcnkob3B0aW9uczogQWNjb3VudENvbmZpZ09wdGlvbnMpIHtcbiAgcmV0dXJuIHtcbiAgICByZWRpcmVjdFVybDogJy8nLFxuICAgIC4uLm9wdGlvbnMsXG4gIH07XG59XG5cbmV4cG9ydCBjb25zdCBBQ0NPVU5UX09QVElPTlMgPSBuZXcgSW5qZWN0aW9uVG9rZW4oJ0FDQ09VTlRfT1BUSU9OUycpO1xuXG5ATmdNb2R1bGUoe1xuICBpbXBvcnRzOiBbQ29yZU1vZHVsZSwgVGhlbWVTaGFyZWRNb2R1bGVdLFxuICBwcm92aWRlcnM6IFt7IHByb3ZpZGU6IEFQUF9JTklUSUFMSVpFUiwgbXVsdGk6IHRydWUsIGRlcHM6IFtBY2NvdW50Q29uZmlnU2VydmljZV0sIHVzZUZhY3Rvcnk6IG5vb3AgfV0sXG59KVxuZXhwb3J0IGNsYXNzIEFjY291bnRDb25maWdNb2R1bGUge1xuICBzdGF0aWMgZm9yUm9vdChvcHRpb25zID0ge30gYXMgQWNjb3VudENvbmZpZ09wdGlvbnMpOiBNb2R1bGVXaXRoUHJvdmlkZXJzIHtcbiAgICByZXR1cm4ge1xuICAgICAgbmdNb2R1bGU6IEFjY291bnRDb25maWdNb2R1bGUsXG4gICAgICBwcm92aWRlcnM6IFtcbiAgICAgICAgeyBwcm92aWRlOiBBQ0NPVU5UX09QVElPTlMsIHVzZVZhbHVlOiBvcHRpb25zIH0sXG4gICAgICAgIHtcbiAgICAgICAgICBwcm92aWRlOiAnQUNDT1VOVF9PUFRJT05TJyxcbiAgICAgICAgICB1c2VGYWN0b3J5OiBhY2NvdW50T3B0aW9uc0ZhY3RvcnksXG4gICAgICAgICAgZGVwczogW0FDQ09VTlRfT1BUSU9OU10sXG4gICAgICAgIH0sXG4gICAgICBdLFxuICAgIH07XG4gIH1cbn1cbiJdfQ==