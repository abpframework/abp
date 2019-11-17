/**
 * @fileoverview added by tsickle
 * Generated from: lib/account.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { TableModule } from 'primeng/table';
import { AccountRoutingModule } from './account-routing.module';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { LoginComponent } from './components/login/login.component';
import { ManageProfileComponent } from './components/manage-profile/manage-profile.component';
import { PersonalSettingsComponent } from './components/personal-settings/personal-settings.component';
import { RegisterComponent } from './components/register/register.component';
import { TenantBoxComponent } from './components/tenant-box/tenant-box.component';
import { ACCOUNT_OPTIONS, optionsFactory } from './tokens/options.token';
import { AuthWrapperComponent } from './components/auth-wrapper/auth-wrapper.component';
var AccountModule = /** @class */ (function () {
    function AccountModule() {
    }
    AccountModule.decorators = [
        { type: NgModule, args: [{
                    declarations: [
                        AuthWrapperComponent,
                        LoginComponent,
                        RegisterComponent,
                        TenantBoxComponent,
                        ChangePasswordComponent,
                        ManageProfileComponent,
                        PersonalSettingsComponent,
                    ],
                    imports: [CoreModule, AccountRoutingModule, ThemeSharedModule, TableModule, NgbDropdownModule, NgxValidateCoreModule],
                    exports: [],
                },] }
    ];
    return AccountModule;
}());
export { AccountModule };
/**
 *
 * @deprecated since version 0.9
 * @param {?=} options
 * @return {?}
 */
