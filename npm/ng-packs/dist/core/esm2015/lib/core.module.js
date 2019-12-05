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
import './utils/date-extensions';
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29yZS5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvY29yZS5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDL0MsT0FBTyxFQUFFLGdCQUFnQixFQUFFLGlCQUFpQixFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDM0UsT0FBTyxFQUFFLGVBQWUsRUFBRSxRQUFRLEVBQXVCLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN6RixPQUFPLEVBQUUsV0FBVyxFQUFFLG1CQUFtQixFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDbEUsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQy9DLE9BQU8sRUFBRSxzQkFBc0IsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQzdELE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQy9ELE9BQU8sRUFBRSxVQUFVLEVBQUUsWUFBWSxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3ZELE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUNsRCxPQUFPLEVBQUUsd0JBQXdCLEVBQUUsTUFBTSxnQ0FBZ0MsQ0FBQztBQUMxRSxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSx1Q0FBdUMsQ0FBQztBQUMvRSxPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSxzQ0FBc0MsQ0FBQztBQUM3RSxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSxrQ0FBa0MsQ0FBQztBQUN0RSxPQUFPLEVBQUUsMkJBQTJCLEVBQUUsTUFBTSxpQ0FBaUMsQ0FBQztBQUM5RSxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSxpQ0FBaUMsQ0FBQztBQUNwRSxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0sNEJBQTRCLENBQUM7QUFDMUQsT0FBTyxF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LGtCQUFrQjtvQkFDbEIsaUJBQWlCO29CQUNqQixZQUFZO29CQUNaLG1CQUFtQjtvQkFDbkIsZ0JBQWdCO29CQUNoQixRQUFRO29CQUNSLG1CQUFtQjtvQkFDbkIsbUJBQW1CO29CQUNuQiwyQkFBMkI7b0JBQzNCLGdCQUFnQjtvQkFDaEIsa0NBQWtDO29CQUNsQyx3QkFBd0I7aUJBQ3pCO2dCQUNELFNBQVMsRUFBRSxDQUFDLGdCQUFnQixDQUFDO2dCQUM3QixlQUFlLEVBQUUsQ0FBQyxxQkFBcUIsRUFBRSxzQkFBc0IsQ0FBQzthQUNqRSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbW1vbk1vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvbW1vbic7XG5pbXBvcnQgeyBIdHRwQ2xpZW50TW9kdWxlLCBIVFRQX0lOVEVSQ0VQVE9SUyB9IGZyb20gJ0Bhbmd1bGFyL2NvbW1vbi9odHRwJztcbmltcG9ydCB7IEFQUF9JTklUSUFMSVpFUiwgSW5qZWN0b3IsIE1vZHVsZVdpdGhQcm92aWRlcnMsIE5nTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBGb3Jtc01vZHVsZSwgUmVhY3RpdmVGb3Jtc01vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcbmltcG9ydCB7IFJvdXRlck1vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBOZ3hzUm91dGVyUGx1Z2luTW9kdWxlIH0gZnJvbSAnQG5neHMvcm91dGVyLXBsdWdpbic7XG5pbXBvcnQgeyBOZ3hzU3RvcmFnZVBsdWdpbk1vZHVsZSB9IGZyb20gJ0BuZ3hzL3N0b3JhZ2UtcGx1Z2luJztcbmltcG9ydCB7IE5neHNNb2R1bGUsIE5HWFNfUExVR0lOUyB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9BdXRoTW9kdWxlIH0gZnJvbSAnYW5ndWxhci1vYXV0aDItb2lkYyc7XG5pbXBvcnQgeyBBYnN0cmFjdE5nTW9kZWxDb21wb25lbnQgfSBmcm9tICcuL2Fic3RyYWN0cy9uZy1tb2RlbC5jb21wb25lbnQnO1xuaW1wb3J0IHsgRHluYW1pY0xheW91dENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9keW5hbWljLWxheW91dC5jb21wb25lbnQnO1xuaW1wb3J0IHsgUm91dGVyT3V0bGV0Q29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3JvdXRlci1vdXRsZXQuY29tcG9uZW50JztcbmltcG9ydCB7IEF1dG9mb2N1c0RpcmVjdGl2ZSB9IGZyb20gJy4vZGlyZWN0aXZlcy9hdXRvZm9jdXMuZGlyZWN0aXZlJztcbmltcG9ydCB7IElucHV0RXZlbnREZWJvdW5jZURpcmVjdGl2ZSB9IGZyb20gJy4vZGlyZWN0aXZlcy9kZWJvdW5jZS5kaXJlY3RpdmUnO1xuaW1wb3J0IHsgRWxsaXBzaXNEaXJlY3RpdmUgfSBmcm9tICcuL2RpcmVjdGl2ZXMvZWxsaXBzaXMuZGlyZWN0aXZlJztcbmltcG9ydCB7IEZvckRpcmVjdGl2ZSB9IGZyb20gJy4vZGlyZWN0aXZlcy9mb3IuZGlyZWN0aXZlJztcbmltcG9ydCB7IEZvcm1TdWJtaXREaXJlY3RpdmUgfSBmcm9tICcuL2RpcmVjdGl2ZXMvZm9ybS1zdWJtaXQuZGlyZWN0aXZlJztcbmltcG9ydCB7IFBlcm1pc3Npb25EaXJlY3RpdmUgfSBmcm9tICcuL2RpcmVjdGl2ZXMvcGVybWlzc2lvbi5kaXJlY3RpdmUnO1xuaW1wb3J0IHsgQ2xpY2tFdmVudFN0b3BQcm9wYWdhdGlvbkRpcmVjdGl2ZSB9IGZyb20gJy4vZGlyZWN0aXZlcy9zdG9wLXByb3BhZ2F0aW9uLmRpcmVjdGl2ZSc7XG5pbXBvcnQgeyBWaXNpYmlsaXR5RGlyZWN0aXZlIH0gZnJvbSAnLi9kaXJlY3RpdmVzL3Zpc2liaWxpdHkuZGlyZWN0aXZlJztcbmltcG9ydCB7IEFwaUludGVyY2VwdG9yIH0gZnJvbSAnLi9pbnRlcmNlcHRvcnMvYXBpLmludGVyY2VwdG9yJztcbmltcG9ydCB7IEFCUCB9IGZyb20gJy4vbW9kZWxzL2NvbW1vbic7XG5pbXBvcnQgeyBMb2NhbGl6YXRpb25QaXBlIH0gZnJvbSAnLi9waXBlcy9sb2NhbGl6YXRpb24ucGlwZSc7XG5pbXBvcnQgeyBTb3J0UGlwZSB9IGZyb20gJy4vcGlwZXMvc29ydC5waXBlJztcbmltcG9ydCB7IENvbmZpZ1BsdWdpbiwgTkdYU19DT05GSUdfUExVR0lOX09QVElPTlMgfSBmcm9tICcuL3BsdWdpbnMvY29uZmlnLnBsdWdpbic7XG5pbXBvcnQgeyBMb2NhbGVQcm92aWRlciB9IGZyb20gJy4vcHJvdmlkZXJzL2xvY2FsZS5wcm92aWRlcic7XG5pbXBvcnQgeyBDb25maWdTdGF0ZSB9IGZyb20gJy4vc3RhdGVzL2NvbmZpZy5zdGF0ZSc7XG5pbXBvcnQgeyBQcm9maWxlU3RhdGUgfSBmcm9tICcuL3N0YXRlcy9wcm9maWxlLnN0YXRlJztcbmltcG9ydCB7IFNlc3Npb25TdGF0ZSB9IGZyb20gJy4vc3RhdGVzL3Nlc3Npb24uc3RhdGUnO1xuaW1wb3J0IHsgZ2V0SW5pdGlhbERhdGEsIGxvY2FsZUluaXRpYWxpemVyIH0gZnJvbSAnLi91dGlscy9pbml0aWFsLXV0aWxzJztcbmltcG9ydCAnLi91dGlscy9kYXRlLWV4dGVuc2lvbnMnO1xuXG5ATmdNb2R1bGUoe1xuICBpbXBvcnRzOiBbXG4gICAgTmd4c01vZHVsZS5mb3JGZWF0dXJlKFtQcm9maWxlU3RhdGUsIFNlc3Npb25TdGF0ZSwgQ29uZmlnU3RhdGVdKSxcbiAgICBOZ3hzUm91dGVyUGx1Z2luTW9kdWxlLmZvclJvb3QoKSxcbiAgICBOZ3hzU3RvcmFnZVBsdWdpbk1vZHVsZS5mb3JSb290KHsga2V5OiBbJ1Nlc3Npb25TdGF0ZSddIH0pLFxuICAgIE9BdXRoTW9kdWxlLmZvclJvb3QoKSxcbiAgICBDb21tb25Nb2R1bGUsXG4gICAgSHR0cENsaWVudE1vZHVsZSxcbiAgICBGb3Jtc01vZHVsZSxcbiAgICBSZWFjdGl2ZUZvcm1zTW9kdWxlLFxuICAgIFJvdXRlck1vZHVsZSxcbiAgXSxcbiAgZGVjbGFyYXRpb25zOiBbXG4gICAgUm91dGVyT3V0bGV0Q29tcG9uZW50LFxuICAgIER5bmFtaWNMYXlvdXRDb21wb25lbnQsXG4gICAgQXV0b2ZvY3VzRGlyZWN0aXZlLFxuICAgIEVsbGlwc2lzRGlyZWN0aXZlLFxuICAgIEZvckRpcmVjdGl2ZSxcbiAgICBGb3JtU3VibWl0RGlyZWN0aXZlLFxuICAgIExvY2FsaXphdGlvblBpcGUsXG4gICAgU29ydFBpcGUsXG4gICAgUGVybWlzc2lvbkRpcmVjdGl2ZSxcbiAgICBWaXNpYmlsaXR5RGlyZWN0aXZlLFxuICAgIElucHV0RXZlbnREZWJvdW5jZURpcmVjdGl2ZSxcbiAgICBDbGlja0V2ZW50U3RvcFByb3BhZ2F0aW9uRGlyZWN0aXZlLFxuICAgIEFic3RyYWN0TmdNb2RlbENvbXBvbmVudCxcbiAgXSxcbiAgZXhwb3J0czogW1xuICAgIENvbW1vbk1vZHVsZSxcbiAgICBIdHRwQ2xpZW50TW9kdWxlLFxuICAgIEZvcm1zTW9kdWxlLFxuICAgIFJlYWN0aXZlRm9ybXNNb2R1bGUsXG4gICAgUm91dGVyTW9kdWxlLFxuICAgIFJvdXRlck91dGxldENvbXBvbmVudCxcbiAgICBEeW5hbWljTGF5b3V0Q29tcG9uZW50LFxuICAgIEF1dG9mb2N1c0RpcmVjdGl2ZSxcbiAgICBFbGxpcHNpc0RpcmVjdGl2ZSxcbiAgICBGb3JEaXJlY3RpdmUsXG4gICAgRm9ybVN1Ym1pdERpcmVjdGl2ZSxcbiAgICBMb2NhbGl6YXRpb25QaXBlLFxuICAgIFNvcnRQaXBlLFxuICAgIFBlcm1pc3Npb25EaXJlY3RpdmUsXG4gICAgVmlzaWJpbGl0eURpcmVjdGl2ZSxcbiAgICBJbnB1dEV2ZW50RGVib3VuY2VEaXJlY3RpdmUsXG4gICAgTG9jYWxpemF0aW9uUGlwZSxcbiAgICBDbGlja0V2ZW50U3RvcFByb3BhZ2F0aW9uRGlyZWN0aXZlLFxuICAgIEFic3RyYWN0TmdNb2RlbENvbXBvbmVudCxcbiAgXSxcbiAgcHJvdmlkZXJzOiBbTG9jYWxpemF0aW9uUGlwZV0sXG4gIGVudHJ5Q29tcG9uZW50czogW1JvdXRlck91dGxldENvbXBvbmVudCwgRHluYW1pY0xheW91dENvbXBvbmVudF0sXG59KVxuZXhwb3J0IGNsYXNzIENvcmVNb2R1bGUge1xuICBzdGF0aWMgZm9yUm9vdChvcHRpb25zID0ge30gYXMgQUJQLlJvb3QpOiBNb2R1bGVXaXRoUHJvdmlkZXJzIHtcbiAgICByZXR1cm4ge1xuICAgICAgbmdNb2R1bGU6IENvcmVNb2R1bGUsXG4gICAgICBwcm92aWRlcnM6IFtcbiAgICAgICAgTG9jYWxlUHJvdmlkZXIsXG4gICAgICAgIHtcbiAgICAgICAgICBwcm92aWRlOiBOR1hTX1BMVUdJTlMsXG4gICAgICAgICAgdXNlQ2xhc3M6IENvbmZpZ1BsdWdpbixcbiAgICAgICAgICBtdWx0aTogdHJ1ZSxcbiAgICAgICAgfSxcbiAgICAgICAge1xuICAgICAgICAgIHByb3ZpZGU6IE5HWFNfQ09ORklHX1BMVUdJTl9PUFRJT05TLFxuICAgICAgICAgIHVzZVZhbHVlOiBvcHRpb25zLFxuICAgICAgICB9LFxuICAgICAgICB7XG4gICAgICAgICAgcHJvdmlkZTogSFRUUF9JTlRFUkNFUFRPUlMsXG4gICAgICAgICAgdXNlQ2xhc3M6IEFwaUludGVyY2VwdG9yLFxuICAgICAgICAgIG11bHRpOiB0cnVlLFxuICAgICAgICB9LFxuICAgICAgICB7XG4gICAgICAgICAgcHJvdmlkZTogQVBQX0lOSVRJQUxJWkVSLFxuICAgICAgICAgIG11bHRpOiB0cnVlLFxuICAgICAgICAgIGRlcHM6IFtJbmplY3Rvcl0sXG4gICAgICAgICAgdXNlRmFjdG9yeTogZ2V0SW5pdGlhbERhdGEsXG4gICAgICAgIH0sXG4gICAgICAgIHtcbiAgICAgICAgICBwcm92aWRlOiBBUFBfSU5JVElBTElaRVIsXG4gICAgICAgICAgbXVsdGk6IHRydWUsXG4gICAgICAgICAgZGVwczogW0luamVjdG9yXSxcbiAgICAgICAgICB1c2VGYWN0b3J5OiBsb2NhbGVJbml0aWFsaXplcixcbiAgICAgICAgfSxcbiAgICAgIF0sXG4gICAgfTtcbiAgfVxufVxuIl19