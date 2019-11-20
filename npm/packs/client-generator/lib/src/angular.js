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
var service_templates_1 = require("./templates/angular/service-templates");
var change_case_1 = __importDefault(require("change-case"));
var fs_extra_1 = __importDefault(require("fs-extra"));
var generators_1 = require("./utils/generators");
function angular(data, selectedModules) {
    return __awaiter(this, void 0, void 0, function () {
        var _this = this;
        return __generator(this, function (_a) {
            selectedModules.forEach(function (module) { return __awaiter(_this, void 0, void 0, function () {
                var element;
                return __generator(this, function (_a) {
                    element = data.modules[module];
                    (Object.keys(element.controllers) || []).forEach(function (key) {
                        var controller = element.controllers[key];
                        var actions = element.controllers[key].actions;
                        var actionKeys = Object.keys(actions);
                        var contents = [];
                        actionKeys.forEach(function (key) {
                            var element = actions[key];
                            console.log(element);
                            var parameters = generators_1.parseParameters(element.parameters);
                            switch (element.httpMethod) {
                                case 'GET':
                                    contents.push(service_templates_1.ServiceTemplates.getMethodTemplate(element.name, element.url, generators_1.generateArgs(parameters), parameters));
                                    break;
                                default:
                                    break;
                            }
                        });
                        var service = service_templates_1.ServiceTemplates.classTemplate(controller.controllerName, contents.join('\n'));
                        fs_extra_1.default.writeFileSync("dist/" + change_case_1.default.kebabCase(controller.controllerName) + ".service.ts", service);
                    });
                    return [2 /*return*/];
                });
            }); });
            return [2 /*return*/];
        });
    });
}
exports.angular = angular;
