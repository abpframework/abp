import { DOCUMENT } from '@angular/common';
import { Injectable, inject } from '@angular/core';
@Injectable({
  providedIn: 'root',
})
export class LocalStorageListenerService {
  protected readonly window = inject(DOCUMENT).defaultView;

  constructor() {
    this.window.addEventListener('storage', event => {
      if (event.key === 'access_token') {
        const tokenRemoved = event.newValue === null;
        const tokenAdded = event.oldValue === null && event.newValue !== null;

        if (tokenRemoved || tokenAdded) {
          this.window.location.assign('/');
        }
      }
    });
  }
}
