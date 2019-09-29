/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
import { LayoutComponent } from './components/layout/layout.component';
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
                    declarations: tslib_1.__spread(LAYOUTS, [LayoutComponent, ValidationErrorComponent]),
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
                                email: "AbpAccount::ThisFieldIsNotAValidEmailAddress.",
                                max: "AbpAccount::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]",
                                maxlength: "AbpAccount::ThisFieldMustBeAStringWithAMaximumLengthOf{1}[{{ requiredLength }}]",
                                min: "AbpAccount::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]",
                                minlength: "AbpAccount::ThisFieldMustBeAStringOrArrayTypeWithAMinimumLengthOf[{{ min }},{{ max }}]",
                                required: "AbpAccount::ThisFieldIsRequired.",
                                passwordMismatch: "AbpIdentity::Identity.PasswordConfirmationFailed",
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGhlbWUtYmFzaWMubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi90aGVtZS1iYXNpYy5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzFDLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDekMsT0FBTyxFQUFFLGlCQUFpQixFQUFFLGlCQUFpQixFQUFFLE1BQU0sNEJBQTRCLENBQUM7QUFDbEYsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFDM0QsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUN6QyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxzQkFBc0IsRUFBRSxNQUFNLHNEQUFzRCxDQUFDO0FBQzlGLE9BQU8sRUFBRSwwQkFBMEIsRUFBRSxNQUFNLDhEQUE4RCxDQUFDO0FBQzFHLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLGtEQUFrRCxDQUFDO0FBQ3hGLE9BQU8sRUFBRSxlQUFlLEVBQUUsTUFBTSxzQ0FBc0MsQ0FBQztBQUN2RSxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sdUJBQXVCLENBQUM7QUFDcEQsT0FBTyxFQUFFLHdCQUF3QixFQUFFLE1BQU0sMERBQTBELENBQUM7QUFDcEcsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLDRCQUE0QixDQUFDOztBQUU1RCxNQUFNLEtBQU8sT0FBTyxHQUFHLENBQUMsMEJBQTBCLEVBQUUsc0JBQXNCLEVBQUUsb0JBQW9CLENBQUM7QUFFakc7SUE0QkUsMEJBQW9CLGNBQThCO1FBQTlCLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtJQUFHLENBQUM7O2dCQTVCdkQsUUFBUSxTQUFDO29CQUNSLFlBQVksbUJBQU0sT0FBTyxHQUFFLGVBQWUsRUFBRSx3QkFBd0IsRUFBQztvQkFDckUsT0FBTyxFQUFFO3dCQUNQLFVBQVU7d0JBQ1YsaUJBQWlCO3dCQUNqQixpQkFBaUI7d0JBQ2pCLGlCQUFpQjt3QkFDakIsV0FBVzt3QkFDWCxxQkFBcUI7d0JBQ3JCLFVBQVUsQ0FBQyxVQUFVLENBQUMsQ0FBQyxXQUFXLENBQUMsQ0FBQzt3QkFDcEMscUJBQXFCLENBQUMsT0FBTyxDQUFDOzRCQUM1QixjQUFjLEVBQUUsYUFBYTs0QkFDN0IsVUFBVSxFQUFFO2dDQUNWLEtBQUssRUFBRSwrQ0FBK0M7Z0NBQ3RELEdBQUcsRUFBRSxrRUFBa0U7Z0NBQ3ZFLFNBQVMsRUFBRSxpRkFBaUY7Z0NBQzVGLEdBQUcsRUFBRSxrRUFBa0U7Z0NBQ3ZFLFNBQVMsRUFBRSx3RkFBd0Y7Z0NBQ25HLFFBQVEsRUFBRSxrQ0FBa0M7Z0NBQzVDLGdCQUFnQixFQUFFLGtEQUFrRDs2QkFDckU7NEJBQ0QsYUFBYSxFQUFFLHdCQUF3Qjt5QkFDeEMsQ0FBQztxQkFDSDtvQkFDRCxPQUFPLG1CQUFNLE9BQU8sQ0FBQztvQkFDckIsZUFBZSxtQkFBTSxPQUFPLEdBQUUsd0JBQXdCLEVBQUM7aUJBQ3hEOzs7O2dCQTlCUSxjQUFjOztJQWlDdkIsdUJBQUM7Q0FBQSxBQTdCRCxJQTZCQztTQUZZLGdCQUFnQjs7Ozs7O0lBQ2YsMENBQXNDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29yZU1vZHVsZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBUaGVtZVNoYXJlZE1vZHVsZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IE5nTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBOZ2JDb2xsYXBzZU1vZHVsZSwgTmdiRHJvcGRvd25Nb2R1bGUgfSBmcm9tICdAbmctYm9vdHN0cmFwL25nLWJvb3RzdHJhcCc7XG5pbXBvcnQgeyBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGUgfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xuaW1wb3J0IHsgTmd4c01vZHVsZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IFRvYXN0TW9kdWxlIH0gZnJvbSAncHJpbWVuZy90b2FzdCc7XG5pbXBvcnQgeyBBY2NvdW50TGF5b3V0Q29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2FjY291bnQtbGF5b3V0L2FjY291bnQtbGF5b3V0LmNvbXBvbmVudCc7XG5pbXBvcnQgeyBBcHBsaWNhdGlvbkxheW91dENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9hcHBsaWNhdGlvbi1sYXlvdXQvYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudCc7XG5pbXBvcnQgeyBFbXB0eUxheW91dENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9lbXB0eS1sYXlvdXQvZW1wdHktbGF5b3V0LmNvbXBvbmVudCc7XG5pbXBvcnQgeyBMYXlvdXRDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvbGF5b3V0L2xheW91dC5jb21wb25lbnQnO1xuaW1wb3J0IHsgTGF5b3V0U3RhdGUgfSBmcm9tICcuL3N0YXRlcy9sYXlvdXQuc3RhdGUnO1xuaW1wb3J0IHsgVmFsaWRhdGlvbkVycm9yQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3ZhbGlkYXRpb24tZXJyb3IvdmFsaWRhdGlvbi1lcnJvci5jb21wb25lbnQnO1xuaW1wb3J0IHsgSW5pdGlhbFNlcnZpY2UgfSBmcm9tICcuL3NlcnZpY2VzL2luaXRpYWwuc2VydmljZSc7XG5cbmV4cG9ydCBjb25zdCBMQVlPVVRTID0gW0FwcGxpY2F0aW9uTGF5b3V0Q29tcG9uZW50LCBBY2NvdW50TGF5b3V0Q29tcG9uZW50LCBFbXB0eUxheW91dENvbXBvbmVudF07XG5cbkBOZ01vZHVsZSh7XG4gIGRlY2xhcmF0aW9uczogWy4uLkxBWU9VVFMsIExheW91dENvbXBvbmVudCwgVmFsaWRhdGlvbkVycm9yQ29tcG9uZW50XSxcbiAgaW1wb3J0czogW1xuICAgIENvcmVNb2R1bGUsXG4gICAgVGhlbWVTaGFyZWRNb2R1bGUsXG4gICAgTmdiQ29sbGFwc2VNb2R1bGUsXG4gICAgTmdiRHJvcGRvd25Nb2R1bGUsXG4gICAgVG9hc3RNb2R1bGUsXG4gICAgTmd4VmFsaWRhdGVDb3JlTW9kdWxlLFxuICAgIE5neHNNb2R1bGUuZm9yRmVhdHVyZShbTGF5b3V0U3RhdGVdKSxcbiAgICBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGUuZm9yUm9vdCh7XG4gICAgICB0YXJnZXRTZWxlY3RvcjogJy5mb3JtLWdyb3VwJyxcbiAgICAgIGJsdWVwcmludHM6IHtcbiAgICAgICAgZW1haWw6IGBBYnBBY2NvdW50OjpUaGlzRmllbGRJc05vdEFWYWxpZEVtYWlsQWRkcmVzcy5gLFxuICAgICAgICBtYXg6IGBBYnBBY2NvdW50OjpUaGlzRmllbGRNdXN0QmVCZXR3ZWVuezB9QW5kezF9W3t7IG1pbiB9fSx7eyBtYXggfX1dYCxcbiAgICAgICAgbWF4bGVuZ3RoOiBgQWJwQWNjb3VudDo6VGhpc0ZpZWxkTXVzdEJlQVN0cmluZ1dpdGhBTWF4aW11bUxlbmd0aE9mezF9W3t7IHJlcXVpcmVkTGVuZ3RoIH19XWAsXG4gICAgICAgIG1pbjogYEFicEFjY291bnQ6OlRoaXNGaWVsZE11c3RCZUJldHdlZW57MH1BbmR7MX1be3sgbWluIH19LHt7IG1heCB9fV1gLFxuICAgICAgICBtaW5sZW5ndGg6IGBBYnBBY2NvdW50OjpUaGlzRmllbGRNdXN0QmVBU3RyaW5nT3JBcnJheVR5cGVXaXRoQU1pbmltdW1MZW5ndGhPZlt7eyBtaW4gfX0se3sgbWF4IH19XWAsXG4gICAgICAgIHJlcXVpcmVkOiBgQWJwQWNjb3VudDo6VGhpc0ZpZWxkSXNSZXF1aXJlZC5gLFxuICAgICAgICBwYXNzd29yZE1pc21hdGNoOiBgQWJwSWRlbnRpdHk6OklkZW50aXR5LlBhc3N3b3JkQ29uZmlybWF0aW9uRmFpbGVkYCxcbiAgICAgIH0sXG4gICAgICBlcnJvclRlbXBsYXRlOiBWYWxpZGF0aW9uRXJyb3JDb21wb25lbnQsXG4gICAgfSksXG4gIF0sXG4gIGV4cG9ydHM6IFsuLi5MQVlPVVRTXSxcbiAgZW50cnlDb21wb25lbnRzOiBbLi4uTEFZT1VUUywgVmFsaWRhdGlvbkVycm9yQ29tcG9uZW50XSxcbn0pXG5leHBvcnQgY2xhc3MgVGhlbWVCYXNpY01vZHVsZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgaW5pdGlhbFNlcnZpY2U6IEluaXRpYWxTZXJ2aWNlKSB7fVxufVxuIl19