/**
 * @fileoverview added by tsickle
 * Generated from: lib/theme-shared.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CoreModule, LazyLoadService } from '@abp/ng.core';
import { APP_INITIALIZER, Injector, NgModule } from '@angular/core';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { MessageService } from 'primeng/components/common/messageservice';
import { ToastModule } from 'primeng/toast';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { ButtonComponent } from './components/button/button.component';
import { ChartComponent } from './components/chart/chart.component';
import { ConfirmationComponent } from './components/confirmation/confirmation.component';
import { HttpErrorWrapperComponent } from './components/http-error-wrapper/http-error-wrapper.component';
import { LoaderBarComponent } from './components/loader-bar/loader-bar.component';
import { ModalComponent } from './components/modal/modal.component';
import { SortOrderIconComponent } from './components/sort-order-icon/sort-order-icon.component';
import { TableEmptyMessageComponent } from './components/table-empty-message/table-empty-message.component';
import { ToastComponent } from './components/toast/toast.component';
import styles from './constants/styles';
import { TableSortDirective } from './directives/table-sort.directive';
import { ErrorHandler } from './handlers/error.handler';
import { chartJsLoaded$ } from './utils/widget-utils';
import { HTTP_ERROR_CONFIG, httpErrorConfigFactory } from './tokens/http-error.token';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { DateParserFormatter } from './utils/date-parser-formatter';
import { DatePipe } from '@angular/common';
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
        return lazyLoadService.load(null, 'style', styles, 'head', 'beforeend').toPromise();
    });
    return fn;
}
var ThemeSharedModule = /** @class */ (function () {
    function ThemeSharedModule(errorHandler) {
        this.errorHandler = errorHandler;
    }
    /**
     * @param {?=} options
     * @return {?}
     */
    ThemeSharedModule.forRoot = /**
     * @param {?=} options
     * @return {?}
     */
    function (options) {
        if (options === void 0) { options = (/** @type {?} */ ({})); }
        return {
            ngModule: ThemeSharedModule,
            providers: [
                {
                    provide: APP_INITIALIZER,
                    multi: true,
                    deps: [Injector],
                    useFactory: appendScript,
                },
                { provide: MessageService, useClass: MessageService },
                { provide: HTTP_ERROR_CONFIG, useValue: options.httpErrorConfig },
                {
                    provide: 'HTTP_ERROR_CONFIG',
                    useFactory: httpErrorConfigFactory,
                    deps: [HTTP_ERROR_CONFIG],
                },
                { provide: NgbDateParserFormatter, useClass: DateParserFormatter },
            ],
        };
    };
    ThemeSharedModule.decorators = [
        { type: NgModule, args: [{
                    imports: [CoreModule, ToastModule, NgxValidateCoreModule],
                    declarations: [
                        BreadcrumbComponent,
                        ButtonComponent,
                        ChartComponent,
                        ConfirmationComponent,
                        HttpErrorWrapperComponent,
                        LoaderBarComponent,
                        ModalComponent,
                        TableEmptyMessageComponent,
                        ToastComponent,
                        SortOrderIconComponent,
                        TableSortDirective,
                    ],
                    exports: [
                        BreadcrumbComponent,
                        ButtonComponent,
                        ChartComponent,
                        ConfirmationComponent,
                        LoaderBarComponent,
                        ModalComponent,
                        TableEmptyMessageComponent,
                        ToastComponent,
                        SortOrderIconComponent,
                        TableSortDirective,
                    ],
                    providers: [DatePipe],
                    entryComponents: [HttpErrorWrapperComponent],
                },] }
    ];
    /** @nocollapse */
    ThemeSharedModule.ctorParameters = function () { return [
        { type: ErrorHandler }
    ]; };
    return ThemeSharedModule;
}());
export { ThemeSharedModule };
if (false) {
    /**
     * @type {?}
     * @private
     */
    ThemeSharedModule.prototype.errorHandler;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGhlbWUtc2hhcmVkLm1vZHVsZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL3RoZW1lLXNoYXJlZC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLGVBQWUsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUMzRCxPQUFPLEVBQUUsZUFBZSxFQUFFLFFBQVEsRUFBdUIsUUFBUSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3pGLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBQzNELE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSwwQ0FBMEMsQ0FBQztBQUMxRSxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBRTVDLE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLDhDQUE4QyxDQUFDO0FBQ25GLE9BQU8sRUFBRSxlQUFlLEVBQUUsTUFBTSxzQ0FBc0MsQ0FBQztBQUN2RSxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDcEUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sa0RBQWtELENBQUM7QUFDekYsT0FBTyxFQUFFLHlCQUF5QixFQUFFLE1BQU0sOERBQThELENBQUM7QUFDekcsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0sOENBQThDLENBQUM7QUFDbEYsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLG9DQUFvQyxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxzQkFBc0IsRUFBRSxNQUFNLHdEQUF3RCxDQUFDO0FBQ2hHLE9BQU8sRUFBRSwwQkFBMEIsRUFBRSxNQUFNLGdFQUFnRSxDQUFDO0FBQzVHLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxvQ0FBb0MsQ0FBQztBQUNwRSxPQUFPLE1BQU0sTUFBTSxvQkFBb0IsQ0FBQztBQUN4QyxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSxtQ0FBbUMsQ0FBQztBQUN2RSxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0sMEJBQTBCLENBQUM7QUFDeEQsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBRXRELE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxzQkFBc0IsRUFBRSxNQUFNLDJCQUEyQixDQUFDO0FBQ3RGLE9BQU8sRUFBRSxzQkFBc0IsRUFBRSxNQUFNLDRCQUE0QixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLCtCQUErQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQzs7Ozs7QUFFM0MsTUFBTSxVQUFVLFlBQVksQ0FBQyxRQUFrQjs7UUFDdkMsRUFBRTs7O0lBQUc7UUFDVCxNQUFNLENBQUMsVUFBVSxDQUFDLENBQUMsSUFBSTs7O1FBQUMsY0FBTSxPQUFBLGNBQWMsQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLEVBQXpCLENBQXlCLEVBQUMsQ0FBQzs7WUFFbkQsZUFBZSxHQUFvQixRQUFRLENBQUMsR0FBRyxDQUFDLGVBQWUsQ0FBQztRQUN0RSxPQUFPLGVBQWUsQ0FBQyxJQUFJLENBQUMsSUFBSSxFQUFFLE9BQU8sRUFBRSxNQUFNLEVBQUUsTUFBTSxFQUFFLFdBQVcsQ0FBQyxDQUFDLFNBQVMsRUFBRSxDQUFDO0lBQ3RGLENBQUMsQ0FBQTtJQUVELE9BQU8sRUFBRSxDQUFDO0FBQ1osQ0FBQztBQUVEO0lBK0JFLDJCQUFvQixZQUEwQjtRQUExQixpQkFBWSxHQUFaLFlBQVksQ0FBYztJQUFHLENBQUM7Ozs7O0lBRTNDLHlCQUFPOzs7O0lBQWQsVUFBZSxPQUEwQjtRQUExQix3QkFBQSxFQUFBLDZCQUFVLEVBQUUsRUFBYztRQUN2QyxPQUFPO1lBQ0wsUUFBUSxFQUFFLGlCQUFpQjtZQUMzQixTQUFTLEVBQUU7Z0JBQ1Q7b0JBQ0UsT0FBTyxFQUFFLGVBQWU7b0JBQ3hCLEtBQUssRUFBRSxJQUFJO29CQUNYLElBQUksRUFBRSxDQUFDLFFBQVEsQ0FBQztvQkFDaEIsVUFBVSxFQUFFLFlBQVk7aUJBQ3pCO2dCQUNELEVBQUUsT0FBTyxFQUFFLGNBQWMsRUFBRSxRQUFRLEVBQUUsY0FBYyxFQUFFO2dCQUNyRCxFQUFFLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxRQUFRLEVBQUUsT0FBTyxDQUFDLGVBQWUsRUFBRTtnQkFDakU7b0JBQ0UsT0FBTyxFQUFFLG1CQUFtQjtvQkFDNUIsVUFBVSxFQUFFLHNCQUFzQjtvQkFDbEMsSUFBSSxFQUFFLENBQUMsaUJBQWlCLENBQUM7aUJBQzFCO2dCQUNELEVBQUUsT0FBTyxFQUFFLHNCQUFzQixFQUFFLFFBQVEsRUFBRSxtQkFBbUIsRUFBRTthQUNuRTtTQUNGLENBQUM7SUFDSixDQUFDOztnQkFyREYsUUFBUSxTQUFDO29CQUNSLE9BQU8sRUFBRSxDQUFDLFVBQVUsRUFBRSxXQUFXLEVBQUUscUJBQXFCLENBQUM7b0JBQ3pELFlBQVksRUFBRTt3QkFDWixtQkFBbUI7d0JBQ25CLGVBQWU7d0JBQ2YsY0FBYzt3QkFDZCxxQkFBcUI7d0JBQ3JCLHlCQUF5Qjt3QkFDekIsa0JBQWtCO3dCQUNsQixjQUFjO3dCQUNkLDBCQUEwQjt3QkFDMUIsY0FBYzt3QkFDZCxzQkFBc0I7d0JBQ3RCLGtCQUFrQjtxQkFDbkI7b0JBQ0QsT0FBTyxFQUFFO3dCQUNQLG1CQUFtQjt3QkFDbkIsZUFBZTt3QkFDZixjQUFjO3dCQUNkLHFCQUFxQjt3QkFDckIsa0JBQWtCO3dCQUNsQixjQUFjO3dCQUNkLDBCQUEwQjt3QkFDMUIsY0FBYzt3QkFDZCxzQkFBc0I7d0JBQ3RCLGtCQUFrQjtxQkFDbkI7b0JBQ0QsU0FBUyxFQUFFLENBQUMsUUFBUSxDQUFDO29CQUNyQixlQUFlLEVBQUUsQ0FBQyx5QkFBeUIsQ0FBQztpQkFDN0M7Ozs7Z0JBaERRLFlBQVk7O0lBeUVyQix3QkFBQztDQUFBLEFBdERELElBc0RDO1NBeEJZLGlCQUFpQjs7Ozs7O0lBQ2hCLHlDQUFrQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvcmVNb2R1bGUsIExhenlMb2FkU2VydmljZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBBUFBfSU5JVElBTElaRVIsIEluamVjdG9yLCBNb2R1bGVXaXRoUHJvdmlkZXJzLCBOZ01vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgTmd4VmFsaWRhdGVDb3JlTW9kdWxlIH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcbmltcG9ydCB7IE1lc3NhZ2VTZXJ2aWNlIH0gZnJvbSAncHJpbWVuZy9jb21wb25lbnRzL2NvbW1vbi9tZXNzYWdlc2VydmljZSc7XG5pbXBvcnQgeyBUb2FzdE1vZHVsZSB9IGZyb20gJ3ByaW1lbmcvdG9hc3QnO1xuaW1wb3J0IHsgZm9ya0pvaW4gfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IEJyZWFkY3J1bWJDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvYnJlYWRjcnVtYi9icmVhZGNydW1iLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBCdXR0b25Db21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvYnV0dG9uL2J1dHRvbi5jb21wb25lbnQnO1xuaW1wb3J0IHsgQ2hhcnRDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvY2hhcnQvY2hhcnQuY29tcG9uZW50JztcbmltcG9ydCB7IENvbmZpcm1hdGlvbkNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9jb25maXJtYXRpb24vY29uZmlybWF0aW9uLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBIdHRwRXJyb3JXcmFwcGVyQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2h0dHAtZXJyb3Itd3JhcHBlci9odHRwLWVycm9yLXdyYXBwZXIuY29tcG9uZW50JztcbmltcG9ydCB7IExvYWRlckJhckNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9sb2FkZXItYmFyL2xvYWRlci1iYXIuY29tcG9uZW50JztcbmltcG9ydCB7IE1vZGFsQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL21vZGFsL21vZGFsLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBTb3J0T3JkZXJJY29uQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3NvcnQtb3JkZXItaWNvbi9zb3J0LW9yZGVyLWljb24uY29tcG9uZW50JztcbmltcG9ydCB7IFRhYmxlRW1wdHlNZXNzYWdlQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3RhYmxlLWVtcHR5LW1lc3NhZ2UvdGFibGUtZW1wdHktbWVzc2FnZS5jb21wb25lbnQnO1xuaW1wb3J0IHsgVG9hc3RDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvdG9hc3QvdG9hc3QuY29tcG9uZW50JztcbmltcG9ydCBzdHlsZXMgZnJvbSAnLi9jb25zdGFudHMvc3R5bGVzJztcbmltcG9ydCB7IFRhYmxlU29ydERpcmVjdGl2ZSB9IGZyb20gJy4vZGlyZWN0aXZlcy90YWJsZS1zb3J0LmRpcmVjdGl2ZSc7XG5pbXBvcnQgeyBFcnJvckhhbmRsZXIgfSBmcm9tICcuL2hhbmRsZXJzL2Vycm9yLmhhbmRsZXInO1xuaW1wb3J0IHsgY2hhcnRKc0xvYWRlZCQgfSBmcm9tICcuL3V0aWxzL3dpZGdldC11dGlscyc7XG5pbXBvcnQgeyBSb290UGFyYW1zIH0gZnJvbSAnLi9tb2RlbHMvY29tbW9uJztcbmltcG9ydCB7IEhUVFBfRVJST1JfQ09ORklHLCBodHRwRXJyb3JDb25maWdGYWN0b3J5IH0gZnJvbSAnLi90b2tlbnMvaHR0cC1lcnJvci50b2tlbic7XG5pbXBvcnQgeyBOZ2JEYXRlUGFyc2VyRm9ybWF0dGVyIH0gZnJvbSAnQG5nLWJvb3RzdHJhcC9uZy1ib290c3RyYXAnO1xuaW1wb3J0IHsgRGF0ZVBhcnNlckZvcm1hdHRlciB9IGZyb20gJy4vdXRpbHMvZGF0ZS1wYXJzZXItZm9ybWF0dGVyJztcbmltcG9ydCB7IERhdGVQaXBlIH0gZnJvbSAnQGFuZ3VsYXIvY29tbW9uJztcblxuZXhwb3J0IGZ1bmN0aW9uIGFwcGVuZFNjcmlwdChpbmplY3RvcjogSW5qZWN0b3IpIHtcbiAgY29uc3QgZm4gPSAoKSA9PiB7XG4gICAgaW1wb3J0KCdjaGFydC5qcycpLnRoZW4oKCkgPT4gY2hhcnRKc0xvYWRlZCQubmV4dCh0cnVlKSk7XG5cbiAgICBjb25zdCBsYXp5TG9hZFNlcnZpY2U6IExhenlMb2FkU2VydmljZSA9IGluamVjdG9yLmdldChMYXp5TG9hZFNlcnZpY2UpO1xuICAgIHJldHVybiBsYXp5TG9hZFNlcnZpY2UubG9hZChudWxsLCAnc3R5bGUnLCBzdHlsZXMsICdoZWFkJywgJ2JlZm9yZWVuZCcpLnRvUHJvbWlzZSgpO1xuICB9O1xuXG4gIHJldHVybiBmbjtcbn1cblxuQE5nTW9kdWxlKHtcbiAgaW1wb3J0czogW0NvcmVNb2R1bGUsIFRvYXN0TW9kdWxlLCBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGVdLFxuICBkZWNsYXJhdGlvbnM6IFtcbiAgICBCcmVhZGNydW1iQ29tcG9uZW50LFxuICAgIEJ1dHRvbkNvbXBvbmVudCxcbiAgICBDaGFydENvbXBvbmVudCxcbiAgICBDb25maXJtYXRpb25Db21wb25lbnQsXG4gICAgSHR0cEVycm9yV3JhcHBlckNvbXBvbmVudCxcbiAgICBMb2FkZXJCYXJDb21wb25lbnQsXG4gICAgTW9kYWxDb21wb25lbnQsXG4gICAgVGFibGVFbXB0eU1lc3NhZ2VDb21wb25lbnQsXG4gICAgVG9hc3RDb21wb25lbnQsXG4gICAgU29ydE9yZGVySWNvbkNvbXBvbmVudCxcbiAgICBUYWJsZVNvcnREaXJlY3RpdmUsXG4gIF0sXG4gIGV4cG9ydHM6IFtcbiAgICBCcmVhZGNydW1iQ29tcG9uZW50LFxuICAgIEJ1dHRvbkNvbXBvbmVudCxcbiAgICBDaGFydENvbXBvbmVudCxcbiAgICBDb25maXJtYXRpb25Db21wb25lbnQsXG4gICAgTG9hZGVyQmFyQ29tcG9uZW50LFxuICAgIE1vZGFsQ29tcG9uZW50LFxuICAgIFRhYmxlRW1wdHlNZXNzYWdlQ29tcG9uZW50LFxuICAgIFRvYXN0Q29tcG9uZW50LFxuICAgIFNvcnRPcmRlckljb25Db21wb25lbnQsXG4gICAgVGFibGVTb3J0RGlyZWN0aXZlLFxuICBdLFxuICBwcm92aWRlcnM6IFtEYXRlUGlwZV0sXG4gIGVudHJ5Q29tcG9uZW50czogW0h0dHBFcnJvcldyYXBwZXJDb21wb25lbnRdLFxufSlcbmV4cG9ydCBjbGFzcyBUaGVtZVNoYXJlZE1vZHVsZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgZXJyb3JIYW5kbGVyOiBFcnJvckhhbmRsZXIpIHt9XG5cbiAgc3RhdGljIGZvclJvb3Qob3B0aW9ucyA9IHt9IGFzIFJvb3RQYXJhbXMpOiBNb2R1bGVXaXRoUHJvdmlkZXJzIHtcbiAgICByZXR1cm4ge1xuICAgICAgbmdNb2R1bGU6IFRoZW1lU2hhcmVkTW9kdWxlLFxuICAgICAgcHJvdmlkZXJzOiBbXG4gICAgICAgIHtcbiAgICAgICAgICBwcm92aWRlOiBBUFBfSU5JVElBTElaRVIsXG4gICAgICAgICAgbXVsdGk6IHRydWUsXG4gICAgICAgICAgZGVwczogW0luamVjdG9yXSxcbiAgICAgICAgICB1c2VGYWN0b3J5OiBhcHBlbmRTY3JpcHQsXG4gICAgICAgIH0sXG4gICAgICAgIHsgcHJvdmlkZTogTWVzc2FnZVNlcnZpY2UsIHVzZUNsYXNzOiBNZXNzYWdlU2VydmljZSB9LFxuICAgICAgICB7IHByb3ZpZGU6IEhUVFBfRVJST1JfQ09ORklHLCB1c2VWYWx1ZTogb3B0aW9ucy5odHRwRXJyb3JDb25maWcgfSxcbiAgICAgICAge1xuICAgICAgICAgIHByb3ZpZGU6ICdIVFRQX0VSUk9SX0NPTkZJRycsXG4gICAgICAgICAgdXNlRmFjdG9yeTogaHR0cEVycm9yQ29uZmlnRmFjdG9yeSxcbiAgICAgICAgICBkZXBzOiBbSFRUUF9FUlJPUl9DT05GSUddLFxuICAgICAgICB9LFxuICAgICAgICB7IHByb3ZpZGU6IE5nYkRhdGVQYXJzZXJGb3JtYXR0ZXIsIHVzZUNsYXNzOiBEYXRlUGFyc2VyRm9ybWF0dGVyIH0sXG4gICAgICBdLFxuICAgIH07XG4gIH1cbn1cbiJdfQ==