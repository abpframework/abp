#!/usr/bin/env node
import chalk from 'chalk';
import commander from 'commander';
import ora from 'ora';
import { axiosInstance } from './utils/axios';
const clear = require('clear');
const figlet = require('figlet');

clear();

console.log(chalk.red(figlet.textSync('ABP', { horizontalLayout: 'full' })));

commander
  .version('0.0.1')
  .description('ABP Client Generator')
  .option('-u, --ui', 'UI option (Angular)')
  .parse(process.argv);

if (!process.argv.slice(2).length) {
  commander.outputHelp();
  process.exit(1);
}

const loading = ora('Waiting for API response... \n');
loading.start();
(async function() {
  const data = await axiosInstance.get('a');
  loading.stop();
})();
