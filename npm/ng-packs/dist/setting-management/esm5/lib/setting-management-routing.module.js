/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { DynamicLayoutComponent } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SETTING_MANAGEMENT_ROUTES } from './constants/routes';
import { SettingLayoutComponent } from './components/setting-layout.component';
var ɵ0 = { routes: SETTING_MANAGEMENT_ROUTES, settings: [] };
/** @type {?} */
var routes = [
    {
        path: 'setting-management',
        component: DynamicLayoutComponent,
        children: [{ path: '', component: SettingLayoutComponent }],
        data: ɵ0,
    },
];
var SettingManagementRoutingModule = /** @class */ (function () {
    function SettingManagementRoutingModule() {
    }
    SettingManagementRoutingModule.decorators = [
        { type: NgModule, args: [{
                    imports: [RouterModule.forChild(routes)],
                    exports: [RouterModule],
                },] }
    ];
    return SettingManagementRoutingModule;
}());
export { SettingManagementRoutingModule };
export { ɵ0 };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LXJvdXRpbmcubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5zZXR0aW5nLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc2V0dGluZy1tYW5hZ2VtZW50LXJvdXRpbmcubW9kdWxlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDdEQsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN6QyxPQUFPLEVBQUUsWUFBWSxFQUFVLE1BQU0saUJBQWlCLENBQUM7QUFDdkQsT0FBTyxFQUFFLHlCQUF5QixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFDL0QsT0FBTyxFQUFFLHNCQUFzQixFQUFFLE1BQU0sdUNBQXVDLENBQUM7U0FPckUsRUFBRSxNQUFNLEVBQUUseUJBQXlCLEVBQUUsUUFBUSxFQUFFLEVBQUUsRUFBRTs7SUFMdkQsTUFBTSxHQUFXO0lBQ3JCO1FBQ0UsSUFBSSxFQUFFLG9CQUFvQjtRQUMxQixTQUFTLEVBQUUsc0JBQXNCO1FBQ2pDLFFBQVEsRUFBRSxDQUFDLEVBQUUsSUFBSSxFQUFFLEVBQUUsRUFBRSxTQUFTLEVBQUUsc0JBQXNCLEVBQUUsQ0FBQztRQUMzRCxJQUFJLElBQXFEO0tBQzFEO0NBQ0Y7QUFFRDtJQUFBO0lBSTZDLENBQUM7O2dCQUo3QyxRQUFRLFNBQUM7b0JBQ1IsT0FBTyxFQUFFLENBQUMsWUFBWSxDQUFDLFFBQVEsQ0FBQyxNQUFNLENBQUMsQ0FBQztvQkFDeEMsT0FBTyxFQUFFLENBQUMsWUFBWSxDQUFDO2lCQUN4Qjs7SUFDNEMscUNBQUM7Q0FBQSxBQUo5QyxJQUk4QztTQUFqQyw4QkFBOEIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBEeW5hbWljTGF5b3V0Q29tcG9uZW50IH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IE5nTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBSb3V0ZXJNb2R1bGUsIFJvdXRlcyB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBTRVRUSU5HX01BTkFHRU1FTlRfUk9VVEVTIH0gZnJvbSAnLi9jb25zdGFudHMvcm91dGVzJztcbmltcG9ydCB7IFNldHRpbmdMYXlvdXRDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvc2V0dGluZy1sYXlvdXQuY29tcG9uZW50JztcblxuY29uc3Qgcm91dGVzOiBSb3V0ZXMgPSBbXG4gIHtcbiAgICBwYXRoOiAnc2V0dGluZy1tYW5hZ2VtZW50JyxcbiAgICBjb21wb25lbnQ6IER5bmFtaWNMYXlvdXRDb21wb25lbnQsXG4gICAgY2hpbGRyZW46IFt7IHBhdGg6ICcnLCBjb21wb25lbnQ6IFNldHRpbmdMYXlvdXRDb21wb25lbnQgfV0sXG4gICAgZGF0YTogeyByb3V0ZXM6IFNFVFRJTkdfTUFOQUdFTUVOVF9ST1VURVMsIHNldHRpbmdzOiBbXSB9LFxuICB9LFxuXTtcblxuQE5nTW9kdWxlKHtcbiAgaW1wb3J0czogW1JvdXRlck1vZHVsZS5mb3JDaGlsZChyb3V0ZXMpXSxcbiAgZXhwb3J0czogW1JvdXRlck1vZHVsZV0sXG59KVxuZXhwb3J0IGNsYXNzIFNldHRpbmdNYW5hZ2VtZW50Um91dGluZ01vZHVsZSB7fVxuIl19