"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
function generateArgs(args) {
    return args.reduce(function (acc, val) {
        var arg = "" + val.key + (val.isOptional ? '?' : '') + ": " + val.type;
        if (acc)
            return acc + ", " + arg;
        else
            return arg;
    }, '');
}
exports.generateArgs = generateArgs;
function parseParameters(parameters) {
    return parameters
        .filter(function (param) { return param.bindingSourceId === 'Path'; })
        .map(function (param) {
        return { key: param.name, type: findType(param.typeAsString), isOptional: param.isOptional };
    });
}
exports.parseParameters = parseParameters;
function findType(typeAsString) {
    if (typeAsString.indexOf('Guid') > -1)
        return 'string';
    return 'any';
}
exports.findType = findType;
