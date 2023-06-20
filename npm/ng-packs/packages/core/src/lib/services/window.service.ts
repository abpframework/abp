import { Injectable, inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';

@Injectable({ providedIn: 'root' })
export class AbpWindowService {
  protected readonly window = inject(DOCUMENT).defaultView;
  protected readonly navigator = this.window.navigator;

  copyToClipboard(text: string): Promise<void> {
    return this.navigator.clipboard.writeText(text);
  }

  open(url?: string | URL, target?: string, features?: string): Window {
    return this.window.open(url, target, features);
  }

  reloadPage(): void {
    this.window.location.reload();
  }
}
