import { bold } from 'chalk';

export const log = {
  info: (message: string) => console.log(bold.blue(`\n${message}\n`)),
  error: (message: string) => console.log(bold.underline.red(`\n${message}\n`)),
};
