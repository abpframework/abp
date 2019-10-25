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
import { AuthWrapperComponent } from './components/auth-wrapper/auth-wrapper.component';
var AccountModule = /** @class */ (function() {
  function AccountModule() {}
  AccountModule.decorators = [
    {
      type: NgModule,
      args: [
        {
          declarations: [
            AuthWrapperComponent,
            LoginComponent,
            RegisterComponent,
            TenantBoxComponent,
            ChangePasswordComponent,
            ManageProfileComponent,
            PersonalSettingsComponent,
          ],
          imports: [
            CoreModule,
            AccountRoutingModule,
            ThemeSharedModule,
            TableModule,
            NgbDropdownModule,
            NgxValidateCoreModule,
          ],
          exports: [],
        },
      ],
    },
  ];
  return AccountModule;
})();
export { AccountModule };
/**
 *
 * @deprecated since version 0.9
 * @param {?=} options
 * @return {?}
 */
export function AccountProviders(options) {
  if (options === void 0) {
    options = /** @type {?} */ ({});
  }
  return [
    { provide: ACCOUNT_OPTIONS, useValue: options },
    {
      provide: 'ACCOUNT_OPTIONS',
      useFactory: optionsFactory,
      deps: [ACCOUNT_OPTIONS],
    },
  ];
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmFjY291bnQvIiwic291cmNlcyI6WyJsaWIvYWNjb3VudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDMUMsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDekQsT0FBTyxFQUFFLFFBQVEsRUFBWSxNQUFNLGVBQWUsQ0FBQztBQUNuRCxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUMvRCxPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUMzRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLDBCQUEwQixDQUFDO0FBQ2hFLE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxNQUFNLHdEQUF3RCxDQUFDO0FBQ2pHLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxvQ0FBb0MsQ0FBQztBQUNwRSxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSxzREFBc0QsQ0FBQztBQUM5RixPQUFPLEVBQUUseUJBQXlCLEVBQUUsTUFBTSw0REFBNEQsQ0FBQztBQUN2RyxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSwwQ0FBMEMsQ0FBQztBQUM3RSxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSw4Q0FBOEMsQ0FBQztBQUVsRixPQUFPLEVBQUUsZUFBZSxFQUFFLGNBQWMsRUFBRSxNQUFNLHdCQUF3QixDQUFDO0FBQ3pFLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLGtEQUFrRCxDQUFDO0FBRXhGO0lBQUE7SUFhNEIsQ0FBQzs7Z0JBYjVCLFFBQVEsU0FBQztvQkFDUixZQUFZLEVBQUU7d0JBQ1osb0JBQW9CO3dCQUNwQixjQUFjO3dCQUNkLGlCQUFpQjt3QkFDakIsa0JBQWtCO3dCQUNsQix1QkFBdUI7d0JBQ3ZCLHNCQUFzQjt3QkFDdEIseUJBQXlCO3FCQUMxQjtvQkFDRCxPQUFPLEVBQUUsQ0FBQyxVQUFVLEVBQUUsb0JBQW9CLEVBQUUsaUJBQWlCLEVBQUUsV0FBVyxFQUFFLGlCQUFpQixFQUFFLHFCQUFxQixDQUFDO29CQUNySCxPQUFPLEVBQUUsRUFBRTtpQkFDWjs7SUFDMkIsb0JBQUM7Q0FBQSxBQWI3QixJQWE2QjtTQUFoQixhQUFhOzs7Ozs7O0FBTTFCLE1BQU0sVUFBVSxnQkFBZ0IsQ0FBQyxPQUF1QjtJQUF2Qix3QkFBQSxFQUFBLDZCQUFVLEVBQUUsRUFBVztJQUN0RCxPQUFPO1FBQ0wsRUFBRSxPQUFPLEVBQUUsZUFBZSxFQUFFLFFBQVEsRUFBRSxPQUFPLEVBQUU7UUFDL0M7WUFDRSxPQUFPLEVBQUUsaUJBQWlCO1lBQzFCLFVBQVUsRUFBRSxjQUFjO1lBQzFCLElBQUksRUFBRSxDQUFDLGVBQWUsQ0FBQztTQUN4QjtLQUNGLENBQUM7QUFDSixDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29yZU1vZHVsZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBUaGVtZVNoYXJlZE1vZHVsZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IE5nTW9kdWxlLCBQcm92aWRlciB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgTmdiRHJvcGRvd25Nb2R1bGUgfSBmcm9tICdAbmctYm9vdHN0cmFwL25nLWJvb3RzdHJhcCc7XG5pbXBvcnQgeyBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGUgfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xuaW1wb3J0IHsgVGFibGVNb2R1bGUgfSBmcm9tICdwcmltZW5nL3RhYmxlJztcbmltcG9ydCB7IEFjY291bnRSb3V0aW5nTW9kdWxlIH0gZnJvbSAnLi9hY2NvdW50LXJvdXRpbmcubW9kdWxlJztcbmltcG9ydCB7IENoYW5nZVBhc3N3b3JkQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2NoYW5nZS1wYXNzd29yZC9jaGFuZ2UtcGFzc3dvcmQuY29tcG9uZW50JztcbmltcG9ydCB7IExvZ2luQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2xvZ2luL2xvZ2luLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBNYW5hZ2VQcm9maWxlQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL21hbmFnZS1wcm9maWxlL21hbmFnZS1wcm9maWxlLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBQZXJzb25hbFNldHRpbmdzQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3BlcnNvbmFsLXNldHRpbmdzL3BlcnNvbmFsLXNldHRpbmdzLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBSZWdpc3RlckNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9yZWdpc3Rlci9yZWdpc3Rlci5jb21wb25lbnQnO1xuaW1wb3J0IHsgVGVuYW50Qm94Q29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3RlbmFudC1ib3gvdGVuYW50LWJveC5jb21wb25lbnQnO1xuaW1wb3J0IHsgT3B0aW9ucyB9IGZyb20gJy4vbW9kZWxzL29wdGlvbnMnO1xuaW1wb3J0IHsgQUNDT1VOVF9PUFRJT05TLCBvcHRpb25zRmFjdG9yeSB9IGZyb20gJy4vdG9rZW5zL29wdGlvbnMudG9rZW4nO1xuaW1wb3J0IHsgQXV0aFdyYXBwZXJDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvYXV0aC13cmFwcGVyL2F1dGgtd3JhcHBlci5jb21wb25lbnQnO1xuXG5ATmdNb2R1bGUoe1xuICBkZWNsYXJhdGlvbnM6IFtcbiAgICBBdXRoV3JhcHBlckNvbXBvbmVudCxcbiAgICBMb2dpbkNvbXBvbmVudCxcbiAgICBSZWdpc3RlckNvbXBvbmVudCxcbiAgICBUZW5hbnRCb3hDb21wb25lbnQsXG4gICAgQ2hhbmdlUGFzc3dvcmRDb21wb25lbnQsXG4gICAgTWFuYWdlUHJvZmlsZUNvbXBvbmVudCxcbiAgICBQZXJzb25hbFNldHRpbmdzQ29tcG9uZW50LFxuICBdLFxuICBpbXBvcnRzOiBbQ29yZU1vZHVsZSwgQWNjb3VudFJvdXRpbmdNb2R1bGUsIFRoZW1lU2hhcmVkTW9kdWxlLCBUYWJsZU1vZHVsZSwgTmdiRHJvcGRvd25Nb2R1bGUsIE5neFZhbGlkYXRlQ29yZU1vZHVsZV0sXG4gIGV4cG9ydHM6IFtdLFxufSlcbmV4cG9ydCBjbGFzcyBBY2NvdW50TW9kdWxlIHt9XG5cbi8qKlxuICpcbiAqIEBkZXByZWNhdGVkIHNpbmNlIHZlcnNpb24gMC45XG4gKi9cbmV4cG9ydCBmdW5jdGlvbiBBY2NvdW50UHJvdmlkZXJzKG9wdGlvbnMgPSB7fSBhcyBPcHRpb25zKTogUHJvdmlkZXJbXSB7XG4gIHJldHVybiBbXG4gICAgeyBwcm92aWRlOiBBQ0NPVU5UX09QVElPTlMsIHVzZVZhbHVlOiBvcHRpb25zIH0sXG4gICAge1xuICAgICAgcHJvdmlkZTogJ0FDQ09VTlRfT1BUSU9OUycsXG4gICAgICB1c2VGYWN0b3J5OiBvcHRpb25zRmFjdG9yeSxcbiAgICAgIGRlcHM6IFtBQ0NPVU5UX09QVElPTlNdLFxuICAgIH0sXG4gIF07XG59XG4iXX0=
