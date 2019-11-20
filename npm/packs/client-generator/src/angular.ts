import { APIDefination } from './types/api-defination';
import { ServiceTemplates } from './templates/angular/service-templates';
import changeCase, { param } from 'change-case';
import fse from 'fs-extra';
import { generateArgs, parseParameters } from './utils/generators';

export async function angular(data: APIDefination.Response, selectedModules: string[]) {
  selectedModules.forEach(async module => {
    const element = data.modules[module];

    (Object.keys(element.controllers) || []).forEach(key => {
      const controller = element.controllers[key];

      const actions = element.controllers[key].actions;

      const actionKeys = Object.keys(actions);
      let contents = [] as string[];
      actionKeys.forEach(key => {
        const element = actions[key];
        console.log(element);

        const parameters = parseParameters(element.parameters);
        switch (element.httpMethod) {
          case 'GET':
            contents.push(ServiceTemplates.getMethodTemplate(element.name, element.url, generateArgs(parameters)));
            break;

          default:
            break;
        }
      });
      const service = ServiceTemplates.classTemplate(controller.controllerName, contents.join('\n'));
      fse.writeFileSync(`dist/${changeCase.kebabCase(controller.controllerName)}.service.ts`, service);
    });
  });
}
