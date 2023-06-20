import { Injectable, inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';

@Injectable({ providedIn: 'root' })
export class AbpWindowService {
  protected readonly window = inject(DOCUMENT).defaultView;
  protected readonly navigator = this.window.navigator;

  copyToClipboard(text: string): Promise<void> {
    return this.navigator.clipboard.writeText(text);
  }
}
