/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CoreModule, LazyLoadService } from '@abp/ng.core';
import { APP_INITIALIZER, Injector, NgModule } from '@angular/core';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { MessageService } from 'primeng/components/common/messageservice';
import { ToastModule } from 'primeng/toast';
import { forkJoin } from 'rxjs';
import { take } from 'rxjs/operators';
import { ConfirmationComponent } from './components/confirmation/confirmation.component';
import { ErrorComponent } from './components/errors/error.component';
import { LoaderBarComponent } from './components/loader-bar/loader-bar.component';
import { ModalComponent } from './components/modal/modal.component';
import { ToastComponent } from './components/toast/toast.component';
import styles from './contants/styles';
import { ErrorHandler } from './handlers/error.handler';
import { ButtonComponent } from './components/button/button.component';
import { ValidationErrorComponent } from './components/errors/validation-error.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { ProfileComponent } from './components/profile/profile.component';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
/**
 * @param {?} injector
 * @return {?}
 */
export function appendScript(injector) {
    /** @type {?} */
    var fn = (/**
     * @return {?}
     */
    function () {
        /** @type {?} */
        var lazyLoadService = injector.get(LazyLoadService);
        return forkJoin(lazyLoadService.load(null, 'style', styles, 'head', 'afterbegin') /* lazyLoadService.load(null, 'script', scripts) */).pipe(take(1));
    });
    return fn;
}
var ThemeSharedModule = /** @class */ (function () {
    function ThemeSharedModule() {
    }
    /**
     * @return {?}
     */
    ThemeSharedModule.forRoot = /**
     * @return {?}
     */
    function () {
        return {
            ngModule: ThemeSharedModule,
            providers: [
                {
                    provide: APP_INITIALIZER,
                    multi: true,
                    deps: [Injector, ErrorHandler],
                    useFactory: appendScript,
                },
                { provide: MessageService, useClass: MessageService },
            ],
        };
    };
    ThemeSharedModule.decorators = [
        { type: NgModule, args: [{
                    imports: [
                        CoreModule,
                        ToastModule,
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
                    declarations: [
                        ButtonComponent,
                        ConfirmationComponent,
                        ToastComponent,
                        ModalComponent,
                        ErrorComponent,
                        LoaderBarComponent,
                        ValidationErrorComponent,
                        ChangePasswordComponent,
                        ProfileComponent,
                        BreadcrumbComponent,
                    ],
                    exports: [
                        ButtonComponent,
                        ConfirmationComponent,
                        ToastComponent,
                        ModalComponent,
                        LoaderBarComponent,
                        ChangePasswordComponent,
                        ProfileComponent,
                        BreadcrumbComponent,
                    ],
                    entryComponents: [ErrorComponent, ValidationErrorComponent],
                },] }
    ];
    return ThemeSharedModule;
}());
export { ThemeSharedModule };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGhlbWUtc2hhcmVkLm1vZHVsZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL3RoZW1lLXNoYXJlZC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsZUFBZSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzNELE9BQU8sRUFBRSxlQUFlLEVBQUUsUUFBUSxFQUF1QixRQUFRLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDekYsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFDM0QsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLDBDQUEwQyxDQUFDO0FBQzFFLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDNUMsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNoQyxPQUFPLEVBQUUsSUFBSSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDdEMsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sa0RBQWtELENBQUM7QUFDekYsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLHFDQUFxQyxDQUFDO0FBQ3JFLE9BQU8sRUFBRSxrQkFBa0IsRUFBRSxNQUFNLDhDQUE4QyxDQUFDO0FBQ2xGLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxvQ0FBb0MsQ0FBQztBQUNwRSxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDcEUsT0FBTyxNQUFNLE1BQU0sbUJBQW1CLENBQUM7QUFDdkMsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLDBCQUEwQixDQUFDO0FBQ3hELE9BQU8sRUFBRSxlQUFlLEVBQUUsTUFBTSxzQ0FBc0MsQ0FBQztBQUN2RSxPQUFPLEVBQUUsd0JBQXdCLEVBQUUsTUFBTSxnREFBZ0QsQ0FBQztBQUMxRixPQUFPLEVBQUUsdUJBQXVCLEVBQUUsTUFBTSx3REFBd0QsQ0FBQztBQUNqRyxPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSx3Q0FBd0MsQ0FBQztBQUMxRSxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSw4Q0FBOEMsQ0FBQzs7Ozs7QUFFbkYsTUFBTSxVQUFVLFlBQVksQ0FBQyxRQUFrQjs7UUFDdkMsRUFBRTs7O0lBQUc7O1lBQ0gsZUFBZSxHQUFvQixRQUFRLENBQUMsR0FBRyxDQUFDLGVBQWUsQ0FBQztRQUV0RSxPQUFPLFFBQVEsQ0FDYixlQUFlLENBQUMsSUFBSSxDQUNsQixJQUFJLEVBQ0osT0FBTyxFQUNQLE1BQU0sRUFDTixNQUFNLEVBQ04sWUFBWSxDQUNiLENBQUMsbURBQW1ELENBQ3RELENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDO0lBQ2xCLENBQUMsQ0FBQTtJQUVELE9BQU8sRUFBRSxDQUFDO0FBQ1osQ0FBQztBQUVEO0lBQUE7SUF5REEsQ0FBQzs7OztJQWRRLHlCQUFPOzs7SUFBZDtRQUNFLE9BQU87WUFDTCxRQUFRLEVBQUUsaUJBQWlCO1lBQzNCLFNBQVMsRUFBRTtnQkFDVDtvQkFDRSxPQUFPLEVBQUUsZUFBZTtvQkFDeEIsS0FBSyxFQUFFLElBQUk7b0JBQ1gsSUFBSSxFQUFFLENBQUMsUUFBUSxFQUFFLFlBQVksQ0FBQztvQkFDOUIsVUFBVSxFQUFFLFlBQVk7aUJBQ3pCO2dCQUNELEVBQUUsT0FBTyxFQUFFLGNBQWMsRUFBRSxRQUFRLEVBQUUsY0FBYyxFQUFFO2FBQ3REO1NBQ0YsQ0FBQztJQUNKLENBQUM7O2dCQXhERixRQUFRLFNBQUM7b0JBQ1IsT0FBTyxFQUFFO3dCQUNQLFVBQVU7d0JBQ1YsV0FBVzt3QkFDWCxxQkFBcUIsQ0FBQyxPQUFPLENBQUM7NEJBQzVCLGNBQWMsRUFBRSxhQUFhOzRCQUM3QixVQUFVLEVBQUU7Z0NBQ1YsS0FBSyxFQUFFLCtDQUErQztnQ0FDdEQsR0FBRyxFQUFFLGtFQUFrRTtnQ0FDdkUsU0FBUyxFQUFFLGlGQUFpRjtnQ0FDNUYsR0FBRyxFQUFFLGtFQUFrRTtnQ0FDdkUsU0FBUyxFQUFFLHdGQUF3RjtnQ0FDbkcsUUFBUSxFQUFFLGtDQUFrQztnQ0FDNUMsZ0JBQWdCLEVBQUUsa0RBQWtEOzZCQUNyRTs0QkFDRCxhQUFhLEVBQUUsd0JBQXdCO3lCQUN4QyxDQUFDO3FCQUNIO29CQUNELFlBQVksRUFBRTt3QkFDWixlQUFlO3dCQUNmLHFCQUFxQjt3QkFDckIsY0FBYzt3QkFDZCxjQUFjO3dCQUNkLGNBQWM7d0JBQ2Qsa0JBQWtCO3dCQUNsQix3QkFBd0I7d0JBQ3hCLHVCQUF1Qjt3QkFDdkIsZ0JBQWdCO3dCQUNoQixtQkFBbUI7cUJBQ3BCO29CQUNELE9BQU8sRUFBRTt3QkFDUCxlQUFlO3dCQUNmLHFCQUFxQjt3QkFDckIsY0FBYzt3QkFDZCxjQUFjO3dCQUNkLGtCQUFrQjt3QkFDbEIsdUJBQXVCO3dCQUN2QixnQkFBZ0I7d0JBQ2hCLG1CQUFtQjtxQkFDcEI7b0JBQ0QsZUFBZSxFQUFFLENBQUMsY0FBYyxFQUFFLHdCQUF3QixDQUFDO2lCQUM1RDs7SUFnQkQsd0JBQUM7Q0FBQSxBQXpERCxJQXlEQztTQWZZLGlCQUFpQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvcmVNb2R1bGUsIExhenlMb2FkU2VydmljZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBBUFBfSU5JVElBTElaRVIsIEluamVjdG9yLCBNb2R1bGVXaXRoUHJvdmlkZXJzLCBOZ01vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgTmd4VmFsaWRhdGVDb3JlTW9kdWxlIH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcbmltcG9ydCB7IE1lc3NhZ2VTZXJ2aWNlIH0gZnJvbSAncHJpbWVuZy9jb21wb25lbnRzL2NvbW1vbi9tZXNzYWdlc2VydmljZSc7XG5pbXBvcnQgeyBUb2FzdE1vZHVsZSB9IGZyb20gJ3ByaW1lbmcvdG9hc3QnO1xuaW1wb3J0IHsgZm9ya0pvaW4gfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IHRha2UgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgeyBDb25maXJtYXRpb25Db21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvY29uZmlybWF0aW9uL2NvbmZpcm1hdGlvbi5jb21wb25lbnQnO1xuaW1wb3J0IHsgRXJyb3JDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvZXJyb3JzL2Vycm9yLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBMb2FkZXJCYXJDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvbG9hZGVyLWJhci9sb2FkZXItYmFyLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBNb2RhbENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9tb2RhbC9tb2RhbC5jb21wb25lbnQnO1xuaW1wb3J0IHsgVG9hc3RDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvdG9hc3QvdG9hc3QuY29tcG9uZW50JztcbmltcG9ydCBzdHlsZXMgZnJvbSAnLi9jb250YW50cy9zdHlsZXMnO1xuaW1wb3J0IHsgRXJyb3JIYW5kbGVyIH0gZnJvbSAnLi9oYW5kbGVycy9lcnJvci5oYW5kbGVyJztcbmltcG9ydCB7IEJ1dHRvbkNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9idXR0b24vYnV0dG9uLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBWYWxpZGF0aW9uRXJyb3JDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvZXJyb3JzL3ZhbGlkYXRpb24tZXJyb3IuY29tcG9uZW50JztcbmltcG9ydCB7IENoYW5nZVBhc3N3b3JkQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2NoYW5nZS1wYXNzd29yZC9jaGFuZ2UtcGFzc3dvcmQuY29tcG9uZW50JztcbmltcG9ydCB7IFByb2ZpbGVDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvcHJvZmlsZS9wcm9maWxlLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBCcmVhZGNydW1iQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2JyZWFkY3J1bWIvYnJlYWRjcnVtYi5jb21wb25lbnQnO1xuXG5leHBvcnQgZnVuY3Rpb24gYXBwZW5kU2NyaXB0KGluamVjdG9yOiBJbmplY3Rvcikge1xuICBjb25zdCBmbiA9IGZ1bmN0aW9uKCkge1xuICAgIGNvbnN0IGxhenlMb2FkU2VydmljZTogTGF6eUxvYWRTZXJ2aWNlID0gaW5qZWN0b3IuZ2V0KExhenlMb2FkU2VydmljZSk7XG5cbiAgICByZXR1cm4gZm9ya0pvaW4oXG4gICAgICBsYXp5TG9hZFNlcnZpY2UubG9hZChcbiAgICAgICAgbnVsbCxcbiAgICAgICAgJ3N0eWxlJyxcbiAgICAgICAgc3R5bGVzLFxuICAgICAgICAnaGVhZCcsXG4gICAgICAgICdhZnRlcmJlZ2luJyxcbiAgICAgICkgLyogbGF6eUxvYWRTZXJ2aWNlLmxvYWQobnVsbCwgJ3NjcmlwdCcsIHNjcmlwdHMpICovLFxuICAgICkucGlwZSh0YWtlKDEpKTtcbiAgfTtcblxuICByZXR1cm4gZm47XG59XG5cbkBOZ01vZHVsZSh7XG4gIGltcG9ydHM6IFtcbiAgICBDb3JlTW9kdWxlLFxuICAgIFRvYXN0TW9kdWxlLFxuICAgIE5neFZhbGlkYXRlQ29yZU1vZHVsZS5mb3JSb290KHtcbiAgICAgIHRhcmdldFNlbGVjdG9yOiAnLmZvcm0tZ3JvdXAnLFxuICAgICAgYmx1ZXByaW50czoge1xuICAgICAgICBlbWFpbDogYEFicEFjY291bnQ6OlRoaXNGaWVsZElzTm90QVZhbGlkRW1haWxBZGRyZXNzLmAsXG4gICAgICAgIG1heDogYEFicEFjY291bnQ6OlRoaXNGaWVsZE11c3RCZUJldHdlZW57MH1BbmR7MX1be3sgbWluIH19LHt7IG1heCB9fV1gLFxuICAgICAgICBtYXhsZW5ndGg6IGBBYnBBY2NvdW50OjpUaGlzRmllbGRNdXN0QmVBU3RyaW5nV2l0aEFNYXhpbXVtTGVuZ3RoT2Z7MX1be3sgcmVxdWlyZWRMZW5ndGggfX1dYCxcbiAgICAgICAgbWluOiBgQWJwQWNjb3VudDo6VGhpc0ZpZWxkTXVzdEJlQmV0d2VlbnswfUFuZHsxfVt7eyBtaW4gfX0se3sgbWF4IH19XWAsXG4gICAgICAgIG1pbmxlbmd0aDogYEFicEFjY291bnQ6OlRoaXNGaWVsZE11c3RCZUFTdHJpbmdPckFycmF5VHlwZVdpdGhBTWluaW11bUxlbmd0aE9mW3t7IG1pbiB9fSx7eyBtYXggfX1dYCxcbiAgICAgICAgcmVxdWlyZWQ6IGBBYnBBY2NvdW50OjpUaGlzRmllbGRJc1JlcXVpcmVkLmAsXG4gICAgICAgIHBhc3N3b3JkTWlzbWF0Y2g6IGBBYnBJZGVudGl0eTo6SWRlbnRpdHkuUGFzc3dvcmRDb25maXJtYXRpb25GYWlsZWRgLFxuICAgICAgfSxcbiAgICAgIGVycm9yVGVtcGxhdGU6IFZhbGlkYXRpb25FcnJvckNvbXBvbmVudCxcbiAgICB9KSxcbiAgXSxcbiAgZGVjbGFyYXRpb25zOiBbXG4gICAgQnV0dG9uQ29tcG9uZW50LFxuICAgIENvbmZpcm1hdGlvbkNvbXBvbmVudCxcbiAgICBUb2FzdENvbXBvbmVudCxcbiAgICBNb2RhbENvbXBvbmVudCxcbiAgICBFcnJvckNvbXBvbmVudCxcbiAgICBMb2FkZXJCYXJDb21wb25lbnQsXG4gICAgVmFsaWRhdGlvbkVycm9yQ29tcG9uZW50LFxuICAgIENoYW5nZVBhc3N3b3JkQ29tcG9uZW50LFxuICAgIFByb2ZpbGVDb21wb25lbnQsXG4gICAgQnJlYWRjcnVtYkNvbXBvbmVudCxcbiAgXSxcbiAgZXhwb3J0czogW1xuICAgIEJ1dHRvbkNvbXBvbmVudCxcbiAgICBDb25maXJtYXRpb25Db21wb25lbnQsXG4gICAgVG9hc3RDb21wb25lbnQsXG4gICAgTW9kYWxDb21wb25lbnQsXG4gICAgTG9hZGVyQmFyQ29tcG9uZW50LFxuICAgIENoYW5nZVBhc3N3b3JkQ29tcG9uZW50LFxuICAgIFByb2ZpbGVDb21wb25lbnQsXG4gICAgQnJlYWRjcnVtYkNvbXBvbmVudCxcbiAgXSxcbiAgZW50cnlDb21wb25lbnRzOiBbRXJyb3JDb21wb25lbnQsIFZhbGlkYXRpb25FcnJvckNvbXBvbmVudF0sXG59KVxuZXhwb3J0IGNsYXNzIFRoZW1lU2hhcmVkTW9kdWxlIHtcbiAgc3RhdGljIGZvclJvb3QoKTogTW9kdWxlV2l0aFByb3ZpZGVycyB7XG4gICAgcmV0dXJuIHtcbiAgICAgIG5nTW9kdWxlOiBUaGVtZVNoYXJlZE1vZHVsZSxcbiAgICAgIHByb3ZpZGVyczogW1xuICAgICAgICB7XG4gICAgICAgICAgcHJvdmlkZTogQVBQX0lOSVRJQUxJWkVSLFxuICAgICAgICAgIG11bHRpOiB0cnVlLFxuICAgICAgICAgIGRlcHM6IFtJbmplY3RvciwgRXJyb3JIYW5kbGVyXSxcbiAgICAgICAgICB1c2VGYWN0b3J5OiBhcHBlbmRTY3JpcHQsXG4gICAgICAgIH0sXG4gICAgICAgIHsgcHJvdmlkZTogTWVzc2FnZVNlcnZpY2UsIHVzZUNsYXNzOiBNZXNzYWdlU2VydmljZSB9LFxuICAgICAgXSxcbiAgICB9O1xuICB9XG59XG4iXX0=