#!/usr/bin/env node
import chalk from 'chalk';
import program from 'commander';
import { cli } from './cli';
const clear = require('clear');
const figlet = require('figlet');

clear();

console.log(chalk.red(figlet.textSync('ABP', { horizontalLayout: 'full' })));

program
  .version('0.0.1')
  .description('ABP Client Generator')
  .option('-u, --ui <type>', 'UI option (Angular)')
  .parse(process.argv);

if (!process.argv.slice(2).length || !program.ui || typeof program.ui !== 'string') {
  program.outputHelp();
  process.exit(1);
}

program.ui = program.ui.toLowerCase();

cli(program);
