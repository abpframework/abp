/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC1jb25maWcubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LmNvbmZpZy8iLCJzb3VyY2VzIjpbImxpYi9hY2NvdW50LWNvbmZpZy5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsSUFBSSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ2hELE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFBRSxlQUFlLEVBQUUsY0FBYyxFQUF1QixRQUFRLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDL0YsT0FBTyxFQUFFLG9CQUFvQixFQUFFLE1BQU0sbUNBQW1DLENBQUM7Ozs7QUFFekUsMENBRUM7OztJQURDLDJDQUFxQjs7Ozs7O0FBR3ZCLE1BQU0sVUFBVSxxQkFBcUIsQ0FBQyxPQUE2QjtJQUNqRSx1QkFDRSxXQUFXLEVBQUUsR0FBRyxJQUNiLE9BQU8sRUFDVjtBQUNKLENBQUM7O0FBRUQsTUFBTSxPQUFPLGVBQWUsR0FBRyxJQUFJLGNBQWMsQ0FBQyxpQkFBaUIsQ0FBQztXQUk2QixJQUFJO0FBRXJHLE1BQU0sT0FBTyxtQkFBbUI7Ozs7O0lBQzlCLE1BQU0sQ0FBQyxPQUFPLENBQUMsT0FBTyxHQUFHLG1CQUFBLEVBQUUsRUFBd0I7UUFDakQsT0FBTztZQUNMLFFBQVEsRUFBRSxtQkFBbUI7WUFDN0IsU0FBUyxFQUFFO2dCQUNULEVBQUUsT0FBTyxFQUFFLGVBQWUsRUFBRSxRQUFRLEVBQUUsT0FBTyxFQUFFO2dCQUMvQztvQkFDRSxPQUFPLEVBQUUsaUJBQWlCO29CQUMxQixVQUFVLEVBQUUscUJBQXFCO29CQUNqQyxJQUFJLEVBQUUsQ0FBQyxlQUFlLENBQUM7aUJBQ3hCO2FBQ0Y7U0FDRixDQUFDO0lBQ0osQ0FBQzs7O1lBakJGLFFBQVEsU0FBQztnQkFDUixPQUFPLEVBQUUsQ0FBQyxVQUFVLEVBQUUsaUJBQWlCLENBQUM7Z0JBQ3hDLFNBQVMsRUFBRSxDQUFDLEVBQUUsT0FBTyxFQUFFLGVBQWUsRUFBRSxLQUFLLEVBQUUsSUFBSSxFQUFFLElBQUksRUFBRSxDQUFDLG9CQUFvQixDQUFDLEVBQUUsVUFBVSxJQUFNLEVBQUUsQ0FBQzthQUN2RyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvcmVNb2R1bGUsIG5vb3AgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xyXG5pbXBvcnQgeyBUaGVtZVNoYXJlZE1vZHVsZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcclxuaW1wb3J0IHsgQVBQX0lOSVRJQUxJWkVSLCBJbmplY3Rpb25Ub2tlbiwgTW9kdWxlV2l0aFByb3ZpZGVycywgTmdNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgQWNjb3VudENvbmZpZ1NlcnZpY2UgfSBmcm9tICcuL3NlcnZpY2VzL2FjY291bnQtY29uZmlnLnNlcnZpY2UnO1xyXG5cclxuZXhwb3J0IGludGVyZmFjZSBBY2NvdW50Q29uZmlnT3B0aW9ucyB7XHJcbiAgcmVkaXJlY3RVcmw/OiBzdHJpbmc7XHJcbn1cclxuXHJcbmV4cG9ydCBmdW5jdGlvbiBhY2NvdW50T3B0aW9uc0ZhY3Rvcnkob3B0aW9uczogQWNjb3VudENvbmZpZ09wdGlvbnMpIHtcclxuICByZXR1cm4ge1xyXG4gICAgcmVkaXJlY3RVcmw6ICcvJyxcclxuICAgIC4uLm9wdGlvbnMsXHJcbiAgfTtcclxufVxyXG5cclxuZXhwb3J0IGNvbnN0IEFDQ09VTlRfT1BUSU9OUyA9IG5ldyBJbmplY3Rpb25Ub2tlbignQUNDT1VOVF9PUFRJT05TJyk7XHJcblxyXG5ATmdNb2R1bGUoe1xyXG4gIGltcG9ydHM6IFtDb3JlTW9kdWxlLCBUaGVtZVNoYXJlZE1vZHVsZV0sXHJcbiAgcHJvdmlkZXJzOiBbeyBwcm92aWRlOiBBUFBfSU5JVElBTElaRVIsIG11bHRpOiB0cnVlLCBkZXBzOiBbQWNjb3VudENvbmZpZ1NlcnZpY2VdLCB1c2VGYWN0b3J5OiBub29wIH1dLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgQWNjb3VudENvbmZpZ01vZHVsZSB7XHJcbiAgc3RhdGljIGZvclJvb3Qob3B0aW9ucyA9IHt9IGFzIEFjY291bnRDb25maWdPcHRpb25zKTogTW9kdWxlV2l0aFByb3ZpZGVycyB7XHJcbiAgICByZXR1cm4ge1xyXG4gICAgICBuZ01vZHVsZTogQWNjb3VudENvbmZpZ01vZHVsZSxcclxuICAgICAgcHJvdmlkZXJzOiBbXHJcbiAgICAgICAgeyBwcm92aWRlOiBBQ0NPVU5UX09QVElPTlMsIHVzZVZhbHVlOiBvcHRpb25zIH0sXHJcbiAgICAgICAge1xyXG4gICAgICAgICAgcHJvdmlkZTogJ0FDQ09VTlRfT1BUSU9OUycsXHJcbiAgICAgICAgICB1c2VGYWN0b3J5OiBhY2NvdW50T3B0aW9uc0ZhY3RvcnksXHJcbiAgICAgICAgICBkZXBzOiBbQUNDT1VOVF9PUFRJT05TXSxcclxuICAgICAgICB9LFxyXG4gICAgICBdLFxyXG4gICAgfTtcclxuICB9XHJcbn1cclxuIl19