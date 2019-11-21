(function(global, factory) {
  typeof exports === 'object' && typeof module !== 'undefined'
    ? factory(
        exports,
        require('@abp/ng.core'),
        require('@abp/ng.theme.shared'),
        require('@angular/core'),
        require('@ng-bootstrap/ng-bootstrap'),
        require('@ngx-validate/core'),
        require('primeng/table'),
        require('@angular/router'),
        require('@angular/forms'),
        require('@ngxs/router-plugin'),
        require('@ngxs/store'),
        require('angular-oauth2-oidc'),
        require('rxjs'),
        require('rxjs/operators'),
        require('snq'),
        require('@angular/animations'),
      )
    : typeof define === 'function' && define.amd
    ? define('@abp/ng.account', [
        'exports',
        '@abp/ng.core',
        '@abp/ng.theme.shared',
        '@angular/core',
        '@ng-bootstrap/ng-bootstrap',
        '@ngx-validate/core',
        'primeng/table',
        '@angular/router',
        '@angular/forms',
        '@ngxs/router-plugin',
        '@ngxs/store',
        'angular-oauth2-oidc',
        'rxjs',
        'rxjs/operators',
        'snq',
        '@angular/animations',
      ], factory)
    : ((global = global || self),
      factory(
        ((global.abp = global.abp || {}), (global.abp.ng = global.abp.ng || {}), (global.abp.ng.account = {})),
        global.ng_core,
        global.ng_theme_shared,
        global.ng.core,
        global.ngBootstrap,
        global.core$1,
        global.table,
        global.ng.router,
        global.ng.forms,
        global.routerPlugin,
        global.store,
        global.angularOauth2Oidc,
        global.rxjs,
        global.rxjs.operators,
        global.snq,
        global.ng.animations,
      ));
})(this, function(
  exports,
  ng_core,
  ng_theme_shared,
  core,
  ngBootstrap,
  core$1,
  table,
  router,
  forms,
  routerPlugin,
  store,
  angularOauth2Oidc,
  rxjs,
  operators,
  snq,
  animations,
) {
  'use strict';

  snq = snq && snq.hasOwnProperty('default') ? snq['default'] : snq;

  /*! *****************************************************************************
    Copyright (c) Microsoft Corporation. All rights reserved.
    Licensed under the Apache License, Version 2.0 (the "License"); you may not use
    this file except in compliance with the License. You may obtain a copy of the
    License at http://www.apache.org/licenses/LICENSE-2.0

    THIS CODE IS PROVIDED ON AN *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
    KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED
    WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
    MERCHANTABLITY OR NON-INFRINGEMENT.

    See the Apache Version 2.0 License for specific language governing permissions
    and limitations under the License.
    ***************************************************************************** */
  /* global Reflect, Promise */

  var extendStatics = function(d, b) {
    extendStatics =
      Object.setPrototypeOf ||
      ({ __proto__: [] } instanceof Array &&
        function(d, b) {
          d.__proto__ = b;
        }) ||
      function(d, b) {
        for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
      };
    return extendStatics(d, b);
  };

  function __extends(d, b) {
    extendStatics(d, b);
    function __() {
      this.constructor = d;
    }
    d.prototype = b === null ? Object.create(b) : ((__.prototype = b.prototype), new __());
  }

  var __assign = function() {
    __assign =
      Object.assign ||
      function __assign(t) {
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
  var maxLength = forms.Validators.maxLength,
    minLength = forms.Validators.minLength,
    required = forms.Validators.required;
  var LoginComponent = /** @class */ (function() {
    function LoginComponent(fb, oauthService, store, toasterService, options) {
      this.fb = fb;
      this.oauthService = oauthService;
      this.store = store;
      this.toasterService = toasterService;
      this.options = options;
      this.oauthService.configure(this.store.selectSnapshot(ng_core.ConfigState.getOne('environment')).oAuthConfig);
      this.oauthService.loadDiscoveryDocument();
      this.form = this.fb.group({
        username: ['', [required, maxLength(255)]],
        password: ['', [required, maxLength(32)]],
        remember: [false],
      });
    }
    /**
     * @return {?}
     */
    LoginComponent.prototype.onSubmit
    /**
     * @return {?}
     */ = function() {
      var _this = this;
      if (this.form.invalid) return;
      // this.oauthService.setStorage(this.form.value.remember ? localStorage : sessionStorage);
      this.inProgress = true;
      rxjs
        .from(
          this.oauthService.fetchTokenUsingPasswordFlow(
            this.form.get('username').value,
            this.form.get('password').value,
          ),
        )
        .pipe(
          operators.switchMap(
            /**
             * @return {?}
             */
            function() {
              return _this.store.dispatch(new ng_core.GetAppConfiguration());
            },
          ),
          operators.tap(
            /**
             * @return {?}
             */
            function() {
              /** @type {?} */
              var redirectUrl =
                snq(
                  /**
                   * @return {?}
                   */
                  (function() {
                    return window.history.state;
                  }),
                ).redirectUrl ||
                (_this.options || {}).redirectUrl ||
                '/';
              _this.store.dispatch(new routerPlugin.Navigate([redirectUrl]));
            },
          ),
          operators.catchError(
            /**
             * @param {?} err
             * @return {?}
             */
            function(err) {
              _this.toasterService.error(
                snq(
                  /**
                   * @return {?}
                   */
                  function() {
                    return err.error.error_description;
                  },
                ) ||
                  snq(
                    /**
                     * @return {?}
                     */
                    function() {
                      return err.error.error.message;
                    },
                    'AbpAccount::DefaultErrorMessage',
                  ),
                'Error',
                { life: 7000 },
              );
              return rxjs.throwError(err);
            },
          ),
          operators.finalize(
            /**
             * @return {?}
             */
            function() {
              return (_this.inProgress = false);
            },
          ),
        )
        .subscribe();
    };
    LoginComponent.decorators = [
      {
        type: core.Component,
        args: [
          {
            selector: 'abp-login',
            template:
              '<abp-auth-wrapper [mainContentRef]="mainContentRef" [cancelContentRef]="cancelContentRef">\n  <ng-template #mainContentRef>\n    <h4>{{ \'AbpAccount::Login\' | abpLocalization }}</h4>\n    <strong>\n      {{ \'AbpAccount::AreYouANewUser\' | abpLocalization }}\n      <a class="text-decoration-none" routerLink="/account/register">{{ \'AbpAccount::Register\' | abpLocalization }}</a>\n    </strong>\n    <form [formGroup]="form" (ngSubmit)="onSubmit()" novalidate class="mt-4">\n      <div class="form-group">\n        <label for="login-input-user-name-or-email-address">{{\n          \'AbpAccount::UserNameOrEmailAddress\' | abpLocalization\n        }}</label>\n        <input\n          class="form-control"\n          type="text"\n          id="login-input-user-name-or-email-address"\n          formControlName="username"\n          autofocus\n        />\n      </div>\n      <div class="form-group">\n        <label for="login-input-password">{{ \'AbpAccount::Password\' | abpLocalization }}</label>\n        <input class="form-control" type="password" id="login-input-password" formControlName="password" />\n      </div>\n      <div class="form-check" validationTarget validationStyle>\n        <label class="form-check-label" for="login-input-remember-me">\n          <input class="form-check-input" type="checkbox" id="login-input-remember-me" formControlName="remember" />\n          {{ \'AbpAccount::RememberMe\' | abpLocalization }}\n        </label>\n      </div>\n      <abp-button\n        [loading]="inProgress"\n        buttonType="submit"\n        name="Action"\n        buttonClass="btn-block btn-lg mt-3 btn btn-primary"\n      >\n        {{ \'AbpAccount::Login\' | abpLocalization }}\n      </abp-button>\n    </form>\n  </ng-template>\n  <ng-template #cancelContentRef>\n    <div class="card-footer text-center border-0">\n      <a routerLink="/">\n        <button type="button" name="Action" value="Cancel" class="px-2 py-0 btn btn-link">\n          {{ \'AbpAccount::Cancel\' | abpLocalization }}\n        </button>\n      </a>\n    </div>\n  </ng-template>\n</abp-auth-wrapper>\n',
          },
        ],
      },
    ];
    /** @nocollapse */
    LoginComponent.ctorParameters = function() {
      return [
        { type: forms.FormBuilder },
        { type: angularOauth2Oidc.OAuthService },
        { type: store.Store },
        { type: ng_theme_shared.ToasterService },
        { type: undefined, decorators: [{ type: core.Optional }, { type: core.Inject, args: ['ACCOUNT_OPTIONS'] }] },
      ];
    };
    return LoginComponent;
  })();
  if (false) {
    /** @type {?} */
    LoginComponent.prototype.form;
    /** @type {?} */
    LoginComponent.prototype.inProgress;
    /**
     * @type {?}
     * @private
     */
    LoginComponent.prototype.fb;
    /**
     * @type {?}
     * @private
     */
    LoginComponent.prototype.oauthService;
    /**
     * @type {?}
     * @private
     */
    LoginComponent.prototype.store;
    /**
     * @type {?}
     * @private
     */
    LoginComponent.prototype.toasterService;
    /**
     * @type {?}
     * @private
     */
    LoginComponent.prototype.options;
  }

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  var ManageProfileComponent = /** @class */ (function() {
    function ManageProfileComponent() {
      this.selectedTab = 0;
    }
    ManageProfileComponent.decorators = [
      {
        type: core.Component,
        args: [
          {
            selector: 'abp-manage-profile',
            template:
              '<div id="AbpContentToolbar"></div>\n\n<div class="card border-0 shadow-sm">\n  <div class="card-body">\n    <div class="row">\n      <div class="col-3">\n        <ul class="nav flex-column nav-pills" id="nav-tab" role="tablist">\n          <li class="nav-item" (click)="selectedTab = 0">\n            <a class="nav-link" [ngClass]="{ active: selectedTab === 0 }" role="tab" href="javascript:void(0)">{{\n              \'AbpUi::ChangePassword\' | abpLocalization\n            }}</a>\n          </li>\n          <li class="nav-item" (click)="selectedTab = 1">\n            <a class="nav-link" [ngClass]="{ active: selectedTab === 1 }" role="tab" href="javascript:void(0)">{{\n              \'AbpAccount::PersonalSettings\' | abpLocalization\n            }}</a>\n          </li>\n        </ul>\n      </div>\n      <div class="col-9">\n        <div class="tab-content" *ngIf="selectedTab === 0" [@fadeIn]>\n          <div class="tab-pane active" role="tabpanel">\n            <h4>\n              {{ \'AbpIdentity::ChangePassword\' | abpLocalization }}\n              <hr />\n            </h4>\n            <abp-change-password-form></abp-change-password-form>\n          </div>\n        </div>\n        <div class="tab-content" *ngIf="selectedTab === 1" [@fadeIn]>\n          <div class="tab-pane active" role="tabpanel">\n            <h4>\n              {{ \'AbpIdentity::PersonalSettings\' | abpLocalization }}\n              <hr />\n            </h4>\n            <abp-personal-settings-form></abp-personal-settings-form>\n          </div>\n        </div>\n      </div>\n    </div>\n  </div>\n</div>\n',
            animations: [
              animations.trigger('fadeIn', [
                animations.transition(':enter', animations.useAnimation(ng_theme_shared.fadeIn)),
              ]),
            ],
          },
        ],
      },
    ];
    return ManageProfileComponent;
  })();
  if (false) {
    /** @type {?} */
    ManageProfileComponent.prototype.selectedTab;
  }

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  var AccountService = /** @class */ (function() {
    function AccountService(rest) {
      this.rest = rest;
    }
    /**
     * @param {?} tenantName
     * @return {?}
     */
    AccountService.prototype.findTenant
    /**
     * @param {?} tenantName
     * @return {?}
     */ = function(tenantName) {
      /** @type {?} */
      var request = {
        method: 'GET',
        url: '/api/abp/multi-tenancy/tenants/by-name/' + tenantName,
      };
      return this.rest.request(request);
    };
    /**
     * @param {?} body
     * @return {?}
     */
    AccountService.prototype.register
    /**
     * @param {?} body
     * @return {?}
     */ = function(body) {
      /** @type {?} */
      var request = {
        method: 'POST',
        url: '/api/account/register',
        body: body,
      };
      return this.rest.request(request, { skipHandleError: true });
    };
    AccountService.decorators = [
      {
        type: core.Injectable,
        args: [
          {
            providedIn: 'root',
          },
        ],
      },
    ];
    /** @nocollapse */
    AccountService.ctorParameters = function() {
      return [{ type: ng_core.RestService }];
    };
    /** @nocollapse */ AccountService.ngInjectableDef = core.ɵɵdefineInjectable({
      factory: function AccountService_Factory() {
        return new AccountService(core.ɵɵinject(ng_core.RestService));
      },
      token: AccountService,
      providedIn: 'root',
    });
    return AccountService;
  })();
  if (false) {
    /**
     * @type {?}
     * @private
     */
    AccountService.prototype.rest;
  }

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  var maxLength$1 = forms.Validators.maxLength,
    minLength$1 = forms.Validators.minLength,
    required$1 = forms.Validators.required,
    email = forms.Validators.email;
  var RegisterComponent = /** @class */ (function() {
    function RegisterComponent(fb, accountService, oauthService, store, toasterService) {
      this.fb = fb;
      this.accountService = accountService;
      this.oauthService = oauthService;
      this.store = store;
      this.toasterService = toasterService;
      this.oauthService.configure(this.store.selectSnapshot(ng_core.ConfigState.getOne('environment')).oAuthConfig);
      this.oauthService.loadDiscoveryDocument();
      this.form = this.fb.group({
        username: ['', [required$1, maxLength$1(255)]],
        password: ['', [required$1, maxLength$1(32)]],
        email: ['', [required$1, email]],
      });
    }
    /**
     * @return {?}
     */
    RegisterComponent.prototype.onSubmit
    /**
     * @return {?}
     */ = function() {
      var _this = this;
      if (this.form.invalid) return;
      this.inProgress = true;
      /** @type {?} */
      var newUser = /** @type {?} */ ({
        userName: this.form.get('username').value,
        password: this.form.get('password').value,
        emailAddress: this.form.get('email').value,
        appName: 'Angular',
      });
      this.accountService
        .register(newUser)
        .pipe(
          operators.switchMap(
            /**
             * @return {?}
             */
            function() {
              return rxjs.from(_this.oauthService.fetchTokenUsingPasswordFlow(newUser.userName, newUser.password));
            },
          ),
          operators.switchMap(
            /**
             * @return {?}
             */
            function() {
              return _this.store.dispatch(new ng_core.GetAppConfiguration());
            },
          ),
          operators.tap(
            /**
             * @return {?}
             */
            function() {
              return _this.store.dispatch(new routerPlugin.Navigate(['/']));
            },
          ),
          operators.take(1),
          operators.catchError(
            /**
             * @param {?} err
             * @return {?}
             */
            function(err) {
              _this.toasterService.error(
                snq(
                  /**
                   * @return {?}
                   */
                  function() {
                    return err.error.error_description;
                  },
                ) ||
                  snq(
                    /**
                     * @return {?}
                     */
                    function() {
                      return err.error.error.message;
                    },
                    'AbpAccount::DefaultErrorMessage',
                  ),
                'Error',
                { life: 7000 },
              );
              return rxjs.throwError(err);
            },
          ),
          operators.finalize(
            /**
             * @return {?}
             */
            function() {
              return (_this.inProgress = false);
            },
          ),
        )
        .subscribe();
    };
    RegisterComponent.decorators = [
      {
        type: core.Component,
        args: [
          {
            selector: 'abp-register',
            template:
              '<abp-auth-wrapper [mainContentRef]="mainContentRef">\n  <ng-template #mainContentRef>\n    <h4>{{ \'AbpAccount::Register\' | abpLocalization }}</h4>\n    <strong>\n      {{ \'AbpAccount::AlreadyRegistered\' | abpLocalization }}\n      <a class="text-decoration-none" routerLink="/account/login">{{ \'AbpAccount::Login\' | abpLocalization }}</a>\n    </strong>\n    <form [formGroup]="form" (ngSubmit)="onSubmit()" novalidate class="mt-4">\n      <div class="form-group">\n        <label for="input-user-name">{{ \'AbpAccount::UserName\' | abpLocalization }}</label\n        ><span> * </span\n        ><input autofocus type="text" id="input-user-name" class="form-control" formControlName="username" />\n      </div>\n      <div class="form-group">\n        <label for="input-email-address">{{ \'AbpAccount::EmailAddress\' | abpLocalization }}</label\n        ><span> * </span><input type="email" id="input-email-address" class="form-control" formControlName="email" />\n      </div>\n      <div class="form-group">\n        <label for="input-password">{{ \'AbpAccount::Password\' | abpLocalization }}</label\n        ><span> * </span><input type="password" id="input-password" class="form-control" formControlName="password" />\n      </div>\n      <abp-button\n        [loading]="inProgress"\n        buttonType="submit"\n        name="Action"\n        buttonClass="btn-block btn-lg mt-3 btn btn-primary"\n      >\n        {{ \'AbpAccount::Register\' | abpLocalization }}\n      </abp-button>\n    </form>\n  </ng-template>\n</abp-auth-wrapper>\n',
          },
        ],
      },
    ];
    /** @nocollapse */
    RegisterComponent.ctorParameters = function() {
      return [
        { type: forms.FormBuilder },
        { type: AccountService },
        { type: angularOauth2Oidc.OAuthService },
        { type: store.Store },
        { type: ng_theme_shared.ToasterService },
      ];
    };
    return RegisterComponent;
  })();
  if (false) {
    /** @type {?} */
    RegisterComponent.prototype.form;
    /** @type {?} */
    RegisterComponent.prototype.inProgress;
    /**
     * @type {?}
     * @private
     */
    RegisterComponent.prototype.fb;
    /**
     * @type {?}
     * @private
     */
    RegisterComponent.prototype.accountService;
    /**
     * @type {?}
     * @private
     */
    RegisterComponent.prototype.oauthService;
    /**
     * @type {?}
     * @private
     */
    RegisterComponent.prototype.store;
    /**
     * @type {?}
     * @private
     */
    RegisterComponent.prototype.toasterService;
  }

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  /** @type {?} */
  var routes = [
    { path: '', pathMatch: 'full', redirectTo: 'login' },
    {
      path: '',
      component: ng_core.DynamicLayoutComponent,
      children: [
        { path: 'login', component: LoginComponent },
        { path: 'register', component: RegisterComponent },
        {
          path: 'manage-profile',
          component: ManageProfileComponent,
        },
      ],
    },
  ];
  var AccountRoutingModule = /** @class */ (function() {
    function AccountRoutingModule() {}
    AccountRoutingModule.decorators = [
      {
        type: core.NgModule,
        args: [
          {
            imports: [router.RouterModule.forChild(routes)],
            exports: [router.RouterModule],
          },
        ],
      },
    ];
    return AccountRoutingModule;
  })();

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  var minLength$2 = forms.Validators.minLength,
    required$2 = forms.Validators.required;
  /** @type {?} */
  var PASSWORD_FIELDS = ['newPassword', 'repeatNewPassword'];
  var ChangePasswordComponent = /** @class */ (function() {
    function ChangePasswordComponent(fb, store, toasterService) {
      this.fb = fb;
      this.store = store;
      this.toasterService = toasterService;
      this.mapErrorsFn
      /**
       * @param {?} errors
       * @param {?} groupErrors
       * @param {?} control
       * @return {?}
       */ = function(errors, groupErrors, control) {
        if (PASSWORD_FIELDS.indexOf(control.name) < 0) return errors;
        return errors.concat(
          groupErrors.filter(
            /**
             * @param {?} __0
             * @return {?}
             */
            function(_a) {
              var key = _a.key;
              return key === 'passwordMismatch';
            },
          ),
        );
      };
    }
    /**
     * @return {?}
     */
    ChangePasswordComponent.prototype.ngOnInit
    /**
     * @return {?}
     */ = function() {
      this.form = this.fb.group(
        {
          password: ['', required$2],
          newPassword: ['', required$2],
          repeatNewPassword: ['', required$2],
        },
        {
          validators: [core$1.comparePasswords(PASSWORD_FIELDS)],
        },
      );
    };
    /**
     * @return {?}
     */
    ChangePasswordComponent.prototype.onSubmit
    /**
     * @return {?}
     */ = function() {
      var _this = this;
      if (this.form.invalid) return;
      this.inProgress = true;
      this.store
        .dispatch(
          new ng_core.ChangePassword({
            currentPassword: this.form.get('password').value,
            newPassword: this.form.get('newPassword').value,
          }),
        )
        .pipe(
          operators.finalize(
            /**
             * @return {?}
             */
            function() {
              return (_this.inProgress = false);
            },
          ),
        )
        .subscribe({
          /**
           * @return {?}
           */
          next: function() {
            _this.form.reset();
            _this.toasterService.success('AbpAccount::PasswordChangedMessage', 'Success', { life: 5000 });
          },
          /**
           * @param {?} err
           * @return {?}
           */
          error: function(err) {
            _this.toasterService.error(
              snq(
                /**
                 * @return {?}
                 */
                function() {
                  return err.error.error.message;
                },
                'AbpAccount::DefaultErrorMessage',
              ),
              'Error',
              {
                life: 7000,
              },
            );
          },
        });
    };
    ChangePasswordComponent.decorators = [
      {
        type: core.Component,
        args: [
          {
            selector: 'abp-change-password-form',
            template:
              '<form [formGroup]="form" (ngSubmit)="onSubmit()" [mapErrorsFn]="mapErrorsFn">\n  <div class="form-group">\n    <label for="current-password">{{ \'AbpIdentity::DisplayName:CurrentPassword\' | abpLocalization }}</label\n    ><span> * </span\n    ><input type="password" id="current-password" class="form-control" formControlName="password" autofocus />\n  </div>\n  <div class="form-group">\n    <label for="new-password">{{ \'AbpIdentity::DisplayName:NewPassword\' | abpLocalization }}</label\n    ><span> * </span><input type="password" id="new-password" class="form-control" formControlName="newPassword" />\n  </div>\n  <div class="form-group">\n    <label for="confirm-new-password">{{ \'AbpIdentity::DisplayName:NewPasswordConfirm\' | abpLocalization }}</label\n    ><span> * </span\n    ><input type="password" id="confirm-new-password" class="form-control" formControlName="repeatNewPassword" />\n  </div>\n  <abp-button\n    iconClass="fa fa-check"\n    buttonClass="btn btn-primary color-white"\n    buttonType="submit"\n    [loading]="inProgress"\n    >{{ \'AbpIdentity::Save\' | abpLocalization }}</abp-button\n  >\n</form>\n',
          },
        ],
      },
    ];
    /** @nocollapse */
    ChangePasswordComponent.ctorParameters = function() {
      return [{ type: forms.FormBuilder }, { type: store.Store }, { type: ng_theme_shared.ToasterService }];
    };
    return ChangePasswordComponent;
  })();
  if (false) {
    /** @type {?} */
    ChangePasswordComponent.prototype.form;
    /** @type {?} */
    ChangePasswordComponent.prototype.inProgress;
    /** @type {?} */
    ChangePasswordComponent.prototype.mapErrorsFn;
    /**
     * @type {?}
     * @private
     */
    ChangePasswordComponent.prototype.fb;
    /**
     * @type {?}
     * @private
     */
    ChangePasswordComponent.prototype.store;
    /**
     * @type {?}
     * @private
     */
    ChangePasswordComponent.prototype.toasterService;
  }

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  var maxLength$2 = forms.Validators.maxLength,
    required$3 = forms.Validators.required,
    email$1 = forms.Validators.email;
  var PersonalSettingsComponent = /** @class */ (function() {
    function PersonalSettingsComponent(fb, store, toasterService) {
      this.fb = fb;
      this.store = store;
      this.toasterService = toasterService;
    }
    /**
     * @return {?}
     */
    PersonalSettingsComponent.prototype.ngOnInit
    /**
     * @return {?}
     */ = function() {
      this.buildForm();
    };
    /**
     * @return {?}
     */
    PersonalSettingsComponent.prototype.buildForm
    /**
     * @return {?}
     */ = function() {
      var _this = this;
      this.store
        .dispatch(new ng_core.GetProfile())
        .pipe(
          operators.withLatestFrom(this.profile$),
          operators.take(1),
        )
        .subscribe(
          /**
           * @param {?} __0
           * @return {?}
           */
          function(_a) {
            var _b = __read(_a, 2),
              profile = _b[1];
            _this.form = _this.fb.group({
              userName: [profile.userName, [required$3, maxLength$2(256)]],
              email: [profile.email, [required$3, email$1, maxLength$2(256)]],
              name: [profile.name || '', [maxLength$2(64)]],
              surname: [profile.surname || '', [maxLength$2(64)]],
              phoneNumber: [profile.phoneNumber || '', [maxLength$2(16)]],
            });
          },
        );
    };
    /**
     * @return {?}
     */
    PersonalSettingsComponent.prototype.submit
    /**
     * @return {?}
     */ = function() {
      var _this = this;
      if (this.form.invalid) return;
      this.inProgress = true;
      this.store
        .dispatch(new ng_core.UpdateProfile(this.form.value))
        .pipe(
          operators.finalize(
            /**
             * @return {?}
             */
            function() {
              return (_this.inProgress = false);
            },
          ),
        )
        .subscribe(
          /**
           * @return {?}
           */
          function() {
            _this.toasterService.success('AbpAccount::PersonalSettingsSaved', 'Success', { life: 5000 });
          },
        );
    };
    PersonalSettingsComponent.decorators = [
      {
        type: core.Component,
        args: [
          {
            selector: 'abp-personal-settings-form',
            template:
              '<form novalidate *ngIf="form" [formGroup]="form" (ngSubmit)="submit()">\n  <div class="form-group">\n    <label for="username">{{ \'AbpIdentity::DisplayName:UserName\' | abpLocalization }}</label\n    ><span> * </span\n    ><input\n      type="text"\n      id="username"\n      class="form-control"\n      formControlName="userName"\n      autofocus\n      (keydown.space)="$event.preventDefault()"\n    />\n  </div>\n  <div class="row">\n    <div class="col col-md-6">\n      <div class="form-group">\n        <label for="name">{{ \'AbpIdentity::DisplayName:Name\' | abpLocalization }}</label\n        ><input type="text" id="name" class="form-control" formControlName="name" />\n      </div>\n    </div>\n    <div class="col col-md-6">\n      <div class="form-group">\n        <label for="surname">{{ \'AbpIdentity::DisplayName:Surname\' | abpLocalization }}</label\n        ><input type="text" id="surname" class="form-control" formControlName="surname" />\n      </div>\n    </div>\n  </div>\n  <div class="form-group">\n    <label for="email-address">{{ \'AbpIdentity::DisplayName:Email\' | abpLocalization }}</label\n    ><span> * </span><input type="text" id="email-address" class="form-control" formControlName="email" />\n  </div>\n  <div class="form-group">\n    <label for="phone-number">{{ \'AbpIdentity::DisplayName:PhoneNumber\' | abpLocalization }}</label\n    ><input type="text" id="phone-number" class="form-control" formControlName="phoneNumber" />\n  </div>\n  <abp-button\n    buttonType="submit"\n    iconClass="fa fa-check"\n    buttonClass="btn btn-primary color-white"\n    [loading]="inProgress"\n  >\n    {{ \'AbpIdentity::Save\' | abpLocalization }}</abp-button\n  >\n</form>\n',
          },
        ],
      },
    ];
    /** @nocollapse */
    PersonalSettingsComponent.ctorParameters = function() {
      return [{ type: forms.FormBuilder }, { type: store.Store }, { type: ng_theme_shared.ToasterService }];
    };
    __decorate(
      [store.Select(ng_core.ProfileState.getProfile), __metadata('design:type', rxjs.Observable)],
      PersonalSettingsComponent.prototype,
      'profile$',
      void 0,
    );
    return PersonalSettingsComponent;
  })();
  if (false) {
    /** @type {?} */
    PersonalSettingsComponent.prototype.profile$;
    /** @type {?} */
    PersonalSettingsComponent.prototype.form;
    /** @type {?} */
    PersonalSettingsComponent.prototype.inProgress;
    /**
     * @type {?}
     * @private
     */
    PersonalSettingsComponent.prototype.fb;
    /**
     * @type {?}
     * @private
     */
    PersonalSettingsComponent.prototype.store;
    /**
     * @type {?}
     * @private
     */
    PersonalSettingsComponent.prototype.toasterService;
  }

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  var TenantBoxComponent = /** @class */ (function() {
    function TenantBoxComponent(store, toasterService, accountService) {
      this.store = store;
      this.toasterService = toasterService;
      this.accountService = accountService;
      this.tenant = /** @type {?} */ ({});
    }
    /**
     * @return {?}
     */
    TenantBoxComponent.prototype.ngOnInit
    /**
     * @return {?}
     */ = function() {
      this.tenant = this.store.selectSnapshot(ng_core.SessionState.getTenant) || /** @type {?} */ ({});
      this.tenantName = this.tenant.name || '';
    };
    /**
     * @return {?}
     */
    TenantBoxComponent.prototype.onSwitch
    /**
     * @return {?}
     */ = function() {
      this.isModalVisible = true;
    };
    /**
     * @return {?}
     */
    TenantBoxComponent.prototype.save
    /**
     * @return {?}
     */ = function() {
      var _this = this;
      if (this.tenant.name) {
        this.accountService
          .findTenant(this.tenant.name)
          .pipe(
            operators.take(1),
            operators.catchError(
              /**
               * @param {?} err
               * @return {?}
               */
              function(err) {
                _this.toasterService.error(
                  snq(
                    /**
                     * @return {?}
                     */
                    function() {
                      return err.error.error_description;
                    },
                    'AbpUi::DefaultErrorMessage',
                  ),
                  'AbpUi::Error',
                );
                return rxjs.throwError(err);
              },
            ),
          )
          .subscribe(
            /**
             * @param {?} __0
             * @return {?}
             */
            function(_a) {
              var success = _a.success,
                tenantId = _a.tenantId;
              if (success) {
                _this.tenant = {
                  id: tenantId,
                  name: _this.tenant.name,
                };
                _this.tenantName = _this.tenant.name;
                _this.isModalVisible = false;
              } else {
                _this.toasterService.error('AbpUiMultiTenancy::GivenTenantIsNotAvailable', 'AbpUi::Error', {
                  messageLocalizationParams: [_this.tenant.name],
                });
                _this.tenant = /** @type {?} */ ({});
              }
              _this.store.dispatch(new ng_core.SetTenant(success ? _this.tenant : null));
            },
          );
      } else {
        this.store.dispatch(new ng_core.SetTenant(null));
        this.tenantName = null;
        this.isModalVisible = false;
      }
    };
    TenantBoxComponent.decorators = [
      {
        type: core.Component,
        args: [
          {
            selector: 'abp-tenant-box',
            template:
              '<div class="card shadow-sm rounded mb-3">\n  <div class="card-body px-5">\n    <div class="row">\n      <div class="col">\n        <span style="font-size: 0.8em;" class="text-uppercase text-muted">{{\n          \'AbpUiMultiTenancy::Tenant\' | abpLocalization\n        }}</span\n        ><br />\n        <h6 class="m-0 d-inline-block">\n          <span>\n            {{ tenantName || (\'AbpUiMultiTenancy::NotSelected\' | abpLocalization) }}\n          </span>\n        </h6>\n      </div>\n      <div class="col-auto">\n        <a\n          id="AbpTenantSwitchLink"\n          href="javascript:void(0);"\n          class="btn btn-sm mt-3 btn-outline-primary"\n          (click)="onSwitch()"\n          >{{ \'AbpUiMultiTenancy::Switch\' | abpLocalization }}</a\n        >\n      </div>\n    </div>\n  </div>\n</div>\n\n<abp-modal [(visible)]="isModalVisible" size="md">\n  <ng-template #abpHeader>\n    <h5>Switch Tenant</h5>\n  </ng-template>\n  <ng-template #abpBody>\n    <form (ngSubmit)="save()">\n      <div class="mt-2">\n        <div class="form-group">\n          <label for="name">{{ \'AbpUiMultiTenancy::Name\' | abpLocalization }}</label>\n          <input [(ngModel)]="tenant.name" type="text" id="name" name="tenant" class="form-control" autofocus />\n        </div>\n        <p>{{ \'AbpUiMultiTenancy::SwitchTenantHint\' | abpLocalization }}</p>\n      </div>\n    </form>\n  </ng-template>\n  <ng-template #abpFooter>\n    <button #abpClose type="button" class="btn btn-secondary">\n      {{ \'AbpTenantManagement::Cancel\' | abpLocalization }}\n    </button>\n    <button type="button" class="btn btn-primary" (click)="save()">\n      <i class="fa fa-check mr-1"></i> <span>{{ \'AbpTenantManagement::Save\' | abpLocalization }}</span>\n    </button>\n  </ng-template>\n</abp-modal>\n',
          },
        ],
      },
    ];
    /** @nocollapse */
    TenantBoxComponent.ctorParameters = function() {
      return [{ type: store.Store }, { type: ng_theme_shared.ToasterService }, { type: AccountService }];
    };
    return TenantBoxComponent;
  })();
  if (false) {
    /** @type {?} */
    TenantBoxComponent.prototype.tenant;
    /** @type {?} */
    TenantBoxComponent.prototype.tenantName;
    /** @type {?} */
    TenantBoxComponent.prototype.isModalVisible;
    /**
     * @type {?}
     * @private
     */
    TenantBoxComponent.prototype.store;
    /**
     * @type {?}
     * @private
     */
    TenantBoxComponent.prototype.toasterService;
    /**
     * @type {?}
     * @private
     */
    TenantBoxComponent.prototype.accountService;
  }

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  /**
   * @param {?} options
   * @return {?}
   */
  function optionsFactory(options) {
    return __assign({ redirectUrl: '/' }, options);
  }
  /** @type {?} */
  var ACCOUNT_OPTIONS = new core.InjectionToken('ACCOUNT_OPTIONS');

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  var AuthWrapperComponent = /** @class */ (function() {
    function AuthWrapperComponent() {}
    AuthWrapperComponent.decorators = [
      {
        type: core.Component,
        args: [
          {
            selector: 'abp-auth-wrapper',
            template:
              '<div class="row">\n  <div class="mx-auto col col-md-5">\n    <abp-tenant-box></abp-tenant-box>\n\n    <div class="abp-account-container">\n      <div class="card mt-3 shadow-sm rounded">\n        <div class="card-body p-5">\n          <ng-content *ngTemplateOutlet="mainContentRef"></ng-content>\n        </div>\n        <ng-content *ngTemplateOutlet="cancelContentRef"></ng-content>\n      </div>\n    </div>\n  </div>\n</div>\n',
          },
        ],
      },
    ];
    AuthWrapperComponent.propDecorators = {
      mainContentRef: [{ type: core.Input }],
      cancelContentRef: [{ type: core.Input }],
    };
    return AuthWrapperComponent;
  })();
  if (false) {
    /** @type {?} */
    AuthWrapperComponent.prototype.mainContentRef;
    /** @type {?} */
    AuthWrapperComponent.prototype.cancelContentRef;
  }

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  var AccountModule = /** @class */ (function() {
    function AccountModule() {}
    AccountModule.decorators = [
      {
        type: core.NgModule,
        args: [
          {
            declarations: [
              AuthWrapperComponent,
              LoginComponent,
              RegisterComponent,
              TenantBoxComponent,
              ChangePasswordComponent,
              ManageProfileComponent,
              PersonalSettingsComponent,
            ],
            imports: [
              ng_core.CoreModule,
              AccountRoutingModule,
              ng_theme_shared.ThemeSharedModule,
              table.TableModule,
              ngBootstrap.NgbDropdownModule,
              core$1.NgxValidateCoreModule,
            ],
            exports: [],
          },
        ],
      },
    ];
    return AccountModule;
  })();
  /**
   *
   * @deprecated since version 0.9
   * @param {?=} options
   * @return {?}
   */
  function AccountProviders(options) {
    if (options === void 0) {
      options = /** @type {?} */ ({});
    }
    return [
      { provide: ACCOUNT_OPTIONS, useValue: options },
      {
        provide: 'ACCOUNT_OPTIONS',
        useFactory: optionsFactory,
        deps: [ACCOUNT_OPTIONS],
      },
    ];
  }

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  /**
   *
   * @deprecated since version 0.9
   * @type {?}
   */
  var ACCOUNT_ROUTES = {
    routes: /** @type {?} */ ([
      {
        name: 'Account',
        path: 'account',
        invisible: true,
        layout: 'application' /* application */,
        children: [{ path: 'login', name: 'Login', order: 1 }, { path: 'register', name: 'Register', order: 2 }],
      },
    ]),
  };

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  /**
   * @record
   */
  function Options() {}
  if (false) {
    /** @type {?|undefined} */
    Options.prototype.redirectUrl;
  }

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  /**
   * @record
   */
  function RegisterRequest() {}
  if (false) {
    /** @type {?} */
    RegisterRequest.prototype.userName;
    /** @type {?} */
    RegisterRequest.prototype.emailAddress;
    /** @type {?} */
    RegisterRequest.prototype.password;
    /** @type {?|undefined} */
    RegisterRequest.prototype.appName;
  }
  /**
   * @record
   */
  function RegisterResponse() {}
  if (false) {
    /** @type {?} */
    RegisterResponse.prototype.tenantId;
    /** @type {?} */
    RegisterResponse.prototype.userName;
    /** @type {?} */
    RegisterResponse.prototype.name;
    /** @type {?} */
    RegisterResponse.prototype.surname;
    /** @type {?} */
    RegisterResponse.prototype.email;
    /** @type {?} */
    RegisterResponse.prototype.emailConfirmed;
    /** @type {?} */
    RegisterResponse.prototype.phoneNumber;
    /** @type {?} */
    RegisterResponse.prototype.phoneNumberConfirmed;
    /** @type {?} */
    RegisterResponse.prototype.twoFactorEnabled;
    /** @type {?} */
    RegisterResponse.prototype.lockoutEnabled;
    /** @type {?} */
    RegisterResponse.prototype.lockoutEnd;
    /** @type {?} */
    RegisterResponse.prototype.concurrencyStamp;
    /** @type {?} */
    RegisterResponse.prototype.isDeleted;
    /** @type {?} */
    RegisterResponse.prototype.deleterId;
    /** @type {?} */
    RegisterResponse.prototype.deletionTime;
    /** @type {?} */
    RegisterResponse.prototype.lastModificationTime;
    /** @type {?} */
    RegisterResponse.prototype.lastModifierId;
    /** @type {?} */
    RegisterResponse.prototype.creationTime;
    /** @type {?} */
    RegisterResponse.prototype.creatorId;
    /** @type {?} */
    RegisterResponse.prototype.id;
  }

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  /**
   * @record
   */
  function TenantIdResponse() {}
  if (false) {
    /** @type {?} */
    TenantIdResponse.prototype.success;
    /** @type {?} */
    TenantIdResponse.prototype.tenantId;
  }

  exports.ACCOUNT_OPTIONS = ACCOUNT_OPTIONS;
  exports.ACCOUNT_ROUTES = ACCOUNT_ROUTES;
  exports.AccountModule = AccountModule;
  exports.AccountProviders = AccountProviders;
  exports.ChangePasswordComponent = ChangePasswordComponent;
  exports.LoginComponent = LoginComponent;
  exports.ManageProfileComponent = ManageProfileComponent;
  exports.PersonalSettingsComponent = PersonalSettingsComponent;
  exports.RegisterComponent = RegisterComponent;
  exports.optionsFactory = optionsFactory;
  exports.ɵa = AuthWrapperComponent;
  exports.ɵb = LoginComponent;
  exports.ɵd = RegisterComponent;
  exports.ɵe = AccountService;
  exports.ɵf = TenantBoxComponent;
  exports.ɵg = ChangePasswordComponent;
  exports.ɵh = ManageProfileComponent;
  exports.ɵi = PersonalSettingsComponent;
  exports.ɵj = AccountRoutingModule;
  exports.ɵk = optionsFactory;
  exports.ɵl = ACCOUNT_OPTIONS;

  Object.defineProperty(exports, '__esModule', { value: true });
});
//# sourceMappingURL=abp-ng.account.umd.js.map
