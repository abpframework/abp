"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
var axios_1 = __importDefault(require("axios"));
var https = require("https");
var httpsAgent = new https.Agent({
    rejectUnauthorized: false,
});
exports.axiosInstance = axios_1.default.create({ httpsAgent: httpsAgent });
exports.axiosInstance.interceptors.request.use(function (config) {
    if (config.method !== 'OPTIONS') {
        if (!config.headers['content-type']) {
            config.headers['accept'] = 'application/json';
        }
    }
    return config;
}, function (error) {
    // Do something with request error
    return Promise.reject(error);
});
// axiosInstance.get = (...ar) => new Promise(resolve => {
//   setTimeout(() => {
//     resolve({
//       "modules": {
//         "Account": {
//           "rootPath": "Account",
//           "controllers": {
//             "Volo.Abp.Account.Web.Areas.Account.Controllers.AccountController": {
//               "controllerName": "Account",
//               "typeAsString": "Volo.Abp.Account.Web.Areas.Account.Controllers.AccountController, Volo.Abp.Account.Web",
//               "interfaces": [],
//               "actions": {
//                 "LoginByLogin": {
//                   "uniqueName": "LoginByLogin",
//                   "name": "Login",
//                   "httpMethod": "POST",
//                   "url": "api/account/login",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "login",
//                       "typeAsString": "Volo.Abp.Account.Web.Areas.Account.Controllers.Models.UserLoginInfo, Volo.Abp.Account.Web",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "login",
//                       "name": "login",
//                       "typeAsString": "Volo.Abp.Account.Web.Areas.Account.Controllers.Models.UserLoginInfo, Volo.Abp.Account.Web",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "Body"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Account.Web.Areas.Account.Controllers.Models.AbpLoginResult, Volo.Abp.Account.Web"
//                   }
//                 },
//                 "CheckPasswordByLogin": {
//                   "uniqueName": "CheckPasswordByLogin",
//                   "name": "CheckPassword",
//                   "httpMethod": "POST",
//                   "url": "api/account/checkPassword",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "login",
//                       "typeAsString": "Volo.Abp.Account.Web.Areas.Account.Controllers.Models.UserLoginInfo, Volo.Abp.Account.Web",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "login",
//                       "name": "login",
//                       "typeAsString": "Volo.Abp.Account.Web.Areas.Account.Controllers.Models.UserLoginInfo, Volo.Abp.Account.Web",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "Body"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Account.Web.Areas.Account.Controllers.Models.AbpLoginResult, Volo.Abp.Account.Web"
//                   }
//                 }
//               }
//             }
//           }
//         },
//         "app": {
//           "rootPath": "app",
//           "controllers": {
//             "Acme.AngSecTest.Controllers.TestController": {
//               "controllerName": "Test",
//               "typeAsString": "Acme.AngSecTest.Controllers.TestController, Acme.AngSecTest.HttpApi",
//               "interfaces": [],
//               "actions": {
//                 "GetAsync": {
//                   "uniqueName": "GetAsync",
//                   "name": "GetAsync",
//                   "httpMethod": "GET",
//                   "url": "api/test",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [],
//                   "parameters": [],
//                   "returnValue": {
//                     "typeAsString": "System.Collections.Generic.List`1[[Acme.AngSecTest.Models.Test.TestModel, Acme.AngSecTest.HttpApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib"
//                   }
//                 }
//               }
//             },
//             "Acme.AngSecTest.Controllers.Automobiles.carController": {
//               "controllerName": "car",
//               "typeAsString": "Acme.AngSecTest.Controllers.Automobiles.carController, Acme.AngSecTest.HttpApi",
//               "interfaces": [
//                 {
//                   "typeAsString": "Acme.AngSecTest.Automobiles.IcarAppService, Acme.AngSecTest.Application.Contracts"
//                 }
//               ],
//               "actions": {
//                 "GetAsyncById": {
//                   "uniqueName": "GetAsyncById",
//                   "name": "GetAsync",
//                   "httpMethod": "GET",
//                   "url": "api/car/{id}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Acme.AngSecTest.Automobiles.carDto, Acme.AngSecTest.Application.Contracts"
//                   }
//                 },
//                 "GetListAsyncByInput": {
//                   "uniqueName": "GetListAsyncByInput",
//                   "name": "GetListAsync",
//                   "httpMethod": "GET",
//                   "url": "api/car",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "input",
//                       "typeAsString": "Acme.AngSecTest.Automobiles.GetcarsInput, Acme.AngSecTest.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "input",
//                       "name": "FilterText",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "Name",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "YearMin",
//                       "typeAsString": "System.Nullable`1[[System.Int64, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "YearMax",
//                       "typeAsString": "System.Nullable`1[[System.Int64, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "ProductionDateMin",
//                       "typeAsString": "System.Nullable`1[[System.DateTime, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "ProductionDateMax",
//                       "typeAsString": "System.Nullable`1[[System.DateTime, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "IsStillGoing",
//                       "typeAsString": "System.Nullable`1[[System.Boolean, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "Sorting",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "SkipCount",
//                       "typeAsString": "System.Int32, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "MaxResultCount",
//                       "typeAsString": "System.Int32, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Application.Dtos.PagedResultDto`1[[Acme.AngSecTest.Automobiles.carDto, Acme.AngSecTest.Application.Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], Volo.Abp.Ddd.Application.Contracts"
//                   }
//                 },
//                 "CreateAsyncByInput": {
//                   "uniqueName": "CreateAsyncByInput",
//                   "name": "CreateAsync",
//                   "httpMethod": "POST",
//                   "url": "api/car",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "input",
//                       "typeAsString": "Acme.AngSecTest.Automobiles.carCreateDto, Acme.AngSecTest.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "input",
//                       "name": "input",
//                       "typeAsString": "Acme.AngSecTest.Automobiles.carCreateDto, Acme.AngSecTest.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "Body"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Acme.AngSecTest.Automobiles.carDto, Acme.AngSecTest.Application.Contracts"
//                   }
//                 },
//                 "UpdateAsyncByIdAndInput": {
//                   "uniqueName": "UpdateAsyncByIdAndInput",
//                   "name": "UpdateAsync",
//                   "httpMethod": "PUT",
//                   "url": "api/car/{id}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     },
//                     {
//                       "name": "input",
//                       "typeAsString": "Acme.AngSecTest.Automobiles.carUpdateDto, Acme.AngSecTest.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "input",
//                       "typeAsString": "Acme.AngSecTest.Automobiles.carUpdateDto, Acme.AngSecTest.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "Body"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Acme.AngSecTest.Automobiles.carDto, Acme.AngSecTest.Application.Contracts"
//                   }
//                 },
//                 "DeleteAsyncById": {
//                   "uniqueName": "DeleteAsyncById",
//                   "name": "DeleteAsync",
//                   "httpMethod": "DELETE",
//                   "url": "api/car/{id}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "System.Void, System.Private.CoreLib"
//                   }
//                 }
//               }
//             },
//             "Acme.AngSecTest.AppServices.Automobiles.carAppService": {
//               "controllerName": "car",
//               "typeAsString": "Acme.AngSecTest.AppServices.Automobiles.carAppService, Acme.AngSecTest.Application",
//               "interfaces": [
//                 {
//                   "typeAsString": "Volo.Abp.Validation.IValidationEnabled, Volo.Abp.Validation"
//                 },
//                 {
//                   "typeAsString": "Volo.Abp.Auditing.IAuditingEnabled, Volo.Abp.Auditing"
//                 },
//                 {
//                   "typeAsString": "Acme.AngSecTest.Automobiles.IcarAppService, Acme.AngSecTest.Application.Contracts"
//                 }
//               ],
//               "actions": {
//                 "GetListAsyncByInput": {
//                   "uniqueName": "GetListAsyncByInput",
//                   "name": "GetListAsync",
//                   "httpMethod": "GET",
//                   "url": "api/app/car",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "input",
//                       "typeAsString": "Acme.AngSecTest.Automobiles.GetcarsInput, Acme.AngSecTest.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "input",
//                       "name": "FilterText",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "Name",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "YearMin",
//                       "typeAsString": "System.Nullable`1[[System.Int64, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "YearMax",
//                       "typeAsString": "System.Nullable`1[[System.Int64, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "ProductionDateMin",
//                       "typeAsString": "System.Nullable`1[[System.DateTime, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "ProductionDateMax",
//                       "typeAsString": "System.Nullable`1[[System.DateTime, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "IsStillGoing",
//                       "typeAsString": "System.Nullable`1[[System.Boolean, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "Sorting",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "SkipCount",
//                       "typeAsString": "System.Int32, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "MaxResultCount",
//                       "typeAsString": "System.Int32, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Application.Dtos.PagedResultDto`1[[Acme.AngSecTest.Automobiles.carDto, Acme.AngSecTest.Application.Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], Volo.Abp.Ddd.Application.Contracts"
//                   }
//                 },
//                 "GetAsyncById": {
//                   "uniqueName": "GetAsyncById",
//                   "name": "GetAsync",
//                   "httpMethod": "GET",
//                   "url": "api/app/car/{id}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Acme.AngSecTest.Automobiles.carDto, Acme.AngSecTest.Application.Contracts"
//                   }
//                 },
//                 "DeleteAsyncById": {
//                   "uniqueName": "DeleteAsyncById",
//                   "name": "DeleteAsync",
//                   "httpMethod": "DELETE",
//                   "url": "api/app/car/{id}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "System.Void, System.Private.CoreLib"
//                   }
//                 },
//                 "CreateAsyncByInput": {
//                   "uniqueName": "CreateAsyncByInput",
//                   "name": "CreateAsync",
//                   "httpMethod": "POST",
//                   "url": "api/app/car",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "input",
//                       "typeAsString": "Acme.AngSecTest.Automobiles.carCreateDto, Acme.AngSecTest.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "input",
//                       "name": "input",
//                       "typeAsString": "Acme.AngSecTest.Automobiles.carCreateDto, Acme.AngSecTest.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "Body"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Acme.AngSecTest.Automobiles.carDto, Acme.AngSecTest.Application.Contracts"
//                   }
//                 },
//                 "UpdateAsyncByIdAndInput": {
//                   "uniqueName": "UpdateAsyncByIdAndInput",
//                   "name": "UpdateAsync",
//                   "httpMethod": "PUT",
//                   "url": "api/app/car/{id}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     },
//                     {
//                       "name": "input",
//                       "typeAsString": "Acme.AngSecTest.Automobiles.carUpdateDto, Acme.AngSecTest.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "input",
//                       "typeAsString": "Acme.AngSecTest.Automobiles.carUpdateDto, Acme.AngSecTest.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "Body"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Acme.AngSecTest.Automobiles.carDto, Acme.AngSecTest.Application.Contracts"
//                   }
//                 }
//               }
//             },
//             "Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.AbpApplicationConfigurationController": {
//               "controllerName": "AbpApplicationConfiguration",
//               "typeAsString": "Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.AbpApplicationConfigurationController, Volo.Abp.AspNetCore.Mvc",
//               "interfaces": [
//                 {
//                   "typeAsString": "Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.IAbpApplicationConfigurationAppService, Volo.Abp.AspNetCore.Mvc.Contracts"
//                 }
//               ],
//               "actions": {
//                 "GetAsync": {
//                   "uniqueName": "GetAsync",
//                   "name": "GetAsync",
//                   "httpMethod": "GET",
//                   "url": "api/abp/application-configuration",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [],
//                   "parameters": [],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ApplicationConfigurationDto, Volo.Abp.AspNetCore.Mvc.Contracts"
//                   }
//                 }
//               }
//             },
//             "Volo.Abp.AspNetCore.Mvc.ApiExploring.AbpApiDefinitionController": {
//               "controllerName": "AbpApiDefinition",
//               "typeAsString": "Volo.Abp.AspNetCore.Mvc.ApiExploring.AbpApiDefinitionController, Volo.Abp.AspNetCore.Mvc",
//               "interfaces": [],
//               "actions": {
//                 "Get": {
//                   "uniqueName": "Get",
//                   "name": "Get",
//                   "httpMethod": "GET",
//                   "url": "api/abp/api-definition",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [],
//                   "parameters": [],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Http.Modeling.ApplicationApiDescriptionModel, Volo.Abp.Http"
//                   }
//                 }
//               }
//             },
//             "Pages.Abp.MultiTenancy.AbpTenantController": {
//               "controllerName": "AbpTenant",
//               "typeAsString": "Pages.Abp.MultiTenancy.AbpTenantController, Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy",
//               "interfaces": [
//                 {
//                   "typeAsString": "Volo.Abp.AspNetCore.Mvc.MultiTenancy.IAbpTenantAppService, Volo.Abp.AspNetCore.Mvc.Contracts"
//                 }
//               ],
//               "actions": {
//                 "FindTenantByNameAsyncByName": {
//                   "uniqueName": "FindTenantByNameAsyncByName",
//                   "name": "FindTenantByNameAsync",
//                   "httpMethod": "GET",
//                   "url": "api/abp/multi-tenancy/find-tenant/{name}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "name",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "name",
//                       "name": "name",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.AspNetCore.Mvc.MultiTenancy.FindTenantResultDto, Volo.Abp.AspNetCore.Mvc.Contracts"
//                   }
//                 },
//                 "FindTenantByIdAsyncById": {
//                   "uniqueName": "FindTenantByIdAsyncById",
//                   "name": "FindTenantByIdAsync",
//                   "httpMethod": "GET",
//                   "url": "api/abp/multi-tenancy/tenants/by-id/{id}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.AspNetCore.Mvc.MultiTenancy.FindTenantResultDto, Volo.Abp.AspNetCore.Mvc.Contracts"
//                   }
//                 }
//               }
//             }
//           }
//         },
//         "account": {
//           "rootPath": "account",
//           "controllers": {
//             "Volo.Abp.Account.AccountController": {
//               "controllerName": "Account",
//               "typeAsString": "Volo.Abp.Account.AccountController, Volo.Abp.Account.HttpApi",
//               "interfaces": [
//                 {
//                   "typeAsString": "Volo.Abp.Account.IAccountAppService, Volo.Abp.Account.Application.Contracts"
//                 }
//               ],
//               "actions": {
//                 "RegisterAsyncByInput": {
//                   "uniqueName": "RegisterAsyncByInput",
//                   "name": "RegisterAsync",
//                   "httpMethod": "POST",
//                   "url": "api/account/register",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.Account.RegisterDto, Volo.Abp.Account.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "input",
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.Account.RegisterDto, Volo.Abp.Account.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "Body"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Identity.IdentityUserDto, Volo.Abp.Identity.Application.Contracts"
//                   }
//                 }
//               }
//             }
//           }
//         },
//         "abp": {
//           "rootPath": "abp",
//           "controllers": {
//             "Volo.Abp.FeatureManagement.FeaturesController": {
//               "controllerName": "Features",
//               "typeAsString": "Volo.Abp.FeatureManagement.FeaturesController, Volo.Abp.FeatureManagement.HttpApi",
//               "interfaces": [
//                 {
//                   "typeAsString": "Volo.Abp.FeatureManagement.IFeatureAppService, Volo.Abp.FeatureManagement.Application.Contracts"
//                 }
//               ],
//               "actions": {
//                 "GetAsyncByProviderNameAndProviderKey": {
//                   "uniqueName": "GetAsyncByProviderNameAndProviderKey",
//                   "name": "GetAsync",
//                   "httpMethod": "GET",
//                   "url": "api/abp/features",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "providerName",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     },
//                     {
//                       "name": "providerKey",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "providerName",
//                       "name": "providerName",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "providerKey",
//                       "name": "providerKey",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.FeatureManagement.FeatureListDto, Volo.Abp.FeatureManagement.Application.Contracts"
//                   }
//                 },
//                 "UpdateAsyncByProviderNameAndProviderKeyAndInput": {
//                   "uniqueName": "UpdateAsyncByProviderNameAndProviderKeyAndInput",
//                   "name": "UpdateAsync",
//                   "httpMethod": "PUT",
//                   "url": "api/abp/features",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "providerName",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     },
//                     {
//                       "name": "providerKey",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     },
//                     {
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.FeatureManagement.UpdateFeaturesDto, Volo.Abp.FeatureManagement.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "providerName",
//                       "name": "providerName",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "providerKey",
//                       "name": "providerKey",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.FeatureManagement.UpdateFeaturesDto, Volo.Abp.FeatureManagement.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "Body"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "System.Void, System.Private.CoreLib"
//                   }
//                 }
//               }
//             },
//             "Volo.Abp.PermissionManagement.PermissionsController": {
//               "controllerName": "Permissions",
//               "typeAsString": "Volo.Abp.PermissionManagement.PermissionsController, Volo.Abp.PermissionManagement.HttpApi",
//               "interfaces": [
//                 {
//                   "typeAsString": "Volo.Abp.PermissionManagement.IPermissionAppService, Volo.Abp.PermissionManagement.Application.Contracts"
//                 }
//               ],
//               "actions": {
//                 "GetAsyncByProviderNameAndProviderKey": {
//                   "uniqueName": "GetAsyncByProviderNameAndProviderKey",
//                   "name": "GetAsync",
//                   "httpMethod": "GET",
//                   "url": "api/abp/permissions",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "providerName",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     },
//                     {
//                       "name": "providerKey",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "providerName",
//                       "name": "providerName",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "providerKey",
//                       "name": "providerKey",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.PermissionManagement.GetPermissionListResultDto, Volo.Abp.PermissionManagement.Application.Contracts"
//                   }
//                 },
//                 "UpdateAsyncByProviderNameAndProviderKeyAndInput": {
//                   "uniqueName": "UpdateAsyncByProviderNameAndProviderKeyAndInput",
//                   "name": "UpdateAsync",
//                   "httpMethod": "PUT",
//                   "url": "api/abp/permissions",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "providerName",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     },
//                     {
//                       "name": "providerKey",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     },
//                     {
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.PermissionManagement.UpdatePermissionsDto, Volo.Abp.PermissionManagement.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "providerName",
//                       "name": "providerName",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "providerKey",
//                       "name": "providerKey",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.PermissionManagement.UpdatePermissionsDto, Volo.Abp.PermissionManagement.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "Body"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "System.Void, System.Private.CoreLib"
//                   }
//                 }
//               }
//             }
//           }
//         },
//         "multi-tenancy": {
//           "rootPath": "multi-tenancy",
//           "controllers": {
//             "Volo.Abp.TenantManagement.TenantController": {
//               "controllerName": "Tenant",
//               "typeAsString": "Volo.Abp.TenantManagement.TenantController, Volo.Abp.TenantManagement.HttpApi",
//               "interfaces": [
//                 {
//                   "typeAsString": "Volo.Abp.TenantManagement.ITenantAppService, Volo.Abp.TenantManagement.Application.Contracts"
//                 }
//               ],
//               "actions": {
//                 "GetAsyncById": {
//                   "uniqueName": "GetAsyncById",
//                   "name": "GetAsync",
//                   "httpMethod": "GET",
//                   "url": "api/multi-tenancy/tenants/{id}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.TenantManagement.TenantDto, Volo.Abp.TenantManagement.Application.Contracts"
//                   }
//                 },
//                 "GetListAsyncByInput": {
//                   "uniqueName": "GetListAsyncByInput",
//                   "name": "GetListAsync",
//                   "httpMethod": "GET",
//                   "url": "api/multi-tenancy/tenants",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.TenantManagement.GetTenantsInput, Volo.Abp.TenantManagement.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "input",
//                       "name": "Filter",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "Sorting",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "SkipCount",
//                       "typeAsString": "System.Int32, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "MaxResultCount",
//                       "typeAsString": "System.Int32, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Application.Dtos.PagedResultDto`1[[Volo.Abp.TenantManagement.TenantDto, Volo.Abp.TenantManagement.Application.Contracts, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null]], Volo.Abp.Ddd.Application.Contracts"
//                   }
//                 },
//                 "CreateAsyncByInput": {
//                   "uniqueName": "CreateAsyncByInput",
//                   "name": "CreateAsync",
//                   "httpMethod": "POST",
//                   "url": "api/multi-tenancy/tenants",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.TenantManagement.TenantCreateDto, Volo.Abp.TenantManagement.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "input",
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.TenantManagement.TenantCreateDto, Volo.Abp.TenantManagement.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "Body"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.TenantManagement.TenantDto, Volo.Abp.TenantManagement.Application.Contracts"
//                   }
//                 },
//                 "UpdateAsyncByIdAndInput": {
//                   "uniqueName": "UpdateAsyncByIdAndInput",
//                   "name": "UpdateAsync",
//                   "httpMethod": "PUT",
//                   "url": "api/multi-tenancy/tenants/{id}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     },
//                     {
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.TenantManagement.TenantUpdateDto, Volo.Abp.TenantManagement.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.TenantManagement.TenantUpdateDto, Volo.Abp.TenantManagement.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "Body"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.TenantManagement.TenantDto, Volo.Abp.TenantManagement.Application.Contracts"
//                   }
//                 },
//                 "DeleteAsyncById": {
//                   "uniqueName": "DeleteAsyncById",
//                   "name": "DeleteAsync",
//                   "httpMethod": "DELETE",
//                   "url": "api/multi-tenancy/tenants/{id}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "System.Void, System.Private.CoreLib"
//                   }
//                 },
//                 "GetDefaultConnectionStringAsyncById": {
//                   "uniqueName": "GetDefaultConnectionStringAsyncById",
//                   "name": "GetDefaultConnectionStringAsync",
//                   "httpMethod": "GET",
//                   "url": "api/multi-tenancy/tenants/{id}/default-connection-string",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "System.String, System.Private.CoreLib"
//                   }
//                 },
//                 "UpdateDefaultConnectionStringAsyncByIdAndDefaultConnectionString": {
//                   "uniqueName": "UpdateDefaultConnectionStringAsyncByIdAndDefaultConnectionString",
//                   "name": "UpdateDefaultConnectionStringAsync",
//                   "httpMethod": "PUT",
//                   "url": "api/multi-tenancy/tenants/{id}/default-connection-string",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     },
//                     {
//                       "name": "defaultConnectionString",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     },
//                     {
//                       "nameOnMethod": "defaultConnectionString",
//                       "name": "defaultConnectionString",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "System.Void, System.Private.CoreLib"
//                   }
//                 },
//                 "DeleteDefaultConnectionStringAsyncById": {
//                   "uniqueName": "DeleteDefaultConnectionStringAsyncById",
//                   "name": "DeleteDefaultConnectionStringAsync",
//                   "httpMethod": "DELETE",
//                   "url": "api/multi-tenancy/tenants/{id}/default-connection-string",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "System.Void, System.Private.CoreLib"
//                   }
//                 }
//               }
//             }
//           }
//         },
//         "identity": {
//           "rootPath": "identity",
//           "controllers": {
//             "Volo.Abp.Identity.IdentityRoleController": {
//               "controllerName": "IdentityRole",
//               "typeAsString": "Volo.Abp.Identity.IdentityRoleController, Volo.Abp.Identity.HttpApi",
//               "interfaces": [
//                 {
//                   "typeAsString": "Volo.Abp.Identity.IIdentityRoleAppService, Volo.Abp.Identity.Application.Contracts"
//                 }
//               ],
//               "actions": {
//                 "GetListAsync": {
//                   "uniqueName": "GetListAsync",
//                   "name": "GetListAsync",
//                   "httpMethod": "GET",
//                   "url": "api/identity/roles",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [],
//                   "parameters": [],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Application.Dtos.ListResultDto`1[[Volo.Abp.Identity.IdentityRoleDto, Volo.Abp.Identity.Application.Contracts, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null]], Volo.Abp.Ddd.Application.Contracts"
//                   }
//                 },
//                 "GetAsyncById": {
//                   "uniqueName": "GetAsyncById",
//                   "name": "GetAsync",
//                   "httpMethod": "GET",
//                   "url": "api/identity/roles/{id}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Identity.IdentityRoleDto, Volo.Abp.Identity.Application.Contracts"
//                   }
//                 },
//                 "CreateAsyncByInput": {
//                   "uniqueName": "CreateAsyncByInput",
//                   "name": "CreateAsync",
//                   "httpMethod": "POST",
//                   "url": "api/identity/roles",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.Identity.IdentityRoleCreateDto, Volo.Abp.Identity.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "input",
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.Identity.IdentityRoleCreateDto, Volo.Abp.Identity.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "Body"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Identity.IdentityRoleDto, Volo.Abp.Identity.Application.Contracts"
//                   }
//                 },
//                 "UpdateAsyncByIdAndInput": {
//                   "uniqueName": "UpdateAsyncByIdAndInput",
//                   "name": "UpdateAsync",
//                   "httpMethod": "PUT",
//                   "url": "api/identity/roles/{id}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     },
//                     {
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.Identity.IdentityRoleUpdateDto, Volo.Abp.Identity.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.Identity.IdentityRoleUpdateDto, Volo.Abp.Identity.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "Body"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Identity.IdentityRoleDto, Volo.Abp.Identity.Application.Contracts"
//                   }
//                 },
//                 "DeleteAsyncById": {
//                   "uniqueName": "DeleteAsyncById",
//                   "name": "DeleteAsync",
//                   "httpMethod": "DELETE",
//                   "url": "api/identity/roles/{id}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "System.Void, System.Private.CoreLib"
//                   }
//                 }
//               }
//             },
//             "Volo.Abp.Identity.IdentityUserController": {
//               "controllerName": "IdentityUser",
//               "typeAsString": "Volo.Abp.Identity.IdentityUserController, Volo.Abp.Identity.HttpApi",
//               "interfaces": [
//                 {
//                   "typeAsString": "Volo.Abp.Identity.IIdentityUserAppService, Volo.Abp.Identity.Application.Contracts"
//                 }
//               ],
//               "actions": {
//                 "GetAsyncById": {
//                   "uniqueName": "GetAsyncById",
//                   "name": "GetAsync",
//                   "httpMethod": "GET",
//                   "url": "api/identity/users/{id}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Identity.IdentityUserDto, Volo.Abp.Identity.Application.Contracts"
//                   }
//                 },
//                 "GetListAsyncByInput": {
//                   "uniqueName": "GetListAsyncByInput",
//                   "name": "GetListAsync",
//                   "httpMethod": "GET",
//                   "url": "api/identity/users",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.Identity.GetIdentityUsersInput, Volo.Abp.Identity.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "input",
//                       "name": "Filter",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "Sorting",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "SkipCount",
//                       "typeAsString": "System.Int32, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "MaxResultCount",
//                       "typeAsString": "System.Int32, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Application.Dtos.PagedResultDto`1[[Volo.Abp.Identity.IdentityUserDto, Volo.Abp.Identity.Application.Contracts, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null]], Volo.Abp.Ddd.Application.Contracts"
//                   }
//                 },
//                 "CreateAsyncByInput": {
//                   "uniqueName": "CreateAsyncByInput",
//                   "name": "CreateAsync",
//                   "httpMethod": "POST",
//                   "url": "api/identity/users",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.Identity.IdentityUserCreateDto, Volo.Abp.Identity.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "input",
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.Identity.IdentityUserCreateDto, Volo.Abp.Identity.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "Body"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Identity.IdentityUserDto, Volo.Abp.Identity.Application.Contracts"
//                   }
//                 },
//                 "UpdateAsyncByIdAndInput": {
//                   "uniqueName": "UpdateAsyncByIdAndInput",
//                   "name": "UpdateAsync",
//                   "httpMethod": "PUT",
//                   "url": "api/identity/users/{id}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     },
//                     {
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.Identity.IdentityUserUpdateDto, Volo.Abp.Identity.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.Identity.IdentityUserUpdateDto, Volo.Abp.Identity.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "Body"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Identity.IdentityUserDto, Volo.Abp.Identity.Application.Contracts"
//                   }
//                 },
//                 "DeleteAsyncById": {
//                   "uniqueName": "DeleteAsyncById",
//                   "name": "DeleteAsync",
//                   "httpMethod": "DELETE",
//                   "url": "api/identity/users/{id}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "System.Void, System.Private.CoreLib"
//                   }
//                 },
//                 "GetRolesAsyncById": {
//                   "uniqueName": "GetRolesAsyncById",
//                   "name": "GetRolesAsync",
//                   "httpMethod": "GET",
//                   "url": "api/identity/users/{id}/roles",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Application.Dtos.ListResultDto`1[[Volo.Abp.Identity.IdentityRoleDto, Volo.Abp.Identity.Application.Contracts, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null]], Volo.Abp.Ddd.Application.Contracts"
//                   }
//                 },
//                 "UpdateRolesAsyncByIdAndInput": {
//                   "uniqueName": "UpdateRolesAsyncByIdAndInput",
//                   "name": "UpdateRolesAsync",
//                   "httpMethod": "PUT",
//                   "url": "api/identity/users/{id}/roles",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     },
//                     {
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.Identity.IdentityUserUpdateRolesDto, Volo.Abp.Identity.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     },
//                     {
//                       "nameOnMethod": "input",
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.Identity.IdentityUserUpdateRolesDto, Volo.Abp.Identity.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "Body"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "System.Void, System.Private.CoreLib"
//                   }
//                 },
//                 "FindByUsernameAsyncByUsername": {
//                   "uniqueName": "FindByUsernameAsyncByUsername",
//                   "name": "FindByUsernameAsync",
//                   "httpMethod": "GET",
//                   "url": "api/identity/users/by-username/{userName}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "username",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "username",
//                       "name": "username",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Identity.IdentityUserDto, Volo.Abp.Identity.Application.Contracts"
//                   }
//                 },
//                 "FindByEmailAsyncByEmail": {
//                   "uniqueName": "FindByEmailAsyncByEmail",
//                   "name": "FindByEmailAsync",
//                   "httpMethod": "GET",
//                   "url": "api/identity/users/by-email/{email}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "email",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "email",
//                       "name": "email",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Identity.IdentityUserDto, Volo.Abp.Identity.Application.Contracts"
//                   }
//                 }
//               }
//             },
//             "Volo.Abp.Identity.IdentityUserLookupController": {
//               "controllerName": "IdentityUserLookup",
//               "typeAsString": "Volo.Abp.Identity.IdentityUserLookupController, Volo.Abp.Identity.HttpApi",
//               "interfaces": [
//                 {
//                   "typeAsString": "Volo.Abp.Identity.IIdentityUserLookupAppService, Volo.Abp.Identity.Application.Contracts"
//                 }
//               ],
//               "actions": {
//                 "FindByIdAsyncById": {
//                   "uniqueName": "FindByIdAsyncById",
//                   "name": "FindByIdAsync",
//                   "httpMethod": "GET",
//                   "url": "api/identity/users/lookup/{id}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "id",
//                       "name": "id",
//                       "typeAsString": "System.Guid, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Users.UserData, Volo.Abp.Users.Abstractions"
//                   }
//                 },
//                 "FindByUserNameAsyncByUserName": {
//                   "uniqueName": "FindByUserNameAsyncByUserName",
//                   "name": "FindByUserNameAsync",
//                   "httpMethod": "GET",
//                   "url": "api/identity/users/lookup/by-username/{userName}",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "userName",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "userName",
//                       "name": "userName",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": [],
//                       "bindingSourceId": "Path"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Users.UserData, Volo.Abp.Users.Abstractions"
//                   }
//                 }
//               }
//             },
//             "Volo.Abp.Identity.ProfileController": {
//               "controllerName": "Profile",
//               "typeAsString": "Volo.Abp.Identity.ProfileController, Volo.Abp.Identity.HttpApi",
//               "interfaces": [
//                 {
//                   "typeAsString": "Volo.Abp.Identity.IProfileAppService, Volo.Abp.Identity.Application.Contracts"
//                 }
//               ],
//               "actions": {
//                 "GetAsync": {
//                   "uniqueName": "GetAsync",
//                   "name": "GetAsync",
//                   "httpMethod": "GET",
//                   "url": "api/identity/my-profile",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [],
//                   "parameters": [],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Identity.ProfileDto, Volo.Abp.Identity.Application.Contracts"
//                   }
//                 },
//                 "UpdateAsyncByInput": {
//                   "uniqueName": "UpdateAsyncByInput",
//                   "name": "UpdateAsync",
//                   "httpMethod": "PUT",
//                   "url": "api/identity/my-profile",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.Identity.UpdateProfileDto, Volo.Abp.Identity.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "input",
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.Identity.UpdateProfileDto, Volo.Abp.Identity.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "Body"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Volo.Abp.Identity.ProfileDto, Volo.Abp.Identity.Application.Contracts"
//                   }
//                 },
//                 "ChangePasswordAsyncByInput": {
//                   "uniqueName": "ChangePasswordAsyncByInput",
//                   "name": "ChangePasswordAsync",
//                   "httpMethod": "POST",
//                   "url": "api/identity/my-profile/change-password",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.Identity.ChangePasswordInput, Volo.Abp.Identity.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "input",
//                       "name": "input",
//                       "typeAsString": "Volo.Abp.Identity.ChangePasswordInput, Volo.Abp.Identity.Application.Contracts",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "Body"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "System.Void, System.Private.CoreLib"
//                   }
//                 }
//               }
//             }
//           }
//         },
//         "Abp": {
//           "rootPath": "Abp",
//           "controllers": {
//             "Volo.Abp.AspNetCore.Mvc.ProxyScripting.AbpServiceProxyScriptController": {
//               "controllerName": "AbpServiceProxyScript",
//               "typeAsString": "Volo.Abp.AspNetCore.Mvc.ProxyScripting.AbpServiceProxyScriptController, Volo.Abp.AspNetCore.Mvc",
//               "interfaces": [],
//               "actions": {
//                 "GetAllByModel": {
//                   "uniqueName": "GetAllByModel",
//                   "name": "GetAll",
//                   "httpMethod": "GET",
//                   "url": "Abp/ServiceProxyScript",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "model",
//                       "typeAsString": "Volo.Abp.AspNetCore.Mvc.ProxyScripting.ServiceProxyGenerationModel, Volo.Abp.AspNetCore.Mvc",
//                       "isOptional": false,
//                       "defaultValue": null
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "model",
//                       "name": "Type",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "model",
//                       "name": "UseCache",
//                       "typeAsString": "System.Boolean, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "model",
//                       "name": "Modules",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "model",
//                       "name": "Controllers",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "model",
//                       "name": "Actions",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "System.String, System.Private.CoreLib"
//                   }
//                 }
//               }
//             },
//             "Volo.Abp.AspNetCore.Mvc.Localization.AbpLanguagesController": {
//               "controllerName": "AbpLanguages",
//               "typeAsString": "Volo.Abp.AspNetCore.Mvc.Localization.AbpLanguagesController, Volo.Abp.AspNetCore.Mvc",
//               "interfaces": [],
//               "actions": {
//                 "SwitchByCultureAndUiCultureAndReturnUrl": {
//                   "uniqueName": "SwitchByCultureAndUiCultureAndReturnUrl",
//                   "name": "Switch",
//                   "httpMethod": "GET",
//                   "url": "Abp/Languages/Switch",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [
//                     {
//                       "name": "culture",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null
//                     },
//                     {
//                       "name": "uiCulture",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": true,
//                       "defaultValue": ""
//                     },
//                     {
//                       "name": "returnUrl",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": true,
//                       "defaultValue": ""
//                     }
//                   ],
//                   "parameters": [
//                     {
//                       "nameOnMethod": "culture",
//                       "name": "culture",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "uiCulture",
//                       "name": "uiCulture",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     },
//                     {
//                       "nameOnMethod": "returnUrl",
//                       "name": "returnUrl",
//                       "typeAsString": "System.String, System.Private.CoreLib",
//                       "isOptional": false,
//                       "defaultValue": null,
//                       "constraintTypes": null,
//                       "bindingSourceId": "ModelBinding"
//                     }
//                   ],
//                   "returnValue": {
//                     "typeAsString": "Microsoft.AspNetCore.Mvc.IActionResult, Microsoft.AspNetCore.Mvc.Abstractions"
//                   }
//                 }
//               }
//             },
//             "Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.AbpApplicationConfigurationScriptController": {
//               "controllerName": "AbpApplicationConfigurationScript",
//               "typeAsString": "Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.AbpApplicationConfigurationScriptController, Volo.Abp.AspNetCore.Mvc",
//               "interfaces": [],
//               "actions": {
//                 "Get": {
//                   "uniqueName": "Get",
//                   "name": "Get",
//                   "httpMethod": "GET",
//                   "url": "Abp/ApplicationConfigurationScript",
//                   "supportedVersions": [],
//                   "parametersOnMethod": [],
//                   "parameters": [],
//                   "returnValue": {
//                     "typeAsString": "System.String, System.Private.CoreLib"
//                   }
//                 }
//               }
//             }
//           }
//         }
//       }
//     } as any)
//   }, 0);
// })
