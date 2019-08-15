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
    const fn = (/**
     * @return {?}
     */
    function () {
        /** @type {?} */
        const lazyLoadService = injector.get(LazyLoadService);
        return forkJoin(lazyLoadService.load(null, 'style', styles, 'head', 'afterbegin') /* lazyLoadService.load(null, 'script', scripts) */).pipe(take(1));
    });
    return fn;
}
export class ThemeSharedModule {
    /**
     * @return {?}
     */
    static forRoot() {
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
    }
}
ThemeSharedModule.decorators = [
    { type: NgModule, args: [{
                imports: [
                    CoreModule,
                    ToastModule,
                    NgbModalModule,
                    NgxValidateCoreModule.forRoot({
                        targetSelector: '.form-group',
                        blueprints: {
                            email: `AbpAccount::ThisFieldIsNotAValidEmailAddress.`,
                            max: `AbpAccount::ThisFieldMustBeAStringWithAMaximumLengthOf{1}[{{ max }}]`,
                            maxlength: `AbpAccount::ThisFieldMustBeAStringWithAMaximumLengthOf{1}[{{ requiredLength }}]`,
                            min: `AbpAccount::ThisFieldMustBeAStringWithAMinimumLengthOf{1}AndAMaximumLengthOf{0}[{{ min }},{{ max }}]`,
                            minlength: `AbpAccount::ThisFieldMustBeAStringWithAMinimumLengthOf{1}AndAMaximumLengthOf{0}[{{ min }},{{ max }}]`,
                            required: `AbpAccount::ThisFieldIsRequired.`,
                            passwordMismatch: `AbpIdentity::Identity.PasswordConfirmationFailed`,
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGhlbWUtc2hhcmVkLm1vZHVsZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL3RoZW1lLXNoYXJlZC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsZUFBZSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzNELE9BQU8sRUFBRSxlQUFlLEVBQUUsUUFBUSxFQUF1QixRQUFRLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDekYsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLDRCQUE0QixDQUFDO0FBQzVELE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBQzNELE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSwwQ0FBMEMsQ0FBQztBQUMxRSxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDaEMsT0FBTyxFQUFFLElBQUksRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3RDLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLGtEQUFrRCxDQUFDO0FBQ3pGLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxxQ0FBcUMsQ0FBQztBQUNyRSxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSw4Q0FBOEMsQ0FBQztBQUNsRixPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDcEUsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLG9DQUFvQyxDQUFDO0FBQ3BFLE9BQU8sTUFBTSxNQUFNLG1CQUFtQixDQUFDO0FBQ3ZDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSwwQkFBMEIsQ0FBQztBQUN4RCxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sc0NBQXNDLENBQUM7QUFDdkUsT0FBTyxFQUFFLHdCQUF3QixFQUFFLE1BQU0sZ0RBQWdELENBQUM7Ozs7O0FBRTFGLE1BQU0sVUFBVSxZQUFZLENBQUMsUUFBa0I7O1VBQ3ZDLEVBQUU7OztJQUFHOztjQUNILGVBQWUsR0FBb0IsUUFBUSxDQUFDLEdBQUcsQ0FBQyxlQUFlLENBQUM7UUFFdEUsT0FBTyxRQUFRLENBQ2IsZUFBZSxDQUFDLElBQUksQ0FDbEIsSUFBSSxFQUNKLE9BQU8sRUFDUCxNQUFNLEVBQ04sTUFBTSxFQUNOLFlBQVksQ0FDYixDQUFDLG1EQUFtRCxDQUN0RCxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQztJQUNsQixDQUFDLENBQUE7SUFFRCxPQUFPLEVBQUUsQ0FBQztBQUNaLENBQUM7QUFpQ0QsTUFBTSxPQUFPLGlCQUFpQjs7OztJQUM1QixNQUFNLENBQUMsT0FBTztRQUNaLE9BQU87WUFDTCxRQUFRLEVBQUUsaUJBQWlCO1lBQzNCLFNBQVMsRUFBRTtnQkFDVDtvQkFDRSxPQUFPLEVBQUUsZUFBZTtvQkFDeEIsS0FBSyxFQUFFLElBQUk7b0JBQ1gsSUFBSSxFQUFFLENBQUMsUUFBUSxFQUFFLFlBQVksQ0FBQztvQkFDOUIsVUFBVSxFQUFFLFlBQVk7aUJBQ3pCO2dCQUNELEVBQUUsT0FBTyxFQUFFLGNBQWMsRUFBRSxRQUFRLEVBQUUsY0FBYyxFQUFFO2FBQ3REO1NBQ0YsQ0FBQztJQUNKLENBQUM7OztZQTdDRixRQUFRLFNBQUM7Z0JBQ1IsT0FBTyxFQUFFO29CQUNQLFVBQVU7b0JBQ1YsV0FBVztvQkFDWCxjQUFjO29CQUNkLHFCQUFxQixDQUFDLE9BQU8sQ0FBQzt3QkFDNUIsY0FBYyxFQUFFLGFBQWE7d0JBQzdCLFVBQVUsRUFBRTs0QkFDVixLQUFLLEVBQUUsK0NBQStDOzRCQUN0RCxHQUFHLEVBQUUsc0VBQXNFOzRCQUMzRSxTQUFTLEVBQUUsaUZBQWlGOzRCQUM1RixHQUFHLEVBQUUsc0dBQXNHOzRCQUMzRyxTQUFTLEVBQUUsc0dBQXNHOzRCQUNqSCxRQUFRLEVBQUUsa0NBQWtDOzRCQUM1QyxnQkFBZ0IsRUFBRSxrREFBa0Q7eUJBQ3JFO3dCQUNELGFBQWEsRUFBRSx3QkFBd0I7cUJBQ3hDLENBQUM7aUJBQ0g7Z0JBQ0QsWUFBWSxFQUFFO29CQUNaLGVBQWU7b0JBQ2YscUJBQXFCO29CQUNyQixjQUFjO29CQUNkLGNBQWM7b0JBQ2QsY0FBYztvQkFDZCxrQkFBa0I7b0JBQ2xCLHdCQUF3QjtpQkFDekI7Z0JBQ0QsT0FBTyxFQUFFLENBQUMsY0FBYyxFQUFFLGVBQWUsRUFBRSxxQkFBcUIsRUFBRSxjQUFjLEVBQUUsY0FBYyxFQUFFLGtCQUFrQixDQUFDO2dCQUNySCxlQUFlLEVBQUUsQ0FBQyxjQUFjLEVBQUUsd0JBQXdCLENBQUM7YUFDNUQiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb3JlTW9kdWxlLCBMYXp5TG9hZFNlcnZpY2UgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgQVBQX0lOSVRJQUxJWkVSLCBJbmplY3RvciwgTW9kdWxlV2l0aFByb3ZpZGVycywgTmdNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE5nYk1vZGFsTW9kdWxlIH0gZnJvbSAnQG5nLWJvb3RzdHJhcC9uZy1ib290c3RyYXAnO1xuaW1wb3J0IHsgTmd4VmFsaWRhdGVDb3JlTW9kdWxlIH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcbmltcG9ydCB7IE1lc3NhZ2VTZXJ2aWNlIH0gZnJvbSAncHJpbWVuZy9jb21wb25lbnRzL2NvbW1vbi9tZXNzYWdlc2VydmljZSc7XG5pbXBvcnQgeyBUb2FzdE1vZHVsZSB9IGZyb20gJ3ByaW1lbmcvdG9hc3QnO1xuaW1wb3J0IHsgZm9ya0pvaW4gfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IHRha2UgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgeyBDb25maXJtYXRpb25Db21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvY29uZmlybWF0aW9uL2NvbmZpcm1hdGlvbi5jb21wb25lbnQnO1xuaW1wb3J0IHsgRXJyb3JDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvZXJyb3JzL2Vycm9yLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBMb2FkZXJCYXJDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvbG9hZGVyLWJhci9sb2FkZXItYmFyLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBNb2RhbENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9tb2RhbC9tb2RhbC5jb21wb25lbnQnO1xuaW1wb3J0IHsgVG9hc3RDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvdG9hc3QvdG9hc3QuY29tcG9uZW50JztcbmltcG9ydCBzdHlsZXMgZnJvbSAnLi9jb250YW50cy9zdHlsZXMnO1xuaW1wb3J0IHsgRXJyb3JIYW5kbGVyIH0gZnJvbSAnLi9oYW5kbGVycy9lcnJvci5oYW5kbGVyJztcbmltcG9ydCB7IEJ1dHRvbkNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9idXR0b24vYnV0dG9uLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBWYWxpZGF0aW9uRXJyb3JDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvZXJyb3JzL3ZhbGlkYXRpb24tZXJyb3IuY29tcG9uZW50JztcblxuZXhwb3J0IGZ1bmN0aW9uIGFwcGVuZFNjcmlwdChpbmplY3RvcjogSW5qZWN0b3IpIHtcbiAgY29uc3QgZm4gPSBmdW5jdGlvbigpIHtcbiAgICBjb25zdCBsYXp5TG9hZFNlcnZpY2U6IExhenlMb2FkU2VydmljZSA9IGluamVjdG9yLmdldChMYXp5TG9hZFNlcnZpY2UpO1xuXG4gICAgcmV0dXJuIGZvcmtKb2luKFxuICAgICAgbGF6eUxvYWRTZXJ2aWNlLmxvYWQoXG4gICAgICAgIG51bGwsXG4gICAgICAgICdzdHlsZScsXG4gICAgICAgIHN0eWxlcyxcbiAgICAgICAgJ2hlYWQnLFxuICAgICAgICAnYWZ0ZXJiZWdpbicsXG4gICAgICApIC8qIGxhenlMb2FkU2VydmljZS5sb2FkKG51bGwsICdzY3JpcHQnLCBzY3JpcHRzKSAqLyxcbiAgICApLnBpcGUodGFrZSgxKSk7XG4gIH07XG5cbiAgcmV0dXJuIGZuO1xufVxuXG5ATmdNb2R1bGUoe1xuICBpbXBvcnRzOiBbXG4gICAgQ29yZU1vZHVsZSxcbiAgICBUb2FzdE1vZHVsZSxcbiAgICBOZ2JNb2RhbE1vZHVsZSxcbiAgICBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGUuZm9yUm9vdCh7XG4gICAgICB0YXJnZXRTZWxlY3RvcjogJy5mb3JtLWdyb3VwJyxcbiAgICAgIGJsdWVwcmludHM6IHtcbiAgICAgICAgZW1haWw6IGBBYnBBY2NvdW50OjpUaGlzRmllbGRJc05vdEFWYWxpZEVtYWlsQWRkcmVzcy5gLFxuICAgICAgICBtYXg6IGBBYnBBY2NvdW50OjpUaGlzRmllbGRNdXN0QmVBU3RyaW5nV2l0aEFNYXhpbXVtTGVuZ3RoT2Z7MX1be3sgbWF4IH19XWAsXG4gICAgICAgIG1heGxlbmd0aDogYEFicEFjY291bnQ6OlRoaXNGaWVsZE11c3RCZUFTdHJpbmdXaXRoQU1heGltdW1MZW5ndGhPZnsxfVt7eyByZXF1aXJlZExlbmd0aCB9fV1gLFxuICAgICAgICBtaW46IGBBYnBBY2NvdW50OjpUaGlzRmllbGRNdXN0QmVBU3RyaW5nV2l0aEFNaW5pbXVtTGVuZ3RoT2Z7MX1BbmRBTWF4aW11bUxlbmd0aE9mezB9W3t7IG1pbiB9fSx7eyBtYXggfX1dYCxcbiAgICAgICAgbWlubGVuZ3RoOiBgQWJwQWNjb3VudDo6VGhpc0ZpZWxkTXVzdEJlQVN0cmluZ1dpdGhBTWluaW11bUxlbmd0aE9mezF9QW5kQU1heGltdW1MZW5ndGhPZnswfVt7eyBtaW4gfX0se3sgbWF4IH19XWAsXG4gICAgICAgIHJlcXVpcmVkOiBgQWJwQWNjb3VudDo6VGhpc0ZpZWxkSXNSZXF1aXJlZC5gLFxuICAgICAgICBwYXNzd29yZE1pc21hdGNoOiBgQWJwSWRlbnRpdHk6OklkZW50aXR5LlBhc3N3b3JkQ29uZmlybWF0aW9uRmFpbGVkYCxcbiAgICAgIH0sXG4gICAgICBlcnJvclRlbXBsYXRlOiBWYWxpZGF0aW9uRXJyb3JDb21wb25lbnQsXG4gICAgfSksXG4gIF0sXG4gIGRlY2xhcmF0aW9uczogW1xuICAgIEJ1dHRvbkNvbXBvbmVudCxcbiAgICBDb25maXJtYXRpb25Db21wb25lbnQsXG4gICAgVG9hc3RDb21wb25lbnQsXG4gICAgTW9kYWxDb21wb25lbnQsXG4gICAgRXJyb3JDb21wb25lbnQsXG4gICAgTG9hZGVyQmFyQ29tcG9uZW50LFxuICAgIFZhbGlkYXRpb25FcnJvckNvbXBvbmVudCxcbiAgXSxcbiAgZXhwb3J0czogW05nYk1vZGFsTW9kdWxlLCBCdXR0b25Db21wb25lbnQsIENvbmZpcm1hdGlvbkNvbXBvbmVudCwgVG9hc3RDb21wb25lbnQsIE1vZGFsQ29tcG9uZW50LCBMb2FkZXJCYXJDb21wb25lbnRdLFxuICBlbnRyeUNvbXBvbmVudHM6IFtFcnJvckNvbXBvbmVudCwgVmFsaWRhdGlvbkVycm9yQ29tcG9uZW50XSxcbn0pXG5leHBvcnQgY2xhc3MgVGhlbWVTaGFyZWRNb2R1bGUge1xuICBzdGF0aWMgZm9yUm9vdCgpOiBNb2R1bGVXaXRoUHJvdmlkZXJzIHtcbiAgICByZXR1cm4ge1xuICAgICAgbmdNb2R1bGU6IFRoZW1lU2hhcmVkTW9kdWxlLFxuICAgICAgcHJvdmlkZXJzOiBbXG4gICAgICAgIHtcbiAgICAgICAgICBwcm92aWRlOiBBUFBfSU5JVElBTElaRVIsXG4gICAgICAgICAgbXVsdGk6IHRydWUsXG4gICAgICAgICAgZGVwczogW0luamVjdG9yLCBFcnJvckhhbmRsZXJdLFxuICAgICAgICAgIHVzZUZhY3Rvcnk6IGFwcGVuZFNjcmlwdCxcbiAgICAgICAgfSxcbiAgICAgICAgeyBwcm92aWRlOiBNZXNzYWdlU2VydmljZSwgdXNlQ2xhc3M6IE1lc3NhZ2VTZXJ2aWNlIH0sXG4gICAgICBdLFxuICAgIH07XG4gIH1cbn1cbiJdfQ==