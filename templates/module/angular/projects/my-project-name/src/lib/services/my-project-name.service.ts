import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';

@Injectable({
  providedIn: 'root',
})
export class MyProjectNameService {
  apiName = 'MyProjectName';

  constructor(private restService: RestService) {}

  sample() {
    return this.restService.request<void, any>(
      { method: 'GET', url: '/api/MyProjectName/sample' },
      { apiName: this.apiName }
    );
  }
}
