"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var TenantService = /** @class */ (function () {
    function TenantService(restService) {
        this.restService = restService;
    }
    TenantService.prototype.get = function (id) {
        return this.restService.request({
            method: 'GET',
            url: "/api/saas/tenants/" + id,
        });
    };
    TenantService.prototype.getList = function () {
        return this.restService.request({
            method: 'GET',
            url: "/api/saas/tenants",
        });
    };
    TenantService.prototype.getDefaultConnectionString = function (id) {
        return this.restService.request({
            method: 'GET',
            url: "/api/saas/tenants/" + id + "/default-connection-string",
        });
    };
    TenantService = __decorate([
        core_1.Injectable({
            providedIn: 'root',
        })
    ], TenantService);
    return TenantService;
}());
exports.TenantService = TenantService;
