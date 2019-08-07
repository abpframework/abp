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
var AccountModule = /** @class */ (function () {
    function AccountModule() {
    }
    /**
     * @param {?=} options
     * @return {?}
     */
    AccountModule.forRoot = /**
     * @param {?=} options
     * @return {?}
     */
    function (options) {
        if (options === void 0) { options = (/** @type {?} */ ({})); }
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
    };
    AccountModule.decorators = [
        { type: NgModule, args: [{
                    declarations: [LoginComponent, RegisterComponent, TenantBoxComponent],
                    imports: [CoreModule, AccountRoutingModule, ThemeSharedModule, TableModule, NgbDropdownModule, NgxValidateCoreModule],
                    exports: [],
                },] }
    ];
    return AccountModule;
}());
export { AccountModule };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmFjY291bnQvIiwic291cmNlcyI6WyJsaWIvYWNjb3VudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDMUMsT0FBTyxFQUFFLFFBQVEsRUFBdUIsTUFBTSxlQUFlLENBQUM7QUFDOUQsT0FBTyxFQUFFLG9CQUFvQixFQUFFLE1BQU0sMEJBQTBCLENBQUM7QUFDaEUsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLG9DQUFvQyxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLDBDQUEwQyxDQUFDO0FBQzdFLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFBRSxrQkFBa0IsRUFBRSxNQUFNLDhDQUE4QyxDQUFDO0FBRWxGLE9BQU8sRUFBRSxlQUFlLEVBQUUsY0FBYyxFQUFFLE1BQU0sd0JBQXdCLENBQUM7QUFDekUsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUM1QyxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUMvRCxPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUUzRDtJQUFBO0lBbUJBLENBQUM7Ozs7O0lBYlEscUJBQU87Ozs7SUFBZCxVQUFlLE9BQXVCO1FBQXZCLHdCQUFBLEVBQUEsNkJBQVUsRUFBRSxFQUFXO1FBQ3BDLE9BQU87WUFDTCxRQUFRLEVBQUUsYUFBYTtZQUN2QixTQUFTLEVBQUU7Z0JBQ1QsRUFBRSxPQUFPLEVBQUUsZUFBZSxFQUFFLFFBQVEsRUFBRSxPQUFPLEVBQUU7Z0JBQy9DO29CQUNFLE9BQU8sRUFBRSxpQkFBaUI7b0JBQzFCLFVBQVUsRUFBRSxjQUFjO29CQUMxQixJQUFJLEVBQUUsQ0FBQyxlQUFlLENBQUM7aUJBQ3hCO2FBQ0Y7U0FDRixDQUFDO0lBQ0osQ0FBQzs7Z0JBbEJGLFFBQVEsU0FBQztvQkFDUixZQUFZLEVBQUUsQ0FBQyxjQUFjLEVBQUUsaUJBQWlCLEVBQUUsa0JBQWtCLENBQUM7b0JBQ3JFLE9BQU8sRUFBRSxDQUFDLFVBQVUsRUFBRSxvQkFBb0IsRUFBRSxpQkFBaUIsRUFBRSxXQUFXLEVBQUUsaUJBQWlCLEVBQUUscUJBQXFCLENBQUM7b0JBQ3JILE9BQU8sRUFBRSxFQUFFO2lCQUNaOztJQWVELG9CQUFDO0NBQUEsQUFuQkQsSUFtQkM7U0FkWSxhQUFhIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29yZU1vZHVsZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBOZ01vZHVsZSwgTW9kdWxlV2l0aFByb3ZpZGVycyB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgQWNjb3VudFJvdXRpbmdNb2R1bGUgfSBmcm9tICcuL2FjY291bnQtcm91dGluZy5tb2R1bGUnO1xuaW1wb3J0IHsgTG9naW5Db21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvbG9naW4vbG9naW4uY29tcG9uZW50JztcbmltcG9ydCB7IFJlZ2lzdGVyQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3JlZ2lzdGVyL3JlZ2lzdGVyLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBUaGVtZVNoYXJlZE1vZHVsZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IFRlbmFudEJveENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy90ZW5hbnQtYm94L3RlbmFudC1ib3guY29tcG9uZW50JztcbmltcG9ydCB7IE9wdGlvbnMgfSBmcm9tICcuL21vZGVscy9vcHRpb25zJztcbmltcG9ydCB7IEFDQ09VTlRfT1BUSU9OUywgb3B0aW9uc0ZhY3RvcnkgfSBmcm9tICcuL3Rva2Vucy9vcHRpb25zLnRva2VuJztcbmltcG9ydCB7IFRhYmxlTW9kdWxlIH0gZnJvbSAncHJpbWVuZy90YWJsZSc7XG5pbXBvcnQgeyBOZ2JEcm9wZG93bk1vZHVsZSB9IGZyb20gJ0BuZy1ib290c3RyYXAvbmctYm9vdHN0cmFwJztcbmltcG9ydCB7IE5neFZhbGlkYXRlQ29yZU1vZHVsZSB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XG5cbkBOZ01vZHVsZSh7XG4gIGRlY2xhcmF0aW9uczogW0xvZ2luQ29tcG9uZW50LCBSZWdpc3RlckNvbXBvbmVudCwgVGVuYW50Qm94Q29tcG9uZW50XSxcbiAgaW1wb3J0czogW0NvcmVNb2R1bGUsIEFjY291bnRSb3V0aW5nTW9kdWxlLCBUaGVtZVNoYXJlZE1vZHVsZSwgVGFibGVNb2R1bGUsIE5nYkRyb3Bkb3duTW9kdWxlLCBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGVdLFxuICBleHBvcnRzOiBbXSxcbn0pXG5leHBvcnQgY2xhc3MgQWNjb3VudE1vZHVsZSB7XG4gIHN0YXRpYyBmb3JSb290KG9wdGlvbnMgPSB7fSBhcyBPcHRpb25zKTogTW9kdWxlV2l0aFByb3ZpZGVycyB7XG4gICAgcmV0dXJuIHtcbiAgICAgIG5nTW9kdWxlOiBBY2NvdW50TW9kdWxlLFxuICAgICAgcHJvdmlkZXJzOiBbXG4gICAgICAgIHsgcHJvdmlkZTogQUNDT1VOVF9PUFRJT05TLCB1c2VWYWx1ZTogb3B0aW9ucyB9LFxuICAgICAgICB7XG4gICAgICAgICAgcHJvdmlkZTogJ0FDQ09VTlRfT1BUSU9OUycsXG4gICAgICAgICAgdXNlRmFjdG9yeTogb3B0aW9uc0ZhY3RvcnksXG4gICAgICAgICAgZGVwczogW0FDQ09VTlRfT1BUSU9OU10sXG4gICAgICAgIH0sXG4gICAgICBdLFxuICAgIH07XG4gIH1cbn1cbiJdfQ==