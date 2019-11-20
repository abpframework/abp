import changeCase from 'change-case';
import { replacer } from '../../utils/replacer';
import { Argument } from '../../utils/generators';
import snq from 'snq';

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

  export function getMethodTemplate(name: string, url: string, args: string = '', queryParams?: boolean) {
    return `
  ${changeCase.camelCase(replacer(name))}(${args}${queryParams ? ', queryParams: any' : ''}): Observable<any> {
    ${requestTemplate('GET', url, false, !!queryParams)}
  }`;
  }

  export function requestTemplate(method: string, url: string, body?: boolean, queryParams?: boolean) {
    const reg = /(?<=\{)(.*)(?=\})/g;

    (url.match(reg) || []).forEach(matched => {
      const index = url.indexOf(`{${matched}}`);
      url = url.slice(0, index) + '$' + url.slice(index);
    });

    return `return this.restService.request<void, any>({
      method: '${method}',
      url: \`/${url}\`,${queryParams ? 'params: queryParams,' : ''}${body ? 'body,' : ''}
    });`;
  }
}
