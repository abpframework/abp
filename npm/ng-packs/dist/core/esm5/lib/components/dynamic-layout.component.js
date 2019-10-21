/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { Component } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import snq from 'snq';
import { ConfigState } from '../states/config.state';
import { takeUntilDestroy } from '../utils/rxjs-utils';
var DynamicLayoutComponent = /** @class */ (function() {
  function DynamicLayoutComponent(router, route, store) {
    var _this = this;
    this.router = router;
    this.route = route;
    this.store = store;
    var _a = this.store.selectSnapshot(ConfigState.getAll),
      layouts = _a.requirements.layouts,
      routes = _a.routes;
    if ((this.route.snapshot.data || {}).layout) {
      this.layout = layouts
        .filter(
          /**
           * @param {?} l
           * @return {?}
           */
          function(l) {
            return !!l;
          },
        )
        .find(
          /**
           * @param {?} l
           * @return {?}
           */
          function(l) {
            return (
              snq(
                /**
                 * @return {?}
                 */
                function() {
                  return l.type.toLowerCase().indexOf(_this.route.snapshot.data.layout);
                },
                -1,
              ) > -1
            );
          },
        );
    }
    this.router.events.pipe(takeUntilDestroy(this)).subscribe(
      /**
       * @param {?} event
       * @return {?}
       */
      function(event) {
        if (event instanceof NavigationEnd) {
          var segments = _this.router.parseUrl(event.url).root.children.primary.segments;
          /** @type {?} */
          var layout_1 = (_this.route.snapshot.data || {}).layout || findLayout(segments, routes);
          _this.layout = layouts
            .filter(
              /**
               * @param {?} l
               * @return {?}
               */
              function(l) {
                return !!l;
              },
            )
            .find(
              /**
               * @param {?} l
               * @return {?}
               */
              function(l) {
                return (
                  snq(
                    /**
                     * @return {?}
                     */
                    function() {
                      return l.type.toLowerCase().indexOf(layout_1);
                    },
                    -1,
                  ) > -1
                );
              },
            );
        }
      },
    );
  }
  /**
   * @return {?}
   */
  DynamicLayoutComponent.prototype.ngOnDestroy
  /**
   * @return {?}
   */ = function() {};
  DynamicLayoutComponent.decorators = [
    {
      type: Component,
      args: [
        {
          selector: 'abp-dynamic-layout',
          template:
            '\n    <ng-container *ngTemplateOutlet="layout ? componentOutlet : routerOutlet"></ng-container>\n    <ng-template #routerOutlet><router-outlet></router-outlet></ng-template>\n    <ng-template #componentOutlet><ng-container *ngComponentOutlet="layout"></ng-container></ng-template>\n  ',
        },
      ],
    },
  ];
  /** @nocollapse */
  DynamicLayoutComponent.ctorParameters = function() {
    return [{ type: Router }, { type: ActivatedRoute }, { type: Store }];
  };
  tslib_1.__decorate(
    [Select(ConfigState.getOne('requirements')), tslib_1.__metadata('design:type', Observable)],
    DynamicLayoutComponent.prototype,
    'requirements$',
    void 0,
  );
  return DynamicLayoutComponent;
})();
export { DynamicLayoutComponent };
if (false) {
  /** @type {?} */
  DynamicLayoutComponent.prototype.requirements$;
  /** @type {?} */
  DynamicLayoutComponent.prototype.layout;
  /**
   * @type {?}
   * @private
   */
  DynamicLayoutComponent.prototype.router;
  /**
   * @type {?}
   * @private
   */
  DynamicLayoutComponent.prototype.route;
  /**
   * @type {?}
   * @private
   */
  DynamicLayoutComponent.prototype.store;
}
/**
 * @param {?} segments
 * @param {?} routes
 * @return {?}
 */
