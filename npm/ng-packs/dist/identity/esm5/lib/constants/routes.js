/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
export var IDENTITY_ROUTES = {
    routes: (/** @type {?} */ ([
        {
            name: 'AbpUiNavigation::Menu:Administration',
            path: '',
            order: 1,
            wrapper: true,
        },
        {
            name: 'AbpIdentity::Menu:IdentityManagement',
            path: 'identity',
            order: 1,
            parentName: 'AbpUiNavigation::Menu:Administration',
            layout: "application" /* application */,
            iconClass: 'fa fa-id-card-o',
            children: [
                { path: 'roles', name: 'AbpIdentity::Roles', order: 2, requiredPolicy: 'AbpIdentity.Roles' },
                { path: 'users', name: 'AbpIdentity::Users', order: 1, requiredPolicy: 'AbpIdentity.Users' },
            ],
        },
    ])),
};
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm91dGVzLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5pZGVudGl0eS8iLCJzb3VyY2VzIjpbImxpYi9jb25zdGFudHMvcm91dGVzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBRUEsTUFBTSxLQUFPLGVBQWUsR0FBRztJQUM3QixNQUFNLEVBQUUsbUJBQUE7UUFDTjtZQUNFLElBQUksRUFBRSxzQ0FBc0M7WUFDNUMsSUFBSSxFQUFFLEVBQUU7WUFDUixLQUFLLEVBQUUsQ0FBQztZQUNSLE9BQU8sRUFBRSxJQUFJO1NBQ2Q7UUFDRDtZQUNFLElBQUksRUFBRSxzQ0FBc0M7WUFDNUMsSUFBSSxFQUFFLFVBQVU7WUFDaEIsS0FBSyxFQUFFLENBQUM7WUFDUixVQUFVLEVBQUUsc0NBQXNDO1lBQ2xELE1BQU0saUNBQXlCO1lBQy9CLFNBQVMsRUFBRSxpQkFBaUI7WUFDNUIsUUFBUSxFQUFFO2dCQUNSLEVBQUUsSUFBSSxFQUFFLE9BQU8sRUFBRSxJQUFJLEVBQUUsb0JBQW9CLEVBQUUsS0FBSyxFQUFFLENBQUMsRUFBRSxjQUFjLEVBQUUsbUJBQW1CLEVBQUU7Z0JBQzVGLEVBQUUsSUFBSSxFQUFFLE9BQU8sRUFBRSxJQUFJLEVBQUUsb0JBQW9CLEVBQUUsS0FBSyxFQUFFLENBQUMsRUFBRSxjQUFjLEVBQUUsbUJBQW1CLEVBQUU7YUFDN0Y7U0FDRjtLQUNGLEVBQW1CO0NBQ3JCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgZUxheW91dFR5cGUsIEFCUCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5cbmV4cG9ydCBjb25zdCBJREVOVElUWV9ST1VURVMgPSB7XG4gIHJvdXRlczogW1xuICAgIHtcbiAgICAgIG5hbWU6ICdBYnBVaU5hdmlnYXRpb246Ok1lbnU6QWRtaW5pc3RyYXRpb24nLFxuICAgICAgcGF0aDogJycsXG4gICAgICBvcmRlcjogMSxcbiAgICAgIHdyYXBwZXI6IHRydWUsXG4gICAgfSxcbiAgICB7XG4gICAgICBuYW1lOiAnQWJwSWRlbnRpdHk6Ok1lbnU6SWRlbnRpdHlNYW5hZ2VtZW50JyxcbiAgICAgIHBhdGg6ICdpZGVudGl0eScsXG4gICAgICBvcmRlcjogMSxcbiAgICAgIHBhcmVudE5hbWU6ICdBYnBVaU5hdmlnYXRpb246Ok1lbnU6QWRtaW5pc3RyYXRpb24nLFxuICAgICAgbGF5b3V0OiBlTGF5b3V0VHlwZS5hcHBsaWNhdGlvbixcbiAgICAgIGljb25DbGFzczogJ2ZhIGZhLWlkLWNhcmQtbycsXG4gICAgICBjaGlsZHJlbjogW1xuICAgICAgICB7IHBhdGg6ICdyb2xlcycsIG5hbWU6ICdBYnBJZGVudGl0eTo6Um9sZXMnLCBvcmRlcjogMiwgcmVxdWlyZWRQb2xpY3k6ICdBYnBJZGVudGl0eS5Sb2xlcycgfSxcbiAgICAgICAgeyBwYXRoOiAndXNlcnMnLCBuYW1lOiAnQWJwSWRlbnRpdHk6OlVzZXJzJywgb3JkZXI6IDEsIHJlcXVpcmVkUG9saWN5OiAnQWJwSWRlbnRpdHkuVXNlcnMnIH0sXG4gICAgICBdLFxuICAgIH0sXG4gIF0gYXMgQUJQLkZ1bGxSb3V0ZVtdLFxufTtcbiJdfQ==