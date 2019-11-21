(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@abp/ng.core'), require('@abp/ng.theme.shared'), require('@angular/core'), require('@ng-bootstrap/ng-bootstrap'), require('@ngx-validate/core'), require('@ngxs/store'), require('primeng/toast'), require('@ngxs/router-plugin'), require('angular-oauth2-oidc'), require('just-compare'), require('rxjs'), require('rxjs/operators'), require('snq')) :
    typeof define === 'function' && define.amd ? define('@abp/ng.theme.basic', ['exports', '@abp/ng.core', '@abp/ng.theme.shared', '@angular/core', '@ng-bootstrap/ng-bootstrap', '@ngx-validate/core', '@ngxs/store', 'primeng/toast', '@ngxs/router-plugin', 'angular-oauth2-oidc', 'just-compare', 'rxjs', 'rxjs/operators', 'snq'], factory) :
    (global = global || self, factory((global.abp = global.abp || {}, global.abp.ng = global.abp.ng || {}, global.abp.ng.theme = global.abp.ng.theme || {}, global.abp.ng.theme.basic = {}), global.ng_core, global.ng_theme_shared, global.ng.core, global.ngBootstrap, global.core$1, global.store, global.toast, global.routerPlugin, global.angularOauth2Oidc, global.compare, global.rxjs, global.rxjs.operators, global.snq));
}(this, (function (exports, ng_core, ng_theme_shared, core, ngBootstrap, core$1, store, toast, routerPlugin, angularOauth2Oidc, compare, rxjs, operators, snq) { 'use strict';
          reject(e);
        }
      }
      function rejected(value) {
        try {
          step(generator['throw'](value));
        } catch (e) {
          reject(e);
        }
      }
      function step(result) {
        result.done
          ? resolve(result.value)
          : new P(function(resolve) {
              resolve(result.value);
            }).then(fulfilled, rejected);
      }
      step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
  }

  function __generator(thisArg, body) {
    var _ = {
        label: 0,
        sent: function() {
          if (t[0] & 1) throw t[1];
          return t[1];
        },
        trys: [],
        ops: [],
      },
      f,
      y,
      t,
      g;
    return (
      (g = { next: verb(0), throw: verb(1), return: verb(2) }),
      typeof Symbol === 'function' &&
        (g[Symbol.iterator] = function() {
          return this;
        }),
      g
    );
    function verb(n) {
      return function(v) {
        return step([n, v]);
      };
    }
    function step(op) {
      if (f) throw new TypeError('Generator is already executing.');
      while (_)
        try {
          if (
            ((f = 1),
            y &&
              (t = op[0] & 2 ? y['return'] : op[0] ? y['throw'] || ((t = y['return']) && t.call(y), 0) : y.next) &&
              !(t = t.call(y, op[1])).done)
          )
            return t;
          if (((y = 0), t)) op = [op[0] & 2, t.value];
          switch (op[0]) {
            case 0:
            case 1:
              t = op;
              break;
            case 4:
              _.label++;
              return { value: op[1], done: false };
            case 5:
              _.label++;
              y = op[1];
              op = [0];
              continue;
            case 7:
              op = _.ops.pop();
              _.trys.pop();
              continue;
            default:
              if (!((t = _.trys), (t = t.length > 0 && t[t.length - 1])) && (op[0] === 6 || op[0] === 2)) {
                _ = 0;
                continue;
              }
              if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) {
                _.label = op[1];
                break;
              }
              if (op[0] === 6 && _.label < t[1]) {
                _.label = t[1];
                t = op;
                break;
              }
              if (t && _.label < t[2]) {
                _.label = t[2];
                _.ops.push(op);
                break;
              }
              if (t[2]) _.ops.pop();
              _.trys.pop();
              continue;
          }
          op = body.call(thisArg, _);
        } catch (e) {
          op = [6, e];
          y = 0;
        } finally {
          f = t = 0;
        }
      if (op[0] & 5) throw op[1];
      return { value: op[0] ? op[1] : void 0, done: true };
    }
  }

  function __exportStar(m, exports) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
  }

  function __values(o) {
    var m = typeof Symbol === 'function' && o[Symbol.iterator],
      i = 0;
    if (m) return m.call(o);
    return {
      next: function() {
        if (o && i >= o.length) o = void 0;
        return { value: o && o[i++], done: !o };
      },
    };
  }

  function __read(o, n) {
    var m = typeof Symbol === 'function' && o[Symbol.iterator];
    if (!m) return o;
    var i = m.call(o),
      r,
      ar = [],
      e;
    try {
      while ((n === void 0 || n-- > 0) && !(r = i.next()).done) ar.push(r.value);
    } catch (error) {
      e = { error: error };
    } finally {
      try {
        if (r && !r.done && (m = i['return'])) m.call(i);
      } finally {
        if (e) throw e.error;
      }
    }
    return ar;
  }

  function __spread() {
    for (var ar = [], i = 0; i < arguments.length; i++) ar = ar.concat(__read(arguments[i]));
    return ar;
  }

  function __spreadArrays() {
    for (var s = 0, i = 0, il = arguments.length; i < il; i++) s += arguments[i].length;
    for (var r = Array(s), k = 0, i = 0; i < il; i++)
      for (var a = arguments[i], j = 0, jl = a.length; j < jl; j++, k++) r[k] = a[j];
    return r;
  }

  function __await(v) {
    return this instanceof __await ? ((this.v = v), this) : new __await(v);
  }

  function __asyncGenerator(thisArg, _arguments, generator) {
    if (!Symbol.asyncIterator) throw new TypeError('Symbol.asyncIterator is not defined.');
    var g = generator.apply(thisArg, _arguments || []),
      i,
      q = [];
    return (
      (i = {}),
      verb('next'),
      verb('throw'),
      verb('return'),
      (i[Symbol.asyncIterator] = function() {
        return this;
      }),
      i
    );
    function verb(n) {
      if (g[n])
        i[n] = function(v) {
          return new Promise(function(a, b) {
            q.push([n, v, a, b]) > 1 || resume(n, v);
          });
        };
    }
    function resume(n, v) {
      try {
        step(g[n](v));
      } catch (e) {
        settle(q[0][3], e);
      }
    }
    function step(r) {
      r.value instanceof __await ? Promise.resolve(r.value.v).then(fulfill, reject) : settle(q[0][2], r);
    }
    function fulfill(value) {
      resume('next', value);
    }
    function reject(value) {
      resume('throw', value);
    }
    function settle(f, v) {
      if ((f(v), q.shift(), q.length)) resume(q[0][0], q[0][1]);
    }
  }

  function __asyncDelegator(o) {
    var i, p;
    return (
      (i = {}),
      verb('next'),
      verb('throw', function(e) {
        throw e;
      }),
      verb('return'),
      (i[Symbol.iterator] = function() {
        return this;
      }),
      i
    );
    function verb(n, f) {
      i[n] = o[n]
        ? function(v) {
            return (p = !p) ? { value: __await(o[n](v)), done: n === 'return' } : f ? f(v) : v;
          }
        : f;
    }
  }

  function __asyncValues(o) {
    if (!Symbol.asyncIterator) throw new TypeError('Symbol.asyncIterator is not defined.');
    var m = o[Symbol.asyncIterator],
      i;
    return m
      ? m.call(o)
      : ((o = typeof __values === 'function' ? __values(o) : o[Symbol.iterator]()),
        (i = {}),
        verb('next'),
        verb('throw'),
        verb('return'),
        (i[Symbol.asyncIterator] = function() {
          return this;
        }),
        i);
    function verb(n) {
      i[n] =
        o[n] &&
        function(v) {
          return new Promise(function(resolve, reject) {
            (v = o[n](v)), settle(resolve, reject, v.done, v.value);
          });
        };
    }
    function settle(resolve, reject, d, v) {
      Promise.resolve(v).then(function(v) {
        resolve({ value: v, done: d });
      }, reject);
    }
  }

  function __makeTemplateObject(cooked, raw) {
    if (Object.defineProperty) {
      Object.defineProperty(cooked, 'raw', { value: raw });
    } else {
      cooked.raw = raw;
    }
    return cooked;
  }

  function __importStar(mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (Object.hasOwnProperty.call(mod, k)) result[k] = mod[k];
    result.default = mod;
    return result;
  }

  function __importDefault(mod) {
    return mod && mod.__esModule ? mod : { default: mod };
  }

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  var AccountLayoutComponent = /** @class */ (function() {
    function AccountLayoutComponent() {}
    // required for dynamic component
    AccountLayoutComponent.type = 'account' /* account */;
    AccountLayoutComponent.decorators = [
      {
        type: core.Component,
        args: [
          {
            selector: 'abp-layout-account',
            template:
              '\n    <router-outlet></router-outlet>\n    <abp-confirmation></abp-confirmation>\n    <abp-toast></abp-toast>\n  ',
          },
        ],
      },
    ];
    return AccountLayoutComponent;
  })();
  if (false) {
    /** @type {?} */
    AccountLayoutComponent.type;
  }

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  var AddNavigationElement = /** @class */ (function() {
    function AddNavigationElement(payload) {
      this.payload = payload;
    }
    AddNavigationElement.type = '[Layout] Add Navigation Element';
    return AddNavigationElement;
  })();
  if (false) {
    /** @type {?} */
    AddNavigationElement.type;
    /** @type {?} */
    AddNavigationElement.prototype.payload;
  }
  var RemoveNavigationElementByName = /** @class */ (function() {
    function RemoveNavigationElementByName(name) {
      this.name = name;
    }
    RemoveNavigationElementByName.type = '[Layout] Remove Navigation ElementByName';
    return RemoveNavigationElementByName;
  })();
  if (false) {
    /** @type {?} */
    RemoveNavigationElementByName.type;
    /** @type {?} */
    RemoveNavigationElementByName.prototype.name;
  }

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  var LayoutState = /** @class */ (function() {
    function LayoutState() {}
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/account-layout/account-layout.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var AccountLayoutComponent = /** @class */ (function () {
        function AccountLayoutComponent() {
        }
        // required for dynamic component
        AccountLayoutComponent.type = "account" /* account */;
        AccountLayoutComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-layout-account',
                        template: "\n    <router-outlet></router-outlet>\n    <abp-confirmation></abp-confirmation>\n    <abp-toast></abp-toast>\n  "
                    }] }
        ];
        return AccountLayoutComponent;
    }());
    if (false) {
        /** @type {?} */
        AccountLayoutComponent.type;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/actions/layout.actions.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    LayoutState.prototype.layoutAddAction
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/actions/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/states/layout.state.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    LayoutState.prototype.layoutRemoveAction
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */ = function(_a, _b) {
      var getState = _a.getState,
        patchState = _a.patchState;
      var name = _b.name;
      var navigationElements = getState().navigationElements;
      /** @type {?} */
      var index = navigationElements.findIndex(
        /**
         * @param {?} element
         * @return {?}
         */
        (function(element) {
          return element.name === name;
        }),
      );
      if (index > -1) {
        navigationElements = navigationElements.splice(index, 1);
      }
      return patchState({
        navigationElements: navigationElements,
      });
    };
    __decorate(
      [
        store.Action(AddNavigationElement),
        __metadata('design:type', Function),
        __metadata('design:paramtypes', [Object, AddNavigationElement]),
        __metadata('design:returntype', void 0),
      ],
      LayoutState.prototype,
      'layoutAddAction',
      null,
    );
    __decorate(
      [
        store.Action(RemoveNavigationElementByName),
        __metadata('design:type', Function),
        __metadata('design:paramtypes', [Object, RemoveNavigationElementByName]),
        __metadata('design:returntype', void 0),
      ],
     * @fileoverview added by tsickle
     * Generated from: lib/components/account-layout/account-layout.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var AccountLayoutComponent = /** @class */ (function () {
        function AccountLayoutComponent() {
        }
        // required for dynamic component
        AccountLayoutComponent.type = "account" /* account */;
        AccountLayoutComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-layout-account',
                        template: "\n    <router-outlet></router-outlet>\n    <abp-confirmation></abp-confirmation>\n    <abp-toast></abp-toast>\n  "
                    }] }
        ];
        return AccountLayoutComponent;
    }());
    if (false) {
        /** @type {?} */
        AccountLayoutComponent.type;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/actions/layout.actions.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
      /**
       * @param {?} _
       * @param {?} item
     * @fileoverview added by tsickle
     * Generated from: lib/actions/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/states/layout.state.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
            this.renderer = renderer;
            this.isCollapsed = true;
            this.rightPartElements = [];
            this.trackByFn = (/**
             * @param {?} _
             * @param {?} item
             * @return {?}
             */
            function (_, item) { return item.name; });
            this.trackElementByFn = (/**
             * @param {?} _
             * @param {?} element
             * @return {?}
             */
            function (_, element) { return element; });
        }
        Object.defineProperty(ApplicationLayoutComponent.prototype, "appInfo", {
            get: 
            // do not set true or false
            /**
             * @return {?}
             */
            function () {
                return this.store.selectSnapshot(ng_core.ConfigState.getApplicationInfo);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(ApplicationLayoutComponent.prototype, "visibleRoutes$", {
            get: /**
             * @return {?}
             */
            function () {
                return this.routes$.pipe(operators.map((/**
                 * @param {?} routes
                 * @return {?}
                 */
                function (routes) { return getVisibleRoutes(routes); })));
            },
          ),
        );
      },
      enumerable: true,
      configurable: true,
    });
    Object.defineProperty(ApplicationLayoutComponent.prototype, 'defaultLanguage$', {
      /**
       * @return {?}
       */
      get: function() {
        var _this = this;
        return this.languages$.pipe(
          operators.map(
            /**
             * @param {?} languages
             * @return {?}
             */
            function(languages) {
              return snq(
                /**
                 * @return {?}
                 */
                function() {
                  return languages.find(
                    /**
                     * @param {?} lang
                     * @return {?}
                     */
                    function(lang) {
                      return lang.cultureName === _this.selectedLangCulture;
                    },
                  ).displayName;
                },
              );
            },
            '',
          ),
        );
      },
      enumerable: true,
      configurable: true,
    });
    Object.defineProperty(ApplicationLayoutComponent.prototype, 'dropdownLanguages$', {
      /**
       * @return {?}
       */
      get: function() {
        var _this = this;
        return this.languages$.pipe(
          operators.map(
            /**
             * @param {?} languages
             * @return {?}
             */
            function(languages) {
              return snq(
                /**
                 * @return {?}
                 */
                function() {
                  return languages.filter(
                    /**
                     * @param {?} lang
                     * @return {?}
                     */
                    function(lang) {
                      return lang.cultureName !== _this.selectedLangCulture;
                    },
                  );
                },
              );
            },
        LayoutState.prototype.layoutRemoveAction = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var getState = _a.getState, patchState = _a.patchState;
            var name = _b.name;
            var navigationElements = getState().navigationElements;
            /** @type {?} */
            var index = navigationElements.findIndex((/**
             * @param {?} element
             * @return {?}
             */
            function (element) { return element.name === name; }));
            if (index > -1) {
                navigationElements = navigationElements.splice(index, 1);
            }
            return patchState({
                navigationElements: navigationElements,
            });
        };
        __decorate([
            store.Action(AddNavigationElement),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, AddNavigationElement]),
            __metadata("design:returntype", void 0)
        ], LayoutState.prototype, "layoutAddAction", null);
        __decorate([
            store.Action(RemoveNavigationElementByName),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, RemoveNavigationElementByName]),
            __metadata("design:returntype", void 0)
        ], LayoutState.prototype, "layoutRemoveAction", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", Array)
        ], LayoutState, "getNavigationElements", null);
        LayoutState = __decorate([
            store.State({
                name: 'LayoutState',
                defaults: (/** @type {?} */ ({ navigationElements: [] })),
            })
        ], LayoutState);
        return LayoutState;
    }());

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/states/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/application-layout/application-layout.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ApplicationLayoutComponent = /** @class */ (function () {
        function ApplicationLayoutComponent(store, oauthService, renderer) {
            this.store = store;
            this.oauthService = oauthService;
            this.renderer = renderer;
            this.isCollapsed = true;
            this.rightPartElements = [];
            this.trackByFn = (/**
             * @param {?} _
             * @param {?} item
             * @return {?}
             */
            function (_, item) { return item.name; });
            this.trackElementByFn = (/**
             * @param {?} _
             * @param {?} element
             * @return {?}
             */
            function (_, element) { return element; });
        }
        Object.defineProperty(ApplicationLayoutComponent.prototype, "appInfo", {
            get: 
            // do not set true or false
            /**
             * @return {?}
             */
            function () {
                return this.store.selectSnapshot(ng_core.ConfigState.getApplicationInfo);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(ApplicationLayoutComponent.prototype, "visibleRoutes$", {
            get: /**
             * @return {?}
             */
            function () {
                return this.routes$.pipe(operators.map((/**
                 * @param {?} routes
                 * @return {?}
                 */
                function (routes) { return getVisibleRoutes(routes); })));
         * @return {?}
         */
        function () {
            this.oauthService.logOut();
            this.store.dispatch(new routerPlugin.Navigate(['/'], null, {
                state: { redirectUrl: this.store.selectSnapshot(routerPlugin.RouterState).state.url },
            }));
            this.store.dispatch(new ng_core.GetAppConfiguration());
        };
        /**
         * @param {?} event
         * @param {?} childrenContainer
         * @return {?}
         */
        ApplicationLayoutComponent.prototype.openChange = /**
         * @param {?} event
         * @param {?} childrenContainer
         * @return {?}
         */
        function (event, childrenContainer) {
            var _this = this;
            if (!event) {
                Object.keys(childrenContainer.style)
                    .filter((/**
                 * @param {?} key
                 * @return {?}
                 */
                function (key) { return Number.isInteger(+key); }))
                    .forEach((/**
                 * @param {?} key
                 * @return {?}
                 */
                function (key) {
                    _this.renderer.removeStyle(childrenContainer, childrenContainer.style[key]);
                }));
                this.renderer.removeStyle(childrenContainer, 'left');
            }
        };
        // required for dynamic component
        ApplicationLayoutComponent.type = "application" /* application */;
        ApplicationLayoutComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-layout-application',
                        template: "<nav\r\n  class=\"navbar navbar-expand-md navbar-dark bg-dark shadow-sm flex-column flex-md-row mb-4\"\r\n  id=\"main-navbar\"\r\n  style=\"min-height: 4rem;\"\r\n>\r\n  <div class=\"container \">\r\n    <a class=\"navbar-brand\" routerLink=\"/\">\r\n      <img *ngIf=\"appInfo.logoUrl; else appName\" [src]=\"appInfo.logoUrl\" [alt]=\"appInfo.name\" />\r\n    </a>\r\n    <button\r\n      class=\"navbar-toggler\"\r\n      type=\"button\"\r\n      [attr.aria-expanded]=\"!isCollapsed\"\r\n      (click)=\"isCollapsed = !isCollapsed\"\r\n    >\r\n      <span class=\"navbar-toggler-icon\"></span>\r\n    </button>\r\n    <div class=\"navbar-collapse\" [class.overflow-hidden]=\"smallScreen\" id=\"main-navbar-collapse\">\r\n      <ng-container *ngTemplateOutlet=\"!smallScreen ? navigations : null\"></ng-container>\r\n\r\n      <div *ngIf=\"smallScreen\" [@collapseWithMargin]=\"isCollapsed ? 'collapsed' : 'expanded'\">\r\n        <ng-container *ngTemplateOutlet=\"navigations\"></ng-container>\r\n      </div>\r\n\r\n      <ng-template #navigations>\r\n        <ul class=\"navbar-nav mx-auto\">\r\n          <ng-container\r\n            *ngFor=\"let route of visibleRoutes$ | async; trackBy: trackByFn\"\r\n            [ngTemplateOutlet]=\"route?.children?.length ? dropdownLink : defaultLink\"\r\n            [ngTemplateOutletContext]=\"{ $implicit: route }\"\r\n          >\r\n          </ng-container>\r\n\r\n          <ng-template #defaultLink let-route>\r\n            <li class=\"nav-item\" [abpPermission]=\"route.requiredPolicy\">\r\n              <a class=\"nav-link\" [routerLink]=\"[route.url]\"\r\n                ><i *ngIf=\"route.iconClass\" [ngClass]=\"route.iconClass\"></i> {{ route.name | abpLocalization }}</a\r\n              >\r\n            </li>\r\n          </ng-template>\r\n\r\n          <ng-template #dropdownLink let-route>\r\n            <li\r\n              #navbarRootDropdown\r\n              [abpPermission]=\"route.requiredPolicy\"\r\n              [abpVisibility]=\"routeContainer\"\r\n              class=\"nav-item dropdown\"\r\n              display=\"static\"\r\n              (click)=\"\r\n                navbarRootDropdown.expand ? (navbarRootDropdown.expand = false) : (navbarRootDropdown.expand = true)\r\n              \"\r\n            >\r\n              <a\r\n                class=\"nav-link dropdown-toggle\"\r\n                data-toggle=\"dropdown\"\r\n                aria-haspopup=\"true\"\r\n                aria-expanded=\"false\"\r\n                href=\"javascript:void(0)\"\r\n              >\r\n                <i *ngIf=\"route.iconClass\" [ngClass]=\"route.iconClass\"></i> {{ route.name | abpLocalization }}\r\n              </a>\r\n              <div\r\n                #routeContainer\r\n                class=\"dropdown-menu border-0 shadow-sm\"\r\n                (click)=\"$event.preventDefault(); $event.stopPropagation()\"\r\n                [class.abp-collapsed-height]=\"smallScreen\"\r\n                [class.d-block]=\"smallScreen\"\r\n                [class.abp-mh-25]=\"smallScreen && navbarRootDropdown.expand\"\r\n              >\r\n                <ng-template\r\n                  #forTemplate\r\n                  ngFor\r\n                  [ngForOf]=\"route.children\"\r\n                  [ngForTrackBy]=\"trackByFn\"\r\n                  [ngForTemplate]=\"childWrapper\"\r\n                ></ng-template>\r\n              </div>\r\n            </li>\r\n          </ng-template>\r\n\r\n          <ng-template #childWrapper let-child>\r\n            <ng-template\r\n              [ngTemplateOutlet]=\"child?.children?.length ? dropdownChild : defaultChild\"\r\n              [ngTemplateOutletContext]=\"{ $implicit: child }\"\r\n            ></ng-template>\r\n          </ng-template>\r\n\r\n          <ng-template #defaultChild let-child>\r\n            <div class=\"dropdown-submenu\" [abpPermission]=\"child.requiredPolicy\">\r\n              <a class=\"dropdown-item\" [routerLink]=\"[child.url]\">\r\n                <i *ngIf=\"child.iconClass\" [ngClass]=\"child.iconClass\"></i>\r\n                {{ child.name | abpLocalization }}</a\r\n              >\r\n            </div>\r\n          </ng-template>\r\n\r\n          <ng-template #dropdownChild let-child>\r\n            <div\r\n              [abpVisibility]=\"childrenContainer\"\r\n              class=\"dropdown-submenu\"\r\n              ngbDropdown\r\n              #dropdownSubmenu=\"ngbDropdown\"\r\n              [display]=\"isDropdownChildDynamic ? 'dynamic' : 'static'\"\r\n              placement=\"right-top\"\r\n              [autoClose]=\"true\"\r\n              [abpPermission]=\"child.requiredPolicy\"\r\n              (openChange)=\"openChange($event, childrenContainer)\"\r\n            >\r\n              <div ngbDropdownToggle [class.dropdown-toggle]=\"false\">\r\n                <a\r\n                  abpEllipsis=\"210px\"\r\n                  [abpEllipsisEnabled]=\"isDropdownChildDynamic\"\r\n                  role=\"button\"\r\n                  class=\"btn d-block text-left dropdown-toggle\"\r\n                >\r\n                  <i *ngIf=\"child.iconClass\" [ngClass]=\"child.iconClass\"></i>\r\n                  {{ child.name | abpLocalization }}\r\n                </a>\r\n              </div>\r\n              <div\r\n                #childrenContainer\r\n                class=\"dropdown-menu border-0 shadow-sm\"\r\n                [class.abp-collapsed-height]=\"smallScreen\"\r\n                [class.d-block]=\"smallScreen\"\r\n                [class.abp-mh-25]=\"smallScreen && dropdownSubmenu.isOpen()\"\r\n              >\r\n                <ng-template\r\n                  ngFor\r\n                  [ngForOf]=\"child.children\"\r\n                  [ngForTrackBy]=\"trackByFn\"\r\n                  [ngForTemplate]=\"childWrapper\"\r\n                ></ng-template>\r\n              </div>\r\n            </div>\r\n          </ng-template>\r\n        </ul>\r\n\r\n        <ul class=\"navbar-nav\">\r\n          <ng-container\r\n            *ngFor=\"let element of rightPartElements; trackBy: trackElementByFn\"\r\n            [ngTemplateOutlet]=\"element\"\r\n          ></ng-container>\r\n        </ul>\r\n      </ng-template>\r\n    </div>\r\n  </div>\r\n</nav>\r\n\r\n<div [@slideFromBottom]=\"outlet && outlet.activatedRoute && outlet.activatedRoute.routeConfig.path\" class=\"container\">\r\n  <router-outlet #outlet=\"outlet\"></router-outlet>\r\n</div>\r\n\r\n<abp-confirmation></abp-confirmation>\r\n<abp-toast></abp-toast>\r\n\r\n<ng-template #appName>\r\n  {{ appInfo.name }}\r\n</ng-template>\r\n\r\n<ng-template #language>\r\n  <li class=\"nav-item\">\r\n    <div class=\"dropdown\" ngbDropdown #languageDropdown=\"ngbDropdown\" display=\"static\">\r\n      <a\r\n        ngbDropdownToggle\r\n        class=\"nav-link\"\r\n        href=\"javascript:void(0)\"\r\n        role=\"button\"\r\n        id=\"dropdownMenuLink\"\r\n        data-toggle=\"dropdown\"\r\n        aria-haspopup=\"true\"\r\n        aria-expanded=\"false\"\r\n      >\r\n        {{ defaultLanguage$ | async }}\r\n      </a>\r\n      <div\r\n        class=\"dropdown-menu dropdown-menu-right border-0 shadow-sm\"\r\n        aria-labelledby=\"dropdownMenuLink\"\r\n        [class.abp-collapsed-height]=\"smallScreen\"\r\n        [class.d-block]=\"smallScreen\"\r\n        [class.abp-mh-25]=\"smallScreen && languageDropdown.isOpen()\"\r\n      >\r\n        <a\r\n          *ngFor=\"let lang of dropdownLanguages$ | async\"\r\n          href=\"javascript:void(0)\"\r\n          class=\"dropdown-item\"\r\n          (click)=\"onChangeLang(lang.cultureName)\"\r\n          >{{ lang?.displayName }}</a\r\n        >\r\n      </div>\r\n    </div>\r\n  </li>\r\n</ng-template>\r\n\r\n<ng-template #currentUser>\r\n  <li *ngIf=\"(currentUser$ | async)?.isAuthenticated\" class=\"nav-item\">\r\n    <div ngbDropdown class=\"dropdown\" #currentUserDropdown=\"ngbDropdown\" display=\"static\">\r\n      <a\r\n        ngbDropdownToggle\r\n        class=\"nav-link\"\r\n        href=\"javascript:void(0)\"\r\n        role=\"button\"\r\n        id=\"dropdownMenuLink\"\r\n        data-toggle=\"dropdown\"\r\n        aria-haspopup=\"true\"\r\n        aria-expanded=\"false\"\r\n      >\r\n        {{ (currentUser$ | async)?.userName }}\r\n      </a>\r\n      <div\r\n        class=\"dropdown-menu dropdown-menu-right border-0 shadow-sm\"\r\n        aria-labelledby=\"dropdownMenuLink\"\r\n        [class.abp-collapsed-height]=\"smallScreen\"\r\n        [class.d-block]=\"smallScreen\"\r\n        [class.abp-mh-25]=\"smallScreen && currentUserDropdown.isOpen()\"\r\n      >\r\n        <a class=\"dropdown-item\" routerLink=\"/account/manage-profile\">{{\r\n          'AbpAccount::ManageYourProfile' | abpLocalization\r\n        }}</a>\r\n        <a class=\"dropdown-item\" href=\"javascript:void(0)\" (click)=\"logout()\">{{\r\n          'AbpUi::Logout' | abpLocalization\r\n        }}</a>\r\n      </div>\r\n    </div>\r\n  </li>\r\n</ng-template>\r\n",
                        animations: [ng_theme_shared.slideFromBottom, ng_theme_shared.collapseWithMargin]
                    }] }
        ];
        /** @nocollapse */
        ApplicationLayoutComponent.ctorParameters = function () { return [
            { type: store.Store },
            { type: angularOauth2Oidc.OAuthService },
            { type: core.Renderer2 }
        ]; };
        ApplicationLayoutComponent.propDecorators = {
            currentUserRef: [{ type: core.ViewChild, args: ['currentUser', { static: false, read: core.TemplateRef },] }],
            languageRef: [{ type: core.ViewChild, args: ['language', { static: false, read: core.TemplateRef },] }]
        };
        __decorate([
            store.Select(ng_core.ConfigState.getOne('routes')),
            __metadata("design:type", rxjs.Observable)
        ], ApplicationLayoutComponent.prototype, "routes$", void 0);
        __decorate([
            store.Select(ng_core.ConfigState.getOne('currentUser')),
            __metadata("design:type", rxjs.Observable)
        ], ApplicationLayoutComponent.prototype, "currentUser$", void 0);
        __decorate([
            store.Select(ng_core.ConfigState.getDeep('localization.languages')),
            __metadata("design:type", rxjs.Observable)
        ], ApplicationLayoutComponent.prototype, "languages$", void 0);
        __decorate([
            store.Select(LayoutState.getNavigationElements),
            __metadata("design:type", rxjs.Observable)
        ], ApplicationLayoutComponent.prototype, "navElements$", void 0);
        return ApplicationLayoutComponent;
    }());
    if (false) {
        /** @type {?} */
        ApplicationLayoutComponent.type;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.routes$;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.currentUser$;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.languages$;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.navElements$;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.currentUserRef;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.languageRef;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.isDropdownChildDynamic;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.isCollapsed;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.smallScreen;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.rightPartElements;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.trackByFn;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.trackElementByFn;
        /**
         * @type {?}
         * @private
        ApplicationLayoutComponent.prototype.checkWindowWidth = /**
         * @private
         * @return {?}
         */
        function () {
            var _this = this;
            setTimeout((/**
             * @return {?}
             */
            function () {
                if (window.innerWidth < 768) {
                    _this.isDropdownChildDynamic = false;
                    if (_this.smallScreen === false) {
                        _this.isCollapsed = false;
                        setTimeout((/**
                         * @return {?}
                         */
                        function () {
                            _this.isCollapsed = true;
                        }), 100);
                    }
                    _this.smallScreen = true;
                }
                else {
                    _this.isDropdownChildDynamic = true;
                    _this.smallScreen = false;
                }
            }), 0);
        };
      {
        type: core.Component,
        args: [
          {
            selector: 'abp-layout-application',
            template:
              '<nav\n  class="navbar navbar-expand-md navbar-dark bg-dark shadow-sm flex-column flex-md-row mb-4"\n  id="main-navbar"\n  style="min-height: 4rem;"\n>\n  <div class="container ">\n    <a class="navbar-brand" routerLink="/">\n      <img *ngIf="appInfo.logoUrl; else appName" [src]="appInfo.logoUrl" [alt]="appInfo.name" />\n    </a>\n    <button\n      class="navbar-toggler"\n      type="button"\n      [attr.aria-expanded]="!isCollapsed"\n      (click)="isCollapsed = !isCollapsed"\n    >\n      <span class="navbar-toggler-icon"></span>\n    </button>\n    <div class="navbar-collapse" id="main-navbar-collapse">\n      <ng-container *ngTemplateOutlet="!smallScreen ? navigations : null"></ng-container>\n\n      <div *ngIf="smallScreen" class="overflow-hidden" [@collapseWithMargin]="isCollapsed ? \'collapsed\' : \'expanded\'">\n        <ng-container *ngTemplateOutlet="navigations"></ng-container>\n      </div>\n\n      <ng-template #navigations>\n        <ul class="navbar-nav mx-auto">\n          <ng-container\n            *ngFor="let route of visibleRoutes$ | async; trackBy: trackByFn"\n            [ngTemplateOutlet]="route?.children?.length ? dropdownLink : defaultLink"\n            [ngTemplateOutletContext]="{ $implicit: route }"\n          >\n          </ng-container>\n\n          <ng-template #defaultLink let-route>\n            <li class="nav-item" [abpPermission]="route.requiredPolicy">\n              <a class="nav-link" [routerLink]="[route.url]">{{ route.name | abpLocalization }}</a>\n            </li>\n          </ng-template>\n\n          <ng-template #dropdownLink let-route>\n            <li\n              #navbarRootDropdown\n              [abpPermission]="route.requiredPolicy"\n              [abpVisibility]="routeContainer"\n              class="nav-item dropdown"\n              display="static"\n              (click)="\n                navbarRootDropdown.expand ? (navbarRootDropdown.expand = false) : (navbarRootDropdown.expand = true)\n              "\n            >\n              <a\n                class="nav-link dropdown-toggle"\n                data-toggle="dropdown"\n                aria-haspopup="true"\n                aria-expanded="false"\n                href="javascript:void(0)"\n              >\n                <i *ngIf="route.iconClass" [ngClass]="route.iconClass"></i> {{ route.name | abpLocalization }}\n              </a>\n              <div\n                #routeContainer\n                class="dropdown-menu border-0 shadow-sm"\n                [class.d-block]="smallScreen"\n                [class.overflow-hidden]="smallScreen"\n                (click)="$event.preventDefault(); $event.stopPropagation()"\n              >\n                <div\n                  class="abp-collapsed abp-main-nav-dropdown"\n                  [class.expanded]="smallScreen ? navbarRootDropdown.expand : true"\n                >\n                  <ng-template\n                    #forTemplate\n                    ngFor\n                    [ngForOf]="route.children"\n                    [ngForTrackBy]="trackByFn"\n                    [ngForTemplate]="childWrapper"\n                  ></ng-template>\n                </div>\n              </div>\n            </li>\n          </ng-template>\n\n          <ng-template #childWrapper let-child>\n            <ng-template\n              [ngTemplateOutlet]="child?.children?.length ? dropdownChild : defaultChild"\n              [ngTemplateOutletContext]="{ $implicit: child }"\n            ></ng-template>\n          </ng-template>\n\n          <ng-template #defaultChild let-child>\n            <div class="dropdown-submenu" [abpPermission]="child.requiredPolicy">\n              <a class="dropdown-item" [routerLink]="[child.url]">\n                <i *ngIf="child.iconClass" [ngClass]="child.iconClass"></i>\n                {{ child.name | abpLocalization }}</a\n              >\n            </div>\n          </ng-template>\n\n          <ng-template #dropdownChild let-child>\n            <div\n              [abpVisibility]="childrenContainer"\n              class="dropdown-submenu"\n              ngbDropdown\n              #dropdownSubmenu="ngbDropdown"\n              [display]="isDropdownChildDynamic ? \'dynamic\' : \'static\'"\n              placement="right-top"\n              [autoClose]="true"\n              [abpPermission]="child.requiredPolicy"\n              (openChange)="openChange($event, childrenContainer)"\n            >\n              <div ngbDropdownToggle [class.dropdown-toggle]="false">\n                <a\n                  abpEllipsis="210px"\n                  [abpEllipsisEnabled]="isDropdownChildDynamic"\n                  role="button"\n                  class="btn d-block text-left dropdown-toggle"\n                >\n                  <i *ngIf="child.iconClass" [ngClass]="child.iconClass"></i>\n                  {{ child.name | abpLocalization }}\n                </a>\n              </div>\n              <div\n                #childrenContainer\n                class="dropdown-menu border-0 shadow-sm"\n                [class.d-block]="smallScreen"\n                [class.overflow-hidden]="smallScreen"\n              >\n                <div\n                  class="abp-collapsed abp-main-nav-dropdown"\n                  [class.expanded]="smallScreen ? dropdownSubmenu.isOpen() : true"\n                >\n                  <ng-template\n                    ngFor\n                    [ngForOf]="child.children"\n                    [ngForTrackBy]="trackByFn"\n                    [ngForTemplate]="childWrapper"\n                  ></ng-template>\n                </div>\n              </div>\n            </div>\n          </ng-template>\n        </ul>\n\n        <ul class="navbar-nav">\n          <ng-container\n            *ngFor="let element of rightPartElements; trackBy: trackElementByFn"\n            [ngTemplateOutlet]="element"\n          ></ng-container>\n        </ul>\n      </ng-template>\n    </div>\n  </div>\n</nav>\n\n<div [@slideFromBottom]="outlet && outlet.activatedRoute && outlet.activatedRoute.routeConfig.path" class="container">\n  <router-outlet #outlet="outlet"></router-outlet>\n</div>\n\n<abp-confirmation></abp-confirmation>\n<abp-toast></abp-toast>\n\n<ng-template #appName>\n  {{ appInfo.name }}\n</ng-template>\n\n<ng-template #language>\n  <li class="nav-item">\n    <div class="dropdown" ngbDropdown #languageDropdown="ngbDropdown" display="static">\n      <a\n        ngbDropdownToggle\n        class="nav-link"\n        href="javascript:void(0)"\n        role="button"\n        id="dropdownMenuLink"\n        data-toggle="dropdown"\n        aria-haspopup="true"\n        aria-expanded="false"\n      >\n        {{ defaultLanguage$ | async }}\n      </a>\n      <div\n        class="dropdown-menu dropdown-menu-right border-0 shadow-sm"\n        aria-labelledby="dropdownMenuLink"\n        [class.d-block]="smallScreen"\n        [class.overflow-hidden]="smallScreen"\n      >\n        <div\n          class="abp-collapsed abp-main-nav-dropdown"\n          [class.expanded]="smallScreen ? languageDropdown.isOpen() : true"\n        >\n          <a\n            *ngFor="let lang of dropdownLanguages$ | async"\n            href="javascript:void(0)"\n            class="dropdown-item"\n            (click)="onChangeLang(lang.cultureName)"\n            >{{ lang?.displayName }}</a\n          >\n        </div>\n      </div>\n    </div>\n  </li>\n</ng-template>\n\n<ng-template #currentUser>\n  <li *ngIf="(currentUser$ | async)?.isAuthenticated" class="nav-item">\n    <div ngbDropdown class="dropdown" #currentUserDropdown="ngbDropdown" display="static">\n      <a\n        ngbDropdownToggle\n        class="nav-link"\n        href="javascript:void(0)"\n        role="button"\n        id="dropdownMenuLink"\n        data-toggle="dropdown"\n        aria-haspopup="true"\n        aria-expanded="false"\n      >\n        {{ (currentUser$ | async)?.userName }}\n      </a>\n      <div\n        class="dropdown-menu dropdown-menu-right border-0 shadow-sm"\n        aria-labelledby="dropdownMenuLink"\n        [class.overflow-hidden]="smallScreen"\n        [class.d-block]="smallScreen"\n      >\n        <div\n          class="abp-collapsed abp-main-nav-dropdown"\n          [class.expanded]="smallScreen ? currentUserDropdown.isOpen() : true"\n        >\n          <a class="dropdown-item" routerLink="/account/manage-profile">{{\n            \'AbpAccount::ManageYourProfile\' | abpLocalization\n          }}</a>\n          <a class="dropdown-item" href="javascript:void(0)" (click)="logout()">{{\n            \'AbpUi::Logout\' | abpLocalization\n          }}</a>\n        </div>\n      </div>\n    </div>\n  </li>\n</ng-template>\n',
            animations: [ng_theme_shared.slideFromBottom, ng_theme_shared.collapseWithMargin],
          },
        ],
      },
    ];
    /** @nocollapse */
    ApplicationLayoutComponent.ctorParameters = function() {
      return [{ type: store.Store }, { type: angularOauth2Oidc.OAuthService }, { type: core.Renderer2 }];
    };
    ApplicationLayoutComponent.propDecorators = {
      currentUserRef: [{ type: core.ViewChild, args: ['currentUser', { static: false, read: core.TemplateRef }] }],
      languageRef: [{ type: core.ViewChild, args: ['language', { static: false, read: core.TemplateRef }] }],
    };
    __decorate(
      [store.Select(ng_core.ConfigState.getOne('routes')), __metadata('design:type', rxjs.Observable)],
      ApplicationLayoutComponent.prototype,
      'routes$',
      void 0,
    );
    __decorate(
      [store.Select(ng_core.ConfigState.getOne('currentUser')), __metadata('design:type', rxjs.Observable)],
      ApplicationLayoutComponent.prototype,
      'currentUser$',
                function () { return (_this.rightPartElements = elements); }), 0);
            }));
            this.checkWindowWidth();
            rxjs.fromEvent(window, 'resize')
                .pipe(ng_core.takeUntilDestroy(this), operators.debounceTime(150))
                .subscribe((/**
             * @return {?}
             */
            function () {
                _this.checkWindowWidth();
            }));
        };
        /**
         * @return {?}
         */
        ApplicationLayoutComponent.prototype.ngOnDestroy = /**
         * @return {?}
         */
        function () { };
        /**
         * @param {?} cultureName
         * @return {?}
         */
        ApplicationLayoutComponent.prototype.onChangeLang = /**
         * @param {?} cultureName
         * @return {?}
         */
        function (cultureName) {
            this.store.dispatch(new ng_core.SetLanguage(cultureName));
        };
        /**
         * @return {?}
         */
        ApplicationLayoutComponent.prototype.logout = /**
         * @return {?}
         */
        function () {
            this.oauthService.logOut();
            this.store.dispatch(new routerPlugin.Navigate(['/'], null, {
                state: { redirectUrl: this.store.selectSnapshot(routerPlugin.RouterState).state.url },
            }));
            this.store.dispatch(new ng_core.GetAppConfiguration());
        };
        /**
         * @param {?} event
         * @param {?} childrenContainer
         * @return {?}
         */
        ApplicationLayoutComponent.prototype.openChange = /**
         * @param {?} event
         * @param {?} childrenContainer
         * @return {?}
         */
        function (event, childrenContainer) {
            var _this = this;
            if (!event) {
                Object.keys(childrenContainer.style)
                    .filter((/**
                 * @param {?} key
                 * @return {?}
                 */
                function (key) { return Number.isInteger(+key); }))
                    .forEach((/**
                 * @param {?} key
                 * @return {?}
                 */
                function (key) {
                    _this.renderer.removeStyle(childrenContainer, childrenContainer.style[key]);
                }));
                this.renderer.removeStyle(childrenContainer, 'left');
            }
        };
        // required for dynamic component
        ApplicationLayoutComponent.type = "application" /* application */;
        ApplicationLayoutComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-layout-application',
                        template: "<nav\r\n  class=\"navbar navbar-expand-md navbar-dark bg-dark shadow-sm flex-column flex-md-row mb-4\"\r\n  id=\"main-navbar\"\r\n  style=\"min-height: 4rem;\"\r\n>\r\n  <div class=\"container \">\r\n    <a class=\"navbar-brand\" routerLink=\"/\">\r\n      <img *ngIf=\"appInfo.logoUrl; else appName\" [src]=\"appInfo.logoUrl\" [alt]=\"appInfo.name\" />\r\n    </a>\r\n    <button\r\n      class=\"navbar-toggler\"\r\n      type=\"button\"\r\n      [attr.aria-expanded]=\"!isCollapsed\"\r\n      (click)=\"isCollapsed = !isCollapsed\"\r\n    >\r\n      <span class=\"navbar-toggler-icon\"></span>\r\n    </button>\r\n    <div class=\"navbar-collapse\" [class.overflow-hidden]=\"smallScreen\" id=\"main-navbar-collapse\">\r\n      <ng-container *ngTemplateOutlet=\"!smallScreen ? navigations : null\"></ng-container>\r\n\r\n      <div *ngIf=\"smallScreen\" [@collapseWithMargin]=\"isCollapsed ? 'collapsed' : 'expanded'\">\r\n        <ng-container *ngTemplateOutlet=\"navigations\"></ng-container>\r\n      </div>\r\n\r\n      <ng-template #navigations>\r\n        <ul class=\"navbar-nav mx-auto\">\r\n          <ng-container\r\n            *ngFor=\"let route of visibleRoutes$ | async; trackBy: trackByFn\"\r\n            [ngTemplateOutlet]=\"route?.children?.length ? dropdownLink : defaultLink\"\r\n            [ngTemplateOutletContext]=\"{ $implicit: route }\"\r\n          >\r\n          </ng-container>\r\n\r\n          <ng-template #defaultLink let-route>\r\n            <li class=\"nav-item\" [abpPermission]=\"route.requiredPolicy\">\r\n              <a class=\"nav-link\" [routerLink]=\"[route.url]\"\r\n                ><i *ngIf=\"route.iconClass\" [ngClass]=\"route.iconClass\"></i> {{ route.name | abpLocalization }}</a\r\n              >\r\n            </li>\r\n          </ng-template>\r\n\r\n          <ng-template #dropdownLink let-route>\r\n            <li\r\n              #navbarRootDropdown\r\n              [abpPermission]=\"route.requiredPolicy\"\r\n              [abpVisibility]=\"routeContainer\"\r\n              class=\"nav-item dropdown\"\r\n              display=\"static\"\r\n              (click)=\"\r\n                navbarRootDropdown.expand ? (navbarRootDropdown.expand = false) : (navbarRootDropdown.expand = true)\r\n              \"\r\n            >\r\n              <a\r\n                class=\"nav-link dropdown-toggle\"\r\n                data-toggle=\"dropdown\"\r\n                aria-haspopup=\"true\"\r\n                aria-expanded=\"false\"\r\n                href=\"javascript:void(0)\"\r\n              >\r\n                <i *ngIf=\"route.iconClass\" [ngClass]=\"route.iconClass\"></i> {{ route.name | abpLocalization }}\r\n              </a>\r\n              <div\r\n                #routeContainer\r\n                class=\"dropdown-menu border-0 shadow-sm\"\r\n                (click)=\"$event.preventDefault(); $event.stopPropagation()\"\r\n                [class.abp-collapsed-height]=\"smallScreen\"\r\n                [class.d-block]=\"smallScreen\"\r\n                [class.abp-mh-25]=\"smallScreen && navbarRootDropdown.expand\"\r\n              >\r\n                <ng-template\r\n                  #forTemplate\r\n                  ngFor\r\n                  [ngForOf]=\"route.children\"\r\n                  [ngForTrackBy]=\"trackByFn\"\r\n                  [ngForTemplate]=\"childWrapper\"\r\n                ></ng-template>\r\n              </div>\r\n            </li>\r\n          </ng-template>\r\n\r\n          <ng-template #childWrapper let-child>\r\n            <ng-template\r\n              [ngTemplateOutlet]=\"child?.children?.length ? dropdownChild : defaultChild\"\r\n              [ngTemplateOutletContext]=\"{ $implicit: child }\"\r\n            ></ng-template>\r\n          </ng-template>\r\n\r\n          <ng-template #defaultChild let-child>\r\n            <div class=\"dropdown-submenu\" [abpPermission]=\"child.requiredPolicy\">\r\n              <a class=\"dropdown-item\" [routerLink]=\"[child.url]\">\r\n                <i *ngIf=\"child.iconClass\" [ngClass]=\"child.iconClass\"></i>\r\n                {{ child.name | abpLocalization }}</a\r\n              >\r\n            </div>\r\n          </ng-template>\r\n\r\n          <ng-template #dropdownChild let-child>\r\n            <div\r\n              [abpVisibility]=\"childrenContainer\"\r\n              class=\"dropdown-submenu\"\r\n              ngbDropdown\r\n              #dropdownSubmenu=\"ngbDropdown\"\r\n              [display]=\"isDropdownChildDynamic ? 'dynamic' : 'static'\"\r\n              placement=\"right-top\"\r\n              [autoClose]=\"true\"\r\n              [abpPermission]=\"child.requiredPolicy\"\r\n              (openChange)=\"openChange($event, childrenContainer)\"\r\n            >\r\n              <div ngbDropdownToggle [class.dropdown-toggle]=\"false\">\r\n                <a\r\n                  abpEllipsis=\"210px\"\r\n                  [abpEllipsisEnabled]=\"isDropdownChildDynamic\"\r\n                  role=\"button\"\r\n                  class=\"btn d-block text-left dropdown-toggle\"\r\n                >\r\n                  <i *ngIf=\"child.iconClass\" [ngClass]=\"child.iconClass\"></i>\r\n                  {{ child.name | abpLocalization }}\r\n                </a>\r\n              </div>\r\n              <div\r\n                #childrenContainer\r\n                class=\"dropdown-menu border-0 shadow-sm\"\r\n                [class.abp-collapsed-height]=\"smallScreen\"\r\n                [class.d-block]=\"smallScreen\"\r\n                [class.abp-mh-25]=\"smallScreen && dropdownSubmenu.isOpen()\"\r\n              >\r\n                <ng-template\r\n                  ngFor\r\n                  [ngForOf]=\"child.children\"\r\n                  [ngForTrackBy]=\"trackByFn\"\r\n                  [ngForTemplate]=\"childWrapper\"\r\n                ></ng-template>\r\n              </div>\r\n            </div>\r\n          </ng-template>\r\n        </ul>\r\n\r\n        <ul class=\"navbar-nav\">\r\n          <ng-container\r\n            *ngFor=\"let element of rightPartElements; trackBy: trackElementByFn\"\r\n            [ngTemplateOutlet]=\"element\"\r\n          ></ng-container>\r\n        </ul>\r\n      </ng-template>\r\n    </div>\r\n  </div>\r\n</nav>\r\n\r\n<div [@slideFromBottom]=\"outlet && outlet.activatedRoute && outlet.activatedRoute.routeConfig.path\" class=\"container\">\r\n  <router-outlet #outlet=\"outlet\"></router-outlet>\r\n</div>\r\n\r\n<abp-confirmation></abp-confirmation>\r\n<abp-toast></abp-toast>\r\n\r\n<ng-template #appName>\r\n  {{ appInfo.name }}\r\n</ng-template>\r\n\r\n<ng-template #language>\r\n  <li class=\"nav-item\">\r\n    <div class=\"dropdown\" ngbDropdown #languageDropdown=\"ngbDropdown\" display=\"static\">\r\n      <a\r\n        ngbDropdownToggle\r\n        class=\"nav-link\"\r\n        href=\"javascript:void(0)\"\r\n        role=\"button\"\r\n        id=\"dropdownMenuLink\"\r\n        data-toggle=\"dropdown\"\r\n        aria-haspopup=\"true\"\r\n        aria-expanded=\"false\"\r\n      >\r\n        {{ defaultLanguage$ | async }}\r\n      </a>\r\n      <div\r\n        class=\"dropdown-menu dropdown-menu-right border-0 shadow-sm\"\r\n        aria-labelledby=\"dropdownMenuLink\"\r\n        [class.abp-collapsed-height]=\"smallScreen\"\r\n        [class.d-block]=\"smallScreen\"\r\n        [class.abp-mh-25]=\"smallScreen && languageDropdown.isOpen()\"\r\n      >\r\n        <a\r\n          *ngFor=\"let lang of dropdownLanguages$ | async\"\r\n          href=\"javascript:void(0)\"\r\n          class=\"dropdown-item\"\r\n          (click)=\"onChangeLang(lang.cultureName)\"\r\n          >{{ lang?.displayName }}</a\r\n        >\r\n      </div>\r\n    </div>\r\n  </li>\r\n</ng-template>\r\n\r\n<ng-template #currentUser>\r\n  <li *ngIf=\"(currentUser$ | async)?.isAuthenticated\" class=\"nav-item\">\r\n    <div ngbDropdown class=\"dropdown\" #currentUserDropdown=\"ngbDropdown\" display=\"static\">\r\n      <a\r\n        ngbDropdownToggle\r\n        class=\"nav-link\"\r\n        href=\"javascript:void(0)\"\r\n        role=\"button\"\r\n        id=\"dropdownMenuLink\"\r\n        data-toggle=\"dropdown\"\r\n        aria-haspopup=\"true\"\r\n        aria-expanded=\"false\"\r\n      >\r\n        {{ (currentUser$ | async)?.userName }}\r\n      </a>\r\n      <div\r\n        class=\"dropdown-menu dropdown-menu-right border-0 shadow-sm\"\r\n        aria-labelledby=\"dropdownMenuLink\"\r\n        [class.abp-collapsed-height]=\"smallScreen\"\r\n        [class.d-block]=\"smallScreen\"\r\n        [class.abp-mh-25]=\"smallScreen && currentUserDropdown.isOpen()\"\r\n      >\r\n        <a class=\"dropdown-item\" routerLink=\"/account/manage-profile\">{{\r\n          'AbpAccount::ManageYourProfile' | abpLocalization\r\n        }}</a>\r\n        <a class=\"dropdown-item\" href=\"javascript:void(0)\" (click)=\"logout()\">{{\r\n          'AbpUi::Logout' | abpLocalization\r\n        }}</a>\r\n      </div>\r\n    </div>\r\n  </li>\r\n</ng-template>\r\n",
                        animations: [ng_theme_shared.slideFromBottom, ng_theme_shared.collapseWithMargin]
                    }] }
        ];
        /** @nocollapse */
        ApplicationLayoutComponent.ctorParameters = function () { return [
            { type: store.Store },
            { type: angularOauth2Oidc.OAuthService },
            { type: core.Renderer2 }
        ]; };
        ApplicationLayoutComponent.propDecorators = {
            currentUserRef: [{ type: core.ViewChild, args: ['currentUser', { static: false, read: core.TemplateRef },] }],
            languageRef: [{ type: core.ViewChild, args: ['language', { static: false, read: core.TemplateRef },] }]
        };
        __decorate([
            store.Select(ng_core.ConfigState.getOne('routes')),
            __metadata("design:type", rxjs.Observable)
        ], ApplicationLayoutComponent.prototype, "routes$", void 0);
        __decorate([
            store.Select(ng_core.ConfigState.getOne('currentUser')),
            __metadata("design:type", rxjs.Observable)
        ], ApplicationLayoutComponent.prototype, "currentUser$", void 0);
        __decorate([
            store.Select(ng_core.ConfigState.getDeep('localization.languages')),
            __metadata("design:type", rxjs.Observable)
        ], ApplicationLayoutComponent.prototype, "languages$", void 0);
        __decorate([
            store.Select(LayoutState.getNavigationElements),
            __metadata("design:type", rxjs.Observable)
        ], ApplicationLayoutComponent.prototype, "navElements$", void 0);
        return ApplicationLayoutComponent;
    }());
    if (false) {
        /** @type {?} */
        ApplicationLayoutComponent.type;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.routes$;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.currentUser$;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.languages$;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.navElements$;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.currentUserRef;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.languageRef;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.isDropdownChildDynamic;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.isCollapsed;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.smallScreen;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.rightPartElements;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.trackByFn;
        /** @type {?} */
        ApplicationLayoutComponent.prototype.trackElementByFn;
        /**
         * @type {?}
         * @private
         */
        ApplicationLayoutComponent.prototype.store;
        /**
         * @type {?}
         * @private
         */
        ApplicationLayoutComponent.prototype.oauthService;
        /**
         * @type {?}
         * @private
         */
        ApplicationLayoutComponent.prototype.renderer;
    }
     * @fileoverview added by tsickle
     * Generated from: lib/components/empty-layout/empty-layout.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var EmptyLayoutComponent = /** @class */ (function () {
        function EmptyLayoutComponent() {
        }
        EmptyLayoutComponent.type = "empty" /* empty */;
        EmptyLayoutComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-layout-empty',
                        template: "\n    <router-outlet></router-outlet>\n    <abp-confirmation></abp-confirmation>\n    <abp-toast></abp-toast>\n  "
                    }] }
        ];
        return EmptyLayoutComponent;
    }());
    if (false) {
        /** @type {?} */
        EmptyLayoutComponent.type;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/validation-error/validation-error.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ValidationErrorComponent = /** @class */ (function (_super) {
        __extends(ValidationErrorComponent, _super);
        function ValidationErrorComponent() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        Object.defineProperty(ValidationErrorComponent.prototype, "abpErrors", {
            get: /**
     * @fileoverview added by tsickle
     * Generated from: lib/constants/styles.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var styles = "\n.content-header-title {\n    font-size: 24px;\n}\n\n.entry-row {\n    margin-bottom: 15px;\n}\n\n#main-navbar-tools a.dropdown-toggle {\n    text-decoration: none;\n    color: #fff;\n}\n\n.navbar .dropdown-submenu {\n    position: relative;\n}\n.navbar .dropdown-menu {\n    margin: 0;\n    padding: 0;\n}\n    .navbar .dropdown-menu a {\n        font-size: .9em;\n        padding: 10px 15px;\n        display: block;\n        min-width: 210px;\n        text-align: left;\n        border-radius: 0.25rem;\n        min-height: 44px;\n    }\n.navbar .dropdown-submenu a::after {\n    transform: rotate(-90deg);\n    position: absolute;\n    right: 16px;\n    top: 18px;\n}\n.navbar .dropdown-submenu .dropdown-menu {\n    top: 0;\n    left: 100%;\n}\n\n.card-header .btn {\n    padding: 2px 6px;\n}\n.card-header h5 {\n    margin: 0;\n}\n.container > .card {\n    box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075) !important;\n}\n\n@media screen and (min-width: 768px) {\n    .navbar .dropdown:hover > .dropdown-menu {\n        display: block;\n    }\n\n    .navbar .dropdown-submenu:hover > .dropdown-menu {\n        display: block;\n    }\n}\n.input-validation-error {\n    border-color: #dc3545;\n}\n.field-validation-error {\n    font-size: 0.8em;\n}\n";

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/services/initial.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     * @fileoverview added by tsickle
     * Generated from: lib/theme-basic.module.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
        ThemeBasicModule.decorators = [
            { type: core.NgModule, args: [{
                        declarations: __spread(LAYOUTS, [ValidationErrorComponent]),
                        imports: [
                            ng_core.CoreModule,
                            ng_theme_shared.ThemeSharedModule,
                            ngBootstrap.NgbCollapseModule,
                            ngBootstrap.NgbDropdownModule,
                            toast.ToastModule,
                            core$1.NgxValidateCoreModule,
                            store.NgxsModule.forFeature([LayoutState]),
                            core$1.NgxValidateCoreModule.forRoot({
                                targetSelector: '.form-group',
                                blueprints: {
                                    email: 'AbpAccount::ThisFieldIsNotAValidEmailAddress.',
                                    max: 'AbpAccount::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]',
                                    maxlength: 'AbpAccount::ThisFieldMustBeAStringOrArrayTypeWithAMaximumLengthoOf{0}[{{ requiredLength }}]',
                                    min: 'AbpAccount::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]',
                                    minlength: 'AbpAccount::ThisFieldMustBeAStringOrArrayTypeWithAMinimumLengthOf{0}[{{ requiredLength }}]',
                                    required: 'AbpAccount::ThisFieldIsRequired.',
                                    passwordMismatch: 'AbpIdentity::Identity.PasswordConfirmationFailed',
                                },
                                errorTemplate: ValidationErrorComponent,
                            }),
                        ],
                        exports: __spread(LAYOUTS),
                        entryComponents: __spread(LAYOUTS, [ValidationErrorComponent]),
                    },] }
        ];
        /** @nocollapse */
        ThemeBasicModule.ctorParameters = function () { return [
            { type: InitialService }
        ]; };
        return ThemeBasicModule;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        ThemeBasicModule.prototype.initialService;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/models/layout.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var Layout;
    (function (Layout) {
        /**
         * @record
         */
        function State() { }
        Layout.State = State;
        if (false) {
            /** @type {?} */
            State.prototype.navigationElements;
        }
        /**
         * @record
         */
        function NavigationElement() { }
        Layout.NavigationElement = NavigationElement;
        if (false) {
            /** @type {?} */
            NavigationElement.prototype.name;
            /** @type {?} */
            NavigationElement.prototype.element;
            /** @type {?|undefined} */
            NavigationElement.prototype.order;
        }
    })(Layout || (Layout = {}));

    exports.AccountLayoutComponent = AccountLayoutComponent;
    exports.AddNavigationElement = AddNavigationElement;
    exports.ApplicationLayoutComponent = ApplicationLayoutComponent;
    exports.EmptyLayoutComponent = EmptyLayoutComponent;
    exports.LAYOUTS = LAYOUTS;
    exports.LayoutState = LayoutState;
    exports.RemoveNavigationElementByName = RemoveNavigationElementByName;
    exports.ThemeBasicModule = ThemeBasicModule;
    exports.ValidationErrorComponent = ValidationErrorComponent;
    exports.ɵa = ApplicationLayoutComponent;
    exports.ɵb = LayoutState;
    exports.ɵc = AccountLayoutComponent;
    exports.ɵd = EmptyLayoutComponent;
    exports.ɵe = ValidationErrorComponent;
    exports.ɵf = LayoutState;
    exports.ɵg = AddNavigationElement;
    exports.ɵh = RemoveNavigationElementByName;
    exports.ɵj = InitialService;

    Object.defineProperty(exports, '__esModule', { value: true });

})));
//# sourceMappingURL=abp-ng.theme.basic.umd.js.map
