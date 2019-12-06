/**
 * @fileoverview added by tsickle
 * Generated from: lib/interceptors/api.interceptor.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBpLmludGVyY2VwdG9yLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2ludGVyY2VwdG9ycy9hcGkuaW50ZXJjZXB0b3IudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBRTNDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUNuRCxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxXQUFXLENBQUM7QUFDekMsT0FBTyxFQUFFLFdBQVcsRUFBRSxVQUFVLEVBQUUsTUFBTSwyQkFBMkIsQ0FBQztBQUNwRSxPQUFPLEVBQUUsUUFBUSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFHMUMsTUFBTSxPQUFPLGNBQWM7Ozs7O0lBQ3pCLFlBQW9CLFlBQTBCLEVBQVUsS0FBWTtRQUFoRCxpQkFBWSxHQUFaLFlBQVksQ0FBYztRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87SUFBRyxDQUFDOzs7Ozs7SUFFeEUsU0FBUyxDQUFDLE9BQXlCLEVBQUUsSUFBaUI7UUFDcEQsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxXQUFXLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQzs7Y0FFeEMsT0FBTyxHQUFHLG1CQUFBLEVBQUUsRUFBTzs7Y0FFbkIsS0FBSyxHQUFHLElBQUksQ0FBQyxZQUFZLENBQUMsY0FBYyxFQUFFO1FBQ2hELElBQUksQ0FBQyxPQUFPLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxlQUFlLENBQUMsSUFBSSxLQUFLLEVBQUU7WUFDbEQsT0FBTyxDQUFDLGVBQWUsQ0FBQyxHQUFHLFVBQVUsS0FBSyxFQUFFLENBQUM7U0FDOUM7O2NBRUssSUFBSSxHQUFHLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFlBQVksQ0FBQyxXQUFXLENBQUM7UUFDaEUsSUFBSSxDQUFDLE9BQU8sQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLGlCQUFpQixDQUFDLElBQUksSUFBSSxFQUFFO1lBQ25ELE9BQU8sQ0FBQyxpQkFBaUIsQ0FBQyxHQUFHLElBQUksQ0FBQztTQUNuQzs7Y0FFSyxNQUFNLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsWUFBWSxDQUFDLFNBQVMsQ0FBQztRQUNoRSxJQUFJLENBQUMsT0FBTyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsVUFBVSxDQUFDLElBQUksTUFBTSxFQUFFO1lBQzlDLE9BQU8sQ0FBQyxVQUFVLENBQUMsR0FBRyxNQUFNLENBQUMsRUFBRSxDQUFDO1NBQ2pDO1FBRUQsT0FBTyxJQUFJO2FBQ1IsTUFBTSxDQUNMLE9BQU8sQ0FBQyxLQUFLLENBQUM7WUFDWixVQUFVLEVBQUUsT0FBTztTQUNwQixDQUFDLENBQ0g7YUFDQSxJQUFJLENBQUMsUUFBUTs7O1FBQUMsR0FBRyxFQUFFLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxVQUFVLENBQUMsT0FBTyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUM7SUFDeEUsQ0FBQzs7O1lBL0JGLFVBQVU7Ozs7WUFORixZQUFZO1lBQ1osS0FBSzs7Ozs7OztJQU9BLHNDQUFrQzs7Ozs7SUFBRSwrQkFBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBIdHRwSW50ZXJjZXB0b3IsIEh0dHBIYW5kbGVyLCBIdHRwUmVxdWVzdCB9IGZyb20gJ0Bhbmd1bGFyL2NvbW1vbi9odHRwJztcbmltcG9ydCB7IE9BdXRoU2VydmljZSB9IGZyb20gJ2FuZ3VsYXItb2F1dGgyLW9pZGMnO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBTZXNzaW9uU3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMnO1xuaW1wb3J0IHsgU3RhcnRMb2FkZXIsIFN0b3BMb2FkZXIgfSBmcm9tICcuLi9hY3Rpb25zL2xvYWRlci5hY3Rpb25zJztcbmltcG9ydCB7IGZpbmFsaXplIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuXG5ASW5qZWN0YWJsZSgpXG5leHBvcnQgY2xhc3MgQXBpSW50ZXJjZXB0b3IgaW1wbGVtZW50cyBIdHRwSW50ZXJjZXB0b3Ige1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIG9BdXRoU2VydmljZTogT0F1dGhTZXJ2aWNlLCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cblxuICBpbnRlcmNlcHQocmVxdWVzdDogSHR0cFJlcXVlc3Q8YW55PiwgbmV4dDogSHR0cEhhbmRsZXIpIHtcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBTdGFydExvYWRlcihyZXF1ZXN0KSk7XG5cbiAgICBjb25zdCBoZWFkZXJzID0ge30gYXMgYW55O1xuXG4gICAgY29uc3QgdG9rZW4gPSB0aGlzLm9BdXRoU2VydmljZS5nZXRBY2Nlc3NUb2tlbigpO1xuICAgIGlmICghcmVxdWVzdC5oZWFkZXJzLmhhcygnQXV0aG9yaXphdGlvbicpICYmIHRva2VuKSB7XG4gICAgICBoZWFkZXJzWydBdXRob3JpemF0aW9uJ10gPSBgQmVhcmVyICR7dG9rZW59YDtcbiAgICB9XG5cbiAgICBjb25zdCBsYW5nID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChTZXNzaW9uU3RhdGUuZ2V0TGFuZ3VhZ2UpO1xuICAgIGlmICghcmVxdWVzdC5oZWFkZXJzLmhhcygnQWNjZXB0LUxhbmd1YWdlJykgJiYgbGFuZykge1xuICAgICAgaGVhZGVyc1snQWNjZXB0LUxhbmd1YWdlJ10gPSBsYW5nO1xuICAgIH1cblxuICAgIGNvbnN0IHRlbmFudCA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoU2Vzc2lvblN0YXRlLmdldFRlbmFudCk7XG4gICAgaWYgKCFyZXF1ZXN0LmhlYWRlcnMuaGFzKCdfX3RlbmFudCcpICYmIHRlbmFudCkge1xuICAgICAgaGVhZGVyc1snX190ZW5hbnQnXSA9IHRlbmFudC5pZDtcbiAgICB9XG5cbiAgICByZXR1cm4gbmV4dFxuICAgICAgLmhhbmRsZShcbiAgICAgICAgcmVxdWVzdC5jbG9uZSh7XG4gICAgICAgICAgc2V0SGVhZGVyczogaGVhZGVycyxcbiAgICAgICAgfSksXG4gICAgICApXG4gICAgICAucGlwZShmaW5hbGl6ZSgoKSA9PiB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBTdG9wTG9hZGVyKHJlcXVlc3QpKSkpO1xuICB9XG59XG4iXX0=