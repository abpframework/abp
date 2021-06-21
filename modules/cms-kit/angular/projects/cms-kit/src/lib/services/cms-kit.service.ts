import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';

@Injectable({
  providedIn: 'root',
})
export class CmsKitService {
  apiName = 'CmsKit';

  constructor(private restService: RestService) {}

  sample() {
    return this.restService.request<void, any>(
      { method: 'GET', url: '/api/CmsKit/sample' },
      { apiName: this.apiName }
    );
  }
}
