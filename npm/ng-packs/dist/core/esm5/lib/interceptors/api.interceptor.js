/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Store } from '@ngxs/store';
import { SessionState } from '../states';
import { LoaderStart, LoaderStop } from '../actions/loader.actions';
import { finalize } from 'rxjs/operators';
var ApiInterceptor = /** @class */ (function () {
    function ApiInterceptor(oAuthService, store) {
        this.oAuthService = oAuthService;
        this.store = store;
    }
    /**
     * @param {?} request
     * @param {?} next
     * @return {?}
     */
    ApiInterceptor.prototype.intercept = /**
     * @param {?} request
     * @param {?} next
     * @return {?}
     */
    function (request, next) {
        var _this = this;
        this.store.dispatch(new LoaderStart(request));
        /** @type {?} */
        var headers = (/** @type {?} */ ({}));
        /** @type {?} */
        var token = this.oAuthService.getAccessToken();
        if (!request.headers.has('Authorization') && token) {
            headers['Authorization'] = "Bearer " + token;
        }
        /** @type {?} */
        var lang = this.store.selectSnapshot(SessionState.getLanguage);
        if (!request.headers.has('Accept-Language') && lang) {
            headers['Accept-Language'] = lang;
        }
        return next
            .handle(request.clone({
            setHeaders: headers,
        }))
            .pipe(finalize((/**
         * @return {?}
         */
        function () { return _this.store.dispatch(new LoaderStop(request)); })));
    };
    ApiInterceptor.decorators = [
        { type: Injectable }
    ];
    /** @nocollapse */
    ApiInterceptor.ctorParameters = function () { return [
        { type: OAuthService },
        { type: Store }
    ]; };
    return ApiInterceptor;
}());
export { ApiInterceptor };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBpLmludGVyY2VwdG9yLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2ludGVyY2VwdG9ycy9hcGkuaW50ZXJjZXB0b3IudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFM0MsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQ25ELE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLFdBQVcsQ0FBQztBQUN6QyxPQUFPLEVBQUUsV0FBVyxFQUFFLFVBQVUsRUFBRSxNQUFNLDJCQUEyQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUUxQztJQUVFLHdCQUFvQixZQUEwQixFQUFVLEtBQVk7UUFBaEQsaUJBQVksR0FBWixZQUFZLENBQWM7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO0lBQUcsQ0FBQzs7Ozs7O0lBRXhFLGtDQUFTOzs7OztJQUFULFVBQVUsT0FBeUIsRUFBRSxJQUFpQjtRQUF0RCxpQkFzQkM7UUFyQkMsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxXQUFXLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQzs7WUFFeEMsT0FBTyxHQUFHLG1CQUFBLEVBQUUsRUFBTzs7WUFFbkIsS0FBSyxHQUFHLElBQUksQ0FBQyxZQUFZLENBQUMsY0FBYyxFQUFFO1FBQ2hELElBQUksQ0FBQyxPQUFPLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxlQUFlLENBQUMsSUFBSSxLQUFLLEVBQUU7WUFDbEQsT0FBTyxDQUFDLGVBQWUsQ0FBQyxHQUFHLFlBQVUsS0FBTyxDQUFDO1NBQzlDOztZQUVLLElBQUksR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxZQUFZLENBQUMsV0FBVyxDQUFDO1FBQ2hFLElBQUksQ0FBQyxPQUFPLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxpQkFBaUIsQ0FBQyxJQUFJLElBQUksRUFBRTtZQUNuRCxPQUFPLENBQUMsaUJBQWlCLENBQUMsR0FBRyxJQUFJLENBQUM7U0FDbkM7UUFFRCxPQUFPLElBQUk7YUFDUixNQUFNLENBQ0wsT0FBTyxDQUFDLEtBQUssQ0FBQztZQUNaLFVBQVUsRUFBRSxPQUFPO1NBQ3BCLENBQUMsQ0FDSDthQUNBLElBQUksQ0FBQyxRQUFROzs7UUFBQyxjQUFNLE9BQUEsS0FBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxVQUFVLENBQUMsT0FBTyxDQUFDLENBQUMsRUFBNUMsQ0FBNEMsRUFBQyxDQUFDLENBQUM7SUFDeEUsQ0FBQzs7Z0JBMUJGLFVBQVU7Ozs7Z0JBTkYsWUFBWTtnQkFDWixLQUFLOztJQWdDZCxxQkFBQztDQUFBLEFBM0JELElBMkJDO1NBMUJZLGNBQWM7Ozs7OztJQUNiLHNDQUFrQzs7Ozs7SUFBRSwrQkFBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBIdHRwSW50ZXJjZXB0b3IsIEh0dHBIYW5kbGVyLCBIdHRwUmVxdWVzdCB9IGZyb20gJ0Bhbmd1bGFyL2NvbW1vbi9odHRwJztcbmltcG9ydCB7IE9BdXRoU2VydmljZSB9IGZyb20gJ2FuZ3VsYXItb2F1dGgyLW9pZGMnO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBTZXNzaW9uU3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMnO1xuaW1wb3J0IHsgTG9hZGVyU3RhcnQsIExvYWRlclN0b3AgfSBmcm9tICcuLi9hY3Rpb25zL2xvYWRlci5hY3Rpb25zJztcbmltcG9ydCB7IGZpbmFsaXplIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuXG5ASW5qZWN0YWJsZSgpXG5leHBvcnQgY2xhc3MgQXBpSW50ZXJjZXB0b3IgaW1wbGVtZW50cyBIdHRwSW50ZXJjZXB0b3Ige1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIG9BdXRoU2VydmljZTogT0F1dGhTZXJ2aWNlLCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cblxuICBpbnRlcmNlcHQocmVxdWVzdDogSHR0cFJlcXVlc3Q8YW55PiwgbmV4dDogSHR0cEhhbmRsZXIpIHtcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBMb2FkZXJTdGFydChyZXF1ZXN0KSk7XG5cbiAgICBjb25zdCBoZWFkZXJzID0ge30gYXMgYW55O1xuXG4gICAgY29uc3QgdG9rZW4gPSB0aGlzLm9BdXRoU2VydmljZS5nZXRBY2Nlc3NUb2tlbigpO1xuICAgIGlmICghcmVxdWVzdC5oZWFkZXJzLmhhcygnQXV0aG9yaXphdGlvbicpICYmIHRva2VuKSB7XG4gICAgICBoZWFkZXJzWydBdXRob3JpemF0aW9uJ10gPSBgQmVhcmVyICR7dG9rZW59YDtcbiAgICB9XG5cbiAgICBjb25zdCBsYW5nID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChTZXNzaW9uU3RhdGUuZ2V0TGFuZ3VhZ2UpO1xuICAgIGlmICghcmVxdWVzdC5oZWFkZXJzLmhhcygnQWNjZXB0LUxhbmd1YWdlJykgJiYgbGFuZykge1xuICAgICAgaGVhZGVyc1snQWNjZXB0LUxhbmd1YWdlJ10gPSBsYW5nO1xuICAgIH1cblxuICAgIHJldHVybiBuZXh0XG4gICAgICAuaGFuZGxlKFxuICAgICAgICByZXF1ZXN0LmNsb25lKHtcbiAgICAgICAgICBzZXRIZWFkZXJzOiBoZWFkZXJzLFxuICAgICAgICB9KSxcbiAgICAgIClcbiAgICAgIC5waXBlKGZpbmFsaXplKCgpID0+IHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IExvYWRlclN0b3AocmVxdWVzdCkpKSk7XG4gIH1cbn1cbiJdfQ==