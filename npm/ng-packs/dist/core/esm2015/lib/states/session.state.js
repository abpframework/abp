/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { Action, Selector, State } from '@ngxs/store';
import { SetLanguage, SetTenant } from '../actions/session.actions';
import { GetAppConfiguration } from '../actions/config.actions';
import { LocalizationService } from '../services/localization.service';
import { from } from 'rxjs';
import { switchMap } from 'rxjs/operators';
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
  setTenantId({ patchState }, { payload }) {
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
  'setTenantId',
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2Vzc2lvbi5zdGF0ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvc2Vzc2lvbi5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLFdBQVcsRUFBRSxTQUFTLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUVwRSxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSwyQkFBMkIsQ0FBQztBQUNoRSxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSxrQ0FBa0MsQ0FBQztBQUN2RSxPQUFPLEVBQUUsSUFBSSxFQUFpQixNQUFNLE1BQU0sQ0FBQztBQUMzQyxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7SUFNOUIsWUFBWSxTQUFaLFlBQVk7Ozs7SUFXdkIsWUFBb0IsbUJBQXdDO1FBQXhDLHdCQUFtQixHQUFuQixtQkFBbUIsQ0FBcUI7SUFBRyxDQUFDOzs7OztJQVRoRSxNQUFNLENBQUMsV0FBVyxDQUFDLEVBQUUsUUFBUSxFQUFpQjtRQUM1QyxPQUFPLFFBQVEsQ0FBQztJQUNsQixDQUFDOzs7OztJQUdELE1BQU0sQ0FBQyxTQUFTLENBQUMsRUFBRSxNQUFNLEVBQWlCO1FBQ3hDLE9BQU8sTUFBTSxDQUFDO0lBQ2hCLENBQUM7Ozs7OztJQUtELFdBQVcsQ0FBQyxFQUFFLFVBQVUsRUFBRSxRQUFRLEVBQStCLEVBQUUsRUFBRSxPQUFPLEVBQWU7UUFDekYsVUFBVSxDQUFDO1lBQ1QsUUFBUSxFQUFFLE9BQU87U0FDbEIsQ0FBQyxDQUFDO1FBRUgsT0FBTyxRQUFRLENBQUMsSUFBSSxtQkFBbUIsRUFBRSxDQUFDLENBQUMsSUFBSSxDQUM3QyxTQUFTOzs7UUFBQyxHQUFHLEVBQUUsQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLG1CQUFtQixDQUFDLGNBQWMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxFQUFDLENBQ3hFLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCxXQUFXLENBQUMsRUFBRSxVQUFVLEVBQStCLEVBQUUsRUFBRSxPQUFPLEVBQWE7UUFDN0UsVUFBVSxDQUFDO1lBQ1QsTUFBTSxFQUFFLE9BQU87U0FDaEIsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztDQUNGLENBQUE7O1lBbkIwQyxtQkFBbUI7O0FBRzVEO0lBREMsTUFBTSxDQUFDLFdBQVcsQ0FBQzs7cURBQzRELFdBQVc7OytDQVExRjtBQUdEO0lBREMsTUFBTSxDQUFDLFNBQVMsQ0FBQzs7cURBQ29ELFNBQVM7OytDQUk5RTtBQTNCRDtJQURDLFFBQVEsRUFBRTs7OztxQ0FHVjtBQUdEO0lBREMsUUFBUSxFQUFFOzs7O21DQUdWO0FBVFUsWUFBWTtJQUp4QixLQUFLLENBQWdCO1FBQ3BCLElBQUksRUFBRSxjQUFjO1FBQ3BCLFFBQVEsRUFBRSxtQkFBQSxFQUFFLEVBQWlCO0tBQzlCLENBQUM7NkNBWXlDLG1CQUFtQjtHQVhqRCxZQUFZLENBOEJ4QjtTQTlCWSxZQUFZOzs7Ozs7SUFXWCwyQ0FBZ0QiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBY3Rpb24sIFNlbGVjdG9yLCBTdGF0ZSwgU3RhdGVDb250ZXh0IH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgU2V0TGFuZ3VhZ2UsIFNldFRlbmFudCB9IGZyb20gJy4uL2FjdGlvbnMvc2Vzc2lvbi5hY3Rpb25zJztcbmltcG9ydCB7IEFCUCwgU2Vzc2lvbiB9IGZyb20gJy4uL21vZGVscyc7XG5pbXBvcnQgeyBHZXRBcHBDb25maWd1cmF0aW9uIH0gZnJvbSAnLi4vYWN0aW9ucy9jb25maWcuYWN0aW9ucyc7XG5pbXBvcnQgeyBMb2NhbGl6YXRpb25TZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvbG9jYWxpemF0aW9uLnNlcnZpY2UnO1xuaW1wb3J0IHsgZnJvbSwgY29tYmluZUxhdGVzdCB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgc3dpdGNoTWFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuXG5AU3RhdGU8U2Vzc2lvbi5TdGF0ZT4oe1xuICBuYW1lOiAnU2Vzc2lvblN0YXRlJyxcbiAgZGVmYXVsdHM6IHt9IGFzIFNlc3Npb24uU3RhdGUsXG59KVxuZXhwb3J0IGNsYXNzIFNlc3Npb25TdGF0ZSB7XG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRMYW5ndWFnZSh7IGxhbmd1YWdlIH06IFNlc3Npb24uU3RhdGUpOiBzdHJpbmcge1xuICAgIHJldHVybiBsYW5ndWFnZTtcbiAgfVxuXG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRUZW5hbnQoeyB0ZW5hbnQgfTogU2Vzc2lvbi5TdGF0ZSk6IEFCUC5CYXNpY0l0ZW0ge1xuICAgIHJldHVybiB0ZW5hbnQ7XG4gIH1cblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGxvY2FsaXphdGlvblNlcnZpY2U6IExvY2FsaXphdGlvblNlcnZpY2UpIHt9XG5cbiAgQEFjdGlvbihTZXRMYW5ndWFnZSlcbiAgc2V0TGFuZ3VhZ2UoeyBwYXRjaFN0YXRlLCBkaXNwYXRjaCB9OiBTdGF0ZUNvbnRleHQ8U2Vzc2lvbi5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBTZXRMYW5ndWFnZSkge1xuICAgIHBhdGNoU3RhdGUoe1xuICAgICAgbGFuZ3VhZ2U6IHBheWxvYWQsXG4gICAgfSk7XG5cbiAgICByZXR1cm4gZGlzcGF0Y2gobmV3IEdldEFwcENvbmZpZ3VyYXRpb24oKSkucGlwZShcbiAgICAgIHN3aXRjaE1hcCgoKSA9PiBmcm9tKHRoaXMubG9jYWxpemF0aW9uU2VydmljZS5yZWdpc3RlckxvY2FsZShwYXlsb2FkKSkpLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKFNldFRlbmFudClcbiAgc2V0VGVuYW50SWQoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxTZXNzaW9uLlN0YXRlPiwgeyBwYXlsb2FkIH06IFNldFRlbmFudCkge1xuICAgIHBhdGNoU3RhdGUoe1xuICAgICAgdGVuYW50OiBwYXlsb2FkLFxuICAgIH0pO1xuICB9XG59XG4iXX0=
