"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ServiceTemplates;
(function (ServiceTemplates) {
    function classTemplate(name) {
        "import { RestService } from '@abp/ng.core';\nimport { Injectable } from '@angular/core';\n\n@Injectable({\n  providedIn: 'root',\n})\nexport class " + name + "Service {\n\n  constructor(private restService: RestService) { }\n\n}";
    }
    ServiceTemplates.classTemplate = classTemplate;
})(ServiceTemplates = exports.ServiceTemplates || (exports.ServiceTemplates = {}));
