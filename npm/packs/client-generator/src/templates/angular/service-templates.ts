import changeCase from 'change-case';

export namespace ServiceTemplates {
  export function classTemplate(name: string, content: string[]) {
    `import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ${changeCase.pascalCase(name)}Service {

  constructor(private restService: RestService) { }

  ${content.forEach(element => {
    element + '\n\n';
  })}
}`;
  }
}