function findLayout(segments, routes) {
  /** @type {?} */
  var layout = 'empty'; /* empty */
  /** @type {?} */
  var route = routes
    .reduce(
      /**
       * @param {?} acc
       * @param {?} val
       * @return {?}
       */
      (function(acc, val) {
        return val.wrapper ? tslib_1.__spread(acc, val.children) : tslib_1.__spread(acc, [val]);
      }),
      [],
    )
    .find(
      /**
       * @param {?} r
       * @return {?}
       */
      (function(r) {
        return r.path === segments[0].path;
      }),
    );
  if (route) {
    if (route.layout) {
      layout = route.layout;
    }
    if (route.children && route.children.length && segments.length > 1) {
      /** @type {?} */
      var child = route.children.find(
        /**
         * @param {?} c
         * @return {?}
         */
        (function(c) {
          return c.path === segments[1].path;
        }),
      );
      if (child && child.layout) {
        layout = child.layout;
      }
    }
  }
  return layout;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZHluYW1pYy1sYXlvdXQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvZHluYW1pYy1sYXlvdXQuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBMEIsTUFBTSxlQUFlLENBQUM7QUFDbEUsT0FBTyxFQUFFLGNBQWMsRUFBRSxhQUFhLEVBQUUsTUFBTSxFQUFjLE1BQU0saUJBQWlCLENBQUM7QUFDcEYsT0FBTyxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDNUMsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNsQyxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFJdEIsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLHdCQUF3QixDQUFDO0FBQ3JELE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBRXZEO0lBYUUsZ0NBQW9CLE1BQWMsRUFBVSxLQUFxQixFQUFVLEtBQVk7UUFBdkYsaUJBdUJDO1FBdkJtQixXQUFNLEdBQU4sTUFBTSxDQUFRO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBZ0I7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQy9FLElBQUEsa0RBRzJDLEVBRi9CLGlDQUFPLEVBQ3ZCLGtCQUMrQztRQUVqRCxJQUFJLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxJQUFJLEVBQUUsQ0FBQyxDQUFDLE1BQU0sRUFBRTtZQUMzQyxJQUFJLENBQUMsTUFBTSxHQUFHLE9BQU87aUJBQ2xCLE1BQU07Ozs7WUFBQyxVQUFBLENBQUMsSUFBSSxPQUFBLENBQUMsQ0FBQyxDQUFDLEVBQUgsQ0FBRyxFQUFDO2lCQUNoQixJQUFJOzs7O1lBQUMsVUFBQyxDQUFNLElBQUssT0FBQSxHQUFHOzs7WUFBQyxjQUFN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WUFFbkUsSUFBSSxLQUFLLElBQUksS0FBSyxDQUFDLE1BQU0sRUFBRTtnQkFDekIsTUFBTSxHQUFHLEtBQUssQ0FBQyxNQUFNLENBQUM7YUFDdkI7U0FDRjtLQUNGO0lBRUQsT0FBTyxNQUFNLENBQUM7QUFDaEIsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgSW5wdXQsIE9uRGVzdHJveSwgVHlwZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgQWN0aXZhdGVkUm91dGUsIE5hdmlnYXRpb25FbmQsIFJvdXRlciwgVXJsU2VnbWVudCB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuaW1wb3J0IHsgZUxheW91dFR5cGUgfSBmcm9tICcuLi9lbnVtcy9jb21tb24nO1xuaW1wb3J0IHsgQ29uZmlnIH0gZnJvbSAnLi4vbW9kZWxzL2NvbmZpZyc7XG5pbXBvcnQgeyBBQlAgfSBmcm9tICcuLi9tb2RlbHMvY29tbW9uJztcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzL2NvbmZpZy5zdGF0ZSc7XG5pbXBvcnQgeyB0YWtlVW50aWxEZXN0cm95IH0gZnJvbSAnLi4vdXRpbHMvcnhqcy11dGlscyc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1keW5hbWljLWxheW91dCcsXG4gIHRlbXBsYXRlOiBgXG4gICAgPG5nLWNvbnRhaW5lciAqbmdUZW1wbGF0ZU91dGxldD1cImxheW91dCA/IGNvbXBvbmVudE91dGxldCA6IHJvdXRlck91dGxldFwiPjwvbmctY29udGFpbmVyPlxuICAgIDxuZy10ZW1wbGF0ZSAjcm91dGVyT3V0bGV0Pjxyb3V0ZXItb3V0bGV0Pjwvcm91dGVyLW91dGxldD48L25nLXRlbXBsYXRlPlxuICAgIDxuZy10ZW1wbGF0ZSAjY29tcG9uZW50T3V0bGV0PjxuZy1jb250YWluZXIgKm5nQ29tcG9uZW50T3V0bGV0PVwibGF5b3V0XCI+PC9uZy1jb250YWluZXI+PC9uZy10ZW1wbGF0ZT5cbiAgYFxufSlcbmV4cG9ydCBjbGFzcyBEeW5hbWljTGF5b3V0Q29tcG9uZW50IGltcGxlbWVudHMgT25EZXN0cm95IHtcbiAgQFNlbGVjdChDb25maWdTdGF0ZS5nZXRPbmUoJ3JlcXVpcmVtZW50cycpKSByZXF1aXJlbWVudHMkOiBPYnNlcnZhYmxlPENvbmZpZy5SZXF1aXJlbWVudHM+O1xuXG4gIGxheW91dDogVHlwZTxhbnk+O1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcm91dGVyOiBSb3V0ZXIsIHByaXZhdGUgcm91dGU6IEFjdGl2YXRlZFJvdXRlLCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge1xuICAgIGNvbnN0IHtcbiAgICAgIHJlcXVpcmVtZW50czogeyBsYXlvdXRzIH0sXG4gICAgICByb3V0ZXNcbiAgICB9ID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRBbGwpO1xuXG4gICAgaWYgKCh0aGlzLnJvdXRlLnNuYXBzaG90LmRhdGEgfHwge30pLmxheW91dCkge1xuICAgICAgdGhpcy5sYXlvdXQgPSBsYXlvdXRzXG4gICAgICAgIC5maWx0ZXIobCA9PiAhIWwpXG4gICAgICAgIC5maW5kKChsOiBhbnkpID0+IHNucSgoKSA9PiBsLnR5cGUudG9Mb3dlckNhc2UoKS5pbmRleE9mKHRoaXMucm91dGUuc25hcHNob3QuZGF0YS5sYXlvdXQpLCAtMSkgPiAtMSk7XG4gICAgfVxuXG4gICAgdGhpcy5yb3V0ZXIuZXZlbnRzLnBpcGUodGFrZVVudGlsRGVzdHJveSh0aGlzKSkuc3Vic2NyaWJlKGV2ZW50ID0+IHtcbiAgICAgIGlmIChldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25FbmQpIHtcbiAgICAgICAgY29uc3QgeyBzZWdtZW50cyB9ID0gdGhpcy5yb3V0ZXIucGFyc2VVcmwoZXZlbnQudXJsKS5yb290LmNoaWxkcmVuLnByaW1hcnk7XG5cbiAgICAgICAgY29uc3QgbGF5b3V0ID0gKHRoaXMucm91dGUuc25hcHNob3QuZGF0YSB8fCB7fSkubGF5b3V0IHx8IGZpbmRMYXlvdXQoc2VnbWVudHMsIHJvdXRlcyk7XG5cbiAgICAgICAgdGhpcy5sYXlvdXQgPSBsYXlvdXRzXG4gICAgICAgICAgLmZpbHRlcihsID0+ICEhbClcbiAgICAgICAgICAuZmluZCgobDogYW55KSA9PiBzbnEoKCkgPT4gbC50eXBlLnRvTG93ZXJDYXNlKCkuaW5kZXhPZihsYXlvdXQpLCAtMSkgPiAtMSk7XG4gICAgICB9XG4gICAgfSk7XG4gIH1cblxuICBuZ09uRGVzdHJveSgpIHt9XG59XG5cbmZ1bmN0aW9uIGZpbmRMYXlvdXQoc2VnbWVudHM6IFVybFNlZ21lbnRbXSwgcm91dGVzOiBBQlAuRnVsbFJvdXRlW10pOiBlTGF5b3V0VHlwZSB7XG4gIGxldCBsYXlvdXQgPSBlTGF5b3V0VHlwZS5lbXB0eTtcblxuICBjb25zdCByb3V0ZSA9IHJvdXRlc1xuICAgIC5yZWR1Y2UoKGFjYywgdmFsKSA9PiAodmFsLndyYXBwZXIgPyBbLi4uYWNjLCAuLi52YWwuY2hpbGRyZW5dIDogWy4uLmFjYywgdmFsXSksIFtdKVxuICAgIC5maW5kKHIgPT4gci5wYXRoID09PSBzZWdtZW50c1swXS5wYXRoKTtcblxuICBpZiAocm91dGUpIHtcbiAgICBpZiAocm91dGUubGF5b3V0KSB7XG4gICAgICBsYXlvdXQgPSByb3V0ZS5sYXlvdXQ7XG4gICAgfVxuXG4gICAgaWYgKHJvdXRlLmNoaWxkcmVuICYmIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCAmJiBzZWdtZW50cy5sZW5ndGggPiAxKSB7XG4gICAgICBjb25zdCBjaGlsZCA9IHJvdXRlLmNoaWxkcmVuLmZpbmQoYyA9PiBjLnBhdGggPT09IHNlZ21lbnRzWzFdLnBhdGgpO1xuXG4gICAgICBpZiAoY2hpbGQgJiYgY2hpbGQubGF5b3V0KSB7XG4gICAgICAgIGxheW91dCA9IGNoaWxkLmxheW91dDtcbiAgICAgIH1cbiAgICB9XG4gIH1cblxuICByZXR1cm4gbGF5b3V0O1xufVxuIl19
