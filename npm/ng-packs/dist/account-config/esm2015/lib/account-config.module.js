/**
 * @fileoverview added by tsickle
 * Generated from: lib/account-config.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
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
    return Object.assign({ redirectUrl: '/' }, options);
}
/** @type {?} */
export const ACCOUNT_OPTIONS = new InjectionToken('ACCOUNT_OPTIONS');
const ɵ0 = noop;
export class AccountConfigModule {
    /**
     * @param {?=} options
     * @return {?}
     */
    static forRoot(options = (/** @type {?} */ ({}))) {
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
    }
}
AccountConfigModule.decorators = [
    { type: NgModule, args: [{
                imports: [CoreModule, ThemeSharedModule],
                providers: [{ provide: APP_INITIALIZER, multi: true, deps: [AccountConfigService], useFactory: ɵ0 }],
            },] }
];
export { ɵ0 };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC1jb25maWcubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LmNvbmZpZy8iLCJzb3VyY2VzIjpbImxpYi9hY2NvdW50LWNvbmZpZy5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLElBQUksRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUNoRCxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUN6RCxPQUFPLEVBQUUsZUFBZSxFQUFFLGNBQWMsRUFBdUIsUUFBUSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQy9GLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLG1DQUFtQyxDQUFDOzs7O0FBRXpFLDBDQUVDOzs7SUFEQywyQ0FBcUI7Ozs7OztBQUd2QixNQUFNLFVBQVUscUJBQXFCLENBQUMsT0FBNkI7SUFDakUsdUJBQ0UsV0FBVyxFQUFFLEdBQUcsSUFDYixPQUFPLEVBQ1Y7QUFDSixDQUFDOztBQUVELE1BQU0sT0FBTyxlQUFlLEdBQUcsSUFBSSxjQUFjLENBQUMsaUJBQWlCLENBQUM7V0FJNkIsSUFBSTtBQUVyRyxNQUFNLE9BQU8sbUJBQW1COzs7OztJQUM5QixNQUFNLENBQUMsT0FBTyxDQUFDLE9BQU8sR0FBRyxtQkFBQSxFQUFFLEVBQXdCO1FBQ2pELE9BQU87WUFDTCxRQUFRLEVBQUUsbUJBQW1CO1lBQzdCLFNBQVMsRUFBRTtnQkFDVCxFQUFFLE9BQU8sRUFBRSxlQUFlLEVBQUUsUUFBUSxFQUFFLE9BQU8sRUFBRTtnQkFDL0M7b0JBQ0UsT0FBTyxFQUFFLGlCQUFpQjtvQkFDMUIsVUFBVSxFQUFFLHFCQUFxQjtvQkFDakMsSUFBSSxFQUFFLENBQUMsZUFBZSxDQUFDO2lCQUN4QjthQUNGO1NBQ0YsQ0FBQztJQUNKLENBQUM7OztZQWpCRixRQUFRLFNBQUM7Z0JBQ1IsT0FBTyxFQUFFLENBQUMsVUFBVSxFQUFFLGlCQUFpQixDQUFDO2dCQUN4QyxTQUFTLEVBQUUsQ0FBQyxFQUFFLE9BQU8sRUFBRSxlQUFlLEVBQUUsS0FBSyxFQUFFLElBQUksRUFBRSxJQUFJLEVBQUUsQ0FBQyxvQkFBb0IsQ0FBQyxFQUFFLFVBQVUsSUFBTSxFQUFFLENBQUM7YUFDdkciLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb3JlTW9kdWxlLCBub29wIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgVGhlbWVTaGFyZWRNb2R1bGUgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XHJcbmltcG9ydCB7IEFQUF9JTklUSUFMSVpFUiwgSW5qZWN0aW9uVG9rZW4sIE1vZHVsZVdpdGhQcm92aWRlcnMsIE5nTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IEFjY291bnRDb25maWdTZXJ2aWNlIH0gZnJvbSAnLi9zZXJ2aWNlcy9hY2NvdW50LWNvbmZpZy5zZXJ2aWNlJztcclxuXHJcbmV4cG9ydCBpbnRlcmZhY2UgQWNjb3VudENvbmZpZ09wdGlvbnMge1xyXG4gIHJlZGlyZWN0VXJsPzogc3RyaW5nO1xyXG59XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gYWNjb3VudE9wdGlvbnNGYWN0b3J5KG9wdGlvbnM6IEFjY291bnRDb25maWdPcHRpb25zKSB7XHJcbiAgcmV0dXJuIHtcclxuICAgIHJlZGlyZWN0VXJsOiAnLycsXHJcbiAgICAuLi5vcHRpb25zLFxyXG4gIH07XHJcbn1cclxuXHJcbmV4cG9ydCBjb25zdCBBQ0NPVU5UX09QVElPTlMgPSBuZXcgSW5qZWN0aW9uVG9rZW4oJ0FDQ09VTlRfT1BUSU9OUycpO1xyXG5cclxuQE5nTW9kdWxlKHtcclxuICBpbXBvcnRzOiBbQ29yZU1vZHVsZSwgVGhlbWVTaGFyZWRNb2R1bGVdLFxyXG4gIHByb3ZpZGVyczogW3sgcHJvdmlkZTogQVBQX0lOSVRJQUxJWkVSLCBtdWx0aTogdHJ1ZSwgZGVwczogW0FjY291bnRDb25maWdTZXJ2aWNlXSwgdXNlRmFjdG9yeTogbm9vcCB9XSxcclxufSlcclxuZXhwb3J0IGNsYXNzIEFjY291bnRDb25maWdNb2R1bGUge1xyXG4gIHN0YXRpYyBmb3JSb290KG9wdGlvbnMgPSB7fSBhcyBBY2NvdW50Q29uZmlnT3B0aW9ucyk6IE1vZHVsZVdpdGhQcm92aWRlcnMge1xyXG4gICAgcmV0dXJuIHtcclxuICAgICAgbmdNb2R1bGU6IEFjY291bnRDb25maWdNb2R1bGUsXHJcbiAgICAgIHByb3ZpZGVyczogW1xyXG4gICAgICAgIHsgcHJvdmlkZTogQUNDT1VOVF9PUFRJT05TLCB1c2VWYWx1ZTogb3B0aW9ucyB9LFxyXG4gICAgICAgIHtcclxuICAgICAgICAgIHByb3ZpZGU6ICdBQ0NPVU5UX09QVElPTlMnLFxyXG4gICAgICAgICAgdXNlRmFjdG9yeTogYWNjb3VudE9wdGlvbnNGYWN0b3J5LFxyXG4gICAgICAgICAgZGVwczogW0FDQ09VTlRfT1BUSU9OU10sXHJcbiAgICAgICAgfSxcclxuICAgICAgXSxcclxuICAgIH07XHJcbiAgfVxyXG59XHJcbiJdfQ==