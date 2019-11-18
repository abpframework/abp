import changeCase from 'change-case';

export namespace ServiceTemplates {
  export function classTemplate(name: string, contents: string[]) {
    return `import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ${changeCase.pascalCase(name)}Service {

  constructor(private restService: RestService) { }

  ${contents[0]}
}`;
  }
}
