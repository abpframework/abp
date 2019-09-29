/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
export var TENANT_MANAGEMENT_ROUTES = {
    routes: (/** @type {?} */ ([
        {
            name: 'AbpTenantManagement::Menu:TenantManagement',
            path: 'tenant-management',
            parentName: 'AbpUiNavigation::Menu:Administration',
            layout: "application" /* application */,
            iconClass: 'fa fa-users',
            children: [
                {
                    path: 'tenants',
                    name: 'AbpTenantManagement::Tenants',
                    order: 1,
                    requiredPolicy: 'AbpTenantManagement.Tenants',
                },
            ],
        },
    ])),
};
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm91dGVzLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50ZW5hbnQtbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi9jb25zdGFudHMvcm91dGVzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBRUEsTUFBTSxLQUFPLHdCQUF3QixHQUFHO0lBQ3RDLE1BQU0sRUFBRSxtQkFBQTtRQUNOO1lBQ0UsSUFBSSxFQUFFLDRDQUE0QztZQUNsRCxJQUFJLEVBQUUsbUJBQW1CO1lBQ3pCLFVBQVUsRUFBRSxzQ0FBc0M7WUFDbEQsTUFBTSxpQ0FBeUI7WUFDL0IsU0FBUyxFQUFFLGFBQWE7WUFDeEIsUUFBUSxFQUFFO2dCQUNSO29CQUNFLElBQUksRUFBRSxTQUFTO29CQUNmLElBQUksRUFBRSw4QkFBOEI7b0JBQ3BDLEtBQUssRUFBRSxDQUFDO29CQUNSLGNBQWMsRUFBRSw2QkFBNkI7aUJBQzlDO2FBQ0Y7U0FDRjtLQUNGLEVBQW1CO0NBQ3JCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQUJQLCBlTGF5b3V0VHlwZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5cbmV4cG9ydCBjb25zdCBURU5BTlRfTUFOQUdFTUVOVF9ST1VURVMgPSB7XG4gIHJvdXRlczogW1xuICAgIHtcbiAgICAgIG5hbWU6ICdBYnBUZW5hbnRNYW5hZ2VtZW50OjpNZW51OlRlbmFudE1hbmFnZW1lbnQnLFxuICAgICAgcGF0aDogJ3RlbmFudC1tYW5hZ2VtZW50JyxcbiAgICAgIHBhcmVudE5hbWU6ICdBYnBVaU5hdmlnYXRpb246Ok1lbnU6QWRtaW5pc3RyYXRpb24nLFxuICAgICAgbGF5b3V0OiBlTGF5b3V0VHlwZS5hcHBsaWNhdGlvbixcbiAgICAgIGljb25DbGFzczogJ2ZhIGZhLXVzZXJzJyxcbiAgICAgIGNoaWxkcmVuOiBbXG4gICAgICAgIHtcbiAgICAgICAgICBwYXRoOiAndGVuYW50cycsXG4gICAgICAgICAgbmFtZTogJ0FicFRlbmFudE1hbmFnZW1lbnQ6OlRlbmFudHMnLFxuICAgICAgICAgIG9yZGVyOiAxLFxuICAgICAgICAgIHJlcXVpcmVkUG9saWN5OiAnQWJwVGVuYW50TWFuYWdlbWVudC5UZW5hbnRzJyxcbiAgICAgICAgfSxcbiAgICAgIF0sXG4gICAgfSxcbiAgXSBhcyBBQlAuRnVsbFJvdXRlW10sXG59O1xuIl19