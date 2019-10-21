/**
 * @fileoverview added by tsickle
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
var AccountModule = /** @class */ (function () {
    function AccountModule() {
    }
    AccountModule.decorators = [
        { type: NgModule, args: [{
                    declarations: [
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmFjY291bnQvIiwic291cmNlcyI6WyJsaWIvYWNjb3VudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDMUMsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDekQsT0FBTyxFQUFFLFFBQVEsRUFBWSxNQUFNLGVBQWUsQ0FBQztBQUNuRCxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUMvRCxPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUMzRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLDBCQUEwQixDQUFDO0FBQ2hFLE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxNQUFNLHdEQUF3RCxDQUFDO0FBQ2pHLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxvQ0FBb0MsQ0FBQztBQUNwRSxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSxzREFBc0QsQ0FBQztBQUM5RixPQUFPLEVBQUUseUJBQXlCLEVBQUUsTUFBTSw0REFBNEQsQ0FBQztBQUN2RyxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSwwQ0FBMEMsQ0FBQztBQUM3RSxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSw4Q0FBOEMsQ0FBQztBQUVsRixPQUFPLEVBQUUsZUFBZSxFQUFFLGNBQWMsRUFBRSxNQUFNLHdCQUF3QixDQUFDO0FBRXpFO0lBQUE7SUFZNEIsQ0FBQzs7Z0JBWjVCLFFBQVEsU0FBQztvQkFDUixZQUFZLEVBQUU7d0JBQ1osY0FBYzt3QkFDZCxpQkFBaUI7d0JBQ2pCLGtCQUFrQjt3QkFDbEIsdUJBQXVCO3dCQUN2QixzQkFBc0I7d0JBQ3RCLHlCQUF5QjtxQkFDMUI7b0JBQ0QsT0FBTyxFQUFFLENBQUMsVUFBVSxFQUFFLG9CQUFvQixFQUFFLGlCQUFpQixFQUFFLFdBQVcsRUFBRSxpQkFBaUIsRUFBRSxxQkFBcUIsQ0FBQztvQkFDckgsT0FBTyxFQUFFLEVBQUU7aUJBQ1o7O0lBQzJCLG9CQUFDO0NBQUEsQUFaN0IsSUFZNkI7U0FBaEIsYUFBYTs7Ozs7OztBQU0xQixNQUFNLFVBQVUsZ0JBQWdCLENBQUMsT0FBdUI7SUFBdkIsd0JBQUEsRUFBQSw2QkFBVSxFQUFFLEVBQVc7SUFDdEQsT0FBTztRQUNMLEVBQUUsT0FBTyxFQUFFLGVBQWUsRUFBRSxRQUFRLEVBQUUsT0FBTyxFQUFFO1FBQy9DO1lBQ0UsT0FBTyxFQUFFLGlCQUFpQjtZQUMxQixVQUFVLEVBQUUsY0FBYztZQUMxQixJQUFJLEVBQUUsQ0FBQyxlQUFlLENBQUM7U0FDeEI7S0FDRixDQUFDO0FBQ0osQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvcmVNb2R1bGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgVGhlbWVTaGFyZWRNb2R1bGUgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBOZ01vZHVsZSwgUHJvdmlkZXIgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE5nYkRyb3Bkb3duTW9kdWxlIH0gZnJvbSAnQG5nLWJvb3RzdHJhcC9uZy1ib290c3RyYXAnO1xuaW1wb3J0IHsgTmd4VmFsaWRhdGVDb3JlTW9kdWxlIH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcbmltcG9ydCB7IFRhYmxlTW9kdWxlIH0gZnJvbSAncHJpbWVuZy90YWJsZSc7XG5pbXBvcnQgeyBBY2NvdW50Um91dGluZ01vZHVsZSB9IGZyb20gJy4vYWNjb3VudC1yb3V0aW5nLm1vZHVsZSc7XG5pbXBvcnQgeyBDaGFuZ2VQYXNzd29yZENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9jaGFuZ2UtcGFzc3dvcmQvY2hhbmdlLXBhc3N3b3JkLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBMb2dpbkNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9sb2dpbi9sb2dpbi5jb21wb25lbnQnO1xuaW1wb3J0IHsgTWFuYWdlUHJvZmlsZUNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9tYW5hZ2UtcHJvZmlsZS9tYW5hZ2UtcHJvZmlsZS5jb21wb25lbnQnO1xuaW1wb3J0IHsgUGVyc29uYWxTZXR0aW5nc0NvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9wZXJzb25hbC1zZXR0aW5ncy9wZXJzb25hbC1zZXR0aW5ncy5jb21wb25lbnQnO1xuaW1wb3J0IHsgUmVnaXN0ZXJDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvcmVnaXN0ZXIvcmVnaXN0ZXIuY29tcG9uZW50JztcbmltcG9ydCB7IFRlbmFudEJveENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy90ZW5hbnQtYm94L3RlbmFudC1ib3guY29tcG9uZW50JztcbmltcG9ydCB7IE9wdGlvbnMgfSBmcm9tICcuL21vZGVscy9vcHRpb25zJztcbmltcG9ydCB7IEFDQ09VTlRfT1BUSU9OUywgb3B0aW9uc0ZhY3RvcnkgfSBmcm9tICcuL3Rva2Vucy9vcHRpb25zLnRva2VuJztcblxuQE5nTW9kdWxlKHtcbiAgZGVjbGFyYXRpb25zOiBbXG4gICAgTG9naW5Db21wb25lbnQsXG4gICAgUmVnaXN0ZXJDb21wb25lbnQsXG4gICAgVGVuYW50Qm94Q29tcG9uZW50LFxuICAgIENoYW5nZVBhc3N3b3JkQ29tcG9uZW50LFxuICAgIE1hbmFnZVByb2ZpbGVDb21wb25lbnQsXG4gICAgUGVyc29uYWxTZXR0aW5nc0NvbXBvbmVudCxcbiAgXSxcbiAgaW1wb3J0czogW0NvcmVNb2R1bGUsIEFjY291bnRSb3V0aW5nTW9kdWxlLCBUaGVtZVNoYXJlZE1vZHVsZSwgVGFibGVNb2R1bGUsIE5nYkRyb3Bkb3duTW9kdWxlLCBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGVdLFxuICBleHBvcnRzOiBbXSxcbn0pXG5leHBvcnQgY2xhc3MgQWNjb3VudE1vZHVsZSB7fVxuXG4vKipcbiAqXG4gKiBAZGVwcmVjYXRlZCBzaW5jZSB2ZXJzaW9uIDAuOVxuICovXG5leHBvcnQgZnVuY3Rpb24gQWNjb3VudFByb3ZpZGVycyhvcHRpb25zID0ge30gYXMgT3B0aW9ucyk6IFByb3ZpZGVyW10ge1xuICByZXR1cm4gW1xuICAgIHsgcHJvdmlkZTogQUNDT1VOVF9PUFRJT05TLCB1c2VWYWx1ZTogb3B0aW9ucyB9LFxuICAgIHtcbiAgICAgIHByb3ZpZGU6ICdBQ0NPVU5UX09QVElPTlMnLFxuICAgICAgdXNlRmFjdG9yeTogb3B0aW9uc0ZhY3RvcnksXG4gICAgICBkZXBzOiBbQUNDT1VOVF9PUFRJT05TXSxcbiAgICB9LFxuICBdO1xufVxuIl19