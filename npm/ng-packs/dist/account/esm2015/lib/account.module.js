/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CoreModule } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { AccountRoutingModule } from './account-routing.module';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { TenantBoxComponent } from './components/tenant-box/tenant-box.component';
import { ACCOUNT_OPTIONS, optionsFactory } from './tokens/options.token';
import { TableModule } from 'primeng/table';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
export class AccountModule {
    /**
     * @param {?=} options
     * @return {?}
     */
    static forRoot(options = (/** @type {?} */ ({}))) {
        return {
            ngModule: AccountModule,
            providers: [
                { provide: ACCOUNT_OPTIONS, useValue: options },
                {
                    provide: 'ACCOUNT_OPTIONS',
                    useFactory: optionsFactory,
                    deps: [ACCOUNT_OPTIONS],
                },
            ],
        };
    }
}
AccountModule.decorators = [
    { type: NgModule, args: [{
                declarations: [LoginComponent, RegisterComponent, TenantBoxComponent],
                imports: [CoreModule, AccountRoutingModule, ThemeSharedModule, TableModule, NgbDropdownModule, NgxValidateCoreModule],
                exports: [],
            },] }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmFjY291bnQvIiwic291cmNlcyI6WyJsaWIvYWNjb3VudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDMUMsT0FBTyxFQUFFLFFBQVEsRUFBdUIsTUFBTSxlQUFlLENBQUM7QUFDOUQsT0FBTyxFQUFFLG9CQUFvQixFQUFFLE1BQU0sMEJBQTBCLENBQUM7QUFDaEUsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLG9DQUFvQyxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLDBDQUEwQyxDQUFDO0FBQzdFLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFBRSxrQkFBa0IsRUFBRSxNQUFNLDhDQUE4QyxDQUFDO0FBRWxGLE9BQU8sRUFBRSxlQUFlLEVBQUUsY0FBYyxFQUFFLE1BQU0sd0JBQXdCLENBQUM7QUFDekUsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUM1QyxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUMvRCxPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQU8zRCxNQUFNLE9BQU8sYUFBYTs7Ozs7SUFDeEIsTUFBTSxDQUFDLE9BQU8sQ0FBQyxPQUFPLEdBQUcsbUJBQUEsRUFBRSxFQUFXO1FBQ3BDLE9BQU87WUFDTCxRQUFRLEVBQUUsYUFBYTtZQUN2QixTQUFTLEVBQUU7Z0JBQ1QsRUFBRSxPQUFPLEVBQUUsZUFBZSxFQUFFLFFBQVEsRUFBRSxPQUFPLEVBQUU7Z0JBQy9DO29CQUNFLE9BQU8sRUFBRSxpQkFBaUI7b0JBQzFCLFVBQVUsRUFBRSxjQUFjO29CQUMxQixJQUFJLEVBQUUsQ0FBQyxlQUFlLENBQUM7aUJBQ3hCO2FBQ0Y7U0FDRixDQUFDO0lBQ0osQ0FBQzs7O1lBbEJGLFFBQVEsU0FBQztnQkFDUixZQUFZLEVBQUUsQ0FBQyxjQUFjLEVBQUUsaUJBQWlCLEVBQUUsa0JBQWtCLENBQUM7Z0JBQ3JFLE9BQU8sRUFBRSxDQUFDLFVBQVUsRUFBRSxvQkFBb0IsRUFBRSxpQkFBaUIsRUFBRSxXQUFXLEVBQUUsaUJBQWlCLEVBQUUscUJBQXFCLENBQUM7Z0JBQ3JILE9BQU8sRUFBRSxFQUFFO2FBQ1oiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb3JlTW9kdWxlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IE5nTW9kdWxlLCBNb2R1bGVXaXRoUHJvdmlkZXJzIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBBY2NvdW50Um91dGluZ01vZHVsZSB9IGZyb20gJy4vYWNjb3VudC1yb3V0aW5nLm1vZHVsZSc7XG5pbXBvcnQgeyBMb2dpbkNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9sb2dpbi9sb2dpbi5jb21wb25lbnQnO1xuaW1wb3J0IHsgUmVnaXN0ZXJDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvcmVnaXN0ZXIvcmVnaXN0ZXIuY29tcG9uZW50JztcbmltcG9ydCB7IFRoZW1lU2hhcmVkTW9kdWxlIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgVGVuYW50Qm94Q29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3RlbmFudC1ib3gvdGVuYW50LWJveC5jb21wb25lbnQnO1xuaW1wb3J0IHsgT3B0aW9ucyB9IGZyb20gJy4vbW9kZWxzL29wdGlvbnMnO1xuaW1wb3J0IHsgQUNDT1VOVF9PUFRJT05TLCBvcHRpb25zRmFjdG9yeSB9IGZyb20gJy4vdG9rZW5zL29wdGlvbnMudG9rZW4nO1xuaW1wb3J0IHsgVGFibGVNb2R1bGUgfSBmcm9tICdwcmltZW5nL3RhYmxlJztcbmltcG9ydCB7IE5nYkRyb3Bkb3duTW9kdWxlIH0gZnJvbSAnQG5nLWJvb3RzdHJhcC9uZy1ib290c3RyYXAnO1xuaW1wb3J0IHsgTmd4VmFsaWRhdGVDb3JlTW9kdWxlIH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcblxuQE5nTW9kdWxlKHtcbiAgZGVjbGFyYXRpb25zOiBbTG9naW5Db21wb25lbnQsIFJlZ2lzdGVyQ29tcG9uZW50LCBUZW5hbnRCb3hDb21wb25lbnRdLFxuICBpbXBvcnRzOiBbQ29yZU1vZHVsZSwgQWNjb3VudFJvdXRpbmdNb2R1bGUsIFRoZW1lU2hhcmVkTW9kdWxlLCBUYWJsZU1vZHVsZSwgTmdiRHJvcGRvd25Nb2R1bGUsIE5neFZhbGlkYXRlQ29yZU1vZHVsZV0sXG4gIGV4cG9ydHM6IFtdLFxufSlcbmV4cG9ydCBjbGFzcyBBY2NvdW50TW9kdWxlIHtcbiAgc3RhdGljIGZvclJvb3Qob3B0aW9ucyA9IHt9IGFzIE9wdGlvbnMpOiBNb2R1bGVXaXRoUHJvdmlkZXJzIHtcbiAgICByZXR1cm4ge1xuICAgICAgbmdNb2R1bGU6IEFjY291bnRNb2R1bGUsXG4gICAgICBwcm92aWRlcnM6IFtcbiAgICAgICAgeyBwcm92aWRlOiBBQ0NPVU5UX09QVElPTlMsIHVzZVZhbHVlOiBvcHRpb25zIH0sXG4gICAgICAgIHtcbiAgICAgICAgICBwcm92aWRlOiAnQUNDT1VOVF9PUFRJT05TJyxcbiAgICAgICAgICB1c2VGYWN0b3J5OiBvcHRpb25zRmFjdG9yeSxcbiAgICAgICAgICBkZXBzOiBbQUNDT1VOVF9PUFRJT05TXSxcbiAgICAgICAgfSxcbiAgICAgIF0sXG4gICAgfTtcbiAgfVxufVxuIl19