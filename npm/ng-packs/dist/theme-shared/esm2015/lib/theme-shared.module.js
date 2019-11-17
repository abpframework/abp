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
import { forkJoin } from 'rxjs';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { ButtonComponent } from './components/button/button.component';
import { ChartComponent } from './components/chart/chart.component';
import { ConfirmationComponent } from './components/confirmation/confirmation.component';
import { ErrorComponent } from './components/error/error.component';
import { LoaderBarComponent } from './components/loader-bar/loader-bar.component';
import { ModalComponent } from './components/modal/modal.component';
import { SortOrderIconComponent } from './components/sort-order-icon/sort-order-icon.component';
import { TableEmptyMessageComponent } from './components/table-empty-message/table-empty-message.component';
import { ToastComponent } from './components/toast/toast.component';
import styles from './contants/styles';
import { TableSortDirective } from './directives/table-sort.directive';
import { ErrorHandler } from './handlers/error.handler';
import { chartJsLoaded$ } from './utils/widget-utils';
import { HTTP_ERROR_CONFIG, httpErrorConfigFactory } from './tokens/error-pages.token';
/**
 * @param {?} injector
 * @return {?}
 */
export function appendScript(injector) {
    /** @type {?} */
    const fn = (/**
     * @return {?}
     */
    () => {
        import('chart.js').then((/**
         * @return {?}
         */
        () => chartJsLoaded$.next(true)));
        /** @type {?} */
        const lazyLoadService = injector.get(LazyLoadService);
        return forkJoin(lazyLoadService.load(null, 'style', styles, 'head', 'afterbegin') /* lazyLoadService.load(null, 'script', scripts) */).toPromise();
    });
    return fn;
}
export class ThemeSharedModule {
    /**
     * @param {?=} options
     * @return {?}
     */
    static forRoot(options = (/** @type {?} */ ({}))) {
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
                { provide: HTTP_ERROR_CONFIG, useValue: options.httpErrorConfig },
                {
                    provide: 'HTTP_ERROR_CONFIG',
                    useFactory: httpErrorConfigFactory,
                    deps: [HTTP_ERROR_CONFIG],
                },
            ],
        };
    }
}
ThemeSharedModule.decorators = [
    { type: NgModule, args: [{
                imports: [CoreModule, ToastModule, NgxValidateCoreModule],
                declarations: [
                    BreadcrumbComponent,
                    ButtonComponent,
                    ChartComponent,
                    ConfirmationComponent,
                    ErrorComponent,
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
                entryComponents: [ErrorComponent],
            },] }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGhlbWUtc2hhcmVkLm1vZHVsZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL3RoZW1lLXNoYXJlZC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLGVBQWUsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUMzRCxPQUFPLEVBQUUsZUFBZSxFQUFFLFFBQVEsRUFBdUIsUUFBUSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3pGLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBQzNELE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSwwQ0FBMEMsQ0FBQztBQUMxRSxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDaEMsT0FBTyxFQUFFLG1CQUFtQixFQUFFLE1BQU0sOENBQThDLENBQUM7QUFDbkYsT0FBTyxFQUFFLGVBQWUsRUFBRSxNQUFNLHNDQUFzQyxDQUFDO0FBQ3ZFLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxvQ0FBb0MsQ0FBQztBQUNwRSxPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSxrREFBa0QsQ0FBQztBQUN6RixPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDcEUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0sOENBQThDLENBQUM7QUFDbEYsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLG9DQUFvQyxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxzQkFBc0IsRUFBRSxNQUFNLHdEQUF3RCxDQUFDO0FBQ2hHLE9BQU8sRUFBRSwwQkFBMEIsRUFBRSxNQUFNLGdFQUFnRSxDQUFDO0FBQzVHLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxvQ0FBb0MsQ0FBQztBQUNwRSxPQUFPLE1BQU0sTUFBTSxtQkFBbUIsQ0FBQztBQUN2QyxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSxtQ0FBbUMsQ0FBQztBQUN2RSxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0sMEJBQTBCLENBQUM7QUFDeEQsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBRXRELE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxzQkFBc0IsRUFBRSxNQUFNLDRCQUE0QixDQUFDOzs7OztBQUV2RixNQUFNLFVBQVUsWUFBWSxDQUFDLFFBQWtCOztVQUN2QyxFQUFFOzs7SUFBRyxHQUFHLEVBQUU7UUFDZCxNQUFNLENBQUMsVUFBVSxDQUFDLENBQUMsSUFBSTs7O1FBQUMsR0FBRyxFQUFFLENBQUMsY0FBYyxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsRUFBQyxDQUFDOztjQUVuRCxlQUFlLEdBQW9CLFFBQVEsQ0FBQyxHQUFHLENBQUMsZUFBZSxDQUFDO1FBRXRFLE9BQU8sUUFBUSxDQUNiLGVBQWUsQ0FBQyxJQUFJLENBQ2xCLElBQUksRUFDSixPQUFPLEVBQ1AsTUFBTSxFQUNOLE1BQU0sRUFDTixZQUFZLENBQ2IsQ0FBQyxtREFBbUQsQ0FDdEQsQ0FBQyxTQUFTLEVBQUUsQ0FBQztJQUNoQixDQUFDLENBQUE7SUFFRCxPQUFPLEVBQUUsQ0FBQztBQUNaLENBQUM7QUErQkQsTUFBTSxPQUFPLGlCQUFpQjs7Ozs7SUFDNUIsTUFBTSxDQUFDLE9BQU8sQ0FBQyxPQUFPLEdBQUcsbUJBQUEsRUFBRSxFQUFjO1FBQ3ZDLE9BQU87WUFDTCxRQUFRLEVBQUUsaUJBQWlCO1lBQzNCLFNBQVMsRUFBRTtnQkFDVDtvQkFDRSxPQUFPLEVBQUUsZUFBZTtvQkFDeEIsS0FBSyxFQUFFLElBQUk7b0JBQ1gsSUFBSSxFQUFFLENBQUMsUUFBUSxFQUFFLFlBQVksQ0FBQztvQkFDOUIsVUFBVSxFQUFFLFlBQVk7aUJBQ3pCO2dCQUNELEVBQUUsT0FBTyxFQUFFLGNBQWMsRUFBRSxRQUFRLEVBQUUsY0FBYyxFQUFFO2dCQUNyRCxFQUFFLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxRQUFRLEVBQUUsT0FBTyxDQUFDLGVBQWUsRUFBRTtnQkFDakU7b0JBQ0UsT0FBTyxFQUFFLG1CQUFtQjtvQkFDNUIsVUFBVSxFQUFFLHNCQUFzQjtvQkFDbEMsSUFBSSxFQUFFLENBQUMsaUJBQWlCLENBQUM7aUJBQzFCO2FBQ0Y7U0FDRixDQUFDO0lBQ0osQ0FBQzs7O1lBakRGLFFBQVEsU0FBQztnQkFDUixPQUFPLEVBQUUsQ0FBQyxVQUFVLEVBQUUsV0FBVyxFQUFFLHFCQUFxQixDQUFDO2dCQUN6RCxZQUFZLEVBQUU7b0JBQ1osbUJBQW1CO29CQUNuQixlQUFlO29CQUNmLGNBQWM7b0JBQ2QscUJBQXFCO29CQUNyQixjQUFjO29CQUNkLGtCQUFrQjtvQkFDbEIsY0FBYztvQkFDZCwwQkFBMEI7b0JBQzFCLGNBQWM7b0JBQ2Qsc0JBQXNCO29CQUN0QixrQkFBa0I7aUJBQ25CO2dCQUNELE9BQU8sRUFBRTtvQkFDUCxtQkFBbUI7b0JBQ25CLGVBQWU7b0JBQ2YsY0FBYztvQkFDZCxxQkFBcUI7b0JBQ3JCLGtCQUFrQjtvQkFDbEIsY0FBYztvQkFDZCwwQkFBMEI7b0JBQzFCLGNBQWM7b0JBQ2Qsc0JBQXNCO29CQUN0QixrQkFBa0I7aUJBQ25CO2dCQUNELGVBQWUsRUFBRSxDQUFDLGNBQWMsQ0FBQzthQUNsQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvcmVNb2R1bGUsIExhenlMb2FkU2VydmljZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcbmltcG9ydCB7IEFQUF9JTklUSUFMSVpFUiwgSW5qZWN0b3IsIE1vZHVsZVdpdGhQcm92aWRlcnMsIE5nTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IE5neFZhbGlkYXRlQ29yZU1vZHVsZSB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XHJcbmltcG9ydCB7IE1lc3NhZ2VTZXJ2aWNlIH0gZnJvbSAncHJpbWVuZy9jb21wb25lbnRzL2NvbW1vbi9tZXNzYWdlc2VydmljZSc7XHJcbmltcG9ydCB7IFRvYXN0TW9kdWxlIH0gZnJvbSAncHJpbWVuZy90b2FzdCc7XHJcbmltcG9ydCB7IGZvcmtKb2luIH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IEJyZWFkY3J1bWJDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvYnJlYWRjcnVtYi9icmVhZGNydW1iLmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IEJ1dHRvbkNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9idXR0b24vYnV0dG9uLmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IENoYXJ0Q29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2NoYXJ0L2NoYXJ0LmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IENvbmZpcm1hdGlvbkNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9jb25maXJtYXRpb24vY29uZmlybWF0aW9uLmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IEVycm9yQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2Vycm9yL2Vycm9yLmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IExvYWRlckJhckNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9sb2FkZXItYmFyL2xvYWRlci1iYXIuY29tcG9uZW50JztcclxuaW1wb3J0IHsgTW9kYWxDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvbW9kYWwvbW9kYWwuY29tcG9uZW50JztcclxuaW1wb3J0IHsgU29ydE9yZGVySWNvbkNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9zb3J0LW9yZGVyLWljb24vc29ydC1vcmRlci1pY29uLmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IFRhYmxlRW1wdHlNZXNzYWdlQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3RhYmxlLWVtcHR5LW1lc3NhZ2UvdGFibGUtZW1wdHktbWVzc2FnZS5jb21wb25lbnQnO1xyXG5pbXBvcnQgeyBUb2FzdENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy90b2FzdC90b2FzdC5jb21wb25lbnQnO1xyXG5pbXBvcnQgc3R5bGVzIGZyb20gJy4vY29udGFudHMvc3R5bGVzJztcclxuaW1wb3J0IHsgVGFibGVTb3J0RGlyZWN0aXZlIH0gZnJvbSAnLi9kaXJlY3RpdmVzL3RhYmxlLXNvcnQuZGlyZWN0aXZlJztcclxuaW1wb3J0IHsgRXJyb3JIYW5kbGVyIH0gZnJvbSAnLi9oYW5kbGVycy9lcnJvci5oYW5kbGVyJztcclxuaW1wb3J0IHsgY2hhcnRKc0xvYWRlZCQgfSBmcm9tICcuL3V0aWxzL3dpZGdldC11dGlscyc7XHJcbmltcG9ydCB7IFJvb3RQYXJhbXMgfSBmcm9tICcuL21vZGVscy9jb21tb24nO1xyXG5pbXBvcnQgeyBIVFRQX0VSUk9SX0NPTkZJRywgaHR0cEVycm9yQ29uZmlnRmFjdG9yeSB9IGZyb20gJy4vdG9rZW5zL2Vycm9yLXBhZ2VzLnRva2VuJztcclxuXHJcbmV4cG9ydCBmdW5jdGlvbiBhcHBlbmRTY3JpcHQoaW5qZWN0b3I6IEluamVjdG9yKSB7XHJcbiAgY29uc3QgZm4gPSAoKSA9PiB7XHJcbiAgICBpbXBvcnQoJ2NoYXJ0LmpzJykudGhlbigoKSA9PiBjaGFydEpzTG9hZGVkJC5uZXh0KHRydWUpKTtcclxuXHJcbiAgICBjb25zdCBsYXp5TG9hZFNlcnZpY2U6IExhenlMb2FkU2VydmljZSA9IGluamVjdG9yLmdldChMYXp5TG9hZFNlcnZpY2UpO1xyXG5cclxuICAgIHJldHVybiBmb3JrSm9pbihcclxuICAgICAgbGF6eUxvYWRTZXJ2aWNlLmxvYWQoXHJcbiAgICAgICAgbnVsbCxcclxuICAgICAgICAnc3R5bGUnLFxyXG4gICAgICAgIHN0eWxlcyxcclxuICAgICAgICAnaGVhZCcsXHJcbiAgICAgICAgJ2FmdGVyYmVnaW4nLFxyXG4gICAgICApIC8qIGxhenlMb2FkU2VydmljZS5sb2FkKG51bGwsICdzY3JpcHQnLCBzY3JpcHRzKSAqLyxcclxuICAgICkudG9Qcm9taXNlKCk7XHJcbiAgfTtcclxuXHJcbiAgcmV0dXJuIGZuO1xyXG59XHJcblxyXG5ATmdNb2R1bGUoe1xyXG4gIGltcG9ydHM6IFtDb3JlTW9kdWxlLCBUb2FzdE1vZHVsZSwgTmd4VmFsaWRhdGVDb3JlTW9kdWxlXSxcclxuICBkZWNsYXJhdGlvbnM6IFtcclxuICAgIEJyZWFkY3J1bWJDb21wb25lbnQsXHJcbiAgICBCdXR0b25Db21wb25lbnQsXHJcbiAgICBDaGFydENvbXBvbmVudCxcclxuICAgIENvbmZpcm1hdGlvbkNvbXBvbmVudCxcclxuICAgIEVycm9yQ29tcG9uZW50LFxyXG4gICAgTG9hZGVyQmFyQ29tcG9uZW50LFxyXG4gICAgTW9kYWxDb21wb25lbnQsXHJcbiAgICBUYWJsZUVtcHR5TWVzc2FnZUNvbXBvbmVudCxcclxuICAgIFRvYXN0Q29tcG9uZW50LFxyXG4gICAgU29ydE9yZGVySWNvbkNvbXBvbmVudCxcclxuICAgIFRhYmxlU29ydERpcmVjdGl2ZSxcclxuICBdLFxyXG4gIGV4cG9ydHM6IFtcclxuICAgIEJyZWFkY3J1bWJDb21wb25lbnQsXHJcbiAgICBCdXR0b25Db21wb25lbnQsXHJcbiAgICBDaGFydENvbXBvbmVudCxcclxuICAgIENvbmZpcm1hdGlvbkNvbXBvbmVudCxcclxuICAgIExvYWRlckJhckNvbXBvbmVudCxcclxuICAgIE1vZGFsQ29tcG9uZW50LFxyXG4gICAgVGFibGVFbXB0eU1lc3NhZ2VDb21wb25lbnQsXHJcbiAgICBUb2FzdENvbXBvbmVudCxcclxuICAgIFNvcnRPcmRlckljb25Db21wb25lbnQsXHJcbiAgICBUYWJsZVNvcnREaXJlY3RpdmUsXHJcbiAgXSxcclxuICBlbnRyeUNvbXBvbmVudHM6IFtFcnJvckNvbXBvbmVudF0sXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBUaGVtZVNoYXJlZE1vZHVsZSB7XHJcbiAgc3RhdGljIGZvclJvb3Qob3B0aW9ucyA9IHt9IGFzIFJvb3RQYXJhbXMpOiBNb2R1bGVXaXRoUHJvdmlkZXJzIHtcclxuICAgIHJldHVybiB7XHJcbiAgICAgIG5nTW9kdWxlOiBUaGVtZVNoYXJlZE1vZHVsZSxcclxuICAgICAgcHJvdmlkZXJzOiBbXHJcbiAgICAgICAge1xyXG4gICAgICAgICAgcHJvdmlkZTogQVBQX0lOSVRJQUxJWkVSLFxyXG4gICAgICAgICAgbXVsdGk6IHRydWUsXHJcbiAgICAgICAgICBkZXBzOiBbSW5qZWN0b3IsIEVycm9ySGFuZGxlcl0sXHJcbiAgICAgICAgICB1c2VGYWN0b3J5OiBhcHBlbmRTY3JpcHQsXHJcbiAgICAgICAgfSxcclxuICAgICAgICB7IHByb3ZpZGU6IE1lc3NhZ2VTZXJ2aWNlLCB1c2VDbGFzczogTWVzc2FnZVNlcnZpY2UgfSxcclxuICAgICAgICB7IHByb3ZpZGU6IEhUVFBfRVJST1JfQ09ORklHLCB1c2VWYWx1ZTogb3B0aW9ucy5odHRwRXJyb3JDb25maWcgfSxcclxuICAgICAgICB7XHJcbiAgICAgICAgICBwcm92aWRlOiAnSFRUUF9FUlJPUl9DT05GSUcnLFxyXG4gICAgICAgICAgdXNlRmFjdG9yeTogaHR0cEVycm9yQ29uZmlnRmFjdG9yeSxcclxuICAgICAgICAgIGRlcHM6IFtIVFRQX0VSUk9SX0NPTkZJR10sXHJcbiAgICAgICAgfSxcclxuICAgICAgXSxcclxuICAgIH07XHJcbiAgfVxyXG59XHJcbiJdfQ==