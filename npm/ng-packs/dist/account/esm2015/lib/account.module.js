/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { TableModule } from 'primeng/table';
import { AccountRoutingModule } from './account-routing.module';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { TenantBoxComponent } from './components/tenant-box/tenant-box.component';
import { ACCOUNT_OPTIONS, optionsFactory } from './tokens/options.token';
export class AccountModule {
}
AccountModule.decorators = [
    { type: NgModule, args: [{
                declarations: [LoginComponent, RegisterComponent, TenantBoxComponent],
                imports: [CoreModule, AccountRoutingModule, ThemeSharedModule, TableModule, NgbDropdownModule, NgxValidateCoreModule],
                exports: [],
            },] }
];
/**
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmFjY291bnQvIiwic291cmNlcyI6WyJsaWIvYWNjb3VudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDMUMsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDekQsT0FBTyxFQUFFLFFBQVEsRUFBWSxNQUFNLGVBQWUsQ0FBQztBQUNuRCxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUMvRCxPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUMzRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLDBCQUEwQixDQUFDO0FBQ2hFLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxvQ0FBb0MsQ0FBQztBQUNwRSxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSwwQ0FBMEMsQ0FBQztBQUM3RSxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSw4Q0FBOEMsQ0FBQztBQUVsRixPQUFPLEVBQUUsZUFBZSxFQUFFLGNBQWMsRUFBRSxNQUFNLHdCQUF3QixDQUFDO0FBT3pFLE1BQU0sT0FBTyxhQUFhOzs7WUFMekIsUUFBUSxTQUFDO2dCQUNSLFlBQVksRUFBRSxDQUFDLGNBQWMsRUFBRSxpQkFBaUIsRUFBRSxrQkFBa0IsQ0FBQztnQkFDckUsT0FBTyxFQUFFLENBQUMsVUFBVSxFQUFFLG9CQUFvQixFQUFFLGlCQUFpQixFQUFFLFdBQVcsRUFBRSxpQkFBaUIsRUFBRSxxQkFBcUIsQ0FBQztnQkFDckgsT0FBTyxFQUFFLEVBQUU7YUFDWjs7Ozs7O0FBR0QsTUFBTSxVQUFVLGdCQUFnQixDQUFDLE9BQU8sR0FBRyxtQkFBQSxFQUFFLEVBQVc7SUFDdEQsT0FBTztRQUNMLEVBQUUsT0FBTyxFQUFFLGVBQWUsRUFBRSxRQUFRLEVBQUUsT0FBTyxFQUFFO1FBQy9DO1lBQ0UsT0FBTyxFQUFFLGlCQUFpQjtZQUMxQixVQUFVLEVBQUUsY0FBYztZQUMxQixJQUFJLEVBQUUsQ0FBQyxlQUFlLENBQUM7U0FDeEI7S0FDRixDQUFDO0FBQ0osQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvcmVNb2R1bGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgVGhlbWVTaGFyZWRNb2R1bGUgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBOZ01vZHVsZSwgUHJvdmlkZXIgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE5nYkRyb3Bkb3duTW9kdWxlIH0gZnJvbSAnQG5nLWJvb3RzdHJhcC9uZy1ib290c3RyYXAnO1xuaW1wb3J0IHsgTmd4VmFsaWRhdGVDb3JlTW9kdWxlIH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcbmltcG9ydCB7IFRhYmxlTW9kdWxlIH0gZnJvbSAncHJpbWVuZy90YWJsZSc7XG5pbXBvcnQgeyBBY2NvdW50Um91dGluZ01vZHVsZSB9IGZyb20gJy4vYWNjb3VudC1yb3V0aW5nLm1vZHVsZSc7XG5pbXBvcnQgeyBMb2dpbkNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9sb2dpbi9sb2dpbi5jb21wb25lbnQnO1xuaW1wb3J0IHsgUmVnaXN0ZXJDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvcmVnaXN0ZXIvcmVnaXN0ZXIuY29tcG9uZW50JztcbmltcG9ydCB7IFRlbmFudEJveENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy90ZW5hbnQtYm94L3RlbmFudC1ib3guY29tcG9uZW50JztcbmltcG9ydCB7IE9wdGlvbnMgfSBmcm9tICcuL21vZGVscy9vcHRpb25zJztcbmltcG9ydCB7IEFDQ09VTlRfT1BUSU9OUywgb3B0aW9uc0ZhY3RvcnkgfSBmcm9tICcuL3Rva2Vucy9vcHRpb25zLnRva2VuJztcblxuQE5nTW9kdWxlKHtcbiAgZGVjbGFyYXRpb25zOiBbTG9naW5Db21wb25lbnQsIFJlZ2lzdGVyQ29tcG9uZW50LCBUZW5hbnRCb3hDb21wb25lbnRdLFxuICBpbXBvcnRzOiBbQ29yZU1vZHVsZSwgQWNjb3VudFJvdXRpbmdNb2R1bGUsIFRoZW1lU2hhcmVkTW9kdWxlLCBUYWJsZU1vZHVsZSwgTmdiRHJvcGRvd25Nb2R1bGUsIE5neFZhbGlkYXRlQ29yZU1vZHVsZV0sXG4gIGV4cG9ydHM6IFtdLFxufSlcbmV4cG9ydCBjbGFzcyBBY2NvdW50TW9kdWxlIHt9XG5cbmV4cG9ydCBmdW5jdGlvbiBBY2NvdW50UHJvdmlkZXJzKG9wdGlvbnMgPSB7fSBhcyBPcHRpb25zKTogUHJvdmlkZXJbXSB7XG4gIHJldHVybiBbXG4gICAgeyBwcm92aWRlOiBBQ0NPVU5UX09QVElPTlMsIHVzZVZhbHVlOiBvcHRpb25zIH0sXG4gICAge1xuICAgICAgcHJvdmlkZTogJ0FDQ09VTlRfT1BUSU9OUycsXG4gICAgICB1c2VGYWN0b3J5OiBvcHRpb25zRmFjdG9yeSxcbiAgICAgIGRlcHM6IFtBQ0NPVU5UX09QVElPTlNdLFxuICAgIH0sXG4gIF07XG59XG4iXX0=