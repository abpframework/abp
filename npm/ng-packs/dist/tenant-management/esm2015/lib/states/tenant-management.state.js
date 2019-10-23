/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { Action, Selector, State } from '@ngxs/store';
import { switchMap, tap } from 'rxjs/operators';
import {
  CreateTenant,
  DeleteTenant,
  GetTenants,
  GetTenantById,
  UpdateTenant,
} from '../actions/tenant-management.actions';
import { TenantManagementService } from '../services/tenant-management.service';
let TenantManagementState = class TenantManagementState {
  /**
   * @param {?} tenantManagementService
   */
  constructor(tenantManagementService) {
    this.tenantManagementService = tenantManagementService;
  }
  /**
   * @param {?} __0
   * @return {?}
   */
  static get({ result }) {
    return result.items || [];
  }
  /**
   * @param {?} __0
   * @return {?}
   */
  static getTenantsTotalCount({ result }) {
    return result.totalCount;
  }
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  get({ patchState }, { payload }) {
    return this.tenantManagementService.getTenant(payload).pipe(
      tap(
        /**
         * @param {?} result
         * @return {?}
         */
        result =>
          patchState({
            result,
          }),
      ),
    );
  }
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  getById({ patchState }, { payload }) {
    return this.tenantManagementService.getTenantById(payload).pipe(
      tap(
        /**
         * @param {?} selectedItem
         * @return {?}
         */
        selectedItem =>
          patchState({
            selectedItem,
          }),
      ),
    );
  }
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  delete({ dispatch }, { payload }) {
    return this.tenantManagementService.deleteTenant(payload).pipe(
      switchMap(
        /**
         * @return {?}
         */
        () => dispatch(new GetTenants()),
      ),
    );
  }
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  add({ dispatch }, { payload }) {
    return this.tenantManagementService.createTenant(payload).pipe(
      switchMap(
        /**
         * @return {?}
         */
        () => dispatch(new GetTenants()),
      ),
    );
  }
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  update({ dispatch, getState }, { payload }) {
    return dispatch(new GetTenantById(payload.id)).pipe(
      switchMap(
        /**
         * @return {?}
         */
        () => this.tenantManagementService.updateTenant(Object.assign({}, getState().selectedItem, payload)),
      ),
      switchMap(
        /**
         * @return {?}
         */
        () => dispatch(new GetTenants()),
      ),
    );
  }
};
TenantManagementState.ctorParameters = () => [{ type: TenantManagementService }];
tslib_1.__decorate(
  [
    Action(GetTenants),
    tslib_1.__metadata('design:type', Function),
    tslib_1.__metadata('design:paramtypes', [Object, GetTenants]),
    tslib_1.__metadata('design:returntype', void 0),
  ],
  TenantManagementState.prototype,
  'get',
  null,
);
tslib_1.__decorate(
  [
    Action(GetTenantById),
    tslib_1.__metadata('design:type', Function),
    tslib_1.__metadata('design:paramtypes', [Object, GetTenantById]),
    tslib_1.__metadata('design:returntype', void 0),
  ],
  TenantManagementState.prototype,
  'getById',
  null,
);
tslib_1.__decorate(
  [
    Action(DeleteTenant),
    tslib_1.__metadata('design:type', Function),
    tslib_1.__metadata('design:paramtypes', [Object, DeleteTenant]),
    tslib_1.__metadata('design:returntype', void 0),
  ],
  TenantManagementState.prototype,
  'delete',
  null,
);
tslib_1.__decorate(
  [
    Action(CreateTenant),
    tslib_1.__metadata('design:type', Function),
    tslib_1.__metadata('design:paramtypes', [Object, CreateTenant]),
    tslib_1.__metadata('design:returntype', void 0),
  ],
  TenantManagementState.prototype,
  'add',
  null,
);
tslib_1.__decorate(
  [
    Action(UpdateTenant),
    tslib_1.__metadata('design:type', Function),
    tslib_1.__metadata('design:paramtypes', [Object, UpdateTenant]),
    tslib_1.__metadata('design:returntype', void 0),
  ],
  TenantManagementState.prototype,
  'update',
  null,
);
tslib_1.__decorate(
  [
    Selector(),
    tslib_1.__metadata('design:type', Function),
    tslib_1.__metadata('design:paramtypes', [Object]),
    tslib_1.__metadata('design:returntype', Array),
  ],
  TenantManagementState,
  'get',
  null,
);
tslib_1.__decorate(
  [
    Selector(),
    tslib_1.__metadata('design:type', Function),
    tslib_1.__metadata('design:paramtypes', [Object]),
    tslib_1.__metadata('design:returntype', Number),
  ],
  TenantManagementState,
  'getTenantsTotalCount',
  null,
);
TenantManagementState = tslib_1.__decorate(
  [
    State({
      name: 'TenantManagementState',
      defaults: /** @type {?} */ ({ result: {}, selectedItem: {} }),
    }),
    tslib_1.__metadata('design:paramtypes', [TenantManagementService]),
  ],
  TenantManagementState,
);
export { TenantManagementState };
if (false) {
  /**
   * @type {?}
   * @private
   */
  TenantManagementState.prototype.tenantManagementService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQuc3RhdGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy90ZW5hbnQtbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLFNBQVMsRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNoRCxPQUFPLEVBQ0wsWUFBWSxFQUNaLFlBQVksRUFDWixVQUFVLEVBQ1YsYUFBYSxFQUNiLFlBQVksR0FDYixNQUFNLHNDQUFzQyxDQUFDO0FBRTlDLE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxNQUFNLHVDQUF1QyxDQUFDO0lBT25FLHFCQUFxQixTQUFyQixxQkFBcUI7Ozs7SUFXaEMsWUFBb0IsdUJBQWdEO1FBQWhELDRCQUF1QixHQUF2Qix1QkFBdUIsQ0FBeUI7SUFBRyxDQUFDOzs7OztJQVR4RSxNQUFNLENBQUMsR0FBRyxDQUFDLEVBQUUsTUFBTSxFQUEwQjtRQUMzQyxPQUFPLE1BQU0sQ0FBQyxLQUFLLElBQUksRUFBRSxDQUFDO0lBQzVCLENBQUM7Ozs7O0lBR0QsTUFBTSxDQUFDLG9CQUFvQixDQUFDLEVBQUUsTUFBTSxFQUEwQjtRQUM1RCxPQUFPLE1BQU0sQ0FBQyxVQUFVLENBQUM7SUFDM0IsQ0FBQzs7Ozs7O0lBS0QsR0FBRyxDQUFDLEVBQUUsVUFBVSxFQUF3QyxFQUFFLEVBQUUsT0FBTyxFQUFjO1FBQy9FLE9BQU8sSUFBSSxDQUFDLHVCQUF1QixDQUFDLFNBQVMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQ3pELEdBQUc7Ozs7UUFBQyxNQUFNLENBQUMsRUFBRSxDQUNYLFVBQVUsQ0FBQztZQUNULE1BQU07U0FDUCxDQUFDLEVBQ0gsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0QsT0FBTyxDQUFDLEVBQUUsVUFBVSxFQUF3QyxFQUFFLEVBQUUsT0FBTyxFQUFpQjtRQUN0RixPQUFPLElBQUksQ0FBQyx1QkFBdUIsQ0FBQyxhQUFhLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUM3RCxHQUFHOzs7O1FBQUMsWUFBWSxDQUFDLEVBQUUsQ0FDakIsVUFBVSxDQUFDO1lBQ1QsWUFBWTtTQUNiLENBQUMsRUFDSCxDQUNGLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCxNQUFNLENBQUMsRUFBRSxRQUFRLEVBQXdDLEVBQUUsRUFBRSxPQUFPLEVBQWdCO1FBQ2xGLE9BQU8sSUFBSSxDQUFDLHVCQUF1QixDQUFDLFlBQVksQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsU0FBUzs7O1FBQUMsR0FBRyxFQUFFLENBQUMsUUFBUSxDQUFDLElBQUksVUFBVSxFQUFFLENBQUMsRUFBQyxDQUFDLENBQUM7SUFDOUcsQ0FBQzs7Ozs7O0lBR0QsR0FBRyxDQUFDLEVBQUUsUUFBUSxFQUF3QyxFQUFFLEVBQUUsT0FBTyxFQUFnQjtRQUMvRSxPQUFPLElBQUksQ0FBQyx1QkFBdUIsQ0FBQyxZQUFZLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUFDLFNBQVM7OztRQUFDLEdBQUcsRUFBRSxDQUFDLFFBQVEsQ0FBQyxJQUFJLFVBQVUsRUFBRSxDQUFDLEVBQUMsQ0FBQyxDQUFDO0lBQzlHLENBQUM7Ozs7OztJQUdELE1BQU0sQ0FBQyxFQUFFLFFBQVEsRUFBRSxRQUFRLEVBQXdDLEVBQUUsRUFBRSxPQUFPLEVBQWdCO1FBQzVGLE9BQU8sUUFBUSxDQUFDLElBQUksYUFBYSxDQUFDLE9BQU8sQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FDakQsU0FBUzs7O1FBQUMsR0FBRyxFQUFFLENBQUMsSUFBSSxDQUFDLHVCQUF1QixDQUFDLFlBQVksbUJBQU0sUUFBUSxFQUFFLENBQUMsWUFBWSxFQUFLLE9BQU8sRUFBRyxFQUFDLEVBQ3RHLFNBQVM7OztRQUFDLEdBQUcsRUFBRSxDQUFDLFFBQVEsQ0FBQyxJQUFJLFVBQVUsRUFBRSxDQUFDLEVBQUMsQ0FDNUMsQ0FBQztJQUNKLENBQUM7Q0FDRixDQUFBOztZQXpDOEMsdUJBQXVCOztBQUdwRTtJQURDLE1BQU0sQ0FBQyxVQUFVLENBQUM7O3FEQUNvRCxVQUFVOztnREFRaEY7QUFHRDtJQURDLE1BQU0sQ0FBQyxhQUFhLENBQUM7O3FEQUNxRCxhQUFhOztvREFRdkY7QUFHRDtJQURDLE1BQU0sQ0FBQyxZQUFZLENBQUM7O3FEQUNtRCxZQUFZOzttREFFbkY7QUFHRDtJQURDLE1BQU0sQ0FBQyxZQUFZLENBQUM7O3FEQUNnRCxZQUFZOztnREFFaEY7QUFHRDtJQURDLE1BQU0sQ0FBQyxZQUFZLENBQUM7O3FEQUM2RCxZQUFZOzttREFLN0Y7QUFqREQ7SUFEQyxRQUFRLEVBQUU7Ozs7c0NBR1Y7QUFHRDtJQURDLFFBQVEsRUFBRTs7Ozt1REFHVjtBQVRVLHFCQUFxQjtJQUpqQyxLQUFLLENBQXlCO1FBQzdCLElBQUksRUFBRSx1QkFBdUI7UUFDN0IsUUFBUSxFQUFFLG1CQUFBLEVBQUUsTUFBTSxFQUFFLEVBQUUsRUFBRSxZQUFZLEVBQUUsRUFBRSxFQUFFLEVBQTBCO0tBQ3JFLENBQUM7NkNBWTZDLHVCQUF1QjtHQVh6RCxxQkFBcUIsQ0FvRGpDO1NBcERZLHFCQUFxQjs7Ozs7O0lBV3BCLHdEQUF3RCIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFjdGlvbiwgU2VsZWN0b3IsIFN0YXRlLCBTdGF0ZUNvbnRleHQgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBzd2l0Y2hNYXAsIHRhcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7XG4gIENyZWF0ZVRlbmFudCxcbiAgRGVsZXRlVGVuYW50LFxuICBHZXRUZW5hbnRzLFxuICBHZXRUZW5hbnRCeUlkLFxuICBVcGRhdGVUZW5hbnQsXG59IGZyb20gJy4uL2FjdGlvbnMvdGVuYW50LW1hbmFnZW1lbnQuYWN0aW9ucyc7XG5pbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzL3RlbmFudC1tYW5hZ2VtZW50JztcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvdGVuYW50LW1hbmFnZW1lbnQuc2VydmljZSc7XG5pbXBvcnQgeyBBQlAgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuXG5AU3RhdGU8VGVuYW50TWFuYWdlbWVudC5TdGF0ZT4oe1xuICBuYW1lOiAnVGVuYW50TWFuYWdlbWVudFN0YXRlJyxcbiAgZGVmYXVsdHM6IHsgcmVzdWx0OiB7fSwgc2VsZWN0ZWRJdGVtOiB7fSB9IGFzIFRlbmFudE1hbmFnZW1lbnQuU3RhdGUsXG59KVxuZXhwb3J0IGNsYXNzIFRlbmFudE1hbmFnZW1lbnRTdGF0ZSB7XG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXQoeyByZXN1bHQgfTogVGVuYW50TWFuYWdlbWVudC5TdGF0ZSk6IEFCUC5CYXNpY0l0ZW1bXSB7XG4gICAgcmV0dXJuIHJlc3VsdC5pdGVtcyB8fCBbXTtcbiAgfVxuXG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRUZW5hbnRzVG90YWxDb3VudCh7IHJlc3VsdCB9OiBUZW5hbnRNYW5hZ2VtZW50LlN0YXRlKTogbnVtYmVyIHtcbiAgICByZXR1cm4gcmVzdWx0LnRvdGFsQ291bnQ7XG4gIH1cblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlOiBUZW5hbnRNYW5hZ2VtZW50U2VydmljZSkge31cblxuICBAQWN0aW9uKEdldFRlbmFudHMpXG4gIGdldCh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFRlbmFudE1hbmFnZW1lbnQuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0VGVuYW50cykge1xuICAgIHJldHVybiB0aGlzLnRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlLmdldFRlbmFudChwYXlsb2FkKS5waXBlKFxuICAgICAgdGFwKHJlc3VsdCA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICByZXN1bHQsXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihHZXRUZW5hbnRCeUlkKVxuICBnZXRCeUlkKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8VGVuYW50TWFuYWdlbWVudC5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRUZW5hbnRCeUlkKSB7XG4gICAgcmV0dXJuIHRoaXMudGVuYW50TWFuYWdlbWVudFNlcnZpY2UuZ2V0VGVuYW50QnlJZChwYXlsb2FkKS5waXBlKFxuICAgICAgdGFwKHNlbGVjdGVkSXRlbSA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICBzZWxlY3RlZEl0ZW0sXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihEZWxldGVUZW5hbnQpXG4gIGRlbGV0ZSh7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxUZW5hbnRNYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IERlbGV0ZVRlbmFudCkge1xuICAgIHJldHVybiB0aGlzLnRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlLmRlbGV0ZVRlbmFudChwYXlsb2FkKS5waXBlKHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgR2V0VGVuYW50cygpKSkpO1xuICB9XG5cbiAgQEFjdGlvbihDcmVhdGVUZW5hbnQpXG4gIGFkZCh7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxUZW5hbnRNYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IENyZWF0ZVRlbmFudCkge1xuICAgIHJldHVybiB0aGlzLnRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlLmNyZWF0ZVRlbmFudChwYXlsb2FkKS5waXBlKHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgR2V0VGVuYW50cygpKSkpO1xuICB9XG5cbiAgQEFjdGlvbihVcGRhdGVUZW5hbnQpXG4gIHVwZGF0ZSh7IGRpc3BhdGNoLCBnZXRTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8VGVuYW50TWFuYWdlbWVudC5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBVcGRhdGVUZW5hbnQpIHtcbiAgICByZXR1cm4gZGlzcGF0Y2gobmV3IEdldFRlbmFudEJ5SWQocGF5bG9hZC5pZCkpLnBpcGUoXG4gICAgICBzd2l0Y2hNYXAoKCkgPT4gdGhpcy50ZW5hbnRNYW5hZ2VtZW50U2VydmljZS51cGRhdGVUZW5hbnQoeyAuLi5nZXRTdGF0ZSgpLnNlbGVjdGVkSXRlbSwgLi4ucGF5bG9hZCB9KSksXG4gICAgICBzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IEdldFRlbmFudHMoKSkpLFxuICAgICk7XG4gIH1cbn1cbiJdfQ==
