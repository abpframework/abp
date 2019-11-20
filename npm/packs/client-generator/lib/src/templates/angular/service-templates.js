"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
var change_case_1 = __importDefault(require("change-case"));
var replacer_1 = require("../../utils/replacer");
var ServiceTemplates;
(function (ServiceTemplates) {
    function classTemplate(name, content) {
        return "import { RestService } from '@abp/ng.core';\nimport { Injectable } from '@angular/core';\nimport { Observable } from 'rxjs'\n\n@Injectable({\n  providedIn: 'root',\n})\nexport class " + change_case_1.default.pascalCase(name) + "Service {\n  constructor(private restService: RestService) {}\n  " + content + "\n}";
    }
    ServiceTemplates.classTemplate = classTemplate;
    function getMethodTemplate(name, url, args, params, queryParams) {
        if (args === void 0) { args = ''; }
        if (params === void 0) { params = []; }
        return "\n  " + change_case_1.default.camelCase(replacer_1.replacer(name)) + "(" + args + "): Observable<any> {\n    " + requestTemplate('GET', url, params, false, !!queryParams) + "\n  }";
    }
    ServiceTemplates.getMethodTemplate = getMethodTemplate;
    function requestTemplate(method, url, params, body, queryParams) {
        if (params === void 0) { params = []; }
        params.forEach(function (param) {
            var index = url.indexOf("{" + param.key + "}");
            if (index > -1) {
                url = url.slice(0, index) + '$' + url.slice(index);
            }
        });
        return "return this.restService.request<void, any>({\n      method: '" + method + "',\n      url: `/" + url + "`," + (queryParams ? 'params: queryParams,' : '') + (body ? 'body,' : '') + "\n    });";
    }
    ServiceTemplates.requestTemplate = requestTemplate;
})(ServiceTemplates = exports.ServiceTemplates || (exports.ServiceTemplates = {}));
