import { uiSelection, moduleSelection } from './utils/prompt';
import { axiosInstance } from './utils/axios';
import ora = require('ora');
import { angular } from './angular';
import chalk from 'chalk';
import { APIDefination } from './types/api-defination';

export async function cli(program: any) {
  if (program.ui !== 'angular') {
    program.ui = ((await uiSelection(['Angular'])) as string).toLowerCase();
  }
  const loading = ora('Waiting for the API response... \n');
  loading.start();
  let data = {} as APIDefination.Response;
  const apiUrl = 'https://localhost:44305/api/abp/api-definition';
  try {
    data = (await axiosInstance.get(apiUrl)).data;
  } catch (error) {
    console.log(chalk.red('An error occurred when fetching the ' + apiUrl));
    process.exit(1);
  }
  console.log(data);
  loading.stop();

  const selection = async (modules: string[]): Promise<string[]> => {
    const selectedModules = (await moduleSelection(modules)) as string[];

    if (!selectedModules.length) {
      console.log(chalk.red('Please select module(s)'));
      return await selection(modules);
    }

    return selectedModules;
  };

  // const modules = await selection(Object.keys(data.modules));
  const modules = ['saas'];

  switch (program.ui) {
    case 'angular':
      await angular(data, modules);
      break;

    default:
      process.exit(1);
  }
}
