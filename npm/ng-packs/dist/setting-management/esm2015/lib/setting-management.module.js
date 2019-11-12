/**
 * @fileoverview added by tsickle
 * Generated from: lib/setting-management.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { SettingManagementRoutingModule } from './setting-management-routing.module';
import { SettingManagementComponent } from './components/setting-management.component';
import { NgxsModule } from '@ngxs/store';
import { SettingManagementState } from './states/setting-management.state';
export class SettingManagementModule {
}
SettingManagementModule.decorators = [
    { type: NgModule, args: [{
                declarations: [SettingManagementComponent],
                imports: [
                    SettingManagementRoutingModule,
                    CoreModule,
                    ThemeSharedModule,
                    NgxsModule.forFeature([SettingManagementState]),
                ],
            },] }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50Lm1vZHVsZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuc2V0dGluZy1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3NldHRpbmctbWFuYWdlbWVudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzFDLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDekMsT0FBTyxFQUFFLDhCQUE4QixFQUFFLE1BQU0scUNBQXFDLENBQUM7QUFDckYsT0FBTyxFQUFFLDBCQUEwQixFQUFFLE1BQU0sMkNBQTJDLENBQUM7QUFDdkYsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUN6QyxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSxtQ0FBbUMsQ0FBQztBQVczRSxNQUFNLE9BQU8sdUJBQXVCOzs7WUFUbkMsUUFBUSxTQUFDO2dCQUNSLFlBQVksRUFBRSxDQUFDLDBCQUEwQixDQUFDO2dCQUMxQyxPQUFPLEVBQUU7b0JBQ1AsOEJBQThCO29CQUM5QixVQUFVO29CQUNWLGlCQUFpQjtvQkFDakIsVUFBVSxDQUFDLFVBQVUsQ0FBQyxDQUFDLHNCQUFzQixDQUFDLENBQUM7aUJBQ2hEO2FBQ0YiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb3JlTW9kdWxlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgVGhlbWVTaGFyZWRNb2R1bGUgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XHJcbmltcG9ydCB7IE5nTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFNldHRpbmdNYW5hZ2VtZW50Um91dGluZ01vZHVsZSB9IGZyb20gJy4vc2V0dGluZy1tYW5hZ2VtZW50LXJvdXRpbmcubW9kdWxlJztcclxuaW1wb3J0IHsgU2V0dGluZ01hbmFnZW1lbnRDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvc2V0dGluZy1tYW5hZ2VtZW50LmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IE5neHNNb2R1bGUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IFNldHRpbmdNYW5hZ2VtZW50U3RhdGUgfSBmcm9tICcuL3N0YXRlcy9zZXR0aW5nLW1hbmFnZW1lbnQuc3RhdGUnO1xyXG5cclxuQE5nTW9kdWxlKHtcclxuICBkZWNsYXJhdGlvbnM6IFtTZXR0aW5nTWFuYWdlbWVudENvbXBvbmVudF0sXHJcbiAgaW1wb3J0czogW1xyXG4gICAgU2V0dGluZ01hbmFnZW1lbnRSb3V0aW5nTW9kdWxlLFxyXG4gICAgQ29yZU1vZHVsZSxcclxuICAgIFRoZW1lU2hhcmVkTW9kdWxlLFxyXG4gICAgTmd4c01vZHVsZS5mb3JGZWF0dXJlKFtTZXR0aW5nTWFuYWdlbWVudFN0YXRlXSksXHJcbiAgXSxcclxufSlcclxuZXhwb3J0IGNsYXNzIFNldHRpbmdNYW5hZ2VtZW50TW9kdWxlIHt9XHJcbiJdfQ==