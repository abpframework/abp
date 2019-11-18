"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
var axios_1 = __importDefault(require("axios"));
exports.axiosInstance = axios_1.default.create();
exports.axiosInstance.get = function () {
    var ar = [];
    for (var _i = 0; _i < arguments.length; _i++) {
        ar[_i] = arguments[_i];
    }
    return ({
        "modules": {
            "additionalProp1": {
                "rootPath": "string",
                "controllers": {
                    "additionalProp1": {
                        "controllerName": "string",
                        "typeAsString": "string",
                        "interfaces": [
                            {
                                "typeAsString": "string"
                            }
                        ],
                        "actions": {
                            "additionalProp1": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            },
                            "additionalProp2": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            },
                            "additionalProp3": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            }
                        }
                    },
                    "additionalProp2": {
                        "controllerName": "string",
                        "typeAsString": "string",
                        "interfaces": [
                            {
                                "typeAsString": "string"
                            }
                        ],
                        "actions": {
                            "additionalProp1": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            },
                            "additionalProp2": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            },
                            "additionalProp3": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            }
                        }
                    },
                    "additionalProp3": {
                        "controllerName": "string",
                        "typeAsString": "string",
                        "interfaces": [
                            {
                                "typeAsString": "string"
                            }
                        ],
                        "actions": {
                            "additionalProp1": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            },
                            "additionalProp2": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            },
                            "additionalProp3": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            }
                        }
                    }
                }
            },
            "additionalProp2": {
                "rootPath": "string",
                "controllers": {
                    "additionalProp1": {
                        "controllerName": "string",
                        "typeAsString": "string",
                        "interfaces": [
                            {
                                "typeAsString": "string"
                            }
                        ],
                        "actions": {
                            "additionalProp1": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            },
                            "additionalProp2": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            },
                            "additionalProp3": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            }
                        }
                    },
                    "additionalProp2": {
                        "controllerName": "string",
                        "typeAsString": "string",
                        "interfaces": [
                            {
                                "typeAsString": "string"
                            }
                        ],
                        "actions": {
                            "additionalProp1": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            },
                            "additionalProp2": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            },
                            "additionalProp3": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            }
                        }
                    },
                    "additionalProp3": {
                        "controllerName": "string",
                        "typeAsString": "string",
                        "interfaces": [
                            {
                                "typeAsString": "string"
                            }
                        ],
                        "actions": {
                            "additionalProp1": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            },
                            "additionalProp2": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            },
                            "additionalProp3": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            }
                        }
                    }
                }
            },
            "additionalProp3": {
                "rootPath": "string",
                "controllers": {
                    "additionalProp1": {
                        "controllerName": "string",
                        "typeAsString": "string",
                        "interfaces": [
                            {
                                "typeAsString": "string"
                            }
                        ],
                        "actions": {
                            "additionalProp1": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            },
                            "additionalProp2": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            },
                            "additionalProp3": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            }
                        }
                    },
                    "additionalProp2": {
                        "controllerName": "string",
                        "typeAsString": "string",
                        "interfaces": [
                            {
                                "typeAsString": "string"
                            }
                        ],
                        "actions": {
                            "additionalProp1": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            },
                            "additionalProp2": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            },
                            "additionalProp3": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            }
                        }
                    },
                    "additionalProp3": {
                        "controllerName": "string",
                        "typeAsString": "string",
                        "interfaces": [
                            {
                                "typeAsString": "string"
                            }
                        ],
                        "actions": {
                            "additionalProp1": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            },
                            "additionalProp2": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            },
                            "additionalProp3": {
                                "uniqueName": "string",
                                "name": "string",
                                "httpMethod": "string",
                                "url": "string",
                                "supportedVersions": [
                                    "string"
                                ],
                                "parametersOnMethod": [
                                    {
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {}
                                    }
                                ],
                                "parameters": [
                                    {
                                        "nameOnMethod": "string",
                                        "name": "string",
                                        "typeAsString": "string",
                                        "isOptional": true,
                                        "defaultValue": {},
                                        "constraintTypes": [
                                            "string"
                                        ],
                                        "bindingSourceId": "string"
                                    }
                                ],
                                "returnValue": {
                                    "typeAsString": "string"
                                }
                            }
                        }
                    }
                }
            }
        }
    });
};
