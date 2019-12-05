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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGhlbWUtYmFzaWMubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi90aGVtZS1iYXNpYy5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUMxQyxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUN6RCxPQUFPLEVBQUUsUUFBUSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3pDLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxpQkFBaUIsRUFBRSxNQUFNLDRCQUE0QixDQUFDO0FBQ2xGLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBQzNELE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDekMsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUM1QyxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSxzREFBc0QsQ0FBQztBQUM5RixPQUFPLEVBQUUsMEJBQTBCLEVBQUUsTUFBTSw4REFBOEQsQ0FBQztBQUMxRyxPQUFPLEVBQUUsb0JBQW9CLEVBQUUsTUFBTSxrREFBa0QsQ0FBQztBQUN4RixPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sdUJBQXVCLENBQUM7QUFDcEQsT0FBTyxFQUFFLHdCQUF3QixFQUFFLE1BQU0sMERBQTBELENBQUM7QUFDcEcsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLDRCQUE0QixDQUFDOztBQUU1RCxNQUFNLEtBQU8sT0FBTyxHQUFHLENBQUMsMEJBQTBCLEVBQUUsc0JBQXNCLEVBQUUsb0JBQW9CLENBQUM7QUFFakc7SUE0QkUsMEJBQW9CLGNBQThCO1FBQTlCLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtJQUFHLENBQUM7O2dCQTVCdkQsUUFBUSxTQUFDO29CQUNSLFlBQVksbUJBQU0sT0FBTyxHQUFFLHdCQUF3QixFQUFDO29CQUNwRCxPQUFPLEVBQUU7d0JBQ1AsVUFBVTt3QkFDVixpQkFBaUI7d0JBQ2pCLGlCQUFpQjt3QkFDakIsaUJBQWlCO3dCQUNqQixXQUFXO3dCQUNYLHFCQUFxQjt3QkFDckIsVUFBVSxDQUFDLFVBQVUsQ0FBQyxDQUFDLFdBQVcsQ0FBQyxDQUFDO3dCQUNwQyxxQkFBcUIsQ0FBQyxPQUFPLENBQUM7NEJBQzVCLGNBQWMsRUFBRSxhQUFhOzRCQUM3QixVQUFVLEVBQUU7Z0NBQ1YsS0FBSyxFQUFFLCtDQUErQztnQ0FDdEQsR0FBRyxFQUFFLGtFQUFrRTtnQ0FDdkUsU0FBUyxFQUFFLDZGQUE2RjtnQ0FDeEcsR0FBRyxFQUFFLGtFQUFrRTtnQ0FDdkUsU0FBUyxFQUFFLDRGQUE0RjtnQ0FDdkcsUUFBUSxFQUFFLGtDQUFrQztnQ0FDNUMsZ0JBQWdCLEVBQUUsa0RBQWtEOzZCQUNyRTs0QkFDRCxhQUFhLEVBQUUsd0JBQXdCO3lCQUN4QyxDQUFDO3FCQUNIO29CQUNELE9BQU8sbUJBQU0sT0FBTyxDQUFDO29CQUNyQixlQUFlLG1CQUFNLE9BQU8sR0FBRSx3QkFBd0IsRUFBQztpQkFDeEQ7Ozs7Z0JBOUJRLGNBQWM7O0lBaUN2Qix1QkFBQztDQUFBLEFBN0JELElBNkJDO1NBRlksZ0JBQWdCOzs7Ozs7SUFDZiwwQ0FBc0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb3JlTW9kdWxlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IFRoZW1lU2hhcmVkTW9kdWxlIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgTmdNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE5nYkNvbGxhcHNlTW9kdWxlLCBOZ2JEcm9wZG93bk1vZHVsZSB9IGZyb20gJ0BuZy1ib290c3RyYXAvbmctYm9vdHN0cmFwJztcbmltcG9ydCB7IE5neFZhbGlkYXRlQ29yZU1vZHVsZSB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XG5pbXBvcnQgeyBOZ3hzTW9kdWxlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgVG9hc3RNb2R1bGUgfSBmcm9tICdwcmltZW5nL3RvYXN0JztcbmltcG9ydCB7IEFjY291bnRMYXlvdXRDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvYWNjb3VudC1sYXlvdXQvYWNjb3VudC1sYXlvdXQuY29tcG9uZW50JztcbmltcG9ydCB7IEFwcGxpY2F0aW9uTGF5b3V0Q29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2FwcGxpY2F0aW9uLWxheW91dC9hcHBsaWNhdGlvbi1sYXlvdXQuY29tcG9uZW50JztcbmltcG9ydCB7IEVtcHR5TGF5b3V0Q29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2VtcHR5LWxheW91dC9lbXB0eS1sYXlvdXQuY29tcG9uZW50JztcbmltcG9ydCB7IExheW91dFN0YXRlIH0gZnJvbSAnLi9zdGF0ZXMvbGF5b3V0LnN0YXRlJztcbmltcG9ydCB7IFZhbGlkYXRpb25FcnJvckNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy92YWxpZGF0aW9uLWVycm9yL3ZhbGlkYXRpb24tZXJyb3IuY29tcG9uZW50JztcbmltcG9ydCB7IEluaXRpYWxTZXJ2aWNlIH0gZnJvbSAnLi9zZXJ2aWNlcy9pbml0aWFsLnNlcnZpY2UnO1xuXG5leHBvcnQgY29uc3QgTEFZT1VUUyA9IFtBcHBsaWNhdGlvbkxheW91dENvbXBvbmVudCwgQWNjb3VudExheW91dENvbXBvbmVudCwgRW1wdHlMYXlvdXRDb21wb25lbnRdO1xuXG5ATmdNb2R1bGUoe1xuICBkZWNsYXJhdGlvbnM6IFsuLi5MQVlPVVRTLCBWYWxpZGF0aW9uRXJyb3JDb21wb25lbnRdLFxuICBpbXBvcnRzOiBbXG4gICAgQ29yZU1vZHVsZSxcbiAgICBUaGVtZVNoYXJlZE1vZHVsZSxcbiAgICBOZ2JDb2xsYXBzZU1vZHVsZSxcbiAgICBOZ2JEcm9wZG93bk1vZHVsZSxcbiAgICBUb2FzdE1vZHVsZSxcbiAgICBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGUsXG4gICAgTmd4c01vZHVsZS5mb3JGZWF0dXJlKFtMYXlvdXRTdGF0ZV0pLFxuICAgIE5neFZhbGlkYXRlQ29yZU1vZHVsZS5mb3JSb290KHtcbiAgICAgIHRhcmdldFNlbGVjdG9yOiAnLmZvcm0tZ3JvdXAnLFxuICAgICAgYmx1ZXByaW50czoge1xuICAgICAgICBlbWFpbDogJ0FicEFjY291bnQ6OlRoaXNGaWVsZElzTm90QVZhbGlkRW1haWxBZGRyZXNzLicsXG4gICAgICAgIG1heDogJ0FicEFjY291bnQ6OlRoaXNGaWVsZE11c3RCZUJldHdlZW57MH1BbmR7MX1be3sgbWluIH19LHt7IG1heCB9fV0nLFxuICAgICAgICBtYXhsZW5ndGg6ICdBYnBBY2NvdW50OjpUaGlzRmllbGRNdXN0QmVBU3RyaW5nT3JBcnJheVR5cGVXaXRoQU1heGltdW1MZW5ndGhvT2Z7MH1be3sgcmVxdWlyZWRMZW5ndGggfX1dJyxcbiAgICAgICAgbWluOiAnQWJwQWNjb3VudDo6VGhpc0ZpZWxkTXVzdEJlQmV0d2VlbnswfUFuZHsxfVt7eyBtaW4gfX0se3sgbWF4IH19XScsXG4gICAgICAgIG1pbmxlbmd0aDogJ0FicEFjY291bnQ6OlRoaXNGaWVsZE11c3RCZUFTdHJpbmdPckFycmF5VHlwZVdpdGhBTWluaW11bUxlbmd0aE9mezB9W3t7IHJlcXVpcmVkTGVuZ3RoIH19XScsXG4gICAgICAgIHJlcXVpcmVkOiAnQWJwQWNjb3VudDo6VGhpc0ZpZWxkSXNSZXF1aXJlZC4nLFxuICAgICAgICBwYXNzd29yZE1pc21hdGNoOiAnQWJwSWRlbnRpdHk6OklkZW50aXR5LlBhc3N3b3JkQ29uZmlybWF0aW9uRmFpbGVkJyxcbiAgICAgIH0sXG4gICAgICBlcnJvclRlbXBsYXRlOiBWYWxpZGF0aW9uRXJyb3JDb21wb25lbnQsXG4gICAgfSksXG4gIF0sXG4gIGV4cG9ydHM6IFsuLi5MQVlPVVRTXSxcbiAgZW50cnlDb21wb25lbnRzOiBbLi4uTEFZT1VUUywgVmFsaWRhdGlvbkVycm9yQ29tcG9uZW50XSxcbn0pXG5leHBvcnQgY2xhc3MgVGhlbWVCYXNpY01vZHVsZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgaW5pdGlhbFNlcnZpY2U6IEluaXRpYWxTZXJ2aWNlKSB7fVxufVxuIl19