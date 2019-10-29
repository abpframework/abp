/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { Action, Selector, State } from '@ngxs/store';
import { from } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { GetAppConfiguration } from '../actions/config.actions';
import { SetLanguage, SetTenant } from '../actions/session.actions';
import { LocalizationService } from '../services/localization.service';
let SessionState = class SessionState {
  /**
   * @param {?} localizationService
   */
  constructor(localizationService) {
    this.localizationService = localizationService;
  }
  /**
   * @param {?} __0
   * @return {?}
   */
  static getLanguage({ language }) {
    return language;
  }
  /**
   * @param {?} __0
   * @return {?}
   */
  static getTenant({ tenant }) {
    return tenant;
  }
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  setLanguage({ patchState, dispatch }, { payload }) {
    patchState({
      language: payload,
    });
    return dispatch(new GetAppConfiguration()).pipe(
      switchMap(
        /**
         * @return {?}
         */
        () => from(this.localizationService.registerLocale(payload)),
      ),
    );
  }
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  setTenant({ patchState }, { payload }) {
    patchState({
      tenant: payload,
    });
  }
};
SessionState.ctorParameters = () => [{ type: LocalizationService }];
tslib_1.__decorate(
  [
    Action(SetLanguage),
    tslib_1.__metadata('design:type', Function),
    tslib_1.__metadata('design:paramtypes', [Object, SetLanguage]),
    tslib_1.__metadata('design:returntype', void 0),
  ],
  SessionState.prototype,
  'setLanguage',
  null,
);
tslib_1.__decorate(
  [
    Action(SetTenant),
    tslib_1.__metadata('design:type', Function),
    tslib_1.__metadata('design:paramtypes', [Object, SetTenant]),
    tslib_1.__metadata('design:returntype', void 0),
  ],
  SessionState.prototype,
  'setTenant',
  null,
);
tslib_1.__decorate(
  [
    Selector(),
    tslib_1.__metadata('design:type', Function),
    tslib_1.__metadata('design:paramtypes', [Object]),
    tslib_1.__metadata('design:returntype', String),
  ],
  SessionState,
  'getLanguage',
  null,
);
tslib_1.__decorate(
  [
    Selector(),
    tslib_1.__metadata('design:type', Function),
    tslib_1.__metadata('design:paramtypes', [Object]),
    tslib_1.__metadata('design:returntype', Object),
  ],
  SessionState,
  'getTenant',
  null,
);
SessionState = tslib_1.__decorate(
  [
    State({
      name: 'SessionState',
      defaults: /** @type {?} */ ({}),
    }),
    tslib_1.__metadata('design:paramtypes', [LocalizationService]),
  ],
  SessionState,
);
export { SessionState };
if (false) {
  /**
   * @type {?}
   * @private
   */
  SessionState.prototype.localizationService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2Vzc2lvbi5zdGF0ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvc2Vzc2lvbi5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLElBQUksRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUM1QixPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDM0MsT0FBTyxFQUFFLG1CQUFtQixFQUFFLE1BQU0sMkJBQTJCLENBQUM7QUFDaEUsT0FBTyxFQUFFLFdBQVcsRUFBRSxTQUFTLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUVwRSxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSxrQ0FBa0MsQ0FBQztJQU0xRCxZQUFZLFNBQVosWUFBWTs7OztJQVd2QixZQUFvQixtQkFBd0M7UUFBeEMsd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtJQUFHLENBQUM7Ozs7O0lBVGhFLE1BQU0sQ0FBQyxXQUFXLENBQUMsRUFBRSxRQUFRLEVBQWlCO1FBQzVDLE9BQU8sUUFBUSxDQUFDO0lBQ2xCLENBQUM7Ozs7O0lBR0QsTUFBTSxDQUFDLFNBQVMsQ0FBQyxFQUFFLE1BQU0sRUFBaUI7UUFDeEMsT0FBTyxNQUFNLENBQUM7SUFDaEIsQ0FBQzs7Ozs7O0lBS0QsV0FBVyxDQUFDLEVBQUUsVUFBVSxFQUFFLFFBQVEsRUFBK0IsRUFBRSxFQUFFLE9BQU8sRUFBZTtRQUN6RixVQUFVLENBQUM7WUFDVCxRQUFRLEVBQUUsT0FBTztTQUNsQixDQUFDLENBQUM7UUFFSCxPQUFPLFFBQVEsQ0FBQyxJQUFJLG1CQUFtQixFQUFFLENBQUMsQ0FBQyxJQUFJLENBQzdDLFNBQVM7OztRQUFDLEdBQUcsRUFBRSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsbUJBQW1CLENBQUMsY0FBYyxDQUFDLE9BQU8sQ0FBQyxDQUFDLEVBQUMsQ0FDeEUsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELFNBQVMsQ0FBQyxFQUFFLFVBQVUsRUFBK0IsRUFBRSxFQUFFLE9BQU8sRUFBYTtRQUMzRSxVQUFVLENBQUM7WUFDVCxNQUFNLEVBQUUsT0FBTztTQUNoQixDQUFDLENBQUM7SUFDTCxDQUFDO0NBQ0YsQ0FBQTs7WUFuQjBDLG1CQUFtQjs7QUFHNUQ7SUFEQyxNQUFNLENBQUMsV0FBVyxDQUFDOztxREFDNEQsV0FBVzs7K0NBUTFGO0FBR0Q7SUFEQyxNQUFNLENBQUMsU0FBUyxDQUFDOztxREFDa0QsU0FBUzs7NkNBSTVFO0FBM0JEO0lBREMsUUFBUSxFQUFFOzs7O3FDQUdWO0FBR0Q7SUFEQyxRQUFRLEVBQUU7Ozs7bUNBR1Y7QUFUVSxZQUFZO0lBSnhCLEtBQUssQ0FBZ0I7UUFDcEIsSUFBSSxFQUFFLGNBQWM7UUFDcEIsUUFBUSxFQUFFLG1CQUFBLEVBQUUsRUFBaUI7S0FDOUIsQ0FBQzs2Q0FZeUMsbUJBQW1CO0dBWGpELFlBQVksQ0E4QnhCO1NBOUJZLFlBQVk7Ozs7OztJQVdYLDJDQUFnRCIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFjdGlvbiwgU2VsZWN0b3IsIFN0YXRlLCBTdGF0ZUNvbnRleHQgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBmcm9tIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBzd2l0Y2hNYXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgeyBHZXRBcHBDb25maWd1cmF0aW9uIH0gZnJvbSAnLi4vYWN0aW9ucy9jb25maWcuYWN0aW9ucyc7XG5pbXBvcnQgeyBTZXRMYW5ndWFnZSwgU2V0VGVuYW50IH0gZnJvbSAnLi4vYWN0aW9ucy9zZXNzaW9uLmFjdGlvbnMnO1xuaW1wb3J0IHsgQUJQLCBTZXNzaW9uIH0gZnJvbSAnLi4vbW9kZWxzJztcbmltcG9ydCB7IExvY2FsaXphdGlvblNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9sb2NhbGl6YXRpb24uc2VydmljZSc7XG5cbkBTdGF0ZTxTZXNzaW9uLlN0YXRlPih7XG4gIG5hbWU6ICdTZXNzaW9uU3RhdGUnLFxuICBkZWZhdWx0czoge30gYXMgU2Vzc2lvbi5TdGF0ZSxcbn0pXG5leHBvcnQgY2xhc3MgU2Vzc2lvblN0YXRlIHtcbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldExhbmd1YWdlKHsgbGFuZ3VhZ2UgfTogU2Vzc2lvbi5TdGF0ZSk6IHN0cmluZyB7XG4gICAgcmV0dXJuIGxhbmd1YWdlO1xuICB9XG5cbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldFRlbmFudCh7IHRlbmFudCB9OiBTZXNzaW9uLlN0YXRlKTogQUJQLkJhc2ljSXRlbSB7XG4gICAgcmV0dXJuIHRlbmFudDtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgbG9jYWxpemF0aW9uU2VydmljZTogTG9jYWxpemF0aW9uU2VydmljZSkge31cblxuICBAQWN0aW9uKFNldExhbmd1YWdlKVxuICBzZXRMYW5ndWFnZSh7IHBhdGNoU3RhdGUsIGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxTZXNzaW9uLlN0YXRlPiwgeyBwYXlsb2FkIH06IFNldExhbmd1YWdlKSB7XG4gICAgcGF0Y2hTdGF0ZSh7XG4gICAgICBsYW5ndWFnZTogcGF5bG9hZCxcbiAgICB9KTtcblxuICAgIHJldHVybiBkaXNwYXRjaChuZXcgR2V0QXBwQ29uZmlndXJhdGlvbigpKS5waXBlKFxuICAgICAgc3dpdGNoTWFwKCgpID0+IGZyb20odGhpcy5sb2NhbGl6YXRpb25TZXJ2aWNlLnJlZ2lzdGVyTG9jYWxlKHBheWxvYWQpKSksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oU2V0VGVuYW50KVxuICBzZXRUZW5hbnQoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxTZXNzaW9uLlN0YXRlPiwgeyBwYXlsb2FkIH06IFNldFRlbmFudCkge1xuICAgIHBhdGNoU3RhdGUoe1xuICAgICAgdGVuYW50OiBwYXlsb2FkLFxuICAgIH0pO1xuICB9XG59XG4iXX0=