export function AccountProviders(options) {
    if (options === void 0) { options = (/** @type {?} */ ({})); }
    return [
        { provide: ACCOUNT_OPTIONS, useValue: options },
        {
            provide: 'ACCOUNT_OPTIONS',
            useFactory: optionsFactory,
            deps: [ACCOUNT_OPTIONS],
        },
    ];
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmFjY291bnQvIiwic291cmNlcyI6WyJsaWIvYWNjb3VudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzFDLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFBRSxRQUFRLEVBQVksTUFBTSxlQUFlLENBQUM7QUFDbkQsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sNEJBQTRCLENBQUM7QUFDL0QsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFDM0QsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUM1QyxPQUFPLEVBQUUsb0JBQW9CLEVBQUUsTUFBTSwwQkFBMEIsQ0FBQztBQUNoRSxPQUFPLEVBQUUsdUJBQXVCLEVBQUUsTUFBTSx3REFBd0QsQ0FBQztBQUNqRyxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDcEUsT0FBTyxFQUFFLHNCQUFzQixFQUFFLE1BQU0sc0RBQXNELENBQUM7QUFDOUYsT0FBTyxFQUFFLHlCQUF5QixFQUFFLE1BQU0sNERBQTRELENBQUM7QUFDdkcsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sMENBQTBDLENBQUM7QUFDN0UsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0sOENBQThDLENBQUM7QUFFbEYsT0FBTyxFQUFFLGVBQWUsRUFBRSxjQUFjLEVBQUUsTUFBTSx3QkFBd0IsQ0FBQztBQUN6RSxPQUFPLEVBQUUsb0JBQW9CLEVBQUUsTUFBTSxrREFBa0QsQ0FBQztBQUV4RjtJQUFBO0lBYTRCLENBQUM7O2dCQWI1QixRQUFRLFNBQUM7b0JBQ1IsWUFBWSxFQUFFO3dCQUNaLG9CQUFvQjt3QkFDcEIsY0FBYzt3QkFDZCxpQkFBaUI7d0JBQ2pCLGtCQUFrQjt3QkFDbEIsdUJBQXVCO3dCQUN2QixzQkFBc0I7d0JBQ3RCLHlCQUF5QjtxQkFDMUI7b0JBQ0QsT0FBTyxFQUFFLENBQUMsVUFBVSxFQUFFLG9CQUFvQixFQUFFLGlCQUFpQixFQUFFLFdBQVcsRUFBRSxpQkFBaUIsRUFBRSxxQkFBcUIsQ0FBQztvQkFDckgsT0FBTyxFQUFFLEVBQUU7aUJBQ1o7O0lBQzJCLG9CQUFDO0NBQUEsQUFiN0IsSUFhNkI7U0FBaEIsYUFBYTs7Ozs7OztBQU0xQixNQUFNLFVBQVUsZ0JBQWdCLENBQUMsT0FBdUI7SUFBdkIsd0JBQUEsRUFBQSw2QkFBVSxFQUFFLEVBQVc7SUFDdEQsT0FBTztRQUNMLEVBQUUsT0FBTyxFQUFFLGVBQWUsRUFBRSxRQUFRLEVBQUUsT0FBTyxFQUFFO1FBQy9DO1lBQ0UsT0FBTyxFQUFFLGlCQUFpQjtZQUMxQixVQUFVLEVBQUUsY0FBYztZQUMxQixJQUFJLEVBQUUsQ0FBQyxlQUFlLENBQUM7U0FDeEI7S0FDRixDQUFDO0FBQ0osQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvcmVNb2R1bGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xyXG5pbXBvcnQgeyBUaGVtZVNoYXJlZE1vZHVsZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcclxuaW1wb3J0IHsgTmdNb2R1bGUsIFByb3ZpZGVyIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IE5nYkRyb3Bkb3duTW9kdWxlIH0gZnJvbSAnQG5nLWJvb3RzdHJhcC9uZy1ib290c3RyYXAnO1xyXG5pbXBvcnQgeyBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGUgfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xyXG5pbXBvcnQgeyBUYWJsZU1vZHVsZSB9IGZyb20gJ3ByaW1lbmcvdGFibGUnO1xyXG5pbXBvcnQgeyBBY2NvdW50Um91dGluZ01vZHVsZSB9IGZyb20gJy4vYWNjb3VudC1yb3V0aW5nLm1vZHVsZSc7XHJcbmltcG9ydCB7IENoYW5nZVBhc3N3b3JkQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2NoYW5nZS1wYXNzd29yZC9jaGFuZ2UtcGFzc3dvcmQuY29tcG9uZW50JztcclxuaW1wb3J0IHsgTG9naW5Db21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvbG9naW4vbG9naW4uY29tcG9uZW50JztcclxuaW1wb3J0IHsgTWFuYWdlUHJvZmlsZUNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9tYW5hZ2UtcHJvZmlsZS9tYW5hZ2UtcHJvZmlsZS5jb21wb25lbnQnO1xyXG5pbXBvcnQgeyBQZXJzb25hbFNldHRpbmdzQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3BlcnNvbmFsLXNldHRpbmdzL3BlcnNvbmFsLXNldHRpbmdzLmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IFJlZ2lzdGVyQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3JlZ2lzdGVyL3JlZ2lzdGVyLmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IFRlbmFudEJveENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy90ZW5hbnQtYm94L3RlbmFudC1ib3guY29tcG9uZW50JztcclxuaW1wb3J0IHsgT3B0aW9ucyB9IGZyb20gJy4vbW9kZWxzL29wdGlvbnMnO1xyXG5pbXBvcnQgeyBBQ0NPVU5UX09QVElPTlMsIG9wdGlvbnNGYWN0b3J5IH0gZnJvbSAnLi90b2tlbnMvb3B0aW9ucy50b2tlbic7XHJcbmltcG9ydCB7IEF1dGhXcmFwcGVyQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2F1dGgtd3JhcHBlci9hdXRoLXdyYXBwZXIuY29tcG9uZW50JztcclxuXHJcbkBOZ01vZHVsZSh7XHJcbiAgZGVjbGFyYXRpb25zOiBbXHJcbiAgICBBdXRoV3JhcHBlckNvbXBvbmVudCxcclxuICAgIExvZ2luQ29tcG9uZW50LFxyXG4gICAgUmVnaXN0ZXJDb21wb25lbnQsXHJcbiAgICBUZW5hbnRCb3hDb21wb25lbnQsXHJcbiAgICBDaGFuZ2VQYXNzd29yZENvbXBvbmVudCxcclxuICAgIE1hbmFnZVByb2ZpbGVDb21wb25lbnQsXHJcbiAgICBQZXJzb25hbFNldHRpbmdzQ29tcG9uZW50LFxyXG4gIF0sXHJcbiAgaW1wb3J0czogW0NvcmVNb2R1bGUsIEFjY291bnRSb3V0aW5nTW9kdWxlLCBUaGVtZVNoYXJlZE1vZHVsZSwgVGFibGVNb2R1bGUsIE5nYkRyb3Bkb3duTW9kdWxlLCBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGVdLFxyXG4gIGV4cG9ydHM6IFtdLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgQWNjb3VudE1vZHVsZSB7fVxyXG5cclxuLyoqXHJcbiAqXHJcbiAqIEBkZXByZWNhdGVkIHNpbmNlIHZlcnNpb24gMC45XHJcbiAqL1xyXG5leHBvcnQgZnVuY3Rpb24gQWNjb3VudFByb3ZpZGVycyhvcHRpb25zID0ge30gYXMgT3B0aW9ucyk6IFByb3ZpZGVyW10ge1xyXG4gIHJldHVybiBbXHJcbiAgICB7IHByb3ZpZGU6IEFDQ09VTlRfT1BUSU9OUywgdXNlVmFsdWU6IG9wdGlvbnMgfSxcclxuICAgIHtcclxuICAgICAgcHJvdmlkZTogJ0FDQ09VTlRfT1BUSU9OUycsXHJcbiAgICAgIHVzZUZhY3Rvcnk6IG9wdGlvbnNGYWN0b3J5LFxyXG4gICAgICBkZXBzOiBbQUNDT1VOVF9PUFRJT05TXSxcclxuICAgIH0sXHJcbiAgXTtcclxufVxyXG4iXX0=