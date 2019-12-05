import { APIDefination } from './types/api-defination';
import { ServiceTemplates } from './templates/angular/service-templates';
import changeCase from 'change-case';
import fse from 'fs-extra';

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

        switch (element.httpMethod) {
          case 'GET':
            contents.push(ServiceTemplates.getMethodTemplate(element.name, element.url));
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
