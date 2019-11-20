"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
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
};
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
var prompt_1 = require("./utils/prompt");
var axios_1 = require("./utils/axios");
var ora = require("ora");
var angular_1 = require("./angular");
var chalk_1 = __importDefault(require("chalk"));
function cli(program) {
    return __awaiter(this, void 0, void 0, function () {
        var _a, loading, data, apiUrl, error_1, selection, modules, _b;
        var _this = this;
        return __generator(this, function (_c) {
            switch (_c.label) {
                case 0:
                    if (!(program.ui !== 'angular')) return [3 /*break*/, 2];
                    _a = program;
                    return [4 /*yield*/, prompt_1.uiSelection(['Angular'])];
                case 1:
                    _a.ui = (_c.sent()).toLowerCase();
                    _c.label = 2;
                case 2:
                    loading = ora('Waiting for the API response... \n');
                    loading.start();
                    data = {};
                    apiUrl = 'https://localhost:44305/api/abp/api-definition';
                    _c.label = 3;
                case 3:
                    _c.trys.push([3, 5, , 6]);
                    return [4 /*yield*/, axios_1.axiosInstance.get(apiUrl)];
                case 4:
                    data = (_c.sent()).data;
                    return [3 /*break*/, 6];
                case 5:
                    error_1 = _c.sent();
                    console.log(chalk_1.default.red('An error occurred when fetching the ' + apiUrl));
                    process.exit(1);
                    return [3 /*break*/, 6];
                case 6:
                    console.log(data);
                    loading.stop();
                    selection = function (modules) { return __awaiter(_this, void 0, void 0, function () {
                        var selectedModules;
                        return __generator(this, function (_a) {
                            switch (_a.label) {
                                case 0: return [4 /*yield*/, prompt_1.moduleSelection(modules)];
                                case 1:
                                    selectedModules = (_a.sent());
                                    if (!!selectedModules.length) return [3 /*break*/, 3];
                                    console.log(chalk_1.default.red('Please select module(s)'));
                                    return [4 /*yield*/, selection(modules)];
                                case 2: return [2 /*return*/, _a.sent()];
                                case 3: return [2 /*return*/, selectedModules];
                            }
                        });
                    }); };
                    modules = ['saas'];
                    _b = program.ui;
                    switch (_b) {
                        case 'angular': return [3 /*break*/, 7];
                    }
                    return [3 /*break*/, 9];
                case 7: return [4 /*yield*/, angular_1.angular(data, modules)];
                case 8:
                    _c.sent();
                    return [3 /*break*/, 10];
                case 9:
                    process.exit(1);
                    _c.label = 10;
                case 10: return [2 /*return*/];
            }
        });
    });
}
exports.cli = cli;
