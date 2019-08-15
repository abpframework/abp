/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
export var IDENTITY_ROUTES = (/** @type {?} */ ([
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
]));
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm91dGVzLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5pZGVudGl0eS8iLCJzb3VyY2VzIjpbImxpYi9jb25zdGFudHMvcm91dGVzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBRUEsTUFBTSxLQUFPLGVBQWUsR0FBRyxtQkFBQTtJQUM3QjtRQUNFLElBQUksRUFBRSxzQ0FBc0M7UUFDNUMsSUFBSSxFQUFFLEVBQUU7UUFDUixLQUFLLEVBQUUsQ0FBQztRQUNSLE9BQU8sRUFBRSxJQUFJO0tBQ2Q7SUFDRDtRQUNFLElBQUksRUFBRSxzQ0FBc0M7UUFDNUMsSUFBSSxFQUFFLFVBQVU7UUFDaEIsS0FBSyxFQUFFLENBQUM7UUFDUixVQUFVLEVBQUUsc0NBQXNDO1FBQ2xELE1BQU0saUNBQXlCO1FBQy9CLFNBQVMsRUFBRSxpQkFBaUI7UUFDNUIsUUFBUSxFQUFFO1lBQ1IsRUFBRSxJQUFJLEVBQUUsT0FBTyxFQUFFLElBQUksRUFBRSxvQkFBb0IsRUFBRSxLQUFLLEVBQUUsQ0FBQyxFQUFFLGNBQWMsRUFBRSxtQkFBbUIsRUFBRTtZQUM1RixFQUFFLElBQUksRUFBRSxPQUFPLEVBQUUsSUFBSSxFQUFFLG9CQUFvQixFQUFFLEtBQUssRUFBRSxDQUFDLEVBQUUsY0FBYyxFQUFFLG1CQUFtQixFQUFFO1NBQzdGO0tBQ0Y7Q0FDRixFQUFtQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IGVMYXlvdXRUeXBlLCBBQlAgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuXG5leHBvcnQgY29uc3QgSURFTlRJVFlfUk9VVEVTID0gW1xuICB7XG4gICAgbmFtZTogJ0FicFVpTmF2aWdhdGlvbjo6TWVudTpBZG1pbmlzdHJhdGlvbicsXG4gICAgcGF0aDogJycsXG4gICAgb3JkZXI6IDEsXG4gICAgd3JhcHBlcjogdHJ1ZSxcbiAgfSxcbiAge1xuICAgIG5hbWU6ICdBYnBJZGVudGl0eTo6TWVudTpJZGVudGl0eU1hbmFnZW1lbnQnLFxuICAgIHBhdGg6ICdpZGVudGl0eScsXG4gICAgb3JkZXI6IDEsXG4gICAgcGFyZW50TmFtZTogJ0FicFVpTmF2aWdhdGlvbjo6TWVudTpBZG1pbmlzdHJhdGlvbicsXG4gICAgbGF5b3V0OiBlTGF5b3V0VHlwZS5hcHBsaWNhdGlvbixcbiAgICBpY29uQ2xhc3M6ICdmYSBmYS1pZC1jYXJkLW8nLFxuICAgIGNoaWxkcmVuOiBbXG4gICAgICB7IHBhdGg6ICdyb2xlcycsIG5hbWU6ICdBYnBJZGVudGl0eTo6Um9sZXMnLCBvcmRlcjogMiwgcmVxdWlyZWRQb2xpY3k6ICdBYnBJZGVudGl0eS5Sb2xlcycgfSxcbiAgICAgIHsgcGF0aDogJ3VzZXJzJywgbmFtZTogJ0FicElkZW50aXR5OjpVc2VycycsIG9yZGVyOiAxLCByZXF1aXJlZFBvbGljeTogJ0FicElkZW50aXR5LlVzZXJzJyB9LFxuICAgIF0sXG4gIH0sXG5dIGFzIEFCUC5GdWxsUm91dGVbXTtcbiJdfQ==