import changeCase from 'change-case';
import { replacer } from '../../utils/replacer';

export namespace ServiceTemplates {
  export function classTemplate(name: string, content: string) {
    return `import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class ${changeCase.pascalCase(name)}Service {
  constructor(private restService: RestService) {}
  ${content}
}`;
  }

  export function getMethodTemplate(name: string, url: string, params: string[] = [], queryParams?: object) {
    return `
  ${changeCase.camelCase(replacer(name))}(${params.join(', ')}): Observable<any> {
    return this.restService.request<void, any>({
      method: 'GET',
      url: '/${url}${params.length ? '/' + params.join('/') : ''}',
    });
  }`;
  }
}
