#!/usr/bin/env node
"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
var chalk_1 = __importDefault(require("chalk"));
var commander_1 = __importDefault(require("commander"));
var clear = require('clear');
var figlet = require('figlet');
clear();
console.log(chalk_1.default.red(figlet.textSync('ABP', { horizontalLayout: 'full' })));
commander_1.default
    .version('0.0.1')
    .description("ABP Client Generator")
    .option('-u, --ui', 'UI option (Angular)')
    .parse(process.argv);
if (!process.argv.slice(2).length) {
    commander_1.default.outputHelp();
}
