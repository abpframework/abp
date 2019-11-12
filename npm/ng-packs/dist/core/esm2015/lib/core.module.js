/**
 * @fileoverview added by tsickle
 * Generated from: lib/core.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { APP_INITIALIZER, Injector, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgxsRouterPluginModule } from '@ngxs/router-plugin';
import { NgxsStoragePluginModule } from '@ngxs/storage-plugin';
import { NgxsModule, NGXS_PLUGINS } from '@ngxs/store';
import { OAuthModule } from 'angular-oauth2-oidc';
import { AbstractNgModelComponent } from './abstracts/ng-model.component';
import { DynamicLayoutComponent } from './components/dynamic-layout.component';
import { RouterOutletComponent } from './components/router-outlet.component';
import { AutofocusDirective } from './directives/autofocus.directive';
import { InputEventDebounceDirective } from './directives/debounce.directive';
import { EllipsisDirective } from './directives/ellipsis.directive';
import { ForDirective } from './directives/for.directive';
import { FormSubmitDirective } from './directives/form-submit.directive';
import { PermissionDirective } from './directives/permission.directive';
import { ClickEventStopPropagationDirective } from './directives/stop-propagation.directive';
import { VisibilityDirective } from './directives/visibility.directive';
import { ApiInterceptor } from './interceptors/api.interceptor';
import { LocalizationPipe } from './pipes/localization.pipe';
import { SortPipe } from './pipes/sort.pipe';
import { ConfigPlugin, NGXS_CONFIG_PLUGIN_OPTIONS } from './plugins/config.plugin';
import { LocaleProvider } from './providers/locale.provider';
import { ConfigState } from './states/config.state';
import { ProfileState } from './states/profile.state';
import { SessionState } from './states/session.state';
import { getInitialData, localeInitializer } from './utils/initial-utils';
export class CoreModule {
    /**
     * @param {?=} options
     * @return {?}
     */
    static forRoot(options = (/** @type {?} */ ({}))) {
        return {
            ngModule: CoreModule,
            providers: [
                LocaleProvider,
                {
                    provide: NGXS_PLUGINS,
                    useClass: ConfigPlugin,
                    multi: true,
                },
                {
                    provide: NGXS_CONFIG_PLUGIN_OPTIONS,
                    useValue: options,
                },
                {
                    provide: HTTP_INTERCEPTORS,
                    useClass: ApiInterceptor,
                    multi: true,
                },
                {
                    provide: APP_INITIALIZER,
                    multi: true,
                    deps: [Injector],
                    useFactory: getInitialData,
                },
                {
                    provide: APP_INITIALIZER,
                    multi: true,
                    deps: [Injector],
                    useFactory: localeInitializer,
                },
            ],
        };
    }
}
CoreModule.decorators = [
    { type: NgModule, args: [{
                imports: [
                    NgxsModule.forFeature([ProfileState, SessionState, ConfigState]),
                    NgxsRouterPluginModule.forRoot(),
                    NgxsStoragePluginModule.forRoot({ key: ['SessionState'] }),
                    OAuthModule.forRoot(),
                    CommonModule,
                    HttpClientModule,
                    FormsModule,
                    ReactiveFormsModule,
                    RouterModule,
                ],
                declarations: [
                    RouterOutletComponent,
                    DynamicLayoutComponent,
                    AutofocusDirective,
                    EllipsisDirective,
                    ForDirective,
                    FormSubmitDirective,
                    LocalizationPipe,
                    SortPipe,
                    PermissionDirective,
                    VisibilityDirective,
                    InputEventDebounceDirective,
                    ClickEventStopPropagationDirective,
                    AbstractNgModelComponent,
                ],
                exports: [
                    CommonModule,
                    HttpClientModule,
                    FormsModule,
                    ReactiveFormsModule,
                    RouterModule,
                    RouterOutletComponent,
                    DynamicLayoutComponent,
                    AutofocusDirective,
                    EllipsisDirective,
                    ForDirective,
                    FormSubmitDirective,
                    LocalizationPipe,
                    SortPipe,
                    PermissionDirective,
                    VisibilityDirective,
                    InputEventDebounceDirective,
                    LocalizationPipe,
                    ClickEventStopPropagationDirective,
                    AbstractNgModelComponent,
                ],
                providers: [LocalizationPipe],
                entryComponents: [RouterOutletComponent, DynamicLayoutComponent],
            },] }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29yZS5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvY29yZS5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDL0MsT0FBTyxFQUFFLGdCQUFnQixFQUFFLGlCQUFpQixFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDM0UsT0FBTyxFQUFFLGVBQWUsRUFBRSxRQUFRLEVBQXVCLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN6RixPQUFPLEVBQUUsV0FBVyxFQUFFLG1CQUFtQixFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDbEUsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQy9DLE9BQU8sRUFBRSxzQkFBc0IsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQzdELE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQy9ELE9BQU8sRUFBRSxVQUFVLEVBQUUsWUFBWSxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3ZELE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUNsRCxPQUFPLEVBQUUsd0JBQXdCLEVBQUUsTUFBTSxnQ0FBZ0MsQ0FBQztBQUMxRSxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSx1Q0FBdUMsQ0FBQztBQUMvRSxPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSxzQ0FBc0MsQ0FBQztBQUM3RSxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSxrQ0FBa0MsQ0FBQztBQUN0RSxPQUFPLEVBQUUsMkJBQTJCLEVBQUUsTUFBTSxpQ0FBaUMsQ0FBQztBQUM5RSxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSxpQ0FBaUMsQ0FBQztBQUNwRSxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0sNEJBQTRCLENBQUM7QUFDMUQsT0FBTyxF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QkFDakIsWUFBWTtvQkFDWixtQkFBbUI7b0JBQ25CLGdCQUFnQjtvQkFDaEIsUUFBUTtvQkFDUixtQkFBbUI7b0JBQ25CLG1CQUFtQjtvQkFDbkIsMkJBQTJCO29CQUMzQixnQkFBZ0I7b0JBQ2hCLGtDQUFrQztvQkFDbEMsd0JBQXdCO2lCQUN6QjtnQkFDRCxTQUFTLEVBQUUsQ0FBQyxnQkFBZ0IsQ0FBQztnQkFDN0IsZUFBZSxFQUFFLENBQUMscUJBQXFCLEVBQUUsc0JBQXNCLENBQUM7YUFDakUiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21tb25Nb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb21tb24nO1xyXG5pbXBvcnQgeyBIdHRwQ2xpZW50TW9kdWxlLCBIVFRQX0lOVEVSQ0VQVE9SUyB9IGZyb20gJ0Bhbmd1bGFyL2NvbW1vbi9odHRwJztcclxuaW1wb3J0IHsgQVBQX0lOSVRJQUxJWkVSLCBJbmplY3RvciwgTW9kdWxlV2l0aFByb3ZpZGVycywgTmdNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgRm9ybXNNb2R1bGUsIFJlYWN0aXZlRm9ybXNNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9mb3Jtcyc7XHJcbmltcG9ydCB7IFJvdXRlck1vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XHJcbmltcG9ydCB7IE5neHNSb3V0ZXJQbHVnaW5Nb2R1bGUgfSBmcm9tICdAbmd4cy9yb3V0ZXItcGx1Z2luJztcclxuaW1wb3J0IHsgTmd4c1N0b3JhZ2VQbHVnaW5Nb2R1bGUgfSBmcm9tICdAbmd4cy9zdG9yYWdlLXBsdWdpbic7XHJcbmltcG9ydCB7IE5neHNNb2R1bGUsIE5HWFNfUExVR0lOUyB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgT0F1dGhNb2R1bGUgfSBmcm9tICdhbmd1bGFyLW9hdXRoMi1vaWRjJztcclxuaW1wb3J0IHsgQWJzdHJhY3ROZ01vZGVsQ29tcG9uZW50IH0gZnJvbSAnLi9hYnN0cmFjdHMvbmctbW9kZWwuY29tcG9uZW50JztcclxuaW1wb3J0IHsgRHluYW1pY0xheW91dENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9keW5hbWljLWxheW91dC5jb21wb25lbnQnO1xyXG5pbXBvcnQgeyBSb3V0ZXJPdXRsZXRDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvcm91dGVyLW91dGxldC5jb21wb25lbnQnO1xyXG5pbXBvcnQgeyBBdXRvZm9jdXNEaXJlY3RpdmUgfSBmcm9tICcuL2RpcmVjdGl2ZXMvYXV0b2ZvY3VzLmRpcmVjdGl2ZSc7XHJcbmltcG9ydCB7IElucHV0RXZlbnREZWJvdW5jZURpcmVjdGl2ZSB9IGZyb20gJy4vZGlyZWN0aXZlcy9kZWJvdW5jZS5kaXJlY3RpdmUnO1xyXG5pbXBvcnQgeyBFbGxpcHNpc0RpcmVjdGl2ZSB9IGZyb20gJy4vZGlyZWN0aXZlcy9lbGxpcHNpcy5kaXJlY3RpdmUnO1xyXG5pbXBvcnQgeyBGb3JEaXJlY3RpdmUgfSBmcm9tICcuL2RpcmVjdGl2ZXMvZm9yLmRpcmVjdGl2ZSc7XHJcbmltcG9ydCB7IEZvcm1TdWJtaXREaXJlY3RpdmUgfSBmcm9tICcuL2RpcmVjdGl2ZXMvZm9ybS1zdWJtaXQuZGlyZWN0aXZlJztcclxuaW1wb3J0IHsgUGVybWlzc2lvbkRpcmVjdGl2ZSB9IGZyb20gJy4vZGlyZWN0aXZlcy9wZXJtaXNzaW9uLmRpcmVjdGl2ZSc7XHJcbmltcG9ydCB7IENsaWNrRXZlbnRTdG9wUHJvcGFnYXRpb25EaXJlY3RpdmUgfSBmcm9tICcuL2RpcmVjdGl2ZXMvc3RvcC1wcm9wYWdhdGlvbi5kaXJlY3RpdmUnO1xyXG5pbXBvcnQgeyBWaXNpYmlsaXR5RGlyZWN0aXZlIH0gZnJvbSAnLi9kaXJlY3RpdmVzL3Zpc2liaWxpdHkuZGlyZWN0aXZlJztcclxuaW1wb3J0IHsgQXBpSW50ZXJjZXB0b3IgfSBmcm9tICcuL2ludGVyY2VwdG9ycy9hcGkuaW50ZXJjZXB0b3InO1xyXG5pbXBvcnQgeyBBQlAgfSBmcm9tICcuL21vZGVscy9jb21tb24nO1xyXG5pbXBvcnQgeyBMb2NhbGl6YXRpb25QaXBlIH0gZnJvbSAnLi9waXBlcy9sb2NhbGl6YXRpb24ucGlwZSc7XHJcbmltcG9ydCB7IFNvcnRQaXBlIH0gZnJvbSAnLi9waXBlcy9zb3J0LnBpcGUnO1xyXG5pbXBvcnQgeyBDb25maWdQbHVnaW4sIE5HWFNfQ09ORklHX1BMVUdJTl9PUFRJT05TIH0gZnJvbSAnLi9wbHVnaW5zL2NvbmZpZy5wbHVnaW4nO1xyXG5pbXBvcnQgeyBMb2NhbGVQcm92aWRlciB9IGZyb20gJy4vcHJvdmlkZXJzL2xvY2FsZS5wcm92aWRlcic7XHJcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi9zdGF0ZXMvY29uZmlnLnN0YXRlJztcclxuaW1wb3J0IHsgUHJvZmlsZVN0YXRlIH0gZnJvbSAnLi9zdGF0ZXMvcHJvZmlsZS5zdGF0ZSc7XHJcbmltcG9ydCB7IFNlc3Npb25TdGF0ZSB9IGZyb20gJy4vc3RhdGVzL3Nlc3Npb24uc3RhdGUnO1xyXG5pbXBvcnQgeyBnZXRJbml0aWFsRGF0YSwgbG9jYWxlSW5pdGlhbGl6ZXIgfSBmcm9tICcuL3V0aWxzL2luaXRpYWwtdXRpbHMnO1xyXG5cclxuQE5nTW9kdWxlKHtcclxuICBpbXBvcnRzOiBbXHJcbiAgICBOZ3hzTW9kdWxlLmZvckZlYXR1cmUoW1Byb2ZpbGVTdGF0ZSwgU2Vzc2lvblN0YXRlLCBDb25maWdTdGF0ZV0pLFxyXG4gICAgTmd4c1JvdXRlclBsdWdpbk1vZHVsZS5mb3JSb290KCksXHJcbiAgICBOZ3hzU3RvcmFnZVBsdWdpbk1vZHVsZS5mb3JSb290KHsga2V5OiBbJ1Nlc3Npb25TdGF0ZSddIH0pLFxyXG4gICAgT0F1dGhNb2R1bGUuZm9yUm9vdCgpLFxyXG4gICAgQ29tbW9uTW9kdWxlLFxyXG4gICAgSHR0cENsaWVudE1vZHVsZSxcclxuICAgIEZvcm1zTW9kdWxlLFxyXG4gICAgUmVhY3RpdmVGb3Jtc01vZHVsZSxcclxuICAgIFJvdXRlck1vZHVsZSxcclxuICBdLFxyXG4gIGRlY2xhcmF0aW9uczogW1xyXG4gICAgUm91dGVyT3V0bGV0Q29tcG9uZW50LFxyXG4gICAgRHluYW1pY0xheW91dENvbXBvbmVudCxcclxuICAgIEF1dG9mb2N1c0RpcmVjdGl2ZSxcclxuICAgIEVsbGlwc2lzRGlyZWN0aXZlLFxyXG4gICAgRm9yRGlyZWN0aXZlLFxyXG4gICAgRm9ybVN1Ym1pdERpcmVjdGl2ZSxcclxuICAgIExvY2FsaXphdGlvblBpcGUsXHJcbiAgICBTb3J0UGlwZSxcclxuICAgIFBlcm1pc3Npb25EaXJlY3RpdmUsXHJcbiAgICBWaXNpYmlsaXR5RGlyZWN0aXZlLFxyXG4gICAgSW5wdXRFdmVudERlYm91bmNlRGlyZWN0aXZlLFxyXG4gICAgQ2xpY2tFdmVudFN0b3BQcm9wYWdhdGlvbkRpcmVjdGl2ZSxcclxuICAgIEFic3RyYWN0TmdNb2RlbENvbXBvbmVudCxcclxuICBdLFxyXG4gIGV4cG9ydHM6IFtcclxuICAgIENvbW1vbk1vZHVsZSxcclxuICAgIEh0dHBDbGllbnRNb2R1bGUsXHJcbiAgICBGb3Jtc01vZHVsZSxcclxuICAgIFJlYWN0aXZlRm9ybXNNb2R1bGUsXHJcbiAgICBSb3V0ZXJNb2R1bGUsXHJcbiAgICBSb3V0ZXJPdXRsZXRDb21wb25lbnQsXHJcbiAgICBEeW5hbWljTGF5b3V0Q29tcG9uZW50LFxyXG4gICAgQXV0b2ZvY3VzRGlyZWN0aXZlLFxyXG4gICAgRWxsaXBzaXNEaXJlY3RpdmUsXHJcbiAgICBGb3JEaXJlY3RpdmUsXHJcbiAgICBGb3JtU3VibWl0RGlyZWN0aXZlLFxyXG4gICAgTG9jYWxpemF0aW9uUGlwZSxcclxuICAgIFNvcnRQaXBlLFxyXG4gICAgUGVybWlzc2lvbkRpcmVjdGl2ZSxcclxuICAgIFZpc2liaWxpdHlEaXJlY3RpdmUsXHJcbiAgICBJbnB1dEV2ZW50RGVib3VuY2VEaXJlY3RpdmUsXHJcbiAgICBMb2NhbGl6YXRpb25QaXBlLFxyXG4gICAgQ2xpY2tFdmVudFN0b3BQcm9wYWdhdGlvbkRpcmVjdGl2ZSxcclxuICAgIEFic3RyYWN0TmdNb2RlbENvbXBvbmVudCxcclxuICBdLFxyXG4gIHByb3ZpZGVyczogW0xvY2FsaXphdGlvblBpcGVdLFxyXG4gIGVudHJ5Q29tcG9uZW50czogW1JvdXRlck91dGxldENvbXBvbmVudCwgRHluYW1pY0xheW91dENvbXBvbmVudF0sXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBDb3JlTW9kdWxlIHtcclxuICBzdGF0aWMgZm9yUm9vdChvcHRpb25zID0ge30gYXMgQUJQLlJvb3QpOiBNb2R1bGVXaXRoUHJvdmlkZXJzIHtcclxuICAgIHJldHVybiB7XHJcbiAgICAgIG5nTW9kdWxlOiBDb3JlTW9kdWxlLFxyXG4gICAgICBwcm92aWRlcnM6IFtcclxuICAgICAgICBMb2NhbGVQcm92aWRlcixcclxuICAgICAgICB7XHJcbiAgICAgICAgICBwcm92aWRlOiBOR1hTX1BMVUdJTlMsXHJcbiAgICAgICAgICB1c2VDbGFzczogQ29uZmlnUGx1Z2luLFxyXG4gICAgICAgICAgbXVsdGk6IHRydWUsXHJcbiAgICAgICAgfSxcclxuICAgICAgICB7XHJcbiAgICAgICAgICBwcm92aWRlOiBOR1hTX0NPTkZJR19QTFVHSU5fT1BUSU9OUyxcclxuICAgICAgICAgIHVzZVZhbHVlOiBvcHRpb25zLFxyXG4gICAgICAgIH0sXHJcbiAgICAgICAge1xyXG4gICAgICAgICAgcHJvdmlkZTogSFRUUF9JTlRFUkNFUFRPUlMsXHJcbiAgICAgICAgICB1c2VDbGFzczogQXBpSW50ZXJjZXB0b3IsXHJcbiAgICAgICAgICBtdWx0aTogdHJ1ZSxcclxuICAgICAgICB9LFxyXG4gICAgICAgIHtcclxuICAgICAgICAgIHByb3ZpZGU6IEFQUF9JTklUSUFMSVpFUixcclxuICAgICAgICAgIG11bHRpOiB0cnVlLFxyXG4gICAgICAgICAgZGVwczogW0luamVjdG9yXSxcclxuICAgICAgICAgIHVzZUZhY3Rvcnk6IGdldEluaXRpYWxEYXRhLFxyXG4gICAgICAgIH0sXHJcbiAgICAgICAge1xyXG4gICAgICAgICAgcHJvdmlkZTogQVBQX0lOSVRJQUxJWkVSLFxyXG4gICAgICAgICAgbXVsdGk6IHRydWUsXHJcbiAgICAgICAgICBkZXBzOiBbSW5qZWN0b3JdLFxyXG4gICAgICAgICAgdXNlRmFjdG9yeTogbG9jYWxlSW5pdGlhbGl6ZXIsXHJcbiAgICAgICAgfSxcclxuICAgICAgXSxcclxuICAgIH07XHJcbiAgfVxyXG59XHJcbiJdfQ==