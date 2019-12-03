(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@abp/ng.core'), require('@abp/ng.theme.shared'), require('@angular/core'), require('@ngxs/store'), require('rxjs'), require('rxjs/operators')) :
    typeof define === 'function' && define.amd ? define('@abp/ng.permission-management', ['exports', '@abp/ng.core', '@abp/ng.theme.shared', '@angular/core', '@ngxs/store', 'rxjs', 'rxjs/operators'], factory) :
    (global = global || self, factory((global.abp = global.abp || {}, global.abp.ng = global.abp.ng || {}, global.abp.ng['permission-management'] = {}), global.ng_core, global.ng_theme_shared, global.ng.core, global.store, global.rxjs, global.rxjs.operators));
}(this, (function (exports, ng_core, ng_theme_shared, core, store, rxjs, operators) { 'use strict';

    /*! *****************************************************************************
        for (var s, i = 1, n = arguments.length; i < n; i++) {
          s = arguments[i];
          for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p)) t[p] = s[p];
        }
        return t;
      };
    return __assign.apply(this, arguments);
  };

  function __rest(s, e) {
    var t = {};
    for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0) t[p] = s[p];
    if (s != null && typeof Object.getOwnPropertySymbols === 'function')
      for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) {
        if (e.indexOf(p[i]) < 0 && Object.prototype.propertyIsEnumerable.call(s, p[i])) t[p[i]] = s[p[i]];
      }
    return t;
  }

  function __decorate(decorators, target, key, desc) {
    var c = arguments.length,
      r = c < 3 ? target : desc === null ? (desc = Object.getOwnPropertyDescriptor(target, key)) : desc,
      d;
    if (typeof Reflect === 'object' && typeof Reflect.decorate === 'function')
      r = Reflect.decorate(decorators, target, key, desc);
    else
      for (var i = decorators.length - 1; i >= 0; i--)
        if ((d = decorators[i])) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
  }

  function __param(paramIndex, decorator) {
    return function(target, key) {
      decorator(target, key, paramIndex);
    };
  }

  function __metadata(metadataKey, metadataValue) {
    if (typeof Reflect === 'object' && typeof Reflect.metadata === 'function')
      return Reflect.metadata(metadataKey, metadataValue);
  }

  function __awaiter(thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function(resolve, reject) {
      function fulfilled(value) {
        try {
          step(generator.next(value));
        } catch (e) {
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
  var GetPermissions = /** @class */ (function() {
    function GetPermissions(payload) {
      this.payload = payload;
    }
    GetPermissions.type = '[PermissionManagement] Get Permissions';
    return GetPermissions;
  })();
  if (false) {
    /** @type {?} */
    GetPermissions.type;
    /** @type {?} */
    GetPermissions.prototype.payload;
  }
  var UpdatePermissions = /** @class */ (function() {
    function UpdatePermissions(payload) {
      this.payload = payload;
    }
    UpdatePermissions.type = '[PermissionManagement] Update Permissions';
    return UpdatePermissions;
  })();
  if (false) {
    /** @type {?} */
    UpdatePermissions.type;
    /** @type {?} */
    UpdatePermissions.prototype.payload;
  }

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  var PermissionManagementService = /** @class */ (function() {
    function PermissionManagementService(rest) {
      this.rest = rest;
    }
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/actions/permission-management.actions.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    PermissionManagementService.prototype.getPermissions
    /**
     * @param {?} params
     * @return {?}
     */ = function(params) {
      /** @type {?} */
      var request = {
        method: 'GET',
        url: '/api/abp/permissions',
        params: params,
      };
      return this.rest.request(request);
    };
    /**
     * @param {?} __0
     * @return {?}
     */
    PermissionManagementService.prototype.updatePermissions
    /**
     * @param {?} __0
     * @return {?}
     */ = function(_a) {
      var permissions = _a.permissions,
        providerKey = _a.providerKey,
        providerName = _a.providerName;
      /** @type {?} */
      var request = {
        method: 'PUT',
        url: '/api/abp/permissions',
        body: { permissions: permissions },
        params: { providerKey: providerKey, providerName: providerName },
      };
      return this.rest.request(request);
    };
    PermissionManagementService.decorators = [
      {
     * @fileoverview added by tsickle
     * Generated from: lib/actions/permission-management.actions.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
    /** @nocollapse */
    PermissionManagementService.ctorParameters = function() {
      return [{ type: ng_core.RestService }];
    };
    /** @nocollapse */ PermissionManagementService.ngInjectableDef = core.ɵɵdefineInjectable({
      factory: function PermissionManagementService_Factory() {
        return new PermissionManagementService(core.ɵɵinject(ng_core.RestService));
      },
      token: PermissionManagementService,
      providedIn: 'root',
    });
    return PermissionManagementService;
  })();
  if (false) {
    /**
     * @type {?}
     * @private
     */
    PermissionManagementService.prototype.rest;
  }

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  var PermissionManagementState = /** @class */ (function() {
    function PermissionManagementState(permissionManagementService) {
      this.permissionManagementService = permissionManagementService;
    }
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/services/permission-management.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    PermissionManagementState.getPermissionGroups
    /**
     * @param {?} __0
     * @return {?}
     */ = function(_a) {
      var permissionRes = _a.permissionRes;
      return permissionRes.groups || [];
    };
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/states/permission-management.state.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var PermissionManagementState = /** @class */ (function () {
        function PermissionManagementState(permissionManagementService) {
            this.permissionManagementService = permissionManagementService;
        }
        /**
         * @param {?} __0
         * @return {?}
         */
        PermissionManagementState.getPermissionGroups = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var permissionRes = _a.permissionRes;
            return permissionRes.groups || [];
        };
        /**
         * @param {?} __0
         * @return {?}
         */
        PermissionManagementState.getEntityDisplayName = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var permissionRes = _a.permissionRes;
            return permissionRes.entityDisplayName;
        };
     * @fileoverview added by tsickle
     * Generated from: lib/services/permission-management.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
         * @return {?}
         */
        function (_a, _b) {
            var patchState = _a.patchState;
            var payload = _b.payload;
            return this.permissionManagementService.getPermissions(payload).pipe(operators.tap((/**
             * @param {?} permissionResponse
             * @return {?}
             */
            function (permissionResponse) {
     * @fileoverview added by tsickle
     * Generated from: lib/states/permission-management.state.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var PermissionManagementState = /** @class */ (function () {
        function PermissionManagementState(permissionManagementService) {
            this.permissionManagementService = permissionManagementService;
        }
        /**
         * @param {?} __0
         * @return {?}
         */
        PermissionManagementState.getPermissionGroups = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var permissionRes = _a.permissionRes;
            return permissionRes.groups || [];
        };
        /**
         * @param {?} __0
         * @return {?}
         */
        PermissionManagementState.getEntityDisplayName = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var permissionRes = _a.permissionRes;
            return permissionRes.entityDisplayName;
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        PermissionManagementState.prototype.permissionManagementGet = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var patchState = _a.patchState;
            var payload = _b.payload;
            return this.permissionManagementService.getPermissions(payload).pipe(operators.tap((/**
             * @param {?} permissionResponse
             * @return {?}
             */
            function (permissionResponse) {
                return patchState({
                    permissionRes: permissionResponse,
                });
            })));
        };
        /**
         * @param {?} _
         * @param {?} __1
         * @return {?}
         */
        PermissionManagementState.prototype.permissionManagementUpdate = /**
         * @param {?} _
         * @param {?} __1
         * @return {?}
         */
        function (_, _a) {
            var payload = _a.payload;
            return this.permissionManagementService.updatePermissions(payload);
        };
        PermissionManagementState.ctorParameters = function () { return [
            { type: PermissionManagementService }
        ]; };
        __decorate([
            store.Action(GetPermissions),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, GetPermissions]),
            __metadata("design:returntype", void 0)
        ], PermissionManagementState.prototype, "permissionManagementGet", null);
        __decorate([
            store.Action(UpdatePermissions),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, UpdatePermissions]),
            __metadata("design:returntype", void 0)
        ], PermissionManagementState.prototype, "permissionManagementUpdate", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", void 0)
        ], PermissionManagementState, "getPermissionGroups", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", String)
        ], PermissionManagementState, "getEntityDisplayName", null);
        PermissionManagementState = __decorate([
            store.State({
                name: 'PermissionManagementState',
                defaults: (/** @type {?} */ ({ permissionRes: {} })),
            }),
            __metadata("design:paramtypes", [PermissionManagementService])
        ], PermissionManagementState);
        return PermissionManagementState;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        PermissionManagementState.prototype.permissionManagementService;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/permission-management.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
      ],
      PermissionManagementState,
      'getPermissionGroups',
      null,
    );
    __decorate(
      [
        store.Selector(),
        __metadata('design:type', Function),
        __metadata('design:paramtypes', [Object]),
        __metadata('design:returntype', String),
      ],
      PermissionManagementState,
      'getEntitiyDisplayName',
      null,
    );
    PermissionManagementState = __decorate(
      [
        store.State({
          name: 'PermissionManagementState',
          defaults: /** @type {?} */ ({ permissionRes: {} }),
        }),
        __metadata('design:paramtypes', [PermissionManagementService]),
      ],
      PermissionManagementState,
    );
    return PermissionManagementState;
  })();
  if (false) {
    /**
     * @type {?}
     * @private
     */
    PermissionManagementState.prototype.permissionManagementService;
  }

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  var PermissionManagementComponent = /** @class */ (function() {
    function PermissionManagementComponent(store, renderer) {
      this.store = store;
      this.renderer = renderer;
      this.visibleChange = new core.EventEmitter();
      this.permissions = [];
      this.selectThisTab = false;
      this.selectAllTab = false;
      this.modalBusy = false;
      this.trackByFn
      /**
       * @param {?} _
       * @param {?} item
       * @return {?}
       */ = function(_, item) {
        return item.name;
      };
    }
    Object.defineProperty(PermissionManagementComponent.prototype, 'visible', {
      /**
       * @return {?}
       */
      get: function() {
        return this._visible;
      },
      /**
       * @param {?} value
       * @return {?}
       */
      set: function(value) {
        if (!this.selectedGroup) return;
        this._visible = value;
        this.visibleChange.emit(value);
        if (!value) {
          this.selectedGroup = null;
        }
      },
      enumerable: true,
      configurable: true,
    });
    Object.defineProperty(PermissionManagementComponent.prototype, 'selectedGroupPermissions$', {
      /**
       * @return {?}
       */
      get: function() {
        var _this = this;
        return this.groups$.pipe(
          operators.map(
            /**
             * @param {?} groups
             * @return {?}
             */
            function(groups) {
              return _this.selectedGroup
                ? groups.find(
                    /**
                     * @param {?} group
                     * @return {?}
                     */
                    function(group) {
                      return group.name === _this.selectedGroup.name;
                    },
                  ).permissions
                : [];
            },
          ),
          operators.map(
            /**
             * @param {?} permissions
             * @return {?}
             */
            function (per) { return per.name === name; })) || { isGranted: false }).isGranted;
        };
        /**
         * @param {?} grantedProviders
         * @return {?}
         */
        PermissionManagementComponent.prototype.isGrantedByOtherProviderName = /**
         * @param {?} grantedProviders
         * @return {?}
         */
        function (grantedProviders) {
            var _this = this;
            if (grantedProviders.length) {
                return grantedProviders.findIndex((/**
                 * @param {?} p
                 * @return {?}
                 */
                function (p) { return p.providerName !== _this.providerName; })) > -1;
            }
            return false;
        };
        /**
         * @param {?} clickedPermission
         * @param {?} value
         * @return {?}
         */
        PermissionManagementComponent.prototype.onClickCheckbox = /**
         * @param {?} clickedPermission
         * @param {?} value
         * @return {?}
         */
        function (clickedPermission, value) {
            var _this = this;
            if (clickedPermission.isGranted && this.isGrantedByOtherProviderName(clickedPermission.grantedProviders))
                return;
            setTimeout((/**
             * @return {?}
             */
            function(p) {
              return p.providerName === 'Role';
            },
          ) > -1
        );
      }
      return false;
    };
    /**
     * @param {?} clickedPermission
     * @param {?} value
     * @return {?}
     */
    PermissionManagementComponent.prototype.onClickCheckbox
    /**
     * @param {?} clickedPermission
     * @param {?} value
     * @return {?}
     */ = function(clickedPermission, value) {
      var _this = this;
      if (clickedPermission.isGranted && this.isGrantedByRole(clickedPermission.grantedProviders)) return;
      setTimeout(
        /**
         * @return {?}
         */
        function() {
          _this.permissions = _this.permissions.map(
            /**
             * @param {?} per
             * @return {?}
            function (per) { return per.name === name; })) || { isGranted: false }).isGranted;
        };
        /**
         * @param {?} grantedProviders
         * @return {?}
         */
        PermissionManagementComponent.prototype.isGrantedByOtherProviderName = /**
         * @param {?} grantedProviders
         * @return {?}
         */
        function (grantedProviders) {
            var _this = this;
            if (grantedProviders.length) {
                return grantedProviders.findIndex((/**
                 * @param {?} p
                 * @return {?}
                 */
                function (p) { return p.providerName !== _this.providerName; })) > -1;
            }
            return false;
        };
        /**
         * @param {?} clickedPermission
         * @param {?} value
         * @return {?}
         */
        PermissionManagementComponent.prototype.onClickCheckbox = /**
         * @param {?} clickedPermission
         * @param {?} value
         * @return {?}
         */
        function (clickedPermission, value) {
            var _this = this;
            if (clickedPermission.isGranted && this.isGrantedByOtherProviderName(clickedPermission.grantedProviders))
                return;
            setTimeout((/**
            function (permission) { return (__assign({}, permission, { isGranted: _this.isGrantedByOtherProviderName(permission.grantedProviders) || !_this.selectAllTab })); }));
            this.selectThisTab = !this.selectAllTab;
        };
        /**
         * @param {?} group
         * @return {?}
         */
        PermissionManagementComponent.prototype.onChangeGroup = /**
         * @param {?} group
         * @return {?}
         */
        function (group) {
            this.selectedGroup = group;
            this.setTabCheckboxState();
        };
        /**
         * @return {?}
         */
        PermissionManagementComponent.prototype.submit = /**
         * @return {?}
         */
        function () {
            var _this = this;
            this.modalBusy = true;
            /** @type {?} */
            var unchangedPermissions = getPermissions(this.store.selectSnapshot(PermissionManagementState.getPermissionGroups));
            /** @type {?} */
            var changedPermissions = this.permissions
                .filter((/**
             * @param {?} per
             * @return {?}
             */
            function (per) {
                return unchangedPermissions.find((/**
                 * @param {?} unchanged
                 * @return {?}
                 */
                function (unchanged) { return unchanged.name === per.name; })).isGranted === per.isGranted ? false : true;
            }))
                .map((/**
             * @param {?} __0
             * @return {?}
             */
            function (_a) {
                var name = _a.name, isGranted = _a.isGranted;
                return ({ name: name, isGranted: isGranted });
            }));
            if (changedPermissions.length) {
                this.store
                    .dispatch(new UpdatePermissions({
                    providerKey: this.providerKey,
                    providerName: this.providerName,
                    permissions: changedPermissions,
                }))
                    .pipe(operators.finalize((/**
                 * @return {?}
                 */
                function () { return (_this.modalBusy = false); })))
                    .subscribe((/**
                 * @return {?}
                 */
                function () {
                    _this.visible = false;
                }));
            }
            else {
                this.modalBusy = false;
                this.visible = false;
            }
        };
        /**
         * @param {?} permission
         * @return {?}
         */
        PermissionManagementComponent.prototype.openModal = /**
         * @return {?}
         */
        function () {
            var _this = this;
            if (!this.providerKey || !this.providerName) {
                throw new Error('Provider Key and Provider Name are required.');
            }
            this.store
                .dispatch(new GetPermissions({
                providerKey: this.providerKey,
                providerName: this.providerName,
            }))
                .pipe(operators.pluck('PermissionManagementState', 'permissionRes'))
                .subscribe((/**
             * @param {?} permissionRes
             * @return {?}
             */
            function (permissionRes) {
                _this.selectedGroup = permissionRes.groups[0];
                _this.permissions = getPermissions(permissionRes.groups);
                _this.visible = true;
            }));
        };
        /**
         * @return {?}
         */
        PermissionManagementComponent.prototype.onClickSelectThisTab = /**
         * @return {?}
         */
        function () {
            var _this = this;
            this.selectedGroupPermissions$.pipe(operators.take(1)).subscribe((/**
             * @param {?} permissions
             * @return {?}
             */
            function (permissions) {
                permissions.forEach((/**
                 * @param {?} permission
                 * @return {?}
                 */
                function (permission) {
                    if (permission.isGranted && _this.isGrantedByOtherProviderName(permission.grantedProviders))
                        return;
                    /** @type {?} */
                    var index = _this.permissions.findIndex((/**
                     * @param {?} per
                     * @return {?}
                     */
                    function (per) { return per.name === permission.name; }));
                    _this.permissions = __spread(_this.permissions.slice(0, index), [
                        __assign({}, _this.permissions[index], { isGranted: !_this.selectThisTab })
                    ], _this.permissions.slice(index + 1));
                }));
            }));
            this.setGrantCheckboxState();
        };
        /**
         * @type {?}
         * @protected
         */
        PermissionManagementComponent.prototype._visible;
        /** @type {?} */
        PermissionManagementComponent.prototype.visibleChange;
        /** @type {?} */
        PermissionManagementComponent.prototype.groups$;
        /** @type {?} */
            function (permission) { return (__assign({}, permission, { isGranted: _this.isGrantedByOtherProviderName(permission.grantedProviders) || !_this.selectAllTab })); }));
            this.selectThisTab = !this.selectAllTab;
        };
        /**
         * @param {?} group
         * @return {?}
         */
        PermissionManagementComponent.prototype.onChangeGroup = /**
         * @param {?} group
         * @return {?}
         */
        function (group) {
            this.selectedGroup = group;
            this.setTabCheckboxState();
        };
        /**
         * @return {?}
         */
        PermissionManagementComponent.prototype.submit = /**
         * @return {?}
         */
        function () {
            var _this = this;
            this.modalBusy = true;
            /** @type {?} */
            var unchangedPermissions = getPermissions(this.store.selectSnapshot(PermissionManagementState.getPermissionGroups));
            /** @type {?} */
            var changedPermissions = this.permissions
                .filter((/**
             * @param {?} per
             * @return {?}
             */
            function (per) {
                return unchangedPermissions.find((/**
                 * @param {?} unchanged
                 * @return {?}
                 */
                function (unchanged) { return unchanged.name === per.name; })).isGranted === per.isGranted ? false : true;
            }))
                .map((/**
             * @param {?} __0
             * @return {?}
             */
            function (_a) {
                var name = _a.name, isGranted = _a.isGranted;
                return ({ name: name, isGranted: isGranted });
            }));
            if (changedPermissions.length) {
                this.store
                    .dispatch(new UpdatePermissions({
                    providerKey: this.providerKey,
                    providerName: this.providerName,
                    permissions: changedPermissions,
                }))
                    .pipe(operators.finalize((/**
                 * @return {?}
                 */
                function () { return (_this.modalBusy = false); })))
                    .subscribe((/**
                 * @return {?}
                 */
                function () {
                    _this.visible = false;
                }));
            }
            else {
                this.modalBusy = false;
                this.visible = false;
            }
        };
            return this.store.selectSnapshot(PermissionManagementState.getPermissionGroups);
        };
        /**
         * @return {?}
        PermissionManagementComponent.prototype.openModal = /**
         * @return {?}
         */
        function () {
            var _this = this;
            if (!this.providerKey || !this.providerName) {
                throw new Error('Provider Key and Provider Name are required.');
            }
            this.store
                .dispatch(new GetPermissions({
                providerKey: this.providerKey,
                providerName: this.providerName,
            }))
                .pipe(operators.pluck('PermissionManagementState', 'permissionRes'))
                .subscribe((/**
             * @param {?} permissionRes
             * @return {?}
             */
            function (permissionRes) {
                _this.selectedGroup = permissionRes.groups[0];
                _this.permissions = getPermissions(permissionRes.groups);
                _this.visible = true;
            }));
        };
        /**
         * @return {?}
         */
        PermissionManagementComponent.prototype.initModal = /**
         * @return {?}
         */
        function () {
            this.setTabCheckboxState();
            this.setGrantCheckboxState();
        };
        /**
         * @param {?} __0
         * @return {?}
         */
        PermissionManagementComponent.prototype.ngOnChanges = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var visible = _a.visible;
            if (!visible)
                return;
            if (visible.currentValue) {
                this.openModal();
            }
            else if (visible.currentValue === false && this.visible) {
                this.visible = false;
            }
        };
        PermissionManagementComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-permission-management',
                        template: "<abp-modal [(visible)]=\"visible\" (init)=\"initModal()\" [busy]=\"modalBusy\">\r\n  <ng-container *ngIf=\"{ entityName: entityName$ | async } as data\">\r\n    <ng-template #abpHeader>\r\n      <h4>{{ 'AbpPermissionManagement::Permissions' | abpLocalization }} - {{ data.entityName }}</h4>\r\n    </ng-template>\r\n    <ng-template #abpBody>\r\n      <div class=\"custom-checkbox custom-control mb-2\">\r\n        <input\r\n          type=\"checkbox\"\r\n          id=\"select-all-in-all-tabs\"\r\n          name=\"select-all-in-all-tabs\"\r\n          class=\"custom-control-input\"\r\n          [(ngModel)]=\"selectAllTab\"\r\n          (click)=\"onClickSelectAll()\"\r\n        />\r\n        <label class=\"custom-control-label\" for=\"select-all-in-all-tabs\">{{\r\n          'AbpPermissionManagement::SelectAllInAllTabs' | abpLocalization\r\n        }}</label>\r\n      </div>\r\n\r\n      <hr class=\"mt-2 mb-2\" />\r\n      <div class=\"row\">\r\n        <div class=\"col-4\">\r\n          <ul class=\"nav nav-pills flex-column\">\r\n            <li *ngFor=\"let group of groups$ | async; trackBy: trackByFn\" class=\"nav-item\">\r\n              <a\r\n                class=\"nav-link pointer\"\r\n                [class.active]=\"selectedGroup?.name === group?.name\"\r\n                (click)=\"onChangeGroup(group)\"\r\n                >{{ group?.displayName }}</a\r\n              >\r\n            </li>\r\n          </ul>\r\n        </div>\r\n        <div class=\"col-8\">\r\n          <h4>{{ selectedGroup?.displayName }}</h4>\r\n          <hr class=\"mt-2 mb-3\" />\r\n          <div class=\"pl-1 pt-1\">\r\n            <div class=\"custom-checkbox custom-control mb-2\">\r\n              <input\r\n                type=\"checkbox\"\r\n                id=\"select-all-in-this-tabs\"\r\n                name=\"select-all-in-this-tabs\"\r\n                class=\"custom-control-input\"\r\n                [(ngModel)]=\"selectThisTab\"\r\n                (click)=\"onClickSelectThisTab()\"\r\n              />\r\n              <label class=\"custom-control-label\" for=\"select-all-in-this-tabs\">{{\r\n                'AbpPermissionManagement::SelectAllInThisTab' | abpLocalization\r\n              }}</label>\r\n            </div>\r\n            <hr class=\"mb-3\" />\r\n            <div\r\n              *ngFor=\"let permission of selectedGroupPermissions$ | async; let i = index; trackBy: trackByFn\"\r\n              [style.margin-left]=\"permission.margin + 'px'\"\r\n              class=\"custom-checkbox custom-control mb-2\"\r\n            >\r\n              <input\r\n                #permissionCheckbox\r\n                type=\"checkbox\"\r\n                [checked]=\"getChecked(permission.name)\"\r\n                [value]=\"getChecked(permission.name)\"\r\n                [attr.id]=\"permission.name\"\r\n                class=\"custom-control-input\"\r\n                [disabled]=\"isGrantedByOtherProviderName(permission.grantedProviders)\"\r\n              />\r\n              <label\r\n                class=\"custom-control-label\"\r\n                [attr.for]=\"permission.name\"\r\n                (click)=\"onClickCheckbox(permission, permissionCheckbox.value)\"\r\n                >{{ permission.displayName }}\r\n                <span *ngFor=\"let provider of permission.grantedProviders\" class=\"badge badge-light\"\r\n                  >{{ provider.providerName }}: {{ provider.providerKey }}</span\r\n                ></label\r\n              >\r\n            </div>\r\n          </div>\r\n        </div>\r\n      </div>\r\n    </ng-template>\r\n    <ng-template #abpFooter>\r\n      <button type=\"button\" class=\"btn btn-secondary\" #abpClose>\r\n        {{ 'AbpIdentity::Cancel' | abpLocalization }}\r\n      </button>\r\n      <abp-button iconClass=\"fa fa-check\" (click)=\"submit()\">{{ 'AbpIdentity::Save' | abpLocalization }}</abp-button>\r\n    </ng-template>\r\n  </ng-container>\r\n</abp-modal>\r\n"
                    }] }
        ];
        /** @nocollapse */
        PermissionManagementComponent.ctorParameters = function () { return [
            { type: store.Store },
            { type: core.Renderer2 }
        ]; };
        PermissionManagementComponent.propDecorators = {
            providerName: [{ type: core.Input }],
            providerKey: [{ type: core.Input }],
            visible: [{ type: core.Input }],
            visibleChange: [{ type: core.Output }]
        };
        __decorate([
            store.Select(PermissionManagementState.getPermissionGroups),
            __metadata("design:type", rxjs.Observable)
        ], PermissionManagementComponent.prototype, "groups$", void 0);
        __decorate([
            store.Select(PermissionManagementState.getEntityDisplayName),
            __metadata("design:type", rxjs.Observable)
        ], PermissionManagementComponent.prototype, "entityName$", void 0);
        return PermissionManagementComponent;
    }());
    if (false) {
        /** @type {?} */
        PermissionManagementComponent.prototype.providerName;
        /** @type {?} */
        PermissionManagementComponent.prototype.providerKey;
        /**
         * @type {?}
         * @protected
         */
        PermissionManagementComponent.prototype._visible;
        /** @type {?} */
        PermissionManagementComponent.prototype.visibleChange;
        /** @type {?} */
        PermissionManagementComponent.prototype.groups$;
        /** @type {?} */
        PermissionManagementComponent.prototype.entityName$;
        /** @type {?} */
        PermissionManagementComponent.prototype.selectedGroup;
        /** @type {?} */
        PermissionManagementComponent.prototype.permissions;
        /** @type {?} */
        PermissionManagementComponent.prototype.selectThisTab;
        /** @type {?} */
        PermissionManagementComponent.prototype.selectAllTab;
        /** @type {?} */
        PermissionManagementComponent.prototype.modalBusy;
        /** @type {?} */
        PermissionManagementComponent.prototype.trackByFn;
        /**
         * @type {?}
         * @private
         */
        PermissionManagementComponent.prototype.store;
        /**
         * @type {?}
         * @private
         */
        PermissionManagementComponent.prototype.renderer;
    }
     * @fileoverview added by tsickle
     * Generated from: lib/permission-management.module.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     * @fileoverview added by tsickle
     * Generated from: lib/actions/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/models/permission-management.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     * @fileoverview added by tsickle
     * Generated from: lib/models/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/services/permission-management-state.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var PermissionManagementStateService = /** @class */ (function () {
        function PermissionManagementStateService(store) {
            this.store = store;
        }
        /**
         * @return {?}
         */
        PermissionManagementStateService.prototype.getPermissionGroups = /**
         * @return {?}
         */
        function () {
            return this.store.selectSnapshot(PermissionManagementState.getPermissionGroups);
        };
        /**
         * @return {?}
         */
        PermissionManagementStateService.prototype.getEntityDisplayName = /**
         * @return {?}
         */
        function () {
            return this.store.selectSnapshot(PermissionManagementState.getEntityDisplayName);
        };
        PermissionManagementStateService.decorators = [
            { type: core.Injectable, args: [{
                        providedIn: 'root',
                    },] }
        ];
        /** @nocollapse */
        PermissionManagementStateService.ctorParameters = function () { return [
            { type: store.Store }
        ]; };
        /** @nocollapse */ PermissionManagementStateService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function PermissionManagementStateService_Factory() { return new PermissionManagementStateService(core.ɵɵinject(store.Store)); }, token: PermissionManagementStateService, providedIn: "root" });
        return PermissionManagementStateService;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        PermissionManagementStateService.prototype.store;
    }

    exports.GetPermissions = GetPermissions;
    exports.PermissionManagementComponent = PermissionManagementComponent;
    exports.PermissionManagementModule = PermissionManagementModule;
    exports.PermissionManagementService = PermissionManagementService;
    exports.PermissionManagementState = PermissionManagementState;
    exports.PermissionManagementStateService = PermissionManagementStateService;
    exports.UpdatePermissions = UpdatePermissions;
    exports.ɵa = PermissionManagementComponent;
    exports.ɵb = PermissionManagementState;
    exports.ɵc = PermissionManagementService;
    exports.ɵd = GetPermissions;
    exports.ɵe = UpdatePermissions;

    Object.defineProperty(exports, '__esModule', { value: true });

})));
//# sourceMappingURL=abp-ng.permission-management.umd.js.map
