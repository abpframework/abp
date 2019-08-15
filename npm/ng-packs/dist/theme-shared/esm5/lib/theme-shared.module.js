/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CoreModule, LazyLoadService } from '@abp/ng.core';
import { APP_INITIALIZER, Injector, NgModule } from '@angular/core';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
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
                        NgbModalModule,
                        NgxValidateCoreModule.forRoot({
                            targetSelector: '.form-group',
                            blueprints: {
                                email: "AbpAccount::ThisFieldIsNotAValidEmailAddress.",
                                max: "AbpAccount::ThisFieldMustBeAStringWithAMaximumLengthOf{1}[{{ max }}]",
                                maxlength: "AbpAccount::ThisFieldMustBeAStringWithAMaximumLengthOf{1}[{{ requiredLength }}]",
                                min: "AbpAccount::ThisFieldMustBeAStringWithAMinimumLengthOf{1}AndAMaximumLengthOf{0}[{{ min }},{{ max }}]",
                                minlength: "AbpAccount::ThisFieldMustBeAStringWithAMinimumLengthOf{1}AndAMaximumLengthOf{0}[{{ min }},{{ max }}]",
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
                    ],
                    exports: [NgbModalModule, ButtonComponent, ConfirmationComponent, ToastComponent, ModalComponent, LoaderBarComponent],
                    entryComponents: [ErrorComponent, ValidationErrorComponent],
                },] }
    ];
    return ThemeSharedModule;
}());
export { ThemeSharedModule };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGhlbWUtc2hhcmVkLm1vZHVsZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL3RoZW1lLXNoYXJlZC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsZUFBZSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzNELE9BQU8sRUFBRSxlQUFlLEVBQUUsUUFBUSxFQUF1QixRQUFRLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDekYsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLDRCQUE0QixDQUFDO0FBQzVELE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBQzNELE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSwwQ0FBMEMsQ0FBQztBQUMxRSxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDaEMsT0FBTyxFQUFFLElBQUksRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3RDLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLGtEQUFrRCxDQUFDO0FBQ3pGLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxxQ0FBcUMsQ0FBQztBQUNyRSxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSw4Q0FBOEMsQ0FBQztBQUNsRixPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDcEUsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLG9DQUFvQyxDQUFDO0FBQ3BFLE9BQU8sTUFBTSxNQUFNLG1CQUFtQixDQUFDO0FBQ3ZDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSwwQkFBMEIsQ0FBQztBQUN4RCxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sc0NBQXNDLENBQUM7QUFDdkUsT0FBTyxFQUFFLHdCQUF3QixFQUFFLE1BQU0sZ0RBQWdELENBQUM7Ozs7O0FBRTFGLE1BQU0sVUFBVSxZQUFZLENBQUMsUUFBa0I7O1FBQ3ZDLEVBQUU7OztJQUFHOztZQUNILGVBQWUsR0FBb0IsUUFBUSxDQUFDLEdBQUcsQ0FBQyxlQUFlLENBQUM7UUFFdEUsT0FBTyxRQUFRLENBQ2IsZUFBZSxDQUFDLElBQUksQ0FDbEIsSUFBSSxFQUNKLE9BQU8sRUFDUCxNQUFNLEVBQ04sTUFBTSxFQUNOLFlBQVksQ0FDYixDQUFDLG1EQUFtRCxDQUN0RCxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQztJQUNsQixDQUFDLENBQUE7SUFFRCxPQUFPLEVBQUUsQ0FBQztBQUNaLENBQUM7QUFFRDtJQUFBO0lBOENBLENBQUM7Ozs7SUFkUSx5QkFBTzs7O0lBQWQ7UUFDRSxPQUFPO1lBQ0wsUUFBUSxFQUFFLGlCQUFpQjtZQUMzQixTQUFTLEVBQUU7Z0JBQ1Q7b0JBQ0UsT0FBTyxFQUFFLGVBQWU7b0JBQ3hCLEtBQUssRUFBRSxJQUFJO29CQUNYLElBQUksRUFBRSxDQUFDLFFBQVEsRUFBRSxZQUFZLENBQUM7b0JBQzlCLFVBQVUsRUFBRSxZQUFZO2lCQUN6QjtnQkFDRCxFQUFFLE9BQU8sRUFBRSxjQUFjLEVBQUUsUUFBUSxFQUFFLGNBQWMsRUFBRTthQUN0RDtTQUNGLENBQUM7SUFDSixDQUFDOztnQkE3Q0YsUUFBUSxTQUFDO29CQUNSLE9BQU8sRUFBRTt3QkFDUCxVQUFVO3dCQUNWLFdBQVc7d0JBQ1gsY0FBYzt3QkFDZCxxQkFBcUIsQ0FBQyxPQUFPLENBQUM7NEJBQzVCLGNBQWMsRUFBRSxhQUFhOzRCQUM3QixVQUFVLEVBQUU7Z0NBQ1YsS0FBSyxFQUFFLCtDQUErQztnQ0FDdEQsR0FBRyxFQUFFLHNFQUFzRTtnQ0FDM0UsU0FBUyxFQUFFLGlGQUFpRjtnQ0FDNUYsR0FBRyxFQUFFLHNHQUFzRztnQ0FDM0csU0FBUyxFQUFFLHNHQUFzRztnQ0FDakgsUUFBUSxFQUFFLGtDQUFrQztnQ0FDNUMsZ0JBQWdCLEVBQUUsa0RBQWtEOzZCQUNyRTs0QkFDRCxhQUFhLEVBQUUsd0JBQXdCO3lCQUN4QyxDQUFDO3FCQUNIO29CQUNELFlBQVksRUFBRTt3QkFDWixlQUFlO3dCQUNmLHFCQUFxQjt3QkFDckIsY0FBYzt3QkFDZCxjQUFjO3dCQUNkLGNBQWM7d0JBQ2Qsa0JBQWtCO3dCQUNsQix3QkFBd0I7cUJBQ3pCO29CQUNELE9BQU8sRUFBRSxDQUFDLGNBQWMsRUFBRSxlQUFlLEVBQUUscUJBQXFCLEVBQUUsY0FBYyxFQUFFLGNBQWMsRUFBRSxrQkFBa0IsQ0FBQztvQkFDckgsZUFBZSxFQUFFLENBQUMsY0FBYyxFQUFFLHdCQUF3QixDQUFDO2lCQUM1RDs7SUFnQkQsd0JBQUM7Q0FBQSxBQTlDRCxJQThDQztTQWZZLGlCQUFpQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvcmVNb2R1bGUsIExhenlMb2FkU2VydmljZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBBUFBfSU5JVElBTElaRVIsIEluamVjdG9yLCBNb2R1bGVXaXRoUHJvdmlkZXJzLCBOZ01vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgTmdiTW9kYWxNb2R1bGUgfSBmcm9tICdAbmctYm9vdHN0cmFwL25nLWJvb3RzdHJhcCc7XG5pbXBvcnQgeyBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGUgfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xuaW1wb3J0IHsgTWVzc2FnZVNlcnZpY2UgfSBmcm9tICdwcmltZW5nL2NvbXBvbmVudHMvY29tbW9uL21lc3NhZ2VzZXJ2aWNlJztcbmltcG9ydCB7IFRvYXN0TW9kdWxlIH0gZnJvbSAncHJpbWVuZy90b2FzdCc7XG5pbXBvcnQgeyBmb3JrSm9pbiB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgdGFrZSB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7IENvbmZpcm1hdGlvbkNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9jb25maXJtYXRpb24vY29uZmlybWF0aW9uLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBFcnJvckNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9lcnJvcnMvZXJyb3IuY29tcG9uZW50JztcbmltcG9ydCB7IExvYWRlckJhckNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9sb2FkZXItYmFyL2xvYWRlci1iYXIuY29tcG9uZW50JztcbmltcG9ydCB7IE1vZGFsQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL21vZGFsL21vZGFsLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBUb2FzdENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy90b2FzdC90b2FzdC5jb21wb25lbnQnO1xuaW1wb3J0IHN0eWxlcyBmcm9tICcuL2NvbnRhbnRzL3N0eWxlcyc7XG5pbXBvcnQgeyBFcnJvckhhbmRsZXIgfSBmcm9tICcuL2hhbmRsZXJzL2Vycm9yLmhhbmRsZXInO1xuaW1wb3J0IHsgQnV0dG9uQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2J1dHRvbi9idXR0b24uY29tcG9uZW50JztcbmltcG9ydCB7IFZhbGlkYXRpb25FcnJvckNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9lcnJvcnMvdmFsaWRhdGlvbi1lcnJvci5jb21wb25lbnQnO1xuXG5leHBvcnQgZnVuY3Rpb24gYXBwZW5kU2NyaXB0KGluamVjdG9yOiBJbmplY3Rvcikge1xuICBjb25zdCBmbiA9IGZ1bmN0aW9uKCkge1xuICAgIGNvbnN0IGxhenlMb2FkU2VydmljZTogTGF6eUxvYWRTZXJ2aWNlID0gaW5qZWN0b3IuZ2V0KExhenlMb2FkU2VydmljZSk7XG5cbiAgICByZXR1cm4gZm9ya0pvaW4oXG4gICAgICBsYXp5TG9hZFNlcnZpY2UubG9hZChcbiAgICAgICAgbnVsbCxcbiAgICAgICAgJ3N0eWxlJyxcbiAgICAgICAgc3R5bGVzLFxuICAgICAgICAnaGVhZCcsXG4gICAgICAgICdhZnRlcmJlZ2luJyxcbiAgICAgICkgLyogbGF6eUxvYWRTZXJ2aWNlLmxvYWQobnVsbCwgJ3NjcmlwdCcsIHNjcmlwdHMpICovLFxuICAgICkucGlwZSh0YWtlKDEpKTtcbiAgfTtcblxuICByZXR1cm4gZm47XG59XG5cbkBOZ01vZHVsZSh7XG4gIGltcG9ydHM6IFtcbiAgICBDb3JlTW9kdWxlLFxuICAgIFRvYXN0TW9kdWxlLFxuICAgIE5nYk1vZGFsTW9kdWxlLFxuICAgIE5neFZhbGlkYXRlQ29yZU1vZHVsZS5mb3JSb290KHtcbiAgICAgIHRhcmdldFNlbGVjdG9yOiAnLmZvcm0tZ3JvdXAnLFxuICAgICAgYmx1ZXByaW50czoge1xuICAgICAgICBlbWFpbDogYEFicEFjY291bnQ6OlRoaXNGaWVsZElzTm90QVZhbGlkRW1haWxBZGRyZXNzLmAsXG4gICAgICAgIG1heDogYEFicEFjY291bnQ6OlRoaXNGaWVsZE11c3RCZUFTdHJpbmdXaXRoQU1heGltdW1MZW5ndGhPZnsxfVt7eyBtYXggfX1dYCxcbiAgICAgICAgbWF4bGVuZ3RoOiBgQWJwQWNjb3VudDo6VGhpc0ZpZWxkTXVzdEJlQVN0cmluZ1dpdGhBTWF4aW11bUxlbmd0aE9mezF9W3t7IHJlcXVpcmVkTGVuZ3RoIH19XWAsXG4gICAgICAgIG1pbjogYEFicEFjY291bnQ6OlRoaXNGaWVsZE11c3RCZUFTdHJpbmdXaXRoQU1pbmltdW1MZW5ndGhPZnsxfUFuZEFNYXhpbXVtTGVuZ3RoT2Z7MH1be3sgbWluIH19LHt7IG1heCB9fV1gLFxuICAgICAgICBtaW5sZW5ndGg6IGBBYnBBY2NvdW50OjpUaGlzRmllbGRNdXN0QmVBU3RyaW5nV2l0aEFNaW5pbXVtTGVuZ3RoT2Z7MX1BbmRBTWF4aW11bUxlbmd0aE9mezB9W3t7IG1pbiB9fSx7eyBtYXggfX1dYCxcbiAgICAgICAgcmVxdWlyZWQ6IGBBYnBBY2NvdW50OjpUaGlzRmllbGRJc1JlcXVpcmVkLmAsXG4gICAgICAgIHBhc3N3b3JkTWlzbWF0Y2g6IGBBYnBJZGVudGl0eTo6SWRlbnRpdHkuUGFzc3dvcmRDb25maXJtYXRpb25GYWlsZWRgLFxuICAgICAgfSxcbiAgICAgIGVycm9yVGVtcGxhdGU6IFZhbGlkYXRpb25FcnJvckNvbXBvbmVudCxcbiAgICB9KSxcbiAgXSxcbiAgZGVjbGFyYXRpb25zOiBbXG4gICAgQnV0dG9uQ29tcG9uZW50LFxuICAgIENvbmZpcm1hdGlvbkNvbXBvbmVudCxcbiAgICBUb2FzdENvbXBvbmVudCxcbiAgICBNb2RhbENvbXBvbmVudCxcbiAgICBFcnJvckNvbXBvbmVudCxcbiAgICBMb2FkZXJCYXJDb21wb25lbnQsXG4gICAgVmFsaWRhdGlvbkVycm9yQ29tcG9uZW50LFxuICBdLFxuICBleHBvcnRzOiBbTmdiTW9kYWxNb2R1bGUsIEJ1dHRvbkNvbXBvbmVudCwgQ29uZmlybWF0aW9uQ29tcG9uZW50LCBUb2FzdENvbXBvbmVudCwgTW9kYWxDb21wb25lbnQsIExvYWRlckJhckNvbXBvbmVudF0sXG4gIGVudHJ5Q29tcG9uZW50czogW0Vycm9yQ29tcG9uZW50LCBWYWxpZGF0aW9uRXJyb3JDb21wb25lbnRdLFxufSlcbmV4cG9ydCBjbGFzcyBUaGVtZVNoYXJlZE1vZHVsZSB7XG4gIHN0YXRpYyBmb3JSb290KCk6IE1vZHVsZVdpdGhQcm92aWRlcnMge1xuICAgIHJldHVybiB7XG4gICAgICBuZ01vZHVsZTogVGhlbWVTaGFyZWRNb2R1bGUsXG4gICAgICBwcm92aWRlcnM6IFtcbiAgICAgICAge1xuICAgICAgICAgIHByb3ZpZGU6IEFQUF9JTklUSUFMSVpFUixcbiAgICAgICAgICBtdWx0aTogdHJ1ZSxcbiAgICAgICAgICBkZXBzOiBbSW5qZWN0b3IsIEVycm9ySGFuZGxlcl0sXG4gICAgICAgICAgdXNlRmFjdG9yeTogYXBwZW5kU2NyaXB0LFxuICAgICAgICB9LFxuICAgICAgICB7IHByb3ZpZGU6IE1lc3NhZ2VTZXJ2aWNlLCB1c2VDbGFzczogTWVzc2FnZVNlcnZpY2UgfSxcbiAgICAgIF0sXG4gICAgfTtcbiAgfVxufVxuIl19