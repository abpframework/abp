(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('just-compare')) :
    typeof define === 'function' && define.amd ? define('@abp/utils', ['exports', 'just-compare'], factory) :
    (global = global || self, factory((global.abp = global.abp || {}, global.abp.utils = global.abp.utils || {}, global.abp.utils.common = {}), global.compare));
}(this, (function (exports, compare) { 'use strict';

    compare = compare && Object.prototype.hasOwnProperty.call(compare, 'default') ? compare['default'] : compare;

    /*! *****************************************************************************
    Copyright (c) Microsoft Corporation.

    Permission to use, copy, modify, and/or distribute this software for any
    purpose with or without fee is hereby granted.

    THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES WITH
    REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY
    AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY SPECIAL, DIRECT,
    INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES WHATSOEVER RESULTING FROM
    LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR
    OTHER TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION WITH THE USE OR
    PERFORMANCE OF THIS SOFTWARE.
    ***************************************************************************** */
    /* global Reflect, Promise */
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b)
                if (b.hasOwnProperty(p))
                    d[p] = b[p]; };
        return extendStatics(d, b);
    };
    function __extends(d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    }
    var __assign = function () {
        __assign = Object.assign || function __assign(t) {
            for (var s, i = 1, n = arguments.length; i < n; i++) {
                s = arguments[i];
                for (var p in s)
                    if (Object.prototype.hasOwnProperty.call(s, p))
                        t[p] = s[p];
            }
            return t;
        };
        return __assign.apply(this, arguments);
    };
    function __rest(s, e) {
        var t = {};
        for (var p in s)
            if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0)
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
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function")
            r = Reflect.decorate(decorators, target, key, desc);
        else
            for (var i = decorators.length - 1; i >= 0; i--)
                if (d = decorators[i])
                    r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    }
    function __param(paramIndex, decorator) {
        return function (target, key) { decorator(target, key, paramIndex); };
    }
    function __metadata(metadataKey, metadataValue) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function")
            return Reflect.metadata(metadataKey, metadataValue);
    }
    function __awaiter(thisArg, _arguments, P, generator) {
        function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
        return new (P || (P = Promise))(function (resolve, reject) {
            function fulfilled(value) { try {
                step(generator.next(value));
            }
            catch (e) {
                reject(e);
            } }
            function rejected(value) { try {
                step(generator["throw"](value));
            }
            catch (e) {
                reject(e);
            } }
            function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
            step((generator = generator.apply(thisArg, _arguments || [])).next());
        });
    }
    function __generator(thisArg, body) {
        var _ = { label: 0, sent: function () { if (t[0] & 1)
                throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
        return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function () { return this; }), g;
        function verb(n) { return function (v) { return step([n, v]); }; }
        function step(op) {
            if (f)
                throw new TypeError("Generator is already executing.");
            while (_)
                try {
                    if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done)
                        return t;
                    if (y = 0, t)
                        op = [op[0] & 2, t.value];
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
                            if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) {
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
                            if (t[2])
                                _.ops.pop();
                            _.trys.pop();
                            continue;
                    }
                    op = body.call(thisArg, _);
                }
                catch (e) {
                    op = [6, e];
                    y = 0;
                }
                finally {
                    f = t = 0;
                }
            if (op[0] & 5)
                throw op[1];
            return { value: op[0] ? op[1] : void 0, done: true };
        }
    }
    var __createBinding = Object.create ? (function (o, m, k, k2) {
        if (k2 === undefined)
            k2 = k;
        Object.defineProperty(o, k2, { enumerable: true, get: function () { return m[k]; } });
    }) : (function (o, m, k, k2) {
        if (k2 === undefined)
            k2 = k;
        o[k2] = m[k];
    });
    function __exportStar(m, exports) {
        for (var p in m)
            if (p !== "default" && !exports.hasOwnProperty(p))
                __createBinding(exports, m, p);
    }
    function __values(o) {
        var s = typeof Symbol === "function" && Symbol.iterator, m = s && o[s], i = 0;
        if (m)
            return m.call(o);
        if (o && typeof o.length === "number")
            return {
                next: function () {
                    if (o && i >= o.length)
                        o = void 0;
                    return { value: o && o[i++], done: !o };
                }
            };
        throw new TypeError(s ? "Object is not iterable." : "Symbol.iterator is not defined.");
    }
    function __read(o, n) {
        var m = typeof Symbol === "function" && o[Symbol.iterator];
        if (!m)
            return o;
        var i = m.call(o), r, ar = [], e;
        try {
            while ((n === void 0 || n-- > 0) && !(r = i.next()).done)
                ar.push(r.value);
        }
        catch (error) {
            e = { error: error };
        }
        finally {
            try {
                if (r && !r.done && (m = i["return"]))
                    m.call(i);
            }
            finally {
                if (e)
                    throw e.error;
            }
        }
        return ar;
    }
    function __spread() {
        for (var ar = [], i = 0; i < arguments.length; i++)
            ar = ar.concat(__read(arguments[i]));
        return ar;
    }
    function __spreadArrays() {
        for (var s = 0, i = 0, il = arguments.length; i < il; i++)
            s += arguments[i].length;
        for (var r = Array(s), k = 0, i = 0; i < il; i++)
            for (var a = arguments[i], j = 0, jl = a.length; j < jl; j++, k++)
                r[k] = a[j];
        return r;
    }
    ;
    function __await(v) {
        return this instanceof __await ? (this.v = v, this) : new __await(v);
    }
    function __asyncGenerator(thisArg, _arguments, generator) {
        if (!Symbol.asyncIterator)
            throw new TypeError("Symbol.asyncIterator is not defined.");
        var g = generator.apply(thisArg, _arguments || []), i, q = [];
        return i = {}, verb("next"), verb("throw"), verb("return"), i[Symbol.asyncIterator] = function () { return this; }, i;
        function verb(n) { if (g[n])
            i[n] = function (v) { return new Promise(function (a, b) { q.push([n, v, a, b]) > 1 || resume(n, v); }); }; }
        function resume(n, v) { try {
            step(g[n](v));
        }
        catch (e) {
            settle(q[0][3], e);
        } }
        function step(r) { r.value instanceof __await ? Promise.resolve(r.value.v).then(fulfill, reject) : settle(q[0][2], r); }
        function fulfill(value) { resume("next", value); }
        function reject(value) { resume("throw", value); }
        function settle(f, v) { if (f(v), q.shift(), q.length)
            resume(q[0][0], q[0][1]); }
    }
    function __asyncDelegator(o) {
        var i, p;
        return i = {}, verb("next"), verb("throw", function (e) { throw e; }), verb("return"), i[Symbol.iterator] = function () { return this; }, i;
        function verb(n, f) { i[n] = o[n] ? function (v) { return (p = !p) ? { value: __await(o[n](v)), done: n === "return" } : f ? f(v) : v; } : f; }
    }
    function __asyncValues(o) {
        if (!Symbol.asyncIterator)
            throw new TypeError("Symbol.asyncIterator is not defined.");
        var m = o[Symbol.asyncIterator], i;
        return m ? m.call(o) : (o = typeof __values === "function" ? __values(o) : o[Symbol.iterator](), i = {}, verb("next"), verb("throw"), verb("return"), i[Symbol.asyncIterator] = function () { return this; }, i);
        function verb(n) { i[n] = o[n] && function (v) { return new Promise(function (resolve, reject) { v = o[n](v), settle(resolve, reject, v.done, v.value); }); }; }
        function settle(resolve, reject, d, v) { Promise.resolve(v).then(function (v) { resolve({ value: v, done: d }); }, reject); }
    }
    function __makeTemplateObject(cooked, raw) {
        if (Object.defineProperty) {
            Object.defineProperty(cooked, "raw", { value: raw });
        }
        else {
            cooked.raw = raw;
        }
        return cooked;
    }
    ;
    var __setModuleDefault = Object.create ? (function (o, v) {
        Object.defineProperty(o, "default", { enumerable: true, value: v });
    }) : function (o, v) {
        o["default"] = v;
    };
    function __importStar(mod) {
        if (mod && mod.__esModule)
            return mod;
        var result = {};
        if (mod != null)
            for (var k in mod)
                if (Object.hasOwnProperty.call(mod, k))
                    __createBinding(result, mod, k);
        __setModuleDefault(result, mod);
        return result;
    }
    function __importDefault(mod) {
        return (mod && mod.__esModule) ? mod : { default: mod };
    }
    function __classPrivateFieldGet(receiver, privateMap) {
        if (!privateMap.has(receiver)) {
            throw new TypeError("attempted to get private field on non-instance");
        }
        return privateMap.get(receiver);
    }
    function __classPrivateFieldSet(receiver, privateMap, value) {
        if (!privateMap.has(receiver)) {
            throw new TypeError("attempted to set private field on non-instance");
        }
        privateMap.set(receiver, value);
        return value;
    }

    var ListNode = /** @class */ (function () {
        function ListNode(value) {
            this.value = value;
        }
        return ListNode;
    }());
    var LinkedList = /** @class */ (function () {
        function LinkedList() {
            this.size = 0;
        }
        Object.defineProperty(LinkedList.prototype, "head", {
            get: function () {
                return this.first;
            },
            enumerable: false,
            configurable: true
        });
        Object.defineProperty(LinkedList.prototype, "tail", {
            get: function () {
                return this.last;
            },
            enumerable: false,
            configurable: true
        });
        Object.defineProperty(LinkedList.prototype, "length", {
            get: function () {
                return this.size;
            },
            enumerable: false,
            configurable: true
        });
        LinkedList.prototype.attach = function (value, previousNode, nextNode) {
            if (!previousNode)
                return this.addHead(value);
            if (!nextNode)
                return this.addTail(value);
            var node = new ListNode(value);
            node.previous = previousNode;
            previousNode.next = node;
            node.next = nextNode;
            nextNode.previous = node;
            this.size++;
            return node;
        };
        LinkedList.prototype.attachMany = function (values, previousNode, nextNode) {
            if (!values.length)
                return [];
            if (!previousNode)
                return this.addManyHead(values);
            if (!nextNode)
                return this.addManyTail(values);
            var list = new LinkedList();
            list.addManyTail(values);
            list.first.previous = previousNode;
            previousNode.next = list.first;
            list.last.next = nextNode;
            nextNode.previous = list.last;
            this.size += values.length;
            return list.toNodeArray();
        };
        LinkedList.prototype.detach = function (node) {
            if (!node.previous)
                return this.dropHead();
            if (!node.next)
                return this.dropTail();
            node.previous.next = node.next;
            node.next.previous = node.previous;
            this.size--;
            return node;
        };
        LinkedList.prototype.add = function (value) {
            var _this = this;
            return {
                after: function () {
                    var _a;
                    var params = [];
                    for (var _i = 0; _i < arguments.length; _i++) {
                        params[_i] = arguments[_i];
                    }
                    return (_a = _this.addAfter).call.apply(_a, __spread([_this, value], params));
                },
                before: function () {
                    var _a;
                    var params = [];
                    for (var _i = 0; _i < arguments.length; _i++) {
                        params[_i] = arguments[_i];
                    }
                    return (_a = _this.addBefore).call.apply(_a, __spread([_this, value], params));
                },
                byIndex: function (position) { return _this.addByIndex(value, position); },
                head: function () { return _this.addHead(value); },
                tail: function () { return _this.addTail(value); },
            };
        };
        LinkedList.prototype.addMany = function (values) {
            var _this = this;
            return {
                after: function () {
                    var _a;
                    var params = [];
                    for (var _i = 0; _i < arguments.length; _i++) {
                        params[_i] = arguments[_i];
                    }
                    return (_a = _this.addManyAfter).call.apply(_a, __spread([_this, values], params));
                },
                before: function () {
                    var _a;
                    var params = [];
                    for (var _i = 0; _i < arguments.length; _i++) {
                        params[_i] = arguments[_i];
                    }
                    return (_a = _this.addManyBefore).call.apply(_a, __spread([_this, values], params));
                },
                byIndex: function (position) { return _this.addManyByIndex(values, position); },
                head: function () { return _this.addManyHead(values); },
                tail: function () { return _this.addManyTail(values); },
            };
        };
        LinkedList.prototype.addAfter = function (value, previousValue, compareFn) {
            if (compareFn === void 0) { compareFn = compare; }
            var previous = this.find(function (node) { return compareFn(node.value, previousValue); });
            return previous ? this.attach(value, previous, previous.next) : this.addTail(value);
        };
        LinkedList.prototype.addBefore = function (value, nextValue, compareFn) {
            if (compareFn === void 0) { compareFn = compare; }
            var next = this.find(function (node) { return compareFn(node.value, nextValue); });
            return next ? this.attach(value, next.previous, next) : this.addHead(value);
        };
        LinkedList.prototype.addByIndex = function (value, position) {
            if (position < 0)
                position += this.size;
            else if (position >= this.size)
                return this.addTail(value);
            if (position <= 0)
                return this.addHead(value);
            var next = this.get(position);
            return this.attach(value, next.previous, next);
        };
        LinkedList.prototype.addHead = function (value) {
            var node = new ListNode(value);
            node.next = this.first;
            if (this.first)
                this.first.previous = node;
            else
                this.last = node;
            this.first = node;
            this.size++;
            return node;
        };
        LinkedList.prototype.addTail = function (value) {
            var node = new ListNode(value);
            if (this.first) {
                node.previous = this.last;
                this.last.next = node;
                this.last = node;
            }
            else {
                this.first = node;
                this.last = node;
            }
            this.size++;
            return node;
        };
        LinkedList.prototype.addManyAfter = function (values, previousValue, compareFn) {
            if (compareFn === void 0) { compareFn = compare; }
            var previous = this.find(function (node) { return compareFn(node.value, previousValue); });
            return previous ? this.attachMany(values, previous, previous.next) : this.addManyTail(values);
        };
        LinkedList.prototype.addManyBefore = function (values, nextValue, compareFn) {
            if (compareFn === void 0) { compareFn = compare; }
            var next = this.find(function (node) { return compareFn(node.value, nextValue); });
            return next ? this.attachMany(values, next.previous, next) : this.addManyHead(values);
        };
        LinkedList.prototype.addManyByIndex = function (values, position) {
            if (position < 0)
                position += this.size;
            if (position <= 0)
                return this.addManyHead(values);
            if (position >= this.size)
                return this.addManyTail(values);
            var next = this.get(position);
            return this.attachMany(values, next.previous, next);
        };
        LinkedList.prototype.addManyHead = function (values) {
            var _this = this;
            return values.reduceRight(function (nodes, value) {
                nodes.unshift(_this.addHead(value));
                return nodes;
            }, []);
        };
        LinkedList.prototype.addManyTail = function (values) {
            var _this = this;
            return values.map(function (value) { return _this.addTail(value); });
        };
        LinkedList.prototype.drop = function () {
            var _this = this;
            return {
                byIndex: function (position) { return _this.dropByIndex(position); },
                byValue: function () {
                    var params = [];
                    for (var _i = 0; _i < arguments.length; _i++) {
                        params[_i] = arguments[_i];
                    }
                    return _this.dropByValue.apply(_this, params);
                },
                byValueAll: function () {
                    var params = [];
                    for (var _i = 0; _i < arguments.length; _i++) {
                        params[_i] = arguments[_i];
                    }
                    return _this.dropByValueAll.apply(_this, params);
                },
                head: function () { return _this.dropHead(); },
                tail: function () { return _this.dropTail(); },
            };
        };
        LinkedList.prototype.dropMany = function (count) {
            var _this = this;
            return {
                byIndex: function (position) { return _this.dropManyByIndex(count, position); },
                head: function () { return _this.dropManyHead(count); },
                tail: function () { return _this.dropManyTail(count); },
            };
        };
        LinkedList.prototype.dropByIndex = function (position) {
            if (position < 0)
                position += this.size;
            var current = this.get(position);
            return current ? this.detach(current) : undefined;
        };
        LinkedList.prototype.dropByValue = function (value, compareFn) {
            if (compareFn === void 0) { compareFn = compare; }
            var position = this.findIndex(function (node) { return compareFn(node.value, value); });
            return position < 0 ? undefined : this.dropByIndex(position);
        };
        LinkedList.prototype.dropByValueAll = function (value, compareFn) {
            if (compareFn === void 0) { compareFn = compare; }
            var dropped = [];
            for (var current = this.first, position = 0; current; position++, current = current.next) {
                if (compareFn(current.value, value)) {
                    dropped.push(this.dropByIndex(position - dropped.length));
                }
            }
            return dropped;
        };
        LinkedList.prototype.dropHead = function () {
            var head = this.first;
            if (head) {
                this.first = head.next;
                if (this.first)
                    this.first.previous = undefined;
                else
                    this.last = undefined;
                this.size--;
                return head;
            }
            return undefined;
        };
        LinkedList.prototype.dropTail = function () {
            var tail = this.last;
            if (tail) {
                this.last = tail.previous;
                if (this.last)
                    this.last.next = undefined;
                else
                    this.first = undefined;
                this.size--;
                return tail;
            }
            return undefined;
        };
        LinkedList.prototype.dropManyByIndex = function (count, position) {
            if (count <= 0)
                return [];
            if (position < 0)
                position = Math.max(position + this.size, 0);
            else if (position >= this.size)
                return [];
            count = Math.min(count, this.size - position);
            var dropped = [];
            while (count--) {
                var current = this.get(position);
                dropped.push(this.detach(current));
            }
            return dropped;
        };
        LinkedList.prototype.dropManyHead = function (count) {
            if (count <= 0)
                return [];
            count = Math.min(count, this.size);
            var dropped = [];
            while (count--)
                dropped.unshift(this.dropHead());
            return dropped;
        };
        LinkedList.prototype.dropManyTail = function (count) {
            if (count <= 0)
                return [];
            count = Math.min(count, this.size);
            var dropped = [];
            while (count--)
                dropped.push(this.dropTail());
            return dropped;
        };
        LinkedList.prototype.find = function (predicate) {
            for (var current = this.first, position = 0; current; position++, current = current.next) {
                if (predicate(current, position, this))
                    return current;
            }
            return undefined;
        };
        LinkedList.prototype.findIndex = function (predicate) {
            for (var current = this.first, position = 0; current; position++, current = current.next) {
                if (predicate(current, position, this))
                    return position;
            }
            return -1;
        };
        LinkedList.prototype.forEach = function (iteratorFn) {
            for (var node = this.first, position = 0; node; position++, node = node.next) {
                iteratorFn(node, position, this);
            }
        };
        LinkedList.prototype.get = function (position) {
            return this.find(function (_, index) { return position === index; });
        };
        LinkedList.prototype.indexOf = function (value, compareFn) {
            if (compareFn === void 0) { compareFn = compare; }
            return this.findIndex(function (node) { return compareFn(node.value, value); });
        };
        LinkedList.prototype.toArray = function () {
            var array = new Array(this.size);
            this.forEach(function (node, index) { return (array[index] = node.value); });
            return array;
        };
        LinkedList.prototype.toNodeArray = function () {
            var array = new Array(this.size);
            this.forEach(function (node, index) { return (array[index] = node); });
            return array;
        };
        LinkedList.prototype.toString = function (mapperFn) {
            if (mapperFn === void 0) { mapperFn = JSON.stringify; }
            return this.toArray()
                .map(function (value) { return mapperFn(value); })
                .join(' <-> ');
        };
        // Cannot use Generator type because of ng-packagr
        LinkedList.prototype[Symbol.iterator] = function () {
            var node, position;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        node = this.first, position = 0;
                        _a.label = 1;
                    case 1:
                        if (!node) return [3 /*break*/, 4];
                        return [4 /*yield*/, node.value];
                    case 2:
                        _a.sent();
                        _a.label = 3;
                    case 3:
                        position++, node = node.next;
                        return [3 /*break*/, 1];
                    case 4: return [2 /*return*/];
                }
            });
        };
        return LinkedList;
    }());

    /*
     * Public API Surface of utils
     */

    /**
     * Generated bundle index. Do not edit.
     */

    exports.LinkedList = LinkedList;
    exports.ListNode = ListNode;

    Object.defineProperty(exports, '__esModule', { value: true });

})));
//# sourceMappingURL=abp-utils.umd.js.map
