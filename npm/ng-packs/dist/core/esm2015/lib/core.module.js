/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { APP_INITIALIZER, Injector, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgxsRouterPluginModule } from '@ngxs/router-plugin';
import { NgxsStoragePluginModule } from '@ngxs/storage-plugin';
import { NgxsModule, NGXS_PLUGINS } from '@ngxs/store';
import { DynamicLayoutComponent } from './components/dynamic-layout.component';
import { RouterOutletComponent } from './components/router-outlet.component';
import { PermissionDirective } from './directives/permission.directive';
import { VisibilityDirective } from './directives/visibility.directive';
import { ApiInterceptor } from './interceptors/api.interceptor';
import { LocalizationPipe } from './pipes/localization.pipe';
import { ConfigPlugin, NGXS_CONFIG_PLUGIN_OPTIONS } from './plugins/config.plugin';
import { ConfigState } from './states/config.state';
import { ProfileState } from './states/profile.state';
import { SessionState } from './states/session.state';
import { getInitialData } from './utils/initial-utils';
import { EllipsisDirective } from './directives/ellipsis.directive';
import { AutofocusDirective } from './directives/autofocus.directive';
import { InputEventDebounceDirective } from './directives/debounce.directive';
import { ClickEventStopPropagationDirective } from './directives/stop-propagation.directive';
export class CoreModule {
    /**
     * @param {?=} options
     * @return {?}
     */
    static forRoot(options = (/** @type {?} */ ({}))) {
        return {
            ngModule: CoreModule,
            providers: [
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
            ],
        };
    }
}
CoreModule.decorators = [
    { type: NgModule, args: [{
                imports: [
                    NgxsModule.forFeature([ProfileState, SessionState, ConfigState]),
                    NgxsStoragePluginModule.forRoot({ key: 'SessionState' }),
                    NgxsRouterPluginModule.forRoot(),
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
                    LocalizationPipe,
                    PermissionDirective,
                    VisibilityDirective,
                    InputEventDebounceDirective,
                    ClickEventStopPropagationDirective,
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
                    LocalizationPipe,
                    PermissionDirective,
                    VisibilityDirective,
                    InputEventDebounceDirective,
                    LocalizationPipe,
                    ClickEventStopPropagationDirective,
                ],
                providers: [LocalizationPipe],
                entryComponents: [RouterOutletComponent, DynamicLayoutComponent],
            },] }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29yZS5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvY29yZS5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUMvQyxPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUMzRSxPQUFPLEVBQUUsZUFBZSxFQUFFLFFBQVEsRUFBdUIsUUFBUSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3pGLE9BQU8sRUFBRSxXQUFXLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNsRSxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDL0MsT0FBTyxFQUFFLHNCQUFzQixFQUFFLE1BQU0scUJBQXFCLENBQUM7QUFDN0QsT0FBTyxFQUFFLHVCQUF1QixFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDL0QsT0FBTyxFQUFFLFVBQVUsRUFBRSxZQUFZLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDdkQsT0FBTyxFQUFFLHNCQUFzQixFQUFFLE1BQU0sdUNBQXVDLENBQUM7QUFDL0UsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sc0NBQXNDLENBQUM7QUFDN0UsT0FBTyxFQUFFLG1CQUFtQixFQUFFLE1BQU0sbUNBQW1DLENBQUM7QUFDeEUsT0FBTyxFQUFFLG1CQUFtQixFQUFFLE1BQU0sbUNBQW1DLENBQUM7QUFDeEUsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLGdDQUFnQyxDQUFDO0FBRWhFLE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLDJCQUEyQixDQUFDO0FBQzdELE9BQU8sRUFBRSxZQUFZLEVBQUUsMEJBQTBCLEVBQUUsTUFBTSx5QkFBeUIsQ0FBQztBQUNuRixPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sdUJBQXVCLENBQUM7QUFDcEQsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLHdCQUF3QixDQUFDO0FBQ3RELE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSx3QkFBd0IsQ0FBQztBQUN0RCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sdUJBQXVCLENBQUM7QUFDdkQsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0saUNBQWlDLENBQUM7QUFDcEUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0sa0NBQWtDLENBQUM7QUFDdEUsT0FBTyxFQUFFLDJCQUEyQixFQUFFLE1BQU0saUNBQWlDLENBQUM7QUFDOUUsT0FBTyxFQUFFLGtDQUFrQyxFQUFFLE1BQU0seUNBQXlDLENBQUM7QUE0QzdGLE1BQU0sT0FBTyxVQUFVOzs7OztJQUNyQixNQUFNLENBQUMsT0FBTyxDQUFDLE9BQU8sR0FBRyxtQkFBQSxFQUFFLEVBQVk7UUFDckMsT0FBTztZQUNMLFFBQVEsRUFBRSxVQUFVO1lBQ3BCLFNBQVMsRUFBRTtnQkFDVDtvQkFDRSxPQUFPLEVBQUUsWUFBWTtvQkFDckIsUUFBUSxFQUFFLFlBQVk7b0JBQ3RCLEtBQUssRUFBRSxJQUFJO2lCQUNaO2dCQUNEO29CQUNFLE9BQU8sRUFBRSwwQkFBMEI7b0JBQ25DLFFBQVEsRUFBRSxPQUFPO2lCQUNsQjtnQkFDRDtvQkFDRSxPQUFPLEVBQUUsaUJBQWlCO29CQUMxQixRQUFRLEVBQUUsY0FBYztvQkFDeEIsS0FBSyxFQUFFLElBQUk7aUJBQ1o7Z0JBQ0Q7b0JBQ0UsT0FBTyxFQUFFLGVBQWU7b0JBQ3hCLEtBQUssRUFBRSxJQUFJO29CQUNYLElBQUksRUFBRSxDQUFDLFFBQVEsQ0FBQztvQkFDaEIsVUFBVSxFQUFFLGNBQWM7aUJBQzNCO2FBQ0Y7U0FDRixDQUFDO0lBQ0osQ0FBQzs7O1lBckVGLFFBQVEsU0FBQztnQkFDUixPQUFPLEVBQUU7b0JBQ1AsVUFBVSxDQUFDLFVBQVUsQ0FBQyxDQUFDLFlBQVksRUFBRSxZQUFZLEVBQUUsV0FBVyxDQUFDLENBQUM7b0JBQ2hFLHVCQUF1QixDQUFDLE9BQU8sQ0FBQyxFQUFFLEdBQUcsRUFBRSxjQUFjLEVBQUUsQ0FBQztvQkFDeEQsc0JBQXNCLENBQUMsT0FBTyxFQUFFO29CQUNoQyxZQUFZO29CQUNaLGdCQUFnQjtvQkFDaEIsV0FBVztvQkFDWCxtQkFBbUI7b0JBQ25CLFlBQVk7aUJBQ2I7Z0JBQ0QsWUFBWSxFQUFFO29CQUNaLHFCQUFxQjtvQkFDckIsc0JBQXNCO29CQUN0QixrQkFBa0I7b0JBQ2xCLGlCQUFpQjtvQkFDakIsZ0JBQWdCO29CQUNoQixtQkFBbUI7b0JBQ25CLG1CQUFtQjtvQkFDbkIsMkJBQTJCO29CQUMzQixrQ0FBa0M7aUJBQ25DO2dCQUNELE9BQU8sRUFBRTtvQkFDUCxZQUFZO29CQUNaLGdCQUFnQjtvQkFDaEIsV0FBVztvQkFDWCxtQkFBbUI7b0JBQ25CLFlBQVk7b0JBQ1oscUJBQXFCO29CQUNyQixzQkFBc0I7b0JBQ3RCLGtCQUFrQjtvQkFDbEIsaUJBQWlCO29CQUNqQixnQkFBZ0I7b0JBQ2hCLG1CQUFtQjtvQkFDbkIsbUJBQW1CO29CQUNuQiwyQkFBMkI7b0JBQzNCLGdCQUFnQjtvQkFDaEIsa0NBQWtDO2lCQUNuQztnQkFDRCxTQUFTLEVBQUUsQ0FBQyxnQkFBZ0IsQ0FBQztnQkFDN0IsZUFBZSxFQUFFLENBQUMscUJBQXFCLEVBQUUsc0JBQXNCLENBQUM7YUFDakUiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21tb25Nb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb21tb24nO1xuaW1wb3J0IHsgSHR0cENsaWVudE1vZHVsZSwgSFRUUF9JTlRFUkNFUFRPUlMgfSBmcm9tICdAYW5ndWxhci9jb21tb24vaHR0cCc7XG5pbXBvcnQgeyBBUFBfSU5JVElBTElaRVIsIEluamVjdG9yLCBNb2R1bGVXaXRoUHJvdmlkZXJzLCBOZ01vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgRm9ybXNNb2R1bGUsIFJlYWN0aXZlRm9ybXNNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9mb3Jtcyc7XG5pbXBvcnQgeyBSb3V0ZXJNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgTmd4c1JvdXRlclBsdWdpbk1vZHVsZSB9IGZyb20gJ0BuZ3hzL3JvdXRlci1wbHVnaW4nO1xuaW1wb3J0IHsgTmd4c1N0b3JhZ2VQbHVnaW5Nb2R1bGUgfSBmcm9tICdAbmd4cy9zdG9yYWdlLXBsdWdpbic7XG5pbXBvcnQgeyBOZ3hzTW9kdWxlLCBOR1hTX1BMVUdJTlMgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBEeW5hbWljTGF5b3V0Q29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2R5bmFtaWMtbGF5b3V0LmNvbXBvbmVudCc7XG5pbXBvcnQgeyBSb3V0ZXJPdXRsZXRDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvcm91dGVyLW91dGxldC5jb21wb25lbnQnO1xuaW1wb3J0IHsgUGVybWlzc2lvbkRpcmVjdGl2ZSB9IGZyb20gJy4vZGlyZWN0aXZlcy9wZXJtaXNzaW9uLmRpcmVjdGl2ZSc7XG5pbXBvcnQgeyBWaXNpYmlsaXR5RGlyZWN0aXZlIH0gZnJvbSAnLi9kaXJlY3RpdmVzL3Zpc2liaWxpdHkuZGlyZWN0aXZlJztcbmltcG9ydCB7IEFwaUludGVyY2VwdG9yIH0gZnJvbSAnLi9pbnRlcmNlcHRvcnMvYXBpLmludGVyY2VwdG9yJztcbmltcG9ydCB7IEFCUCB9IGZyb20gJy4vbW9kZWxzL2NvbW1vbic7XG5pbXBvcnQgeyBMb2NhbGl6YXRpb25QaXBlIH0gZnJvbSAnLi9waXBlcy9sb2NhbGl6YXRpb24ucGlwZSc7XG5pbXBvcnQgeyBDb25maWdQbHVnaW4sIE5HWFNfQ09ORklHX1BMVUdJTl9PUFRJT05TIH0gZnJvbSAnLi9wbHVnaW5zL2NvbmZpZy5wbHVnaW4nO1xuaW1wb3J0IHsgQ29uZmlnU3RhdGUgfSBmcm9tICcuL3N0YXRlcy9jb25maWcuc3RhdGUnO1xuaW1wb3J0IHsgUHJvZmlsZVN0YXRlIH0gZnJvbSAnLi9zdGF0ZXMvcHJvZmlsZS5zdGF0ZSc7XG5pbXBvcnQgeyBTZXNzaW9uU3RhdGUgfSBmcm9tICcuL3N0YXRlcy9zZXNzaW9uLnN0YXRlJztcbmltcG9ydCB7IGdldEluaXRpYWxEYXRhIH0gZnJvbSAnLi91dGlscy9pbml0aWFsLXV0aWxzJztcbmltcG9ydCB7IEVsbGlwc2lzRGlyZWN0aXZlIH0gZnJvbSAnLi9kaXJlY3RpdmVzL2VsbGlwc2lzLmRpcmVjdGl2ZSc7XG5pbXBvcnQgeyBBdXRvZm9jdXNEaXJlY3RpdmUgfSBmcm9tICcuL2RpcmVjdGl2ZXMvYXV0b2ZvY3VzLmRpcmVjdGl2ZSc7XG5pbXBvcnQgeyBJbnB1dEV2ZW50RGVib3VuY2VEaXJlY3RpdmUgfSBmcm9tICcuL2RpcmVjdGl2ZXMvZGVib3VuY2UuZGlyZWN0aXZlJztcbmltcG9ydCB7IENsaWNrRXZlbnRTdG9wUHJvcGFnYXRpb25EaXJlY3RpdmUgfSBmcm9tICcuL2RpcmVjdGl2ZXMvc3RvcC1wcm9wYWdhdGlvbi5kaXJlY3RpdmUnO1xuXG5ATmdNb2R1bGUoe1xuICBpbXBvcnRzOiBbXG4gICAgTmd4c01vZHVsZS5mb3JGZWF0dXJlKFtQcm9maWxlU3RhdGUsIFNlc3Npb25TdGF0ZSwgQ29uZmlnU3RhdGVdKSxcbiAgICBOZ3hzU3RvcmFnZVBsdWdpbk1vZHVsZS5mb3JSb290KHsga2V5OiAnU2Vzc2lvblN0YXRlJyB9KSxcbiAgICBOZ3hzUm91dGVyUGx1Z2luTW9kdWxlLmZvclJvb3QoKSxcbiAgICBDb21tb25Nb2R1bGUsXG4gICAgSHR0cENsaWVudE1vZHVsZSxcbiAgICBGb3Jtc01vZHVsZSxcbiAgICBSZWFjdGl2ZUZvcm1zTW9kdWxlLFxuICAgIFJvdXRlck1vZHVsZSxcbiAgXSxcbiAgZGVjbGFyYXRpb25zOiBbXG4gICAgUm91dGVyT3V0bGV0Q29tcG9uZW50LFxuICAgIER5bmFtaWNMYXlvdXRDb21wb25lbnQsXG4gICAgQXV0b2ZvY3VzRGlyZWN0aXZlLFxuICAgIEVsbGlwc2lzRGlyZWN0aXZlLFxuICAgIExvY2FsaXphdGlvblBpcGUsXG4gICAgUGVybWlzc2lvbkRpcmVjdGl2ZSxcbiAgICBWaXNpYmlsaXR5RGlyZWN0aXZlLFxuICAgIElucHV0RXZlbnREZWJvdW5jZURpcmVjdGl2ZSxcbiAgICBDbGlja0V2ZW50U3RvcFByb3BhZ2F0aW9uRGlyZWN0aXZlLFxuICBdLFxuICBleHBvcnRzOiBbXG4gICAgQ29tbW9uTW9kdWxlLFxuICAgIEh0dHBDbGllbnRNb2R1bGUsXG4gICAgRm9ybXNNb2R1bGUsXG4gICAgUmVhY3RpdmVGb3Jtc01vZHVsZSxcbiAgICBSb3V0ZXJNb2R1bGUsXG4gICAgUm91dGVyT3V0bGV0Q29tcG9uZW50LFxuICAgIER5bmFtaWNMYXlvdXRDb21wb25lbnQsXG4gICAgQXV0b2ZvY3VzRGlyZWN0aXZlLFxuICAgIEVsbGlwc2lzRGlyZWN0aXZlLFxuICAgIExvY2FsaXphdGlvblBpcGUsXG4gICAgUGVybWlzc2lvbkRpcmVjdGl2ZSxcbiAgICBWaXNpYmlsaXR5RGlyZWN0aXZlLFxuICAgIElucHV0RXZlbnREZWJvdW5jZURpcmVjdGl2ZSxcbiAgICBMb2NhbGl6YXRpb25QaXBlLFxuICAgIENsaWNrRXZlbnRTdG9wUHJvcGFnYXRpb25EaXJlY3RpdmUsXG4gIF0sXG4gIHByb3ZpZGVyczogW0xvY2FsaXphdGlvblBpcGVdLFxuICBlbnRyeUNvbXBvbmVudHM6IFtSb3V0ZXJPdXRsZXRDb21wb25lbnQsIER5bmFtaWNMYXlvdXRDb21wb25lbnRdLFxufSlcbmV4cG9ydCBjbGFzcyBDb3JlTW9kdWxlIHtcbiAgc3RhdGljIGZvclJvb3Qob3B0aW9ucyA9IHt9IGFzIEFCUC5Sb290KTogTW9kdWxlV2l0aFByb3ZpZGVycyB7XG4gICAgcmV0dXJuIHtcbiAgICAgIG5nTW9kdWxlOiBDb3JlTW9kdWxlLFxuICAgICAgcHJvdmlkZXJzOiBbXG4gICAgICAgIHtcbiAgICAgICAgICBwcm92aWRlOiBOR1hTX1BMVUdJTlMsXG4gICAgICAgICAgdXNlQ2xhc3M6IENvbmZpZ1BsdWdpbixcbiAgICAgICAgICBtdWx0aTogdHJ1ZSxcbiAgICAgICAgfSxcbiAgICAgICAge1xuICAgICAgICAgIHByb3ZpZGU6IE5HWFNfQ09ORklHX1BMVUdJTl9PUFRJT05TLFxuICAgICAgICAgIHVzZVZhbHVlOiBvcHRpb25zLFxuICAgICAgICB9LFxuICAgICAgICB7XG4gICAgICAgICAgcHJvdmlkZTogSFRUUF9JTlRFUkNFUFRPUlMsXG4gICAgICAgICAgdXNlQ2xhc3M6IEFwaUludGVyY2VwdG9yLFxuICAgICAgICAgIG11bHRpOiB0cnVlLFxuICAgICAgICB9LFxuICAgICAgICB7XG4gICAgICAgICAgcHJvdmlkZTogQVBQX0lOSVRJQUxJWkVSLFxuICAgICAgICAgIG11bHRpOiB0cnVlLFxuICAgICAgICAgIGRlcHM6IFtJbmplY3Rvcl0sXG4gICAgICAgICAgdXNlRmFjdG9yeTogZ2V0SW5pdGlhbERhdGEsXG4gICAgICAgIH0sXG4gICAgICBdLFxuICAgIH07XG4gIH1cbn1cbiJdfQ==