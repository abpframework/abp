/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
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
export const LAYOUTS = [ApplicationLayoutComponent, AccountLayoutComponent, EmptyLayoutComponent];
export class ThemeBasicModule {
  /**
   * @param {?} initialService
   */
  constructor(initialService) {
    this.initialService = initialService;
  }
}
ThemeBasicModule.decorators = [
  {
    type: NgModule,
    args: [
      {
        declarations: [...LAYOUTS, ValidationErrorComponent],
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
              maxlength: 'AbpAccount::ThisFieldMustBeAStringWithAMaximumLengthOf{1}[{{ requiredLength }}]',
              min: 'AbpAccount::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]',
              minlength: 'AbpAccount::ThisFieldMustBeAStringOrArrayTypeWithAMinimumLengthOf[{{ min }},{{ max }}]',
              required: 'AbpAccount::ThisFieldIsRequired.',
              passwordMismatch: 'AbpIdentity::Identity.PasswordConfirmationFailed',
            },
            errorTemplate: ValidationErrorComponent,
          }),
        ],
        exports: [...LAYOUTS],
        entryComponents: [...LAYOUTS, ValidationErrorComponent],
      },
    ],
  },
];
/** @nocollapse */
ThemeBasicModule.ctorParameters = () => [{ type: InitialService }];
if (false) {
  /**
   * @type {?}
   * @private
   */
  ThemeBasicModule.prototype.initialService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGhlbWUtYmFzaWMubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi90aGVtZS1iYXNpYy5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDMUMsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDekQsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN6QyxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUNsRixPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUMzRCxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3pDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDNUMsT0FBTyxFQUFFLHNCQUFzQixFQUFFLE1BQU0sc0RBQXNELENBQUM7QUFDOUYsT0FBTyxFQUFFLDBCQUEwQixFQUFFLE1BQU0sOERBQThELENBQUM7QUFDMUcsT0FBTyxFQUFFLG9CQUFvQixFQUFFLE1BQU0sa0RBQWtELENBQUM7QUFDeEYsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLHVCQUF1QixDQUFDO0FBQ3BELE9BQU8sRUFBRSx3QkFBd0IsRUFBRSxNQUFNLDBEQUEwRCxDQUFDO0FBQ3BHLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQzs7QUFFNUQsTUFBTSxPQUFPLE9BQU8sR0FBRyxDQUFDLDBCQUEwQixFQUFFLHNCQUFzQixFQUFFLG9CQUFvQixDQUFDO0FBNkJqRyxNQUFNLE9BQU8sZ0JBQWdCOzs7O0lBQzNCLFlBQW9CLGNBQThCO1FBQTlCLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtJQUFHLENBQUM7OztZQTVCdkQsUUFBUSxTQUFDO2dCQUNSLFlBQVksRUFBRSxDQUFDLEdBQUcsT0FBTyxFQUFFLHdCQUF3QixDQUFDO2dCQUNwRCxPQUFPLEVBQUU7b0JBQ1AsVUFBVTtvQkFDVixpQkFBaUI7b0JBQ2pCLGlCQUFpQjtvQkFDakIsaUJBQWlCO29CQUNqQixXQUFXO29CQUNYLHFCQUFxQjtvQkFDckIsVUFBVSxDQUFDLFVBQVUsQ0FBQyxDQUFDLFdBQVcsQ0FBQyxDQUFDO29CQUNwQyxxQkFBcUIsQ0FBQyxPQUFPLENBQUM7d0JBQzVCLGNBQWMsRUFBRSxhQUFhO3dCQUM3QixVQUFVLEVBQUU7NEJBQ1YsS0FBSyxFQUFFLCtDQUErQzs0QkFDdEQsR0FBRyxFQUFFLGtFQUFrRTs0QkFDdkUsU0FBUyxFQUFFLGlGQUFpRjs0QkFDNUYsR0FBRyxFQUFFLGtFQUFrRTs0QkFDdkUsU0FBUyxFQUFFLHdGQUF3Rjs0QkFDbkcsUUFBUSxFQUFFLGtDQUFrQzs0QkFDNUMsZ0JBQWdCLEVBQUUsa0RBQWtEO3lCQUNyRTt3QkFDRCxhQUFhLEVBQUUsd0JBQXdCO3FCQUN4QyxDQUFDO2lCQUNIO2dCQUNELE9BQU8sRUFBRSxDQUFDLEdBQUcsT0FBTyxDQUFDO2dCQUNyQixlQUFlLEVBQUUsQ0FBQyxHQUFHLE9BQU8sRUFBRSx3QkFBd0IsQ0FBQzthQUN4RDs7OztZQTlCUSxjQUFjOzs7Ozs7O0lBZ0NULDBDQUFzQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvcmVNb2R1bGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgVGhlbWVTaGFyZWRNb2R1bGUgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBOZ01vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgTmdiQ29sbGFwc2VNb2R1bGUsIE5nYkRyb3Bkb3duTW9kdWxlIH0gZnJvbSAnQG5nLWJvb3RzdHJhcC9uZy1ib290c3RyYXAnO1xuaW1wb3J0IHsgTmd4VmFsaWRhdGVDb3JlTW9kdWxlIH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcbmltcG9ydCB7IE5neHNNb2R1bGUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBUb2FzdE1vZHVsZSB9IGZyb20gJ3ByaW1lbmcvdG9hc3QnO1xuaW1wb3J0IHsgQWNjb3VudExheW91dENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9hY2NvdW50LWxheW91dC9hY2NvdW50LWxheW91dC5jb21wb25lbnQnO1xuaW1wb3J0IHsgQXBwbGljYXRpb25MYXlvdXRDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvYXBwbGljYXRpb24tbGF5b3V0L2FwcGxpY2F0aW9uLWxheW91dC5jb21wb25lbnQnO1xuaW1wb3J0IHsgRW1wdHlMYXlvdXRDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvZW1wdHktbGF5b3V0L2VtcHR5LWxheW91dC5jb21wb25lbnQnO1xuaW1wb3J0IHsgTGF5b3V0U3RhdGUgfSBmcm9tICcuL3N0YXRlcy9sYXlvdXQuc3RhdGUnO1xuaW1wb3J0IHsgVmFsaWRhdGlvbkVycm9yQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3ZhbGlkYXRpb24tZXJyb3IvdmFsaWRhdGlvbi1lcnJvci5jb21wb25lbnQnO1xuaW1wb3J0IHsgSW5pdGlhbFNlcnZpY2UgfSBmcm9tICcuL3NlcnZpY2VzL2luaXRpYWwuc2VydmljZSc7XG5cbmV4cG9ydCBjb25zdCBMQVlPVVRTID0gW0FwcGxpY2F0aW9uTGF5b3V0Q29tcG9uZW50LCBBY2NvdW50TGF5b3V0Q29tcG9uZW50LCBFbXB0eUxheW91dENvbXBvbmVudF07XG5cbkBOZ01vZHVsZSh7XG4gIGRlY2xhcmF0aW9uczogWy4uLkxBWU9VVFMsIFZhbGlkYXRpb25FcnJvckNvbXBvbmVudF0sXG4gIGltcG9ydHM6IFtcbiAgICBDb3JlTW9kdWxlLFxuICAgIFRoZW1lU2hhcmVkTW9kdWxlLFxuICAgIE5nYkNvbGxhcHNlTW9kdWxlLFxuICAgIE5nYkRyb3Bkb3duTW9kdWxlLFxuICAgIFRvYXN0TW9kdWxlLFxuICAgIE5neFZhbGlkYXRlQ29yZU1vZHVsZSxcbiAgICBOZ3hzTW9kdWxlLmZvckZlYXR1cmUoW0xheW91dFN0YXRlXSksXG4gICAgTmd4VmFsaWRhdGVDb3JlTW9kdWxlLmZvclJvb3Qoe1xuICAgICAgdGFyZ2V0U2VsZWN0b3I6ICcuZm9ybS1ncm91cCcsXG4gICAgICBibHVlcHJpbnRzOiB7XG4gICAgICAgIGVtYWlsOiAnQWJwQWNjb3VudDo6VGhpc0ZpZWxkSXNOb3RBVmFsaWRFbWFpbEFkZHJlc3MuJyxcbiAgICAgICAgbWF4OiAnQWJwQWNjb3VudDo6VGhpc0ZpZWxkTXVzdEJlQmV0d2VlbnswfUFuZHsxfVt7eyBtaW4gfX0se3sgbWF4IH19XScsXG4gICAgICAgIG1heGxlbmd0aDogJ0FicEFjY291bnQ6OlRoaXNGaWVsZE11c3RCZUFTdHJpbmdXaXRoQU1heGltdW1MZW5ndGhPZnsxfVt7eyByZXF1aXJlZExlbmd0aCB9fV0nLFxuICAgICAgICBtaW46ICdBYnBBY2NvdW50OjpUaGlzRmllbGRNdXN0QmVCZXR3ZWVuezB9QW5kezF9W3t7IG1pbiB9fSx7eyBtYXggfX1dJyxcbiAgICAgICAgbWlubGVuZ3RoOiAnQWJwQWNjb3VudDo6VGhpc0ZpZWxkTXVzdEJlQVN0cmluZ09yQXJyYXlUeXBlV2l0aEFNaW5pbXVtTGVuZ3RoT2Zbe3sgbWluIH19LHt7IG1heCB9fV0nLFxuICAgICAgICByZXF1aXJlZDogJ0FicEFjY291bnQ6OlRoaXNGaWVsZElzUmVxdWlyZWQuJyxcbiAgICAgICAgcGFzc3dvcmRNaXNtYXRjaDogJ0FicElkZW50aXR5OjpJZGVudGl0eS5QYXNzd29yZENvbmZpcm1hdGlvbkZhaWxlZCcsXG4gICAgICB9LFxuICAgICAgZXJyb3JUZW1wbGF0ZTogVmFsaWRhdGlvbkVycm9yQ29tcG9uZW50LFxuICAgIH0pLFxuICBdLFxuICBleHBvcnRzOiBbLi4uTEFZT1VUU10sXG4gIGVudHJ5Q29tcG9uZW50czogWy4uLkxBWU9VVFMsIFZhbGlkYXRpb25FcnJvckNvbXBvbmVudF0sXG59KVxuZXhwb3J0IGNsYXNzIFRoZW1lQmFzaWNNb2R1bGUge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGluaXRpYWxTZXJ2aWNlOiBJbml0aWFsU2VydmljZSkge31cbn1cbiJdfQ==
