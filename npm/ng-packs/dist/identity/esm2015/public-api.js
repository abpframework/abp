/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/*
 * Public API Surface of identity
 */
export { IdentityModule } from './lib/identity.module';
export { IdentityGetRoles, IdentityGetRoleById, IdentityDeleteRole, IdentityAddRole, IdentityUpdateRole, IdentityGetUsers, IdentityGetUserById, IdentityDeleteUser, IdentityAddUser, IdentityUpdateUser, IdentityGetUserRoles } from './lib/actions/identity.actions';
export { RolesComponent } from './lib/components/roles/roles.component';
export { IDENTITY_ROUTES } from './lib/constants/routes';
export {} from './lib/models/identity';
export { RoleResolver } from './lib/resolvers/roles.resolver';
export { IdentityService } from './lib/services/identity.service';
export { IdentityState } from './lib/states/identity.state';
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHVibGljLWFwaS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuaWRlbnRpdHkvIiwic291cmNlcyI6WyJwdWJsaWMtYXBpLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7Ozs7QUFJQSwrQkFBYyx1QkFBdUIsQ0FBQztBQUN0QyxxT0FBYyxnQ0FBZ0MsQ0FBQztBQUMvQywrQkFBYyx3Q0FBd0MsQ0FBQztBQUN2RCxnQ0FBYyx3QkFBd0IsQ0FBQztBQUN2QyxlQUFjLHVCQUF1QixDQUFDO0FBQ3RDLDZCQUFjLGdDQUFnQyxDQUFDO0FBQy9DLGdDQUFjLGlDQUFpQyxDQUFDO0FBQ2hELDhCQUFjLDZCQUE2QixDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiLypcbiAqIFB1YmxpYyBBUEkgU3VyZmFjZSBvZiBpZGVudGl0eVxuICovXG5cbmV4cG9ydCAqIGZyb20gJy4vbGliL2lkZW50aXR5Lm1vZHVsZSc7XG5leHBvcnQgKiBmcm9tICcuL2xpYi9hY3Rpb25zL2lkZW50aXR5LmFjdGlvbnMnO1xuZXhwb3J0ICogZnJvbSAnLi9saWIvY29tcG9uZW50cy9yb2xlcy9yb2xlcy5jb21wb25lbnQnO1xuZXhwb3J0ICogZnJvbSAnLi9saWIvY29uc3RhbnRzL3JvdXRlcyc7XG5leHBvcnQgKiBmcm9tICcuL2xpYi9tb2RlbHMvaWRlbnRpdHknO1xuZXhwb3J0ICogZnJvbSAnLi9saWIvcmVzb2x2ZXJzL3JvbGVzLnJlc29sdmVyJztcbmV4cG9ydCAqIGZyb20gJy4vbGliL3NlcnZpY2VzL2lkZW50aXR5LnNlcnZpY2UnO1xuZXhwb3J0ICogZnJvbSAnLi9saWIvc3RhdGVzL2lkZW50aXR5LnN0YXRlJztcbiJdfQ==