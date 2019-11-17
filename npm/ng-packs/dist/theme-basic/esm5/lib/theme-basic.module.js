/**
 * @fileoverview added by tsickle
 * Generated from: lib/theme-basic.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { NgbCollapseModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { NgxsModule } from '@ngxs/store';
import { ToastModule } from 'primeng/toast';
import { AccountLayoutComponent } from './components/account-layout/account-layout.component';
import { ApplicationLayoutComponent } from './components/application-layout/application-layout.component';
import { EmptyLayoutComponent } from './components/empty-layout/empty-layout.component';
import { LayoutState } from './states/layout.state';
import { ValidationErrorComponent } from './components/validation-error/validation-error.component';
import { InitialService } from './services/initial.service';
/** @type {?} */
export var LAYOUTS = [ApplicationLayoutComponent, AccountLayoutComponent, EmptyLayoutComponent];
var ThemeBasicModule = /** @class */ (function () {
    function ThemeBasicModule(initialService) {
        this.initialService = initialService;
    }
    ThemeBasicModule.decorators = [
        { type: NgModule, args: [{
                    declarations: tslib_1.__spread(LAYOUTS, [ValidationErrorComponent]),
                    imports: [
                        CoreModule,
                        ThemeSharedModule,
                        NgbCollapseModule,
                        NgbDropdownModule,
                        ToastModule,
                        NgxValidateCoreModule,
                        NgxsModule.forFeature([LayoutState]),
                        NgxValidateCoreModule.forRoot({
                            targetSelector: '.form-group',
                            blueprints: {
                                email: 'AbpAccount::ThisFieldIsNotAValidEmailAddress.',
                                max: 'AbpAccount::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]',
                                maxlength: 'AbpAccount::ThisFieldMustBeAStringOrArrayTypeWithAMaximumLengthoOf{0}[{{ requiredLength }}]',
                                min: 'AbpAccount::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]',
                                minlength: 'AbpAccount::ThisFieldMustBeAStringOrArrayTypeWithAMinimumLengthOf{0}[{{ requiredLength }}]',
                                required: 'AbpAccount::ThisFieldIsRequired.',
                                passwordMismatch: 'AbpIdentity::Identity.PasswordConfirmationFailed',
                            },
                            errorTemplate: ValidationErrorComponent,
                        }),
                    ],
                    exports: tslib_1.__spread(LAYOUTS),
                    entryComponents: tslib_1.__spread(LAYOUTS, [ValidationErrorComponent]),
                },] }
    ];
    /** @nocollapse */
    ThemeBasicModule.ctorParameters = function () { return [
        { type: InitialService }
    ]; };
    return ThemeBasicModule;
}());
export { ThemeBasicModule };
if (false) {
    /**
     * @type {?}
     * @private
     */
    ThemeBasicModule.prototype.initialService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGhlbWUtYmFzaWMubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi90aGVtZS1iYXNpYy5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUMxQyxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUN6RCxPQUFPLEVBQUUsUUFBUSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3pDLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxpQkFBaUIsRUFBRSxNQUFNLDRCQUE0QixDQUFDO0FBQ2xGLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBQzNELE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDekMsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUM1QyxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSxzREFBc0QsQ0FBQztBQUM5RixPQUFPLEVBQUUsMEJBQTBCLEVBQUUsTUFBTSw4REFBOEQsQ0FBQztBQUMxRyxPQUFPLEVBQUUsb0JBQW9CLEVBQUUsTUFBTSxrREFBa0QsQ0FBQztBQUN4RixPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sdUJBQXVCLENBQUM7QUFDcEQsT0FBTyxFQUFFLHdCQUF3QixFQUFFLE1BQU0sMERBQTBELENBQUM7QUFDcEcsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLDRCQUE0QixDQUFDOztBQUU1RCxNQUFNLEtBQU8sT0FBTyxHQUFHLENBQUMsMEJBQTBCLEVBQUUsc0JBQXNCLEVBQUUsb0JBQW9CLENBQUM7QUFFakc7SUE0QkUsMEJBQW9CLGNBQThCO1FBQTlCLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtJQUFHLENBQUM7O2dCQTVCdkQsUUFBUSxTQUFDO29CQUNSLFlBQVksbUJBQU0sT0FBTyxHQUFFLHdCQUF3QixFQUFDO29CQUNwRCxPQUFPLEVBQUU7d0JBQ1AsVUFBVTt3QkFDVixpQkFBaUI7d0JBQ2pCLGlCQUFpQjt3QkFDakIsaUJBQWlCO3dCQUNqQixXQUFXO3dCQUNYLHFCQUFxQjt3QkFDckIsVUFBVSxDQUFDLFVBQVUsQ0FBQyxDQUFDLFdBQVcsQ0FBQyxDQUFDO3dCQUNwQyxxQkFBcUIsQ0FBQyxPQUFPLENBQUM7NEJBQzVCLGNBQWMsRUFBRSxhQUFhOzRCQUM3QixVQUFVLEVBQUU7Z0NBQ1YsS0FBSyxFQUFFLCtDQUErQztnQ0FDdEQsR0FBRyxFQUFFLGtFQUFrRTtnQ0FDdkUsU0FBUyxFQUFFLDZGQUE2RjtnQ0FDeEcsR0FBRyxFQUFFLGtFQUFrRTtnQ0FDdkUsU0FBUyxFQUFFLDRGQUE0RjtnQ0FDdkcsUUFBUSxFQUFFLGtDQUFrQztnQ0FDNUMsZ0JBQWdCLEVBQUUsa0RBQWtEOzZCQUNyRTs0QkFDRCxhQUFhLEVBQUUsd0JBQXdCO3lCQUN4QyxDQUFDO3FCQUNIO29CQUNELE9BQU8sbUJBQU0sT0FBTyxDQUFDO29CQUNyQixlQUFlLG1CQUFNLE9BQU8sR0FBRSx3QkFBd0IsRUFBQztpQkFDeEQ7Ozs7Z0JBOUJRLGNBQWM7O0lBaUN2Qix1QkFBQztDQUFBLEFBN0JELElBNkJDO1NBRlksZ0JBQWdCOzs7Ozs7SUFDZiwwQ0FBc0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb3JlTW9kdWxlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgVGhlbWVTaGFyZWRNb2R1bGUgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XHJcbmltcG9ydCB7IE5nTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IE5nYkNvbGxhcHNlTW9kdWxlLCBOZ2JEcm9wZG93bk1vZHVsZSB9IGZyb20gJ0BuZy1ib290c3RyYXAvbmctYm9vdHN0cmFwJztcclxuaW1wb3J0IHsgTmd4VmFsaWRhdGVDb3JlTW9kdWxlIH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcclxuaW1wb3J0IHsgTmd4c01vZHVsZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgVG9hc3RNb2R1bGUgfSBmcm9tICdwcmltZW5nL3RvYXN0JztcclxuaW1wb3J0IHsgQWNjb3VudExheW91dENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9hY2NvdW50LWxheW91dC9hY2NvdW50LWxheW91dC5jb21wb25lbnQnO1xyXG5pbXBvcnQgeyBBcHBsaWNhdGlvbkxheW91dENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9hcHBsaWNhdGlvbi1sYXlvdXQvYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IEVtcHR5TGF5b3V0Q29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2VtcHR5LWxheW91dC9lbXB0eS1sYXlvdXQuY29tcG9uZW50JztcclxuaW1wb3J0IHsgTGF5b3V0U3RhdGUgfSBmcm9tICcuL3N0YXRlcy9sYXlvdXQuc3RhdGUnO1xyXG5pbXBvcnQgeyBWYWxpZGF0aW9uRXJyb3JDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvdmFsaWRhdGlvbi1lcnJvci92YWxpZGF0aW9uLWVycm9yLmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IEluaXRpYWxTZXJ2aWNlIH0gZnJvbSAnLi9zZXJ2aWNlcy9pbml0aWFsLnNlcnZpY2UnO1xyXG5cclxuZXhwb3J0IGNvbnN0IExBWU9VVFMgPSBbQXBwbGljYXRpb25MYXlvdXRDb21wb25lbnQsIEFjY291bnRMYXlvdXRDb21wb25lbnQsIEVtcHR5TGF5b3V0Q29tcG9uZW50XTtcclxuXHJcbkBOZ01vZHVsZSh7XHJcbiAgZGVjbGFyYXRpb25zOiBbLi4uTEFZT1VUUywgVmFsaWRhdGlvbkVycm9yQ29tcG9uZW50XSxcclxuICBpbXBvcnRzOiBbXHJcbiAgICBDb3JlTW9kdWxlLFxyXG4gICAgVGhlbWVTaGFyZWRNb2R1bGUsXHJcbiAgICBOZ2JDb2xsYXBzZU1vZHVsZSxcclxuICAgIE5nYkRyb3Bkb3duTW9kdWxlLFxyXG4gICAgVG9hc3RNb2R1bGUsXHJcbiAgICBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGUsXHJcbiAgICBOZ3hzTW9kdWxlLmZvckZlYXR1cmUoW0xheW91dFN0YXRlXSksXHJcbiAgICBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGUuZm9yUm9vdCh7XHJcbiAgICAgIHRhcmdldFNlbGVjdG9yOiAnLmZvcm0tZ3JvdXAnLFxyXG4gICAgICBibHVlcHJpbnRzOiB7XHJcbiAgICAgICAgZW1haWw6ICdBYnBBY2NvdW50OjpUaGlzRmllbGRJc05vdEFWYWxpZEVtYWlsQWRkcmVzcy4nLFxyXG4gICAgICAgIG1heDogJ0FicEFjY291bnQ6OlRoaXNGaWVsZE11c3RCZUJldHdlZW57MH1BbmR7MX1be3sgbWluIH19LHt7IG1heCB9fV0nLFxyXG4gICAgICAgIG1heGxlbmd0aDogJ0FicEFjY291bnQ6OlRoaXNGaWVsZE11c3RCZUFTdHJpbmdPckFycmF5VHlwZVdpdGhBTWF4aW11bUxlbmd0aG9PZnswfVt7eyByZXF1aXJlZExlbmd0aCB9fV0nLFxyXG4gICAgICAgIG1pbjogJ0FicEFjY291bnQ6OlRoaXNGaWVsZE11c3RCZUJldHdlZW57MH1BbmR7MX1be3sgbWluIH19LHt7IG1heCB9fV0nLFxyXG4gICAgICAgIG1pbmxlbmd0aDogJ0FicEFjY291bnQ6OlRoaXNGaWVsZE11c3RCZUFTdHJpbmdPckFycmF5VHlwZVdpdGhBTWluaW11bUxlbmd0aE9mezB9W3t7IHJlcXVpcmVkTGVuZ3RoIH19XScsXHJcbiAgICAgICAgcmVxdWlyZWQ6ICdBYnBBY2NvdW50OjpUaGlzRmllbGRJc1JlcXVpcmVkLicsXHJcbiAgICAgICAgcGFzc3dvcmRNaXNtYXRjaDogJ0FicElkZW50aXR5OjpJZGVudGl0eS5QYXNzd29yZENvbmZpcm1hdGlvbkZhaWxlZCcsXHJcbiAgICAgIH0sXHJcbiAgICAgIGVycm9yVGVtcGxhdGU6IFZhbGlkYXRpb25FcnJvckNvbXBvbmVudCxcclxuICAgIH0pLFxyXG4gIF0sXHJcbiAgZXhwb3J0czogWy4uLkxBWU9VVFNdLFxyXG4gIGVudHJ5Q29tcG9uZW50czogWy4uLkxBWU9VVFMsIFZhbGlkYXRpb25FcnJvckNvbXBvbmVudF0sXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBUaGVtZUJhc2ljTW9kdWxlIHtcclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGluaXRpYWxTZXJ2aWNlOiBJbml0aWFsU2VydmljZSkge31cclxufVxyXG4iXX0=