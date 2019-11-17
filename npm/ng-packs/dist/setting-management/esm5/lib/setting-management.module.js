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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50Lm1vZHVsZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuc2V0dGluZy1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3NldHRpbmctbWFuYWdlbWVudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzFDLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDekMsT0FBTyxFQUFFLDhCQUE4QixFQUFFLE1BQU0scUNBQXFDLENBQUM7QUFDckYsT0FBTyxFQUFFLDBCQUEwQixFQUFFLE1BQU0sMkNBQTJDLENBQUM7QUFDdkYsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUN6QyxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSxtQ0FBbUMsQ0FBQztBQUUzRTtJQUFBO0lBU3NDLENBQUM7O2dCQVR0QyxRQUFRLFNBQUM7b0JBQ1IsWUFBWSxFQUFFLENBQUMsMEJBQTBCLENBQUM7b0JBQzFDLE9BQU8sRUFBRTt3QkFDUCw4QkFBOEI7d0JBQzlCLFVBQVU7d0JBQ1YsaUJBQWlCO3dCQUNqQixVQUFVLENBQUMsVUFBVSxDQUFDLENBQUMsc0JBQXNCLENBQUMsQ0FBQztxQkFDaEQ7aUJBQ0Y7O0lBQ3FDLDhCQUFDO0NBQUEsQUFUdkMsSUFTdUM7U0FBMUIsdUJBQXVCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29yZU1vZHVsZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcbmltcG9ydCB7IFRoZW1lU2hhcmVkTW9kdWxlIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xyXG5pbXBvcnQgeyBOZ01vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBTZXR0aW5nTWFuYWdlbWVudFJvdXRpbmdNb2R1bGUgfSBmcm9tICcuL3NldHRpbmctbWFuYWdlbWVudC1yb3V0aW5nLm1vZHVsZSc7XHJcbmltcG9ydCB7IFNldHRpbmdNYW5hZ2VtZW50Q29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3NldHRpbmctbWFuYWdlbWVudC5jb21wb25lbnQnO1xyXG5pbXBvcnQgeyBOZ3hzTW9kdWxlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBTZXR0aW5nTWFuYWdlbWVudFN0YXRlIH0gZnJvbSAnLi9zdGF0ZXMvc2V0dGluZy1tYW5hZ2VtZW50LnN0YXRlJztcclxuXHJcbkBOZ01vZHVsZSh7XHJcbiAgZGVjbGFyYXRpb25zOiBbU2V0dGluZ01hbmFnZW1lbnRDb21wb25lbnRdLFxyXG4gIGltcG9ydHM6IFtcclxuICAgIFNldHRpbmdNYW5hZ2VtZW50Um91dGluZ01vZHVsZSxcclxuICAgIENvcmVNb2R1bGUsXHJcbiAgICBUaGVtZVNoYXJlZE1vZHVsZSxcclxuICAgIE5neHNNb2R1bGUuZm9yRmVhdHVyZShbU2V0dGluZ01hbmFnZW1lbnRTdGF0ZV0pLFxyXG4gIF0sXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBTZXR0aW5nTWFuYWdlbWVudE1vZHVsZSB7fVxyXG4iXX0=