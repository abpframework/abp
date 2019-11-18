import { uiSelection, moduleSelection } from './utils/prompt';
import { axiosInstance } from './utils/axios';
import ora = require('ora');

export const cli = async (program: any) => {
  if (program.ui !== 'angular') {
    program.ui = ((await uiSelection(['Angular'])) as string).toLowerCase();
  }

  const loading = ora('Waiting for the API response... \n');
  loading.start();
  const data = (await axiosInstance.get('a')) as any;
  loading.stop();
  const modules = await moduleSelection(Object.keys(data.modules));
  console.log(modules);
};
