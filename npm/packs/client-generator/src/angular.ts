import { APIDefination } from './types/api-defination';
import { ServiceTemplates } from './templates/angular/service-templates';
import changeCase from 'change-case';
import fse from 'fs-extra';

export async function angular(data: APIDefination.Response, selectedModules: string[]) {
  selectedModules.forEach(async module => {
    const element = data.modules[module];

    let contents = [] as string[];
    (Object.keys(element.controllers) || []).forEach(key => {
      console.warn(element.controllers[key].controllerName);
      contents.push(' ');
    });

    const service = ServiceTemplates.classTemplate(element.rootPath, contents);
    await fse.writeFile(`${changeCase.kebabCase(element.rootPath + '-service')}.ts`, service);
    console.log(service);
  });
}
