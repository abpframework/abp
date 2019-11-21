"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
var change_case_1 = __importDefault(require("change-case"));
var ServiceTemplates;
(function (ServiceTemplates) {
    function classTemplate(name, content) {
        return "import { RestService } from '@abp/ng.core';\nimport { Injectable } from '@angular/core';\n\n@Injectable({\n  providedIn: 'root',\n})\nexport class " + change_case_1.default.pascalCase(name) + "Service {\n\n  constructor(private restService: RestService) { }\n\n  " + content.forEach(function (element) {
            element + '\n\n';
        }) + "\n}";
    }
    ServiceTemplates.classTemplate = classTemplate;
})(ServiceTemplates = exports.ServiceTemplates || (exports.ServiceTemplates = {}));
