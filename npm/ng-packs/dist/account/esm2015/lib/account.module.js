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
export class AccountModule {
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
/**
 *
 * @deprecated since version 0.9
 * @param {?=} options
 * @return {?}
 */
export function AccountProviders(options = (/** @type {?} */ ({}))) {
    return [
        { provide: ACCOUNT_OPTIONS, useValue: options },
        {
            provide: 'ACCOUNT_OPTIONS',
            useFactory: optionsFactory,
            deps: [ACCOUNT_OPTIONS],
        },
    ];
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmFjY291bnQvIiwic291cmNlcyI6WyJsaWIvYWNjb3VudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDMUMsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDekQsT0FBTyxFQUFFLFFBQVEsRUFBWSxNQUFNLGVBQWUsQ0FBQztBQUNuRCxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUMvRCxPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUMzRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLDBCQUEwQixDQUFDO0FBQ2hFLE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxNQUFNLHdEQUF3RCxDQUFDO0FBQ2pHLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxvQ0FBb0MsQ0FBQztBQUNwRSxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSxzREFBc0QsQ0FBQztBQUM5RixPQUFPLEVBQUUseUJBQXlCLEVBQUUsTUFBTSw0REFBNEQsQ0FBQztBQUN2RyxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSwwQ0FBMEMsQ0FBQztBQUM3RSxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSw4Q0FBOEMsQ0FBQztBQUVsRixPQUFPLEVBQUUsZUFBZSxFQUFFLGNBQWMsRUFBRSxNQUFNLHdCQUF3QixDQUFDO0FBY3pFLE1BQU0sT0FBTyxhQUFhOzs7WUFaekIsUUFBUSxTQUFDO2dCQUNSLFlBQVksRUFBRTtvQkFDWixjQUFjO29CQUNkLGlCQUFpQjtvQkFDakIsa0JBQWtCO29CQUNsQix1QkFBdUI7b0JBQ3ZCLHNCQUFzQjtvQkFDdEIseUJBQXlCO2lCQUMxQjtnQkFDRCxPQUFPLEVBQUUsQ0FBQyxVQUFVLEVBQUUsb0JBQW9CLEVBQUUsaUJBQWlCLEVBQUUsV0FBVyxFQUFFLGlCQUFpQixFQUFFLHFCQUFxQixDQUFDO2dCQUNySCxPQUFPLEVBQUUsRUFBRTthQUNaOzs7Ozs7OztBQU9ELE1BQU0sVUFBVSxnQkFBZ0IsQ0FBQyxPQUFPLEdBQUcsbUJBQUEsRUFBRSxFQUFXO0lBQ3RELE9BQU87UUFDTCxFQUFFLE9BQU8sRUFBRSxlQUFlLEVBQUUsUUFBUSxFQUFFLE9BQU8sRUFBRTtRQUMvQztZQUNFLE9BQU8sRUFBRSxpQkFBaUI7WUFDMUIsVUFBVSxFQUFFLGNBQWM7WUFDMUIsSUFBSSxFQUFFLENBQUMsZUFBZSxDQUFDO1NBQ3hCO0tBQ0YsQ0FBQztBQUNKLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb3JlTW9kdWxlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgVGhlbWVTaGFyZWRNb2R1bGUgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XHJcbmltcG9ydCB7IE5nTW9kdWxlLCBQcm92aWRlciB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBOZ2JEcm9wZG93bk1vZHVsZSB9IGZyb20gJ0BuZy1ib290c3RyYXAvbmctYm9vdHN0cmFwJztcclxuaW1wb3J0IHsgTmd4VmFsaWRhdGVDb3JlTW9kdWxlIH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcclxuaW1wb3J0IHsgVGFibGVNb2R1bGUgfSBmcm9tICdwcmltZW5nL3RhYmxlJztcclxuaW1wb3J0IHsgQWNjb3VudFJvdXRpbmdNb2R1bGUgfSBmcm9tICcuL2FjY291bnQtcm91dGluZy5tb2R1bGUnO1xyXG5pbXBvcnQgeyBDaGFuZ2VQYXNzd29yZENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9jaGFuZ2UtcGFzc3dvcmQvY2hhbmdlLXBhc3N3b3JkLmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IExvZ2luQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2xvZ2luL2xvZ2luLmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IE1hbmFnZVByb2ZpbGVDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvbWFuYWdlLXByb2ZpbGUvbWFuYWdlLXByb2ZpbGUuY29tcG9uZW50JztcclxuaW1wb3J0IHsgUGVyc29uYWxTZXR0aW5nc0NvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9wZXJzb25hbC1zZXR0aW5ncy9wZXJzb25hbC1zZXR0aW5ncy5jb21wb25lbnQnO1xyXG5pbXBvcnQgeyBSZWdpc3RlckNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9yZWdpc3Rlci9yZWdpc3Rlci5jb21wb25lbnQnO1xyXG5pbXBvcnQgeyBUZW5hbnRCb3hDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvdGVuYW50LWJveC90ZW5hbnQtYm94LmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IE9wdGlvbnMgfSBmcm9tICcuL21vZGVscy9vcHRpb25zJztcclxuaW1wb3J0IHsgQUNDT1VOVF9PUFRJT05TLCBvcHRpb25zRmFjdG9yeSB9IGZyb20gJy4vdG9rZW5zL29wdGlvbnMudG9rZW4nO1xyXG5cclxuQE5nTW9kdWxlKHtcclxuICBkZWNsYXJhdGlvbnM6IFtcclxuICAgIExvZ2luQ29tcG9uZW50LFxyXG4gICAgUmVnaXN0ZXJDb21wb25lbnQsXHJcbiAgICBUZW5hbnRCb3hDb21wb25lbnQsXHJcbiAgICBDaGFuZ2VQYXNzd29yZENvbXBvbmVudCxcclxuICAgIE1hbmFnZVByb2ZpbGVDb21wb25lbnQsXHJcbiAgICBQZXJzb25hbFNldHRpbmdzQ29tcG9uZW50LFxyXG4gIF0sXHJcbiAgaW1wb3J0czogW0NvcmVNb2R1bGUsIEFjY291bnRSb3V0aW5nTW9kdWxlLCBUaGVtZVNoYXJlZE1vZHVsZSwgVGFibGVNb2R1bGUsIE5nYkRyb3Bkb3duTW9kdWxlLCBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGVdLFxyXG4gIGV4cG9ydHM6IFtdLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgQWNjb3VudE1vZHVsZSB7fVxyXG5cclxuLyoqXHJcbiAqXHJcbiAqIEBkZXByZWNhdGVkIHNpbmNlIHZlcnNpb24gMC45XHJcbiAqL1xyXG5leHBvcnQgZnVuY3Rpb24gQWNjb3VudFByb3ZpZGVycyhvcHRpb25zID0ge30gYXMgT3B0aW9ucyk6IFByb3ZpZGVyW10ge1xyXG4gIHJldHVybiBbXHJcbiAgICB7IHByb3ZpZGU6IEFDQ09VTlRfT1BUSU9OUywgdXNlVmFsdWU6IG9wdGlvbnMgfSxcclxuICAgIHtcclxuICAgICAgcHJvdmlkZTogJ0FDQ09VTlRfT1BUSU9OUycsXHJcbiAgICAgIHVzZUZhY3Rvcnk6IG9wdGlvbnNGYWN0b3J5LFxyXG4gICAgICBkZXBzOiBbQUNDT1VOVF9PUFRJT05TXSxcclxuICAgIH0sXHJcbiAgXTtcclxufVxyXG4iXX0=