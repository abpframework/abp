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
var SettingManagementModule = /** @class */ (function () {
    function SettingManagementModule() {
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
    return SettingManagementModule;
}());
export { SettingManagementModule };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50Lm1vZHVsZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuc2V0dGluZy1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3NldHRpbmctbWFuYWdlbWVudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzFDLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDekMsT0FBTyxFQUFFLDhCQUE4QixFQUFFLE1BQU0scUNBQXFDLENBQUM7QUFDckYsT0FBTyxFQUFFLDBCQUEwQixFQUFFLE1BQU0sMkNBQTJDLENBQUM7QUFDdkYsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUN6QyxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSxtQ0FBbUMsQ0FBQztBQUUzRTtJQUFBO0lBU3NDLENBQUM7O2dCQVR0QyxRQUFRLFNBQUM7b0JBQ1IsWUFBWSxFQUFFLENBQUMsMEJBQTBCLENBQUM7b0JBQzFDLE9BQU8sRUFBRTt3QkFDUCw4QkFBOEI7d0JBQzlCLFVBQVU7d0JBQ1YsaUJBQWlCO3dCQUNqQixVQUFVLENBQUMsVUFBVSxDQUFDLENBQUMsc0JBQXNCLENBQUMsQ0FBQztxQkFDaEQ7aUJBQ0Y7O0lBQ3FDLDhCQUFDO0NBQUEsQUFUdkMsSUFTdUM7U0FBMUIsdUJBQXVCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29yZU1vZHVsZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBUaGVtZVNoYXJlZE1vZHVsZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IE5nTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTZXR0aW5nTWFuYWdlbWVudFJvdXRpbmdNb2R1bGUgfSBmcm9tICcuL3NldHRpbmctbWFuYWdlbWVudC1yb3V0aW5nLm1vZHVsZSc7XG5pbXBvcnQgeyBTZXR0aW5nTWFuYWdlbWVudENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9zZXR0aW5nLW1hbmFnZW1lbnQuY29tcG9uZW50JztcbmltcG9ydCB7IE5neHNNb2R1bGUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBTZXR0aW5nTWFuYWdlbWVudFN0YXRlIH0gZnJvbSAnLi9zdGF0ZXMvc2V0dGluZy1tYW5hZ2VtZW50LnN0YXRlJztcblxuQE5nTW9kdWxlKHtcbiAgZGVjbGFyYXRpb25zOiBbU2V0dGluZ01hbmFnZW1lbnRDb21wb25lbnRdLFxuICBpbXBvcnRzOiBbXG4gICAgU2V0dGluZ01hbmFnZW1lbnRSb3V0aW5nTW9kdWxlLFxuICAgIENvcmVNb2R1bGUsXG4gICAgVGhlbWVTaGFyZWRNb2R1bGUsXG4gICAgTmd4c01vZHVsZS5mb3JGZWF0dXJlKFtTZXR0aW5nTWFuYWdlbWVudFN0YXRlXSksXG4gIF0sXG59KVxuZXhwb3J0IGNsYXNzIFNldHRpbmdNYW5hZ2VtZW50TW9kdWxlIHt9XG4iXX0=