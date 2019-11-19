import { APIDefination } from './types/api-defination';
import { ServiceTemplates } from './templates/angular/service-templates';
import changeCase from 'change-case';
import fse from 'fs-extra';

export async function angular(data: APIDefination.Response, selectedModules: string[]) {
  selectedModules.forEach(async module => {
    const element = data.modules[module];

    let contents = [] as string[];
    const actions = (Object.keys(element.controllers) || []).map(key => element.controllers[key].actions);

    actions.forEach(action => {
      const actionKeys = Object.keys(action);
      actionKeys.forEach(key => {
        const element = action[key];
        console.log(element);

        switch (element.httpMethod) {
          case 'GET':
            contents.push(ServiceTemplates.getMethodTemplate(element.name, element.url));
            break;

          default:
            break;
        }
      });
    });

    const service = ServiceTemplates.classTemplate(element.rootPath, contents.join('\n'));
    await fse.writeFile(`dist/${changeCase.kebabCase(element.rootPath)}.service.ts`, service);
  });
}
