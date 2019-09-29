/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Store } from '@ngxs/store';
import { SessionState } from '../states';
import { StartLoader, StopLoader } from '../actions/loader.actions';
import { finalize } from 'rxjs/operators';
export class ApiInterceptor {
    /**
     * @param {?} oAuthService
     * @param {?} store
     */
    constructor(oAuthService, store) {
        this.oAuthService = oAuthService;
        this.store = store;
    }
    /**
     * @param {?} request
     * @param {?} next
     * @return {?}
     */
    intercept(request, next) {
        this.store.dispatch(new StartLoader(request));
        /** @type {?} */
        const headers = (/** @type {?} */ ({}));
        /** @type {?} */
        const token = this.oAuthService.getAccessToken();
        if (!request.headers.has('Authorization') && token) {
            headers['Authorization'] = `Bearer ${token}`;
        }
        /** @type {?} */
        const lang = this.store.selectSnapshot(SessionState.getLanguage);
        if (!request.headers.has('Accept-Language') && lang) {
            headers['Accept-Language'] = lang;
        }
        /** @type {?} */
        const tenant = this.store.selectSnapshot(SessionState.getTenant);
        if (!request.headers.has('__tenant') && tenant) {
            headers['__tenant'] = tenant.id;
        }
        return next
            .handle(request.clone({
            setHeaders: headers,
        }))
            .pipe(finalize((/**
         * @return {?}
         */
        () => this.store.dispatch(new StopLoader(request)))));
    }
}
ApiInterceptor.decorators = [
    { type: Injectable }
];
/** @nocollapse */
ApiInterceptor.ctorParameters = () => [
    { type: OAuthService },
    { type: Store }
];
if (false) {
    /**
     * @type {?}
     * @private
     */
    ApiInterceptor.prototype.oAuthService;
    /**
     * @type {?}
     * @private
     */
    ApiInterceptor.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBpLmludGVyY2VwdG9yLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2ludGVyY2VwdG9ycy9hcGkuaW50ZXJjZXB0b3IudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFM0MsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQ25ELE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLFdBQVcsQ0FBQztBQUN6QyxPQUFPLEVBQUUsV0FBVyxFQUFFLFVBQVUsRUFBRSxNQUFNLDJCQUEyQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUcxQyxNQUFNLE9BQU8sY0FBYzs7Ozs7SUFDekIsWUFBb0IsWUFBMEIsRUFBVSxLQUFZO1FBQWhELGlCQUFZLEdBQVosWUFBWSxDQUFjO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7OztJQUV4RSxTQUFTLENBQUMsT0FBeUIsRUFBRSxJQUFpQjtRQUNwRCxJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLFdBQVcsQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDOztjQUV4QyxPQUFPLEdBQUcsbUJBQUEsRUFBRSxFQUFPOztjQUVuQixLQUFLLEdBQUcsSUFBSSxDQUFDLFlBQVksQ0FBQyxjQUFjLEVBQUU7UUFDaEQsSUFBSSxDQUFDLE9BQU8sQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLGVBQWUsQ0FBQyxJQUFJLEtBQUssRUFBRTtZQUNsRCxPQUFPLENBQUMsZUFBZSxDQUFDLEdBQUcsVUFBVSxLQUFLLEVBQUUsQ0FBQztTQUM5Qzs7Y0FFSyxJQUFJLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsWUFBWSxDQUFDLFdBQVcsQ0FBQztRQUNoRSxJQUFJLENBQUMsT0FBTyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsaUJBQWlCLENBQUMsSUFBSSxJQUFJLEVBQUU7WUFDbkQsT0FBTyxDQUFDLGlCQUFpQixDQUFDLEdBQUcsSUFBSSxDQUFDO1NBQ25DOztjQUVLLE1BQU0sR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxZQUFZLENBQUMsU0FBUyxDQUFDO1FBQ2hFLElBQUksQ0FBQyxPQUFPLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxVQUFVLENBQUMsSUFBSSxNQUFNLEVBQUU7WUFDOUMsT0FBTyxDQUFDLFVBQVUsQ0FBQyxHQUFHLE1BQU0sQ0FBQyxFQUFFLENBQUM7U0FDakM7UUFFRCxPQUFPLElBQUk7YUFDUixNQUFNLENBQ0wsT0FBTyxDQUFDLEtBQUssQ0FBQztZQUNaLFVBQVUsRUFBRSxPQUFPO1NBQ3BCLENBQUMsQ0FDSDthQUNBLElBQUksQ0FBQyxRQUFROzs7UUFBQyxHQUFHLEVBQUUsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLFVBQVUsQ0FBQyxPQUFPLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQztJQUN4RSxDQUFDOzs7WUEvQkYsVUFBVTs7OztZQU5GLFlBQVk7WUFDWixLQUFLOzs7Ozs7O0lBT0Esc0NBQWtDOzs7OztJQUFFLCtCQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEh0dHBJbnRlcmNlcHRvciwgSHR0cEhhbmRsZXIsIEh0dHBSZXF1ZXN0IH0gZnJvbSAnQGFuZ3VsYXIvY29tbW9uL2h0dHAnO1xuaW1wb3J0IHsgT0F1dGhTZXJ2aWNlIH0gZnJvbSAnYW5ndWxhci1vYXV0aDItb2lkYyc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IFNlc3Npb25TdGF0ZSB9IGZyb20gJy4uL3N0YXRlcyc7XG5pbXBvcnQgeyBTdGFydExvYWRlciwgU3RvcExvYWRlciB9IGZyb20gJy4uL2FjdGlvbnMvbG9hZGVyLmFjdGlvbnMnO1xuaW1wb3J0IHsgZmluYWxpemUgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5cbkBJbmplY3RhYmxlKClcbmV4cG9ydCBjbGFzcyBBcGlJbnRlcmNlcHRvciBpbXBsZW1lbnRzIEh0dHBJbnRlcmNlcHRvciB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgb0F1dGhTZXJ2aWNlOiBPQXV0aFNlcnZpY2UsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIGludGVyY2VwdChyZXF1ZXN0OiBIdHRwUmVxdWVzdDxhbnk+LCBuZXh0OiBIdHRwSGFuZGxlcikge1xuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFN0YXJ0TG9hZGVyKHJlcXVlc3QpKTtcblxuICAgIGNvbnN0IGhlYWRlcnMgPSB7fSBhcyBhbnk7XG5cbiAgICBjb25zdCB0b2tlbiA9IHRoaXMub0F1dGhTZXJ2aWNlLmdldEFjY2Vzc1Rva2VuKCk7XG4gICAgaWYgKCFyZXF1ZXN0LmhlYWRlcnMuaGFzKCdBdXRob3JpemF0aW9uJykgJiYgdG9rZW4pIHtcbiAgICAgIGhlYWRlcnNbJ0F1dGhvcml6YXRpb24nXSA9IGBCZWFyZXIgJHt0b2tlbn1gO1xuICAgIH1cblxuICAgIGNvbnN0IGxhbmcgPSB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFNlc3Npb25TdGF0ZS5nZXRMYW5ndWFnZSk7XG4gICAgaWYgKCFyZXF1ZXN0LmhlYWRlcnMuaGFzKCdBY2NlcHQtTGFuZ3VhZ2UnKSAmJiBsYW5nKSB7XG4gICAgICBoZWFkZXJzWydBY2NlcHQtTGFuZ3VhZ2UnXSA9IGxhbmc7XG4gICAgfVxuXG4gICAgY29uc3QgdGVuYW50ID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChTZXNzaW9uU3RhdGUuZ2V0VGVuYW50KTtcbiAgICBpZiAoIXJlcXVlc3QuaGVhZGVycy5oYXMoJ19fdGVuYW50JykgJiYgdGVuYW50KSB7XG4gICAgICBoZWFkZXJzWydfX3RlbmFudCddID0gdGVuYW50LmlkO1xuICAgIH1cblxuICAgIHJldHVybiBuZXh0XG4gICAgICAuaGFuZGxlKFxuICAgICAgICByZXF1ZXN0LmNsb25lKHtcbiAgICAgICAgICBzZXRIZWFkZXJzOiBoZWFkZXJzLFxuICAgICAgICB9KSxcbiAgICAgIClcbiAgICAgIC5waXBlKGZpbmFsaXplKCgpID0+IHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFN0b3BMb2FkZXIocmVxdWVzdCkpKSk7XG4gIH1cbn1cbiJdfQ==