import { uiSelection, moduleSelection } from './utils/prompt';
import { axiosInstance } from './utils/axios';
import ora = require('ora');
import { angular } from './angular';
import chalk from 'chalk';

export async function cli(program: any) {
  if (program.ui !== 'angular') {
    program.ui = ((await uiSelection(['Angular'])) as string).toLowerCase();
  }

  const loading = ora('Waiting for the API response... \n');
  loading.start();
  const data = (await axiosInstance.get('a')) as any;
  loading.stop();

  const selection = async (modules: string[]): Promise<string[]> => {
    const selectedModules = (await moduleSelection(modules)) as string[];

    if (!selectedModules.length) {
      console.log(chalk.red('Please select module(s)'));
      return await selection(modules);
    }

    return selectedModules;
  };

  const modules = await selection(Object.keys(data.modules));

  switch (program.ui) {
    case 'angular':
      await angular(data, modules);
      break;

    default:
      process.exit(1);
  }
}
