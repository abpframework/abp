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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC1jb25maWcubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LmNvbmZpZy8iLCJzb3VyY2VzIjpbImxpYi9hY2NvdW50LWNvbmZpZy5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLElBQUksRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUNoRCxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUN6RCxPQUFPLEVBQUUsZUFBZSxFQUFFLGNBQWMsRUFBdUIsUUFBUSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQy9GLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLG1DQUFtQyxDQUFDOzs7O0FBRXpFLDBDQUVDOzs7SUFEQywyQ0FBcUI7Ozs7OztBQUd2QixNQUFNLFVBQVUscUJBQXFCLENBQUMsT0FBNkI7SUFDakUsdUJBQ0UsV0FBVyxFQUFFLEdBQUcsSUFDYixPQUFPLEVBQ1Y7QUFDSixDQUFDOztBQUVELE1BQU0sT0FBTyxlQUFlLEdBQUcsSUFBSSxjQUFjLENBQUMsaUJBQWlCLENBQUM7V0FJNkIsSUFBSTtBQUVyRyxNQUFNLE9BQU8sbUJBQW1COzs7OztJQUM5QixNQUFNLENBQUMsT0FBTyxDQUFDLE9BQU8sR0FBRyxtQkFBQSxFQUFFLEVBQXdCO1FBQ2pELE9BQU87WUFDTCxRQUFRLEVBQUUsbUJBQW1CO1lBQzdCLFNBQVMsRUFBRTtnQkFDVCxFQUFFLE9BQU8sRUFBRSxlQUFlLEVBQUUsUUFBUSxFQUFFLE9BQU8sRUFBRTtnQkFDL0M7b0JBQ0UsT0FBTyxFQUFFLGlCQUFpQjtvQkFDMUIsVUFBVSxFQUFFLHFCQUFxQjtvQkFDakMsSUFBSSxFQUFFLENBQUMsZUFBZSxDQUFDO2lCQUN4QjthQUNGO1NBQ0YsQ0FBQztJQUNKLENBQUM7OztZQWpCRixRQUFRLFNBQUM7Z0JBQ1IsT0FBTyxFQUFFLENBQUMsVUFBVSxFQUFFLGlCQUFpQixDQUFDO2dCQUN4QyxTQUFTLEVBQUUsQ0FBQyxFQUFFLE9BQU8sRUFBRSxlQUFlLEVBQUUsS0FBSyxFQUFFLElBQUksRUFBRSxJQUFJLEVBQUUsQ0FBQyxvQkFBb0IsQ0FBQyxFQUFFLFVBQVUsSUFBTSxFQUFFLENBQUM7YUFDdkciLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb3JlTW9kdWxlLCBub29wIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IFRoZW1lU2hhcmVkTW9kdWxlIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgQVBQX0lOSVRJQUxJWkVSLCBJbmplY3Rpb25Ub2tlbiwgTW9kdWxlV2l0aFByb3ZpZGVycywgTmdNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEFjY291bnRDb25maWdTZXJ2aWNlIH0gZnJvbSAnLi9zZXJ2aWNlcy9hY2NvdW50LWNvbmZpZy5zZXJ2aWNlJztcblxuZXhwb3J0IGludGVyZmFjZSBBY2NvdW50Q29uZmlnT3B0aW9ucyB7XG4gIHJlZGlyZWN0VXJsPzogc3RyaW5nO1xufVxuXG5leHBvcnQgZnVuY3Rpb24gYWNjb3VudE9wdGlvbnNGYWN0b3J5KG9wdGlvbnM6IEFjY291bnRDb25maWdPcHRpb25zKSB7XG4gIHJldHVybiB7XG4gICAgcmVkaXJlY3RVcmw6ICcvJyxcbiAgICAuLi5vcHRpb25zLFxuICB9O1xufVxuXG5leHBvcnQgY29uc3QgQUNDT1VOVF9PUFRJT05TID0gbmV3IEluamVjdGlvblRva2VuKCdBQ0NPVU5UX09QVElPTlMnKTtcblxuQE5nTW9kdWxlKHtcbiAgaW1wb3J0czogW0NvcmVNb2R1bGUsIFRoZW1lU2hhcmVkTW9kdWxlXSxcbiAgcHJvdmlkZXJzOiBbeyBwcm92aWRlOiBBUFBfSU5JVElBTElaRVIsIG11bHRpOiB0cnVlLCBkZXBzOiBbQWNjb3VudENvbmZpZ1NlcnZpY2VdLCB1c2VGYWN0b3J5OiBub29wIH1dLFxufSlcbmV4cG9ydCBjbGFzcyBBY2NvdW50Q29uZmlnTW9kdWxlIHtcbiAgc3RhdGljIGZvclJvb3Qob3B0aW9ucyA9IHt9IGFzIEFjY291bnRDb25maWdPcHRpb25zKTogTW9kdWxlV2l0aFByb3ZpZGVycyB7XG4gICAgcmV0dXJuIHtcbiAgICAgIG5nTW9kdWxlOiBBY2NvdW50Q29uZmlnTW9kdWxlLFxuICAgICAgcHJvdmlkZXJzOiBbXG4gICAgICAgIHsgcHJvdmlkZTogQUNDT1VOVF9PUFRJT05TLCB1c2VWYWx1ZTogb3B0aW9ucyB9LFxuICAgICAgICB7XG4gICAgICAgICAgcHJvdmlkZTogJ0FDQ09VTlRfT1BUSU9OUycsXG4gICAgICAgICAgdXNlRmFjdG9yeTogYWNjb3VudE9wdGlvbnNGYWN0b3J5LFxuICAgICAgICAgIGRlcHM6IFtBQ0NPVU5UX09QVElPTlNdLFxuICAgICAgICB9LFxuICAgICAgXSxcbiAgICB9O1xuICB9XG59XG4iXX0=