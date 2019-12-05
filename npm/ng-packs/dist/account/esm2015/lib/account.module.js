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
export class AccountModule {
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmFjY291bnQvIiwic291cmNlcyI6WyJsaWIvYWNjb3VudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzFDLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFBRSxRQUFRLEVBQVksTUFBTSxlQUFlLENBQUM7QUFDbkQsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sNEJBQTRCLENBQUM7QUFDL0QsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFDM0QsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUM1QyxPQUFPLEVBQUUsb0JBQW9CLEVBQUUsTUFBTSwwQkFBMEIsQ0FBQztBQUNoRSxPQUFPLEVBQUUsdUJBQXVCLEVBQUUsTUFBTSx3REFBd0QsQ0FBQztBQUNqRyxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDcEUsT0FBTyxFQUFFLHNCQUFzQixFQUFFLE1BQU0sc0RBQXNELENBQUM7QUFDOUYsT0FBTyxFQUFFLHlCQUF5QixFQUFFLE1BQU0sNERBQTRELENBQUM7QUFDdkcsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sMENBQTBDLENBQUM7QUFDN0UsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0sOENBQThDLENBQUM7QUFFbEYsT0FBTyxFQUFFLGVBQWUsRUFBRSxjQUFjLEVBQUUsTUFBTSx3QkFBd0IsQ0FBQztBQUN6RSxPQUFPLEVBQUUsb0JBQW9CLEVBQUUsTUFBTSxrREFBa0QsQ0FBQztBQWV4RixNQUFNLE9BQU8sYUFBYTs7O1lBYnpCLFFBQVEsU0FBQztnQkFDUixZQUFZLEVBQUU7b0JBQ1osb0JBQW9CO29CQUNwQixjQUFjO29CQUNkLGlCQUFpQjtvQkFDakIsa0JBQWtCO29CQUNsQix1QkFBdUI7b0JBQ3ZCLHNCQUFzQjtvQkFDdEIseUJBQXlCO2lCQUMxQjtnQkFDRCxPQUFPLEVBQUUsQ0FBQyxVQUFVLEVBQUUsb0JBQW9CLEVBQUUsaUJBQWlCLEVBQUUsV0FBVyxFQUFFLGlCQUFpQixFQUFFLHFCQUFxQixDQUFDO2dCQUNySCxPQUFPLEVBQUUsRUFBRTthQUNaOzs7Ozs7OztBQU9ELE1BQU0sVUFBVSxnQkFBZ0IsQ0FBQyxPQUFPLEdBQUcsbUJBQUEsRUFBRSxFQUFXO0lBQ3RELE9BQU87UUFDTCxFQUFFLE9BQU8sRUFBRSxlQUFlLEVBQUUsUUFBUSxFQUFFLE9BQU8sRUFBRTtRQUMvQztZQUNFLE9BQU8sRUFBRSxpQkFBaUI7WUFDMUIsVUFBVSxFQUFFLGNBQWM7WUFDMUIsSUFBSSxFQUFFLENBQUMsZUFBZSxDQUFDO1NBQ3hCO0tBQ0YsQ0FBQztBQUNKLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb3JlTW9kdWxlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IFRoZW1lU2hhcmVkTW9kdWxlIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgTmdNb2R1bGUsIFByb3ZpZGVyIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBOZ2JEcm9wZG93bk1vZHVsZSB9IGZyb20gJ0BuZy1ib290c3RyYXAvbmctYm9vdHN0cmFwJztcbmltcG9ydCB7IE5neFZhbGlkYXRlQ29yZU1vZHVsZSB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XG5pbXBvcnQgeyBUYWJsZU1vZHVsZSB9IGZyb20gJ3ByaW1lbmcvdGFibGUnO1xuaW1wb3J0IHsgQWNjb3VudFJvdXRpbmdNb2R1bGUgfSBmcm9tICcuL2FjY291bnQtcm91dGluZy5tb2R1bGUnO1xuaW1wb3J0IHsgQ2hhbmdlUGFzc3dvcmRDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvY2hhbmdlLXBhc3N3b3JkL2NoYW5nZS1wYXNzd29yZC5jb21wb25lbnQnO1xuaW1wb3J0IHsgTG9naW5Db21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvbG9naW4vbG9naW4uY29tcG9uZW50JztcbmltcG9ydCB7IE1hbmFnZVByb2ZpbGVDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvbWFuYWdlLXByb2ZpbGUvbWFuYWdlLXByb2ZpbGUuY29tcG9uZW50JztcbmltcG9ydCB7IFBlcnNvbmFsU2V0dGluZ3NDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvcGVyc29uYWwtc2V0dGluZ3MvcGVyc29uYWwtc2V0dGluZ3MuY29tcG9uZW50JztcbmltcG9ydCB7IFJlZ2lzdGVyQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3JlZ2lzdGVyL3JlZ2lzdGVyLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBUZW5hbnRCb3hDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvdGVuYW50LWJveC90ZW5hbnQtYm94LmNvbXBvbmVudCc7XG5pbXBvcnQgeyBPcHRpb25zIH0gZnJvbSAnLi9tb2RlbHMvb3B0aW9ucyc7XG5pbXBvcnQgeyBBQ0NPVU5UX09QVElPTlMsIG9wdGlvbnNGYWN0b3J5IH0gZnJvbSAnLi90b2tlbnMvb3B0aW9ucy50b2tlbic7XG5pbXBvcnQgeyBBdXRoV3JhcHBlckNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9hdXRoLXdyYXBwZXIvYXV0aC13cmFwcGVyLmNvbXBvbmVudCc7XG5cbkBOZ01vZHVsZSh7XG4gIGRlY2xhcmF0aW9uczogW1xuICAgIEF1dGhXcmFwcGVyQ29tcG9uZW50LFxuICAgIExvZ2luQ29tcG9uZW50LFxuICAgIFJlZ2lzdGVyQ29tcG9uZW50LFxuICAgIFRlbmFudEJveENvbXBvbmVudCxcbiAgICBDaGFuZ2VQYXNzd29yZENvbXBvbmVudCxcbiAgICBNYW5hZ2VQcm9maWxlQ29tcG9uZW50LFxuICAgIFBlcnNvbmFsU2V0dGluZ3NDb21wb25lbnQsXG4gIF0sXG4gIGltcG9ydHM6IFtDb3JlTW9kdWxlLCBBY2NvdW50Um91dGluZ01vZHVsZSwgVGhlbWVTaGFyZWRNb2R1bGUsIFRhYmxlTW9kdWxlLCBOZ2JEcm9wZG93bk1vZHVsZSwgTmd4VmFsaWRhdGVDb3JlTW9kdWxlXSxcbiAgZXhwb3J0czogW10sXG59KVxuZXhwb3J0IGNsYXNzIEFjY291bnRNb2R1bGUge31cblxuLyoqXG4gKlxuICogQGRlcHJlY2F0ZWQgc2luY2UgdmVyc2lvbiAwLjlcbiAqL1xuZXhwb3J0IGZ1bmN0aW9uIEFjY291bnRQcm92aWRlcnMob3B0aW9ucyA9IHt9IGFzIE9wdGlvbnMpOiBQcm92aWRlcltdIHtcbiAgcmV0dXJuIFtcbiAgICB7IHByb3ZpZGU6IEFDQ09VTlRfT1BUSU9OUywgdXNlVmFsdWU6IG9wdGlvbnMgfSxcbiAgICB7XG4gICAgICBwcm92aWRlOiAnQUNDT1VOVF9PUFRJT05TJyxcbiAgICAgIHVzZUZhY3Rvcnk6IG9wdGlvbnNGYWN0b3J5LFxuICAgICAgZGVwczogW0FDQ09VTlRfT1BUSU9OU10sXG4gICAgfSxcbiAgXTtcbn1cbiJdfQ==