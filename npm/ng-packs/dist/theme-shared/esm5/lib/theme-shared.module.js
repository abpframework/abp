/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CoreModule, LazyLoadService } from '@abp/ng.core';
import { APP_INITIALIZER, Injector, NgModule } from '@angular/core';
import { MessageService } from 'primeng/components/common/messageservice';
import { ToastModule } from 'primeng/toast';
import { forkJoin } from 'rxjs';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { ButtonComponent } from './components/button/button.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { ChartComponent } from './components/chart/chart.component';
import { ConfirmationComponent } from './components/confirmation/confirmation.component';
import { ErrorComponent } from './components/error/error.component';
import { LoaderBarComponent } from './components/loader-bar/loader-bar.component';
import { ModalComponent } from './components/modal/modal.component';
import { ProfileComponent } from './components/profile/profile.component';
import { ToastComponent } from './components/toast/toast.component';
import { SortOrderIconComponent } from './components/sort-order-icon/sort-order-icon.component';
import styles from './contants/styles';
import { ErrorHandler } from './handlers/error.handler';
import { chartJsLoaded$ } from './utils/widget-utils';
import { TableEmptyMessageComponent } from './components/table-empty-message/table-empty-message.component';
import { NgxValidateCoreModule } from '@ngx-validate/core';
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
        import('chart.js').then((/**
         * @return {?}
         */
        function () { return chartJsLoaded$.next(true); }));
        /** @type {?} */
        var lazyLoadService = injector.get(LazyLoadService);
        return forkJoin(lazyLoadService.load(null, 'style', styles, 'head', 'afterbegin') /* lazyLoadService.load(null, 'script', scripts) */).toPromise();
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
                    imports: [CoreModule, ToastModule, NgxValidateCoreModule],
                    declarations: [
                        BreadcrumbComponent,
                        ButtonComponent,
                        ChangePasswordComponent,
                        ChartComponent,
                        ConfirmationComponent,
                        ErrorComponent,
                        LoaderBarComponent,
                        ModalComponent,
                        ProfileComponent,
                        TableEmptyMessageComponent,
                        ToastComponent,
                        SortOrderIconComponent,
                    ],
                    exports: [
                        BreadcrumbComponent,
                        ButtonComponent,
                        ChangePasswordComponent,
                        ChartComponent,
                        ConfirmationComponent,
                        LoaderBarComponent,
                        ModalComponent,
                        ProfileComponent,
                        TableEmptyMessageComponent,
                        ToastComponent,
                        SortOrderIconComponent,
                    ],
                    entryComponents: [ErrorComponent],
                },] }
    ];
    return ThemeSharedModule;
}());
export { ThemeSharedModule };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGhlbWUtc2hhcmVkLm1vZHVsZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL3RoZW1lLXNoYXJlZC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsZUFBZSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzNELE9BQU8sRUFBRSxlQUFlLEVBQUUsUUFBUSxFQUF1QixRQUFRLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDekYsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLDBDQUEwQyxDQUFDO0FBQzFFLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDNUMsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUVoQyxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSw4Q0FBOEMsQ0FBQztBQUNuRixPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sc0NBQXNDLENBQUM7QUFDdkUsT0FBTyxFQUFFLHVCQUF1QixFQUFFLE1BQU0sd0RBQXdELENBQUM7QUFDakcsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLG9DQUFvQyxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLGtEQUFrRCxDQUFDO0FBQ3pGLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxvQ0FBb0MsQ0FBQztBQUNwRSxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSw4Q0FBOEMsQ0FBQztBQUNsRixPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDcEUsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sd0NBQXdDLENBQUM7QUFDMUUsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLG9DQUFvQyxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxzQkFBc0IsRUFBRSxNQUFNLHdEQUF3RCxDQUFDO0FBQ2hHLE9BQU8sTUFBTSxNQUFNLG1CQUFtQixDQUFDO0FBQ3ZDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSwwQkFBMEIsQ0FBQztBQUN4RCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDdEQsT0FBTyxFQUFFLDBCQUEwQixFQUFFLE1BQU0sZ0VBQWdFLENBQUM7QUFDNUcsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7Ozs7O0FBRTNELE1BQU0sVUFBVSxZQUFZLENBQUMsUUFBa0I7O1FBQ3ZDLEVBQUU7OztJQUFHO1FBQ1QsTUFBTSxDQUFDLFVBQVUsQ0FBQyxDQUFDLElBQUk7OztRQUFDLGNBQU0sT0FBQSxjQUFjLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxFQUF6QixDQUF5QixFQUFDLENBQUM7O1lBRW5ELGVBQWUsR0FBb0IsUUFBUSxDQUFDLEdBQUcsQ0FBQyxlQUFlLENBQUM7UUFFdEUsT0FBTyxRQUFRLENBQ2IsZUFBZSxDQUFDLElBQUksQ0FDbEIsSUFBSSxFQUNKLE9BQU8sRUFDUCxNQUFNLEVBQ04sTUFBTSxFQUNOLFlBQVksQ0FDYixDQUFDLG1EQUFtRCxDQUN0RCxDQUFDLFNBQVMsRUFBRSxDQUFDO0lBQ2hCLENBQUMsQ0FBQTtJQUVELE9BQU8sRUFBRSxDQUFDO0FBQ1osQ0FBQztBQUVEO0lBQUE7SUE4Q0EsQ0FBQzs7OztJQWRRLHlCQUFPOzs7SUFBZDtRQUNFLE9BQU87WUFDTCxRQUFRLEVBQUUsaUJBQWlCO1lBQzNCLFNBQVMsRUFBRTtnQkFDVDtvQkFDRSxPQUFPLEVBQUUsZUFBZTtvQkFDeEIsS0FBSyxFQUFFLElBQUk7b0JBQ1gsSUFBSSxFQUFFLENBQUMsUUFBUSxFQUFFLFlBQVksQ0FBQztvQkFDOUIsVUFBVSxFQUFFLFlBQVk7aUJBQ3pCO2dCQUNELEVBQUUsT0FBTyxFQUFFLGNBQWMsRUFBRSxRQUFRLEVBQUUsY0FBYyxFQUFFO2FBQ3REO1NBQ0YsQ0FBQztJQUNKLENBQUM7O2dCQTdDRixRQUFRLFNBQUM7b0JBQ1IsT0FBTyxFQUFFLENBQUMsVUFBVSxFQUFFLFdBQVcsRUFBRSxxQkFBcUIsQ0FBQztvQkFDekQsWUFBWSxFQUFFO3dCQUNaLG1CQUFtQjt3QkFDbkIsZUFBZTt3QkFDZix1QkFBdUI7d0JBQ3ZCLGNBQWM7d0JBQ2QscUJBQXFCO3dCQUNyQixjQUFjO3dCQUNkLGtCQUFrQjt3QkFDbEIsY0FBYzt3QkFDZCxnQkFBZ0I7d0JBQ2hCLDBCQUEwQjt3QkFDMUIsY0FBYzt3QkFDZCxzQkFBc0I7cUJBQ3ZCO29CQUNELE9BQU8sRUFBRTt3QkFDUCxtQkFBbUI7d0JBQ25CLGVBQWU7d0JBQ2YsdUJBQXVCO3dCQUN2QixjQUFjO3dCQUNkLHFCQUFxQjt3QkFDckIsa0JBQWtCO3dCQUNsQixjQUFjO3dCQUNkLGdCQUFnQjt3QkFDaEIsMEJBQTBCO3dCQUMxQixjQUFjO3dCQUNkLHNCQUFzQjtxQkFDdkI7b0JBQ0QsZUFBZSxFQUFFLENBQUMsY0FBYyxDQUFDO2lCQUNsQzs7SUFnQkQsd0JBQUM7Q0FBQSxBQTlDRCxJQThDQztTQWZZLGlCQUFpQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvcmVNb2R1bGUsIExhenlMb2FkU2VydmljZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBBUFBfSU5JVElBTElaRVIsIEluamVjdG9yLCBNb2R1bGVXaXRoUHJvdmlkZXJzLCBOZ01vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgTWVzc2FnZVNlcnZpY2UgfSBmcm9tICdwcmltZW5nL2NvbXBvbmVudHMvY29tbW9uL21lc3NhZ2VzZXJ2aWNlJztcbmltcG9ydCB7IFRvYXN0TW9kdWxlIH0gZnJvbSAncHJpbWVuZy90b2FzdCc7XG5pbXBvcnQgeyBmb3JrSm9pbiB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgdGFrZSB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7IEJyZWFkY3J1bWJDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvYnJlYWRjcnVtYi9icmVhZGNydW1iLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBCdXR0b25Db21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvYnV0dG9uL2J1dHRvbi5jb21wb25lbnQnO1xuaW1wb3J0IHsgQ2hhbmdlUGFzc3dvcmRDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvY2hhbmdlLXBhc3N3b3JkL2NoYW5nZS1wYXNzd29yZC5jb21wb25lbnQnO1xuaW1wb3J0IHsgQ2hhcnRDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvY2hhcnQvY2hhcnQuY29tcG9uZW50JztcbmltcG9ydCB7IENvbmZpcm1hdGlvbkNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9jb25maXJtYXRpb24vY29uZmlybWF0aW9uLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBFcnJvckNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9lcnJvci9lcnJvci5jb21wb25lbnQnO1xuaW1wb3J0IHsgTG9hZGVyQmFyQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2xvYWRlci1iYXIvbG9hZGVyLWJhci5jb21wb25lbnQnO1xuaW1wb3J0IHsgTW9kYWxDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvbW9kYWwvbW9kYWwuY29tcG9uZW50JztcbmltcG9ydCB7IFByb2ZpbGVDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvcHJvZmlsZS9wcm9maWxlLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBUb2FzdENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy90b2FzdC90b2FzdC5jb21wb25lbnQnO1xuaW1wb3J0IHsgU29ydE9yZGVySWNvbkNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9zb3J0LW9yZGVyLWljb24vc29ydC1vcmRlci1pY29uLmNvbXBvbmVudCc7XG5pbXBvcnQgc3R5bGVzIGZyb20gJy4vY29udGFudHMvc3R5bGVzJztcbmltcG9ydCB7IEVycm9ySGFuZGxlciB9IGZyb20gJy4vaGFuZGxlcnMvZXJyb3IuaGFuZGxlcic7XG5pbXBvcnQgeyBjaGFydEpzTG9hZGVkJCB9IGZyb20gJy4vdXRpbHMvd2lkZ2V0LXV0aWxzJztcbmltcG9ydCB7IFRhYmxlRW1wdHlNZXNzYWdlQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3RhYmxlLWVtcHR5LW1lc3NhZ2UvdGFibGUtZW1wdHktbWVzc2FnZS5jb21wb25lbnQnO1xuaW1wb3J0IHsgTmd4VmFsaWRhdGVDb3JlTW9kdWxlIH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcblxuZXhwb3J0IGZ1bmN0aW9uIGFwcGVuZFNjcmlwdChpbmplY3RvcjogSW5qZWN0b3IpIHtcbiAgY29uc3QgZm4gPSAoKSA9PiB7XG4gICAgaW1wb3J0KCdjaGFydC5qcycpLnRoZW4oKCkgPT4gY2hhcnRKc0xvYWRlZCQubmV4dCh0cnVlKSk7XG5cbiAgICBjb25zdCBsYXp5TG9hZFNlcnZpY2U6IExhenlMb2FkU2VydmljZSA9IGluamVjdG9yLmdldChMYXp5TG9hZFNlcnZpY2UpO1xuXG4gICAgcmV0dXJuIGZvcmtKb2luKFxuICAgICAgbGF6eUxvYWRTZXJ2aWNlLmxvYWQoXG4gICAgICAgIG51bGwsXG4gICAgICAgICdzdHlsZScsXG4gICAgICAgIHN0eWxlcyxcbiAgICAgICAgJ2hlYWQnLFxuICAgICAgICAnYWZ0ZXJiZWdpbicsXG4gICAgICApIC8qIGxhenlMb2FkU2VydmljZS5sb2FkKG51bGwsICdzY3JpcHQnLCBzY3JpcHRzKSAqLyxcbiAgICApLnRvUHJvbWlzZSgpO1xuICB9O1xuXG4gIHJldHVybiBmbjtcbn1cblxuQE5nTW9kdWxlKHtcbiAgaW1wb3J0czogW0NvcmVNb2R1bGUsIFRvYXN0TW9kdWxlLCBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGVdLFxuICBkZWNsYXJhdGlvbnM6IFtcbiAgICBCcmVhZGNydW1iQ29tcG9uZW50LFxuICAgIEJ1dHRvbkNvbXBvbmVudCxcbiAgICBDaGFuZ2VQYXNzd29yZENvbXBvbmVudCxcbiAgICBDaGFydENvbXBvbmVudCxcbiAgICBDb25maXJtYXRpb25Db21wb25lbnQsXG4gICAgRXJyb3JDb21wb25lbnQsXG4gICAgTG9hZGVyQmFyQ29tcG9uZW50LFxuICAgIE1vZGFsQ29tcG9uZW50LFxuICAgIFByb2ZpbGVDb21wb25lbnQsXG4gICAgVGFibGVFbXB0eU1lc3NhZ2VDb21wb25lbnQsXG4gICAgVG9hc3RDb21wb25lbnQsXG4gICAgU29ydE9yZGVySWNvbkNvbXBvbmVudCxcbiAgXSxcbiAgZXhwb3J0czogW1xuICAgIEJyZWFkY3J1bWJDb21wb25lbnQsXG4gICAgQnV0dG9uQ29tcG9uZW50LFxuICAgIENoYW5nZVBhc3N3b3JkQ29tcG9uZW50LFxuICAgIENoYXJ0Q29tcG9uZW50LFxuICAgIENvbmZpcm1hdGlvbkNvbXBvbmVudCxcbiAgICBMb2FkZXJCYXJDb21wb25lbnQsXG4gICAgTW9kYWxDb21wb25lbnQsXG4gICAgUHJvZmlsZUNvbXBvbmVudCxcbiAgICBUYWJsZUVtcHR5TWVzc2FnZUNvbXBvbmVudCxcbiAgICBUb2FzdENvbXBvbmVudCxcbiAgICBTb3J0T3JkZXJJY29uQ29tcG9uZW50LFxuICBdLFxuICBlbnRyeUNvbXBvbmVudHM6IFtFcnJvckNvbXBvbmVudF0sXG59KVxuZXhwb3J0IGNsYXNzIFRoZW1lU2hhcmVkTW9kdWxlIHtcbiAgc3RhdGljIGZvclJvb3QoKTogTW9kdWxlV2l0aFByb3ZpZGVycyB7XG4gICAgcmV0dXJuIHtcbiAgICAgIG5nTW9kdWxlOiBUaGVtZVNoYXJlZE1vZHVsZSxcbiAgICAgIHByb3ZpZGVyczogW1xuICAgICAgICB7XG4gICAgICAgICAgcHJvdmlkZTogQVBQX0lOSVRJQUxJWkVSLFxuICAgICAgICAgIG11bHRpOiB0cnVlLFxuICAgICAgICAgIGRlcHM6IFtJbmplY3RvciwgRXJyb3JIYW5kbGVyXSxcbiAgICAgICAgICB1c2VGYWN0b3J5OiBhcHBlbmRTY3JpcHQsXG4gICAgICAgIH0sXG4gICAgICAgIHsgcHJvdmlkZTogTWVzc2FnZVNlcnZpY2UsIHVzZUNsYXNzOiBNZXNzYWdlU2VydmljZSB9LFxuICAgICAgXSxcbiAgICB9O1xuICB9XG59XG4iXX0=