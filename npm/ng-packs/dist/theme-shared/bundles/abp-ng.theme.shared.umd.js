(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@abp/ng.core'), require('@angular/core'), require('@ngx-validate/core'), require('primeng/components/common/messageservice'), require('primeng/toast'), require('rxjs'), require('@angular/router'), require('@ngxs/store'), require('rxjs/operators'), require('@angular/animations'), require('primeng/table'), require('just-clone'), require('@angular/common/http'), require('@ngxs/router-plugin'), require('snq')) :
    typeof define === 'function' && define.amd ? define('@abp/ng.theme.shared', ['exports', '@abp/ng.core', '@angular/core', '@ngx-validate/core', 'primeng/components/common/messageservice', 'primeng/toast', 'rxjs', '@angular/router', '@ngxs/store', 'rxjs/operators', '@angular/animations', 'primeng/table', 'just-clone', '@angular/common/http', '@ngxs/router-plugin', 'snq'], factory) :
    (global = global || self, factory((global.abp = global.abp || {}, global.abp.ng = global.abp.ng || {}, global.abp.ng.theme = global.abp.ng.theme || {}, global.abp.ng.theme.shared = {}), global.ng_core, global.ng.core, global.core$1, global.messageservice, global.toast, global.rxjs, global.ng.router, global.store, global.rxjs.operators, global.ng.animations, global.table, global.clone, global.ng.common.http, global.routerPlugin, global.snq));
}(this, (function (exports, ng_core, core, core$1, messageservice, toast, rxjs, router, store, operators, animations, table, clone, http, routerPlugin, snq) { 'use strict';

    clone = clone && clone.hasOwnProperty('default') ? clone['default'] : clone;
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
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };

    function __extends(d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    }

    var __assign = function() {
        __assign = Object.assign || function __assign(t) {
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
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0)
            t[p] = s[p];
        if (s != null && typeof Object.getOwnPropertySymbols === "function")
            for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) {
                if (e.indexOf(p[i]) < 0 && Object.prototype.propertyIsEnumerable.call(s, p[i]))
                    t[p[i]] = s[p[i]];
            }
        return t;
    }

    function __decorate(decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    }

    function __param(paramIndex, decorator) {
        return function (target, key) { decorator(target, key, paramIndex); }
    }

    function __metadata(metadataKey, metadataValue) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(metadataKey, metadataValue);
    }

    function __awaiter(thisArg, _arguments, P, generator) {
        return new (P || (P = Promise))(function (resolve, reject) {
            function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
            function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
            function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
            step((generator = generator.apply(thisArg, _arguments || [])).next());
        });
    }

    function __generator(thisArg, body) {
        var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
        return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
        function verb(n) { return function (v) { return step([n, v]); }; }
        function step(op) {
            if (f) throw new TypeError("Generator is already executing.");
            while (_) try {
                if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
                if (y = 0, t) op = [op[0] & 2, t.value];
                switch (op[0]) {
                    case 0: case 1: t = op; break;
                    case 4: _.label++; return { value: op[1], done: false };
                    case 5: _.label++; y = op[1]; op = [0]; continue;
                    case 7: op = _.ops.pop(); _.trys.pop(); continue;
                    default:
                        if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                        if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                        if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                        if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                        if (t[2]) _.ops.pop();
                        _.trys.pop(); continue;
                }
                op = body.call(thisArg, _);
            } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
            if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
        }
    }

    function __exportStar(m, exports) {
        for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
    }

    function __values(o) {
        var m = typeof Symbol === "function" && o[Symbol.iterator], i = 0;
        if (m) return m.call(o);
        return {
            next: function () {
                if (o && i >= o.length) o = void 0;
                return { value: o && o[i++], done: !o };
            }
        };
    }

    function __read(o, n) {
        var m = typeof Symbol === "function" && o[Symbol.iterator];
        if (!m) return o;
        var i = m.call(o), r, ar = [], e;
        try {
            while ((n === void 0 || n-- > 0) && !(r = i.next()).done) ar.push(r.value);
        }
        catch (error) { e = { error: error }; }
        finally {
            try {
                if (r && !r.done && (m = i["return"])) m.call(i);
            }
            finally { if (e) throw e.error; }
        }
        return ar;
    }

    function __spread() {
        for (var ar = [], i = 0; i < arguments.length; i++)
            ar = ar.concat(__read(arguments[i]));
        return ar;
    }

    function __spreadArrays() {
        for (var s = 0, i = 0, il = arguments.length; i < il; i++) s += arguments[i].length;
        for (var r = Array(s), k = 0, i = 0; i < il; i++)
            for (var a = arguments[i], j = 0, jl = a.length; j < jl; j++, k++)
                r[k] = a[j];
        return r;
    };

    function __await(v) {
        return this instanceof __await ? (this.v = v, this) : new __await(v);
    }

    function __asyncGenerator(thisArg, _arguments, generator) {
        if (!Symbol.asyncIterator) throw new TypeError("Symbol.asyncIterator is not defined.");
        var g = generator.apply(thisArg, _arguments || []), i, q = [];
        return i = {}, verb("next"), verb("throw"), verb("return"), i[Symbol.asyncIterator] = function () { return this; }, i;
        function verb(n) { if (g[n]) i[n] = function (v) { return new Promise(function (a, b) { q.push([n, v, a, b]) > 1 || resume(n, v); }); }; }
        function resume(n, v) { try { step(g[n](v)); } catch (e) { settle(q[0][3], e); } }
        function step(r) { r.value instanceof __await ? Promise.resolve(r.value.v).then(fulfill, reject) : settle(q[0][2], r); }
        function fulfill(value) { resume("next", value); }
        function reject(value) { resume("throw", value); }
        function settle(f, v) { if (f(v), q.shift(), q.length) resume(q[0][0], q[0][1]); }
    }

    function __asyncDelegator(o) {
        var i, p;
        return i = {}, verb("next"), verb("throw", function (e) { throw e; }), verb("return"), i[Symbol.iterator] = function () { return this; }, i;
        function verb(n, f) { i[n] = o[n] ? function (v) { return (p = !p) ? { value: __await(o[n](v)), done: n === "return" } : f ? f(v) : v; } : f; }
    }

    function __asyncValues(o) {
        if (!Symbol.asyncIterator) throw new TypeError("Symbol.asyncIterator is not defined.");
        var m = o[Symbol.asyncIterator], i;
        return m ? m.call(o) : (o = typeof __values === "function" ? __values(o) : o[Symbol.iterator](), i = {}, verb("next"), verb("throw"), verb("return"), i[Symbol.asyncIterator] = function () { return this; }, i);
        function verb(n) { i[n] = o[n] && function (v) { return new Promise(function (resolve, reject) { v = o[n](v), settle(resolve, reject, v.done, v.value); }); }; }
        function settle(resolve, reject, d, v) { Promise.resolve(v).then(function(v) { resolve({ value: v, done: d }); }, reject); }
    }

    function __makeTemplateObject(cooked, raw) {
        if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
        return cooked;
    };

    function __importStar(mod) {
        if (mod && mod.__esModule) return mod;
        var result = {};
        if (mod != null) for (var k in mod) if (Object.hasOwnProperty.call(mod, k)) result[k] = mod[k];
        result.default = mod;
        return result;
    }

    function __importDefault(mod) {
        return (mod && mod.__esModule) ? mod : { default: mod };
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/breadcrumb/breadcrumb.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var BreadcrumbComponent = /** @class */ (function () {
        function BreadcrumbComponent(router, store) {
            this.router = router;
            this.store = store;
            this.segments = [];
        }
        /**
         * @return {?}
         */
        BreadcrumbComponent.prototype.ngOnInit = /**
         * @return {?}
         */
        function () {
            this.show = !!this.store.selectSnapshot((/**
             * @param {?} state
             * @return {?}
             */
            function (state) { return state.LeptonLayoutState; }));
            /** @type {?} */
            var splittedUrl = this.router.url.split('/').filter((/**
             * @param {?} chunk
             * @return {?}
             */
            function (chunk) { return chunk; }));
            /** @type {?} */
            var currentUrl = this.store.selectSnapshot(ng_core.ConfigState.getRoute(splittedUrl[0]));
            this.segments.push(currentUrl.name);
            if (splittedUrl.length > 1) {
                var _a = __read(splittedUrl), arr = _a.slice(1);
                /** @type {?} */
                var childRoute = currentUrl;
                var _loop_1 = function (i) {
                    /** @type {?} */
                    var element = arr[i];
                    childRoute = childRoute.children.find((/**
                     * @param {?} child
                     * @return {?}
                     */
                    function (child) { return child.path === element; }));
                    this_1.segments.push(childRoute.name);
                };
                var this_1 = this;
                for (var i = 0; i < arr.length; i++) {
                    _loop_1(i);
                }
            }
        };
        BreadcrumbComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-breadcrumb',
                        template: "<ol *ngIf=\"show\" class=\"breadcrumb\">\r\n  <li class=\"breadcrumb-item\">\r\n    <a routerLink=\"/\"><i class=\"fa fa-home\"></i> </a>\r\n  </li>\r\n  <li\r\n    *ngFor=\"let segment of segments; let last = last\"\r\n    class=\"breadcrumb-item\"\r\n    [class.active]=\"last\"\r\n    aria-current=\"page\"\r\n  >\r\n    {{ segment | abpLocalization }}\r\n  </li>\r\n</ol>\r\n"
                    }] }
        ];
        /** @nocollapse */
        BreadcrumbComponent.ctorParameters = function () { return [
            { type: router.Router },
            { type: store.Store }
        ]; };
        return BreadcrumbComponent;
    }());
    if (false) {
        /** @type {?} */
        BreadcrumbComponent.prototype.show;
        /** @type {?} */
        BreadcrumbComponent.prototype.segments;
        /**
         * @type {?}
         * @private
         */
        BreadcrumbComponent.prototype.router;
        /**
         * @type {?}
         * @private
         */
        BreadcrumbComponent.prototype.store;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/button/button.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ButtonComponent = /** @class */ (function () {
        function ButtonComponent(renderer) {
            this.renderer = renderer;
            this.buttonId = '';
            this.buttonClass = 'btn btn-primary';
            this.buttonType = 'button';
            this.loading = false;
            this.disabled = false;
            // tslint:disable-next-line: no-output-native
            this.click = new core.EventEmitter();
            // tslint:disable-next-line: no-output-native
            this.focus = new core.EventEmitter();
            // tslint:disable-next-line: no-output-native
            this.blur = new core.EventEmitter();
        }
        Object.defineProperty(ButtonComponent.prototype, "icon", {
            get: /**
             * @return {?}
             */
            function () {
                return "" + (this.loading ? 'fa fa-spinner fa-spin' : this.iconClass || 'd-none');
            },
            enumerable: true,
            configurable: true
        });
        /**
         * @return {?}
         */
        ButtonComponent.prototype.ngOnInit = /**
         * @return {?}
         */
        function () {
            var _this = this;
            if (this.attributes) {
                Object.keys(this.attributes).forEach((/**
                 * @param {?} key
                 * @return {?}
                 */
                function (key) {
                    _this.renderer.setAttribute(_this.buttonRef.nativeElement, key, _this.attributes[key]);
                }));
            }
        };
        /**
         * @param {?} event
         * @return {?}
         */
        ButtonComponent.prototype.onClick = /**
         * @param {?} event
         * @return {?}
         */
        function (event) {
            event.stopPropagation();
            this.click.next(event);
        };
        /**
         * @param {?} event
         * @return {?}
         */
        ButtonComponent.prototype.onFocus = /**
         * @param {?} event
         * @return {?}
         */
        function (event) {
            event.stopPropagation();
            this.focus.next(event);
        };
        /**
         * @param {?} event
         * @return {?}
         */
        ButtonComponent.prototype.onBlur = /**
         * @param {?} event
         * @return {?}
         */
        function (event) {
            event.stopPropagation();
            this.blur.next(event);
        };
        ButtonComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-button',
                        // tslint:disable-next-line: component-max-inline-declarations
                        template: "\n    <button\n      #button\n      [id]=\"buttonId\"\n      [attr.type]=\"buttonType\"\n      [ngClass]=\"buttonClass\"\n      [disabled]=\"loading || disabled\"\n      (click)=\"onClick($event)\"\n      (focus)=\"onFocus($event)\"\n      (blur)=\"onBlur($event)\"\n    >\n      <i [ngClass]=\"icon\" class=\"mr-1\"></i><ng-content></ng-content>\n    </button>\n  "
                    }] }
        ];
        /** @nocollapse */
        ButtonComponent.ctorParameters = function () { return [
            { type: core.Renderer2 }
        ]; };
        ButtonComponent.propDecorators = {
            buttonId: [{ type: core.Input }],
            buttonClass: [{ type: core.Input }],
            buttonType: [{ type: core.Input }],
            iconClass: [{ type: core.Input }],
            loading: [{ type: core.Input }],
            disabled: [{ type: core.Input }],
            attributes: [{ type: core.Input }],
            click: [{ type: core.Output }],
            focus: [{ type: core.Output }],
            blur: [{ type: core.Output }],
            buttonRef: [{ type: core.ViewChild, args: ['button', { static: true },] }]
        };
        return ButtonComponent;
    }());
    if (false) {
        /** @type {?} */
        ButtonComponent.prototype.buttonId;
        /** @type {?} */
        ButtonComponent.prototype.buttonClass;
        /** @type {?} */
        ButtonComponent.prototype.buttonType;
        /** @type {?} */
        ButtonComponent.prototype.iconClass;
        /** @type {?} */
        ButtonComponent.prototype.loading;
        /** @type {?} */
        ButtonComponent.prototype.disabled;
        /** @type {?} */
        ButtonComponent.prototype.attributes;
        /** @type {?} */
        ButtonComponent.prototype.click;
        /** @type {?} */
        ButtonComponent.prototype.focus;
        /** @type {?} */
        ButtonComponent.prototype.blur;
        /** @type {?} */
        ButtonComponent.prototype.buttonRef;
        /**
         * @type {?}
         * @private
         */
        ButtonComponent.prototype.renderer;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/utils/widget-utils.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @param {?} count
     * @return {?}
     */
    function getRandomBackgroundColor(count) {
        /** @type {?} */
        var colors = [];
        for (var i = 0; i < count; i++) {
            /** @type {?} */
            var r = ((i + 5) * (i + 5) * 474) % 255;
            /** @type {?} */
            var g = ((i + 5) * (i + 5) * 1600) % 255;
            /** @type {?} */
            var b = ((i + 5) * (i + 5) * 84065) % 255;
            colors.push('rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
        }
        return colors;
    }
    /** @type {?} */
    var chartJsLoaded$ = new rxjs.ReplaySubject(1);

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/chart/chart.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ChartComponent = /** @class */ (function () {
        function ChartComponent(el, cdRef) {
            var _this = this;
            this.el = el;
            this.cdRef = cdRef;
            this.options = {};
            this.plugins = [];
            this.responsive = true;
            // tslint:disable-next-line: no-output-on-prefix
            this.onDataSelect = new core.EventEmitter();
            this.initialized = new rxjs.BehaviorSubject(this);
            this.onCanvasClick = (/**
             * @param {?} event
             * @return {?}
             */
            function (event) {
                if (_this.chart) {
                    /** @type {?} */
                    var element = _this.chart.getElementAtEvent(event);
                    /** @type {?} */
                    var dataset = _this.chart.getDatasetAtEvent(event);
                    if (element && element.length && dataset) {
                        _this.onDataSelect.emit({
                            originalEvent: event,
                            element: element[0],
                            dataset: dataset,
                        });
                    }
                }
            });
            this.initChart = (/**
             * @return {?}
             */
            function () {
                /** @type {?} */
                var opts = _this.options || {};
                opts.responsive = _this.responsive;
                // allows chart to resize in responsive mode
                if (opts.responsive && (_this.height || _this.width)) {
                    opts.maintainAspectRatio = false;
                }
                _this.chart = new Chart(_this.canvas, {
                    type: _this.type,
                    data: _this.data,
                    options: _this.options,
                    plugins: _this.plugins,
                });
                _this.cdRef.detectChanges();
            });
            this.generateLegend = (/**
             * @return {?}
             */
            function () {
                if (_this.chart) {
                    return _this.chart.generateLegend();
                }
            });
            this.refresh = (/**
             * @return {?}
             */
            function () {
                if (_this.chart) {
                    _this.chart.update();
                    _this.cdRef.detectChanges();
                }
            });
            this.reinit = (/**
             * @return {?}
             */
            function () {
                if (_this.chart) {
                    _this.chart.destroy();
                    _this.initChart();
                }
            });
        }
        Object.defineProperty(ChartComponent.prototype, "data", {
            get: /**
             * @return {?}
             */
            function () {
                return this._data;
            },
            set: /**
             * @param {?} val
             * @return {?}
             */
            function (val) {
                this._data = val;
                this.reinit();
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(ChartComponent.prototype, "canvas", {
            get: /**
             * @return {?}
             */
            function () {
                return this.el.nativeElement.children[0].children[0];
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(ChartComponent.prototype, "base64Image", {
            get: /**
             * @return {?}
             */
            function () {
                return this.chart.toBase64Image();
            },
            enumerable: true,
            configurable: true
        });
        /**
         * @return {?}
         */
        ChartComponent.prototype.ngAfterViewInit = /**
         * @return {?}
         */
        function () {
            var _this = this;
            chartJsLoaded$.subscribe((/**
             * @return {?}
             */
            function () {
                _this.testChartJs();
                _this.initChart();
                _this._initialized = true;
            }));
        };
        /**
         * @return {?}
         */
        ChartComponent.prototype.testChartJs = /**
         * @return {?}
         */
        function () {
            try {
                // tslint:disable-next-line: no-unused-expression
                Chart;
            }
            catch (error) {
                throw new Error("Chart is not found. Import the Chart from app.module like shown below:\n      import('chart.js');\n      ");
            }
        };
        /**
         * @return {?}
         */
        ChartComponent.prototype.ngOnDestroy = /**
         * @return {?}
         */
        function () {
            if (this.chart) {
                this.chart.destroy();
                this._initialized = false;
                this.chart = null;
            }
        };
        ChartComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-chart',
                        template: "<div\r\n  style=\"position:relative\"\r\n  [style.width]=\"responsive && !width ? null : width\"\r\n  [style.height]=\"responsive && !height ? null : height\"\r\n>\r\n  <canvas\r\n    [attr.width]=\"responsive && !width ? null : width\"\r\n    [attr.height]=\"responsive && !height ? null : height\"\r\n    (click)=\"onCanvasClick($event)\"\r\n  ></canvas>\r\n</div>\r\n"
                    }] }
        ];
        /** @nocollapse */
        ChartComponent.ctorParameters = function () { return [
            { type: core.ElementRef },
            { type: core.ChangeDetectorRef }
        ]; };
        ChartComponent.propDecorators = {
            type: [{ type: core.Input }],
            options: [{ type: core.Input }],
            plugins: [{ type: core.Input }],
            width: [{ type: core.Input }],
            height: [{ type: core.Input }],
            responsive: [{ type: core.Input }],
            onDataSelect: [{ type: core.Output }],
            initialized: [{ type: core.Output }],
            data: [{ type: core.Input }]
        };
        return ChartComponent;
    }());
    if (false) {
        /** @type {?} */
        ChartComponent.prototype.type;
        /** @type {?} */
        ChartComponent.prototype.options;
        /** @type {?} */
        ChartComponent.prototype.plugins;
        /** @type {?} */
        ChartComponent.prototype.width;
        /** @type {?} */
        ChartComponent.prototype.height;
        /** @type {?} */
        ChartComponent.prototype.responsive;
        /** @type {?} */
        ChartComponent.prototype.onDataSelect;
        /** @type {?} */
        ChartComponent.prototype.initialized;
        /**
         * @type {?}
         * @private
         */
        ChartComponent.prototype._initialized;
        /** @type {?} */
        ChartComponent.prototype._data;
        /** @type {?} */
        ChartComponent.prototype.chart;
        /** @type {?} */
        ChartComponent.prototype.onCanvasClick;
        /** @type {?} */
        ChartComponent.prototype.initChart;
        /** @type {?} */
        ChartComponent.prototype.generateLegend;
        /** @type {?} */
        ChartComponent.prototype.refresh;
        /** @type {?} */
        ChartComponent.prototype.reinit;
        /** @type {?} */
        ChartComponent.prototype.el;
        /**
         * @type {?}
         * @private
         */
        ChartComponent.prototype.cdRef;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/abstracts/toaster.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @abstract
     * @template T
     */
    var   /**
     * @abstract
     * @template T
     */
    AbstractToaster = /** @class */ (function () {
        function AbstractToaster(messageService) {
            this.messageService = messageService;
            this.key = 'abpToast';
            this.sticky = false;
        }
        /**
         * @param {?} message
         * @param {?} title
         * @param {?=} options
         * @return {?}
         */
        AbstractToaster.prototype.info = /**
         * @param {?} message
         * @param {?} title
         * @param {?=} options
         * @return {?}
         */
        function (message, title, options) {
            return this.show(message, title, 'info', options);
        };
        /**
         * @param {?} message
         * @param {?} title
         * @param {?=} options
         * @return {?}
         */
        AbstractToaster.prototype.success = /**
         * @param {?} message
         * @param {?} title
         * @param {?=} options
         * @return {?}
         */
        function (message, title, options) {
            return this.show(message, title, 'success', options);
        };
        /**
         * @param {?} message
         * @param {?} title
         * @param {?=} options
         * @return {?}
         */
        AbstractToaster.prototype.warn = /**
         * @param {?} message
         * @param {?} title
         * @param {?=} options
         * @return {?}
         */
        function (message, title, options) {
            return this.show(message, title, 'warn', options);
        };
        /**
         * @param {?} message
         * @param {?} title
         * @param {?=} options
         * @return {?}
         */
        AbstractToaster.prototype.error = /**
         * @param {?} message
         * @param {?} title
         * @param {?=} options
         * @return {?}
         */
        function (message, title, options) {
            return this.show(message, title, 'error', options);
        };
        /**
         * @protected
         * @param {?} message
         * @param {?} title
         * @param {?} severity
         * @param {?=} options
         * @return {?}
         */
        AbstractToaster.prototype.show = /**
         * @protected
         * @param {?} message
         * @param {?} title
         * @param {?} severity
         * @param {?=} options
         * @return {?}
         */
        function (message, title, severity, options) {
            this.messageService.clear(this.key);
            this.messageService.add(__assign({ severity: severity, detail: message || '', summary: title || '' }, options, { key: this.key }, (typeof (options || ((/** @type {?} */ ({})))).sticky === 'undefined' && { sticky: this.sticky })));
            this.status$ = new rxjs.Subject();
            return this.status$;
        };
        /**
         * @param {?=} status
         * @return {?}
         */
        AbstractToaster.prototype.clear = /**
         * @param {?=} status
         * @return {?}
         */
        function (status) {
            this.messageService.clear(this.key);
            this.status$.next(status || "dismiss" /* dismiss */);
            this.status$.complete();
        };
        return AbstractToaster;
    }());
    if (false) {
        /** @type {?} */
        AbstractToaster.prototype.status$;
        /** @type {?} */
        AbstractToaster.prototype.key;
        /** @type {?} */
        AbstractToaster.prototype.sticky;
        /**
         * @type {?}
         * @protected
         */
        AbstractToaster.prototype.messageService;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/services/confirmation.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ConfirmationService = /** @class */ (function (_super) {
        __extends(ConfirmationService, _super);
        function ConfirmationService(messageService) {
            var _this = _super.call(this, messageService) || this;
            _this.messageService = messageService;
            _this.key = 'abpConfirmation';
            _this.sticky = true;
            _this.destroy$ = new rxjs.Subject();
            return _this;
        }
        /**
         * @param {?} message
         * @param {?} title
         * @param {?} severity
         * @param {?=} options
         * @return {?}
         */
        ConfirmationService.prototype.show = /**
         * @param {?} message
         * @param {?} title
         * @param {?} severity
         * @param {?=} options
         * @return {?}
         */
        function (message, title, severity, options) {
            this.listenToEscape();
            return _super.prototype.show.call(this, message, title, severity, options);
        };
        /**
         * @param {?=} status
         * @return {?}
         */
        ConfirmationService.prototype.clear = /**
         * @param {?=} status
         * @return {?}
         */
        function (status) {
            _super.prototype.clear.call(this, status);
            this.destroy$.next();
        };
        /**
         * @return {?}
         */
        ConfirmationService.prototype.listenToEscape = /**
         * @return {?}
         */
        function () {
            var _this = this;
            rxjs.fromEvent(document, 'keyup')
                .pipe(operators.takeUntil(this.destroy$), operators.debounceTime(150), operators.filter((/**
             * @param {?} key
             * @return {?}
             */
            function (key) { return key && key.key === 'Escape'; })))
                .subscribe((/**
             * @param {?} _
             * @return {?}
             */
            function (_) {
                _this.clear();
            }));
        };
        ConfirmationService.decorators = [
            { type: core.Injectable, args: [{ providedIn: 'root' },] }
        ];
        /** @nocollapse */
        ConfirmationService.ctorParameters = function () { return [
            { type: messageservice.MessageService }
        ]; };
        /** @nocollapse */ ConfirmationService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function ConfirmationService_Factory() { return new ConfirmationService(core.ɵɵinject(messageservice.MessageService)); }, token: ConfirmationService, providedIn: "root" });
        return ConfirmationService;
    }(AbstractToaster));
    if (false) {
        /** @type {?} */
        ConfirmationService.prototype.key;
        /** @type {?} */
        ConfirmationService.prototype.sticky;
        /** @type {?} */
        ConfirmationService.prototype.destroy$;
        /**
         * @type {?}
         * @protected
         */
        ConfirmationService.prototype.messageService;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/confirmation/confirmation.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ConfirmationComponent = /** @class */ (function () {
        function ConfirmationComponent(confirmationService) {
            this.confirmationService = confirmationService;
            this.confirm = "confirm" /* confirm */;
            this.reject = "reject" /* reject */;
            this.dismiss = "dismiss" /* dismiss */;
        }
        /**
         * @param {?} status
         * @return {?}
         */
        ConfirmationComponent.prototype.close = /**
         * @param {?} status
         * @return {?}
         */
        function (status) {
            this.confirmationService.clear(status);
        };
        ConfirmationComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-confirmation',
                        // tslint:disable-next-line: component-max-inline-declarations
                        template: "\n    <p-toast\n      position=\"center\"\n      key=\"abpConfirmation\"\n      (onClose)=\"close(dismiss)\"\n      [modal]=\"true\"\n      [baseZIndex]=\"1000\"\n      styleClass=\"abp-confirm\"\n    >\n      <ng-template let-message pTemplate=\"message\">\n        <i class=\"fa fa-exclamation-circle abp-confirm-icon\"></i>\n        <div *ngIf=\"message.summary\" class=\"abp-confirm-summary\">\n          {{ message.summary | abpLocalization: message.titleLocalizationParams }}\n        </div>\n        <div class=\"abp-confirm-body\">\n          {{ message.detail | abpLocalization: message.messageLocalizationParams }}\n        </div>\n\n        <div class=\"abp-confirm-footer justify-content-center\">\n          <button\n            *ngIf=\"!message.hideCancelBtn\"\n            id=\"cancel\"\n            type=\"button\"\n            class=\"btn btn-sm btn-primary\"\n            (click)=\"close(reject)\"\n          >\n            {{ message.cancelText || message.cancelCopy || 'AbpIdentity::Cancel' | abpLocalization }}\n          </button>\n          <button\n            *ngIf=\"!message.hideYesBtn\"\n            id=\"confirm\"\n            type=\"button\"\n            class=\"btn btn-sm btn-primary\"\n            (click)=\"close(confirm)\"\n            autofocus\n          >\n            <span>{{ message.yesText || message.yesCopy || 'AbpIdentity::Yes' | abpLocalization }}</span>\n          </button>\n        </div>\n      </ng-template>\n    </p-toast>\n  "
                    }] }
        ];
        /** @nocollapse */
        ConfirmationComponent.ctorParameters = function () { return [
            { type: ConfirmationService }
        ]; };
        return ConfirmationComponent;
    }());
    if (false) {
        /** @type {?} */
        ConfirmationComponent.prototype.confirm;
        /** @type {?} */
        ConfirmationComponent.prototype.reject;
        /** @type {?} */
        ConfirmationComponent.prototype.dismiss;
        /**
         * @type {?}
         * @private
         */
        ConfirmationComponent.prototype.confirmationService;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/error/error.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ErrorComponent = /** @class */ (function () {
        function ErrorComponent() {
            this.status = 0;
            this.title = 'Oops!';
            this.details = 'Sorry, an error has occured.';
            this.customComponent = null;
        }
        Object.defineProperty(ErrorComponent.prototype, "statusText", {
            get: /**
             * @return {?}
             */
            function () {
                return this.status ? "[" + this.status + "]" : '';
            },
            enumerable: true,
            configurable: true
        });
        /**
         * @return {?}
         */
        ErrorComponent.prototype.ngAfterViewInit = /**
         * @return {?}
         */
        function () {
            var _this = this;
            if (this.customComponent) {
                /** @type {?} */
                var customComponentRef = this.cfRes.resolveComponentFactory(this.customComponent).create(null);
                customComponentRef.instance.errorStatus = this.status;
                customComponentRef.instance.destroy$ = this.destroy$;
                this.containerRef.nativeElement.appendChild(((/** @type {?} */ (customComponentRef.hostView))).rootNodes[0]);
                customComponentRef.changeDetectorRef.detectChanges();
            }
            rxjs.fromEvent(document, 'keyup')
                .pipe(ng_core.takeUntilDestroy(this), operators.debounceTime(150), operators.filter((/**
             * @param {?} key
             * @return {?}
             */
            function (key) { return key && key.key === 'Escape'; })))
                .subscribe((/**
             * @return {?}
             */
            function () {
                _this.destroy();
            }));
        };
        /**
         * @return {?}
         */
        ErrorComponent.prototype.ngOnDestroy = /**
         * @return {?}
         */
        function () { };
        /**
         * @return {?}
         */
        ErrorComponent.prototype.destroy = /**
         * @return {?}
         */
        function () {
            this.destroy$.next();
            this.destroy$.complete();
        };
        ErrorComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-error',
                        template: "<div #container id=\"abp-error\" class=\"error\">\r\n  <button id=\"abp-close-button\" type=\"button\" class=\"close mr-3\" (click)=\"destroy()\">\r\n    <span aria-hidden=\"true\">&times;</span>\r\n  </button>\r\n\r\n  <div *ngIf=\"!customComponent\" class=\"row centered\">\r\n    <div class=\"col-md-12\">\r\n      <div class=\"error-template\">\r\n        <h1>{{ statusText }} {{ title | abpLocalization }}</h1>\r\n        <div class=\"error-details\">\r\n          {{ details | abpLocalization }}\r\n        </div>\r\n        <div class=\"error-actions\">\r\n          <a (click)=\"destroy()\" routerLink=\"/\" class=\"btn btn-primary btn-md mt-2\"\r\n            ><span class=\"glyphicon glyphicon-home\"></span>\r\n            {{ { key: '::Menu:Home', defaultValue: 'Home' } | abpLocalization }}\r\n          </a>\r\n        </div>\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n",
                        styles: [".error{position:fixed;top:0;background-color:#fff;width:100vw;height:100vh;z-index:999999}.centered{position:fixed;top:50%;left:50%;-webkit-transform:translate(-50%,-50%);transform:translate(-50%,-50%)}"]
                    }] }
        ];
        ErrorComponent.propDecorators = {
            containerRef: [{ type: core.ViewChild, args: ['container', { static: false },] }]
        };
        return ErrorComponent;
    }());
    if (false) {
        /** @type {?} */
        ErrorComponent.prototype.cfRes;
        /** @type {?} */
        ErrorComponent.prototype.status;
        /** @type {?} */
        ErrorComponent.prototype.title;
        /** @type {?} */
        ErrorComponent.prototype.details;
        /** @type {?} */
        ErrorComponent.prototype.customComponent;
        /** @type {?} */
        ErrorComponent.prototype.destroy$;
        /** @type {?} */
        ErrorComponent.prototype.containerRef;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/loader-bar/loader-bar.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var LoaderBarComponent = /** @class */ (function () {
        function LoaderBarComponent(actions, router, cdRef) {
            this.actions = actions;
            this.router = router;
            this.cdRef = cdRef;
            this.containerClass = 'abp-loader-bar';
            this.color = '#77b6ff';
            this.isLoading = false;
            this.progressLevel = 0;
            this.intervalPeriod = 350;
            this.stopDelay = 820;
            this.filter = (/**
             * @param {?} action
             * @return {?}
             */
            function (action) { return action.payload.url.indexOf('openid-configuration') < 0; });
        }
        Object.defineProperty(LoaderBarComponent.prototype, "boxShadow", {
            get: /**
             * @return {?}
             */
            function () {
                return "0 0 10px rgba(" + this.color + ", 0.5)";
            },
            enumerable: true,
            configurable: true
        });
        /**
         * @return {?}
         */
        LoaderBarComponent.prototype.ngOnInit = /**
         * @return {?}
         */
        function () {
            var _this = this;
            this.actions
                .pipe(store.ofActionSuccessful(ng_core.StartLoader, ng_core.StopLoader), operators.filter(this.filter), core$1.takeUntilDestroy(this))
                .subscribe((/**
             * @param {?} action
             * @return {?}
             */
            function (action) {
                if (action instanceof ng_core.StartLoader)
                    _this.startLoading();
                else
                    _this.stopLoading();
            }));
            this.router.events
                .pipe(operators.filter((/**
             * @param {?} event
             * @return {?}
             */
            function (event) {
                return event instanceof router.NavigationStart || event instanceof router.NavigationEnd || event instanceof router.NavigationError;
            })), core$1.takeUntilDestroy(this))
                .subscribe((/**
             * @param {?} event
             * @return {?}
             */
            function (event) {
                if (event instanceof router.NavigationStart)
                    _this.startLoading();
                else
                    _this.stopLoading();
            }));
        };
        /**
         * @return {?}
         */
        LoaderBarComponent.prototype.ngOnDestroy = /**
         * @return {?}
         */
        function () {
            this.interval.unsubscribe();
        };
        /**
         * @return {?}
         */
        LoaderBarComponent.prototype.startLoading = /**
         * @return {?}
         */
        function () {
            var _this = this;
            if (this.isLoading || this.progressLevel !== 0)
                return;
            this.isLoading = true;
            this.interval = rxjs.interval(this.intervalPeriod).subscribe((/**
             * @return {?}
             */
            function () {
                if (_this.progressLevel < 75) {
                    _this.progressLevel += Math.random() * 10;
                }
                else if (_this.progressLevel < 90) {
                    _this.progressLevel += 0.4;
                }
                else if (_this.progressLevel < 100) {
                    _this.progressLevel += 0.1;
                }
                else {
                    _this.interval.unsubscribe();
                }
                _this.cdRef.detectChanges();
            }));
        };
        /**
         * @return {?}
         */
        LoaderBarComponent.prototype.stopLoading = /**
         * @return {?}
         */
        function () {
            var _this = this;
            this.interval.unsubscribe();
            this.progressLevel = 100;
            this.isLoading = false;
            if (this.timer && !this.timer.closed)
                return;
            this.timer = rxjs.timer(this.stopDelay).subscribe((/**
             * @return {?}
             */
            function () {
                _this.progressLevel = 0;
                _this.cdRef.detectChanges();
            }));
        };
        LoaderBarComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-loader-bar',
                        template: "\n    <div id=\"abp-loader-bar\" [ngClass]=\"containerClass\" [class.is-loading]=\"isLoading\">\n      <div\n        class=\"abp-progress\"\n        [style.width.vw]=\"progressLevel\"\n        [ngStyle]=\"{\n          'background-color': color,\n          'box-shadow': boxShadow\n        }\"\n      ></div>\n    </div>\n  ",
                        styles: [".abp-loader-bar{left:0;opacity:0;position:fixed;top:0;-webkit-transition:opacity .4s linear .4s;transition:opacity .4s linear .4s;z-index:99999}.abp-loader-bar.is-loading{opacity:1;-webkit-transition:none;transition:none}.abp-loader-bar .abp-progress{height:3px;left:0;position:fixed;top:0;-webkit-transition:width .4s;transition:width .4s}"]
                    }] }
        ];
        /** @nocollapse */
        LoaderBarComponent.ctorParameters = function () { return [
            { type: store.Actions },
            { type: router.Router },
            { type: core.ChangeDetectorRef }
        ]; };
        LoaderBarComponent.propDecorators = {
            containerClass: [{ type: core.Input }],
            color: [{ type: core.Input }],
            isLoading: [{ type: core.Input }],
            filter: [{ type: core.Input }]
        };
        return LoaderBarComponent;
    }());
    if (false) {
        /** @type {?} */
        LoaderBarComponent.prototype.containerClass;
        /** @type {?} */
        LoaderBarComponent.prototype.color;
        /** @type {?} */
        LoaderBarComponent.prototype.isLoading;
        /** @type {?} */
        LoaderBarComponent.prototype.progressLevel;
        /** @type {?} */
        LoaderBarComponent.prototype.interval;
        /** @type {?} */
        LoaderBarComponent.prototype.timer;
        /** @type {?} */
        LoaderBarComponent.prototype.intervalPeriod;
        /** @type {?} */
        LoaderBarComponent.prototype.stopDelay;
        /** @type {?} */
        LoaderBarComponent.prototype.filter;
        /**
         * @type {?}
         * @private
         */
        LoaderBarComponent.prototype.actions;
        /**
         * @type {?}
         * @private
         */
        LoaderBarComponent.prototype.router;
        /**
         * @type {?}
         * @private
         */
        LoaderBarComponent.prototype.cdRef;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/animations/fade.animations.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /** @type {?} */
    var fadeIn = animations.animation([animations.style({ opacity: '0' }), animations.animate('{{ time}} {{ easing }}', animations.style({ opacity: '1' }))], {
        params: { time: '350ms', easing: 'ease' },
    });
    /** @type {?} */
    var fadeOut = animations.animation([animations.style({ opacity: '1' }), animations.animate('{{ time}} {{ easing }}', animations.style({ opacity: '0' }))], { params: { time: '350ms', easing: 'ease' } });
    /** @type {?} */
    var fadeInDown = animations.animation([
        animations.style({ opacity: '0', transform: '{{ transform }} translateY(-20px)' }),
        animations.animate('{{ time }} {{ easing }}', animations.style({ opacity: '1', transform: '{{ transform }} translateY(0)' })),
    ], { params: { time: '350ms', easing: 'ease', transform: '' } });
    /** @type {?} */
    var fadeInUp = animations.animation([
        animations.style({ opacity: '0', transform: '{{ transform }} translateY(20px)' }),
        animations.animate('{{ time }} {{ easing }}', animations.style({ opacity: '1', transform: '{{ transform }} translateY(0)' })),
    ], { params: { time: '350ms', easing: 'ease', transform: '' } });
    /** @type {?} */
    var fadeInLeft = animations.animation([
        animations.style({ opacity: '0', transform: '{{ transform }} translateX(20px)' }),
        animations.animate('{{ time }} {{ easing }}', animations.style({ opacity: '1', transform: '{{ transform }} translateX(0)' })),
    ], { params: { time: '350ms', easing: 'ease', transform: '' } });
    /** @type {?} */
    var fadeInRight = animations.animation([
        animations.style({ opacity: '0', transform: '{{ transform }} translateX(-20px)' }),
        animations.animate('{{ time }} {{ easing }}', animations.style({ opacity: '1', transform: '{{ transform }} translateX(0)' })),
    ], { params: { time: '350ms', easing: 'ease', transform: '' } });
    /** @type {?} */
    var fadeOutDown = animations.animation([
        animations.style({ opacity: '1', transform: '{{ transform }} translateY(0)' }),
        animations.animate('{{ time }} {{ easing }}', animations.style({ opacity: '0', transform: '{{ transform }} translateY(20px)' })),
    ], { params: { time: '350ms', easing: 'ease', transform: '' } });
    /** @type {?} */
    var fadeOutUp = animations.animation([
        animations.style({ opacity: '1', transform: '{{ transform }} translateY(0)' }),
        animations.animate('{{ time }} {{ easing }}', animations.style({ opacity: '0', transform: '{{ transform }} translateY(-20px)' })),
    ], { params: { time: '350ms', easing: 'ease', transform: '' } });
    /** @type {?} */
    var fadeOutLeft = animations.animation([
        animations.style({ opacity: '1', transform: '{{ transform }} translateX(0)' }),
        animations.animate('{{ time }} {{ easing }}', animations.style({ opacity: '0', transform: '{{ transform }} translateX(20px)' })),
    ], { params: { time: '350ms', easing: 'ease', transform: '' } });
    /** @type {?} */
    var fadeOutRight = animations.animation([
        animations.style({ opacity: '1', transform: '{{ transform }} translateX(0)' }),
        animations.animate('{{ time }} {{ easing }}', animations.style({ opacity: '0', transform: '{{ transform }} translateX(-20px)' })),
    ], { params: { time: '350ms', easing: 'ease', transform: '' } });

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/animations/modal.animations.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /** @type {?} */
    var fadeAnimation = animations.trigger('fade', [
        animations.transition(':enter', animations.useAnimation(fadeIn)),
        animations.transition(':leave', animations.useAnimation(fadeOut)),
    ]);
    /** @type {?} */
    var dialogAnimation = animations.trigger('dialog', [
        animations.transition(':enter', animations.useAnimation(fadeInDown)),
        animations.transition(':leave', animations.useAnimation(fadeOut)),
    ]);

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/modal/modal.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ModalComponent = /** @class */ (function () {
        function ModalComponent(renderer, confirmationService) {
            this.renderer = renderer;
            this.confirmationService = confirmationService;
            this.centered = false;
            this.modalClass = '';
            this.size = 'lg';
            this.visibleChange = new core.EventEmitter();
            this.init = new core.EventEmitter();
            this.appear = new core.EventEmitter();
            this.disappear = new core.EventEmitter();
            this._visible = false;
            this._busy = false;
            this.isModalOpen = false;
            this.isConfirmationOpen = false;
            this.destroy$ = new rxjs.Subject();
        }
        Object.defineProperty(ModalComponent.prototype, "visible", {
            get: /**
             * @return {?}
             */
            function () {
                return this._visible;
            },
            set: /**
             * @param {?} value
             * @return {?}
             */
            function (value) {
                var _this = this;
                if (typeof value !== 'boolean')
                    return;
                this.isModalOpen = value;
                this._visible = value;
                this.visibleChange.emit(value);
                if (value) {
                    setTimeout((/**
                     * @return {?}
                     */
                    function () { return _this.listen(); }), 0);
                    this.renderer.addClass(document.body, 'modal-open');
                    this.appear.emit();
                }
                else {
                    this.renderer.removeClass(document.body, 'modal-open');
                    this.disappear.emit();
                    this.destroy$.next();
                }
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(ModalComponent.prototype, "busy", {
            get: /**
             * @return {?}
             */
            function () {
                return this._busy;
            },
            set: /**
             * @param {?} value
             * @return {?}
             */
            function (value) {
                if (this.abpSubmit && this.abpSubmit instanceof ButtonComponent) {
                    this.abpSubmit.loading = value;
                }
                this._busy = value;
            },
            enumerable: true,
            configurable: true
        });
        /**
         * @return {?}
         */
        ModalComponent.prototype.ngOnDestroy = /**
         * @return {?}
         */
        function () {
            this.destroy$.next();
        };
        /**
         * @return {?}
         */
        ModalComponent.prototype.close = /**
         * @return {?}
         */
        function () {
            var _this = this;
            if (this.busy)
                return;
            /** @type {?} */
            var nodes = getFlatNodes(((/** @type {?} */ (this.modalContent.nativeElement.querySelector('#abp-modal-body')))).childNodes);
            if (hasNgDirty(nodes)) {
                if (this.isConfirmationOpen)
                    return;
                this.isConfirmationOpen = true;
                this.confirmationService
                    .warn('AbpAccount::AreYouSureYouWantToCancelEditingWarningMessage', 'AbpAccount::AreYouSure')
                    .subscribe((/**
                 * @param {?} status
                 * @return {?}
                 */
                function (status) {
                    _this.isConfirmationOpen = false;
                    if (status === "confirm" /* confirm */) {
                        _this.visible = false;
                    }
                }));
            }
            else {
                this.visible = false;
            }
        };
        /**
         * @return {?}
         */
        ModalComponent.prototype.listen = /**
         * @return {?}
         */
        function () {
            var _this = this;
            rxjs.fromEvent(document, 'keyup')
                .pipe(operators.takeUntil(this.destroy$), operators.debounceTime(150), operators.filter((/**
             * @param {?} key
             * @return {?}
             */
            function (key) { return key && key.key === 'Escape'; })))
                .subscribe((/**
             * @return {?}
             */
            function () {
                _this.close();
            }));
            setTimeout((/**
             * @return {?}
             */
            function () {
                if (!_this.abpClose)
                    return;
                rxjs.fromEvent(_this.abpClose.nativeElement, 'click')
                    .pipe(operators.takeUntil(_this.destroy$), operators.filter((/**
                 * @return {?}
                 */
                function () { return !!_this.modalContent; })))
                    .subscribe((/**
                 * @return {?}
                 */
                function () { return _this.close(); }));
            }), 0);
            this.init.emit();
        };
        ModalComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-modal',
                        template: "<ng-container *ngIf=\"visible\">\r\n  <div class=\"modal show {{ modalClass }}\" tabindex=\"-1\" role=\"dialog\">\r\n    <div class=\"modal-backdrop\" [@fade]=\"isModalOpen\" (click)=\"close()\"></div>\r\n    <div\r\n      id=\"abp-modal-dialog\"\r\n      class=\"modal-dialog modal-{{ size }}\"\r\n      role=\"document\"\r\n      [class.modal-dialog-centered]=\"centered\"\r\n      [@dialog]=\"isModalOpen\"\r\n      #abpModalContent\r\n    >\r\n      <div id=\"abp-modal-content\" class=\"modal-content\">\r\n        <div id=\"abp-modal-header\" class=\"modal-header\">\r\n          <ng-container *ngTemplateOutlet=\"abpHeader\"></ng-container>\r\n          \u200B\r\n          <button id=\"abp-modal-close-button\" type=\"button\" class=\"close\" aria-label=\"Close\" (click)=\"close()\">\r\n            <span aria-hidden=\"true\">&times;</span>\r\n          </button>\r\n        </div>\r\n        <div id=\"abp-modal-body\" class=\"modal-body\">\r\n          <ng-container *ngTemplateOutlet=\"abpBody\"></ng-container>\r\n        </div>\r\n        <div id=\"abp-modal-footer\" class=\"modal-footer\">\r\n          <ng-container *ngTemplateOutlet=\"abpFooter\"></ng-container>\r\n        </div>\r\n      </div>\r\n    </div>\r\n    <ng-content></ng-content>\r\n  </div>\r\n</ng-container>\r\n",
                        animations: [fadeAnimation, dialogAnimation]
                    }] }
        ];
        /** @nocollapse */
        ModalComponent.ctorParameters = function () { return [
            { type: core.Renderer2 },
            { type: ConfirmationService }
        ]; };
        ModalComponent.propDecorators = {
            visible: [{ type: core.Input }],
            busy: [{ type: core.Input }],
            centered: [{ type: core.Input }],
            modalClass: [{ type: core.Input }],
            size: [{ type: core.Input }],
            abpSubmit: [{ type: core.ContentChild, args: [ButtonComponent, { static: false, read: ButtonComponent },] }],
            abpHeader: [{ type: core.ContentChild, args: ['abpHeader', { static: false },] }],
            abpBody: [{ type: core.ContentChild, args: ['abpBody', { static: false },] }],
            abpFooter: [{ type: core.ContentChild, args: ['abpFooter', { static: false },] }],
            abpClose: [{ type: core.ContentChild, args: ['abpClose', { static: false, read: core.ElementRef },] }],
            modalContent: [{ type: core.ViewChild, args: ['abpModalContent', { static: false },] }],
            abpButtons: [{ type: core.ViewChildren, args: ['abp-button',] }],
            visibleChange: [{ type: core.Output }],
            init: [{ type: core.Output }],
            appear: [{ type: core.Output }],
            disappear: [{ type: core.Output }]
        };
        return ModalComponent;
    }());
    if (false) {
        /** @type {?} */
        ModalComponent.prototype.centered;
        /** @type {?} */
        ModalComponent.prototype.modalClass;
        /** @type {?} */
        ModalComponent.prototype.size;
        /** @type {?} */
        ModalComponent.prototype.abpSubmit;
        /** @type {?} */
        ModalComponent.prototype.abpHeader;
        /** @type {?} */
        ModalComponent.prototype.abpBody;
        /** @type {?} */
        ModalComponent.prototype.abpFooter;
        /** @type {?} */
        ModalComponent.prototype.abpClose;
        /** @type {?} */
        ModalComponent.prototype.modalContent;
        /** @type {?} */
        ModalComponent.prototype.abpButtons;
        /** @type {?} */
        ModalComponent.prototype.visibleChange;
        /** @type {?} */
        ModalComponent.prototype.init;
        /** @type {?} */
        ModalComponent.prototype.appear;
        /** @type {?} */
        ModalComponent.prototype.disappear;
        /** @type {?} */
        ModalComponent.prototype._visible;
        /** @type {?} */
        ModalComponent.prototype._busy;
        /** @type {?} */
        ModalComponent.prototype.isModalOpen;
        /** @type {?} */
        ModalComponent.prototype.isConfirmationOpen;
        /** @type {?} */
        ModalComponent.prototype.destroy$;
        /**
         * @type {?}
         * @private
         */
        ModalComponent.prototype.renderer;
        /**
         * @type {?}
         * @private
         */
        ModalComponent.prototype.confirmationService;
    }
    /**
     * @param {?} nodes
     * @return {?}
     */
    function getFlatNodes(nodes) {
        return Array.from(nodes).reduce((/**
         * @param {?} acc
         * @param {?} val
         * @return {?}
         */
        function (acc, val) { return __spread(acc, (val.childNodes && val.childNodes.length ? getFlatNodes(val.childNodes) : [val])); }), []);
    }
    /**
     * @param {?} nodes
     * @return {?}
     */
    function hasNgDirty(nodes) {
        return nodes.findIndex((/**
         * @param {?} node
         * @return {?}
         */
        function (node) { return (node.className || '').indexOf('ng-dirty') > -1; })) > -1;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/sort-order-icon/sort-order-icon.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var SortOrderIconComponent = /** @class */ (function () {
        function SortOrderIconComponent() {
            this.selectedKeyChange = new core.EventEmitter();
            this.orderChange = new core.EventEmitter();
        }
        Object.defineProperty(SortOrderIconComponent.prototype, "selectedKey", {
            get: /**
             * @return {?}
             */
            function () {
                return this._selectedKey;
            },
            set: /**
             * @param {?} value
             * @return {?}
             */
            function (value) {
                this._selectedKey = value;
                this.selectedKeyChange.emit(value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(SortOrderIconComponent.prototype, "order", {
            get: /**
             * @return {?}
             */
            function () {
                return this._order;
            },
            set: /**
             * @param {?} value
             * @return {?}
             */
            function (value) {
                this._order = value;
                this.orderChange.emit(value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(SortOrderIconComponent.prototype, "icon", {
            get: /**
             * @return {?}
             */
            function () {
                if (!this.selectedKey)
                    return 'fa-sort';
                if (this.selectedKey === this.key)
                    return "fa-sort-" + this.order;
                else
                    return '';
            },
            enumerable: true,
            configurable: true
        });
        /**
         * @param {?} key
         * @return {?}
         */
        SortOrderIconComponent.prototype.sort = /**
         * @param {?} key
         * @return {?}
         */
        function (key) {
            this.selectedKey = key;
            switch (this.order) {
                case '':
                    this.order = 'asc';
                    break;
                case 'asc':
                    this.order = 'desc';
                    this.orderChange.emit('desc');
                    break;
                case 'desc':
                    this.order = '';
                    this.selectedKey = '';
                    break;
            }
        };
        SortOrderIconComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-sort-order-icon',
                        template: "<span class=\"float-right {{ iconClass }}\">\r\n  <i class=\"fa {{ icon }}\"></i>\r\n</span>\r\n"
                    }] }
        ];
        SortOrderIconComponent.propDecorators = {
            selectedKey: [{ type: core.Input }],
            selectedKeyChange: [{ type: core.Output }],
            key: [{ type: core.Input }],
            order: [{ type: core.Input }],
            orderChange: [{ type: core.Output }],
            iconClass: [{ type: core.Input }]
        };
        return SortOrderIconComponent;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        SortOrderIconComponent.prototype._order;
        /**
         * @type {?}
         * @private
         */
        SortOrderIconComponent.prototype._selectedKey;
        /** @type {?} */
        SortOrderIconComponent.prototype.selectedKeyChange;
        /** @type {?} */
        SortOrderIconComponent.prototype.key;
        /** @type {?} */
        SortOrderIconComponent.prototype.orderChange;
        /** @type {?} */
        SortOrderIconComponent.prototype.iconClass;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/table-empty-message/table-empty-message.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var TableEmptyMessageComponent = /** @class */ (function () {
        function TableEmptyMessageComponent() {
            this.colspan = 2;
            this.localizationResource = 'AbpAccount';
            this.localizationProp = 'NoDataAvailableInDatatable';
        }
        Object.defineProperty(TableEmptyMessageComponent.prototype, "emptyMessage", {
            get: /**
             * @return {?}
             */
            function () {
                return this.message || this.localizationResource + "::" + this.localizationProp;
            },
            enumerable: true,
            configurable: true
        });
        TableEmptyMessageComponent.decorators = [
            { type: core.Component, args: [{
                        // tslint:disable-next-line: component-selector
                        selector: '[abp-table-empty-message]',
                        template: "\n    <td class=\"text-center\" [attr.colspan]=\"colspan\">\n      {{ emptyMessage | abpLocalization }}\n    </td>\n  "
                    }] }
        ];
        TableEmptyMessageComponent.propDecorators = {
            colspan: [{ type: core.Input }],
            message: [{ type: core.Input }],
            localizationResource: [{ type: core.Input }],
            localizationProp: [{ type: core.Input }]
        };
        return TableEmptyMessageComponent;
    }());
    if (false) {
        /** @type {?} */
        TableEmptyMessageComponent.prototype.colspan;
        /** @type {?} */
        TableEmptyMessageComponent.prototype.message;
        /** @type {?} */
        TableEmptyMessageComponent.prototype.localizationResource;
        /** @type {?} */
        TableEmptyMessageComponent.prototype.localizationProp;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/toast/toast.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ToastComponent = /** @class */ (function () {
        function ToastComponent() {
        }
        ToastComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-toast',
                        // tslint:disable-next-line: component-max-inline-declarations
                        template: "\n    <p-toast position=\"bottom-right\" key=\"abpToast\" styleClass=\"abp-toast\" [baseZIndex]=\"1000\">\n      <ng-template let-message pTemplate=\"message\">\n        <span\n          class=\"ui-toast-icon pi\"\n          [ngClass]=\"{\n            'pi-info-circle': message.severity === 'info',\n            'pi-exclamation-triangle': message.severity === 'warn',\n            'pi-times': message.severity === 'error',\n            'pi-check': message.severity === 'success'\n          }\"\n        ></span>\n        <div class=\"ui-toast-message-text-content\">\n          <div class=\"ui-toast-summary\">{{ message.summary | abpLocalization: message.titleLocalizationParams }}</div>\n          <div class=\"ui-toast-detail\">{{ message.detail | abpLocalization: message.messageLocalizationParams }}</div>\n        </div>\n      </ng-template>\n    </p-toast>\n  "
                    }] }
        ];
        return ToastComponent;
    }());

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/contants/styles.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var styles = "\n.is-invalid .form-control {\n  border-color: #dc3545;\n  border-style: solid !important;\n}\n\n.is-invalid .invalid-feedback,\n.is-invalid + * .invalid-feedback {\n  display: block;\n}\n\n.data-tables-filter {\n  text-align: right;\n}\n\n.pointer {\n  cursor: pointer;\n}\n\n.navbar .dropdown-submenu a::after {\n  transform: rotate(-90deg);\n  position: absolute;\n  right: 16px;\n  top: 18px;\n}\n\n.navbar .dropdown-menu {\n  min-width: 215px;\n}\n\n.ui-table-scrollable-body::-webkit-scrollbar {\n  height: 5px !important;\n}\n\n.ui-table-scrollable-body::-webkit-scrollbar-track {\n  background: #ddd;\n}\n\n.ui-table-scrollable-body::-webkit-scrollbar-thumb {\n  background: #8a8686;\n}\n\n.modal.show {\n  display: block !important;\n}\n\n.modal-backdrop {\n  position: absolute !important;\n  top: 0 !important;\n  left: 0 !important;\n  width: 100% !important;\n  height: 100% !important;\n  background-color: rgba(0, 0, 0, 0.6) !important;\n  z-index: 1040 !important;\n}\n\n.modal-dialog {\n  z-index: 1050 !important;\n}\n\n.abp-ellipsis-inline {\n  display: inline-block;\n  overflow: hidden;\n  text-overflow: ellipsis;\n  white-space: nowrap;\n}\n\n.abp-ellipsis {\n  overflow: hidden !important;\n  text-overflow: ellipsis;\n  white-space: nowrap;\n}\n\n.abp-toast .ui-toast-message {\n  box-sizing: border-box !important;\n  border: 2px solid transparent !important;\n  border-radius: 4px !important;\n  background-color: #f4f4f7 !important;\n  color: #1b1d29 !important;\n}\n\n.abp-toast .ui-toast-message-content {\n  padding: 10px !important;\n}\n\n.abp-toast .ui-toast-message-content .ui-toast-icon {\n  top: 0 !important;\n  left: 0 !important;\n  padding: 10px !important;\n}\n\n.abp-toast .ui-toast-summary {\n  margin: 0 !important;\n  font-weight: 700 !important;\n}\n\n.abp-toast .ui-toast-message.ui-toast-message-error {\n  border-color: #ba1659 !important;\n}\n\n.abp-toast .ui-toast-message.ui-toast-message-error .ui-toast-message-content .ui-toast-icon {\n  color: #ba1659 !important;\n}\n\n.abp-toast .ui-toast-message.ui-toast-message-warning {\n  border-color: #ed5d98 !important;\n}\n\n.abp-toast .ui-toast-message.ui-toast-message-warning .ui-toast-message-content .ui-toast-icon {\n  color: #ed5d98 !important;\n}\n\n.abp-toast .ui-toast-message.ui-toast-message-success {\n  border-color: #1c9174 !important;\n}\n\n.abp-toast .ui-toast-message.ui-toast-message-success .ui-toast-message-content .ui-toast-icon {\n  color: #1c9174 !important;\n}\n\n.abp-toast .ui-toast-message.ui-toast-message-info {\n  border-color: #fccb31 !important;\n}\n\n.abp-toast .ui-toast-message.ui-toast-message-info .ui-toast-message-content .ui-toast-icon {\n  color: #fccb31 !important;\n}\n\n.abp-confirm .ui-toast-message {\n  box-sizing: border-box !important;\n  padding: 0px !important;\n  border:0 none !important;\n  border-radius: 4px !important;\n  background-color: #fff !important;\n  color: rgba(0, 0, 0, .65) !important;\n  font-family: \"Poppins\", sans-serif;\n  text-align: center !important;\n}\n\n.abp-confirm .ui-toast-message-content {\n  padding: 0px !important;\n}\n\n.abp-confirm .abp-confirm-icon {\n  margin: 32px 50px 5px !important;\n  color: #f8bb86 !important;\n  font-size: 52px !important;\n}\n\n.abp-confirm .ui-toast-close-icon {\n  display: none !important;\n}\n\n.abp-confirm .abp-confirm-summary {\n  display: block !important;\n  margin-bottom: 13px !important;\n  padding: 13px 16px 0px !important;\n  font-weight: 600 !important;\n  font-size: 18px !important;\n}\n\n.abp-confirm .abp-confirm-body {\n  display: inline-block !important;\n  padding: 0px 10px !important;\n}\n\n.abp-confirm .abp-confirm-footer {\n  display: block !important;\n  margin-top: 30px !important;\n  padding: 16px !important;\n  background-color: #f4f4f7 !important;\n  text-align: right !important;\n}\n\n.abp-confirm .abp-confirm-footer .btn {\n  margin-left: 10px !important;\n}\n\n.ui-widget-overlay {\n  z-index: 1000;\n}\n\n.color-white {\n  color: #FFF !important;\n}\n\n/* <animations */\n\n.fade-in-top {\n  animation: fadeInTop 0.2s ease-in-out;\n}\n\n.fade-out-top {\n  animation: fadeOutTop 0.2s ease-in-out;\n}\n\n.abp-collapsed-height {\n  -moz-transition: max-height linear 0.35s;\n  -ms-transition: max-height linear 0.35s;\n  -o-transition: max-height linear 0.35s;\n  -webkit-transition: max-height linear 0.35s;\n  overflow:hidden;\n  transition:max-height 0.35s linear;\n  height:auto;\n  max-height: 0;\n}\n\n.abp-mh-25 {\n  max-height: 25vh;\n}\n\n.abp-mh-50 {\n  transition:max-height 0.65s linear;\n  max-height: 50vh;\n}\n\n.abp-mh-75 {\n  transition:max-height 0.85s linear;\n  max-height: 75vh;\n}\n\n.abp-mh-100 {\n  transition:max-height 1s linear;\n  max-height: 100vh;\n}\n\n@keyframes fadeInTop {\n  from {\n    transform: translateY(-5px);\n    opacity: 0;\n  }\n\n  to {\n    transform: translateY(0px);\n    opacity: 1;\n  }\n}\n\n@keyframes fadeOutTop {\n  to {\n    transform: translateY(-5px);\n    opacity: 0;\n  }\n}\n\n/* </animations */\n\n";

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/directives/table-sort.directive.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @record
     */
    function TableSortOptions() { }
    if (false) {
        /** @type {?} */
        TableSortOptions.prototype.key;
        /** @type {?} */
        TableSortOptions.prototype.order;
    }
    var TableSortDirective = /** @class */ (function () {
        function TableSortDirective(table, sortPipe) {
            this.table = table;
            this.sortPipe = sortPipe;
            this.value = [];
        }
        /**
         * @param {?} __0
         * @return {?}
         */
        TableSortDirective.prototype.ngOnChanges = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var value = _a.value, abpTableSort = _a.abpTableSort;
            if (value || abpTableSort) {
                this.abpTableSort = this.abpTableSort || ((/** @type {?} */ ({})));
                this.table.value = this.sortPipe.transform(clone(this.value), this.abpTableSort.order, this.abpTableSort.key);
            }
        };
        TableSortDirective.decorators = [
            { type: core.Directive, args: [{
                        selector: '[abpTableSort]',
                        providers: [ng_core.SortPipe],
                    },] }
        ];
        /** @nocollapse */
        TableSortDirective.ctorParameters = function () { return [
            { type: table.Table, decorators: [{ type: core.Optional }, { type: core.Self }] },
            { type: ng_core.SortPipe }
        ]; };
        TableSortDirective.propDecorators = {
            abpTableSort: [{ type: core.Input }],
            value: [{ type: core.Input }]
        };
        return TableSortDirective;
    }());
    if (false) {
        /** @type {?} */
        TableSortDirective.prototype.abpTableSort;
        /** @type {?} */
        TableSortDirective.prototype.value;
        /**
         * @type {?}
         * @private
         */
        TableSortDirective.prototype.table;
        /**
         * @type {?}
         * @private
         */
        TableSortDirective.prototype.sortPipe;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/tokens/error-pages.token.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @param {?=} config
     * @return {?}
     */
    function httpErrorConfigFactory(config) {
        if (config === void 0) { config = (/** @type {?} */ ({})); }
        if (config.errorScreen && config.errorScreen.component && !config.errorScreen.forWhichErrors) {
            config.errorScreen.forWhichErrors = [401, 403, 404, 500];
        }
        return (/** @type {?} */ (__assign({ errorScreen: {} }, config)));
    }
    /** @type {?} */
    var HTTP_ERROR_CONFIG = new core.InjectionToken('HTTP_ERROR_CONFIG');

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/handlers/error.handler.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /** @type {?} */
    var DEFAULT_ERROR_MESSAGES = {
        defaultError: {
            title: 'An error has occurred!',
            details: 'Error detail not sent by server.',
        },
        defaultError401: {
            title: 'You are not authenticated!',
            details: 'You should be authenticated (sign in) in order to perform this operation.',
        },
        defaultError403: {
            title: 'You are not authorized!',
            details: 'You are not allowed to perform this operation.',
        },
        defaultError404: {
            title: 'Resource not found!',
            details: 'The resource requested could not found on the server.',
        },
        defaultError500: {
            title: 'Internal server error',
            details: 'Error detail not sent by server.',
        },
    };
    var ErrorHandler = /** @class */ (function () {
        function ErrorHandler(actions, store$1, confirmationService, appRef, cfRes, rendererFactory, injector, httpErrorConfig) {
            var _this = this;
            this.actions = actions;
            this.store = store$1;
            this.confirmationService = confirmationService;
            this.appRef = appRef;
            this.cfRes = cfRes;
            this.rendererFactory = rendererFactory;
            this.injector = injector;
            this.httpErrorConfig = httpErrorConfig;
            this.actions.pipe(store.ofActionSuccessful(ng_core.RestOccurError, routerPlugin.RouterError, routerPlugin.RouterDataResolved)).subscribe((/**
             * @param {?} res
             * @return {?}
             */
            function (res) {
                if (res instanceof ng_core.RestOccurError) {
                    var _a = res.payload, err_1 = _a === void 0 ? (/** @type {?} */ ({})) : _a;
                    /** @type {?} */
                    var body = snq((/**
                     * @return {?}
                     */
                    function () { return ((/** @type {?} */ (err_1))).error.error; }), DEFAULT_ERROR_MESSAGES.defaultError.title);
                    if (err_1 instanceof http.HttpErrorResponse && err_1.headers.get('_AbpErrorFormat')) {
                        /** @type {?} */
                        var confirmation$ = _this.showError(null, null, body);
                        if (err_1.status === 401) {
                            confirmation$.subscribe((/**
                             * @return {?}
                             */
                            function () {
                                _this.navigateToLogin();
                            }));
                        }
                    }
                    else {
                        switch (((/** @type {?} */ (err_1))).status) {
                            case 401:
                                _this.canCreateCustomError(401)
                                    ? _this.show401Page()
                                    : _this.showError({
                                        key: 'AbpAccount::DefaultErrorMessage401',
                                        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.title,
                                    }, {
                                        key: 'AbpAccount::DefaultErrorMessage401Detail',
                                        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.details,
                                    }).subscribe((/**
                                     * @return {?}
                                     */
                                    function () { return _this.navigateToLogin(); }));
                                break;
                            case 403:
                                _this.createErrorComponent({
                                    title: {
                                        key: 'AbpAccount::DefaultErrorMessage403',
                                        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError403.title,
                                    },
                                    details: {
                                        key: 'AbpAccount::DefaultErrorMessage403Detail',
                                        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError403.details,
                                    },
                                    status: 403,
                                });
                                break;
                            case 404:
                                _this.canCreateCustomError(404)
                                    ? _this.show404Page()
                                    : _this.showError({
                                        key: 'AbpAccount::DefaultErrorMessage404',
                                        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.details,
                                    }, {
                                        key: 'AbpAccount::DefaultErrorMessage404Detail',
                                        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.title,
                                    });
                                break;
                            case 500:
                                _this.createErrorComponent({
                                    title: {
                                        key: 'AbpAccount::500Message',
                                        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError500.title,
                                    },
                                    details: {
                                        key: 'AbpAccount::InternalServerErrorMessage',
                                        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError500.details,
                                    },
                                    status: 500,
                                });
                                break;
                            case 0:
                                if (((/** @type {?} */ (err_1))).statusText === 'Unknown Error') {
                                    _this.createErrorComponent({
                                        title: {
                                            key: 'AbpAccount::DefaultErrorMessage',
                                            defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title,
                                        },
                                    });
                                }
                                break;
                            default:
                                _this.showError(DEFAULT_ERROR_MESSAGES.defaultError.details, DEFAULT_ERROR_MESSAGES.defaultError.title);
                                break;
                        }
                    }
                }
                else if (res instanceof routerPlugin.RouterError && snq((/**
                 * @return {?}
                 */
                function () { return res.event.error.indexOf('Cannot match') > -1; }), false)) {
                    _this.show404Page();
                }
                else if (res instanceof routerPlugin.RouterDataResolved && _this.componentRef) {
                    _this.componentRef.destroy();
                    _this.componentRef = null;
                }
            }));
        }
        /**
         * @private
         * @return {?}
         */
        ErrorHandler.prototype.show401Page = /**
         * @private
         * @return {?}
         */
        function () {
            this.createErrorComponent({
                title: {
                    key: 'AbpAccount::401Message',
                    defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.title,
                },
                status: 401,
            });
        };
        /**
         * @private
         * @return {?}
         */
        ErrorHandler.prototype.show404Page = /**
         * @private
         * @return {?}
         */
        function () {
            this.createErrorComponent({
                title: {
                    key: 'AbpAccount::404Message',
                    defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.title,
                },
                status: 404,
            });
        };
        /**
         * @private
         * @param {?=} message
         * @param {?=} title
         * @param {?=} body
         * @return {?}
         */
        ErrorHandler.prototype.showError = /**
         * @private
         * @param {?=} message
         * @param {?=} title
         * @param {?=} body
         * @return {?}
         */
        function (message, title, body) {
            if (body) {
                if (body.details) {
                    message = body.details;
                    title = body.message;
                }
                else {
                    message = body.message || DEFAULT_ERROR_MESSAGES.defaultError.title;
                }
            }
            return this.confirmationService.error(message, title, {
                hideCancelBtn: true,
                yesText: 'AbpAccount::Close',
            });
        };
        /**
         * @private
         * @return {?}
         */
        ErrorHandler.prototype.navigateToLogin = /**
         * @private
         * @return {?}
         */
        function () {
            console.warn(this.store.selectSnapshot(routerPlugin.RouterState.url));
            this.store.dispatch(new routerPlugin.Navigate(['/account/login'], null, { state: { redirectUrl: this.store.selectSnapshot(routerPlugin.RouterState.url) } }));
        };
        /**
         * @param {?} instance
         * @return {?}
         */
        ErrorHandler.prototype.createErrorComponent = /**
         * @param {?} instance
         * @return {?}
         */
        function (instance) {
            var _this = this;
            /** @type {?} */
            var renderer = this.rendererFactory.createRenderer(null, null);
            /** @type {?} */
            var host = renderer.selectRootElement(document.body, true);
            this.componentRef = this.cfRes.resolveComponentFactory(ErrorComponent).create(this.injector);
            for (var key in this.componentRef.instance) {
                if (this.componentRef.instance.hasOwnProperty(key)) {
                    this.componentRef.instance[key] = instance[key];
                }
            }
            if (this.canCreateCustomError((/** @type {?} */ (instance.status)))) {
                this.componentRef.instance.cfRes = this.cfRes;
                this.componentRef.instance.customComponent = this.httpErrorConfig.errorScreen.component;
            }
            this.appRef.attachView(this.componentRef.hostView);
            renderer.appendChild(host, ((/** @type {?} */ (this.componentRef.hostView))).rootNodes[0]);
            /** @type {?} */
            var destroy$ = new rxjs.Subject();
            this.componentRef.instance.destroy$ = destroy$;
            destroy$.subscribe((/**
             * @return {?}
             */
            function () {
                _this.componentRef.destroy();
                _this.componentRef = null;
            }));
        };
        /**
         * @param {?} status
         * @return {?}
         */
        ErrorHandler.prototype.canCreateCustomError = /**
         * @param {?} status
         * @return {?}
         */
        function (status) {
            var _this = this;
            return snq((/**
             * @return {?}
             */
            function () {
                return _this.httpErrorConfig.errorScreen.component &&
                    _this.httpErrorConfig.errorScreen.forWhichErrors.indexOf(status) > -1;
            }));
        };
        ErrorHandler.decorators = [
            { type: core.Injectable, args: [{ providedIn: 'root' },] }
        ];
        /** @nocollapse */
        ErrorHandler.ctorParameters = function () { return [
            { type: store.Actions },
            { type: store.Store },
            { type: ConfirmationService },
            { type: core.ApplicationRef },
            { type: core.ComponentFactoryResolver },
            { type: core.RendererFactory2 },
            { type: core.Injector },
            { type: undefined, decorators: [{ type: core.Inject, args: [HTTP_ERROR_CONFIG,] }] }
        ]; };
        /** @nocollapse */ ErrorHandler.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function ErrorHandler_Factory() { return new ErrorHandler(core.ɵɵinject(store.Actions), core.ɵɵinject(store.Store), core.ɵɵinject(ConfirmationService), core.ɵɵinject(core.ApplicationRef), core.ɵɵinject(core.ComponentFactoryResolver), core.ɵɵinject(core.RendererFactory2), core.ɵɵinject(core.INJECTOR), core.ɵɵinject(HTTP_ERROR_CONFIG)); }, token: ErrorHandler, providedIn: "root" });
        return ErrorHandler;
    }());
    if (false) {
        /** @type {?} */
        ErrorHandler.prototype.componentRef;
        /**
         * @type {?}
         * @private
         */
        ErrorHandler.prototype.actions;
        /**
         * @type {?}
         * @private
         */
        ErrorHandler.prototype.store;
        /**
         * @type {?}
         * @private
         */
        ErrorHandler.prototype.confirmationService;
        /**
         * @type {?}
         * @private
         */
        ErrorHandler.prototype.appRef;
        /**
         * @type {?}
         * @private
         */
        ErrorHandler.prototype.cfRes;
        /**
         * @type {?}
         * @private
         */
        ErrorHandler.prototype.rendererFactory;
        /**
         * @type {?}
         * @private
         */
        ErrorHandler.prototype.injector;
        /**
         * @type {?}
         * @private
         */
        ErrorHandler.prototype.httpErrorConfig;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/theme-shared.module.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @param {?} injector
     * @return {?}
     */
    function appendScript(injector) {
        /** @type {?} */
        var fn = (/**
         * @return {?}
         */
        function () {
            import('chart.js').then((/**
             * @return {?}
             */
            function () { return chartJsLoaded$.next(true); }));
            /** @type {?} */
            var lazyLoadService = injector.get(ng_core.LazyLoadService);
            return rxjs.forkJoin(lazyLoadService.load(null, 'style', styles, 'head', 'afterbegin') /* lazyLoadService.load(null, 'script', scripts) */).toPromise();
        });
        return fn;
    }
    var ThemeSharedModule = /** @class */ (function () {
        function ThemeSharedModule() {
        }
        /**
         * @param {?=} options
         * @return {?}
         */
        ThemeSharedModule.forRoot = /**
         * @param {?=} options
         * @return {?}
         */
        function (options) {
            if (options === void 0) { options = (/** @type {?} */ ({})); }
            return {
                ngModule: ThemeSharedModule,
                providers: [
                    {
                        provide: core.APP_INITIALIZER,
                        multi: true,
                        deps: [core.Injector, ErrorHandler],
                        useFactory: appendScript,
                    },
                    { provide: messageservice.MessageService, useClass: messageservice.MessageService },
                    { provide: HTTP_ERROR_CONFIG, useValue: options.httpErrorConfig },
                    {
                        provide: 'HTTP_ERROR_CONFIG',
                        useFactory: httpErrorConfigFactory,
                        deps: [HTTP_ERROR_CONFIG],
                    },
                ],
            };
        };
        ThemeSharedModule.decorators = [
            { type: core.NgModule, args: [{
                        imports: [ng_core.CoreModule, toast.ToastModule, core$1.NgxValidateCoreModule],
                        declarations: [
                            BreadcrumbComponent,
                            ButtonComponent,
                            ChartComponent,
                            ConfirmationComponent,
                            ErrorComponent,
                            LoaderBarComponent,
                            ModalComponent,
                            TableEmptyMessageComponent,
                            ToastComponent,
                            SortOrderIconComponent,
                            TableSortDirective,
                        ],
                        exports: [
                            BreadcrumbComponent,
                            ButtonComponent,
                            ChartComponent,
                            ConfirmationComponent,
                            LoaderBarComponent,
                            ModalComponent,
                            TableEmptyMessageComponent,
                            ToastComponent,
                            SortOrderIconComponent,
                            TableSortDirective,
                        ],
                        entryComponents: [ErrorComponent],
                    },] }
        ];
        return ThemeSharedModule;
    }());

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/animations/bounce.animations.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /** @type {?} */
    var bounceIn = animations.animation([
        animations.style({ opacity: '0', display: '{{ display }}' }),
        animations.animate('{{ time}} {{ easing }}', animations.keyframes([
            animations.style({ opacity: '0', transform: '{{ transform }} scale(0.0)', offset: 0 }),
            animations.style({ opacity: '0', transform: '{{ transform }} scale(0.8)', offset: 0.5 }),
            animations.style({ opacity: '1', transform: '{{ transform }} scale(1.0)', offset: 1 })
        ]))
    ], {
        params: {
            time: '350ms',
            easing: 'cubic-bezier(.7,.31,.72,1.47)',
            display: 'block',
            transform: 'translate(-50%, -50%)'
        }
    });

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/animations/collapse.animations.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /** @type {?} */
    var collapseY = animations.animation([
        animations.style({ height: '*', overflow: 'hidden', 'box-sizing': 'border-box' }),
        animations.animate('{{ time }} {{ easing }}', animations.style({ height: '0', padding: '0px' })),
    ], { params: { time: '350ms', easing: 'ease' } });
    /** @type {?} */
    var collapseYWithMargin = animations.animation([animations.style({ 'margin-top': '0' }), animations.animate('{{ time }} {{ easing }}', animations.style({ 'margin-top': '-100%' }))], {
        params: { time: '500ms', easing: 'ease' },
    });
    /** @type {?} */
    var collapseX = animations.animation([
        animations.style({ width: '*', overflow: 'hidden', 'box-sizing': 'border-box' }),
        animations.animate('{{ time }} {{ easing }}', animations.style({ width: '0', padding: '0px' })),
    ], { params: { time: '350ms', easing: 'ease' } });
    /** @type {?} */
    var expandY = animations.animation([
        animations.style({ height: '0', overflow: 'hidden', 'box-sizing': 'border-box' }),
        animations.animate('{{ time }} {{ easing }}', animations.style({ height: '*', padding: '*' })),
    ], { params: { time: '350ms', easing: 'ease' } });
    /** @type {?} */
    var expandYWithMargin = animations.animation([animations.style({ 'margin-top': '-100%' }), animations.animate('{{ time }} {{ easing }}', animations.style({ 'margin-top': '0' }))], {
        params: { time: '500ms', easing: 'ease' },
    });
    /** @type {?} */
    var expandX = animations.animation([
        animations.style({ width: '0', overflow: 'hidden', 'box-sizing': 'border-box' }),
        animations.animate('{{ time }} {{ easing }}', animations.style({ width: '*', padding: '*' })),
    ], { params: { time: '350ms', easing: 'ease' } });
    /** @type {?} */
    var collapse = animations.trigger('collapse', [
        animations.state('collapsed', animations.style({ height: '0', overflow: 'hidden' })),
        animations.state('expanded', animations.style({ height: '*', overflow: 'hidden' })),
        animations.transition('expanded => collapsed', animations.useAnimation(collapseY)),
        animations.transition('collapsed => expanded', animations.useAnimation(expandY)),
    ]);
    /** @type {?} */
    var collapseWithMargin = animations.trigger('collapseWithMargin', [
        animations.state('collapsed', animations.style({ 'margin-top': '-100%' })),
        animations.state('expanded', animations.style({ 'margin-top': '0' })),
        animations.transition('expanded => collapsed', animations.useAnimation(collapseYWithMargin), {
            params: { time: '400ms', easing: 'linear' },
        }),
        animations.transition('collapsed => expanded', animations.useAnimation(expandYWithMargin)),
    ]);
    /** @type {?} */
    var collapseLinearWithMargin = animations.trigger('collapseLinearWithMargin', [
        animations.state('collapsed', animations.style({ 'margin-top': '-100%' })),
        animations.state('expanded', animations.style({ 'margin-top': '0' })),
        animations.transition('expanded => collapsed', animations.useAnimation(collapseYWithMargin, { params: { time: '200ms', easing: 'linear' } })),
        animations.transition('collapsed => expanded', animations.useAnimation(expandYWithMargin, { params: { time: '250ms', easing: 'linear' } })),
    ]);

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/animations/slide.animations.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /** @type {?} */
    var slideFromBottom = animations.trigger('slideFromBottom', [
        animations.transition('* <=> *', [
            animations.style({ 'margin-top': '20px', opacity: '0' }),
            animations.animate('0.2s ease-out', animations.style({ opacity: '1', 'margin-top': '0px' })),
        ]),
    ]);

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/animations/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/directives/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/models/common.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @record
     */
    function RootParams() { }
    if (false) {
        /** @type {?} */
        RootParams.prototype.httpErrorConfig;
    }
    /**
     * @record
     */
    function HttpErrorConfig() { }
    if (false) {
        /** @type {?|undefined} */
        HttpErrorConfig.prototype.errorScreen;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/models/confirmation.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var Confirmation;
    (function (Confirmation) {
        /**
         * @record
         */
        function Options() { }
        Confirmation.Options = Options;
        if (false) {
            /** @type {?|undefined} */
            Options.prototype.hideCancelBtn;
            /** @type {?|undefined} */
            Options.prototype.hideYesBtn;
            /** @type {?|undefined} */
            Options.prototype.cancelText;
            /** @type {?|undefined} */
            Options.prototype.yesText;
            /**
             * @deprecated to be deleted in v2
             * @type {?|undefined}
             */
            Options.prototype.cancelCopy;
            /**
             * @deprecated to be deleted in v2
             * @type {?|undefined}
             */
            Options.prototype.yesCopy;
        }
    })(Confirmation || (Confirmation = {}));

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/models/setting-management.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @record
     */
    function SettingTab() { }
    if (false) {
        /** @type {?} */
        SettingTab.prototype.component;
        /** @type {?} */
        SettingTab.prototype.name;
        /** @type {?} */
        SettingTab.prototype.order;
        /** @type {?|undefined} */
        SettingTab.prototype.requiredPolicy;
    }
    /** @type {?} */
    var SETTING_TABS = (/** @type {?} */ ([]));
    /**
     * @param {?} tab
     * @return {?}
     */
    function addSettingTab(tab) {
        if (!Array.isArray(tab)) {
            tab = [tab];
        }
        SETTING_TABS.push.apply(SETTING_TABS, __spread(tab));
    }
    /**
     * @return {?}
     */
    function getSettingTabs() {
        return SETTING_TABS;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/models/statistics.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var Statistics;
    (function (Statistics) {
        /**
         * @record
         */
        function Response() { }
        Statistics.Response = Response;
        if (false) {
            /** @type {?} */
            Response.prototype.data;
        }
        /**
         * @record
         */
        function Data() { }
        Statistics.Data = Data;
        /**
         * @record
         */
        function Filter() { }
        Statistics.Filter = Filter;
        if (false) {
            /** @type {?} */
            Filter.prototype.startDate;
            /** @type {?} */
            Filter.prototype.endDate;
        }
    })(Statistics || (Statistics = {}));

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/models/toaster.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    (function (Toaster) {
        /**
         * @record
         */
        function Options() { }
        Toaster.Options = Options;
        if (false) {
            /** @type {?|undefined} */
            Options.prototype.id;
            /** @type {?|undefined} */
            Options.prototype.closable;
            /** @type {?|undefined} */
            Options.prototype.life;
            /** @type {?|undefined} */
            Options.prototype.sticky;
            /** @type {?|undefined} */
            Options.prototype.data;
            /** @type {?|undefined} */
            Options.prototype.messageLocalizationParams;
            /** @type {?|undefined} */
            Options.prototype.titleLocalizationParams;
        }
    })(exports.Toaster || (exports.Toaster = {}));

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/models/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/services/toaster.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ToasterService = /** @class */ (function (_super) {
        __extends(ToasterService, _super);
        function ToasterService(messageService) {
            var _this = _super.call(this, messageService) || this;
            _this.messageService = messageService;
            return _this;
        }
        /**
         * @param {?} messages
         * @return {?}
         */
        ToasterService.prototype.addAll = /**
         * @param {?} messages
         * @return {?}
         */
        function (messages) {
            var _this = this;
            this.messageService.addAll(messages.map((/**
             * @param {?} message
             * @return {?}
             */
            function (message) { return (__assign({ key: _this.key }, message)); })));
        };
        ToasterService.decorators = [
            { type: core.Injectable, args: [{ providedIn: 'root' },] }
        ];
        /** @nocollapse */
        ToasterService.ctorParameters = function () { return [
            { type: messageservice.MessageService }
        ]; };
        /** @nocollapse */ ToasterService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function ToasterService_Factory() { return new ToasterService(core.ɵɵinject(messageservice.MessageService)); }, token: ToasterService, providedIn: "root" });
        return ToasterService;
    }(AbstractToaster));
    if (false) {
        /**
         * @type {?}
         * @protected
         */
        ToasterService.prototype.messageService;
    }

    exports.BreadcrumbComponent = BreadcrumbComponent;
    exports.ButtonComponent = ButtonComponent;
    exports.ChartComponent = ChartComponent;
    exports.ConfirmationComponent = ConfirmationComponent;
    exports.ConfirmationService = ConfirmationService;
    exports.LoaderBarComponent = LoaderBarComponent;
    exports.ModalComponent = ModalComponent;
    exports.SortOrderIconComponent = SortOrderIconComponent;
    exports.TableEmptyMessageComponent = TableEmptyMessageComponent;
    exports.TableSortDirective = TableSortDirective;
    exports.ThemeSharedModule = ThemeSharedModule;
    exports.ToastComponent = ToastComponent;
    exports.ToasterService = ToasterService;
    exports.addSettingTab = addSettingTab;
    exports.appendScript = appendScript;
    exports.bounceIn = bounceIn;
    exports.chartJsLoaded$ = chartJsLoaded$;
    exports.collapse = collapse;
    exports.collapseLinearWithMargin = collapseLinearWithMargin;
    exports.collapseWithMargin = collapseWithMargin;
    exports.collapseX = collapseX;
    exports.collapseY = collapseY;
    exports.collapseYWithMargin = collapseYWithMargin;
    exports.dialogAnimation = dialogAnimation;
    exports.expandX = expandX;
    exports.expandY = expandY;
    exports.expandYWithMargin = expandYWithMargin;
    exports.fadeAnimation = fadeAnimation;
    exports.fadeIn = fadeIn;
    exports.fadeInDown = fadeInDown;
    exports.fadeInLeft = fadeInLeft;
    exports.fadeInRight = fadeInRight;
    exports.fadeInUp = fadeInUp;
    exports.fadeOut = fadeOut;
    exports.fadeOutDown = fadeOutDown;
    exports.fadeOutLeft = fadeOutLeft;
    exports.fadeOutRight = fadeOutRight;
    exports.fadeOutUp = fadeOutUp;
    exports.getRandomBackgroundColor = getRandomBackgroundColor;
    exports.getSettingTabs = getSettingTabs;
    exports.slideFromBottom = slideFromBottom;
    exports.ɵa = BreadcrumbComponent;
    exports.ɵb = ButtonComponent;
    exports.ɵc = ChartComponent;
    exports.ɵd = ConfirmationComponent;
    exports.ɵe = ConfirmationService;
    exports.ɵf = AbstractToaster;
    exports.ɵg = ErrorComponent;
    exports.ɵh = LoaderBarComponent;
    exports.ɵi = ModalComponent;
    exports.ɵj = fadeAnimation;
    exports.ɵk = dialogAnimation;
    exports.ɵl = fadeIn;
    exports.ɵm = fadeOut;
    exports.ɵn = fadeInDown;
    exports.ɵo = TableEmptyMessageComponent;
    exports.ɵp = ToastComponent;
    exports.ɵq = SortOrderIconComponent;
    exports.ɵr = TableSortDirective;
    exports.ɵs = ErrorHandler;
    exports.ɵt = httpErrorConfigFactory;
    exports.ɵu = HTTP_ERROR_CONFIG;

    Object.defineProperty(exports, '__esModule', { value: true });

})));
//# sourceMappingURL=abp-ng.theme.shared.umd.js.map
