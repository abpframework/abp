"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var EditionService = /** @class */ (function () {
    function EditionService(restService) {
        this.restService = restService;
    }
    EditionService.prototype.get = function (id) {
        return this.restService.request({
            method: 'GET',
            url: "/api/saas/editions/" + id,
        });
    };
    EditionService.prototype.getList = function () {
        return this.restService.request({
            method: 'GET',
            url: "/api/saas/editions",
        });
    };
    EditionService.prototype.getUsageStatistics = function () {
        return this.restService.request({
            method: 'GET',
            url: "/api/saas/editions/statistics/usage-statistic",
        });
    };
    EditionService = __decorate([
        core_1.Injectable({
            providedIn: 'root',
        })
    ], EditionService);
    return EditionService;
}());
exports.EditionService = EditionService;
