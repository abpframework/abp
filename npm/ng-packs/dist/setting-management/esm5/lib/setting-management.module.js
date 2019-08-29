/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CoreModule } from '@abp/ng.core';
import { NgModule, Self } from '@angular/core';
import { SettingComponent } from './components/setting/setting.component';
import { SettingManagementRoutingModule } from './setting-management-routing.module';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { InitialService } from './components/services/initial.service';
var SettingManagementModule = /** @class */ (function () {
    function SettingManagementModule(initialService) {
    }
    SettingManagementModule.decorators = [
        { type: NgModule, args: [{
                    declarations: [SettingComponent],
                    imports: [SettingManagementRoutingModule, CoreModule, ThemeSharedModule],
                    providers: [InitialService],
                },] }
    ];
    /** @nocollapse */
    SettingManagementModule.ctorParameters = function () { return [
        { type: InitialService, decorators: [{ type: Self }] }
    ]; };
    return SettingManagementModule;
}());
export { SettingManagementModule };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50Lm1vZHVsZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuc2V0dGluZy1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3NldHRpbmctbWFuYWdlbWVudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQVEsTUFBTSxjQUFjLENBQUM7QUFDaEQsT0FBTyxFQUFFLFFBQVEsRUFBd0MsSUFBSSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3JGLE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLHdDQUF3QyxDQUFDO0FBQzFFLE9BQU8sRUFBRSw4QkFBOEIsRUFBRSxNQUFNLHFDQUFxQyxDQUFDO0FBQ3JGLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSx1Q0FBdUMsQ0FBQztBQUV2RTtJQU1FLGlDQUFvQixjQUE4QjtJQUFHLENBQUM7O2dCQU52RCxRQUFRLFNBQUM7b0JBQ1IsWUFBWSxFQUFFLENBQUMsZ0JBQWdCLENBQUM7b0JBQ2hDLE9BQU8sRUFBRSxDQUFDLDhCQUE4QixFQUFFLFVBQVUsRUFBRSxpQkFBaUIsQ0FBQztvQkFDeEUsU0FBUyxFQUFFLENBQUMsY0FBYyxDQUFDO2lCQUM1Qjs7OztnQkFOUSxjQUFjLHVCQVFSLElBQUk7O0lBQ25CLDhCQUFDO0NBQUEsQUFQRCxJQU9DO1NBRlksdUJBQXVCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29yZU1vZHVsZSwgbm9vcCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBOZ01vZHVsZSwgTW9kdWxlV2l0aFByb3ZpZGVycywgQVBQX0lOSVRJQUxJWkVSLCBTZWxmIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTZXR0aW5nQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3NldHRpbmcvc2V0dGluZy5jb21wb25lbnQnO1xuaW1wb3J0IHsgU2V0dGluZ01hbmFnZW1lbnRSb3V0aW5nTW9kdWxlIH0gZnJvbSAnLi9zZXR0aW5nLW1hbmFnZW1lbnQtcm91dGluZy5tb2R1bGUnO1xuaW1wb3J0IHsgVGhlbWVTaGFyZWRNb2R1bGUgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBJbml0aWFsU2VydmljZSB9IGZyb20gJy4vY29tcG9uZW50cy9zZXJ2aWNlcy9pbml0aWFsLnNlcnZpY2UnO1xuXG5ATmdNb2R1bGUoe1xuICBkZWNsYXJhdGlvbnM6IFtTZXR0aW5nQ29tcG9uZW50XSxcbiAgaW1wb3J0czogW1NldHRpbmdNYW5hZ2VtZW50Um91dGluZ01vZHVsZSwgQ29yZU1vZHVsZSwgVGhlbWVTaGFyZWRNb2R1bGVdLFxuICBwcm92aWRlcnM6IFtJbml0aWFsU2VydmljZV0sXG59KVxuZXhwb3J0IGNsYXNzIFNldHRpbmdNYW5hZ2VtZW50TW9kdWxlIHtcbiAgY29uc3RydWN0b3IoQFNlbGYoKSBpbml0aWFsU2VydmljZTogSW5pdGlhbFNlcnZpY2UpIHt9XG59XG4iXX0=