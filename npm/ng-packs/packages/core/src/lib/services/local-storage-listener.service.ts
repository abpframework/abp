import { DOCUMENT } from '@angular/common';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LocalStorageListenerService {
  protected readonly window = inject(DOCUMENT).defaultView;

  constructor() {
    this.window.addEventListener('storage', event => {
      if (event.key === 'access_token' && event.newValue === null) {
        this.window.location.reload();
      }
    });
  }
}
