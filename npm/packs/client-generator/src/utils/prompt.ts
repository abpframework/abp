import inquirer from 'inquirer';

export const uiSelection = async (uiArray: string[]) => {
  return inquirer
    .prompt({ type: 'list', name: 'Please select an UI', choices: uiArray })
    .then(res => res['Please select an UI']);
};

export const moduleSelection = async (modules: string[]) => {
  return inquirer
    .prompt({ type: 'checkbox', name: 'Please select module(s)', choices: modules })
    .then(res => res['Please select module(s)']);
};
